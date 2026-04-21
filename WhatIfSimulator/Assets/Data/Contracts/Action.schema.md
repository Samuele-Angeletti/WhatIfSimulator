# Action Schema

## Required fields

- `schemaVersion` : string
- `action` : string
- `target` : string
- `region` : string
- `intensity` : number

## Optional fields

- `reason` : string

## Constraints

- `intensity` must be between `0.0` and `1.0`
- `action`, `target`, and `region` must be non-empty strings
- Actions must be validated before they reach the simulation engine

## Determinism notes

Actions are inputs into deterministic simulation logic. The action document itself is data only and must not embed runtime behavior.

## JsonUtility compatibility

Use flat serializable fields for canonical action payloads. Keep nested optional data explicit if added later.
