# WorldState Schema

## Required fields

- `schemaVersion` : string
- `seed` : integer
- `currentTimestamp` : number
- `tick` : integer
- `regions` : array of `RegionState`

## Optional fields

- `activeScenarioIds` : array of string
- `globalMetrics` : array of key/value metric entries

## Constraints

- `tick` must be `>= 0`
- `regions` must contain at least one region
- Region identifiers must be unique within the snapshot
- Numeric values should be finite and deterministic for the same seed and starting state

## Determinism notes

`WorldState` is the complete snapshot at a simulation tick. The same seed, scenario set, and prior state transitions must produce the same `WorldState`.

## JsonUtility compatibility

Use explicit serializable objects and arrays. Canonical examples avoid raw dictionary maps and instead use entry arrays for dynamic metrics or resources.
