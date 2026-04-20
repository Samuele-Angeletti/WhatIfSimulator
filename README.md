# What If Simulator

> *A platform for asking questions to reality — and observing the consequences.*

**What If Simulator** is an open-source, simulation-driven game that lets players explore alternate histories by modifying key events in the timeline of the universe.

The core question is simple but powerful:

> *What happens if a fundamental event in history never occurred?*

The simulation can start from any configurable point in time (default: pre-dinosaur extinction) and evolves autonomously based on deterministic rules. Players intervene through constrained, high-level actions that influence — but never directly control — the world's evolution.

---

## Core Philosophy

### 1. Simulation First
The simulation engine is the single authoritative source of truth. No external system — including LLMs — is allowed to directly alter simulation logic or outcomes.

### 2. Player as Influence, Not Controller
Players don't micromanage entities. Instead, they act as an external force applying macro-level pressure on the world's systems.

### 3. Emergent Timeline
History is never scripted. It emerges from the interaction of four core systems: population, economy, technology, and conflict. Small changes can cascade into radically different outcomes.

### 4. LLM as Interpreter and Narrator
LLMs serve two well-defined roles: translating player intent into valid simulation actions, and generating narrative output from simulation state. They never compute simulation outcomes.

---

## Key Features

- 🌍 Fully simulated world evolution from configurable starting points
- 🧩 Modular, data-driven "What If" scenario system (no code required for basic mods)
- 🧠 Natural language player input via LLM interpretation
- 📖 Dynamic narrative generation — timeline summaries, news articles, historical events
- ⏩ Temporal fast-forward system — jump across eons in chunks, with automatic divergence resolution and event generation for the skipped interval
- ⚙️ Deterministic, reproducible simulation core with seeded randomness
- 🔌 Open-source and designed for community extension

---

## Architecture

```
[Player Input]
      ↓
[LLM Interpreter]        — translates natural language into structured actions
      ↓
[Action Validator]       — ensures the action is legal and within constraints
      ↓
[Simulation Engine]      — applies action, advances world state
      ↓
[World State Update]     — updates all subsystems
      ↓
[LLM Narrator]           — generates human-readable output from new state
      ↓
[Player Output]
```

---

## Simulation Systems

The world is modelled as a set of independent but interacting subsystems. Each one operates on deterministic rules, consumes shared state, and produces updated state every simulation tick.

### Population
Handles growth and decline rates, migration patterns, and demographic shifts across regions.

### Economy
Models resource production, trade flows between regions, and scarcity-driven effects on other systems.

### Technology
Tracks research progression, diffusion of innovations across regions, and the compounding effects of technological acceleration.

### Conflict
Simulates wars, alliance formation, political instability, and their feedback loops with population and economy.

---

## Timeline Model

The world is represented as a continuous timeline navigated through **temporal chunks** — discrete intervals of simulated time that the engine processes as a unit. This allows the player to fast-forward across vast stretches of history (e.g. from the Cretaceous to the first human civilizations) without simulating every intermediate tick in real time.

### Temporal Chunking & Fast-Forward

Rather than advancing tick by tick, the player can jump forward in time by selecting a chunk size. The engine then:

1. **Computes the compressed simulation** for the entire skipped interval, applying system rules at a coarser resolution appropriate to the chunk size.
2. **Resolves divergence paths** — any What If modifications active during the chunk are propagated forward, branching the timeline from the baseline where relevant.
3. **Generates significant events** that would plausibly have occurred during the skipped period, based on the world state at the chunk's start and the active simulation parameters.
4. **Emits a summary** of the chunk — key events, state deltas, and narrative output — so the player understands what happened during the fast-forward.

This means the engine must operate at **two temporal resolutions**:

| Mode | Tick Granularity | Use Case |
|------|-----------------|----------|
| **Real-time** | Fine (years / decades) | Active player interaction |
| **Fast-forward** | Coarse (millennia / eons) | Skipping to a new era |

