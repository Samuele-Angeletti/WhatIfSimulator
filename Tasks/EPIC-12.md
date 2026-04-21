# EPIC-12 · 3D Globe — Core

**Phase:** Phase 1 — MVP

> The primary visual interface. Must be implemented before any visual layer work begins.

> **Status legend:** `[ ]` To do · `[~]` In progress · `[x]` Done

## Tasks

- [ ] Create sphere mesh with sufficient polygon density for low-poly aesthetic at all zoom levels
- [ ] Apply base land/ocean texture (low-poly, realistic style — not cartoon, not hyper-realistic)
- [ ] Implement free camera rotation (click and drag)
- [ ] Implement zoom (scroll wheel / pinch) with min/max bounds
- [ ] Implement pan at Level 2 and Level 3 zoom (drag across surface)
- [ ] Implement zoom threshold detection — determines which detail level is active
- [ ] Implement atmosphere rim glow (shader, visible at Level 1)
- [ ] Implement slow-rotating cloud layer (estetico, independent of simulation tick rate)
