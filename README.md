# What If Simulator

> *A platform for asking questions to reality - and observing the consequences.*

**What If Simulator** is an open-source, simulation-driven game that lets players explore alternate histories by modifying key events in the timeline of the universe.

The core question is simple but powerful:

> *What happens if a fundamental event in history never occurred?*

The simulation can start from any configurable point in time, with the default starting point set before the dinosaur extinction. It evolves autonomously based on deterministic rules. Players intervene through constrained, high-level actions that influence, but never directly control, the world's evolution.

---

## Core Philosophy

### 1. Simulation First

The simulation engine is the single authoritative source of truth. No external system, including LLMs, is allowed to directly alter simulation logic or outcomes.

### 2. Player as Influence, Not Controller

Players do not micromanage entities. Instead, they act as an external force applying macro-level pressure on the world's systems.

### 3. Emergent Timeline

History is never scripted. It emerges from the interaction of four core systems: population, economy, technology, and conflict. Small changes can cascade into radically different outcomes.

### 4. LLM as Interpreter and Narrator

LLMs serve two well-defined roles: translating player intent into valid simulation actions, and generating narrative output from simulation state. They never compute simulation outcomes.

---

## Key Features

- Fully simulated world evolution from configurable starting points
- Modular, data-driven "What If" scenario system
- Natural language player input via LLM interpretation
- Dynamic narrative generation for summaries, articles, and historical events
- Temporal fast-forward across large spans of time with divergence resolution
- Deterministic, reproducible simulation core with seeded randomness
- Open-source structure designed for community extension

---

## Architecture

```text
[Player Input]
      |
[LLM Interpreter]        - translates natural language into structured actions
      |
[Action Validator]       - ensures the action is legal and within constraints
      |
[Simulation Engine]      - applies action, advances world state
      |
[World State Update]     - updates all subsystems
      |
[LLM Narrator]           - generates human-readable output from new state
      |
[Player Output]
```

---

## Simulation Systems

The world is modeled as a set of independent but interacting subsystems. Each one operates on deterministic rules, consumes shared state, and produces updated state every simulation tick.

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

The world is represented as a continuous timeline navigated through **temporal chunks** - discrete intervals of simulated time that the engine processes as a unit. This allows the player to fast-forward across vast stretches of history without simulating every intermediate tick in real time.

### Temporal Chunking and Fast-Forward

Rather than advancing tick by tick, the player can jump forward in time by selecting a chunk size. The engine then:

1. Computes the compressed simulation for the skipped interval.
2. Resolves divergence paths caused by active What If modifications.
3. Generates significant events for the skipped period.
4. Emits a summary of the chunk so the player understands what happened.

The engine therefore operates at two temporal resolutions:

| Mode | Tick Granularity | Use Case |
| --- | --- | --- |
| Real-time | Fine, such as years or decades | Active player interaction |
| Fast-forward | Coarse, such as millennia or eons | Skipping to a new era |

Chunk boundaries are configurable per scenario and can be exposed to the player as selectable eras.

### Divergence Resolution

When fast-forwarding through a modified timeline, the engine determines how What If changes compound over the skipped interval. For each chunk, it evaluates which baseline events still occur, which are suppressed or altered, and what emergent events the modified trajectory produces instead.

Divergence resolution is deterministic. Given the same seed, starting state, and active modifications, the same fast-forward must always produce the same outcome.

### Event Structure

All simulation events, whether generated in real-time or during a fast-forward chunk, share the same canonical shape:

```json
{
  "schemaVersion": "1.0.0",
  "timestamp": -65000000,
  "eventType": "extinction_event",
  "description": "Mass extinction triggered by asteroid impact.",
  "chunkGenerated": false,
  "impactEntries": [
    {
      "key": "population",
      "value": -0.9
    },
    {
      "key": "biodiversity",
      "value": -0.95
    }
  ]
}
```

The `chunkGenerated` flag indicates whether the event was resolved during a fast-forward pass or during active real-time simulation. Canonical JSON examples use explicit entry arrays such as `impactEntries` so they remain compatible with Unity `JsonUtility`.

---

## What If System

"What If" scenarios are defined as self-contained, data-driven JSON modules. No code is required for basic scenarios.

### Example Scenario

```json
{
  "schemaVersion": "1.0.0",
  "id": "no_dinosaur_extinction",
  "name": "The comet never arrived",
  "description": "The Cretaceous-Paleogene mass extinction event does not occur.",
  "modifications": [
    {
      "modificationType": "remove_event",
      "removeEvent": {
        "targetEventId": "dinosaur_extinction"
      }
    }
  ]
}
```

### Supported Modification Types

