# -*- coding: utf-8 -*-
import os, re

form_path = 'src/WindowsFormsApp3/Forms/Main/MaterialSelectFormModern.cs'
with open(form_path, 'r', encoding='utf-8') as f:
    form = f.read()

# 1. Field declarations
old_fields = '''        // 顴设见京菜单
        private System.Windows.Forms.ContextMenuStrip _presetContextMenu;
        private string _currentPresetName = "";

        // 订单号模式与續级正则配置
        private readonly Dictionary<string, string> _orderRegexDict = new Dictionary<string, string>();
        private OrderNumberMode _currentOrderNumberMode = OrderNumberMode.None;
        private string _selectedOrderRegexName = "";
        private string _selectedOrderRegexPattern = "";'''

new_fields = '''        // 酴诺鞁键菝单;
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
        private ToolStripMenuItem _miModeRegex;'''

if old_fields in form:
    form = form.replace(old_fields, new_fields, 1)
else:
    form = form.replace(old_fields.replace('\r\n', '\n'), new_fields.replace('\r\n', '\n'), 1)

# 2. Properties
old_props = '''        public OrderNumberMode CurrentOrderNumberMode => _currentOrderNumberMode;
        public string SelectedOrderRegexName => _selectedOrderRegexName;
        public string SelectedOrderRegexPattern => _selectedOrderRegexPattern;'''

new_props = '''        public OrderNumberMode CurrentOrderNumberMode => _currentOrderNumberMode;
        public string SelectedOrderRegexName => _selectedOrderRegexName;
        public string SelectedOrderRegexPattern => _selectedOrderRegexPattern;
        public System.Windows.Forms.ContextMenuStrip OrderNumberModeMenu => _orderNumberModeMenu;
        public AntdUI.Button BtnOrderNumberMode => btnOrderNumberMode;'''

if old_props in form:
    form = form.replace(old_props, new_props, 1)
else:
    form = form.replace(old_props.replace('\r\n', '\n'), new_props.replace('\r\n', '\n'), 1)

