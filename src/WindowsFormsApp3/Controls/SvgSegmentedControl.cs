using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Svg;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Controls
{
    /// <summary>
    /// 支持SVG图标的分段控件
    /// </summary>
    [Serializable]
    public class SvgSegmentedControl : Control
    {
        private List<SvgSegmentItem> _items = new List<SvgSegmentItem>();
        private int _selectedIndex = 0;
        private int _hoverIndex = -1;
        private Color _selectedColor = Color.FromArgb(52, 152, 219);
        private Color _unselectedColor = Color.FromArgb(240, 240, 240);
        private Color _borderColor = Color.FromArgb(200, 200, 200);
        private int _segmentSpacing = 1;

        public List<SvgSegmentItem> Items
        {
            get => _items;
            set
            {
                _items = value ?? new List<SvgSegmentItem>();
                Invalidate();
            }
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex != value && value >= 0 && value < _items.Count)
                {
                    _selectedIndex = value;
                    Invalidate();
                    SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public SvgSegmentItem SelectedItem => _selectedIndex >= 0 && _selectedIndex < _items.Count ? _items[_selectedIndex] : null;

        public Color SelectedColor
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                Invalidate();
            }
        }

        public Color UnselectedColor
        {
            get => _unselectedColor;
            set
            {
                _unselectedColor = value;
                Invalidate();
            }
        }

        public event EventHandler SelectedIndexChanged;

        public SvgSegmentedControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            Cursor = Cursors.Hand;
            Font = new Font("Microsoft YaHei UI", 9F);
            InitializeDefaultItems();
        }

        private void InitializeDefaultItems()
        {
            _items = new List<SvgSegmentItem>
            {
                new SvgSegmentItem { SvgPath = SvgIconLibrary.Sparkle, Text = "光膜", Tag = "光膜" },
                new SvgSegmentItem { SvgPath = SvgIconLibrary.Moon, Text = "哑膜", Tag = "哑膜" },
                new SvgSegmentItem { SvgPath = SvgIconLibrary.Prohibited, Text = "不过膜", Tag = "不过膜" },
                new SvgSegmentItem { SvgPath = SvgIconLibrary.RedCircle, Text = "红膜", Tag = "红膜" }
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (_items.Count == 0) return;

            float segmentWidth = (float)(Width - _segmentSpacing * (_items.Count - 1)) / _items.Count;
            float currentX = 0;

            for (int i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                RectangleF segmentRect = new RectangleF(currentX, 0, segmentWidth, Height);

                // 绘制背景
                Color bgColor = i == _selectedIndex ? _selectedColor : _unselectedColor;
                if (_hoverIndex == i)
                {
                    // 鼠标悬停时稍微改变颜色
                    bgColor = ControlPaint.Light(bgColor, 0.1f);
                }

                using (Brush brush = new SolidBrush(bgColor))
                {
                    g.FillRectangle(brush, segmentRect);
                }

                // 绘制SVG图标
                if (!string.IsNullOrEmpty(item.SvgPath))
                {
                    try
                    {
                        var svg = SvgDocument.FromSvg<SvgDocument>(item.SvgPath);
                        if (svg != null)
                        {
                            // 设置图标尺寸
                            int iconSize = 16;
                            svg.Width = iconSize;
                            svg.Height = iconSize;

                            // 渲染SVG
                            using (Bitmap svgBitmap = svg.Draw())
                            {
                                if (svgBitmap != null)
                                {
                                    // 计算图标位置 (左侧居中)
                                    float iconX = segmentRect.X + 8;
                                    float iconY = segmentRect.Y + (segmentRect.Height - iconSize) / 2;

                                    g.DrawImage(svgBitmap, iconX, iconY, iconSize, iconSize);
                                }
                            }
                        }
                    }
                    catch
                    {
                        // SVG渲染失败时绘制简单图标
                        DrawFallbackIcon(g, segmentRect, i);
                    }
                }

                // 绘制文本
                if (!string.IsNullOrEmpty(item.Text))
                {
                    SizeF textSize = g.MeasureString(item.Text, Font);
                    float textX = segmentRect.X + 8 + 16 + 4; // 左边距 + 图标宽度 + 间距
                    float textY = segmentRect.Y + (segmentRect.Height - textSize.Height) / 2;

                    Color textColor = i == _selectedIndex ? Color.White : Color.Black;
                    using (Brush textBrush = new SolidBrush(textColor))
                    {
                        g.DrawString(item.Text, Font, textBrush, textX, textY);
                    }
                }

                currentX += segmentWidth + _segmentSpacing;
            }

            // 绘制外边框
            using (Pen pen = new Pen(_borderColor))
            {
                g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            }
        }

        private void DrawFallbackIcon(Graphics g, RectangleF rect, int index)
        {
            RectangleF iconRect = new RectangleF(rect.X + 8, rect.Y + (rect.Height - 16) / 2, 16, 16);
            Color iconColor = index == _selectedIndex ? Color.White : Color.Gray;

            using (Brush brush = new SolidBrush(iconColor))
            {
                // 根据索引绘制不同的简单图标
                switch (index)
                {
                    case 0: // 光膜 - 星形
                        g.FillPolygon(brush, GetStarPoints(iconRect));
                        break;
                    case 1: // 哑膜 - 圆形
                        g.FillEllipse(brush, iconRect);
                        break;
                    case 2: // 不过膜 - 叉号
                        g.FillRectangle(brush, iconRect);
                        break;
                    case 3: // 红膜 - 红色圆形
                        g.FillEllipse(brush, iconRect);
                        break;
                }
            }
        }

        private PointF[] GetStarPoints(RectangleF rect)
        {
            PointF center = new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            float outerRadius = Math.Min(rect.Width, rect.Height) / 2;
            float innerRadius = outerRadius * 0.4f;

            PointF[] points = new PointF[10];
            for (int i = 0; i < 10; i++)
            {
                float angle = (float)((Math.PI / 5) * i - Math.PI / 2);
                float radius = i % 2 == 0 ? outerRadius : innerRadius;
                points[i] = new PointF(
                    center.X + (float)(Math.Cos(angle) * radius),
                    center.Y + (float)(Math.Sin(angle) * radius)
                );
            }
            return points;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (_items.Count == 0) return;

            float segmentWidth = (float)(Width - _segmentSpacing * (_items.Count - 1)) / _items.Count;
            int clickedIndex = (int)(e.X / (segmentWidth + _segmentSpacing));

            if (clickedIndex >= 0 && clickedIndex < _items.Count)
            {
                SelectedIndex = clickedIndex;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_items.Count == 0) return;

            float segmentWidth = (float)(Width - _segmentSpacing * (_items.Count - 1)) / _items.Count;
            int newHoverIndex = (int)(e.X / (segmentWidth + _segmentSpacing));

            if (newHoverIndex != _hoverIndex)
            {
                _hoverIndex = newHoverIndex;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hoverIndex = -1;
            Invalidate();
        }

        /// <summary>
        /// 根据Tag查找并选择项目
        /// </summary>
        public void SelectByTag(string tag)
        {
            var index = _items.FindIndex(item => item.Tag?.ToString() == tag);
            if (index >= 0)
            {
                SelectedIndex = index;
            }
        }
    }

    /// <summary>
    /// SVG分段控件项目
    /// </summary>
    [Serializable]
    public class SvgSegmentItem
    {
        public string SvgPath { get; set; } = "";
        public string Text { get; set; } = "";
        public object Tag { get; set; }
    }
}