using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp3.UI
{
    /// <summary>
    /// 右键菜单的展示契约。调用方负责在显示前确定目标、状态和命令。
    /// </summary>
    public sealed class ContextMenuRequest
    {
        public ContextMenuRequest(Control target, IReadOnlyList<ContextMenuItemSpec> items)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public Control Target { get; }

        public IReadOnlyList<ContextMenuItemSpec> Items { get; }

        /// <summary>
        /// 菜单锚点的目标控件客户区坐标。渲染器会统一转换为屏幕坐标后交给 AntdUI。
        /// </summary>
        public Point? Location { get; set; }

        /// <summary>
        /// 鼠标右键触发时使用系统物理鼠标位置，避免 DPI 虚拟化坐标偏移。
        /// </summary>
        public bool UseMousePosition { get; set; }

        public AntdUI.TAMode ColorScheme { get; set; } = AntdUI.TAMode.Auto;

        public int Radius { get; set; } = 8;

        /// <summary>
        /// 判断是否为 Windows 标准的键盘右键菜单快捷键。
        /// </summary>
        public static bool IsKeyboardInvocation(Keys keyCode, Keys modifiers)
        {
            return (keyCode == Keys.Apps && modifiers == Keys.None) ||
                (keyCode == Keys.F10 && modifiers == Keys.Shift);
        }

        /// <summary>
        /// 返回右键事件在目标控件客户区内的精确坐标。
        /// </summary>
        public static Point GetMouseInvocationLocation(int x, int y)
        {
            return new Point(x, y);
        }

        /// <summary>
        /// 获取键盘触发右键菜单时的客户端锚点，优先使用当前或已选单元格。
        /// </summary>
        public static Point GetKeyboardInvocationLocation(Control target)
        {
            if (target is DataGridView dataGridView)
            {
                var cell = dataGridView.CurrentCell ?? dataGridView.SelectedCells.Cast<DataGridViewCell>().FirstOrDefault();
                if (cell != null && cell.RowIndex >= 0 && cell.ColumnIndex >= 0)
                {
                    var bounds = dataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true);
                    if (!bounds.IsEmpty)
                    {
                        return new Point(Math.Max(0, bounds.Left), Math.Max(0, bounds.Bottom));
                    }
                }
            }

            return new Point(
                Math.Max(0, target.ClientSize.Width / 2),
                Math.Max(0, target.ClientSize.Height / 2));
        }
    }

    /// <summary>
    /// 单个右键菜单项的展示状态。它不保存或执行任何业务命令。
    /// </summary>
    public sealed class ContextMenuItemSpec
    {
        public ContextMenuItemSpec(string id, string text)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("菜单项 ID 不能为空。", nameof(id));
            }

            Id = id;
            Text = text ?? string.Empty;
        }

        private ContextMenuItemSpec()
        {
            IsDivider = true;
            Id = string.Empty;
            Text = string.Empty;
        }

        public string Id { get; }

        public string Text { get; }

        public string ShortcutText { get; set; }

        public Image Icon { get; set; }

        public string IconSvg { get; set; }

        public bool Enabled { get; set; } = true;

        public bool Checked { get; set; }

        public bool IsDangerous { get; set; }

        public bool IsDivider { get; }

        public object Tag { get; set; }

        public IReadOnlyList<ContextMenuItemSpec> Items { get; set; } = Array.Empty<ContextMenuItemSpec>();

        public static ContextMenuItemSpec Divider()
        {
            return new ContextMenuItemSpec();
        }
    }
}
