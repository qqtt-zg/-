const fs = require('fs');
const path = require('path');

const presPath = path.resolve('src/WindowsFormsApp3/Presenters/FileRenamePanelPresenter.cs');
let content = fs.readFileSync(presPath, 'utf8');

// 1. 添加 CollectPendingBatchFiles 辅助方法
const helperMethod = `        /// <summary>
        /// 收集当前队列与监控同目录下的所有待处理文件
        /// </summary>
        private List<string> CollectPendingBatchFiles(string currentFilePath)
        {
            var pendingList = new List<string>();
            if (!string.IsNullOrEmpty(currentFilePath) && File.Exists(currentFilePath))
            {
                pendingList.Add(currentFilePath);
            }

            // 1. 从当前待处理队列中收集
            lock (_fileQueueLock)
            {
                foreach (var p in _pendingFiles)
                {
                    if (!pendingList.Contains(p, StringComparer.OrdinalIgnoreCase) && File.Exists(p))
                    {
                        pendingList.Add(p);
                    }
                }
            }

            // 2. 扫描当前文件所在目录下的所有其他待处理 PDF 文件（应对监控多文件并发到达场景）
            try
            {
                if (!string.IsNullOrEmpty(currentFilePath))
                {
                    string dir = Path.GetDirectoryName(currentFilePath);
                    if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                    {
                        var dirPdfs = Directory.GetFiles(dir, "*.pdf", SearchOption.TopDirectoryOnly);
                        foreach (var pdf in dirPdfs)
                        {
                            string name = Path.GetFileName(pdf);
                            if (name.StartsWith("~$") || name.StartsWith(".")) continue;
                            if (ShouldProcessFile(pdf) && !pendingList.Contains(pdf, StringComparer.OrdinalIgnoreCase))
                            {
                                pendingList.Add(pdf);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning($"[CollectPendingBatchFiles] 扫描监控同目录文件失败: {ex.Message}");
            }

            return pendingList;
        }
`;

// 检查是否已包含 CollectPendingBatchFiles
if (!content.includes('private List<string> CollectPendingBatchFiles')) {
    const insertPoint = 'private async Task ProcessNewFileAsync(string filePath)';
    content = content.replace(insertPoint, helperMethod + '\r\n        ' + insertPoint);
}

// 2. 替换 ProcessNewFileAsync 中的 ShowMaterialSelectionDialog 调用
const oldInvokeDialog = `                                // ✅ 修复：使用匹配正则结果（cmbRegex2）传递给对话框，用于Excel数据匹配
                                string matchingRegexResult = GetRegexResultForMatching(fileInfo);
                                dialogResult = _view.ShowMaterialSelectionDialog(
                                    materials: _materials,
                                    fileName: fileInfo.FullPath,  // ✅ 修复：传递完整路径用于PDF预览
                                    regexResult: matchingRegexResult ?? "",
                                    width: fileInfo.Width ?? "",
                                    height: fileInfo.Height ?? "",
                                    tetBleed: fileInfo.TetBleed ?? "",
                                    isColumnCombineMode: AppSettings.EnableColumnCombine,
                                    columnNames: GetExcelColumnNames(),
                                    columnItemsMap: GetExcelColumnItemsMap(),
                                    initialSerialNumber: GetNextSerialNumber(),
                                    enableSerialSearchResultToRegex: _excelImportService.EnableSerialSearchResultToRegex,
                                    serialSearchResultColumnIndex: _excelImportService.SerialSearchResultColumnIndex,
                                    out selectionResult
                                );`;

const newInvokeDialog = `                                // ✅ 修复：使用匹配正则结果（cmbRegex2）传递给对话框，用于Excel数据匹配
                                string matchingRegexResult = GetRegexResultForMatching(fileInfo);
                                var pendingList = CollectPendingBatchFiles(fileInfo.FullPath);
                                dialogResult = _view.ShowMaterialSelectionDialog(
                                    materials: _materials,
                                    fileName: fileInfo.FullPath,  // ✅ 修复：传递完整路径用于PDF预览
                                    regexResult: matchingRegexResult ?? "",
                                    width: fileInfo.Width ?? "",
                                    height: fileInfo.Height ?? "",
                                    tetBleed: fileInfo.TetBleed ?? "",
                                    isColumnCombineMode: AppSettings.EnableColumnCombine,
                                    columnNames: GetExcelColumnNames(),
                                    columnItemsMap: GetExcelColumnItemsMap(),
                                    initialSerialNumber: GetNextSerialNumber(),
                                    enableSerialSearchResultToRegex: _excelImportService.EnableSerialSearchResultToRegex,
                                    serialSearchResultColumnIndex: _excelImportService.SerialSearchResultColumnIndex,
                                    out selectionResult,
                                    pendingFilePaths: pendingList
                                );`;

content = content.replace(oldInvokeDialog, newInvokeDialog);

const oldElseDialog = `                            // ✅ 修复：使用匹配正则结果(cmbRegex2)传递给对话框，用于Excel数据匹配
                            string matchingRegexResult = GetRegexResultForMatching(fileInfo);
                            dialogResult = _view.ShowMaterialSelectionDialog(
                                materials: _materials,
                                fileName: fileInfo.FullPath,  // ✅ 修复：传递完整路径
                                regexResult: matchingRegexResult ?? "",
                                width: fileInfo.Width ?? "",
                                height: fileInfo.Height ?? "",
                                tetBleed: fileInfo.TetBleed ?? "",
                                isColumnCombineMode: AppSettings.EnableColumnCombine,
                                columnNames: GetExcelColumnNames(),
                                columnItemsMap: GetExcelColumnItemsMap(),
                                initialSerialNumber: GetNextSerialNumber(),
                                enableSerialSearchResultToRegex: _excelImportService.EnableSerialSearchResultToRegex,
                                serialSearchResultColumnIndex: _excelImportService.SerialSearchResultColumnIndex,
                                out selectionResult
                            );`;

const newElseDialog = `                            // ✅ 修复：使用匹配正则结果(cmbRegex2)传递给对话框，用于Excel数据匹配
                            string matchingRegexResult = GetRegexResultForMatching(fileInfo);
                            var pendingList2 = CollectPendingBatchFiles(fileInfo.FullPath);
                            dialogResult = _view.ShowMaterialSelectionDialog(
                                materials: _materials,
                                fileName: fileInfo.FullPath,  // ✅ 修复：传递完整路径
                                regexResult: matchingRegexResult ?? "",
                                width: fileInfo.Width ?? "",
                                height: fileInfo.Height ?? "",
                                tetBleed: fileInfo.TetBleed ?? "",
                                isColumnCombineMode: AppSettings.EnableColumnCombine,
                                columnNames: GetExcelColumnNames(),
                                columnItemsMap: GetExcelColumnItemsMap(),
                                initialSerialNumber: GetNextSerialNumber(),
                                enableSerialSearchResultToRegex: _excelImportService.EnableSerialSearchResultToRegex,
                                serialSearchResultColumnIndex: _excelImportService.SerialSearchResultColumnIndex,
                                out selectionResult,
                                pendingFilePaths: pendingList2
                            );`;

content = content.replace(oldElseDialog, newElseDialog);

fs.writeFileSync(presPath, content, 'utf8');
console.log('FileRenamePanelPresenter.cs patched successfully!');
