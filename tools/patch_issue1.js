
const fs = require("fs");
const path = require("path");

// --- 1. MaterialSelectFormModern.cs ---
const msPath = path.resolve("src/WindowsFormsApp3/Forms/Main/MaterialSelectFormModern.cs");
let msContent = fs.readFileSync(msPath, "utf8");

// Add anti-reentry booleans and SetControlBoundsSafe helper
const fieldsTarget = "private string _selectedOrderRegexPattern = \"\";";
const fieldsReplacement = `private string _selectedOrderRegexPattern = "";
        private bool _isHandlingOrderModeChange = false;
        private bool _isHandlingOrderRegexChange = false;

        private static void SetControlBoundsSafe(Control ctrl, int x, int y, int width, int height)
        {
            if (ctrl == null) return;
            if (ctrl.Left != x || ctrl.Top != y || ctrl.Width != width || ctrl.Height != height)
            {
                ctrl.SetBounds(x, y, width, height);
            }
        }`;

if (!msContent.includes("_isHandlingOrderModeChange")) {
    msContent = msContent.replace(fieldsTarget, fieldsReplacement);
}

// Locate region from InitializeOrderNumberControls to UpdateOrderNumberControlsLayout
const startMarker = "/// <summary>\r\n        /// 初始化订单号模式下拉框与正则提取二级下拉框";
const endMarker = "/// <summary>\r\n        /// 使用选定的正则规则提取当前文件及批量列表中的订单号";

const sIdx = msContent.indexOf(startMarker);
const eIdx = msContent.indexOf(endMarker);

if (sIdx === -1 || eIdx === -1) {
    console.error("Markers not found:", sIdx, eIdx);
    process.exit(1);
}

