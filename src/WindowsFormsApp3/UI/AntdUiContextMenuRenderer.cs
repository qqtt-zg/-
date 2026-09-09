using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp3.UI
{
    /// <summary>
    /// 将调用方提供的菜单展示契约映射到 AntdUI，不承担命令或状态管理职责。
    /// </summary>
    public static class AntdUiContextMenuRenderer
    {
        public static Form Show(ContextMenuRequest request, Action<ContextMenuItemSpec> onItemSelected)
        {
            return AntdUI.ContextMenuStrip.open(CreateConfig(request, onItemSelected));
        }

        public static AntdUI.ContextMenuStrip.Config CreateConfig(
            ContextMenuRequest request,
            Action<ContextMenuItemSpec> onItemSelected)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var config = new AntdUI.ContextMenuStrip.Config(
                request.Target,
                item =>
                {
                    if (item?.Tag is ContextMenuItemSpec spec && !spec.IsDivider)
                    {
                        onItemSelected?.Invoke(spec);
                    }
                },
                BuildItems(request.Items),
                request.Radius)
            {
                // 调用方统一传入目标控件客户区坐标；AntdUI LayeredFormContextMenuStrip
                // 的 Location 契约是屏幕坐标，因此在适配层集中转换一次。
                Location = request.UseMousePosition
                    ? null
                    : ToScreenLocation(request.Target, request.Location),
                ColorScheme = request.ColorScheme,
                Radius = request.Radius,
                // 确保通过 Apps/Shift+F10 唤起时，方向键和 Enter 由菜单接管。
                UFocus = true
            };

            return config;
        }

        internal static Point? ToScreenLocation(Control target, Point? clientLocation)
        {
            if (target == null || !clientLocation.HasValue)
            {
                return null;
            }

            var logicalScreenLocation = target.PointToScreen(clientLocation.Value);
            // 在当前 .NET Framework DPI 兼容模式下，WinForms 会将该逻辑屏幕坐标
            // 交给分层窗体所在的 DPI 上下文转换一次；手动再乘 DPI 会导致二次缩放。
            return logicalScreenLocation;
        }

        public static AntdUI.IContextMenuStripItem[] BuildItems(IReadOnlyList<ContextMenuItemSpec> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            return items.Select(BuildItem).ToArray();
        }

        private static AntdUI.IContextMenuStripItem BuildItem(ContextMenuItemSpec item)
        {
            if (item == null)
            {
                throw new ArgumentException("菜单项不能为 null。", nameof(item));
            }

            if (item.IsDivider)
            {
                return new AntdUI.ContextMenuStripItemDivider();
            }

            return new AntdUI.ContextMenuStripItem(item.Text)
            {
                ID = item.Id,
                SubText = item.ShortcutText,
                Icon = item.Icon,
                IconSvg = item.IconSvg,
                Enabled = item.Enabled,
                Checked = item.Checked,
                Fore = item.IsDangerous ? AntdUiThemeBridge.CurrentTokens?.Danger : (Color?)null,
                Sub = BuildItems(item.Items),
                Tag = item
            };
        }
    }
}
