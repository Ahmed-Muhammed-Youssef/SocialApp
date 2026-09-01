# 07 - Unit Test Coverage Report

**Measured:** 2026-09-01 · **Branch:** `develop` @ `e26c4bb` · **Scope:** the four unit test projects under `test/unit` (integration tests excluded — they need Docker and measure different things)

Raw tool output is archived at [`docs/testing/coverage-baseline-2026-09-01.txt`](./testing/coverage-baseline-2026-09-01.txt). Reproduce with:

```bash
for p in Domain Application Infrastructure Shared; do
  dotnet test "test/unit/$p.Test" --collect:"XPlat Code Coverage" --results-directory "cov/$p"
done
reportgenerator "-reports:cov/**/coverage.cobertura.xml" -targetdir:cov/report -reporttypes:TextSummary
```

---

## 1. Headline numbers

All 128 unit tests pass (Domain 76, Application 30, Infrastructure 14, Shared 8).

| Metric | All code | Hand-written code only |
|---|---|---|
| **Line coverage** | **4.9%** (516 / 10,383) | **28.2%** (516 / 1,827) |
| **Branch coverage** | 28.8% (108 / 374) | 28.8% (108 / 374) |
| **Method coverage** | 33.3% (170 / 510) | 37.7% (170 / 450) |

The 4.9% figure is not the number to manage against. EF Core migrations and `ModelSnapshot` are ~8,500 generated lines that should never be unit tested; excluding them (plus EF entity `Configurations` and `DependencyInjection` wiring) gives the honest **28.2%**.

Per assembly, excluding generated code:

| Assembly | Line coverage | Read |
|---|---|---|
| `Domain` | **62.6%** | Healthiest layer. Aggregates are well covered. |
| `Application` | **37.6%** | 12 of 31 handlers tested; the other 19 sit at 0%. |
| `Shared` | **18.9%** | `Result<T>`, the Specification engine and `RepositoryBase` — used by every feature — barely touched. |
| `Infrastructure` | **11.3%**† | Only `TokenProvider`, `CurrentUserService`, `OnlineUsersStore` and `JwtAuthOptions` are covered (those four at 100%). |

† The other three figures are quoted directly from [`coverage-baseline-2026-09-01.txt`](./testing/coverage-baseline-2026-09-01.txt), which is an *unfiltered* run and reports `Infrastructure 0.8%` — dominated by migrations. `11.3%` is the migration/config-filtered figure, taken from [`coverage-after-session-2026-09-01.txt`](./testing/coverage-after-session-2026-09-01.txt); it is valid as a baseline because `Infrastructure` gained no tests in that session, so the filtered number is identical before and after.

**There is no unit test project for the `API` assembly at all.** Controllers, SignalR hubs, MVC filters and all seven FluentValidation validators are absent from the coverage report entirely — they are reachable only through the integration suite.

---

## 2. The finding that matters: a live bug in untested code