const newOrderLogic = `/// <summary>
        /// 初始化订单号模式下拉框与正则提取二级下拉框
        /// </summary>
        private void InitializeOrderNumberControls()
        {
            try
            {
                if (cmbOrderNumberMode == null || cmbOrderRegexPattern == null) return;

                // 1. 初始化一级模式下拉框
                cmbOrderNumberMode.List = true;
                cmbOrderNumberMode.Items.Clear();
                cmbOrderNumberMode.Items.Add("无");
                cmbOrderNumberMode.Items.Add("自动递增");
                cmbOrderNumberMode.Items.Add("正则提取");

                // 2. 初始化二级正则表达式字典与下拉列表
                cmbOrderRegexPattern.List = true;
                _orderRegexDict.Clear();
                cmbOrderRegexPattern.Items.Clear();

                string rawPatterns = AppSettings.RegexPatterns;
                if (!string.IsNullOrEmpty(rawPatterns))
                {
                    string[] patterns = rawPatterns.Split(new[] { \x27|\x27 }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var p in patterns)
                    {
                        var parts = p.Split(\x27=\x27);
                        if (parts.Length == 2 && !string.IsNullOrWhiteSpace(parts[0]) && !_orderRegexDict.ContainsKey(parts[0]))
                        {
                            _orderRegexDict[parts[0]] = parts[1];
                            cmbOrderRegexPattern.Items.Add(parts[0]);
                        }
                    }
                }

                // 若无预设正则，提供常用默认规则
                if (cmbOrderRegexPattern.Items.Count == 0)
                {
                    _orderRegexDict["订单号_PO数字"] = @"PO\\d+";
                    _orderRegexDict["订单号_6位及以上数字"] = @"\\d{6,}";
                    _orderRegexDict["订单号_带横杠"] = @"[A-Z0-9]+-[A-Z0-9]+";
                    foreach (var k in _orderRegexDict.Keys)
                    {
                        cmbOrderRegexPattern.Items.Add(k);
                    }
                }

                // 3. 事件绑定（仅绑定 SelectedIndexChanged，防止双重触发与焦点竞争）
                cmbOrderNumberMode.SelectedIndexChanged += CmbOrderNumberMode_SelectedIndexChanged;
                cmbOrderRegexPattern.SelectedIndexChanged += CmbOrderRegexPattern_SelectedIndexChanged;

                // 4. 恢复上次状态
                RestoreOrderNumberModeState();
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[MaterialSelectFormModern] 初始化订单号控件失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 恢复订单号模式与正则配置状态
        /// </summary>
        private void RestoreOrderNumberModeState()
        {
            try
            {
                if (cmbOrderNumberMode == null) return;

                string savedMode = AppSettings.GetValue<string>("OrderNumberMode1");
                if (string.IsNullOrEmpty(savedMode))
                {
                    bool legacyAutoInc = AppSettings.GetValue<bool>("AutoIncrementOrderNumber1");
                    savedMode = legacyAutoInc ? "自动递增" : "无";
                }
                else
                {
                    if (savedMode == "True" || savedMode == "AutoIncrement" || savedMode == "1") savedMode = "自动递增";
                    else if (savedMode == "False" || savedMode == "None" || savedMode == "0") savedMode = "无";
                    else if (savedMode == "RegexExtraction" || savedMode == "Regex" || savedMode == "2") savedMode = "正则提取";
                }

                int modeIdx = -1;
                for (int i = 0; i < cmbOrderNumberMode.Items.Count; i++)
                {
                    if (cmbOrderNumberMode.Items[i]?.ToString() == savedMode)
                    {
                        modeIdx = i;
                        break;
                    }
                }

                _isHandlingOrderModeChange = true;
                try
                {
                    if (modeIdx >= 0)
                    {
                        cmbOrderNumberMode.SelectedIndex = modeIdx;
                        cmbOrderNumberMode.SelectedValue = cmbOrderNumberMode.Items[modeIdx];
                        cmbOrderNumberMode.Text = cmbOrderNumberMode.Items[modeIdx]?.ToString();
                    }
                    else
                    {
                        cmbOrderNumberMode.SelectedIndex = 0;
                        if (cmbOrderNumberMode.Items.Count > 0)
                        {
                            cmbOrderNumberMode.SelectedValue = cmbOrderNumberMode.Items[0];
                            cmbOrderNumberMode.Text = cmbOrderNumberMode.Items[0]?.ToString();
                        }
                    }
                }
                finally
                {
                    _isHandlingOrderModeChange = false;
                }

                string currentText = cmbOrderNumberMode.Text;
                if (currentText == "正则提取") _currentOrderNumberMode = OrderNumberMode.RegexExtraction;
                else if (currentText == "自动递增") _currentOrderNumberMode = OrderNumberMode.AutoIncrement;
                else _currentOrderNumberMode = OrderNumberMode.None;

                // 恢复选中的正则规则
                if (cmbOrderRegexPattern != null && cmbOrderRegexPattern.Items.Count > 0)
                {
                    string lastRegexName = AppSettings.GetValue<string>("LastSelectedOrderRegexName");
                    int regexIdx = -1;
                    if (!string.IsNullOrEmpty(lastRegexName))
                    {
                        for (int i = 0; i < cmbOrderRegexPattern.Items.Count; i++)
                        {
                            if (cmbOrderRegexPattern.Items[i]?.ToString() == lastRegexName)
                            {
                                regexIdx = i;
                                break;
                            }
                        }
                    }

                    _isHandlingOrderRegexChange = true;
                    try
                    {
                        if (regexIdx >= 0)
                        {
                            cmbOrderRegexPattern.SelectedIndex = regexIdx;
                            cmbOrderRegexPattern.SelectedValue = cmbOrderRegexPattern.Items[regexIdx];
                            cmbOrderRegexPattern.Text = cmbOrderRegexPattern.Items[regexIdx]?.ToString();
                        }
                        else
                        {
                            cmbOrderRegexPattern.SelectedIndex = 0;
                            cmbOrderRegexPattern.SelectedValue = cmbOrderRegexPattern.Items[0];
                            cmbOrderRegexPattern.Text = cmbOrderRegexPattern.Items[0]?.ToString();
                        }
                    }
                    finally
                    {
                        _isHandlingOrderRegexChange = false;
                    }

                    string selName = cmbOrderRegexPattern.Text;
                    if (!string.IsNullOrEmpty(selName) && _orderRegexDict.TryGetValue(selName, out string pattern))
                    {
                        _selectedOrderRegexName = selName;
                        _selectedOrderRegexPattern = pattern;
                    }
                }

                UpdateOrderNumberControlsLayout();
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[MaterialSelectFormModern] 恢复订单号模式失败: {ex.Message}", ex);
            }
        }

        private void HandleOrderNumberModeChanged()
        {
            if (_isHandlingOrderModeChange) return;
            _isHandlingOrderModeChange = true;
            try
            {
                string modeText = cmbOrderNumberMode?.SelectedValue?.ToString() ?? cmbOrderNumberMode?.Text ?? "无";
                if (modeText == "正则提取" || modeText == "RegexExtraction")
                {
                    _currentOrderNumberMode = OrderNumberMode.RegexExtraction;
                }
                else if (modeText == "自动递增" || modeText == "AutoIncrement")
                {
                    _currentOrderNumberMode = OrderNumberMode.AutoIncrement;
                }
                else
                {
                    _currentOrderNumberMode = OrderNumberMode.None;
                }

                AppSettings.Set("OrderNumberMode1", modeText);
                AppSettings.Set("AutoIncrementOrderNumber1", _currentOrderNumberMode == OrderNumberMode.AutoIncrement);
                AppSettings.Save();

                UpdateOrderNumberControlsLayout();
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[MaterialSelectFormModern] 订单号模式变更失败: {ex.Message}", ex);
            }
            finally
            {
                _isHandlingOrderModeChange = false;
            }
        }

        private void HandleOrderRegexPatternChanged()
        {
            if (_isHandlingOrderRegexChange) return;
            _isHandlingOrderRegexChange = true;
            try
            {
                string regexName = cmbOrderRegexPattern?.SelectedValue?.ToString() ?? cmbOrderRegexPattern?.Text ?? "";
                if (!string.IsNullOrEmpty(regexName) && _orderRegexDict.TryGetValue(regexName, out string pattern))
                {
                    _selectedOrderRegexName = regexName;
                    _selectedOrderRegexPattern = pattern;
                    AppSettings.Set("LastSelectedOrderRegexName", regexName);
                    AppSettings.Save();

                    ApplyRegexOrderExtraction();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[MaterialSelectFormModern] 正则表达式切换失败: {ex.Message}", ex);
            }
            finally
            {
                _isHandlingOrderRegexChange = false;
            }
        }

        private void CmbOrderNumberMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleOrderNumberModeChanged();
        }

        private void CmbOrderRegexPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleOrderRegexPatternChanged();
        }

        /// <summary>
        /// 动态调整订单号区域控件布局
        /// </summary>
        private void UpdateOrderNumberControlsLayout()
        {
            try
            {
                if (orderNumberLabel == null || orderNumberTextBox == null || cmbOrderNumberMode == null) return;

                int baseTop = 329;
                int labelTop = 333;

                if (_currentOrderNumberMode == OrderNumberMode.RegexExtraction)
                {
                    if (cmbOrderRegexPattern != null)
                    {
                        cmbOrderRegexPattern.Visible = true;
                        cmbOrderRegexPattern.BringToFront();
                    }

                    // 紧凑排布（右半区 X >= 146）
                    orderNumberLabel.Text = "订单:";
                    SetControlBoundsSafe(orderNumberLabel, 146, labelTop, 38, 25);
                    SetControlBoundsSafe(orderNumberTextBox, 186, baseTop, 58, 32);
                    SetControlBoundsSafe(cmbOrderNumberMode, 248, baseTop, 68, 32);
                    cmbOrderNumberMode.BringToFront();

                    if (cmbOrderRegexPattern != null)
                    {
                        SetControlBoundsSafe(cmbOrderRegexPattern, 318, baseTop, 68, 32);
                        cmbOrderRegexPattern.BringToFront();
                    }

                    ApplyRegexOrderExtraction();
                }
                else
                {
                    if (cmbOrderRegexPattern != null)
                    {
                        cmbOrderRegexPattern.Visible = false;
                    }

                    // 标准排布（右半区 X >= 150）
                    orderNumberLabel.Text = "订单号:";
                    SetControlBoundsSafe(orderNumberLabel, 150, labelTop, 42, 25);
                    SetControlBoundsSafe(orderNumberTextBox, 197, baseTop, 98, 32);
                    SetControlBoundsSafe(cmbOrderNumberMode, 300, baseTop, 82, 32);
                    cmbOrderNumberMode.BringToFront();

                    UpdateBatchOrderNumbers();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[MaterialSelectFormModern] 更新订单号布局失败: {ex.Message}", ex);
            }
        }
        
        `;

