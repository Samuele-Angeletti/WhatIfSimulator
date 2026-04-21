# LLM

`WhatIfSimulator/Assets/LLM` is reserved for future interpreter and narrator integration.

This folder will eventually contain:

- Natural language input interpretation
- Narrative generation
- Validation layers around LLM-facing DTOs

The simulation engine remains the sole authority over simulation outcomes. Code in this folder must interpret or narrate simulation state, never replace deterministic simulation logic.
