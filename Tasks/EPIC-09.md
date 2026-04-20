# EPIC-09 · Temporal Chunking & Fast-Forward

**Phase:** Phase 1 — MVP

> **Status legend:** `[ ]` To do · `[~]` In progress · `[x]` Done

## Tasks

- [ ] Implement chunk definition format — start timestamp, end timestamp, resolution level
- [ ] Implement coarse-resolution simulation pass (fast-forward mode)
- [ ] Implement divergence resolver — propagates What If modifications across a skipped chunk
- [ ] Implement event generator for fast-forward chunks (probabilistic but seeded)
- [ ] Implement `ChunkSummary` emitter — outputs key events and state delta for the skipped interval
- [ ] Define built-in era boundaries (e.g. Cretaceous, Holocene, Early Civilization, Medieval, Modern)
- [ ] Write integration test — fast-forward from −65M to −10K produces valid world state