msContent = msContent.substring(0, sIdx) + newOrderLogic + msContent.substring(eIdx);

// Remove duplicate lines of "if (savedWidth > 450) savedWidth = 400;"
msContent = msContent.replace(/if \(savedWidth > 450\) savedWidth = 400;\r?\n\s*if \(savedWidth > 450\) savedWidth = 400;/g, "if (savedWidth > 450) savedWidth = 400;");

// Fix MaterialSelectFormModern_Shown async safety
const oldShown = `            this.BeginInvoke(new Action(async () =>
            {
                await Task.Delay(300); // 等待窗体完全渲染
                await TryLoadPendingPdf();
                LogHelper.Debug("[PDF 预览] Shown事件中检查PDF加载");
                
                // 额外刷新PDF预览控件
                await Task.Delay(200);
                if (_isPreviewExpanded && PdfPreview != null)
                {
                    PdfPreview.ApplyBestFitZoomPublic();
                    LogHelper.Debug("[PDF 预览] Shown事件中额外刷新PDF预览");
                }
                
                // 🔧 最终确认：再次延迟后强制应用缩放（确保万无一失）
                await Task.Delay(300);
                if (_isPreviewExpanded && PdfPreview != null && PdfPreview.PageCount > 0)
                {
                    LogHelper.Debug("[PDF 预览] Shown事件最终确认应用缩放");
                    PdfPreview.ApplyBestFitZoomPublic();
                }
                
                // 窗体内容准备好后，恢复显示
                if (this.Opacity == 0)
                {
                    this.Opacity = _opacityValue > 0 ? _opacityValue : 1.0;
                    LogHelper.Debug("[PDF 预览] 窗体内容准备完成，恢复显示");
                }
            }));`;

