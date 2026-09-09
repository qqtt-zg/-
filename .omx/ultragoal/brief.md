# AntdUI interaction migration

## Stories

### 1. Validate hard UI parity gates
Prove AntdUI 2.4.8 can preserve required Modal DialogResult/default-button semantics and define a parity-safe approach for FileRename persistent column checklist; block affected migration if either proof fails.

### 2. Add thin interaction contracts and renderers
Add request models and thin AntdUI menu/modal renderers. Callers must retain business commands, target selection, dynamic state, keyboard handling, and explicit owner/UI-thread behavior.

### 3. Add a global AntdUI theme bridge
Synchronize existing ThemeDefinition colors at the non-recursive theme entry point. Validate deep, light, eye-care green, and classic blue without per-popup global mutations.

### 4. Migrate simple popup menu hosts
Migrate the in-app main more menu, PDF tab menu, and PDF viewer menu while retaining native tray menu and existing commands.

### 5. Migrate grid and file-renaming menus
Migrate DataGridView, DgvContextMenu, FileRename, and database menus while preserving targets, shortcuts, checked state, nested structure, and the approved persistent column checklist behavior.

### 6. Migrate dynamic and nonstandard menus
Migrate MaterialSelect, batch workbench, event-group tree, and floating drop-zone menus while preserving dynamic state, keyboard invocation, top-most handling, and persistence.

### 7. Migrate short decision dialogs
Migrate short business confirmation dialogs through owner-bound Modal requests with exact existing result semantics. Leave input dialogs, file pickers, progress/complex forms, one-button alerts, and update workflow unchanged.

### 8. Verify and review the complete migration
Run targeted tests, four-theme and DPI interaction matrix, full build/tests, cleanup, independent code review, and architecture invariant audit. Preserve all unrelated working-tree changes.
