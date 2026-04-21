# EPIC-22 · LLM Infrastructure

**Phase:** Phase 3 — Beta

> **Status legend:** `[ ]` To do · `[~]` In progress · `[x]` Done

## Tasks

- [ ] Implement configuration system for LLM backend (endpoint, model, auth)
- [ ] Implement request queue — prevents simultaneous LLM calls from blocking the simulation
- [ ] Implement graceful degradation — game remains fully playable if LLM is unavailable
