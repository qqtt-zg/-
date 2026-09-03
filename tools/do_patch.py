# -*- coding: utf-8 -*-
import re

form_path = "src/WindowsFormsApp3/Forms/Main/MaterialSelectFormModern.cs"
with open(form_path, "r", encoding="utf-8") as f:
    form = f.read()

# 1. Field declarations
old_fields = """        // 预设右键菜单
        private System.Windows.Forms.ContextMenuStrip _presetContextMenu;
        private string _currentPresetName = "";

        // 订单号模式与二级正则配置
        private readonly Dictionary<string, string> _orderRegexDict = new Dictionary<string, string>();
        private OrderNumberMode _currentOrderNumberMode = OrderNumberMode.None;
        private string _selectedOrderRegexName = "";
        private string _selectedOrderRegexPattern = "";"""

new_fields = """        // 预设右键菜单
        private System.Windows.Forms.ContextMenuStrip _presetContextMenu;
        private string _currentPresetName = "";

        // 订单号模式与二级正则配置
        private readonly Dictionary<string, string> _orderRegexDict = new Dictionary<string, string>();
        private OrderNumberMode _currentOrderNumberMode = OrderNumberMode.None;
        private string _selectedOrderRegexName = "";
        private string _selectedOrderRegexPattern = "";

        // 订单号模式下拉菜单
        private System.Windows.Forms.ContextMenuStrip _orderNumberModeMenu;
        private ToolStripMenuItem _miModeNone;
        private ToolStripMenuItem _miModeAutoInc;
        private ToolStripMenuItem _miModeRegex;"""

form = form.replace(old_fields, new_fields, 1)

# 2. Properties
old_props = """        public OrderNumberMode CurrentOrderNumberMode => _currentOrderNumberMode;
        public string SelectedOrderRegexName => _selectedOrderRegexName;
        public string SelectedOrderRegexPattern => _selectedOrderRegexPattern;"""

new_props = """        public OrderNumberMode CurrentOrderNumberMode => _currentOrderNumberMode;
        public string SelectedOrderRegexName => _selectedOrderRegexName;
        public string SelectedOrderRegexPattern => _selectedOrderRegexPattern;
        public System.Windows.Forms.ContextMenuStrip OrderNumberModeMenu => _orderNumberModeMenu;
        public AntdUI.Button BtnOrderNumberMode => btnOrderNumberMode;"""

form = form.replace(old_props, new_props, 1)

