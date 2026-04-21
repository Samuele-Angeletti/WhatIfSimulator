# Contributing to What If Simulator

This repository is split across two working areas:

- The repository root contains governance and planning documents such as `README.md`, `TASKS.md`, `Tasks/`, `.kairos/`, and `.github/`.
- The Unity project lives under `WhatIfSimulator/`. Gameplay code, data assets, tests, and future tooling belong under `WhatIfSimulator/Assets`.

`README.md` is the source of truth for the project vision, determinism requirements, approved library policy, and simulation boundaries. When in doubt, align new work to `README.md` first.

## Branching Strategy

- `master` is the protected integration branch.
- Create short-lived feature branches from `master` using descriptive names such as `epic-01-core-data-schemas` or `docs/contributing-guide`.
- Keep unrelated work in separate branches and pull requests.
- Rebase or merge from `master` regularly so schema, docs, and implementation changes stay aligned.

## Pull Request Process

1. Confirm the work maps to an epic or issue.
2. Update documentation and examples alongside code when contracts change.
3. Run the relevant local checks before opening the PR.
4. Open a pull request against `master`.
5. Request at least one review before merge.

Pull requests should include:

- A short summary of the change.
- The epic, task, or issue it addresses.
- Notes about determinism impact, if any.
- Notes about schema or documentation changes, if any.
- Screenshots only when UI work makes them useful.

## Branch Protection

Branch protection on `master` is a maintainer action performed in GitHub repository settings. The expected protection rules are:

- Require a pull request before merging.
- Require at least one review approval before merging.
- Prevent direct pushes to `master`.

If you have maintainer access, configure those settings before marking EPIC-00 complete. If you do not have maintainer access, note the remaining admin step in the related issue or pull request.

## External Library Approval Policy

Use only what ships with a standard Unity project unless a library is explicitly approved.

Any library that requires installation through the Unity Package Manager, manual DLL inclusion, or another non-default setup step must be approved before use. This includes serializers, async helpers, animation libraries, editor tooling, and ML or inference SDKs.

To request approval, open the `Library Approval Request` issue template and include:

1. Library name and version
2. Source link or package registry link
3. Why it is needed
4. What Unity-native alternative was considered
5. License

Do not merge pull requests that add unapproved external dependencies.

## Repository Layout

Canonical Unity implementation paths live under `WhatIfSimulator/Assets`:

- `WhatIfSimulator/Assets/Core` for simulation engine and subsystem code
- `WhatIfSimulator/Assets/Data` for schemas, example JSON, and future content assets
- `WhatIfSimulator/Assets/LLM` for interpreter and narrator integration work
- `WhatIfSimulator/Assets/UI` for timeline, event feed, and presentation code
- `WhatIfSimulator/Assets/Tests` for EditMode and PlayMode tests

Repo-root documentation paths stay outside the Unity project:

- `README.md` for product and architecture intent
- `TASKS.md` and `Tasks/` for roadmap tracking
- `.github/ISSUE_TEMPLATE/` for issue workflows
- `.kairos/` for approved workflow artifacts

## C# Code Style Conventions

These conventions apply to project code and tests unless a Unity-specific pattern makes a small deviation clearer.

### Naming

- Use `PascalCase` for classes, structs, enums, properties, and public methods.
- Use `camelCase` for local variables and private serialized fields.
- Prefix private serialized fields with `_`, for example `_currentTick`.
- Use clear noun-based names for data containers and DTOs such as `WorldStateSnapshot` or `ImpactEntry`.
- Match file names to the primary type in the file.

### File Structure

- Prefer one primary type per file.
- Keep namespaces aligned to the folder structure when project code is introduced.
- Place tests under `WhatIfSimulator/Assets/Tests`.
- Keep schema examples and contract docs under `WhatIfSimulator/Assets/Data`.

### Comments

- Write comments only when intent would otherwise be unclear.
- Prefer comments that explain why a constraint exists, not what a line of code already says.
- Document determinism-sensitive assumptions explicitly.

### Serialization

- Favor `JsonUtility`-friendly shapes for JSON-backed DTOs.
- Prefer explicit objects and arrays over raw dictionary fields.
- Use discriminated objects for variant payloads rather than runtime polymorphism.

## Testing Expectations

- Add or update tests when changing contracts, data shapes, or validation rules.
- Keep example JSON files synchronized with the written schema docs.
- Treat determinism as a testable requirement, not a descriptive goal.

## Commit Hygiene

- Make focused commits with clear messages.
- Avoid mixing schema, engine, and unrelated cleanup in the same pull request.
- Do not edit generated Unity files unless the change is intentional and understood.
