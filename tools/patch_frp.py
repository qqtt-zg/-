import os

frp_path = os.path.abspath("src/WindowsFormsApp3/Forms/Panels/FileRenamePanel.cs")
with open(frp_path, "r", encoding="utf-8") as f:
    content = f.read()

target = """result = new MaterialSelectionResult
                        {
                            SelectedMaterial = dialog.SelectedMaterial,"""

replacement = """result = new MaterialSelectionResult
                        {
                            IsApplyToAll = dialog.IsApplyToAll,
                            OrderNumberMode = dialog.CurrentOrderNumberMode,
                            SelectedOrderRegexName = dialog.SelectedOrderRegexName ?? "",
                            SelectedOrderRegexPattern = dialog.SelectedOrderRegexPattern ?? "",
                            BatchItems = dialog.BatchFileItems ?? new List<BatchFileItem>(),
                            SelectedMaterial = dialog.SelectedMaterial,"""

normalized_target = target.replace("\r\n", "\n")
normalized_content = content.replace("\r\n", "\n")

if normalized_target in normalized_content:
    normalized_content = normalized_content.replace(normalized_target, replacement.replace("\r\n", "\n"), 1)
    final_content = normalized_content.replace("\n", "\r\n")
    with open(frp_path, "w", encoding="utf-8") as f:
        f.write(final_content)
    print("FileRenamePanel.cs patched successfully!")
else:
    print("target not found in FileRenamePanel.cs")
