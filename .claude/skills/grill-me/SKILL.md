---
name: grill-me
description: Interviews the user relentlessly about a feature, plan, or design until requirements are unambiguous, then writes them to requirements.md. Use when the user says "grill me", asks to clarify/stress-test a plan or idea before building it, or when a feature request is vague enough that building it now would mean guessing. Runs inline in the main conversation, not as a delegated subagent, because it needs AskUserQuestion.
---

# Grill Me

Interview the user about `$ARGUMENTS` (or the feature/plan under discussion) until every branch of the decision tree is resolved. Don't accept vague answers — if a reply is still ambiguous, ask a sharper follow-up instead of moving on.

## Why this runs here, not in a subagent

Subagents (via the Agent tool) can't call `AskUserQuestion` and can't pause mid-task to wait for a reply — they run to completion and hand back a final report. An interview only works turn-by-turn in the main conversation, so this skill's instructions are for you (the main agent) to follow directly.

## Process

1. Ask with `AskUserQuestion`, max 2 questions per round, until all 7 areas below are covered. Skip an area only if the answer is already obvious from context — don't ask what you already know.
2. Push back on hand-wavy answers ("it should be fast" → ask for a number or a comparison; "handle errors gracefully" → ask which errors, and what "gracefully" means here).
3. When all 7 areas are resolved, write `docs/requirements.md` (create `docs/` if missing) with the structure below.
4. Show the user the finished file and ask for explicit approval before anything downstream (planning, implementation) proceeds. Do not treat silence or a topic change as approval.

## The 7 areas

1. **Problem & motivation** — what's being solved, why now, what happens if it isn't
2. **Users & context** — who uses it, how often, in what environment (MAUI mobile, Blazor Web, or both?)
3. **Inputs & outputs** — the main path: what goes in, what comes out
4. **Technical constraints** — which layer owns this (Domain/Application/Infrastructure/UI), which module, target platform(s); does it need a new module or extend an existing one? If new module: does it depend on an existing module (Location/Weather/Clothing/…)?
5. **Success criteria** — how "done" is recognized, ideally measurable
6. **Edge cases & failure modes** — what must never happen; what's allowed to fail loudly vs. silently
7. **Out of scope** — what this explicitly does not cover (as important as what it does)

## requirements.md structure

```markdown
# Requirements: [Feature name]

**Date**: [YYYY-MM-DD]

## Problem
[what's being solved and why]

## Users & Context
[who, how often, environment — MAUI/Blazor/both]

## Functional Requirements
- FR1: ...
- FR2: ...

## Non-Functional Requirements
- NFR1: ...

## Technical Constraints
[layer ownership, module, target platform, dependencies]

## Acceptance Criteria
- [ ] ...
- [ ] ...

## Edge Cases & Failure Modes
- [case] → [expected behavior]

## Out of Scope
- ...

## Open Questions
- [anything still unresolved, if the user chose to defer it]
```

## Rules

- Don't write requirements.md until the interview is actually done — a half-answered document is worse than an incomplete one you keep asking about.
- If the user explicitly defers a question ("decide that yourself" / "doesn't matter"), record your assumption under **Open Questions** instead of silently picking one.
- One round of questions can carry follow-ups from the previous answer — you don't need all 7 areas resolved in a fixed number of rounds, just resolved before writing the file.
