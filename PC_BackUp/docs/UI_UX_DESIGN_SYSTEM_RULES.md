# Desktop UI/UX Design System Rules

## 1. Purpose and scope

This document defines reusable visual and interaction rules for professional desktop business tools. It applies to WinForms screens, custom controls, and designer-authored layouts. It is intentionally framework-neutral where possible, so it can be copied to another desktop project.

The goal is calm, readable, task-oriented software. Visual polish must make important work easier to understand; decoration is not a goal by itself.

## 2. Design principles

1. **Content before navigation** — The work area, not the sidebar, is the visual focus.
2. **One task, one hierarchy** — Each screen presents a page title, a short description, the primary task, and supporting information in that order.
3. **Calm, not cute** — Use restrained rounding, subtle borders, and low-contrast neutral surfaces. Avoid excessive colour blocks, oversized icons, gradients, and decorative illustrations.
4. **Meaningful colour only** — Accent colour denotes the primary action or current selection. Status colours denote a specific state, never decoration.
5. **Predictable states** — Loading, empty, success, warning, error, disabled, hover, focus, and pressed states must be deliberately designed.

## 3. Visual tokens

Keep tokens in one shared theme class or resource. Individual screens must not introduce arbitrary near-duplicate values.

| Token group | Rule |
| --- | --- |
| Canvas | Use a very light neutral gray or blue-gray canvas behind cards; do not use pure-white canvas and pure-white cards together without a visible border or tonal separation. |
| Surface | Use white or near-white for cards and form surfaces. |
| Text | Use one near-black primary text token, one muted secondary text token, and one disabled text token. Body text and metadata must not use the same contrast. |
| Primary | Reserve one accessible blue for the primary action, current selection, and keyboard focus. Do not use it as a page background. |
| Status | Green = complete/safe, amber = attention, red = destructive/error, blue = informational. Pair every status colour with text or an icon; colour alone is insufficient. |
| Borders | Use a single low-contrast 1 px border token for default cards and inputs. Elevation, if used, must be subtle and never replace focus indication. |
| Radius | Cards: 10–12 px. Buttons/inputs: 8 px. Do not use pill corners except for compact tags. |
| Spacing | Base unit: 4 px. Use 8 / 12 / 16 / 24 / 32 px increments. Do not mix arbitrary gaps in the same screen. |

### Accessibility baseline

- Normal-sized text must maintain at least 4.5:1 contrast against its background.
- Interactive controls need a visible keyboard focus treatment independent of hover and selection.
- Never communicate a state using colour alone.
- Keep button labels verb-led and unambiguous; avoid icon-only destructive actions unless a tooltip and confirmation are present.

## 4. Page layout rules

Every top-level page uses this sequence:

1. Header: title plus one-line task description.
2. Optional summary row: up to four equal semantic summary cards.
3. Main content: a primary card/list that consumes the most width and a secondary contextual card only where it supports the primary task.
4. Action area: primary action at the right, secondary/cancel action immediately to its left.
5. Bottom breathing room: reserve at least 24 px after the final content or action group. Scrollable pages need the same bottom padding inside the scroll container.

Do not place unrelated actions inside the page header. Do not create cards solely to surround a single simple label.

## 5. Reusable components

Create each component as a toolbox-visible custom WinForms control with designer-editable properties. A component must document its purpose, variants, required states, public properties, and designer usage.

| Component | Required variants and rules |
| --- | --- |
| `AppPageHeader` | Title, description, optional contextual action. Fixed vertical rhythm across all pages. |
| `AppCard` | Default / emphasized / status variants. Provides background, border, radius, internal padding, and no business logic. |
| `StatCard` | Icon, label, value, optional trend/status. Do not use it for buttons. |
| `AppButton` | Primary, secondary, danger, quiet. Includes normal, hover, pressed, disabled, and keyboard-focus states. |
| `AppField` | Label, optional helper text, input, validation message. Labels must remain visible when fields contain text. |
| `AppList` / `AppDataGrid` | Standard header, row height, selected state, empty state, and loading state. |
| `EmptyStatePanel` | Contextual icon, clear title, one-line explanation, and one valid next action when applicable. |
| `SkeletonPanel` | Layout-shaped placeholders with fixed geometry; no animated high-contrast shimmer. |
| `StatusBanner` | Info, success, warning, error. Use for page-level feedback, not transient micro-events. |

### Component documentation template

Each custom control gets a sibling Markdown document containing:

- Intent and allowed contexts
- Screenshot or visual sketch
- Public designer properties with defaults
- Supported states and state-transition rules
- Accessibility and keyboard behavior
- One designer placement example
- Explicit non-goals

## 6. Lists, cards, and forms

- Use a card for grouped, scan-friendly information; use a list/grid for repeatable records. Do not render a large record set as individual floating cards.
- Keep list headers visually attached to rows. Use consistent row height, 12–16 px horizontal cell padding, and a single selected-row treatment.
- Align labels and values to a common grid. For desktop forms, use a stable label column rather than individually positioned label widths.
- Place helper text directly beneath the associated field, not in a distant status bar.
- Destructive row actions must be separated from routine actions and must expose their meaning in text or tooltip.

## 7. Interaction and state rules

### Loading

- Preserve the page shell and layout while data loads; do not replace it with a blank panel.
- Show skeletons matching the eventual card or list geometry after a short delay, so fast loads do not flash.
- Disable only controls whose action depends on the missing data. Keep navigation usable.
- Switch from skeleton to content in one layout pass using double buffering and layout suspension; avoid per-control repaint flicker.

### Empty state

- State what is absent, why it matters, and the next step.
- Example: “아직 백업 이력이 없습니다. 첫 백업을 만들면 이곳에서 복원과 비교를 할 수 있습니다.”
- When the user can resolve it on the page, offer one primary action such as “새 백업 만들기”.

### Feedback and errors

- Reflect a completed action near the action that caused it, then optionally summarize it in a status banner.
- Validation errors appear next to the relevant input and explain how to resolve them.
- Confirmation is required for irreversible actions; do not use a warning colour as the only safeguard.

## 8. Implementation and review rules

- Centralize colours, typography, dimensions, radii, and control defaults in one theme source.
- Reuse custom controls rather than duplicating painted borders, rounded regions, button state logic, or empty states in individual pages.
- Keep visual controls free of backup/recovery business logic. Let a screen compose controls and bind view data.
- All reusable controls must remain editable from the Visual Studio designer. Expose meaningful properties with `Category`, `DefaultValue`, and `Description` metadata where appropriate.
- Review each changed screen at minimum window size, normal size, and a dense-data/empty-data/loading-data state.
- Test keyboard tab order, focus visibility, 125%/150% Windows text scaling, and Korean text expansion before acceptance.

## 9. Definition of done for a screen refresh

- The page header, spacing rhythm, card treatment, typography, buttons, and status colours use shared components/tokens.
- The primary user task is visually obvious within three seconds.
- Loading and empty states are implemented and do not cause a blank or flickering content region.
- A minimum 24 px bottom breathing area remains at the final scroll position.
- Designer placement and component documentation are updated for every new reusable control.

