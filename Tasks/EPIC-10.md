# EPIC-10 · Action System (Predefined, No LLM)

**Phase:** Phase 1 — MVP

> **Status legend:** `[ ]` To do · `[~]` In progress · `[x]` Done

## Tasks

- [ ] Implement `ActionValidator` — checks action schema validity and intensity bounds
- [ ] Implement `ActionExecutor` — applies a validated action to world state
- [ ] Implement predefined action set: `boost_technology`, `destabilize_region`, `increase_resources`, `accelerate_diffusion`
- [ ] Implement action effect model — maps intensity to magnitude of world state change
- [ ] Write integration test — each predefined action produces expected state delta