> **Fixed 2026-09-01** as Step 1 of [08 - Test Coverage Plan](./08-test-coverage-plan.md#4-session-results). Left the original analysis below as-written since it's what drove the fix and the prioritisation in the plan.

`AssignRoleToUserHandler` **had 0% coverage and was broken**.

`src/API/Application/Features/UserRoles/AssignRoleToUser/AssignRoleToUserHandler.cs:21`

```csharp
IdentityRole? role = await roleManager.FindByIdAsync(user.IdentityId);   // ← wrong argument
```

It looks the role up by the **user's** identity id instead of `command.RoleId`. The command carries the value (`AssignRoleToUserCommand(int UserId, string RoleId)`) and the controller populates it correctly from the route (`POST api/users/{userId}/roles/{roleId}`) — the handler simply never reads it. `RoleManager.FindByIdAsync` searches `AspNetRoles` by primary key, so passing an `AspNetUsers` key returns `null` for every real request.

**Effect: assigning a role to a user always fails with `404 "Role not found"`.** Admin role management — one of the features listed in the README — does not work in production.

The sibling handler five files away gets it right, which is what makes this a coverage problem rather than a design problem:

`src/API/Application/Features/UserRoles/RemoveRoleFromUser/RemoveRoleFromUserHandler.cs:21`

```csharp
var role = await roleManager.FindByIdAsync(command.RoleId);              // ← correct
```

Both handlers are untested. One is wrong. A single "assigns the requested role" test would have caught it, and the existing `CreateRoleHandlerTests` already demonstrates the exact `Substitute.For<RoleManager<IdentityRole>>(...)` pattern needed to write it.

This bug also fails in the quietest possible way: a plausible `404` that an operator will read as "I typed the role id wrong." Nothing in logs or telemetry distinguishes it from legitimate bad input.

---

## 3. Where the gaps are

### 3.1 Untested Application handlers (19 of 31 at 0%)

Grouped by what breaks when they break:

**Authorization and identity** — failures here are silent and consequential:

| Handler | Why it matters |
|---|---|
| `AssignRoleToUserHandler` | **Broken today** (§2). Privilege assignment. |
| `RemoveRoleFromUserHandler` | Privilege revocation. If it silently no-ops, access is never actually revoked. |
| `RefreshTokenHandler` | Token rotation and replay invalidation. Carries an open `@TODO: Add Session-wide revocation on breach`. |
| `GoogleSignInHandler` | Provisions accounts from an external credential. |
| `DeleteFriendRequestHandler` | Enforces `fr.RequesterId != senderId` — the only thing stopping a user deleting someone else's request. |
| `SetProfilePictureHandler` | Ownership enforced entirely by `SetProfilePictureIfOwnedAsync` returning 0 rows. |
| `DeleteUserPictureHandler` | Ownership check plus an irreversible Cloudinary delete. |
| `GetUserPictureByIdHandler` | Scopes picture reads to the calling user. |

**Reads and secondary writes** — failures here are visible fast, so they carry less risk:
`GetChatsHandler`, `ConnectToChatHandler`, `DisconnectFromChatHandler`, `GetFriendRequestsHandler`, `GetUserPostsHandler`, `GetPicturesHandler`, `CreateUserPictureHandler`, `GetRolesHandler`, `DeleteRoleHandler`, `GetUserRolesHandler`, `GetCitiesHandler`.

### 3.2 `Shared` — highest leverage per test

Small, dependency-free, and on the path of literally every request:

| Type | Coverage | Why it matters |
|---|---|---|
| `Results.Result<T>` | 57.6% | Every handler's return contract; controllers branch on `Status` to pick the HTTP code. A wrong status mapping mis-reports every endpoint that uses it. |
| `Extensions.ClaimsPrincipalExtensions` | **0%** | `GetPublicId()` is the **only** identity resolution path for `ChatHub`, `OnlineUsersHub` and the `LogUserActivity` filter. Untested, and it duplicates logic that `Infrastructure.Auth.CurrentUserService` implements separately (that copy is at 100%) — two implementations of one rule, one of them unverified. |
| `Specification.SpecificationEvaluator` + `Specification<T>` | **0%** | Translates specifications into EF queries. A silent bug leaks or hides rows. |
| `RepositoryBase.RepositoryBase<T>` | **0%** | Base class behind every repository. |
| `Extensions.DateTimeExtensions` | 36% | Has tests, but `ToRelativeTimeString`'s five branches are unexercised. |
| `Pagination.PaginationParams` | 66.6% | Page-size clamping — the guard against unbounded queries. |

### 3.3 Domain

Strongest layer at 62.6%, and the important aggregates are genuinely covered (`FriendRequest` and `Friend` 100%, `DirectChat` 95.4%, `Message` 90.9%, `RefreshToken` 93.7%, `ApplicationUser` 80.6%).

Remaining holes are narrow: `Post` 51.5%, and `UserPicture`, `MediaAggregate.Picture`, the `UserByIdFilter` / `UserByIdentityFilter` / `UserWithPicturesSpecification` filter specs all at 0%.

### 3.4 Correctly out of unit-test scope

Listing these so future coverage numbers aren't chased for their own sake. These need the Testcontainers integration suite, not mocks:

`ApplicationDatabaseContext`, all `Data/Configurations/*`, all `Migrations/*`, every repository, `UnitOfWork`, `DatabaseInitializer`, `CloudinaryPictureService`, `GoogleCredentialValidator`, `UserProvisioningService` (real transactions), and `CachedUserRepository`.

`RefreshTokenHandler` belongs here too: it opens a transaction and calls `ExecuteUpdateAsync` directly on the `DbSet`, neither of which can be meaningfully faked with NSubstitute. Its *risk* is Tier A, but its *test* has to be an integration test.

`CachedUserRepository` deserves a design note rather than a unit test: it caches `GetByIdAsync` / `GetDtoByIdAsync` for 1 minute and never invalidates on write, so `UpdateUserHandler` and `SetProfilePictureHandler` can leave stale reads for up to a minute. That may be intentional, but it is currently unstated and unverified.

---

## 4. Housekeeping observed while measuring

- **Duplicate integration test files.** `test/integration/API.Test/Features/AuthTests.cs` (75 lines, namespace `API.Test.Features`) coexists with `Features/Auth/AuthTests.cs` (269 lines, namespace `API.Test.Features.Auth`); same for `UsersTests.cs`. They differ, so this is leftover from a reorganisation rather than a copy. Both are compiled into the project (distinct namespaces, so there is no clash), but neither actually runs today — that project fails to build on the `NU1903` advisory described in [08 §4](./08-test-coverage-plan.md#4-session-results).
- **Untracked `tests/` directory** at repo root, distinct from the real `test/`, not referenced by `SocialApp.slnx`. Build leftover; safe to delete.
- **Test tree mirrors the old source layout.** Tests sit at `Application.Test/Features/Posts/Create/` while the source is at `Application/Features/Users/Posts/CreatePost/`. Harmless, but it makes "does this handler have a test?" harder to answer than it should be.

---

## 5. Conclusion

Coverage on hand-written code is **28.2%**, and the distribution is inverted relative to risk: the well-tested layer (`Domain`, 62.6%) is the one whose invariants are enforced by construction anyway, while the layers that decide *who is allowed to do what* — Application authorization handlers, `Shared` auth extensions, and the entire `API` layer — are the least covered.

§2 is the evidence, not the hypothesis: the single most privileged untested handler in the codebase is broken right now, and has been shipping in that state.

The prioritised remediation plan is [08 - Test Coverage Plan](./08-test-coverage-plan.md).
