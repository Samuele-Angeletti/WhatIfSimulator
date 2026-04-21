# ChunkSummary Schema

## Required fields

- `schemaVersion` : string
- `startTimestamp` : number
- `endTimestamp` : number
- `resolution` : string
- `events` : array of `ChunkEventReference`
- `stateDeltaEntries` : array of key/value entries
- `narrativePlaceholder` : string

## Optional fields

- `divergenceNotes` : array of string

## Constraints

- `endTimestamp` must be greater than `startTimestamp`
- `events` may be empty, but the array must be present
- `narrativePlaceholder` should always be set, even before narrator systems exist

## Determinism notes

Chunk summaries represent deterministic fast-forward results. Summary contents must remain reproducible for the same seed, active scenarios, and chunk boundaries.

## JsonUtility compatibility

Use explicit arrays for event references and state deltas. Avoid nested dictionary maps in canonical JSON examples.
