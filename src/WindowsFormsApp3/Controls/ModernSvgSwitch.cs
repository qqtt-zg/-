using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WindowsFormsApp3.Utils;
using Svg;

namespace WindowsFormsApp3.Controls
{
    /// <summary>
    /// 现代化SVG开关控件 - 模拟iOS/Android风格
    /// </summary>
    public class ModernSvgSwitch : Control
    {
        private bool _isChecked = false;
        private string _checkedSvgPath = "";
        private string _uncheckedSvgPath = "";
        private bool _isAnimating = false;
        private bool _isPressed = false;
        // private float _animationProgress = 0f; // 暂时未使用，消除警告
        private float _targetPosition = 0f;
        private float _currentPosition = 0f;

        // 颜色配置
        private Color _trackOnColor = Color.FromArgb(52, 199, 89);   // iOS绿色
        private Color _trackOffColor = Color.FromArgb(142, 142, 147); // iOS灰色
        private Color _thumbColor = Color.White;
        private Color _shadowColor = Color.FromArgb(50, 0, 0, 0);

        // 尺寸配置
        private int _trackHeight = 31;
        private int _thumbSize = 27;
        private int _iconSize = 18;

        // 动画配置
        private Timer _animationTimer;
        private const int AnimationInterval = 16; // ~60fps

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    _targetPosition = _isChecked ? 1f : 0f;
                    StartAnimation();
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

        public Color TrackOnColor
        {
            get => _trackOnColor;
            set
            {
                _trackOnColor = value;
                Invalidate();
            }
        }

        public Color TrackOffColor
        {
            get => _trackOffColor;
            set
            {
                _trackOffColor = value;
                Invalidate();
            }
        }

        public event EventHandler IsCheckedChanged;

        public ModernSvgSwitch()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            Cursor = Cursors.Hand;
            BackColor = Color.Transparent;

            InitializeAnimation();
            InitializeDefaultValues();
        }

        private void InitializeAnimation()
        {
            _animationTimer = new Timer();
            _animationTimer.Interval = AnimationInterval;
            _animationTimer.Tick += OnAnimationTick;
        }

        private void InitializeDefaultValues()
        {
            _checkedSvgPath = ExternalSvgIconLibrary.LoadColorMode("rainbow");
            _uncheckedSvgPath = ExternalSvgIconLibrary.LoadColorMode("black-circle");

            Size = new Size(51, _trackHeight);
            MinimumSize = new Size(51, _trackHeight);
            MaximumSize = new Size(80, _trackHeight);
        }

        private void StartAnimation()
        {
            if (!_isAnimating)
            {
                _isAnimating = true;
                _animationTimer.Start();
            }
        }

