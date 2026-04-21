# Data

`WhatIfSimulator/Assets/Data` is the canonical home for simulation contracts, example JSON, and future data-driven content.

Current responsibilities:

- Contract documentation under `Contracts/`
- Example JSON files under schema-specific subfolders
- A stable surface for future loaders, validators, tests, and contributor tooling

Repo-root governance lives outside the Unity project. Use:

- `README.md` for product intent
- `CONTRIBUTING.md` for workflow and style rules
- `.github/ISSUE_TEMPLATE/` for issue workflows

Data files in this folder should stay compatible with Unity `JsonUtility`, which means:

- Prefer explicit objects and arrays
- Avoid raw dictionary maps in canonical JSON examples
- Use discriminated payload objects for variant scenario modifications