| Type | Description |
| --- | --- |
| `remove_event` | Prevents a specific event from firing |
| `add_event` | Injects a new event into the timeline |
| `modify_parameter` | Alters a system parameter such as a growth rate |
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
  "schemaVersion": "1.0.0",
  "action": "boost_technology",
  "target": "electricity",
  "region": "global",
  "intensity": 0.6
}
```

`intensity` is normalized from `0.0` to `1.0`.

---

## LLM Integration

LLMs operate strictly within two defined roles and never influence simulation logic directly.

### Role 1 - Input Interpreter

Transforms natural language player input into validated simulation actions.

Input:

```text
I want humanity to discover electricity earlier
```

Output:

```json
{
  "schemaVersion": "1.0.0",
  "action": "boost_technology",
  "target": "electricity",
  "region": "global",
  "intensity": 0.6
}
```

All LLM output is validated against the action schema before being passed to the simulation engine. Invalid or out-of-bounds actions are rejected.

### Role 2 - Narrative Generator

Generates human-readable output from structured simulation state: timeline summaries, in-world news articles, historical retrospectives, and contextual explanations.

Example output:

> *"In the early 19th century, accelerated research led to the premature discovery of electrical systems, reshaping industrial development across multiple regions."*

### Deployment Phases

| Phase | LLM Mode | Capabilities |
| --- | --- | --- |
| MVP | None | Predefined actions only |
| Alpha | Local, such as `llama.cpp` | Basic input interpretation |
| Beta | Self-hosted backend | Full interpretation and narration |
| Release | Self-hosted backend | Full integration and mod support |

---

## Technology Stack

| Layer | Technology |
| --- | --- |
| Engine | Unity, desktop-first, with C# |
| Data | JSON config files and ScriptableObjects |
| LLM, optional | Local inference or self-hosted backend |

---

## Development Environment

The project targets **Unity** as its sole development environment. All simulation logic, data handling, and UI are implemented in C# within Unity.

### Approved Libraries

Contributors may freely use anything available in a standard Unity project without additional setup:

- `UnityEngine` and its standard namespaces
- Unity's built-in `JsonUtility`
- The C# standard library
- Unity packages bundled by default in the target Unity version

### External Library Policy

Any library that requires explicit installation, via the Unity Package Manager, manual DLL inclusion, or any other mechanism outside a standard Unity project, must be approved before use.

To request approval for an external library, open an issue with:

1. Library name and version
2. Link to source or package registry
3. Reason it is needed and what Unity-native alternative was considered
4. License

No pull request that introduces an unapproved external dependency will be merged.

---

## Project Structure

This repository has two layers:

- Repo-root documentation and workflow files such as `README.md`, `TASKS.md`, `Tasks/`, `.github/`, and `.kairos/`
- The Unity project under `WhatIfSimulator/`

Canonical implementation folders live inside the Unity project:

```text
WhatIfSimulator/
    Assets/
        Core/
            SimulationEngine.cs
            Systems/
                Population/
                Economy/
                Technology/
                Conflict/
        Data/
            Contracts/
            WhatIf/
            Events/
            Actions/
        LLM/
            Interpreter/
            Narrator/
        UI/
            TimelineView/
            EventFeed/
```

`WhatIfSimulator/Assets/Data` is the canonical contract surface for example JSON, schema specs, and future loaders.
The canonical implementation roots are `WhatIfSimulator/Assets/Core`, `WhatIfSimulator/Assets/Data`, `WhatIfSimulator/Assets/LLM`, and `WhatIfSimulator/Assets/UI`.

---

## Determinism and Reproducibility

Reproducibility is a first-class requirement. Every simulation run must be fully deterministic given the same seed and starting conditions.

Requirements:

- Fixed simulation timestep
- All randomness seeded and logged
- Full state transitions logged at each tick
- Any given seed and scenario must produce identical output across runs

---

## Modding Guidelines

Community contributors can create What If scenarios as JSON, define new events, and extend subsystems in advanced cases. All contributions must:

- Preserve deterministic simulation behavior
- Pass data validation checks
- Avoid direct changes to simulation engine core logic unless the work explicitly targets that layer

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

- LLM integration, interpreter and narrator
- Dynamic narrative output

### Release

- Full mod support
- Community scenario tooling
- Scalability improvements

---

## Known Design Challenges

- Complexity versus accessibility
- Emergent coherence across interacting systems
- LLM output validation
- Chaos prevention when stacking powerful scenarios
- Dual-resolution consistency between coarse and fine simulation modes
- Divergence legibility when explaining why a modified timeline looks the way it does

---

## License

Open-source, license TBD.

## Contributing

Contributions are welcome. Priority areas:

- Simulation subsystem design
- Data and event schema design
- Modding tools and documentation
- LLM integration, interpreter and narrator

See the modding guidelines above before submitting a scenario or system extension.
See `CONTRIBUTING.md` for branching, review expectations, canonical folder paths, and the external library approval workflow.
