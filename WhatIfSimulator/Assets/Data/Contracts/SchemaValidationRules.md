# Schema Validation Rules

## Required-field behavior

- Missing required fields fail validation.
- Empty strings fail validation for identifier and label fields.
- Arrays that are part of the canonical shape should be present even when empty.

## Numeric constraints

- `Action.intensity` must stay within `0.0` to `1.0`
- `WorldState.tick` must be `>= 0`
- `ChunkSummary.endTimestamp` must be greater than `startTimestamp`

## Allowed enum-like values

- `WhatIfScenario.modificationType` must be one of the approved modification identifiers
- Resolution fields should use stable project-defined identifiers

## JsonUtility caveats

- Avoid raw dictionary maps in canonical JSON
- Prefer explicit entry arrays such as `impactEntries`
- Use fixed payload sections for variant modifications
- Keep payload shapes serializable through Unity `JsonUtility`

## Error codes

- `SCHEMA_REQUIRED_FIELD_MISSING`
- `SCHEMA_INVALID_TYPE`
- `SCHEMA_INVALID_RANGE`
- `SCHEMA_UNSUPPORTED_MAP_SHAPE`
- `SCHEMA_UNKNOWN_MODIFICATION_TYPE`
- `SCHEMA_MULTIPLE_PAYLOADS_SET`
- `SCHEMA_REFERENCE_NOT_FOUND`
- `SCHEMA_DETERMINISM_RULE_VIOLATION`
- `DOC_PATH_MISMATCH`
- `GOVERNANCE_POLICY_MISMATCH`

## ValidationResult pattern

Future validators should return a non-throwing `ValidationResult` object with:

- `isValid`
- `errors`
- `warnings`

Invalid files should be rejected before they are consumed by simulation or LLM-adjacent systems.
