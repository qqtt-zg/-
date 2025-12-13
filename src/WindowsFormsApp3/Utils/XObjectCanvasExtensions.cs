using System;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Kernel.Geom;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// XObject相关的Canvas扩展方法
    /// 提供高级XObject操作和变换矩阵处理
    /// </summary>
    public static class XObjectCanvasExtensions
    {
        /// <summary>
        /// 添加带变换的XObject（支持所有旋转角度）
        /// 自动计算和应用正确的变换矩阵
        /// </summary>
        /// <param name="canvas">PdfCanvas对象</param>
        /// <param name="xobject">要添加的XObject</param>
        /// <param name="rotation">旋转角度（0, 90, 180, 270）</param>
        /// <param name="targetPage">目标页面（用于获取页面尺寸）</param>
        public static void AddXObjectWithTransformation(this PdfCanvas canvas,
            PdfFormXObject xobject, int rotation, PdfPage targetPage = null)
        {
            try
            {
                if (rotation == 0)
                {
                    // 无旋转，直接添加
                    canvas.AddXObject(xobject);
                    return;
                }

                var bbox = xobject.GetBBox();
                Rectangle bboxRect = new Rectangle(bbox.GetAsNumber(0).FloatValue(), bbox.GetAsNumber(1).FloatValue(),
                                               bbox.GetAsNumber(2).FloatValue(), bbox.GetAsNumber(3).FloatValue());
                float bboxWidth = (float)bboxRect.GetWidth();
                float bboxHeight = (float)bboxRect.GetHeight();
                float pageWidth = targetPage?.GetPageSize().GetWidth() ?? bboxWidth;
                float pageHeight = targetPage?.GetPageSize().GetHeight() ?? bboxHeight;

                // 应用旋转变换矩阵
                ApplyRotationMatrix(canvas, rotation, pageWidth, pageHeight);

                // 添加XObject
                canvas.AddXObject(xobject);
            }
            catch (Exception ex)
            {
                LogHelper.Debug($"添加变换XObject失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 添加XObject到指定位置（支持位置和缩放）
        /// </summary>
        /// <param name="canvas">PdfCanvas对象</param>
        /// <param name="xobject">要添加的XObject</param>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="scaleX">X轴缩放比例</param>
        /// <param name="scaleY">Y轴缩放比例</param>
        public static void AddXObjectAt(this PdfCanvas canvas, PdfFormXObject xobject,
            float x, float y, float scaleX = 1.0f, float scaleY = 1.0f)
        {
            try
            {
                canvas.SaveState();

                // 应用变换矩阵：先平移，再缩放
                canvas.ConcatMatrix(scaleX, 0, 0, scaleY, x, y);

                // 添加XObject
                canvas.AddXObject(xobject);

                canvas.RestoreState();
            }
            catch (Exception ex)
            {
                LogHelper.Debug($"添加位置XObject失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 创建XObject组（多个XObject的组合）
        /// </summary>
        /// <param name="canvas">PdfCanvas对象</param>
        /// <param name="xobjects">XObject数组</param>
        /// <param name="layout">布局方式</param>
        public static void AddXObjectGroup(this PdfCanvas canvas, PdfFormXObject[] xobjects,
            XObjectGroupLayout layout = XObjectGroupLayout.Vertical)
        {
            try
            {
                if (xobjects == null || xobjects.Length == 0)
                    return;

                float currentX = 0;
                float currentY = 0;
                float maxWidth = 0;

                foreach (var xobject in xobjects)
                {
                    var bbox = xobject.GetBBox();
                    Rectangle bboxRect = new Rectangle(bbox.GetAsNumber(0).FloatValue(), bbox.GetAsNumber(1).FloatValue(),
                                                   bbox.GetAsNumber(2).FloatValue(), bbox.GetAsNumber(3).FloatValue());
                    float width = (float)bboxRect.GetWidth();
                    float height = (float)bboxRect.GetHeight();

                    // 添加XObject到当前位置
                    canvas.AddXObjectAt(xobject, currentX, currentY);

                    // 更新位置
                    switch (layout)
                    {
                        case XObjectGroupLayout.Horizontal:
                            currentX += width;
                            currentY = 0;
                            maxWidth = Math.Max(maxWidth, currentX);
                            break;
                        case XObjectGroupLayout.Vertical:
                            currentX = 0;
                            currentY += height;
                            maxWidth = Math.Max(maxWidth, width);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Debug($"添加XObject组失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 应用旋转矩阵变换
        /// </summary>
        private static void ApplyRotationMatrix(PdfCanvas canvas, int rotation, float pageWidth, float pageHeight)
        {
            switch (rotation)
            {
                case 90:
                    // 90度顺时针旋转：(x, y) -> (height - y, x)
                    canvas.ConcatMatrix(0, -1, 1, 0, 0, pageHeight);
                    break;
                case 180:
                    // 180度旋转：(x, y) -> (width - x, height - y)
                    canvas.ConcatMatrix(-1, 0, 0, -1, pageWidth, pageHeight);
                    break;
                case 270:
                    // 270度顺时针旋转（90度逆时针）：(x, y) -> (y, width - x)
                    canvas.ConcatMatrix(0, 1, -1, 0, pageWidth, 0);
                    break;
                default:
                    // 不支持的旋转角度，抛出异常
                    throw new ArgumentException($"不支持的旋转角度: {rotation}。仅支持0, 90, 180, 270度。");
            }
        }

        /// <summary>
        /// 创建XObject的裁剪区域
        /// </summary>
        /// <param name="canvas">PdfCanvas对象</param>
        /// <param name="xobject">要裁剪的XObject</param>
        /// <param name="clipRect">裁剪矩形</param>
        public static void AddClippedXObject(this PdfCanvas canvas, PdfFormXObject xobject, Rectangle clipRect)
        {
            try
            {
                canvas.SaveState();

                // 设置裁剪路径
                canvas.Rectangle(clipRect.GetLeft(), clipRect.GetBottom(),
                    clipRect.GetWidth(), clipRect.GetHeight());
                canvas.Clip();
                canvas.EndPath();

                // 添加XObject
                canvas.AddXObject(xobject);

                canvas.RestoreState();
            }
            catch (Exception ex)
            {
                LogHelper.Debug($"添加裁剪XObject失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 添加XObject并应用透明度
        /// </summary>
        /// <param name="canvas">PdfCanvas对象</param>
        /// <param name="xobject">要添加的XObject</param>
        /// <param name="alpha">透明度值（0.0-1.0）</param>
        public static void AddXObjectWithAlpha(this PdfCanvas canvas, PdfFormXObject xobject, float alpha)
        {
            try
            {
                canvas.SaveState();

                // 设置透明度
                canvas.SetExtGState(new iText.Kernel.Pdf.Extgstate.PdfExtGState()
                    .SetFillOpacity(alpha)
                    .SetStrokeOpacity(alpha));

                // 添加XObject
                canvas.AddXObject(xobject);

                canvas.RestoreState();
            }
            catch (Exception ex)
            {
                LogHelper.Debug($"添加透明XObject失败: {ex.Message}");
                throw;
            }
        }
    }

    /// <summary>
    /// XObject组布局方式
    /// </summary>
    public enum XObjectGroupLayout
    {
        /// <summary>
        /// 水平布局
        /// </summary>
        Horizontal,

        /// <summary>
        /// 垂直布局
        /// </summary>
        Vertical
    }

    /// <summary>
    /// XObject缓存管理器
    /// 用于管理XObject的创建和重用，提高性能
    /// </summary>
    public class XObjectCacheManager
    {
        private readonly System.Collections.Generic.Dictionary<string, PdfFormXObject> _cache;
        private readonly object _lockObject = new object();

        public XObjectCacheManager()
        {
            _cache = new System.Collections.Generic.Dictionary<string, PdfFormXObject>();
        }

        /// <summary>
        /// 获取或创建XObject
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="factory">XObject创建工厂方法</param>
        /// <returns>XObject对象</returns>
        public PdfFormXObject GetOrCreate(string key, Func<PdfFormXObject> factory)
        {
            lock (_lockObject)
            {
                if (_cache.TryGetValue(key, out var cachedXObject))
                {
                    LogHelper.Debug($"使用缓存XObject: {key}");
                    return cachedXObject;
                }

                LogHelper.Debug($"创建新XObject: {key}");
                var newXObject = factory();
                _cache[key] = newXObject;
                return newXObject;
            }
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearCache()
        {
            lock (_lockObject)
            {
                _cache.Clear();
                LogHelper.Debug("XObject缓存已清除");
            }
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        public (int Count, long EstimatedSize) GetCacheStats()
        {
            lock (_lockObject)
            {
                return (_cache.Count, EstimateCacheSize());
            }
        }

        private long EstimateCacheSize()
        {
            // 简单的缓存大小估算
            return _cache.Count * 1024; // 假设每个XObject平均1KB
        }
    }

    /// <summary>
    /// XObject变换信息
    /// 存储XObject的位置、旋转、缩放等变换信息
    /// </summary>
    public class XObjectTransform
    {
        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;
        public float ScaleX { get; set; } = 1.0f;
        public float ScaleY { get; set; } = 1.0f;
        public int Rotation { get; set; } = 0;
        public float Alpha { get; set; } = 1.0f;
        public Rectangle ClipRect { get; set; } = null;

        /// <summary>
        /// 创建标准变换矩阵
        /// </summary>
        /// <returns>变换矩阵</returns>
        public float[] GetTransformMatrix()
        {
            // 组合变换：缩放 -> 旋转 -> 平移
            float cos = (float)Math.Cos(Rotation * Math.PI / 180);
            float sin = (float)Math.Sin(Rotation * Math.PI / 180);

            return new float[]
            {
                ScaleX * cos, ScaleX * sin,
                -ScaleY * sin, ScaleY * cos,
                X, Y
            };
        }

        /// <summary>
        /// 应用变换到Canvas
        /// </summary>
        /// <param name="canvas">目标Canvas</param>
        public void ApplyToCanvas(PdfCanvas canvas)
        {
            var matrix = GetTransformMatrix();
            canvas.ConcatMatrix(matrix[0], matrix[1], matrix[2], matrix[3], matrix[4], matrix[5]);

            if (Alpha < 1.0f)
            {
                canvas.SetExtGState(new iText.Kernel.Pdf.Extgstate.PdfExtGState()
                    .SetFillOpacity(Alpha)
                    .SetStrokeOpacity(Alpha));
            }
        }
    }
}