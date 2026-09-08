using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp3.Models;

namespace WindowsFormsApp3.Forms.Controls
{
    /// <summary>
    /// 方案五：智能工艺分组卡片头部控件（用于统一呈现公共工艺参数）
    /// </summary>
    public class BatchGroupHeaderCard : Panel
    {
        private readonly BatchProcessGroup _group;
        public BatchProcessGroup Group => _group;

        public event EventHandler HeaderClicked;
        public event EventHandler LockBadgeClicked;

        private Rectangle _lockBadgeRect;

        public BatchGroupHeaderCard(BatchProcessGroup group)
        {
            _group = group ?? throw new ArgumentNullException(nameof(group));
            this.Dock = DockStyle.Top;
            this.Height = 36;
            this.Padding = new Padding(6, 4, 6, 4);
            this.DoubleBuffered = true;
            this.Cursor = Cursors.Hand;
            this.MouseDown += BatchGroupHeaderCard_MouseDown;

            UpdateCardStyle();
        }

        private void BatchGroupHeaderCard_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_lockBadgeRect.Contains(e.Location))
                {
                    LockBadgeClicked?.Invoke(this, EventArgs.Empty);
                    return;
                }
                HeaderClicked?.Invoke(this, EventArgs.Empty);
            }
        }

        public void UpdateCardStyle()
        {
            if (_group.IsPreserveGroup)
            {
                this.BackColor = Color.FromArgb(249, 240, 255); // 返单淡紫色
            }
            else if (_group.IsLocked)
            {
                this.BackColor = Color.FromArgb(245, 245, 245);
            }
            else
            {
                this.BackColor = Color.FromArgb(240, 247, 255); // 新单浅蓝色
            }
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 边框
            Color borderColor = _group.IsPreserveGroup 
                ? Color.FromArgb(211, 173, 247) 
                : (_group.IsLocked ? Color.FromArgb(217, 217, 217) : Color.FromArgb(145, 202, 255));
            using (var pen = new Pen(borderColor, 1F))
            {
                g.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            }

            int centerY = this.Height / 2;

            // 1. 先计算并绘制右侧状态指示标签（去除了无法渲染的 Emoji，纯净文字避免乱码方块）
            string statusText = (_group.IsPreserveGroup || _group.IsLocked) ? "已锁定" : "联动";
            Color statusBg = (_group.IsPreserveGroup || _group.IsLocked) ? Color.FromArgb(249, 240, 255) : Color.FromArgb(246, 255, 237);
            Color statusFg = (_group.IsPreserveGroup || _group.IsLocked) ? Color.FromArgb(114, 46, 209) : Color.FromArgb(82, 196, 26);
            Color statusBorder = (_group.IsPreserveGroup || _group.IsLocked) ? Color.FromArgb(211, 173, 247) : Color.FromArgb(183, 235, 143);

            int sW = 54;
            using (var statFont = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold))
            {
                var sSize = g.MeasureString(statusText, statFont);
                sW = (int)Math.Ceiling(sSize.Width) + 12;
                int sH = 20;
                int sX = this.Width - sW - 6;
                int sY = centerY - (sH / 2);

                _lockBadgeRect = new Rectangle(sX, sY, sW, sH);
                using (var sbBrush = new SolidBrush(statusBg))
                {
                    g.FillRectangle(sbBrush, sX, sY, sW, sH);
                }
                using (var pen = new Pen(statusBorder, 1F))
                {
                    g.DrawRectangle(pen, sX, sY, sW, sH);
                }
                using (var textBrush = new SolidBrush(statusFg))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center,
                        FormatFlags = StringFormatFlags.NoWrap
                    };
                    g.DrawString(statusText, statFont, textBrush, new RectangleF(sX, sY, sW, sH), sf);
                }
            }

            // 左侧绘制区域边界限制
            int rightBoundary = this.Width - sW - 10;
            int curX = 6;

            // 2. 折叠箭头
            string arrow = _group.IsCollapsed ? "▸" : "▾";
            using (var arrowFont = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.FromArgb(80, 80, 80)))
            {
                g.DrawString(arrow, arrowFont, brush, curX, centerY - 7);
                curX += 13;
            }

            // 3. 组标题（如【一组】、【二组】，简短紧凑）
            Color titleColor = _group.IsPreserveGroup 
                ? Color.FromArgb(114, 46, 209) 
                : (_group.IsLocked ? Color.FromArgb(38, 38, 38) : Color.FromArgb(9, 88, 217));
            using (var titleFont = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold))
            using (var brush = new SolidBrush(titleColor))
            {
                string title = _group.GroupName ?? "【一组】";
                g.DrawString(title, titleFont, brush, curX, centerY - 7);
                var size = g.MeasureString(title, titleFont);
                curX += (int)Math.Ceiling(size.Width) + 4;
            }

            // 4. 款数徽章
            int count = _group.Items?.Count ?? 0;
            string countText = _group.IsPreserveGroup ? $"{count}款返单" : $"{count}款新单";
            using (var countFont = new Font("Microsoft YaHei UI", 7.5F))
            {
                var cSize = g.MeasureString(countText, countFont);
                int badgeW = (int)Math.Ceiling(cSize.Width) + 6;
                int badgeH = 17;
                int badgeY = centerY - (badgeH / 2);
                Color badgeBg = _group.IsPreserveGroup ? Color.FromArgb(114, 46, 209) : Color.FromArgb(22, 119, 255);
                using (var bgBrush = new SolidBrush(badgeBg))
                {
                    g.FillRectangle(bgBrush, curX, badgeY, badgeW, badgeH);
                }
                using (var textBrush = new SolidBrush(Color.White))
                {
                    g.DrawString(countText, countFont, textBrush, curX + 3, badgeY + 1);
                }
                curX += badgeW + 5;
            }

            // 5. 公共工艺参数胶囊列表（全部解耦为独立胶囊，无颜文字乱码）
            if (_group.IsPreserveGroup)
            {
                DrawPill(g, ref curX, centerY, rightBoundary, "返单前缀", 
                    Color.FromArgb(249, 240, 255), Color.FromArgb(114, 46, 209), Color.FromArgb(211, 173, 247));
            }

            // 独立胶囊：材料
            string matText = string.IsNullOrEmpty(_group.Material) ? "未指材料" : _group.Material;
            DrawPill(g, ref curX, centerY, rightBoundary, matText, 
                _group.IsPreserveGroup ? Color.FromArgb(249, 240, 255) : Color.FromArgb(255, 247, 230), 
                _group.IsPreserveGroup ? Color.FromArgb(114, 46, 209) : Color.FromArgb(212, 107, 8), 
                _group.IsPreserveGroup ? Color.FromArgb(211, 173, 247) : Color.FromArgb(255, 213, 145));

            // 独立胶囊：颜色模式（黑白 / 彩色）
            string colorMode = _group.ColorMode;
            string filmType = _group.FilmType;
            if (string.IsNullOrEmpty(colorMode) && string.IsNullOrEmpty(filmType) && !string.IsNullOrEmpty(_group.Process))
            {
                // 兼容老数据从 Process 解析
                if (_group.Process.Contains("黑白"))
                {
                    colorMode = "黑白";
                    filmType = _group.Process.Replace("黑白", "").Trim();
                }
                else if (_group.Process.Contains("彩色"))
                {
                    colorMode = "彩色";
                    filmType = _group.Process.Replace("彩色", "").Trim();
                }
                else
                {
                    filmType = _group.Process;
                }
            }
            if (string.IsNullOrEmpty(colorMode)) colorMode = "黑白";
            DrawPill(g, ref curX, centerY, rightBoundary, colorMode, 
                Color.FromArgb(245, 245, 245), Color.FromArgb(89, 89, 89), Color.FromArgb(217, 217, 217));

            // 独立胶囊：膜类型（光膜 / 哑膜 / 红膜 / 不过膜）
            if (!string.IsNullOrEmpty(filmType))
            {
                DrawPill(g, ref curX, centerY, rightBoundary, filmType, 
                    Color.FromArgb(246, 255, 237), Color.FromArgb(56, 158, 13), Color.FromArgb(183, 235, 143));
            }

            // 独立胶囊：切刀形状（直角 / 圆角R5 / 异形）
            string shapeName = "直角";
            if (_group.Shape == "Round" || _group.Shape == "圆角" || _group.Shape == "RoundRect")
            {
                shapeName = (!string.IsNullOrEmpty(_group.RoundRadius) && _group.RoundRadius != "0") ? $"圆角R{_group.RoundRadius}" : "圆角";
            }
            else if (_group.Shape == "Special" || _group.Shape == "异形")
            {
                shapeName = "异形";
            }
            DrawPill(g, ref curX, centerY, rightBoundary, shapeName, 
                Color.FromArgb(246, 255, 237), Color.FromArgb(56, 158, 13), Color.FromArgb(183, 235, 143));

            // 独立胶囊：材料类型（卷装 / 平张）
            string matType = !string.IsNullOrEmpty(_group.MaterialType) ? _group.MaterialType : "";
            string layPattern = !string.IsNullOrEmpty(_group.LayoutPattern) ? _group.LayoutPattern : "";
            if (string.IsNullOrEmpty(matType) && string.IsNullOrEmpty(layPattern) && !string.IsNullOrEmpty(_group.ImpositionMode))
            {
                var parts = _group.ImpositionMode.Split(new[] { '·', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 0) matType = parts[0];
                if (parts.Length > 1) layPattern = parts[1];
            }
            if (string.IsNullOrEmpty(matType)) matType = "平张";
            if (string.IsNullOrEmpty(layPattern)) layPattern = "连拼";

            DrawPill(g, ref curX, centerY, rightBoundary, matType, 
                Color.FromArgb(230, 255, 251), Color.FromArgb(8, 151, 156), Color.FromArgb(135, 232, 222));

            // 独立胶囊：排版模式（折手 / 连拼）
            DrawPill(g, ref curX, centerY, rightBoundary, layPattern, 
                Color.FromArgb(230, 255, 251), Color.FromArgb(8, 151, 156), Color.FromArgb(135, 232, 222));

            // 独立胶囊：目标路径（去除无法显示的 Emoji，最多显示三级层级）
            string pathText = "根目录";
            if (!string.IsNullOrEmpty(_group.ExportPath))
            {
                try
                {
                    pathText = FormatExportPathSummary(_group.ExportPath);
                }
                catch
                {
                    pathText = _group.ExportPath;
                }
            }
            DrawPill(g, ref curX, centerY, rightBoundary, pathText, 
                Color.FromArgb(240, 245, 255), Color.FromArgb(47, 84, 235), Color.FromArgb(179, 199, 255));
        }

        private void DrawPill(Graphics g, ref int curX, int centerY, int rightBoundary, string text, Color bg, Color fg, Color border)
        {
            if (string.IsNullOrEmpty(text)) return;
            using (var font = new Font("Microsoft YaHei UI", 7.5F, FontStyle.Bold))
            {
                var size = g.MeasureString(text, font);
                int pillW = (int)Math.Ceiling(size.Width) + 6;
                int pillH = 18;
                int pillY = centerY - (pillH / 2);

                if (curX + pillW > rightBoundary)
                {
                    int availW = rightBoundary - curX;
                    if (availW < 18) return;
                    pillW = availW;
                }

                using (var brush = new SolidBrush(bg))
                {
                    g.FillRectangle(brush, curX, pillY, pillW, pillH);
                }
                using (var pen = new Pen(border, 1F))
                {
                    g.DrawRectangle(pen, curX, pillY, pillW, pillH);
                }
                using (var textBrush = new SolidBrush(fg))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.EllipsisCharacter,
                        FormatFlags = StringFormatFlags.NoWrap
                    };
                    g.DrawString(text, font, textBrush, new RectangleF(curX, pillY, pillW, pillH), sf);
                }
                curX += pillW + 4;
            }
        }

        /// <summary>
        /// 将完整路径格式化为最多三级显示（如 威立德→卷装→折手KM）
        /// </summary>
        public static string FormatExportPathSummary(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath)) return "根目录";
            try
            {
                string p = fullPath.Replace('/', '\\').TrimEnd('\\');

                List<string> configuredRoots = null;
                try
                {
                    configuredRoots = WindowsFormsApp3.Utils.AppSettings.ExportPaths;
                }
                catch { }
                if (configuredRoots != null && configuredRoots.Count > 0)
                {
                    foreach (var root in configuredRoots)
                    {
                        if (string.IsNullOrWhiteSpace(root)) continue;
                        string r = root.Replace('/', '\\').TrimEnd('\\');
                        if (p.StartsWith(r, StringComparison.OrdinalIgnoreCase))
                        {
                            string rel = p.Substring(r.Length).Trim('\\');
                            if (!string.IsNullOrEmpty(rel))
                            {
                                var segs = rel.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                                if (segs.Length > 3)
                                {
                                    segs = segs.Skip(segs.Length - 3).ToArray();
                                }
                                return string.Join("→", segs);
                            }
                        }
                    }
                }

                // 兜底：去除盘符后提取末尾最多三级
                int colonIdx = p.IndexOf(':');
                string rest = colonIdx >= 0 ? p.Substring(colonIdx + 1) : p;
                var parts = rest.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 3)
                {
                    parts = parts.Skip(parts.Length - 3).ToArray();
                }

                if (parts.Length > 0)
                {
                    return string.Join("→", parts);
                }

                return fullPath;
            }
            catch
            {
                return fullPath;
            }
        }
    }
}