const newShown = `            this.BeginInvoke(new Action(async () =>
            {
                if (this.IsDisposed || !this.IsHandleCreated) return;
                await Task.Delay(300); // 等待窗体完全渲染
                if (this.IsDisposed || !this.IsHandleCreated) return;
                await TryLoadPendingPdf();
                LogHelper.Debug("[PDF 预览] Shown事件中检查PDF加载");
                
                // 额外刷新PDF预览控件
                await Task.Delay(200);
                if (this.IsDisposed || !this.IsHandleCreated) return;
                if (_isPreviewExpanded && PdfPreview != null)
                {
                    PdfPreview.ApplyBestFitZoomPublic();
                    LogHelper.Debug("[PDF 预览] Shown事件中额外刷新PDF预览");
                }
                
                // 🔧 最终确认：再次延迟后强制应用缩放（确保万无一失）
                await Task.Delay(300);
                if (this.IsDisposed || !this.IsHandleCreated) return;
                if (_isPreviewExpanded && PdfPreview != null && PdfPreview.PageCount > 0)
                {
                    LogHelper.Debug("[PDF 预览] Shown事件最终确认应用缩放");
                    PdfPreview.ApplyBestFitZoomPublic();
                }
                
                // 窗体内容准备好后，恢复显示
                if (this.Opacity == 0)
                {
                    this.Opacity = _opacityValue > 0 ? _opacityValue : 1.0;
                    LogHelper.Debug("[PDF 预览] 窗体内容准备完成，恢复显示");
                }
            }));`;

if (msContent.includes(oldShown)) {
    msContent = msContent.replace(oldShown, newShown);
}

// Ensure CRLF
msContent = msContent.replace(/\r?\n/g, "\r\n");
fs.writeFileSync(msPath, msContent, "utf8");
console.log("MaterialSelectFormModern.cs patched successfully!");

// --- 2. FileRenamePanel.cs ---
const frpPath = path.resolve("src/WindowsFormsApp3/Forms/Panels/FileRenamePanel.cs");
let frpContent = fs.readFileSync(frpPath, "utf8");

const oldResult = `                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        result = new MaterialSelectionResult
                        {`;

const newResult = `                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        result = new MaterialSelectionResult
                        {
                            IsApplyToAll = dialog.IsApplyToAll,
                            OrderNumberMode = dialog.CurrentOrderNumberMode,
                            SelectedOrderRegexName = dialog.SelectedOrderRegexName ?? "",
                            SelectedOrderRegexPattern = dialog.SelectedOrderRegexPattern ?? "",
                            BatchItems = dialog.BatchFileItems ?? new List<BatchFileItem>(),`;

if (frpContent.includes(oldResult) && !frpContent.includes("IsApplyToAll = dialog.IsApplyToAll")) {
    frpContent = frpContent.replace(oldResult, newResult);
    frpContent = frpContent.replace(/\r?\n/g, "\r\n");
    fs.writeFileSync(frpPath, frpContent, "utf8");
    console.log("FileRenamePanel.cs patched successfully!");
} else {
    console.log("FileRenamePanel.cs already patched or pattern not found.");
}
