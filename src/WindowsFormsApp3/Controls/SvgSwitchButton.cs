using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WindowsFormsApp3.Utils;
using Svg;

namespace WindowsFormsApp3.Controls
{
    /// <summary>
    /// 支持SVG图标的自定义开关控件
    /// </summary>
    public class SvgSwitchButton : Control
    {
        private bool _isChecked = false;
        private string _checkedSvgPath = "";
        private string _uncheckedSvgPath = "";
        private string _checkedText = "";
        private string _uncheckedText = "";
        private Color _checkedColor = Color.FromArgb(52, 152, 219);
        private Color _uncheckedColor = Color.FromArgb(200, 200, 200);
        private Color _checkedHoverColor = Color.FromArgb(41, 128, 185);
        private Color _uncheckedHoverColor = Color.FromArgb(180, 180, 180);
        private bool _isHovering = false;
        private int _thumbSize = 20;
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

        public Color CheckedColor
        {
            get => _checkedColor;
            set
            {
                _checkedColor = value;
                Invalidate();
            }
        }

        public Color UncheckedColor
        {
            get => _uncheckedColor;
            set
            {
                _uncheckedColor = value;
                Invalidate();
            }
        }

        public event EventHandler IsCheckedChanged;

        public SvgSwitchButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            Cursor = Cursors.Hand;
            Font = new Font("Microsoft YaHei UI", 9F);
            InitializeDefaultValues();
        }

        private void InitializeDefaultValues()
        {
            // 设置默认的SVG图标
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
            Color currentColor = _isChecked ?
                (_isHovering ? _checkedHoverColor : _checkedColor) :
                (_isHovering ? _uncheckedHoverColor : _uncheckedColor);

            // 绘制背景轨道
            Rectangle trackRect = new Rectangle(
                5,
                (Height - _thumbSize) / 2,
                Width - 10,
                _thumbSize
            );

            using (Brush trackBrush = new SolidBrush(currentColor))
            {
                g.FillRectangle(trackBrush, trackRect);
            }

            // 绘制边框
            using (Pen borderPen = new Pen(Color.FromArgb(200, 200, 200)))
            {
                g.DrawRectangle(borderPen, trackRect);
            }

            // 绘制滑块
            int thumbX = _isChecked ? (Width - 10 - _thumbSize) : 5;
            Rectangle thumbRect = new Rectangle(
                thumbX,
                (Height - _thumbSize) / 2,
                _thumbSize,
                _thumbSize
            );

            using (Brush thumbBrush = new SolidBrush(Color.White))
            {
                g.FillRectangle(thumbBrush, thumbRect);
            }

            using (Pen thumbBorderPen = new Pen(Color.FromArgb(150, 150, 150)))
            {
                g.DrawRectangle(thumbBorderPen, thumbRect);
            }

            // 绘制SVG图标
            string svgPath = _isChecked ? _checkedSvgPath : _uncheckedSvgPath;
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
                                // 在滑块中绘制图标
                                Rectangle iconRect = new Rectangle(
                                    thumbX + (_thumbSize - _iconSize) / 2,
                                    (Height - _iconSize) / 2,
                                    _iconSize,
                                    _iconSize
                                );

                                // 绘制白色圆形背景
                                using (Brush iconBgBrush = new SolidBrush(Color.White))
                                {
                                    g.FillEllipse(iconBgBrush, iconRect);
                                }

                                g.DrawImage(svgBitmap, iconRect);
                            }
                        }
                    }
                }
                catch
                {
                    // SVG渲染失败时绘制简单图标
                    DrawFallbackIcon(g, thumbRect, _isChecked);
                }
            }

            // 绘制文本（可选）
            string text = _isChecked ? _checkedText : _uncheckedText;
            if (!string.IsNullOrEmpty(text))
            {
                SizeF textSize = g.MeasureString(text, Font);
                PointF textLocation;

                if (_isChecked)
                {
                    // 文本在左侧
                    textLocation = new PointF(
                        5,
                        (Height - textSize.Height) / 2
                    );
                }
                else
                {
                    // 文本在右侧
                    textLocation = new PointF(
                        Width - textSize.Width - 5,
                        (Height - textSize.Height) / 2
                    );
                }

                Color textColor = _isChecked ? Color.White : Color.Black;
                using (Brush textBrush = new SolidBrush(textColor))
                {
                    g.DrawString(text, Font, textBrush, textLocation);
                }
            }
        }

        private void DrawFallbackIcon(Graphics g, Rectangle thumbRect, bool isChecked)
        {
            Rectangle iconRect = new Rectangle(
                thumbRect.X + (thumbRect.Width - _iconSize) / 2,
                thumbRect.Y + (thumbRect.Height - _iconSize) / 2,
                _iconSize,
                _iconSize
            );

            Color iconColor = isChecked ? Color.FromArgb(255, 100, 100) : Color.Black;
            using (Brush iconBrush = new SolidBrush(iconColor))
            {
                if (isChecked)
                {
                    // 绘制彩虹圆形（简化版）
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