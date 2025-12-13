using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindowsFormsApp3.Controls
{
    /// <summary>
    /// 极简风格SVG开关控件 - 简约但支持SVG图标
    /// </summary>
    public class MinimalSvgSwitch : Control
    {
        #region 字段和属性

        private bool _isChecked = false;
        private float _animationProgress = 0.0f;
        private Timer _animationTimer;
        private Rectangle _thumbRect;
        private bool _isHovered = false;

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
        /// 选中时颜色 (Material Blue)
        /// </summary>
        public Color ActiveColor { get; set; } = Color.FromArgb(33, 150, 243);

        /// <summary>
        /// 未选中时颜色 (浅灰)
        /// </summary>
        public Color InactiveColor { get; set; } = Color.FromArgb(224, 224, 224);

        /// <summary>
        /// 滑块颜色
        /// </summary>
        public Color ThumbColor { get; set; } = Color.White;

        /// <summary>
        /// 动画持续时间 (毫秒)
        /// </summary>
        public int AnimationDuration { get; set; } = 150;

        #endregion

        #region 构造函数

        public MinimalSvgSwitch()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.DoubleBuffer |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;

            // 极简尺寸: 60x24
            Size = new Size(60, 24);

            _animationTimer = new Timer();
            _animationTimer.Interval = 16; // 60 FPS
            _animationTimer.Tick += AnimationTimer_Tick;

            UpdateThumbPosition();
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
            UpdateThumbPosition();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Toggle();
            }
            base.OnMouseDown(e);
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
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 绘制轨道
            DrawTrack(g);

            // 绘制滑块
            DrawThumb(g);

            // 绘制SVG图标
            DrawSvgIcon(g);
        }

        #endregion

        #region 私有方法

        private void UpdateThumbPosition()
        {
            int thumbSize = Height - 4;
            int maxLeft = Width - thumbSize - 2;
            int currentLeft = 2 + (int)((maxLeft - 2) * _animationProgress);
            _thumbRect = new Rectangle(currentLeft, 2, thumbSize, thumbSize);
        }

        private void StartAnimation()
        {
            if (!_animationTimer.Enabled)
            {
                _animationTimer.Start();
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            float targetProgress = _isChecked ? 1.0f : 0.0f;
            float speed = 0.2f; // 快速动画

            if (Math.Abs(_animationProgress - targetProgress) < 0.01f)
            {
                _animationProgress = targetProgress;
                _animationTimer.Stop();
            }
            else
            {
                _animationProgress += (targetProgress - _animationProgress) * speed;
            }

            UpdateThumbPosition();
            Invalidate();
        }

        private void DrawTrack(Graphics g)
        {
            Rectangle trackRect = new Rectangle(0, Height / 2 - 4, Width, 8);
            
            // 圆角轨道
            using (GraphicsPath path = CreateRoundedRectanglePath(trackRect, 4))
            {
                // 根据状态选择颜色
                Color trackColor = _isChecked ? ActiveColor : InactiveColor;
                
                using (SolidBrush brush = new SolidBrush(trackColor))
                {
                    g.FillPath(brush, path);
                }

                // 如果悬停，添加轻微的高亮效果
                if (_isHovered)
                {
                    using (Pen pen = new Pen(Color.FromArgb(30, Color.White), 1))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }

        private void DrawThumb(Graphics g)
        {
            // 滑块阴影效果 (极简)
            if (_isChecked)
            {
                Rectangle shadowRect = new Rectangle(_thumbRect.X + 1, _thumbRect.Y + 1, _thumbRect.Width, _thumbRect.Height);
                using (GraphicsPath path = CreateRoundedRectanglePath(shadowRect, shadowRect.Height / 2))
                {
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(20, Color.Black)))
                    {
                        g.FillPath(brush, path);
                    }
                }
            }

            // 主滑块
            using (GraphicsPath path = CreateRoundedRectanglePath(_thumbRect, _thumbRect.Height / 2))
            {
                // 滑块渐变 (极简渐变)
                Color lightColor = _isHovered ? Color.FromArgb(248, 248, 248) : ThumbColor;
                Color darkColor = ThumbColor;

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    _thumbRect, lightColor, darkColor, LinearGradientMode.Vertical))
                {
                    g.FillPath(brush, path);
                }

                // 滑块边框 (极细边框)
                using (Pen pen = new Pen(Color.FromArgb(200, Color.Black), 0.5f))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        private void DrawSvgIcon(Graphics g)
        {
            if (string.IsNullOrEmpty(CheckedSvgPath) || string.IsNullOrEmpty(UncheckedSvgPath))
                return;

            string svgPath = _isChecked ? CheckedSvgPath : UncheckedSvgPath;

            // SVG图标尺寸: 根据滑块大小调整，增大图标尺寸使其更明显
            int iconSize = Math.Max(10, Math.Min(16, _thumbRect.Width / 2));
            Rectangle iconRect = new Rectangle(
                _thumbRect.X + (_thumbRect.Width - iconSize) / 2,
                _thumbRect.Y + (_thumbRect.Height - iconSize) / 2,
                iconSize, iconSize);

            try
            {
                using (GraphicsPath path = CreateSvgPath(svgPath, iconRect))
                {
                    // 根据状态和图标类型选择颜色
                    Color iconColor;

                    if (_isChecked)
                    {
                        // 选中状态的颜色
                        switch (svgPath.ToLower())
                        {
                            case "rainbow":
                                // 彩虹模式使用渐变效果
                                iconColor = Color.FromArgb(255, 193, 7); // 金黄色
                                break;
                            case "sparkle":
                                iconColor = Color.FromArgb(255, 235, 59); // 亮黄色
                                break;
                            default:
                                iconColor = Color.White; // 白色图标
                                break;
                        }
                    }
                    else
                    {
                        // 未选中状态的颜色
                        switch (svgPath.ToLower())
                        {
                            case "black-circle":
                                iconColor = Color.FromArgb(100, Color.Black); // 半透明黑色
                                break;
                            case "moon":
                                iconColor = Color.FromArgb(120, Color.Black); // 月亮灰色
                                break;
                            default:
                                iconColor = Color.FromArgb(120, Color.Black); // 半透明黑色
                                break;
                        }
                    }

                    // 添加阴影效果（仅在选中状态）
                    if (_isChecked)
                    {
                        Rectangle shadowRect = new Rectangle(iconRect.X + 1, iconRect.Y + 1, iconRect.Width, iconRect.Height);
                        using (GraphicsPath shadowPath = CreateSvgPath(svgPath, shadowRect))
                        {
                            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(50, Color.Black)))
                            {
                                g.FillPath(shadowBrush, shadowPath);
                            }
                        }
                    }

                    // 绘制主图标
                    using (SolidBrush brush = new SolidBrush(iconColor))
                    {
                        g.FillPath(brush, path);
                    }

                    // 添加高光效果（可选）
                    if (_isHovered && _isChecked)
                    {
                        using (Pen pen = new Pen(Color.FromArgb(100, Color.White), 0.5f))
                        {
                            g.DrawPath(pen, path);
                        }
                    }
                }
            }
            catch
            {
                // 如果SVG解析失败，绘制简单的指示点
                DrawSimpleIndicator(g);
            }
        }

        private void DrawSimpleIndicator(Graphics g)
        {
            Rectangle indicatorRect = new Rectangle(
                _thumbRect.X + _thumbRect.Width / 2 - 2,
                _thumbRect.Y + _thumbRect.Height / 2 - 2,
                4, 4);

            Color indicatorColor = _isChecked ? 
                ActiveColor : 
                Color.FromArgb(120, Color.Black);

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
            GraphicsPath path = new GraphicsPath();

            try
            {
                // 如果是简单的关键词，绘制对应的形状
                switch (svgPath.ToLower())
                {
                    case "rainbow":
                        // 绘制彩虹圆圈
                        Rectangle rect = new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 2, bounds.Height - 2);
                        path.AddEllipse(rect);
                        break;

                    case "black-circle":
                        // 绘制实心圆圈
                        Rectangle solidRect = new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 2, bounds.Height - 2);
                        path.AddEllipse(solidRect);
                        break;

                    case "sparkle":
                        // 绘制星形
                        CreateStarPath(path, bounds);
                        break;

                    case "moon":
                        // 绘制月牙形
                        CreateMoonPath(path, bounds);
                        break;

                    default:
                        // 默认绘制小圆圈
                        Rectangle defaultRect = new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 4, bounds.Height - 4);
                        path.AddEllipse(defaultRect);
                        break;
                }
            }
            catch
            {
                // 如果绘制失败，使用默认圆圈
                Rectangle rect = new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 4, bounds.Height - 4);
                path.AddEllipse(rect);
            }

            return path;
        }

        private void CreateStarPath(GraphicsPath path, Rectangle bounds)
        {
            Point center = new Point(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
            int outerRadius = Math.Min(bounds.Width, bounds.Height) / 2 - 1;
            int innerRadius = outerRadius / 2;

            Point[] points = new Point[10]; // 5个外角，5个内角

            for (int i = 0; i < 10; i++)
            {
                double angle = Math.PI * i / 5 - Math.PI / 2;
                int radius = (i % 2 == 0) ? outerRadius : innerRadius;

                points[i] = new Point(
                    center.X + (int)(Math.Cos(angle) * radius),
                    center.Y + (int)(Math.Sin(angle) * radius)
                );
            }

            path.AddPolygon(points);
        }

        private void CreateMoonPath(GraphicsPath path, Rectangle bounds)
        {
            // 创建月牙形状
            int diameter = Math.Min(bounds.Width, bounds.Height) - 2;
            Rectangle mainCircle = new Rectangle(bounds.X + 1, bounds.Y + 1, diameter, diameter);
            Rectangle shadowCircle = new Rectangle(bounds.X + diameter/3, bounds.Y + 1, diameter, diameter);

            path.AddEllipse(mainCircle);
            using (GraphicsPath shadowPath = new GraphicsPath())
            {
                shadowPath.AddEllipse(shadowCircle);
                path.AddPath(shadowPath, false);
            }
        }

        protected virtual void OnCheckedChanged(EventArgs e)
        {
            CheckedChanged?.Invoke(this, e);
        }

        #endregion
    }

    /// <summary>
    /// 极简开关控件工厂
    /// </summary>
    public static class MinimalSvgSwitchFactory
    {
        /// <summary>
        /// 创建标准极简开关
        /// </summary>
        public static MinimalSvgSwitch CreateStandard(
            string checkedSvgPath = "rainbow",
            string uncheckedSvgPath = "black-circle")
        {
            var switchControl = new MinimalSvgSwitch();
            switchControl.CheckedSvgPath = checkedSvgPath;
            switchControl.UncheckedSvgPath = uncheckedSvgPath;
            switchControl.ActiveColor = Color.FromArgb(33, 150, 243); // Material Blue
            switchControl.InactiveColor = Color.FromArgb(224, 224, 224); // 浅灰
            return switchControl;
        }

        /// <summary>
        /// 创建绿色主题极简开关
        /// </summary>
        public static MinimalSvgSwitch CreateGreenTheme(
            string checkedSvgPath = "rainbow",
            string uncheckedSvgPath = "black-circle")
        {
            var switchControl = CreateStandard(checkedSvgPath, uncheckedSvgPath);
            switchControl.ActiveColor = Color.FromArgb(76, 175, 80); // Material Green
            return switchControl;
        }

        /// <summary>
        /// 创建橙色主题极简开关
        /// </summary>
        public static MinimalSvgSwitch CreateOrangeTheme(
            string checkedSvgPath = "rainbow",
            string uncheckedSvgPath = "black-circle")
        {
            var switchControl = CreateStandard(checkedSvgPath, uncheckedSvgPath);
            switchControl.ActiveColor = Color.FromArgb(255, 152, 0); // Material Orange
            return switchControl;
        }

        /// <summary>
        /// 创建彩色模式专用开关
        /// </summary>
        public static MinimalSvgSwitch CreateColorModeSwitch()
        {
            return CreateGreenTheme("rainbow", "black-circle");
        }
    }
}