using System;
using System.Text;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// SVG路径生成器 - 用于程序化生成SVG路径数据
    /// </summary>
    public static class SvgPathGenerator
    {
        /// <summary>
        /// 生成圆形路径
        /// </summary>
        /// <param name="cx">中心X坐标</param>
        /// <param name="cy">中心Y坐标</param>
        /// <param name="r">半径</param>
        public static string Circle(float cx, float cy, float r)
        {
            return $"M {cx - r:F1} {cy:F1} A {r:F1} {r:F1} 0 1 1 {cx + r:F1} {cy:F1} A {r:F1} {r:F1} 0 1 1 {cx - r:F1} {cy:F1}";
        }

        /// <summary>
        /// 生成半圆路径
        /// </summary>
        public static string HalfCircle(float cx, float cy, float r, bool isTopHalf = true)
        {
            if (isTopHalf)
            {
                return $"M {cx - r:F1} {cy:F1} A {r:F1} {r:F1} 0 1 1 {cx + r:F1} {cy:F1}";
            }
            else
            {
                return $"M {cx + r:F1} {cy:F1} A {r:F1} {r:F1} 0 1 1 {cx - r:F1} {cy:F1}";
            }
        }

        /// <summary>
        /// 生成矩形路径
        /// </summary>
        public static string Rectangle(float x, float y, float width, float height)
        {
            return $"M {x:F1} {y:F1} L {x + width:F1} {y:F1} L {x + width:F1} {y + height:F1} L {x:F1} {y + height:F1} Z";
        }

        /// <summary>
        /// 生成圆角矩形路径
        /// </summary>
        public static string RoundedRectangle(float x, float y, float width, float height, float radius)
        {
            var sb = new StringBuilder();
            float r = Math.Min(radius, Math.Min(width / 2, height / 2));
            
            sb.Append($"M {x + r:F1} {y:F1}");
            sb.Append($" L {x + width - r:F1} {y:F1}");
            sb.Append($" Q {x + width:F1} {y:F1} {x + width:F1} {y + r:F1}");
            sb.Append($" L {x + width:F1} {y + height - r:F1}");
            sb.Append($" Q {x + width:F1} {y + height:F1} {x + width - r:F1} {y + height:F1}");
            sb.Append($" L {x + r:F1} {y + height:F1}");
            sb.Append($" Q {x:F1} {y + height:F1} {x:F1} {y + height - r:F1}");
            sb.Append($" L {x:F1} {y + r:F1}");
            sb.Append($" Q {x:F1} {y:F1} {x + r:F1} {y:F1} Z");
            return sb.ToString();
        }

        /// <summary>
        /// 生成三角形路径
        /// </summary>
        public static string Triangle(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            return $"M {x1:F1} {y1:F1} L {x2:F1} {y2:F1} L {x3:F1} {y3:F1} Z";
        }

        /// <summary>
        /// 生成等边三角形路径
        /// </summary>
        public static string EquilateralTriangle(float cx, float cy, float size)
        {
            float height = (float)(size * Math.Sqrt(3) / 2);
            float x1 = cx;
            float y1 = cy - height * 2 / 3;
            float x2 = cx - size / 2;
            float y2 = cy + height / 3;
            float x3 = cx + size / 2;
            float y3 = cy + height / 3;
            
            return Triangle(x1, y1, x2, y2, x3, y3);
        }

        /// <summary>
        /// 生成五角星路径
        /// </summary>
        public static string Star(float cx, float cy, float outerRadius, float innerRadius, int points = 5)
        {
            var sb = new StringBuilder();
            double angle = -Math.PI / 2;
            double step = Math.PI / points;

            for (int i = 0; i < points * 2; i++)
            {
                double radius = i % 2 == 0 ? outerRadius : innerRadius;
                double x = cx + Math.Cos(angle) * radius;
                double y = cy + Math.Sin(angle) * radius;

                if (i == 0)
                    sb.Append($"M {x:F2} {y:F2}");
                else
                    sb.Append($" L {x:F2} {y:F2}");

                angle += step;
            }
            sb.Append(" Z");
            return sb.ToString();
        }

        /// <summary>
        /// 生成心形路径
        /// </summary>
        public static string Heart(float cx, float cy, float size = 24)
        {
            float scale = size / 24f;
            return $"M {cx + 12 * scale:F2} {cy + 21.35 * scale:F2} " +
                   $"L {cx + 10.55 * scale:F2} {cy + 20.03 * scale:F2} " +
                   $"C {cx + 5.4 * scale:F2} {cy + 15.36 * scale:F2} {cx + 2 * scale:F2} {cy + 12.28 * scale:F2} {cx + 2 * scale:F2} {cy + 8.5 * scale:F2} " +
                   $"C {cx + 2 * scale:F2} {cy + 5.42 * scale:F2} {cx + 4.42 * scale:F2} {cy + 3 * scale:F2} {cx + 7.5 * scale:F2} {cy + 3 * scale:F2} " +
                   $"C {cx + 9.24 * scale:F2} {cy + 3 * scale:F2} {cx + 10.91 * scale:F2} {cy + 3.81 * scale:F2} {cx + 12 * scale:F2} {cy + 5.09 * scale:F2} " +
                   $"C {cx + 13.09 * scale:F2} {cy + 3.81 * scale:F2} {cx + 14.76 * scale:F2} {cy + 3 * scale:F2} {cx + 16.5 * scale:F2} {cy + 3 * scale:F2} " +
                   $"C {cx + 19.58 * scale:F2} {cy + 3 * scale:F2} {cx + 22 * scale:F2} {cy + 5.42 * scale:F2} {cx + 22 * scale:F2} {cy + 8.5 * scale:F2} " +
                   $"C {cx + 22 * scale:F2} {cy + 12.28 * scale:F2} {cx + 18.6 * scale:F2} {cy + 15.36 * scale:F2} {cx + 13.45 * scale:F2} {cy + 20.04 * scale:F2} " +
                   $"L {cx + 12 * scale:F2} {cy + 21.35 * scale:F2} Z";
        }

        /// <summary>
        /// 生成加号路径
        /// </summary>
        public static string Plus(float cx, float cy, float size)
        {
            float halfSize = size / 2;
            return $"M {cx - halfSize:F1} {cy:F1} L {cx + halfSize:F1} {cy:F1} M {cx:F1} {cy - halfSize:F1} L {cx:F1} {cy + halfSize:F1}";
        }

        /// <summary>
        /// 生成减号路径
        /// </summary>
        public static string Minus(float cx, float cy, float size)
        {
            float halfSize = size / 2;
            return $"M {cx - halfSize:F1} {cy:F1} L {cx + halfSize:F1} {cy:F1}";
        }

        /// <summary>
        /// 生成乘号路径
        /// </summary>
        public static string Multiply(float cx, float cy, float size)
        {
            float halfSize = size / 2;
            float offset = halfSize * 0.7f;
            return $"M {cx - offset:F1} {cy - offset:F1} L {cx + offset:F1} {cy + offset:F1} M {cx + offset:F1} {cy - offset:F1} L {cx - offset:F1} {cy + offset:F1}";
        }

        /// <summary>
        /// 生成除号路径
        /// </summary>
        public static string Divide(float cx, float cy, float size)
        {
            float halfSize = size / 2;
            return $"M {cx - halfSize:F1} {cy:F1} L {cx + halfSize:F1} {cy:F1} M {cx:F1} {cy - halfSize:F1} L {cx:F1} {cy - halfSize + 2:F1} M {cx:F1} {cy + halfSize:F1} L {cx:F1} {cy + halfSize - 2:F1}";
        }

        /// <summary>
        /// 生成对勾路径
        /// </summary>
        public static string Check(float cx, float cy, float size)
        {
            float scale = size / 24f;
            return $"M {cx - 8 * scale:F1} {cy + 2 * scale:F1} L {cx - 2 * scale:F1} {cy + 8 * scale:F1} L {cx + 8 * scale:F1} {cy - 6 * scale:F1}";
        }

        /// <summary>
        /// 生成X号路径
        /// </summary>
        public static string X(float cx, float cy, float size)
        {
            float halfSize = size / 2;
            return $"M {cx - halfSize:F1} {cy - halfSize:F1} L {cx + halfSize:F1} {cy + halfSize:F1} M {cx + halfSize:F1} {cy - halfSize:F1} L {cx - halfSize:F1} {cy + halfSize:F1}";
        }

        /// <summary>
        /// 生成播放按钮路径
        /// </summary>
        public static string Play(float cx, float cy, float size)
        {
            float halfSize = size / 2;
            float x1 = cx - halfSize * 0.6f;
            float y1 = cy - halfSize;
            float x2 = cx - halfSize * 0.6f;
            float y2 = cy + halfSize;
            float x3 = cx + halfSize;
            float y3 = cy;
            
            return Triangle(x1, y1, x2, y2, x3, y3);
        }

        /// <summary>
        /// 生成暂停按钮路径
        /// </summary>
        public static string Pause(float cx, float cy, float size)
        {
            float barWidth = size / 3;
            float barHeight = size;
            float gap = barWidth / 2;
            
            return $"M {cx - barWidth - gap/2:F1} {cy - barHeight/2:F1} L {cx - barWidth - gap/2:F1} {cy + barHeight/2:F1} " +
                   $"L {cx - barWidth - gap/2:F1} {cy - barHeight/2:F1} L {cx - barWidth - gap/2:F1} {cy + barHeight/2:F1} " +
                   $"M {cx + gap/2:F1} {cy - barHeight/2:F1} L {cx + gap/2:F1} {cy + barHeight/2:F1} " +
                   $"M {cx + gap/2:F1} {cy - barHeight/2:F1} L {cx + gap/2:F1} {cy + barHeight/2:F1}";
        }

        /// <summary>
        /// 生成停止按钮路径
        /// </summary>
        public static string Stop(float cx, float cy, float size)
        {
            return Rectangle(cx - size/2, cy - size/2, size, size);
        }

        /// <summary>
        /// 生成太阳路径
        /// </summary>
        public static string Sun(float cx, float cy, float size)
        {
            var sb = new StringBuilder();
            float radius = size / 2;
            float rayLength = radius * 0.3f;
            
            // 中心圆
            sb.Append(Circle(cx, cy, radius * 0.8f));
            
            // 光线
            for (int i = 0; i < 8; i++)
            {
                double angle = i * Math.PI / 4;
                float innerX = cx + (float)Math.Cos(angle) * radius;
                float innerY = cy + (float)Math.Sin(angle) * radius;
                float outerX = cx + (float)Math.Cos(angle) * (radius + rayLength);
                float outerY = cy + (float)Math.Sin(angle) * (radius + rayLength);
                
                sb.Append($" M {innerX:F1} {innerY:F1} L {outerX:F1} {outerY:F1}");
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// 生成月亮路径
        /// </summary>
        public static string Moon(float cx, float cy, float size)
        {
            float radius = size / 2;
            return $"M {cx + radius * 0.3f:F1} {cy - radius * 0.7f:F1} " +
                   $"A {radius:F1} {radius:F1} 0 1 1 {cx + radius * 0.3f:F1} {cy + radius * 0.7f:F1} " +
                   $"A {radius * 0.7f:F1} {radius * 0.7f:F1} 0 1 0 {cx + radius * 0.3f:F1} {cy - radius * 0.7f:F1}";
        }

        /// <summary>
        /// 生成云朵路径
        /// </summary>
        public static string Cloud(float cx, float cy, float size)
        {
            float scale = size / 24f;
            return $"M {cx + 6 * scale:F1} {cy + 18 * scale:F1} " +
                   $"C {cx + 6 * scale:F1} {cy + 12 * scale:F1} {cx + 4 * scale:F1} {cy + 8 * scale:F1} {cx + 0 * scale:F1} {cy + 8 * scale:F1} " +
                   $"C {cx - 4 * scale:F1} {cy + 8 * scale:F1} {cx - 6 * scale:F1} {cy + 11 * scale:F1} {cx - 6 * scale:F1} {cy + 14 * scale:F1} " +
                   $"C {cx - 6 * scale:F1} {cy + 17 * scale:F1} {cx - 4 * scale:F1} {cy + 20 * scale:F1} {cx + 0 * scale:F1} {cy + 20 * scale:F1} " +
                   $"C {cx + 3 * scale:F1} {cy + 20 * scale:F1} {cx + 6 * scale:F1} {cy + 18 * scale:F1} {cx + 6 * scale:F1} {cy + 18 * scale:F1}";
        }
    }

    /// <summary>
    /// 常用SVG图标集合
    /// </summary>
    public static class CommonSvgIcons
    {
        public static string Check => SvgPathGenerator.Check(12, 12, 16);
        public static string X => SvgPathGenerator.X(12, 12, 16);
        public static string Plus => SvgPathGenerator.Plus(12, 12, 16);
        public static string Minus => SvgPathGenerator.Minus(12, 12, 16);
        public static string Heart => SvgPathGenerator.Heart(12, 12, 20);
        public static string Star => SvgPathGenerator.Star(12, 12, 8, 4, 5);
        public static string Sun => SvgPathGenerator.Sun(12, 12, 20);
        public static string Moon => SvgPathGenerator.Moon(12, 12, 20);
        public static string Cloud => SvgPathGenerator.Cloud(12, 12, 24);
        public static string Play => SvgPathGenerator.Play(12, 12, 20);
        public static string Pause => SvgPathGenerator.Pause(12, 12, 20);
        public static string Stop => SvgPathGenerator.Stop(12, 12, 16);
        public static string Circle => SvgPathGenerator.Circle(12, 12, 10);
        public static string Triangle => SvgPathGenerator.EquilateralTriangle(12, 12, 16);
        public static string Square => SvgPathGenerator.Rectangle(4, 4, 16, 16);
        public static string Diamond => SvgPathGenerator.Triangle(12, 4, 20, 12, 12, 20);
    }
}