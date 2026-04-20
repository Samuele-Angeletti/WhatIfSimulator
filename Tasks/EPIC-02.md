# EPIC-02 · Simulation Engine Core

**Phase:** Phase 1 — MVP

> **Status legend:** `[ ]` To do · `[~]` In progress · `[x]` Done

## Tasks

- [ ] Implement `SimulationEngine` — main loop that advances world state by one tick
- [ ] Implement seeded random number generator wrapper (deterministic, loggable)
- [ ] Implement fixed timestep tick system
- [ ] Implement state transition logger (records full state at each tick)
- [ ] Implement `WorldState` loader and saver (serialize/deserialize from JSON via `JsonUtility`)
- [ ] Implement seed + scenario reproducibility test — same inputs must always produce identical output
- [ ] Write unit tests for determinism (run simulation twice with same seed, assert identical output)