**Example jump:** from −65,000,000 (pre-extinction) to −10,000 (early Holocene, first human settlements). The engine skips ~65 million years, calculates the macro-level consequences of any active What If modifications across that span, and surfaces the civilizational starting state the player will interact with next.

Chunk boundaries are configurable per scenario and can be exposed to the player as selectable "eras" to jump to.

### Divergence Resolution

When fast-forwarding through a modified timeline, the engine must determine how the What If changes compound over the skipped interval. This is handled by **divergence resolution**: for each chunk, the engine evaluates which baseline events would still occur, which are suppressed or altered by active modifications, and what emergent events the modified trajectory would produce instead.

Divergence resolution is deterministic — given the same seed, starting state, and active modifications, the same fast-forward will always produce the same outcome.

### Event Structure

All simulation events — whether generated in real-time or during a fast-forward chunk — share the same structure:

```json
{
  "timestamp": -65000000,
  "event_type": "extinction_event",
  "description": "Mass extinction triggered by asteroid impact.",
  "chunk_generated": false,
  "impact": {
    "population": -0.9,
    "biodiversity": -0.95
  }
}
```

The `chunk_generated` flag indicates whether the event was resolved during a fast-forward pass (coarse resolution) or during active real-time simulation (fine resolution).

---

## What If System

"What If" scenarios are defined as self-contained, data-driven JSON modules. No code is required for basic scenarios.

### Example Scenario

```json
{
  "id": "no_dinosaur_extinction",
  "name": "The comet never arrived",
  "description": "The Cretaceous–Paleogene mass extinction event does not occur.",
  "modifications": [
    {
      "type": "remove_event",
      "target": "dinosaur_extinction"
    }
  ]
}
```

### Supported Modification Types

| Type | Description |
|------|-------------|
| `remove_event` | Prevents a specific event from firing |
| `add_event` | Injects a new event into the timeline |
| `modify_parameter` | Alters a system parameter (e.g., growth rate) |
| `inject_rule` | Adds a new conditional rule to a subsystem |
| `alter_initial_conditions` | Changes the world state at the scenario start point |

Multiple What If modules can be composed together. All modifications must preserve deterministic behavior.

---

## Player Interaction

Players influence the simulation through constrained, high-level actions. Direct entity control is intentionally not available.

### Example Actions

- Boost technological development in a region
- Destabilize a political system
- Increase resource availability
- Accelerate or slow cultural diffusion

### Action Structure

```json
{
  "action": "boost_technology",
  "target": "electricity",
  "region": "global",
  "intensity": 0.6
}
```

`intensity` is normalized from `0.0` (minimal effect) to `1.0` (maximum allowed influence).

---

## LLM Integration

LLMs operate strictly within two defined roles and never influence simulation logic directly.

### Role 1 — Input Interpreter

Transforms natural language player input into validated simulation actions.

**Example:**

Input:
```
"I want humanity to discover electricity earlier"
```

Output:
```json
{
  "action": "boost_technology",
  "target": "electricity",
  "region": "global",
  "intensity": 0.6
}
```

All LLM output is validated against the action schema before being passed to the simulation engine. Invalid or out-of-bounds actions are rejected.

### Role 2 — Narrative Generator

Generates human-readable output from structured simulation state: timeline summaries, in-world news articles, historical retrospectives, and contextual explanations.

**Example output:**
> *"In the early 19th century, accelerated research led to the premature discovery of electrical systems, reshaping industrial development across multiple regions."*

### Deployment Phases

| Phase | LLM Mode | Capabilities |
|-------|----------|--------------|
| MVP | None | Predefined actions only |
| Alpha | Local (e.g., llama.cpp) | Basic input interpretation |
| Beta | Self-hosted backend | Full interpretation + narration |
| Release | Self-hosted backend | Full integration + mod support |

---

## Technology Stack

