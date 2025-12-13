using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using Svg;

namespace WindowsFormsApp3.Controls
{
    /// <summary>
    /// 支持SVG图标的自定义按钮控件
    /// </summary>
    public class SvgIconButton : Control
    {
        private bool _isChecked = false;
        private string _checkedSvgPath = "";
        private string _uncheckedSvgPath = "";
        private string _checkedText = "彩色";
        private string _uncheckedText = "黑白";
        private Color _fillColor = Color.FromArgb(52, 152, 219);
        private Color _fillHoverColor = Color.FromArgb(41, 128, 185);
        private bool _isHovering = false;
        private SvgDocument _checkedSvg;
        private SvgDocument _uncheckedSvg;
        private int _iconSize = 16;

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
                LoadSvgDocuments();
                Invalidate();
            }
        }

        public string UncheckedSvgPath
        {
            get => _uncheckedSvgPath;
            set
            {
                _uncheckedSvgPath = value;
                LoadSvgDocuments();
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

        public Color FillColor
        {
            get => _fillColor;
            set
            {
                _fillColor = value;
                Invalidate();
            }
        }

        public Color FillHoverColor
        {
            get => _fillHoverColor;
            set
            {
                _fillHoverColor = value;
                Invalidate();
            }
        }

        public int IconSize
        {
            get => _iconSize;
            set
            {
                _iconSize = value;
                Invalidate();
            }
        }

        public event EventHandler IsCheckedChanged;

        public SvgIconButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            Cursor = Cursors.Hand;
            Font = new Font("Microsoft YaHei UI", 9F);
            InitializeDefaultSvgs();
        }

        private void InitializeDefaultSvgs()
        {
            // 彩虹图标SVG
            _checkedSvgPath = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24"" fill=""none"">
                <path d=""M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8z"" fill=""url(#rainbow)""/>
                <defs>
                    <linearGradient id=""rainbow"" x1=""0%"" y1=""0%"" x2=""100%"" y2=""100%"">
                        <stop offset=""0%"" style=""stop-color:#ff0000"" />
                        <stop offset=""16.66%"" style=""stop-color:#ff8800"" />
                        <stop offset=""33.33%"" style=""stop-color:#ffff00"" />
                        <stop offset=""50%"" style=""stop-color:#00ff00"" />
                        <stop offset=""66.66%"" style=""stop-color:#0088ff"" />
                        <stop offset=""83.33%"" style=""stop-color:#0000ff"" />
                        <stop offset=""100%"" style=""stop-color:#8800ff"" />
                    </linearGradient>
                </defs>
            </svg>";

            // 黑色圆圈图标SVG
            _uncheckedSvgPath = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24"" fill=""none"">
                <circle cx=""12"" cy=""12"" r=""8"" fill=""#000000""/>
            </svg>";

            LoadSvgDocuments();
        }

        private void LoadSvgDocuments()
        {
            try
            {
                if (!string.IsNullOrEmpty(_checkedSvgPath))
                {
                    _checkedSvg = SvgDocument.FromSvg<SvgDocument>(_checkedSvgPath);
                }
                if (!string.IsNullOrEmpty(_uncheckedSvgPath))
                {
                    _uncheckedSvg = SvgDocument.FromSvg<SvgDocument>(_uncheckedSvgPath);
                }
            }
            catch
            {
                // 如果SVG加载失败，创建简单的图形作为备用
                _checkedSvg = CreateFallbackSvg(true);
                _uncheckedSvg = CreateFallbackSvg(false);
            }
        }

        private SvgDocument CreateFallbackSvg(bool isChecked)
        {
            var svg = new SvgDocument();
            svg.ViewBox = new SvgViewBox(0, 0, 24, 24);
            svg.Width = 24;
            svg.Height = 24;

            if (isChecked)
            {
                // 创建彩虹渐变圆形
                var circle = new SvgCircle()
                {
                    CenterX = 12,
                    CenterY = 12,
                    Radius = 8,
                    Fill = new SvgColourServer(Color.FromArgb(255, 100, 100))
                };
                svg.Children.Add(circle);
            }
            else
            {
                // 创建黑色圆形
                var circle = new SvgCircle()
                {
                    CenterX = 12,
                    CenterY = 12,
                    Radius = 8,
                    Fill = new SvgColourServer(Color.Black)
                };
                svg.Children.Add(circle);
            }

            return svg;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle bounds = new Rectangle(0, 0, Width, Height);
            Color currentColor = _isHovering ? _fillHoverColor : _fillColor;

            // 绘制背景
            using (Brush brush = new SolidBrush(currentColor))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框
            using (Pen pen = new Pen(Color.FromArgb(200, 200, 200)))
            {
                g.DrawRectangle(pen, bounds);
            }

            // 绘制SVG图标
            SvgDocument currentSvg = _isChecked ? _checkedSvg : _uncheckedSvg;
            if (currentSvg != null)
            {
                try
                {
                    // 设置SVG尺寸
                    currentSvg.Width = _iconSize;
                    currentSvg.Height = _iconSize;

                    // 渲染SVG到位图
                    using (Bitmap svgBitmap = currentSvg.Draw())
                    {
                        if (svgBitmap != null)
                        {
                            // 计算图标位置
                            int iconX = 10;
                            int iconY = (Height - _iconSize) / 2;

                            // 绘制图标
                            g.DrawImage(svgBitmap, iconX, iconY, _iconSize, _iconSize);
                        }
                    }
                }
                catch
                {
                    // 如果SVG渲染失败，绘制简单的备用图形
                    DrawFallbackIcon(g, _isChecked);
                }
            }

            // 绘制文本
            string text = _isChecked ? _checkedText : _uncheckedText;
            SizeF textSize = g.MeasureString(text, Font);
            PointF textLocation = new PointF(
                10 + _iconSize + 5, // 图标左边距 + 图标宽度 + 间距
                (Height - textSize.Height) / 2
            );

            using (Brush textBrush = new SolidBrush(ForeColor))
            {
                g.DrawString(text, Font, textBrush, textLocation);
            }
        }

        private void DrawFallbackIcon(Graphics g, bool isChecked)
        {
            Rectangle iconRect = new Rectangle(10, (Height - _iconSize) / 2, _iconSize, _iconSize);

            if (isChecked)
            {
                // 绘制彩虹圆形
                using (Brush brush = new SolidBrush(Color.FromArgb(255, 100, 100)))
                {
                    g.FillEllipse(brush, iconRect);
                }
            }
            else
            {
                // 绘制黑色圆形
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    g.FillEllipse(brush, iconRect);
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