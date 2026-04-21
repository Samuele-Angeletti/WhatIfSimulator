# EPIC-20 · LLM Interpreter

**Phase:** Phase 3 — Beta

> **Status legend:** `[ ]` To do · `[~]` In progress · `[x]` Done

## Tasks

- [ ] Define interpreter system prompt — constraints, output schema, rejection criteria
- [ ] Implement LLM API client (local inference or self-hosted backend, configurable)
- [ ] Implement natural language → Action JSON pipeline
- [ ] Implement output validator — rejects any LLM response that fails action schema
- [ ] Implement fallback behavior — if LLM output is invalid after N retries, return structured error to player
- [ ] Write integration test — a set of natural language inputs maps to correct validated actions
