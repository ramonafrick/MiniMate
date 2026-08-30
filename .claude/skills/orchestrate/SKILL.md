---
name: orchestrate
description: >
  Full development workflow for a feature, run end-to-end in 8 gated phases:
  Clarify (grill-me) → Plan (Opus, vertical slices) → Implement (Sonnet, per
  slice, code+tests) → Build (Haiku, loop to green) → Review (Sonnet,
  quality+security, severity-picked fixes) → Document → Update project
  long-term memory (CLAUDE.md/skills) → Audit those files for size/SRP.
  Human approval gates after Clarify and after Plan. Invoke with
  /orchestrate [feature description].
disable-model-invocation: true
---

# Orchestrator: Full Development Workflow

Aufgabe/Feature: $ARGUMENTS

Führe die 8 Phasen strikt in Reihenfolge aus. Jede Phase, die an einen Subagent delegiert wird, läuft in isoliertem Kontext — das schützt den Haupt-Kontext und garantiert das dort festgelegte Modell unabhängig vom Sitzungsmodell. Phasen, die *nicht* delegiert werden (1, 7, 8, alle HITL-Gates), laufen im aktuellen Sitzungsmodell.

| Phase | Name | Modell | Wie |
|---|---|---|---|
| 1 | Clarify | Sitzungsmodell | `grill-me` Skill, inline (braucht AskUserQuestion) |
| — | **HITL-Gate** | — | AskUserQuestion: requirements.md freigeben? |
| 2 | Plan | **Opus** | Agent `Plan`, `model: opus` |
| — | **HITL-Gate** | — | AskUserQuestion: plan.md freigeben, welcher Slice zuerst? |
| 3+4 | Implement + Build (pro Slice, Loop) | Sonnet / Haiku | Agent `implementer` → Agent `build-fixer` |
| 5 | Review | Sonnet | Agent `code-reviewer` + Agent `security-reviewer` parallel |
| 6 | Documentation | Sonnet | Agent `docs-writer` |
| 7 | Long-Term Memory | Sitzungsmodell | inline, aktualisiert Projekt-`.claude/` |
| 8 | Size/SRP-Audit | Sitzungsmodell | inline |

Vor dem Start: Frage kurz, ob das Feature für MAUI, Blazor Web oder beide Plattformen gilt, und welches Modul betroffen ist (aus der Projektstruktur ableiten wenn möglich).

---

## PHASE 1: Clarify

Folge dem `grill-me`-Skill für `$ARGUMENTS` (7 Bereiche, `AskUserQuestion`, max. 2 Fragen pro Runde). Ergebnis: `docs/requirements.md`.

**STOPP** — Zeige `docs/requirements.md`, frage per `AskUserQuestion` explizit: freigeben und weiter zu Phase 2, oder Änderungen? Ohne explizite Freigabe nicht weitermachen.

---

## PHASE 2: Plan (Vertical Slices)

Dispatch: Agent-Tool, `subagent_type: "Plan"`, `model: "opus"`. Gib `docs/requirements.md` mit.

Was ein guter Slice ist: schneidet durch alle betroffenen Schichten (Domain → Application → Infrastructure → UI), end-to-end testbar ohne den nächsten Slice, passt in eine Session (max. ~3 neue Dateien).

Ergebnis: `docs/plan.md` — Architektur-Überblick, pro Slice (Name, Ziel, Dateien mit Layer-Zuordnung, Tests, Akzeptanz, Komplexität), Build-Befehle, Abhängigkeiten zwischen Slices.

**STOPP** — Zeige die Slice-Tabelle, frage per `AskUserQuestion`: freigeben, und welcher Slice zuerst?

---

## PHASE 3+4: Implement → Build (Loop pro Slice)

Für jeden Slice, in der freigegebenen Reihenfolge:

