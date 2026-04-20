# What If Simulator — Project Pipeline

This document tracks the full development pipeline, organized by epic and phase.  
Each epic maps to a functional area of the project. Tasks within an epic are ordered by dependency — complete them top to bottom unless otherwise noted.

> **Status legend:** `[ ]` To do · `[~]` In progress · `[x]` Done

---

## Phase 0 — Project Foundation

*Prerequisites for any development work. Must be completed before any other epic begins.*

### EPIC-00 · Repository & Standards Setup

- [ ] Initialize Unity project with target Unity version (document version in README)
- [ ] Set up `.gitignore` for Unity projects
- [ ] Define and document folder structure (`/Core`, `/Data`, `/LLM`, `/UI`)
- [ ] Add `README.md`, `\Tasks\EPIC-N.md` (all the tasks), `CONTRIBUTING.md` to repository root
- [ ] Write `CONTRIBUTING.md` — branching strategy, PR process, external library policy
- [ ] Set up branch protection rules on `main` (require PR + review before merge)
- [ ] Create issue templates on GitHub (bug report, feature request, library approval request)
- [ ] Define and document code style conventions (naming, file structure, comment standards) for C#

---

## Phase 1 — MVP

*A working simulation loop with no LLM, predefined actions, and one What If scenario.*

### EPIC-01 · Core Data Schemas

*All data formats must be finalized before engine work begins. These schemas are the contract between all systems.*

- [ ] Define `WorldState` schema — the complete snapshot of simulation state at any tick
- [ ] Define `SimulationEvent` schema (including `chunk_generated` flag and `impact` map)
- [ ] Define `Action` schema (`action`, `target`, `region`, `intensity`)
- [ ] Define `WhatIfScenario` schema (including all supported `modification` types)
- [ ] Define `ChunkSummary` schema — output of a fast-forward pass (events, state delta, narrative placeholder)
- [ ] Write JSON validation rules / specs for each schema
- [ ] Add example files for each schema under `/Data`

---

### EPIC-02 · Simulation Engine Core

- [ ] Implement `SimulationEngine` — main loop that advances world state by one tick
- [ ] Implement seeded random number generator wrapper (deterministic, loggable)
- [ ] Implement fixed timestep tick system
- [ ] Implement state transition logger (records full state at each tick)
- [ ] Implement `WorldState` loader and saver (serialize/deserialize from JSON via `JsonUtility`)
- [ ] Implement seed + scenario reproducibility test — same inputs must always produce identical output
- [ ] Write unit tests for determinism (run simulation twice with same seed, assert identical output)

---

### EPIC-03 · Population System

- [ ] Implement base population growth and decline model (configurable rate parameters)
- [ ] Implement regional population tracking (world divided into configurable regions)
- [ ] Implement migration logic between regions (driven by resource and conflict state)
- [ ] Implement demographic structure (age cohorts, basic mortality model)
- [ ] Expose population parameters as data-driven config (JSON ScriptableObject)
- [ ] Write integration test — population evolves plausibly over N ticks with default parameters

---

### EPIC-04 · Economy System

- [ ] Implement resource types and per-region resource pools
- [ ] Implement resource production model (output per tick, influenced by population and technology)
- [ ] Implement trade flow logic between regions (supply/demand driven)
- [ ] Implement scarcity model — resource shortage triggers effects on population and conflict systems
- [ ] Expose economy parameters as data-driven config
- [ ] Write integration test — scarcity scenario produces expected downstream effects

---

### EPIC-05 · Technology System

- [ ] Define technology tree structure (nodes, prerequisites, diffusion rates)
- [ ] Implement research progression model (per-region advancement per tick)
- [ ] Implement technology diffusion between regions
- [ ] Implement technological acceleration feedback (advanced tech speeds further research)
- [ ] Expose technology tree as data-driven config (JSON)
- [ ] Write integration test — technology spreads and accelerates plausibly over N ticks

---

### EPIC-06 · Conflict System

- [ ] Implement political stability model per region (0.0–1.0 scale)
- [ ] Implement war trigger logic (threshold-based, driven by scarcity and instability)
- [ ] Implement alliance formation and dissolution
- [ ] Implement war outcome model — effects on population, economy, and stability
- [ ] Expose conflict parameters as data-driven config
- [ ] Write integration test — scarcity + instability cascade produces conflict event

---

### EPIC-07 · System Interaction Layer

*The layer that allows the four subsystems to read and write shared state.*

- [ ] Define shared state interface — how systems consume and produce state each tick
- [ ] Implement tick orchestrator — executes all systems in correct order each tick
- [ ] Implement inter-system feedback rules (e.g. conflict reduces economy, economy affects population)
- [ ] Write integration test — all four systems running together produce a coherent evolving world over 100 ticks

---

### EPIC-08 · What If System

- [ ] Implement `WhatIfLoader` — reads and validates scenario JSON files at startup
- [ ] Implement `remove_event` modification type
- [ ] Implement `add_event` modification type
- [ ] Implement `modify_parameter` modification type
- [ ] Implement `inject_rule` modification type
- [ ] Implement `alter_initial_conditions` modification type
- [ ] Implement scenario compositor — applies multiple What If modules together without conflict
- [ ] Create first built-in scenario: `no_dinosaur_extinction`
- [ ] Write integration test — scenario modifies baseline simulation and produces divergent output

---

### EPIC-09 · Temporal Chunking & Fast-Forward

- [ ] Implement chunk definition format — start timestamp, end timestamp, resolution level
- [ ] Implement coarse-resolution simulation pass (fast-forward mode)
- [ ] Implement divergence resolver — propagates What If modifications across a skipped chunk
- [ ] Implement event generator for fast-forward chunks (probabilistic but seeded)
- [ ] Implement `ChunkSummary` emitter — outputs key events and state delta for the skipped interval
- [ ] Define built-in era boundaries (e.g. Cretaceous, Holocene, Early Civilization, Medieval, Modern)
- [ ] Write integration test — fast-forward from −65M to −10K produces valid world state

