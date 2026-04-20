# EPIC-07 · System Interaction Layer

**Phase:** Phase 1 — MVP

> The layer that allows the four subsystems to read and write shared state.

> **Status legend:** `[ ]` To do · `[~]` In progress · `[x]` Done

## Tasks

- [ ] Define shared state interface — how systems consume and produce state each tick
- [ ] Implement tick orchestrator — executes all systems in correct order each tick
- [ ] Implement inter-system feedback rules (e.g. conflict reduces economy, economy affects population)
- [ ] Write integration test — all four systems running together produce a coherent evolving world over 100 ticks