1. **Implement**: Agent-Tool, `subagent_type: "implementer"`. Übergib die Slice-Definition aus plan.md, den relevanten Abschnitt von requirements.md, und die betroffenen Dateien.
2. **Build**: Agent-Tool, `subagent_type: "build-fixer"`, mit den geänderten Dateien. Loopt intern bis grün oder 3 Versuche.
3. Wenn build-fixer nach 3 Versuchen immer noch rot meldet: **STOPP**, zeige den Fehler, frage die Nutzerin — nicht selbst weiterraten.
4. Commit: `git add [geänderte Dateien] && git commit -m "feat([slice]): [was der Slice liefert]"`
5. Kurzer Kontext-Reset: notiere "Slice X abgeschlossen, nächster: Y", dann weiter.

Erst wenn alle Slices durch sind: weiter zu Phase 5.

---

## PHASE 5: Review (Quality + Security, max. 3 Runden)

Runde 1 von maximal 3:

1. Dispatch parallel: Agent `code-reviewer` (Simplification/Konsistenz) und Agent `security-reviewer` (Security/SAST) auf alle in Phase 3+4 geänderten Dateien.
2. Merge beide Findings-Listen in eine, sortiert nach Schwere ([MUST]/🔴 zuerst).
3. `AskUserQuestion` (multiSelect): liste jedes Finding als Option (Schwere + Kurzbeschreibung), Nutzerin wählt aus, welche jetzt umgesetzt werden.
4. Setze die ausgewählten Findings um.
5. Zurück zu **Phase 4** (build-fixer) auf die geänderten Dateien.
6. Wenn nach dieser Runde noch offene Findings da sind: weiter zu Phase 6, notiere sie als bekannte Einschränkung.
7. Weitere Runden möglich, aber **nie mehr als 3 insgesamt**.

---

## PHASE 6: Documentation

Dispatch: Agent `docs-writer` mit `docs/plan.md`, der Liste aller implementierten Dateien, und allen aus Phase 5 unadressierten Findings.

---

## PHASE 7: Long-Term Memory (Projekt, nicht persönliche Cross-Session-Memory)

Das hier ist das Projekt-`.claude/` (CLAUDE.md + Projekt-Skills), nicht das persönliche Memory-System unter `~/.claude/projects/.../memory/`.

1. Vergleiche, was in dieser Feature-Arbeit gelernt/verändert wurde, mit dem aktuellen Projekt-CLAUDE.md und vorhandenen Projekt-Skills (`.claude/skills/*`).
2. Nur aktualisieren, was sich tatsächlich geändert oder erweitert hat: neue Gotchas, neue Konventionen, neue Komponenten/Dateien, geänderte Commands.
3. Nichts hinzufügen, was schon aus Code/Git-History ableitbar ist.

---

## PHASE 8: Size/SRP-Audit

Für jede in Phase 7 berührte oder neu erstellte Datei unter `.claude/`:

- **SKILL.md-Body**: Ziel < 500 Zeilen. Über dem Limit → in `references/*.md` aufteilen, aus SKILL.md eine Ebene tief verlinken, Referenzdateien >100 Zeilen bekommen ein Inhaltsverzeichnis.
- **CLAUDE.md**: Zielwert ~200 Zeilen, Obergrenze ~300.
- SRP-Check: enthält eine Datei mehrere unabhängige Themen? → nach Domäne aufteilen.

---

## Regeln

- Niemals einen Slice beginnen, wenn der Build vorheriger Arbeit noch rot ist.
- Review (Phase 5) und Tests (Teil von Phase 3) nie überspringen.
- HITL-Gates sind turn-basiert — nach dem Stopp endet die Antwort, es geht erst in einer neuen Nutzer-Nachricht weiter. Schweigen oder Themenwechsel zählt nicht als Freigabe.
- Nach 2 gescheiterten Korrekturversuchen am selben Problem: nicht weiter raten, Nutzerin einbeziehen.
- Subagenten für Plan/Implement/Build/Review/Docs: schützt den Haupt-Kontext und garantiert das festgelegte Modell.