---

### EPIC-10 · Action System (Predefined, No LLM)

- [ ] Implement `ActionValidator` — checks action schema validity and intensity bounds
- [ ] Implement `ActionExecutor` — applies a validated action to world state
- [ ] Implement predefined action set: `boost_technology`, `destabilize_region`, `increase_resources`, `accelerate_diffusion`
- [ ] Implement action effect model — maps intensity to magnitude of world state change
- [ ] Write integration test — each predefined action produces expected state delta

---

### EPIC-11 · MVP UI

*Minimal interface to observe the simulation. No visual polish required at this stage.*

- [ ] Implement basic Timeline View — scrollable list of emitted events
- [ ] Implement World State panel — display key state values per region (population, resources, stability)
- [ ] Implement predefined Action panel — buttons for each available action
- [ ] Implement Tick controls — step forward, run, pause
- [ ] Implement Fast-Forward controls — select era, trigger chunk pass, display ChunkSummary
- [ ] Implement seed input field — allow player to set simulation seed before start

---

## Phase 2 — Alpha

*Full system interaction, event generation, and a usable timeline UI.*

### EPIC-12 · Event System & Feed

- [ ] Implement event registry — catalog of all possible event types with trigger conditions
- [ ] Implement event trigger evaluator — checks conditions each tick and fires matching events
- [ ] Implement event feed UI — real-time scrolling display of events as they occur
- [ ] Implement event detail view — click an event to see full description and impact
- [ ] Add 10+ baseline historical events to the event registry (e.g. ice ages, volcanic eruptions, pandemics)

---

### EPIC-13 · Timeline UI (Alpha)

- [ ] Implement visual timeline bar — horizontal axis with era markers and event pins
- [ ] Implement divergence indicator — visual marker showing where the modified timeline branches from baseline
- [ ] Implement era navigation — click an era boundary to jump to it (triggers fast-forward if needed)
- [ ] Implement state history scrubber — review past world states (read-only)

---

### EPIC-14 · Modding Tooling (Basic)

- [ ] Write modding documentation — how to create a What If scenario JSON
- [ ] Implement schema validator CLI tool — contributor can validate a scenario file before submitting
- [ ] Add 2–3 community starter scenarios as examples in `/Data/WhatIf/`

---

## Phase 3 — Beta

*LLM integration: natural language input and narrative generation.*

### EPIC-15 · LLM Interpreter

- [ ] Define interpreter system prompt — constraints, output schema, rejection criteria
- [ ] Implement LLM API client (local inference or self-hosted backend, configurable)
- [ ] Implement natural language → Action JSON pipeline
- [ ] Implement output validator — rejects any LLM response that fails action schema
- [ ] Implement fallback behavior — if LLM output is invalid after N retries, return structured error to player
- [ ] Write integration test — a set of natural language inputs maps to correct validated actions

---

### EPIC-16 · LLM Narrator

- [ ] Define narrator system prompt — tone, era-awareness, factual grounding in simulation state
- [ ] Implement state → narrative pipeline (tick summaries, chunk summaries, event descriptions)
- [ ] Implement in-world news article generator (triggered by significant events)
- [ ] Implement historical retrospective generator (triggered at era boundaries)
- [ ] Write quality test set — a set of world states with expected narrative characteristics

---

### EPIC-17 · LLM Infrastructure

- [ ] Implement configuration system for LLM backend (endpoint, model, auth)
- [ ] Implement request queue — prevents simultaneous LLM calls from blocking the simulation
- [ ] Implement graceful degradation — game remains fully playable if LLM is unavailable

---

## Phase 4 — Release

*Stability, mod support, and community tooling.*

### EPIC-18 · Mod Support & Community Tooling

- [ ] Implement mod loader — discovers and loads scenario files from a `/Mods` directory at runtime
- [ ] Implement mod conflict detector — warns when two loaded mods modify the same target
- [ ] Publish schema definitions and modding documentation as standalone wiki pages
- [ ] Create scenario submission guidelines and review checklist for maintainers

---

### EPIC-19 · Stability & Performance

- [ ] Profile simulation performance — identify bottlenecks in tick loop and chunk passes
- [ ] Implement simulation state compression for long-running sessions
- [ ] Add error boundary system — a subsystem crash must not corrupt global world state
- [ ] Conduct chaos testing — run simulation with extreme parameter values and verify graceful degradation

---

### EPIC-20 · Release Packaging

- [ ] Define minimum supported Unity version and document it
- [ ] Build and test Windows desktop release
- [ ] Build and test macOS desktop release
- [ ] Write player-facing documentation (getting started, how to mod)
- [ ] Tag `v1.0.0` release on GitHub

---

## Dependency Map

```
EPIC-00
  └── EPIC-01
        ├── EPIC-02
        │     ├── EPIC-03
        │     ├── EPIC-04
        │     ├── EPIC-05
        │     ├── EPIC-06
        │     └── EPIC-07 (requires 03+04+05+06)
        │           ├── EPIC-08
        │           ├── EPIC-09
        │           └── EPIC-10
        │                 └── EPIC-11 (MVP complete)
        │                       ├── EPIC-12
        │                       ├── EPIC-13
        │                       └── EPIC-14 (Alpha complete)
        │                             ├── EPIC-15
        │                             ├── EPIC-16
        │                             └── EPIC-17 (Beta complete)
        │                                   ├── EPIC-18
        │                                   ├── EPIC-19
        │                                   └── EPIC-20 (Release)
```
