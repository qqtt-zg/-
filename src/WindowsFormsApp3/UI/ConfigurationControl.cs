using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp3.Services;
using WindowsFormsApp3.Models;
using System.Linq;

namespace WindowsFormsApp3.UI
{
    /// <summary>
    /// 配置控件类，用于处理配置项的UI交互
    /// </summary>
    public class ConfigurationControl : UserControl
    {
        private IConfigService _configService;
        private List<ConfigurationItem> _configItems;
        private FlowLayoutPanel _flowPanel;
        private Button _saveButton;
        private Button _resetButton;
        private Dictionary<string, Control> _configControls;

        /// <summary>
        /// 配置保存事件
        /// </summary>
        public event EventHandler ConfigSaved;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigurationControl()
        {
            InitializeComponent();
            InitializeConfiguration();
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitializeComponent()
        {
            this._flowPanel = new FlowLayoutPanel();
            this._saveButton = new Button();
            this._resetButton = new Button();
            this.SuspendLayout();
            
            // 配置FlowLayoutPanel
            this._flowPanel.Dock = DockStyle.Fill;
            this._flowPanel.AutoScroll = true;
            this._flowPanel.FlowDirection = FlowDirection.TopDown;
            this._flowPanel.WrapContents = false;
            this._flowPanel.Margin = new Padding(10);
            this._flowPanel.Padding = new Padding(10);
            
            // 配置保存按钮
            this._saveButton.Text = "保存配置";
            this._saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this._saveButton.Click += SaveButton_Click;
            
            // 配置重置按钮
            this._resetButton.Text = "重置为默认值";
            this._resetButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this._resetButton.Margin = new Padding(0, 0, 10, 0);
            this._resetButton.Click += ResetButton_Click;
            
            // 配置底部按钮面板
            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Height = 50;
            buttonPanel.Controls.Add(this._resetButton);
            buttonPanel.Controls.Add(this._saveButton);
            this._resetButton.Location = new Point(buttonPanel.Width - 260, 10);
            this._saveButton.Location = new Point(buttonPanel.Width - 120, 10);
            
            // 配置主控件
            this.Controls.Add(this._flowPanel);
            this.Controls.Add(buttonPanel);
            this.Size = new Size(500, 400);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        private void InitializeConfiguration()
        {
            _configService = ServiceLocator.Instance.GetConfigService();
            _configControls = new Dictionary<string, Control>();
            LoadConfigurationItems();
            CreateConfigurationControls();
        }

        /// <summary>
        /// 加载配置项
        /// </summary>
        private void LoadConfigurationItems()
        {
            _configItems = new List<ConfigurationItem>();
            
            // 添加通用配置项
            _configItems.AddRange(new[]
            {
                new ConfigurationItem { Name = "AutoSaveConfig", DisplayName = "自动保存配置", DataType = typeof(bool), DefaultValue = true },
                new ConfigurationItem { Name = "BackupBeforeOperation", DisplayName = "操作前备份", DataType = typeof(bool), DefaultValue = true },
                new ConfigurationItem { Name = "ShowNotification", DisplayName = "显示操作通知", DataType = typeof(bool), DefaultValue = true },
                new ConfigurationItem { Name = "MaxRecentFiles", DisplayName = "最近文件列表大小", DataType = typeof(int), DefaultValue = 10, MinValue = 1, MaxValue = 50 },
                new ConfigurationItem { Name = "DefaultExportFormat", DisplayName = "默认导出格式", DataType = typeof(string), DefaultValue = "Excel", Options = new[] { "Excel", "CSV", "TXT" } },
                new ConfigurationItem { Name = "AutoUpdateCheck", DisplayName = "自动检查更新", DataType = typeof(bool), DefaultValue = false },
                new ConfigurationItem { Name = "Theme", DisplayName = "界面主题", DataType = typeof(string), DefaultValue = "Light", Options = new[] { "Light", "Dark", "System" } }
            });
        }

        /// <summary>
        /// 创建配置控件
        /// </summary>
        private void CreateConfigurationControls()
        {
            _flowPanel.Controls.Clear();
            _configControls.Clear();
            
            foreach (var configItem in _configItems)
            {
                Panel itemPanel = CreateConfigItemPanel(configItem);
                _flowPanel.Controls.Add(itemPanel);
            }
        }

        /// <summary>
        /// 创建配置项面板
        /// </summary>
        /// <param name="configItem">配置项</param>
        /// <returns>配置项面板</returns>
        private Panel CreateConfigItemPanel(ConfigurationItem configItem)
        {
            Panel panel = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(5) };
            
            Label label = new Label 
            {
                Text = configItem.DisplayName,
                Width = 150,
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(5, 5)
            };
            
            Control control = CreateControlForConfigItem(configItem);
            control.Location = new Point(160, 5);
            control.Width = 200;
            
            panel.Controls.Add(label);
            panel.Controls.Add(control);
            
            _configControls[configItem.Name] = control;
            
            return panel;
        }

        /// <summary>
        /// 为配置项创建相应的控件
        /// </summary>
        /// <param name="configItem">配置项</param>
        /// <returns>控件</returns>
        private Control CreateControlForConfigItem(ConfigurationItem configItem)
        {
            Control control;
            object currentValue = _configService.GetValue(configItem.Name, configItem.DefaultValue);
            
            if (configItem.DataType == typeof(bool))
            {
                CheckBox checkBox = new CheckBox { Checked = Convert.ToBoolean(currentValue) };
                control = checkBox;
            }
            else if (configItem.DataType == typeof(int))
            {
                NumericUpDown numericUpDown = new NumericUpDown
                {
                    Value = Convert.ToDecimal(currentValue),
                    Minimum = Convert.ToDecimal(configItem.MinValue ?? 0),
                    Maximum = Convert.ToDecimal(configItem.MaxValue ?? 1000)
                };
                control = numericUpDown;
            }
            else if (configItem.Options != null && configItem.Options.Length > 0)
            {
                ComboBox comboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
                comboBox.Items.AddRange(configItem.Options);
                comboBox.SelectedItem = currentValue.ToString();
                control = comboBox;
            }
            else
            {
                TextBox textBox = new TextBox { Text = currentValue?.ToString() ?? string.Empty };
                control = textBox;
            }
            
            return control;
        }

        /// <summary>
        /// 保存按钮点击事件
        /// </summary>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveConfiguration();
        }

