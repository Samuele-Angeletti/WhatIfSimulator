# SimulationEvent Schema

## Required fields

- `schemaVersion` : string
- `id` : string
- `timestamp` : number
- `eventType` : string
- `description` : string
- `chunkGenerated` : boolean
- `impactEntries` : array of `ImpactEntry`

## Optional fields

- `sourceScenarioIds` : array of string

## Constraints

- `id` must be unique within an event collection
- `impactEntries` uses explicit `{ key, value }` objects
- `eventType` should use a stable identifier format

## Determinism notes

Events generated from the same seed and state progression must keep the same timestamps, type identifiers, and impact values.

## JsonUtility compatibility

Canonical JSON does not use a raw `impact` map. Use `impactEntries` so Unity `JsonUtility` can deserialize the data without a custom serializer.
