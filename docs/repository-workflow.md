# Repository Workflow

## Branches

- `main`: always intended to stay buildable and usable
- `feature/<name>`: new mechanics, content, visuals, or tooling
- `fix/<name>`: bug fixes and regression work
- `chore/<name>`: repo cleanup, docs, localization, and non-feature maintenance

## Recommended Flow

1. Branch from `main`.
2. Keep each branch focused on one topic.
3. Build locally before pushing.
4. Push branch to GitHub.
5. Merge back into `main` after review or validation.

## Suggested Commit Style

- `feat: add new gunslinger card`
- `fix: register starter relic as active relic`
- `docs: add repository setup guide`
- `chore: ignore local build paths`

## Pre-Push Checks

```powershell
dotnet build .\GunslingerMod.csproj -c Debug
```

If the change affects visuals, also verify the mod in-game after copying the latest output to the game `mods` directory.

## Local Files That Should Not Be Committed

- `local.props`
- `.nuget/`
- `.godot/`
- `.mono/`
- generated `.dll` / `.pck`
- local test outputs under `build_out/`
