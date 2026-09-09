using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp3.Models;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.UI
{
    /// <summary>
    /// 文件列表列头使用的可持续多选清单弹层。
    /// </summary>
    internal sealed class ColumnVisibilityChecklistPopup
    {
        private const int BaseDpi = 96;
        private const int PopupWidth = 280;
        private const int RowHeight = 30;
        private const int FooterHeight = 42;
        private const int MaxListHeight = 300;
        private const int PopupPadding = 8;
        private const int ButtonWidth = 88;
        private const int PopupShadow = 8;
        internal const int PopoverRadius = 8;
        internal const int PopoverGap = 4;

        private readonly DataGridView _grid;
        private readonly Action _saveSettings;
        private readonly Action _restoreDefaults;

        public ColumnVisibilityChecklistPopup(
            DataGridView grid,
            Action saveSettings,
            Action restoreDefaults)
        {
            _grid = grid ?? throw new ArgumentNullException(nameof(grid));
            _saveSettings = saveSettings ?? throw new ArgumentNullException(nameof(saveSettings));
            _restoreDefaults = restoreDefaults ?? throw new ArgumentNullException(nameof(restoreDefaults));
        }

        /// <summary>
        /// 在列头下方显示清单。勾选项不会关闭弹层，便于连续调整多列。
        /// </summary>
        public void Show(int columnIndex, Point mouseLocation)
        {
            if (columnIndex < 0 || columnIndex >= _grid.Columns.Count)
            {
                return;
            }

            var dpiScale = GetDpiScale(_grid);
            var content = CreateContent(dpiScale);
            var popupSize = new Size(
                content.Width + ScaleDimension(PopupShadow * 2, dpiScale),
                content.Height + ScaleDimension(PopupShadow * 2, dpiScale));
            var mouseScreen = _grid.PointToScreen(mouseLocation);
            var anchor = GetMouseAnchor(mouseScreen, popupSize);
            var workingArea = Screen.FromPoint(anchor).WorkingArea;
            anchor = ClampAnchorToWorkingArea(anchor, popupSize, workingArea);

            var config = new AntdUI.Popover.Config(_grid, content)
            {
                // CustomPoint 已经是屏幕坐标；不再同时设置 Offset，避免相对坐标被重复叠加。
                CustomPoint = new Rectangle(anchor, new Size(1, 1)),
                ArrowSize = 0,
                ArrowAlign = AntdUI.TAlign.Bottom,
                // Popover 外壳会按 Config.Control 的 DPI 自行缩放；这里必须传逻辑值。
                Radius = PopoverRadius,
                Padding = Size.Empty,
                Gap = PopoverGap,
                Focus = true,
                // 内容尺寸已按目标屏幕 DPI 计算，避免 Popover 再次缩放一遍。
                Dpi = 1F
            };

            AntdUI.Popover.open(config);
        }

        /// <summary>
        /// Popover 使用中心锚点；将其换算为菜单左上角贴近鼠标的位置。
        /// </summary>
        internal static Point GetMouseAnchor(Point mouseScreenLocation, Size popupSize)
        {
            return new Point(
                mouseScreenLocation.X + popupSize.Width / 2,
                mouseScreenLocation.Y);
        }

        private Control CreateContent(float dpiScale)
        {
            var tokens = GetCurrentTokens();
            var popupWidth = ScaleDimension(PopupWidth, dpiScale);
            var rowHeight = ScaleDimension(RowHeight, dpiScale);
            var footerHeight = ScaleDimension(FooterHeight, dpiScale);
            var listHeight = Math.Min(
                ScaleDimension(MaxListHeight, dpiScale),
                Math.Max(rowHeight, _grid.Columns.Count * rowHeight));
            var content = new AntdUI.Panel
            {
                Size = new Size(popupWidth, listHeight + footerHeight),
                Radius = ScaleDimension(8, dpiScale),
                Shadow = ScaleDimension(PopupShadow, dpiScale),
                Padding = new Padding(ScaleDimension(PopupPadding, dpiScale)),
                Back = tokens?.Surface ?? SystemColors.Window,
                ForeColor = tokens?.Foreground ?? SystemColors.WindowText,
                AutoContainerBgTransparent = true
            };

            var list = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = tokens?.Surface ?? SystemColors.Window,
                Padding = new Padding(0, 0, 0, ScaleDimension(4, dpiScale))
            };

            foreach (DataGridViewColumn column in _grid.Columns)
            {
                var checkBox = new AntdUI.Checkbox
                {
                    Text = column.HeaderText,
                    Checked = column.Visible,
                    AutoCheck = true,
                    AutoSize = false,
                    Size = new Size(popupWidth - ScaleDimension(40, dpiScale), rowHeight),
                    Tag = column.Name,
                    TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = tokens?.Foreground ?? SystemColors.WindowText,
                    Fill = tokens?.Primary ?? SystemColors.Highlight
                };

                checkBox.CheckedChanged += ColumnVisibilityChanged;
                list.Controls.Add(checkBox);
            }

            var footer = new System.Windows.Forms.Panel
            {
                Dock = DockStyle.Bottom,
                Height = footerHeight,
                BackColor = tokens?.Surface ?? SystemColors.Window
            };

            var restoreButton = new AntdUI.Button
            {
                Text = "恢复原始",
                Dock = DockStyle.Right,
                Width = ScaleDimension(ButtonWidth, dpiScale),
                Type = AntdUI.TTypeMini.Default,
                Radius = ScaleDimension(6, dpiScale),
                BackColor = tokens?.Surface ?? SystemColors.Window,
                BackHover = tokens?.Hover ?? SystemColors.ControlLight,
                BackActive = tokens?.Active ?? SystemColors.ControlDark,
                ForeColor = tokens?.Foreground ?? SystemColors.WindowText,
                ForeHover = tokens?.Foreground ?? SystemColors.WindowText,
                ForeActive = tokens?.Foreground ?? SystemColors.WindowText,
                DefaultBack = tokens?.Surface ?? SystemColors.Window,
                DefaultBorderColor = tokens?.Border ?? SystemColors.WindowFrame,
                BorderWidth = 1
            };
            restoreButton.Click += (sender, args) => _restoreDefaults();

            var saveButton = new AntdUI.Button
            {
                Text = "保存配置",
                Dock = DockStyle.Right,
                Width = ScaleDimension(ButtonWidth, dpiScale),
                Type = AntdUI.TTypeMini.Primary,
                Radius = ScaleDimension(6, dpiScale),
                BackColor = tokens?.Primary ?? SystemColors.Highlight,
                BackHover = tokens?.Hover ?? SystemColors.ControlLight,
                BackActive = tokens?.Active ?? SystemColors.ControlDark,
                ForeColor = tokens?.Surface ?? SystemColors.Window,
                ForeHover = tokens?.Surface ?? SystemColors.Window,
                ForeActive = tokens?.Surface ?? SystemColors.Window
            };
            saveButton.Click += (sender, args) => _saveSettings();

            footer.Controls.Add(restoreButton);
            footer.Controls.Add(new System.Windows.Forms.Panel
            {
                Dock = DockStyle.Right,
                Width = ScaleDimension(8, dpiScale)
            });
            footer.Controls.Add(saveButton);

            content.Controls.Add(list);
            content.Controls.Add(footer);
            return content;
        }

        internal static float GetDpiScale(Control control)
        {
            if (control == null)
            {
                return 1F;
            }

            var deviceDpi = control.DeviceDpi;
            return deviceDpi > 0 ? deviceDpi / (float)BaseDpi : 1F;
        }

        internal static int ScaleDimension(int value, float dpiScale)
        {
            return Math.Max(1, (int)Math.Round(value * Math.Max(1F, dpiScale)));
        }

        /// <summary>
        /// 将气泡锚点限制在工作区内。X 轴按气泡宽度预留中心对齐空间，Y 轴交给 AntdUI 根据箭头方向上下翻转。
        /// </summary>
        internal static Point ClampAnchorToWorkingArea(Point anchor, Size popupSize, Rectangle workingArea)
        {
            if (workingArea.Width <= 0 || workingArea.Height <= 0)
            {
                return anchor;
            }

            var halfWidth = popupSize.Width / 2;
            var x = workingArea.Width <= popupSize.Width
                ? workingArea.Left + workingArea.Width / 2
                : Math.Max(
                    workingArea.Left + halfWidth,
                    Math.Min(workingArea.Right - (popupSize.Width - halfWidth), anchor.X));
            var y = Math.Max(workingArea.Top, Math.Min(workingArea.Bottom, anchor.Y));
            return new Point(x, y);
        }

        private void ColumnVisibilityChanged(object sender, EventArgs args)
        {
            if (!(sender is AntdUI.Checkbox checkBox) || !(checkBox.Tag is string columnName))
            {
                return;
            }

            var column = _grid.Columns[columnName];
            if (column != null)
            {
                column.Visible = checkBox.Checked;
            }
        }

        private static PopupThemeTokens GetCurrentTokens()
        {
            try
            {
                var theme = Services.ServiceLocator.Instance.GetThemeManager()?.GetCurrentTheme();
                if (theme != null)
                {
                    return AntdUiThemeBridge.CurrentTokens ?? PopupThemeTokens.FromTheme(theme);
                }
            }
            catch
            {
                // 弹层仍可使用系统色，避免主题服务不可用时阻断列管理。
            }

            return AntdUiThemeBridge.CurrentTokens;
        }
    }
}
