# STS2-MOD

Gunslinger custom character mod project for Slay the Spire 2.

## Stack

- Godot `4.5.1` Mono
- `.NET 9`
- Slay the Spire 2 + `BaseLib`

## Repository Layout

- `src/GunslingerMod/`: gameplay code, cards, relics, patches, character model
- `src/Core/`: reusable Godot/BaseLib helper nodes used by the mod
- `scenes/`: Godot scenes for visuals, UI, rest site, merchant, character select
- `images/`: packed UI, portraits, relics, VFX, map and top-panel assets
- `GunslingerMod/localization/`: English, Japanese, Korean localization
- `docs/`: balancing notes, audits, card specs, workflow notes

## Local Setup

1. Install Godot Mono `4.5.1`.
2. Install `.NET SDK 9`.
3. Install Slay the Spire 2 and BaseLib.
4. Copy `local.props.example` to `local.props`.
5. Set your local game path, output path, and Godot executable path.

`local.props` is ignored on purpose because it contains machine-specific paths.

## Build

```powershell
dotnet build .\GunslingerMod.csproj -c Debug
```

The project is configured to:

- build the mod assembly
- generate `mod_manifest.json`
- export the Godot `.pck`
- copy outputs to `ModsOutputDir`

## Main Entry Points

- Character model: `src/GunslingerMod/Models/Characters/Gunslinger.cs`
- Content registration: `src/GunslingerMod/Framework/Registration/GunslingerContentSpec.cs`
- Bootstrap: `src/GunslingerMod/Framework/Bootstrap/GunslingerBootstrap.cs`
- Starter relic: `src/GunslingerMod/Relics/CylinderRelic.cs`

## Git Workflow

- `main`: stable integration branch
- short-lived feature/fix branches from `main`
- merge back through reviewed commits

Detailed notes: `docs/repository-workflow.md`

## Notes

- Generated build artifacts such as `.dll`, `.pck`, `.godot/`, `.mono/`, and `build_out/` are ignored.
- `local.props` and `.nuget/` are ignored to keep local machine state out of the repo.
