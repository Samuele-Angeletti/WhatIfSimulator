# KAIROS Framework

This project uses the KAIROS multi-agent development framework.
Agent definitions are in `agents/` (Markdown) and `.codex/agents/` (TOML for Codex).

Always follow the KAIROS workflow sequence:
pm_agent → architect_agent → implementer_agent → code_reviewer → test_verifier → release_planner

After each phase, present the output and wait for explicit approval (✅ / ✏️ / ⛔) before proceeding.
Save each approved output to `.kairos/0X-*.json`.

Always follow the `README.md` as knowledge of the project