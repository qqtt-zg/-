const fs = require('fs');
const path = require('path');

const msFilePath = path.resolve('src/WindowsFormsApp3/Forms/Main/MaterialSelectFormModern.cs');
let content = fs.readFileSync(msFilePath, 'utf8');

// 1. 修复 ApplyThemeToSelects - 包含 cmbOrderNumberMode 和 cmbOrderRegexPattern
const oldSelects = 'var selects = new AntdUI.Select[] { bleedDropdown, dropdown16 };';
const newSelects = 'var selects = new AntdUI.Select[] { bleedDropdown, dropdown16, cmbOrderNumberMode, cmbOrderRegexPattern };';
content = content.replace(oldSelects, newSelects);

// 2. 修复 PrePositionWindow - 避免读取过大宽度
const oldPrePos = 'int savedWidth = AppSettings.MaterialFormWidth;';
const newPrePos = 'int savedWidth = AppSettings.MaterialFormWidth;\r\n                if (savedWidth > 450) savedWidth = 400;';
content = content.replace(oldPrePos, newPrePos);

// 3. 修复 FormClosing - 如果展开状态则剔除 420px 宽度后再保存
const oldFormClosing = `                // 保存窗口位置和状态
                WindowPositionManager.SaveWindowPosition(this, _isPreviewExpanded);`;

const newFormClosing = `                // 保存窗口位置和状态（如果批量面板处于展开状态，按折叠标准尺寸保存）
                if (_isBatchPanelExpanded)
                {
                    int standardLeft = this.Location.X + BATCH_PANEL_WIDTH;
                    int standardWidth = Math.Max(380, this.Size.Width - BATCH_PANEL_WIDTH);
                    AppSettings.MaterialFormX = standardLeft;
                    AppSettings.MaterialFormY = this.Location.Y;
                    AppSettings.MaterialFormWidth = standardWidth;
                    AppSettings.MaterialFormHeight = this.Size.Height;
                    AppSettings.MaterialFormMaximized = this.WindowState == FormWindowState.Maximized;
                    AppSettings.MaterialFormPreviewExpanded = _isPreviewExpanded;
                    AppSettings.CommitChanges();
                }
                else
                {
                    WindowPositionManager.SaveWindowPosition(this, _isPreviewExpanded);
                }`;
content = content.replace(oldFormClosing, newFormClosing);

// 4. 优化 InitializeOrderNumberControls 和 RestoreOrderNumberModeState
const oldInitControls = `                // 3. 事件绑定
                cmbOrderNumberMode.SelectedIndexChanged += CmbOrderNumberMode_SelectedIndexChanged;
                cmbOrderRegexPattern.SelectedIndexChanged += CmbOrderRegexPattern_SelectedIndexChanged;

                // 4. 恢复上次状态
                RestoreOrderNumberModeState();`;

const newInitControls = `                // 3. 事件绑定（同时监听 SelectedValueChanged 与 SelectedIndexChanged）
                cmbOrderNumberMode.SelectedValueChanged += (s, e) => HandleOrderNumberModeChanged();
                cmbOrderNumberMode.SelectedIndexChanged += (s, e) => HandleOrderNumberModeChanged();
                cmbOrderRegexPattern.SelectedValueChanged += (s, e) => HandleOrderRegexPatternChanged();
                cmbOrderRegexPattern.SelectedIndexChanged += (s, e) => HandleOrderRegexPatternChanged();

                // 4. 恢复上次状态
                RestoreOrderNumberModeState();`;
content = content.replace(oldInitControls, newInitControls);

// 5. 优化 RestoreOrderNumberModeState 的赋值与显示
const oldRestore = `                if (modeIdx >= 0)
                {
                    cmbOrderNumberMode.SelectedIndex = modeIdx;
                }
                else
                {
                    cmbOrderNumberMode.SelectedIndex = 0;
                }`;

const newRestore = `                if (modeIdx >= 0)
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
                }`;
content = content.replace(oldRestore, newRestore);

// 6. 添加 HandleOrderNumberModeChanged 和 HandleOrderRegexPatternChanged 方法
const oldEventHandlers = `        private void CmbOrderNumberMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string modeText = cmbOrderNumberMode.Text;
                if (modeText == "正则提取")
                {
                    _currentOrderNumberMode = OrderNumberMode.RegexExtraction;
                }
                else if (modeText == "自动递增")
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
        }

        private void CmbOrderRegexPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string regexName = cmbOrderRegexPattern.Text;
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
        }`;

const newEventHandlers = `        private void HandleOrderNumberModeChanged()
        {
            try
            {
                string modeText = cmbOrderNumberMode?.SelectedValue?.ToString() ?? cmbOrderNumberMode?.Text ?? "无";
                if (modeText == "正则提取")
                {
                    _currentOrderNumberMode = OrderNumberMode.RegexExtraction;
                }
                else if (modeText == "自动递增")
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
        }

        private void HandleOrderRegexPatternChanged()
        {
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
        }

        private void CmbOrderNumberMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleOrderNumberModeChanged();
        }

        private void CmbOrderRegexPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleOrderRegexPatternChanged();
        }`;

content = content.replace(oldEventHandlers, newEventHandlers);

// 7. 在 UpdateOrderNumberControlsLayout 中确保 BringToFront
const oldLayoutMethod = `                    orderNumberLabel.Location = new Point(10, labelTop);
                    orderNumberLabel.Size = new Size(48, 25);

                    orderNumberTextBox.Location = new Point(60, baseTop);
                    orderNumberTextBox.Size = new Size(125, 32);

                    cmbOrderNumberMode.Location = new Point(190, baseTop);
                    cmbOrderNumberMode.Size = new Size(86, 32);

                    if (cmbOrderRegexPattern != null)
                    {
                        cmbOrderRegexPattern.Location = new Point(280, baseTop);
                        cmbOrderRegexPattern.Size = new Size(98, 32);
                    }`;

const newLayoutMethod = `                    orderNumberLabel.Location = new Point(10, labelTop);
                    orderNumberLabel.Size = new Size(48, 25);

                    orderNumberTextBox.Location = new Point(60, baseTop);
                    orderNumberTextBox.Size = new Size(125, 32);

                    cmbOrderNumberMode.Location = new Point(190, baseTop);
                    cmbOrderNumberMode.Size = new Size(86, 32);
                    cmbOrderNumberMode.BringToFront();

                    if (cmbOrderRegexPattern != null)
                    {
                        cmbOrderRegexPattern.Location = new Point(280, baseTop);
                        cmbOrderRegexPattern.Size = new Size(98, 32);
                        cmbOrderRegexPattern.BringToFront();
                    }`;

content = content.replace(oldLayoutMethod, newLayoutMethod);

fs.writeFileSync(msFilePath, content, 'utf8');
console.log('MaterialSelectFormModern.cs patched successfully!');