        /// <summary>
        /// 重置按钮点击事件
        /// </summary>
        private void ResetButton_Click(object sender, EventArgs e)
        {
            if (UIHelper.ShowConfirmDialog("确定要重置所有配置为默认值吗？") == DialogResult.Yes)
            {
                ResetToDefaults();
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void SaveConfiguration()
        {
            try
            {
                foreach (var configItem in _configItems)
                {
                    if (_configControls.TryGetValue(configItem.Name, out Control control))
                    {
                        object value = GetValueFromControl(control, configItem.DataType);
                        _configService.SetValue(configItem.Name, value);
                    }
                }
                
                _configService.SaveConfig();
                UIHelper.ShowSuccessMessage("配置保存成功！");
                
                ConfigSaved?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage("保存配置失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 从控件获取值
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="dataType">数据类型</param>
        /// <returns>控件值</returns>
        private object GetValueFromControl(Control control, Type dataType)
        {
            if (control is CheckBox checkBox)
            {
                return checkBox.Checked;
            }
            else if (control is NumericUpDown numericUpDown)
            {
                return Convert.ChangeType(numericUpDown.Value, dataType);
            }
            else if (control is ComboBox comboBox)
            {
                return comboBox.SelectedItem;
            }
            else if (control is TextBox textBox)
            {
                return Convert.ChangeType(textBox.Text, dataType);
            }
            
            return null;
        }

        /// <summary>
        /// 重置为默认值
        /// </summary>
        public void ResetToDefaults()
        {
            foreach (var configItem in _configItems)
            {
                if (_configControls.TryGetValue(configItem.Name, out Control control))
                {
                    SetControlValue(control, configItem.DefaultValue);
                }
            }
        }

        /// <summary>
        /// 设置控件值
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="value">值</param>
        private void SetControlValue(Control control, object value)
        {
            if (control is CheckBox checkBox)
            {
                checkBox.Checked = Convert.ToBoolean(value);
            }
            else if (control is NumericUpDown numericUpDown)
            {
                numericUpDown.Value = Convert.ToDecimal(value);
            }
            else if (control is ComboBox comboBox)
            {
                comboBox.SelectedItem = value?.ToString();
            }
            else if (control is TextBox textBox)
            {
                textBox.Text = value?.ToString() ?? string.Empty;
            }
        }

        /// <summary>
        /// 刷新配置控件
        /// </summary>
        public void RefreshConfiguration()
        {
            CreateConfigurationControls();
        }
    }

    /// <summary>
    /// 配置项模型
    /// </summary>
    public class ConfigurationItem
    {
        /// <summary>
        /// 配置项名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public object MinValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public object MaxValue { get; set; }

        /// <summary>
        /// 选项列表（用于下拉选择框）
        /// </summary>
        public string[] Options { get; set; }
    }
}