| Layer | Technology |
|-------|-----------|
| Engine | Unity (C#) — desktop first |
| Data | JSON config files + ScriptableObjects |
| LLM (optional) | Local inference (llama.cpp) or self-hosted backend |

---

## Development Environment

The project targets **Unity** as its sole development environment. All simulation logic, data handling, and UI are implemented in C# within Unity.

### Approved Libraries

Contributors may freely use anything available in a standard Unity project without additional setup:

- **UnityEngine** and all its standard namespaces (`UnityEngine.UI`, `UnityEngine.SceneManagement`, etc.)
- **Unity's built-in JSON utility** (`JsonUtility`) for basic serialization
- **C# standard library** (`System`, `System.Collections`, `System.Linq`, etc.)
- **Unity packages bundled by default** in the target Unity version (e.g. TextMeshPro, Input System, if included in the base project setup)

### External Library Policy

Any library that requires explicit installation — via the Unity Package Manager, manual `.dll` inclusion, or any other mechanism outside a standard Unity project — **must be approved before use**.

This includes, but is not limited to:

| Example Library | Reason Approval Is Required |
|----------------|------------------------------|
| Newtonsoft Json (Json.NET) | Not included in Unity by default |
| UniTask | External async library |
| DOTween | External animation library |
| NaughtyAttributes | Editor tooling dependency |
| Any ML / inference SDK | Significant footprint and licensing implications |

**To request approval for an external library**, open an issue on the repository with the following information:

1. Library name and version
2. Link to source / Package Manager registry
3. Reason it is needed and what Unity-native alternative was considered
4. License

No pull request that introduces an unapproved external dependency will be merged, even if the rest of the contribution is otherwise valid.

---

## Project Structure

```
/Core
    SimulationEngine.cs
    Systems/
        Population/
        Economy/
        Technology/
        Conflict/

/Data
    WhatIf/          ← scenario definition files
    Events/          ← event templates
    Actions/         ← action schemas

/LLM
    Interpreter/     ← natural language → action
    Narrator/        ← state → narrative text

/UI
    TimelineView/
    EventFeed/
```

---

## Determinism & Reproducibility

Reproducibility is a first-class requirement. Every simulation run must be fully deterministic given the same seed and starting conditions.

**Requirements:**
- Fixed simulation timestep
- All randomness seeded and logged
- Full state transitions logged at each tick
- Any given seed + scenario must produce identical output across runs

---

## Modding Guidelines

Community contributors can create What If scenarios (JSON), define new events, and extend subsystems (advanced). All contributions must:

- Preserve deterministic simulation behavior
- Pass data validation checks
- Not modify simulation engine core logic directly

---

## Roadmap

### MVP
- Population and resource subsystems
- One built-in What If scenario
- Predefined player actions only

### Alpha
- Full four-system interaction
- Structured event generation
- Timeline UI

### Beta
- LLM integration (interpreter + narrator)
- Dynamic narrative output

### Release
- Full mod support
- Community scenario tooling
- Scalability improvements

---

## Known Design Challenges

- **Complexity vs. accessibility** — the simulation must be deep enough to be interesting, simple enough to be navigable.
- **Emergent coherence** — ensuring that freely interacting systems produce plausible, not absurd, histories.
- **LLM output validation** — preventing hallucinated or out-of-schema actions from reaching the simulation.
- **Chaos prevention** — stacking multiple high-intensity What If scenarios can produce unstable simulation states; composability rules need careful design.
- **Dual-resolution consistency** — the fast-forward (coarse) engine and the real-time (fine) engine must produce compatible world states; a chunk-resolved state must be a valid starting point for fine simulation.
- **Divergence legibility** — when fast-forwarding through a heavily modified timeline, surfacing *why* the resulting world looks the way it does (which modifications caused which outcomes) is a hard UX and data problem.

---

## License

Open-source — license TBD.

## Contributing

Contributions are welcome. Priority areas:

- Simulation subsystem design
- Data and event schema design
- Modding tools and documentation
- LLM integration (interpreter and narrator)

See the modding guidelines above before submitting a scenario or system extension.
