# 08 - Test Coverage Plan

Companion to [07 - Unit Test Coverage Report](./07-test-coverage-report.md). Baseline: **28.2%** line coverage on hand-written code, 128 passing unit tests.

> **Status: Steps 1–7 completed 2026-09-01.** See [§4 Session results](#4-session-results) for what actually happened, including one deviation from the plan (Step 1's "already in role" sub-case needed a different mock than planned) and the real before/after coverage numbers.

---

## 1. How targets were prioritised

Coverage percentage is a bad objective — 8,500 lines of EF migrations could be "covered" to no benefit. Each candidate was ranked by:

**Blast radius** — what a failure costs. Wrong authorization decision (privileges granted or not revoked, one user acting on another's data) ranks far above a broken read.

**Silence** — whether a failure announces itself. A broken newsfeed is visible in the UI within seconds of deploy; a role assignment that returns a plausible `404`, or a missing ownership check, surfaces only when someone is affected or exploits it. Untested + silent is the dangerous combination, and it is exactly where §2 of the report found a live bug.

**Cost to test** — a handler depending only on `IUnitOfWork` + `ICurrentUserService` is ~5 minutes using the existing `TestHelpers`. A handler calling `ExecuteUpdateAsync` inside a transaction cannot be unit tested at all and must not be forced into one.

This ordering deliberately skips things that would raise the percentage faster (DTO records, EF configurations) in favour of things that would have caught a real defect.

---

## 2. Today's session — 30 minutes

Ordered so the highest-value work lands first; if the session is cut short, everything completed still stands alone. Every item follows the conventions already in `test/unit/Application.Test`: constructor-injected NSubstitute fakes, `TestHelpers.CreateMockUnitOfWork()` / `CreateMockCurrentUserService()`, Arrange/Act/Assert comments, `Handle_Condition_Outcome` naming.

### Step 1 — Prove and fix the role-assignment bug · 6 min

**Why first:** it is the only item in this plan that fixes a broken production feature rather than guarding a working one.

1. Add `test/unit/Application.Test/Features/UserRoles/AssignRoleToUser/AssignRoleToUserHandlerTests.cs`, mocking `RoleManager<IdentityRole>` and `UserManager<IdentityUser>` with the pattern from `Features/Roles/Create/CreateRoleHandlerTests.cs:16-19`:
   - `Handle_UserNotFound_ReturnsNotFound`
   - `Handle_IdentityUserNotFound_ReturnsNotFound`
   - `Handle_RoleNotFound_ReturnsNotFound`
   - `Handle_ValidRequest_AssignsRequestedRoleAndReturnsSuccess` — stub `FindByIdAsync(command.RoleId)`, assert `AddToRoleAsync(identityUser, role.Name)` received. **Expected to fail against current code.**
   - `Handle_AddToRoleFails_ReturnsErrorWithIdentityErrors`
2. Confirm it fails for the right reason (`404 Role not found`, not a mocking error).
3. Fix `AssignRoleToUserHandler.cs:21`: `roleManager.FindByIdAsync(user.IdentityId)` → `roleManager.FindByIdAsync(command.RoleId)`.
4. Re-run; suite green.

### Step 2 — `RemoveRoleFromUserHandler` · 4 min

Mirror of Step 1 (this handler is already correct, so these tests lock in the behaviour and stop a future edit re-introducing Step 1's bug). Four tests: user not found, identity user not found, role not found, success returns `NoContent` and calls `RemoveFromRoleAsync`.

### Step 3 — `DeleteFriendRequestHandler` · 5 min

`src/API/Application/Features/FriendRequests/Delete/DeleteFriendRequestHandler.cs:11` is a three-part authorization guard with zero coverage. Dependencies are `IUnitOfWork` + `ICurrentUserService` only, so `TestHelpers` covers it outright.

- `Handle_RequestNotFound_ReturnsError`
- `Handle_CallerIsNotRequester_ReturnsError` ← the authorization case; requester id 2, caller id 1
- `Handle_RequestNotPending_ReturnsError`
- `Handle_ValidRequest_DeletesAndReturnsNoContent` — assert `Delete` and `CommitAsync` both received

### Step 4 — `SetProfilePictureHandler` · 3 min

Smallest ownership-critical handler in the codebase; ownership rests entirely on the repository returning 0 rows.

- `Handle_PictureNotOwnedOrMissing_ReturnsError` (stub `SetProfilePictureIfOwnedAsync` → `0`)
- `Handle_PictureOwned_ReturnsSuccess` (→ `1`, and assert it was called with the id from `ICurrentUserService`, not from the command)

### Step 5 — `Result<T>` in `Shared.Test` · 6 min

57.6% → ~95%. Pure, no mocks, and it is the contract every controller branches on to choose an HTTP status — the highest coverage gain per minute available.

Add `test/unit/Shared.Test/Results/ResultTests.cs` covering each factory (`Success`, `Created`, `NoContent`, `Error`, `NotFound`, `Unauthorized`) and asserting `IsSuccess`, `Status`, `Value`, and `Errors` for each — particularly that failure results never report `IsSuccess`.

### Step 6 — `ClaimsPrincipalExtensions` in `Shared.Test` · 4 min

0% today, and it is the sole identity path for both SignalR hubs and the `LogUserActivity` filter. Pure function over a hand-built `ClaimsPrincipal`.

Add `test/unit/Shared.Test/Extensions/ClaimsPrincipalExtensionsTests.cs`:
- `GetPublicId_ValidNameIdentifier_ReturnsId`
- `GetPublicId_MissingClaim_Throws`
- `GetPublicId_NonNumericClaim_Throws`
- `GetEmail_MissingClaim_ReturnsEmptyString`
- `GetRoles_MultipleRoleClaims_ReturnsAll`

### Step 7 — Verify · 2 min

Re-run all four unit projects with coverage, regenerate the summary, and record the delta against `docs/testing/coverage-baseline-2026-09-01.txt`.

### Expected outcome

| | Before | After (est.) |
|---|---|---|
| Unit tests | 128 | ~152 |
| Hand-written line coverage | 28.2% | ~34% |
| `Application` | 37.6% | ~46% |
| `Shared` | 18.9% | ~45% |
| Untested Tier-A authz handlers | 8 | 4 |
| Known live bugs in untested code | 1 | 0 |

The percentage moves modestly on purpose. The real deliverable is that four of the eight authorization-critical handlers gain regression tests and the broken one gets fixed.

---

## 3. Explicitly not in today's session

Deferred with reasons, so the next session starts from a decision rather than a rediscovery.

**Needs the integration suite, not unit tests:**
- `RefreshTokenHandler` — opens a transaction and calls `ExecuteUpdateAsync` on the `DbSet`; not fakeable with NSubstitute. Tier-A risk, so this is the top item for the *next* session, as an `API.Test` case against Testcontainers covering rotation, reuse of a consumed token, and an expired token.
- `UserProvisioningService`, `CloudinaryPictureService`, `GoogleCredentialValidator`, all repositories, `UnitOfWork`, `DatabaseInitializer`.

**Next unit-test session (~30 min):**
- `DeleteUserPictureHandler` — needs an `IPictureService` mock; ownership plus the Cloudinary-failure path that must not delete the DB row.
- `GetUserPictureByIdHandler`, `GetPicturesHandler`, `CreateUserPictureHandler`.
- `SpecificationEvaluator` and `Specification<T>` (0%) — the query-shaping engine.
- `PaginationParams` page-size clamping; `DateTimeExtensions.ToRelativeTimeString` branches.
- `Domain.Post` (51.5%), `UserPicture` and `MediaAggregate.Picture` (0%).

**Structural, needs a decision first:**
- **No unit test project exists for the `API` assembly.** Seven FluentValidation validators, all controllers, both SignalR hubs and the MVC filters have no unit coverage and never appear in the coverage report. Validators are cheap and self-contained (`validator.TestValidate(request)`); adding an `API.Test` unit project — named to avoid colliding with the existing integration project `test/integration/API.Test` — is the single largest remaining structural gap.
- `CachedUserRepository` never invalidates on write (1-minute stale window after profile updates). Confirm whether that is intended before writing a test that would enshrine it.
- Housekeeping from report §4: resolve the duplicate `AuthTests.cs` / `UsersTests.cs` pairs in the integration project, and delete the stray untracked root `tests/` directory.

**Worth adding once coverage stabilises:** a coverage step in `.github/workflows/api-ci.yml` publishing the summary, with a floor set just under the achieved number so it ratchets rather than blocks.

---

## 4. Session results

Steps 1–7 executed 2026-09-01. Raw before/after tool output: [`coverage-baseline-2026-09-01.txt`](./testing/coverage-baseline-2026-09-01.txt) / [`coverage-after-session-2026-09-01.txt`](./testing/coverage-after-session-2026-09-01.txt).

| | Planned | Actual |
|---|---|---|
| Unit tests | ~152 | **173** (128 → 173, +45) |
| Hand-written line coverage | ~34% | **33%** (28.2% → 33.0%) |
| `Application` | ~46% | **47.9%** |
| `Shared` | ~45% | **29.7%** (see deviation below) |
| Untested Tier-A authz handlers | 8 → 4 | **8 → 4** |
| Known live bugs in untested code | 1 → 0 | **1 → 0** — fixed |

**The bug is fixed.** `AssignRoleToUserHandler.cs:21` now reads `roleManager.FindByIdAsync(command.RoleId)` instead of `user.IdentityId`. `test/unit/Application.Test/Features/UserRoles/AssignRoleToUser/AssignRoleToUserHandlerTests.cs` was written and run against the *unmodified* handler first — `Handle_ValidRequest_AssignsRequestedRoleAndReturnsSuccess` failed cleanly (`Assert.True: Expected True, Actual False`), confirming the defect before the fix landed, then passed after.

**Deviation from plan:** Step 1's "assign fails" sub-case couldn't use the "assign the same role twice" approach described in the plan — `UserManagerTestHelper`'s real, store-backed `UserManager` is built with a `null` `ILogger`, and `UserManager.AddToRoleAsync`'s "already in role" branch logs before returning the failed `IdentityResult`, which throws through the null logger. (The observed exception was a `NullReferenceException`; a later audit of `Microsoft.Extensions.Identity.Core` 10.0.9 suggests the log call is at Debug level and would more likely surface as `ArgumentNullException`. The exact type is incidental — what matters is that the null logger makes that branch unreachable with the real store-backed manager.) Worked around by mocking `UserManager<IdentityUser>` directly for that one test case (same `Substitute.For<T>(store, null!, ...)` pattern already used for `RoleManager` in this file), rather than routing it through the real store. `UserManagerTestHelper` itself was left unchanged — giving it an optional logger parameter is a candidate for the next session if more tests need this path.

**Every targeted type reached 100% *line* coverage**, measured per-class before/after:

| Type | Before | After |
|---|---|---|
| `AssignRoleToUserHandler` | 0% | **100%** |
| `RemoveRoleFromUserHandler` | 0% | **100%** |
| `DeleteFriendRequestHandler` | 0% | **100%** |
| `SetProfilePictureHandler` | 0% | **100%** |
| `Results.Result<T>` | 57.6% | **100%** |
| `Extensions.ClaimsPrincipalExtensions` | 0% | **100%** |

**100% line coverage is not 100% branch coverage** — solution-wide branch coverage is 38.5%, and an adversarial review of these tests found at least one specific unexercised branch: the `role.Name is null` half of the guard in `AssignRoleToUserHandler.cs:23` / `RemoveRoleFromUserHandler.cs:23` is never evaluated true, because both `Handle_RoleNotFound` tests stub the role itself to `null` and short-circuit. Deleting `|| role.Name is null` from either handler would leave the suite green. Follow-ups in §5 Priority 2.

**`Shared` still landed lower than estimated (29.7% vs ~45%)** despite both its targets hitting 100%. The estimate simply assumed those two types were a larger share of the assembly than they are: `Specification<T>`/`SpecificationEvaluator` (0%), `RepositoryBase<T>` (0%), `PaginationParams` (66.6%) and `DateTimeExtensions.ToRelativeTimeString` were all correctly deferred to §3 and remain untested, and they dominate the remaining denominator. The planned work was fully delivered; the assembly-level projection was miscalibrated.

**Known-deliberate choice:** `ClaimsPrincipalExtensionsTests` asserts `Assert.Throws<Exception>`, because `ClaimsPrincipalExtensions.GetPublicId` throws a bare `Exception`. That pins current behaviour rather than desired behaviour — the extension should throw `InvalidOperationException` (as `Infrastructure.Auth.CurrentUserService` does for the identical rule). Changing both together is queued in §5.

**New files added this session** (all under existing directories, following existing naming/structure — no new top-level structure):
- `test/unit/Application.Test/Features/UserRoles/AssignRoleToUser/AssignRoleToUserHandlerTests.cs` (5 tests)
- `test/unit/Application.Test/Features/UserRoles/RemoveRoleFromUser/RemoveRoleFromUserHandlerTests.cs` (5 tests)
- `test/unit/Application.Test/Features/FriendRequests/Delete/DeleteFriendRequestHandlerTests.cs` (4 tests)
- `test/unit/Application.Test/Features/Users/SetProfilePciture/SetProfilePictureHandlerTests.cs` (2 tests)
- `test/unit/Shared.Test/Results/ResultTests.cs` (22 tests)
- `test/unit/Shared.Test/Extensions/ClaimsPrincipalExtensionsTests.cs` (7 tests)

**The bug class is not systemic.** Swept every `FindByIdAsync` / `FindByNameAsync` call in `Application` and `Infrastructure`: all `userManager.FindByIdAsync` calls correctly pass a user identity id, and all `roleManager.FindByIdAsync` calls correctly pass a role id (`DeleteRoleHandler` uses `command.Id`, `RemoveRoleFromUserHandler` uses `command.RoleId`). The defect was isolated to the single line now fixed.

**One unrelated pre-existing issue surfaced — and it is breaking CI.** `dotnet build SocialApp.slnx` fails solution-wide on `NU1903` promoted to an error by `TreatWarningsAsErrors`: `SSH.NET 2025.1.0` has a known high-severity advisory ([GHSA-q939-rpr3-3284](https://github.com/advisories/GHSA-q939-rpr3-3284)) and arrives transitively via `Testcontainers.MsSql 4.13.0` → `Testcontainers 4.13.0` → `SSH.NET`, in `test/integration/API.Test`.

Verified by stashing all session changes and building a clean `develop` checkout: it fails identically, so this predates and is independent of this session's work. It matters more than a local annoyance because [`api-ci.yml`](../.github/workflows/api-ci.yml) runs `dotnet restore/build/test SocialApp.slnx` on every push to `develop` and every PR into `master`/`develop` — **the same command that fails locally**, so CI is red on `develop` today and any PR opened now inherits a red build regardless of its contents.

The four unit test projects are unaffected and build/run independently, which is why this session's work could be verified at all. Fixing this is Priority 0 in §5 and should land *before* this session's branch, so the coverage PR gets a genuinely green build.

---

## 5. Next session

Ordered by what unblocks the most. Priority 0 is not a coverage task but gates everything else.

### Priority 0 — unblock CI (~10 min, do before landing this session's branch)

CI is red on `develop` for a reason unrelated to coverage (§4). Until it is fixed, no PR can show a green build and no integration test can even compile.

1. Bump `Testcontainers.MsSql` `4.13.0` → `4.14.0` in [`Directory.Packages.props`](../Directory.Packages.props) (4.14.0 is the current release).
2. `dotnet restore SocialApp.slnx`, then re-check whether `SSH.NET` still resolves to `2025.1.0` (`grep '"SSH.NET"' test/integration/API.Test/obj/project.assets.json`).
3. If it still resolves to the vulnerable version, pin it directly — `SSH.NET 2026.0.0` is the only release newer than `2025.1.0` — by adding a `PackageVersion` entry plus a `PackageReference` in `API.Test.csproj`, which overrides the transitive resolution under central package management.
4. Only if neither clears the advisory: scope the audit rather than disabling it wholesale — `<NuGetAuditMode>direct</NuGetAuditMode>` — with a comment explaining that this is a test-only transitive dependency never shipped in the API image. Prefer a real upgrade; treat suppression as the fallback.
5. Gate: `dotnet build SocialApp.slnx` exits 0.

### Priority 1 — close the last two Tier-A authorization gaps (~15 min)

This finishes the tier the whole plan was built around, taking untested Tier-A handlers from 4 to 2 (the remaining two, `RefreshTokenHandler` and `GoogleSignInHandler`, are integration-test work).

- **`DeleteUserPictureHandler`** — the most consequential one left, because it deletes from Cloudinary *before* touching the database:
  - caller has no `ApplicationUser` → `Unauthorized`
  - picture id not among the caller's pictures → `NotFound` (ownership)
  - **Cloudinary delete fails → the DB row must survive**: assert `PictureRepository.Delete` was *not* received and `CommitAsync` was *not* called
  - success → deletes, commits, returns `NoContent`
  - Needs an `IPictureService` mock; everything else is covered by existing `TestHelpers`.
- **`GetUserPictureByIdHandler`** — scoping: a picture belonging to another user must not be returned.

### Priority 2 — act on the adversarial review of this session's tests (~20 min)

An adversarial review confirmed the headline result — the fix is correct, and the `AssignRoleToUser` / `RemoveRoleFromUser` / `DeleteFriendRequest` tests are genuinely mutation-resistant (each fails when the guard they cover is deleted, inverted, or swapped). It found no high-severity tautology. It did find real weaknesses worth closing, roughly in value order:

1. **`Assert.NotEmpty(result.Errors)` can never fail.** `AssignRoleToUserHandlerTests.cs:156`, `RemoveRoleFromUserHandlerTests.cs:153`. `Result<T>.Error(string)` always populates a one-element `Errors`, so this asserts nothing beyond the `Status` check above it. The behaviour actually at risk — `string.Join('\n', result.Errors.Select(e => e.Description))` in both handlers, which is what `UserRolesController` returns to the client — is unverified. Replace with `Assert.Contains("User already in role.", result.Errors)` and add a two-error case pinning the `'\n'` join. (The plan originally named this test `..._ReturnsErrorWithIdentityErrors`; the delivered version dropped both the suffix and the assertion.)
2. **Exercise the `role.Name is null` branch** in both role handlers (see §4) with a `new IdentityRole { Name = null }`.
3. **`SetProfilePictureHandlerTests.cs` "not owned" test stubs a no-op.** `.Returns(0)` matches NSubstitute's default for `Task<int>`, and the test asserts nothing about arguments — so it passes against a handler that ignores `ICurrentUserService`, swaps the two `int` arguments, or never calls the repository. Only its sibling success test pins argument order. Add a `Received(1)` assertion to the ownership-named test.
4. **Unstubbed `RoleManager` auto-substitutes a non-null role.** `Substitute.For<RoleManager<IdentityRole>>(…).FindByIdAsync(…)` returns a proxy with `Name == ""` (not null), so `Handle_UserNotFound` / `Handle_IdentityUserNotFound` pass only because an earlier guard short-circuits first; reordering the handler's guards would push them into `AddToRoleAsync(user, "")`. Stub `FindByIdAsync(Arg.Any<string>())` explicitly in every test in both files.
5. **Distinguish the three `NotFound` outcomes.** All three assert only `Status == NotFound` while the handlers emit three distinct messages, so nothing pins which guard fired. Assert on `result.Errors`.
6. **Assert `Status` on failure paths, and on Assign's success path.** `DeleteFriendRequestHandlerTests` asserts only `IsSuccess == false` on its three failure tests, so switching the handler to `Forbidden`/`NotFound` would silently change the HTTP contract. Conversely `SetProfilePictureHandlerTests` and `AssignRoleToUserHandlerTests` never assert the success status (`Remove` does assert `NoContent`; the two sibling endpoints genuinely differ and Assign's side is unpinned).
7. **Assert `CommitAsync` was *not* called** on `DeleteFriendRequestHandler`'s failure paths — currently only `Delete` is checked, so a refactor that commits regardless of the guard would pass.
8. **Assert `.Succeeded` on the seeding `AddToRoleAsync`** at `RemoveRoleFromUserHandlerTests.cs:108`; if seeding silently failed, the post-condition assertion would pass vacuously.
9. **Drop three redundant `ResultTests`** (`IsSuccess_ForOk`/`ForCreated`/`ForNoContent` duplicate assertions already made by the per-factory facts), and note `Assert.Equal(ResultStatus.Ok, …)` after `Success(T)` cannot fail since `Ok` is the field initialiser. Untouched on `Result<T>`: `CorrelationId`, `Location`'s default under the one-arg `Created`, and `Value` on failure results.

### Priority 3 — test-suite maintainability (~5 min)

- Extract `Substitute.For<UserManager<IdentityUser>>(store, null!, …)` into `TestHelpers.CreateMockUserManager()`. The nine-positional-`null!` incantation is currently duplicated verbatim in the `AssignRoleToUser` and `RemoveRoleFromUser` test files and is brittle against Identity constructor changes.
- Optionally give `UserManagerTestHelper.CreateUserManagerWithUsers` an optional `ILogger<UserManager<IdentityUser>>` (defaulting to `NullLogger`). Its current `null` logger is why `AddToRoleAsync`'s "already in role" branch throws `NullReferenceException` (§4 deviation); fixing it would let those tests use the real store like their siblings instead of a full mock.

### Later sessions (carried from §3)

- Change `ClaimsPrincipalExtensions.GetPublicId` to throw `InvalidOperationException` instead of bare `Exception`, update the assertion in `ClaimsPrincipalExtensionsTests`, and consider collapsing the rule's duplication with `Infrastructure.Auth.CurrentUserService`.
- Add a unit test project for the `API` assembly (7 FluentValidation validators, controllers, hubs, filters) — the largest remaining structural gap. Name it to avoid colliding with the existing integration project `test/integration/API.Test`.
- `RefreshTokenHandler` integration test against Testcontainers: rotation, reuse of a consumed token, expired token. Requires Priority 0 first — it lives in the project that currently fails to build.
- `SpecificationEvaluator` / `Specification<T>`; `PaginationParams` clamping; `DateTimeExtensions.ToRelativeTimeString` branches; `Domain.Post` (51.5%), `UserPicture` / `MediaAggregate.Picture` (0%).
- Decide whether `CachedUserRepository`'s 1-minute non-invalidating cache is intended before writing a test that enshrines it.
- Housekeeping: duplicate `AuthTests.cs` / `UsersTests.cs` in the integration project; stray untracked root `tests/` directory; the `SetProfilePciture` spelling in the source tree.
- Once coverage stabilises, add a coverage step to `api-ci.yml` publishing the summary, with a floor just under the achieved number so it ratchets.
