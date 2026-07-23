# Contributing

## Git conventions

These conventions apply **starting today**. Existing history is not being rewritten — this is a going-forward standard so commits and branches stay consistent and easy to scan.

### Commit messages

Format:

```
<type>(<scope>): <short summary>

[optional body]

[optional footer, e.g. Refs #123]
```

**Types**

| Type       | Use for |
|------------|---------|
| `feat`     | A new feature |
| `fix`      | A bug fix |
| `refactor` | Code change that neither fixes a bug nor adds a feature |
| `docs`     | Documentation only |
| `test`     | Adding or fixing tests |
| `chore`    | Maintenance (deps, tooling, config) with no production code change |
| `perf`     | Performance improvement |
| `build`    | Build system or packaging changes |
| `ci`       | CI/CD pipeline changes (`.github/workflows`, etc.) |
| `style`    | Formatting only, no logic change |

**Scope** (optional, recommended when a change is isolated to one side): `api`, `ui`, or a more specific layer such as `domain`, `application`, `infrastructure`. Omit the scope when a change spans both `src/API` and `src/UI`.

**Issue reference** (optional): when a commit relates to a tracked GitHub issue, add a footer line — `Refs #123`. Not required otherwise.

**Avoid:**
- Non-standard type spellings — use `refactor`, not `refact`.
- Typos in the type.
- Unprefixed messages like "Update README" or "update md".

**Examples**

| Bad (from history) | Good |
|---|---|
| `refact: cleanup service` | `refactor(api): simplify user service dependency injection` |
| `dix: login bug` | `fix(ui): correct redirect after login` |
| `Update README` | `docs: update setup instructions in README` |
| `chore(deps): bump nuget packages` | `chore(api): bump NuGet package versions` |

### Branch naming

Format:

```
<type>/<short-kebab-description>
<type>/<issue-number>-<short-kebab-description>   (when an issue exists)
```

**Types**: `feature/`, `fix/`, `chore/`, `docs/`, `refactor/`, `release/`, `hotfix/`

Keep the description short and kebab-case. An issue number prefix is encouraged when a tracked issue exists, but not mandatory.

Examples: `feature/user-auth`, `fix/123-login-redirect`, `chore/update-nuget-packages`, `release/1.2.0`, `hotfix/critical-payment-bug`.

### Branch workflow

```
master  <- release/*  <- develop <- feature/*
                                  <- fix/*
hotfix/*  branches off master directly, merges back into both master and develop
```

- **`develop`** — integration branch. All feature and fix work merges here first. This is the repo's default branch and the CI build target.
- **`master`** — production/deployable branch. Pushes to `master` trigger the deploy workflows.
- **`feature/*`** and **`fix/*`** — branch from `develop`, merge back into `develop`.
- **`release/*`** — branch from `develop` when preparing a release; merges into `master` (and back into `develop` to keep them in sync).
- **`hotfix/*`** — branch from `master` for urgent production fixes; merges into both `master` and `develop`.

`master` and `develop` are the only branches CI currently treats specially (build and deploy triggers in `.github/workflows/`) — don't rename or delete them.
