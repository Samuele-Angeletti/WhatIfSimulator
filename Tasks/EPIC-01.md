# EPIC-01 · Core Data Schemas

**Phase:** Phase 1 - MVP

> All data formats must be finalized before engine work begins. These schemas are the contract between all systems.

> **Status legend:** `[ ]` To do · `[~]` In progress · `[x]` Done

## Tasks

- [ ] Define `WorldState` schema - the complete snapshot of simulation state at any tick
- [ ] Define `SimulationEvent` schema (including `chunkGenerated` and JsonUtility-safe `impactEntries`)
- [ ] Define `Action` schema (`action`, `target`, `region`, `intensity`)
- [ ] Define `WhatIfScenario` schema (including all supported `modificationType` values and explicit payload sections)
- [ ] Define `ChunkSummary` schema - output of a fast-forward pass (events, state delta, narrative placeholder)
- [ ] Write JSON validation rules and specs for each schema under `WhatIfSimulator/Assets/Data/Contracts`
- [ ] Add example files for each schema under `WhatIfSimulator/Assets/Data`
