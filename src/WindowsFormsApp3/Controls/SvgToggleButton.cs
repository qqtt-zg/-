using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WindowsFormsApp3.Utils;
using Svg;

namespace WindowsFormsApp3.Controls
{
    /// <summary>
    /// 支持SVG图标的切换按钮（模拟Switch效果）
    /// </summary>
    public class SvgToggleButton : Control
    {
        private bool _isChecked = false;
        private string _checkedSvgPath = "";
        private string _uncheckedSvgPath = "";
        private string _checkedText = "";
        private string _uncheckedText = "";
        private Color _checkedBackColor = Color.FromArgb(52, 152, 219);
        private Color _uncheckedBackColor = Color.FromArgb(240, 240, 240);
        private Color _checkedForeColor = Color.White;
        private Color _uncheckedForeColor = Color.Black;
        private bool _isHovering = false;
        private int _iconSize = 20;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    Invalidate();
                    IsCheckedChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public string CheckedSvgPath
        {
            get => _checkedSvgPath;
            set
            {
                _checkedSvgPath = value;
                Invalidate();
            }
        }

        public string UncheckedSvgPath
        {
            get => _uncheckedSvgPath;
            set
            {
                _uncheckedSvgPath = value;
                Invalidate();
            }
        }

        public string CheckedText
        {
            get => _checkedText;
            set
            {
                _checkedText = value;
                Invalidate();
            }
        }

        public string UncheckedText
        {
            get => _uncheckedText;
            set
            {
                _uncheckedText = value;
                Invalidate();
            }
        }

        public Color CheckedBackColor
        {
            get => _checkedBackColor;
            set
            {
                _checkedBackColor = value;
                Invalidate();
            }
        }

        public Color UncheckedBackColor
        {
            get => _uncheckedBackColor;
            set
            {
                _uncheckedBackColor = value;
                Invalidate();
            }
        }

        public event EventHandler IsCheckedChanged;

        public SvgToggleButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            Cursor = Cursors.Hand;
            Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            InitializeDefaultValues();
        }

        private void InitializeDefaultValues()
        {
            _checkedSvgPath = ExternalSvgIconLibrary.LoadColorMode("rainbow");
            _uncheckedSvgPath = ExternalSvgIconLibrary.LoadColorMode("black-circle");
            _checkedText = "彩色";
            _uncheckedText = "黑白";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle bounds = new Rectangle(0, 0, Width, Height);

            // 根据状态选择颜色
            Color bgColor = _isChecked ? _checkedBackColor : _uncheckedBackColor;
            Color textColor = _isChecked ? _checkedForeColor : _uncheckedForeColor;
            string text = _isChecked ? _checkedText : _uncheckedText;
            string svgPath = _isChecked ? _checkedSvgPath : _uncheckedSvgPath;

            // 绘制背景
            if (_isHovering)
            {
                bgColor = ControlPaint.Light(bgColor, 0.1f);
            }

            using (Brush bgBrush = new SolidBrush(bgColor))
            {
                g.FillRectangle(bgBrush, bounds);
            }

            // 绘制圆角
            int cornerRadius = 4;
            using (GraphicsPath path = CreateRoundedRectanglePath(bounds, cornerRadius))
            {
                using (Pen borderPen = new Pen(Color.FromArgb(200, 200, 200)))
                {
                    g.DrawPath(borderPen, path);
                }
            }

            // 计算内容布局
            SizeF textSize = g.MeasureString(text, Font);
            int totalWidth = _iconSize + 8 + (int)textSize.Width;
            int startX = (Width - totalWidth) / 2;
            int startY = (Height - _iconSize) / 2;

            // 绘制SVG图标
            if (!string.IsNullOrEmpty(svgPath))
            {
                try
                {
                    var svg = SvgDocument.FromSvg<SvgDocument>(svgPath);
                    if (svg != null)
                    {
                        svg.Width = _iconSize;
                        svg.Height = _iconSize;

                        using (Bitmap svgBitmap = svg.Draw())
                        {
                            if (svgBitmap != null)
                            {
                                g.DrawImage(svgBitmap, startX, startY, _iconSize, _iconSize);
                            }
                        }
                    }
                }
                catch
                {
                    // SVG渲染失败时绘制简单图标
                    DrawFallbackIcon(g, new Rectangle(startX, startY, _iconSize, _iconSize), _isChecked);
                }
            }

            // 绘制文本
            PointF textLocation = new PointF(
                startX + _iconSize + 8,
                (Height - textSize.Height) / 2
            );

            using (Brush textBrush = new SolidBrush(textColor))
            {
                g.DrawString(text, Font, textBrush, textLocation);
            }
        }

        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            // 左上角
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            // 右上角
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            // 右下角
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            // 左下角
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void DrawFallbackIcon(Graphics g, Rectangle iconRect, bool isChecked)
        {
            Color iconColor = isChecked ? Color.FromArgb(255, 100, 100) : Color.Black;
            using (Brush iconBrush = new SolidBrush(iconColor))
            {
                if (isChecked)
                {
                    // 绘制彩虹色圆形（简化版）
                    g.FillEllipse(iconBrush, iconRect);
                }
                else
                {
                    // 绘制黑色圆形
                    g.FillEllipse(iconBrush, iconRect);
                }
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            IsChecked = !IsChecked;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovering = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovering = false;
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
    }
}