new_region = """        #region 订单号模式与正则二级子菜单逻辑

        /// <summary>
        /// 加载已配置的正则表达式字典（若无则填充常用默认规则）
        /// </summary>
        private void LoadOrderRegexDictionary()
        {
            _orderRegexDict.Clear();
            string rawPatterns = AppSettings.RegexPatterns;
            if (!string.IsNullOrEmpty(rawPatterns))
            {
                string[] patterns = rawPatterns.Split(""" + pipe_sym + """, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in patterns)
                {
                    var parts = p.Split(""" + eq_sym + """);
                    if (parts.Length == 2 && !string.IsNullOrWhiteSpace(parts[0]) && !_orderRegexDict.ContainsKey(parts[0]))
                    {
                        _orderRegexDict[parts[0]] = parts[1];
                    }
                }
            }

            // 若无预设正则，提供常用默认规则
            if (_orderRegexDict.Count == 0)
            {
                _orderRegexDict["订单号_PO数字"] = @"PO\\d+";
                _orderRegexDict["订单号_6位及以上数字"] = """ + d6 + """
                _orderRegexDict["订单号_带横杠"] = @"[A-Z0-9]+-[A-Z0-9]+";
            }
        }

        /// <summary>
        /// 构建订单号模式及正则子菜单
        /// </summary>
        private void BuildOrderNumberModeMenu()
        {
            if (_orderNumberModeMenu == null)
            {
                _orderNumberModeMenu = new System.Windows.Forms.ContextMenuStrip();
                _orderNumberModeMenu.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            }
            _orderNumberModeMenu.Items.Clear();

            _miModeNone = new ToolStripMenuItem("无", null, (s, e) => SelectOrderNumberMode(OrderNumberMode.None));
            _miModeAutoInc = new ToolStripMenuItem("自动递增", null, (s, e) => SelectOrderNumberMode(OrderNumberMode.AutoIncrement));
            _miModeRegex = new ToolStripMenuItem("正则提取");

            // 点击“正则提取”父级项时，使用当前选中的规则或首个规则
            _miModeRegex.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty(_selectedOrderRegexName) && _orderRegexDict.Count > 0)
                {
                    var first = _orderRegexDict.First();
                    SelectOrderRegexRule(first.Key, first.Value);
                }
                else if (!string.IsNullOrEmpty(_selectedOrderRegexName) && _orderRegexDict.TryGetValue(_selectedOrderRegexName, out string pat))
                {
                    SelectOrderRegexRule(_selectedOrderRegexName, pat);
                }
            };

            // 添加二级子菜单项（正则规则列表）
            foreach (var kvp in _orderRegexDict)
            {
                string ruleName = kvp.Key;
                string rulePattern = kvp.Value;
                var subItem = new ToolStripMenuItem(ruleName, null, (s, e) => SelectOrderRegexRule(ruleName, rulePattern));
                _miModeRegex.DropDownItems.Add(subItem);
            }

            _orderNumberModeMenu.Items.Add(_miModeNone);
            _orderNumberModeMenu.Items.Add(_miModeAutoInc);
            _orderNumberModeMenu.Items.Add(_miModeRegex);
        }

        /// <summary>
        /// 初始化订单号模式控件与菜单
        /// </summary>
        private void InitializeOrderNumberControls()
        {
            try
            {
                LoadOrderRegexDictionary();
                BuildOrderNumberModeMenu();
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
                LoadOrderRegexDictionary();
                BuildOrderNumberModeMenu();

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

                string savedRegexName = AppSettings.GetValue<string>("OrderRegexPattern1");
                if (!string.IsNullOrEmpty(savedRegexName) && _orderRegexDict.TryGetValue(savedRegexName, out string pattern))
                {
                    _selectedOrderRegexName = savedRegexName;
                    _selectedOrderRegexPattern = pattern;
                }
                else if (_orderRegexDict.Count > 0)
                {
                    var first = _orderRegexDict.First();
                    _selectedOrderRegexName = first.Key;
                    _selectedOrderRegexPattern = first.Value;
                }

                if (savedMode == "正则提取")
                {
                    SelectOrderRegexRule(_selectedOrderRegexName, _selectedOrderRegexPattern, true);
                }
                else if (savedMode == "自动递增")
                {
                    SelectOrderNumberMode(OrderNumberMode.AutoIncrement, false);
                }
                else
                {
                    SelectOrderNumberMode(OrderNumberMode.None, false);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[MaterialSelectFormModern] 恢复订单号模式状态失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 更新订单号模式按钮显示文本
        /// </summary>
        private void UpdateOrderModeButtonDisplay()
        {
            if (btnOrderNumberMode == null) return;

            switch (_currentOrderNumberMode)
            {
                case OrderNumberMode.AutoIncrement:
                    btnOrderNumberMode.Text = "自动递增 ▾";
                    break;
                case OrderNumberMode.RegexExtraction:
                    btnOrderNumberMode.Text = "正则提取 ▾";
                    break;
                case OrderNumberMode.None:
                default:
                    btnOrderNumberMode.Text = "无 ▾";
                    break;
            }
        }

        /// <summary>
        /// 更新菜单勾选状态
        /// </summary>
        private void UpdateOrderNumberModeMenuChecks()
        {
            if (_miModeNone != null) _miModeNone.Checked = (_currentOrderNumberMode == OrderNumberMode.None);
            if (_miModeAutoInc != null) _miModeAutoInc.Checked = (_currentOrderNumberMode == OrderNumberMode.AutoIncrement);
            if (_miModeRegex != null)
            {
                _miModeRegex.Checked = (_currentOrderNumberMode == OrderNumberMode.RegexExtraction);
                foreach (ToolStripItem sub in _miModeRegex.DropDownItems)
                {
                    if (sub is ToolStripMenuItem subMenu)
                    {
                        subMenu.Checked = (_currentOrderNumberMode == OrderNumberMode.RegexExtraction && subMenu.Text == _selectedOrderRegexName);
                    }
                }
            }
        }

        /// <summary>
        /// 选择订单号模式
        /// </summary>
        public void SelectOrderNumberMode(OrderNumberMode mode, bool saveAndExtract = true)
        {
            _currentOrderNumberMode = mode;
            if (mode == OrderNumberMode.None)
            {
                AppSettings.Set("OrderNumberMode1", "无");
                AppSettings.Set("AutoIncrementOrderNumber1", false);
                UpdateOrderModeButtonDisplay();
                UpdateOrderNumberModeMenuChecks();
                UpdateOrderNumberControlsLayout();
                if (saveAndExtract)
                {
                    UpdateBatchOrderNumbers();
                }
            }
            else if (mode == OrderNumberMode.AutoIncrement)
            {
                AppSettings.Set("OrderNumberMode1", "自动递增");
                AppSettings.Set("AutoIncrementOrderNumber1", true);
                UpdateOrderModeButtonDisplay();
                UpdateOrderNumberModeMenuChecks();
                UpdateOrderNumberControlsLayout();
                if (saveAndExtract)
                {
                    UpdateBatchOrderNumbers();
                }
            }
            else if (mode == OrderNumberMode.RegexExtraction)
            {
                if (string.IsNullOrEmpty(_selectedOrderRegexName) || !_orderRegexDict.ContainsKey(_selectedOrderRegexName))
                {
                    if (_orderRegexDict.Count > 0)
                    {
                        var first = _orderRegexDict.First();
                        _selectedOrderRegexName = first.Key;
                        _selectedOrderRegexPattern = first.Value;
                    }
                }
                SelectOrderRegexRule(_selectedOrderRegexName, _selectedOrderRegexPattern, saveAndExtract);
            }
        }

        /// <summary>
        /// 选择具体的正则表达式规则
        /// </summary>
        public void SelectOrderRegexRule(string ruleName, string pattern, bool saveAndExtract = true)
        {
            _currentOrderNumberMode = OrderNumberMode.RegexExtraction;
            _selectedOrderRegexName = ruleName ?? "";
            _selectedOrderRegexPattern = pattern ?? "";

            AppSettings.Set("OrderNumberMode1", "正则提取");
            AppSettings.Set("AutoIncrementOrderNumber1", false);
            if (!string.IsNullOrEmpty(ruleName))
            {
                AppSettings.Set("OrderRegexPattern1", ruleName);
            }

            UpdateOrderModeButtonDisplay();
            UpdateOrderNumberModeMenuChecks();
            UpdateOrderNumberControlsLayout();

            if (saveAndExtract)
            {
                ApplyRegexOrderExtraction();
            }
        }

        /// <summary>
        /// 点击订单号模式按钮，弹出下拉菜单
        /// </summary>
        private void btnOrderNumberMode_Click(object sender, EventArgs e)
        {
            try
            {
                if (_orderNumberModeMenu != null && btnOrderNumberMode != null)
                {
                    UpdateOrderNumberModeMenuChecks();
                    _orderNumberModeMenu.Show(btnOrderNumberMode, new Point(0, btnOrderNumberMode.Height));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[MaterialSelectFormModern] 显示订单号模式菜单失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 确保订单号控件布局固定整齐
        /// </summary>
        private void UpdateOrderNumberControlsLayout()
        {
            try
            {
                if (orderNumberLabel == null || orderNumberTextBox == null || btnOrderNumberMode == null) return;

                int baseTop = 329;
                int labelTop = 333;

                orderNumberLabel.Text = "订单号:";
                SetControlBoundsSafe(orderNumberLabel, 150, labelTop, 42, 25);
                SetControlBoundsSafe(orderNumberTextBox, 197, baseTop, 98, 32);
                SetControlBoundsSafe(btnOrderNumberMode, 300, baseTop, 82, 32);
                btnOrderNumberMode.BringToFront();
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[MaterialSelectFormModern] 更新订单号布局失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 使用选中的正则规则提取当前文件及列表中的订单号
        /// </summary>
        private void ApplyRegexOrderExtraction()
        {
            try
            {
                if (!string.IsNullOrEmpty(_selectedOrderRegexPattern))
                {
                    // 提取当前文件订单号
                    string extracted = ExtractOrderNumberByRegex(CurrentFileName, _selectedOrderRegexPattern);
                    if (!string.IsNullOrEmpty(extracted))
                    {
                        if (orderNumberTextBox != null)
                        {
                            orderNumberTextBox.Text = extracted;
                        }
                        OrderNumber = extracted;
                    }

                    // 刷新批量列表
                    UpdateBatchOrderNumbers();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[MaterialSelectFormModern] 应用正则提取失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 正则提取订单号辅助方法
        /// </summary>
        public static string ExtractOrderNumberByRegex(string fileName, string pattern)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(pattern))
                return "";

            string pureName = Path.GetFileNameWithoutExtension(fileName);
            try
            {
                var match = Regex.Match(pureName, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    if (match.Groups["order"].Success)
                        return match.Groups["order"].Value;
                    if (match.Groups.Count > 1)
                        return match.Groups[1].Value;
                    return match.Value;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[ExtractOrderNumberByRegex] 正则提取异常: {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// 订单号递增计算辅助方法
        /// </summary>
        public static string CalculateIncrementalOrderNumber(string baseOrderNumber, int offsetIndex)
        {
            if (string.IsNullOrWhiteSpace(baseOrderNumber) || offsetIndex <= 0)
                return baseOrderNumber ?? "";

            Match match = Regex.Match(baseOrderNumber, @"(.*?)(\d+)$");
            if (match.Success)
            {
                string prefix = match.Groups[1].Value;
                string numberStr = match.Groups[2].Value;
                if (long.TryParse(numberStr, out long number))
                {
                    long newNumber = number + offsetIndex;
                    return prefix + newNumber.ToString().PadLeft(numberStr.Length, """ + zero_sym + """);
                }
            }

            return $"{baseOrderNumber}_{offsetIndex + 1}";
        }

        /// <summary>
        /// 刷新批量列表中所有文件的预览订单号
        /// </summary>
        private void UpdateBatchOrderNumbers()
        {
            try
            {
                if (_batchItems == null || _batchItems.Count == 0) return;

                string baseOrder = orderNumberTextBox?.Text ?? "";

                for (int i = 0; i < _batchItems.Count; i++)
                {
                    var item = _batchItems[i];
                    item.Index = i + 1;

                    if (_currentOrderNumberMode == OrderNumberMode.RegexExtraction)
                    {
                        if (!string.IsNullOrEmpty(_selectedOrderRegexPattern))
                        {
                            item.OrderNumber = ExtractOrderNumberByRegex(item.FileName, _selectedOrderRegexPattern);
                        }
                    }
                    else if (_currentOrderNumberMode == OrderNumberMode.AutoIncrement)
                    {
                        item.OrderNumber = CalculateIncrementalOrderNumber(baseOrder, i);
                    }
                    else
                    {
                        item.OrderNumber = baseOrder;
                    }
                }

                dgvBatchFiles?.Refresh();
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[MaterialSelectFormModern] 刷新批量订单号失败: {ex.Message}", ex);
            }
        }

        #endregion"""

