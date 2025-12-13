using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApp3.Controls
{
    /// <summary>
    /// 支持彩色emoji显示的自定义按钮控件
    /// </summary>
    public class ColorEmojiButton : Control
    {
        private bool _isChecked = false;
        private string _checkedText = "🌈 彩色";
        private string _uncheckedText = "⚫ 黑白";
        private Color _fillColor = Color.FromArgb(52, 152, 219);
        private Color _fillHoverColor = Color.FromArgb(41, 128, 185);
        private bool _isHovering = false;

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, out uint pcFonts);

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

        public event EventHandler IsCheckedChanged;

        public ColorEmojiButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI Emoji", 9F);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

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

            // 绘制文本
            string text = _isChecked ? _checkedText : _uncheckedText;
            using (Font emojiFont = new Font("Segoe UI Emoji", 9F, FontStyle.Regular, GraphicsUnit.Point))
            {
                SizeF textSize = g.MeasureString(text, emojiFont);
                PointF textLocation = new PointF(
                    (Width - textSize.Width) / 2,
                    (Height - textSize.Height) / 2
                );

                // 使用GDI渲染彩色emoji
                TextRenderer.DrawText(g, text, emojiFont, Point.Round(textLocation), ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
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