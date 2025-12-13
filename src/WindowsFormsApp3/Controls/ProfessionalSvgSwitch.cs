using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace WindowsFormsApp3.Controls
{
    /// <summary>
    /// 专业级SVG开关控件 - 支持流畅动画、渐变色、阴影效果
    /// </summary>
    public class ProfessionalSvgSwitch : Control
    {
        #region 字段和属性

        private bool _isChecked = false;
        private float _animationProgress = 0.0f;
        private bool _isAnimating = false;
        private Timer _animationTimer;
        private Rectangle _toggleRect;
        private bool _isHovered = false;
        private bool _isPressed = false;

        public event EventHandler CheckedChanged;

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnCheckedChanged(EventArgs.Empty);
                    StartAnimation();
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 选中状态的SVG路径
        /// </summary>
        public string CheckedSvgPath { get; set; } = "";

        /// <summary>
        /// 未选中状态的SVG路径
        /// </summary>
        public string UncheckedSvgPath { get; set; } = "";

        /// <summary>
        /// 选中时轨道颜色
        /// </summary>
        public Color TrackOnColor { get; set; } = Color.FromArgb(76, 175, 80);

        /// <summary>
        /// 未选中时轨道颜色
        /// </summary>
        public Color TrackOffColor { get; set; } = Color.FromArgb(158, 158, 158);

        /// <summary>
        /// 渐变起始颜色
        /// </summary>
        public Color GradientStartColor { get; set; } = Color.FromArgb(100, 181, 246);

        /// <summary>
        /// 渐变结束颜色
        /// </summary>
        public Color GradientEndColor { get; set; } = Color.FromArgb(33, 150, 243);

        /// <summary>
        /// 滑块颜色
        /// </summary>
        public Color ThumbColor { get; set; } = Color.White;

        /// <summary>
        /// 是否显示阴影
        /// </summary>
        public bool ShowShadow { get; set; } = true;

        /// <summary>
        /// 是否启用图标动画
        /// </summary>
        public bool AnimateIcons { get; set; } = true;

        /// <summary>
        /// 图标缩放比例
        /// </summary>
        public float IconScale { get; set; } = 1.0f;

        /// <summary>
        /// 开关尺寸模式
        /// </summary>
        public SwitchSize SizeMode { get; set; } = SwitchSize.Medium;

        #endregion

        #region 枚举

        public enum SwitchSize
        {
            Small,   // 60x30
            Medium,  // 80x40
            Large    // 100x50
        }

        #endregion

        #region 构造函数

        public ProfessionalSvgSwitch()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.DoubleBuffer |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;

            _animationTimer = new Timer();
            _animationTimer.Interval = 16; // 60 FPS
            _animationTimer.Tick += AnimationTimer_Tick;

            UpdateSize();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 切换开关状态
        /// </summary>
        public void Toggle()
        {
            IsChecked = !IsChecked;
        }

        #endregion

        #region 重写方法

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateToggleRect();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isPressed = true;
                Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _isPressed)
            {
                _isPressed = false;
                Toggle();
                Invalidate();
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            _isPressed = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingQuality = CompositingQuality.HighQuality;

            // 绘制阴影
            if (ShowShadow)
            {
                DrawShadow(g);
            }

            // 绘制轨道
            DrawTrack(g);

            // 绘制滑块
            DrawThumb(g);

            // 绘制图标
            DrawIcon(g);
        }

        #endregion

        #region 私有方法

        private void UpdateSize()
        {
            switch (SizeMode)
            {
                case SwitchSize.Small:
                    Size = new Size(60, 30);
                    break;
                case SwitchSize.Medium:
                    Size = new Size(80, 40);
                    break;
                case SwitchSize.Large:
                    Size = new Size(100, 50);
                    break;
            }
            UpdateToggleRect();
        }

        private void UpdateToggleRect()
        {
            int thumbSize = Height - 8;
            int maxLeft = Width - thumbSize - 4;
            int currentLeft = 4 + (int)((maxLeft - 4) * _animationProgress);
            _toggleRect = new Rectangle(currentLeft, 4, thumbSize, thumbSize);
        }

        private void StartAnimation()
        {
            if (!_isAnimating)
            {
                _isAnimating = true;
                _animationTimer.Start();
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            float targetProgress = _isChecked ? 1.0f : 0.0f;
            float speed = 0.15f;

            if (Math.Abs(_animationProgress - targetProgress) < 0.01f)
            {
                _animationProgress = targetProgress;
                _animationTimer.Stop();
                _isAnimating = false;
            }
            else
            {
                _animationProgress += (targetProgress - _animationProgress) * speed;
            }

            UpdateToggleRect();
            Invalidate();
        }

        private void DrawShadow(Graphics g)
        {
            Rectangle shadowRect = new Rectangle(_toggleRect.X + 2, _toggleRect.Y + 2, _toggleRect.Width, _toggleRect.Height);
            Color shadowColor = Color.FromArgb(50, Color.Black);

            using (GraphicsPath path = CreateRoundedRectanglePath(shadowRect, shadowRect.Height / 2))
            {
                using (PathGradientBrush brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = Color.Transparent;
                    brush.SurroundColors = new Color[] { shadowColor };
                    g.FillPath(brush, path);
                }
            }
        }

        private void DrawTrack(Graphics g)
        {
            Rectangle trackRect = new Rectangle(2, Height / 2 - (Height - 8) / 2, Width - 4, Height - 8);

            using (GraphicsPath path = CreateRoundedRectanglePath(trackRect, trackRect.Height / 2))
            {
                // 创建渐变
                Color startColor = _isPressed ? TrackOffColor : 
                                 (_isChecked ? TrackOnColor : TrackOffColor);
                Color endColor = _isChecked ? GradientEndColor : TrackOffColor;

                if (_isChecked && ShowShadow)
                {
                    // 选中状态使用渐变
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        trackRect, GradientStartColor, GradientEndColor, LinearGradientMode.Vertical))
                    {
                        g.FillPath(brush, path);
                    }
                }
                else
                {
                    // 未选中状态使用纯色
                    using (SolidBrush brush = new SolidBrush(startColor))
                    {
                        g.FillPath(brush, path);
                    }
                }

                // 绘制边框
                using (Pen pen = new Pen(Color.FromArgb(100, Color.Black), 1))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        private void DrawThumb(Graphics g)
        {
            Rectangle thumbRect = _toggleRect;
            
            // 鼠标悬停时稍微放大
            if (_isHovered && !_isPressed)
            {
                int inflate = 2;
                thumbRect.Inflate(inflate, inflate);
            }

            // 按下时稍微缩小
            if (_isPressed)
            {
                int deflate = 1;
                thumbRect.Inflate(-deflate, -deflate);
            }

            using (GraphicsPath path = CreateRoundedRectanglePath(thumbRect, thumbRect.Height / 2))
            {
                // 滑块渐变效果
                Color lightColor = _isHovered ? Color.FromArgb(240, 240, 240) : ThumbColor;
                Color darkColor = _isHovered ? Color.FromArgb(220, 220, 220) : Color.FromArgb(240, 240, 240);

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    thumbRect, lightColor, darkColor, LinearGradientMode.Vertical))
                {
                    g.FillPath(brush, path);
                }

                // 滑块边框
                using (Pen pen = new Pen(Color.FromArgb(150, Color.Black), 1))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        private void DrawIcon(Graphics g)
        {
            if (string.IsNullOrEmpty(CheckedSvgPath) || string.IsNullOrEmpty(UncheckedSvgPath))
                return;

            string svgPath = _isChecked ? CheckedSvgPath : UncheckedSvgPath;
            
            // 图标动画缩放
            float scale = IconScale;
            if (AnimateIcons && _isAnimating)
            {
                scale *= (0.8f + 0.2f * (float)Math.Sin(_animationProgress * Math.PI));
            }

            Rectangle iconRect = new Rectangle(
                _toggleRect.X + (_toggleRect.Width - (int)(16 * scale)) / 2,
                _toggleRect.Y + (_toggleRect.Height - (int)(16 * scale)) / 2,
                (int)(16 * scale),
                (int)(16 * scale));

            try
            {
                using (GraphicsPath path = CreateSvgPath(svgPath, iconRect))
                {
                    // 图标颜色根据状态变化
                    Color iconColor = _isChecked ? Color.White : Color.FromArgb(100, Color.Black);
                    using (SolidBrush brush = new SolidBrush(iconColor))
                    {
                        g.FillPath(brush, path);
                    }
                }
            }
            catch
            {
                // 如果SVG解析失败，绘制简单的指示器
                DrawSimpleIndicator(g);
            }
        }

        private void DrawSimpleIndicator(Graphics g)
        {
            Rectangle indicatorRect = new Rectangle(
                _toggleRect.X + _toggleRect.Width / 2 - 4,
                _toggleRect.Y + _toggleRect.Height / 2 - 4,
                8, 8);

            Color indicatorColor = _isChecked ? Color.White : Color.FromArgb(100, Color.Black);
            using (SolidBrush brush = new SolidBrush(indicatorColor))
            {
                g.FillEllipse(brush, indicatorRect);
            }
        }

        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        private GraphicsPath CreateSvgPath(string svgPath, Rectangle bounds)
        {
            // 简化的SVG路径解析
            GraphicsPath path = new GraphicsPath();
            
            // 这里应该实现完整的SVG路径解析
            // 为了简化，我们创建一个简单的路径
            Rectangle rect = new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 4, bounds.Height - 4);
            path.AddEllipse(rect);
            
            return path;
        }

        protected virtual void OnCheckedChanged(EventArgs e)
        {
            CheckedChanged?.Invoke(this, e);
        }

        #endregion
    }
}