        private void OnAnimationTick(object sender, EventArgs e)
        {
            const float animationSpeed = 0.15f;

            if (Math.Abs(_targetPosition - _currentPosition) < 0.01f)
            {
                _currentPosition = _targetPosition;
                _isAnimating = false;
                _animationTimer.Stop();
            }
            else
            {
                _currentPosition += (_targetPosition - _currentPosition) * animationSpeed;
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // 计算轨道矩形
            Rectangle trackRect = new Rectangle(
                0,
                (Height - _trackHeight) / 2,
                Width,
                _trackHeight
            );

            // 绘制轨道阴影
            DrawTrackShadow(g, trackRect);

            // 绘制轨道背景
            DrawTrack(g, trackRect);

            // 计算滑块位置
            int thumbX = (int)(_currentPosition * (Width - _thumbSize));
            Rectangle thumbRect = new Rectangle(
                thumbX,
                (Height - _thumbSize) / 2,
                _thumbSize,
                _thumbSize
            );

            // 绘制滑块阴影
            DrawThumbShadow(g, thumbRect);

            // 绘制滑块
            DrawThumb(g, thumbRect);

            // 绘制图标
            DrawIcon(g, thumbRect);
        }

        private void DrawTrackShadow(Graphics g, Rectangle trackRect)
        {
            // 轨道外阴影
            Rectangle shadowRect = new Rectangle(
                trackRect.X,
                trackRect.Y + 2,
                trackRect.Width,
                trackRect.Height
            );

            using (GraphicsPath path = CreateRoundedRectanglePath(shadowRect, _trackHeight / 2))
            {
                using (Brush shadowBrush = new SolidBrush(_shadowColor))
                {
                    g.FillPath(shadowBrush, path);
                }
            }
        }

        private void DrawTrack(Graphics g, Rectangle trackRect)
        {
            // 插值计算轨道颜色
            Color trackColor = InterpolateColor(_trackOffColor, _trackOnColor, _currentPosition);

            using (GraphicsPath path = CreateRoundedRectanglePath(trackRect, _trackHeight / 2))
            {
                using (Brush trackBrush = new SolidBrush(trackColor))
                {
                    g.FillPath(trackBrush, path);
                }
            }
        }

        private void DrawThumbShadow(Graphics g, Rectangle thumbRect)
        {
            // 滑块阴影
            Rectangle shadowRect = new Rectangle(
                thumbRect.X + 1,
                thumbRect.Y + 2,
                thumbRect.Width,
                thumbRect.Height
            );

            using (GraphicsPath path = CreateRoundedRectanglePath(shadowRect, _thumbSize / 2))
            {
                using (Brush shadowBrush = new SolidBrush(_shadowColor))
                {
                    g.FillPath(shadowBrush, path);
                }
            }
        }

        private void DrawThumb(Graphics g, Rectangle thumbRect)
        {
            // 绘制滑块
            Color thumbColor = _isPressed ?
                Color.FromArgb(240, 240, 240) : // 按下时变暗
                _thumbColor;

            using (GraphicsPath path = CreateRoundedRectanglePath(thumbRect, _thumbSize / 2))
            {
                using (Brush thumbBrush = new SolidBrush(thumbColor))
                {
                    g.FillPath(thumbBrush, path);
                }

                // 绘制边框
                using (Pen borderPen = new Pen(Color.FromArgb(200, 200, 200), 1))
                {
                    g.DrawPath(borderPen, path);
                }
            }
        }

        private void DrawIcon(Graphics g, Rectangle thumbRect)
        {
            string svgPath = _currentPosition > 0.5f ? _checkedSvgPath : _uncheckedSvgPath;

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
                                Rectangle iconRect = new Rectangle(
                                    thumbRect.X + (thumbRect.Width - _iconSize) / 2,
                                    thumbRect.Y + (thumbRect.Height - _iconSize) / 2,
                                    _iconSize,
                                    _iconSize
                                );

                                // 绘制圆形背景
                                using (Brush bgBrush = new SolidBrush(Color.White))
                                {
                                    g.FillEllipse(bgBrush, iconRect);
                                }

                                g.DrawImage(svgBitmap, iconRect);
                            }
                        }
                    }
                }
                catch
                {
                    // SVG渲染失败时绘制简单图标
                    DrawFallbackIcon(g, thumbRect, _currentPosition > 0.5f);
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

            Color iconColor = isChecked ? Color.FromArgb(52, 199, 89) : Color.FromArgb(142, 142, 147);
            using (Brush iconBrush = new SolidBrush(iconColor))
            {
                g.FillEllipse(iconBrush, iconRect);
            }
        }

        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            // 确保不会出现负数
            int width = Math.Max(0, rect.Width);
            int height = Math.Max(0, rect.Height);

            if (width == 0 || height == 0)
            {
                return path;
            }

            // 左上角
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            // 右上角
            path.AddArc(rect.X + width - diameter, rect.Y, diameter, diameter, 270, 90);
            // 右下角
            path.AddArc(rect.X + width - diameter, rect.Y + height - diameter, diameter, diameter, 0, 90);
            // 左下角
            path.AddArc(rect.X, rect.Y + height - diameter, diameter, diameter, 90, 90);

            path.CloseFigure();
            return path;
        }

        private Color InterpolateColor(Color color1, Color color2, float progress)
        {
            int r = (int)(color1.R + (color2.R - color1.R) * progress);
            int g = (int)(color1.G + (color2.G - color1.G) * progress);
            int b = (int)(color1.B + (color2.B - color1.B) * progress);
            int a = (int)(color1.A + (color2.A - color1.A) * progress);

            return Color.FromArgb(Math.Max(0, Math.Min(255, a)),
                               Math.Max(0, Math.Min(255, r)),
                               Math.Max(0, Math.Min(255, g)),
                               Math.Max(0, Math.Min(255, b)));
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            IsChecked = !IsChecked;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _isPressed = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isPressed = false;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isPressed = false;
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // 更新动画位置
            if (_isChecked)
            {
                _currentPosition = 1f;
                _targetPosition = 1f;
            }
            else
            {
                _currentPosition = 0f;
                _targetPosition = 0f;
            }
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _animationTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}