using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WindowsFormsApp3.UI
{
    /// <summary>
    /// 高级面板组件，提供增强的布局和视觉效果
    /// </summary>
    public class AdvancedPanel : Panel
    {
        private int _borderRadius = 0;
        private Color _borderColor = Color.Gray;
        private int _borderWidth = 1;
        private bool _hasDropShadow = false;
        private Color _shadowColor = Color.Black;
        private int _shadowDepth = 5;
        private int _shadowBlur = 5;
        private bool _isCollapsible = false;
        private bool _isCollapsed = false;
        private int _collapsedHeight = 30;
        private int _expandedHeight;
        private Control _collapsibleHeader;

        /// <summary>
        /// 获取或设置边框圆角半径
        /// </summary>
        [Category("Appearance")]
        [Description("边框圆角半径")]
        public int BorderRadius
        {
            get { return _borderRadius; }
            set
            {
                _borderRadius = Math.Max(0, value);
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置边框颜色
        /// </summary>
        [Category("Appearance")]
        [Description("边框颜色")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置边框宽度
        /// </summary>
        [Category("Appearance")]
        [Description("边框宽度")]
        public int BorderWidth
        {
            get { return _borderWidth; }
            set
            {
                _borderWidth = Math.Max(0, value);
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置是否显示阴影
        /// </summary>
        [Category("Appearance")]
        [Description("是否显示阴影")]
        public bool HasDropShadow
        {
            get { return _hasDropShadow; }
            set
            {
                _hasDropShadow = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置阴影颜色
        /// </summary>
        [Category("Appearance")]
        [Description("阴影颜色")]
        public Color ShadowColor
        {
            get { return _shadowColor; }
            set
            {
                _shadowColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置阴影深度
        /// </summary>
        [Category("Appearance")]
        [Description("阴影深度")]
        public int ShadowDepth
        {
            get { return _shadowDepth; }
            set
            {
                _shadowDepth = Math.Max(0, value);
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置阴影模糊度
        /// </summary>
        [Category("Appearance")]
        [Description("阴影模糊度")]
        public int ShadowBlur
        {
            get { return _shadowBlur; }
            set
            {
                _shadowBlur = Math.Max(0, value);
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置是否可折叠
        /// </summary>
        [Category("Behavior")]
        [Description("是否可折叠")]
        public bool IsCollapsible
        {
            get { return _isCollapsible; }
            set
            {
                _isCollapsible = value;
                if (_isCollapsible && _collapsibleHeader == null)
                {
                    CreateCollapsibleHeader();
                }
                else if (!_isCollapsible && _collapsibleHeader != null)
                {
                    Controls.Remove(_collapsibleHeader);
                    _collapsibleHeader.Dispose();
                    _collapsibleHeader = null;
                    IsCollapsed = false;
                }
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置是否已折叠
        /// </summary>
        [Category("Behavior")]
        [Description("是否已折叠")]
        public bool IsCollapsed
        {
            get { return _isCollapsed; }
            set
            {
                if (!_isCollapsible) return;
                
                _isCollapsed = value;
                
                if (_isCollapsed)
                {
                    _expandedHeight = Height;
                    Height = _collapsedHeight;
                }
                else
                {
                    Height = _expandedHeight;
                }
                
                UpdateChildControlsVisibility();
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置折叠后的高度
        /// </summary>
        [Category("Behavior")]
        [Description("折叠后的高度")]
        public int CollapsedHeight
        {
            get { return _collapsedHeight; }
            set
            {
                _collapsedHeight = Math.Max(20, value);
                if (_isCollapsible && _isCollapsed)
                {
                    Height = _collapsedHeight;
                }
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置面板标题
        /// </summary>
        [Category("Appearance")]
        [Description("面板标题")]
        public string PanelTitle { get; set; } = "面板标题";

        /// <summary>
        /// 构造函数
        /// </summary>
        public AdvancedPanel()
        {
            // 设置默认样式
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor,
                true);
            
            this.UpdateStyles();
            this.BackColor = Color.Transparent;
        }

        /// <summary>
        /// 创建可折叠面板的头部
        /// </summary>
        private void CreateCollapsibleHeader()
        {
            _collapsibleHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = _collapsedHeight,
                BackColor = Color.LightGray,
                Cursor = Cursors.Hand
            };
            
            // 添加标题标签
            Label titleLabel = new Label
            {
                Text = PanelTitle,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                Font = new Font(Font, FontStyle.Bold)
            };
            
            // 添加折叠按钮
            Button collapseButton = new Button
            {
                Text = "▼",
                Dock = DockStyle.Right,
                Width = 30,
                Height = _collapsedHeight - 2,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.DimGray
            };
            collapseButton.FlatAppearance.BorderSize = 0;
            collapseButton.Click += (s, e) => IsCollapsed = !IsCollapsed;
            
            _collapsibleHeader.Controls.Add(collapseButton);
            _collapsibleHeader.Controls.Add(titleLabel);
            _collapsibleHeader.Click += (s, e) => IsCollapsed = !IsCollapsed;
            
            Controls.Add(_collapsibleHeader);
        }

        /// <summary>
        /// 更新子控件的可见性
        /// </summary>
        private void UpdateChildControlsVisibility()
        {
            foreach (Control control in Controls)
            {
                if (control != _collapsibleHeader)
                {
                    control.Visible = !_isCollapsed;
                }
            }
        }

        /// <summary>
        /// 重写OnPaint方法
        /// </summary>
        /// <param name="e">绘图事件参数</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // 创建GraphicsPath
            GraphicsPath path = CreateRoundedRectanglePath(ClientRectangle, _borderRadius);
            
            // 绘制阴影
            if (_hasDropShadow)
            {
                DrawShadow(e.Graphics, path);
            }
            
            // 绘制背景
            using (SolidBrush brush = new SolidBrush(BackColor))
            {
                e.Graphics.FillPath(brush, path);
            }
            
            // 绘制边框
            if (_borderWidth > 0)
            {
                using (Pen pen = new Pen(_borderColor, _borderWidth))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
            
            path.Dispose();
        }

        /// <summary>
        /// 创建圆角矩形路径
        /// </summary>
        /// <param name="rectangle">矩形</param>
        /// <param name="radius">圆角半径</param>
        /// <returns>GraphicsPath对象</returns>
        private GraphicsPath CreateRoundedRectanglePath(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            
            // 调整矩形以适应边框宽度
            int halfWidth = _borderWidth / 2;
            rectangle.Inflate(-halfWidth, -halfWidth);
            
            if (radius > 0)
            {
                // 添加圆角
                int diameter = radius * 2;
                Size size = new Size(diameter, diameter);
                Rectangle arc = new Rectangle(rectangle.Location, size);
                
                // 左上角
                path.AddArc(arc, 180, 90);
                
                // 右上角
                arc.X = rectangle.Right - diameter;
                path.AddArc(arc, 270, 90);
                
                // 右下角
                arc.Y = rectangle.Bottom - diameter;
                path.AddArc(arc, 0, 90);
                
                // 左下角
                arc.X = rectangle.Left;
                path.AddArc(arc, 90, 90);
            }
            else
            {
                // 没有圆角，添加矩形
                path.AddRectangle(rectangle);
            }
            
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="graphics">Graphics对象</param>
        /// <param name="path">路径</param>
        private void DrawShadow(Graphics graphics, GraphicsPath path)
        {
            using (PathGradientBrush brush = new PathGradientBrush(path))
            {
                // 设置阴影颜色和透明度
                Color[] colors = new Color[_shadowDepth + 1];
                for (int i = 0; i <= _shadowDepth; i++)
                {
                    int alpha = (int)(255 * (1 - (double)i / _shadowDepth) * 0.3);
                    colors[i] = Color.FromArgb(alpha, _shadowColor);
                }
                
                brush.CenterColor = colors[0];
                brush.SurroundColors = new[] { colors[_shadowDepth] };
                
                // 绘制多层阴影以实现模糊效果
                for (int i = 1; i <= _shadowBlur; i++)
                {
                    using (GraphicsPath shadowPath = CreateRoundedRectanglePath(new Rectangle(
                        ClientRectangle.Left + i,
                        ClientRectangle.Top + i,
                        ClientRectangle.Width,
                        ClientRectangle.Height),
                        _borderRadius))
                    {
                        graphics.FillPath(brush, shadowPath);
                    }
                }
            }
        }

        /// <summary>
        /// 重写OnResize方法
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            
            if (!_isCollapsed && _isCollapsible)
            {
                _expandedHeight = Height;
            }
            
            Invalidate();
        }

        /// <summary>
        /// 切换折叠状态
        /// </summary>
        public void ToggleCollapse()
        {
            IsCollapsed = !IsCollapsed;
        }

        /// <summary>
        /// 展开面板
        /// </summary>
        public void Expand()
        {
            IsCollapsed = false;
        }

        /// <summary>
        /// 折叠面板
        /// </summary>
        public void Collapse()
        {
            IsCollapsed = true;
        }

        /// <summary>
        /// 重写添加控件方法，确保子控件位置正确
        /// </summary>
        /// <param name="value">要添加的控件</param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            
            // 如果是可折叠面板，确保子控件在头部下方
            if (_isCollapsible && e.Control != _collapsibleHeader)
            {
                e.Control.BringToFront();
            }
        }
    }
}