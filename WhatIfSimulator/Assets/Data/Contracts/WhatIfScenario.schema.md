# WhatIfScenario Schema

## Required fields

- `schemaVersion` : string
- `id` : string
- `name` : string
- `description` : string
- `modifications` : array of `ScenarioModification`

## Optional fields

- `tags` : array of string

## Constraints

- Each `ScenarioModification` must declare `modificationType`
- Each modification must set exactly one matching payload block
- Supported `modificationType` values are `remove_event`, `add_event`, `modify_parameter`, `inject_rule`, and `alter_initial_conditions`

## Determinism notes

Scenario composition must preserve deterministic behavior. Two identical scenario sets applied to the same starting state must produce identical results.

## JsonUtility compatibility

Canonical JSON uses discriminated objects with fixed optional payload sections such as `removeEvent` or `modifyParameter`. Avoid runtime polymorphism or loosely shaped objects.