region_pattern = re.compile(r"\s*#region 订单号模式与正则.*?#endregion", re.DOTALL)
form = region_pattern.sub("\r\n" + new_region, form, count=1)

# 4. orderNumberTextBox_TextChanged
old_txt_changed = """        private void orderNumberTextBox_TextChanged(object sender, EventArgs e)
        {

        }"""
new_txt_changed = """        private void orderNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                OrderNumber = orderNumberTextBox.Text;
                if (_currentOrderNumberMode != OrderNumberMode.RegexExtraction)
                {
                    UpdateBatchOrderNumbers();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[orderNumberTextBox_TextChanged] 异常: {ex.Message}", ex);
            }
        }"""
if old_txt_changed in form:
    form = form.replace(old_txt_changed, new_txt_changed, 1)

# 5. ApplyThemeToSelects
old_theme_selects = "var selects = new AntdUI.Select[] { bleedDropdown, dropdown16, cmbOrderNumberMode, cmbOrderRegexPattern };"
new_theme_selects = "var selects = new AntdUI.Select[] { bleedDropdown, dropdown16 };"
if old_theme_selects in form:
    form = form.replace(old_theme_selects, new_theme_selects, 1)

# 6. ApplyThemeToButtons
btn_theme_hook = """            if (btnOrderNumberMode != null)
            {
                ApplyThemeToMaterialButton(btnOrderNumberMode, theme, isDark);
            }
            if (btnApplyToAll != null)
            {
                ApplyThemeToMaterialButton(btnApplyToAll, theme, isDark);
            }
        }"""

form = re.sub(
    r"\s*if\s*\(btnApplyToAll\s*!=\s*null\)[\s\S]*?ApplyThemeToMaterialButton\(btnApplyToAll,\s*theme,\s*isDark\);\s*\}\s*\}",
    "\r\n" + btn_theme_hook,
    form,
    count=1
)

with open(form_path, "w", encoding="utf-8") as f:
    f.write(form)

print("MaterialSelectFormModern.cs patched successfully!")
