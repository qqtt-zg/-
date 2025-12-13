using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp3.Utils;
using WindowsFormsApp3.Services;

namespace WindowsFormsApp3.Forms.Dialogs
{
    /// <summary>
    /// 字体设置窗体 - 允许用户选择和管理中文字体
    /// </summary>
    public partial class FontSettingsForm : Form
    {
        private List<string> _availableFonts = new List<string>();
        private string _selectedFont = "msyh";

        public FontSettingsForm()
        {
            InitializeComponent();
            InitializeFontList();
        }

        /// <summary>
        /// 获取选中的字体名称
        /// </summary>
        /// <returns>字体名称</returns>
        public string GetSelectedFont()
        {
            return _selectedFont;
        }

  
        /// <summary>
        /// 初始化字体列表
        /// </summary>
        private void InitializeFontList()
        {
            try
            {
                FontManager.Initialize();
                _availableFonts = FontManager.GetAvailableFonts();

                // 添加字体显示名称映射
                var fontDisplayNames = new Dictionary<string, string>
                {
                    { "msyh", "微软雅黑" },
                    { "simhei", "黑体" },
                    { "simsun", "宋体" },
                    { "kaiti", "楷体" },
                    { "fangsong", "仿宋" }
                };

                fontListBox.Items.Clear();
                foreach (var fontName in _availableFonts)
                {
                    string displayName = fontDisplayNames.TryGetValue(fontName, out string name)
                        ? name
                        : fontName;
                    fontListBox.Items.Add(new FontItem(fontName, displayName));
                }

                // 默认选中第一个
                if (fontListBox.Items.Count > 0)
                {
                    fontListBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"初始化字体列表失败: {ex.Message}");
                MessageBox.Show($"初始化字体列表失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 字体列表选择改变事件
        /// </summary>
        private void FontListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fontListBox.SelectedItem is FontItem selectedItem)
            {
                _selectedFont = selectedItem.FontName;
                UpdatePreview(selectedItem.FontName);
            }
        }

        /// <summary>
        /// 更新预览
        /// </summary>
        /// <param name="fontName">字体名称</param>
        private void UpdatePreview(string fontName)
        {
            try
            {
                var fontDisplayNames = new Dictionary<string, string>
                {
                    { "msyh", "微软雅黑" },
                    { "simhei", "黑体" },
                    { "simsun", "宋体" },
                    { "kaiti", "楷体" },
                    { "fangsong", "仿宋" }
                };

                string displayName = fontDisplayNames.TryGetValue(fontName, out string name)
                    ? name
                    : fontName;

                try
                {
                    // 尝试使用系统字体进行预览
                    var systemFontName = GetSystemFontName(fontName);
                    previewLabel.Font = new Font(systemFontName, 10F);
                }
                catch
                {
                    // 如果系统字体不可用，使用默认字体
                    previewLabel.Font = new Font("微软雅黑", 10F);
                }

                LogHelper.Debug($"预览字体已更新: {displayName}");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"更新字体预览失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取对应的系统字体名称
        /// </summary>
        /// <param name="internalFontName">内部字体名称</param>
        /// <returns>系统字体名称</returns>
        private string GetSystemFontName(string internalFontName)
        {
            return internalFontName switch
            {
                "msyh" => "微软雅黑",
                "simhei" => "黑体",
                "simsun" => "宋体",
                "kaiti" => "楷体",
                "fangsong" => "仿宋",
                _ => "微软雅黑"
            };
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedFont))
            {
                MessageBox.Show("请选择一个字体", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LogHelper.Info($"用户选择了字体: {_selectedFont}");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 重新加载字体按钮点击事件
        /// </summary>
        private void ReloadButton_Click(object sender, EventArgs e)
        {
            try
            {
                LogHelper.Info("重新加载字体缓存");
                FontManager.ClearCache();
                FontManager.Initialize();
                InitializeFontList();

                MessageBox.Show("字体重新加载成功！", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogHelper.Error($"重新加载字体失败: {ex.Message}");
                MessageBox.Show($"重新加载字体失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>
    /// 字体项目类
    /// </summary>
    public class FontItem
    {
        public string FontName { get; }
        public string DisplayName { get; }

        public FontItem(string fontName, string displayName)
        {
            FontName = fontName;
            DisplayName = displayName;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}