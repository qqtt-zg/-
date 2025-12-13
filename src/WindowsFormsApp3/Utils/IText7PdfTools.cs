using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.IO.Util;
using WindowsFormsApp3.Services;
using SystemPath = System.IO.Path;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// 使用iText 7库专门处理PDF尺寸的工具类
    /// 提供高精度的PDF尺寸测量、页面框处理和转换功能
    /// </summary>
    public static class IText7PdfTools
    {
        /// <summary>
        /// 获取PDF文件的页数
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>PDF页数，如果文件不是PDF或无法读取则返回null</returns>
        public static int? GetPageCount(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return null;

                if (!SystemPath.GetExtension(filePath).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                    return null;

                // 配置PdfReader以避免加密文件等异常
                var readerProperties = new iText.Kernel.Pdf.ReaderProperties();
                using (PdfReader reader = new PdfReader(filePath, readerProperties))
                using (PdfDocument document = new PdfDocument(reader))
                {
                    return document.GetNumberOfPages();
                }
            }
            catch (iText.Kernel.Exceptions.PdfException pdfEx)
            {
                LogHelper.Error($"iText7 PDF异常获取页面数失败 - 文件: {filePath}, 错误: {pdfEx.Message}");
                return null;
            }
            catch (System.IO.IOException ioEx)
            {
                LogHelper.Error($"iText7 IO异常获取页面数失败 - 文件: {filePath}, 错误: {ioEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"iText7获取PDF页面数失败 - 文件: {filePath}, 错误: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取PDF文件第一页的尺寸（毫米）- Adobe Acrobat兼容版本
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="width">输出参数：页面宽度（毫米）</param>
        /// <param name="height">输出参数：页面高度（毫米）</param>
        /// <returns>是否成功获取尺寸</returns>
        public static bool GetFirstPageSize(string filePath, out double width, out double height)
        {
            width = 0;
            height = 0;

            try
            {
                if (!File.Exists(filePath))
                    return false;

                if (!SystemPath.GetExtension(filePath).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                    return false;

                // 配置PdfReader以避免加密文件等异常
                var readerProperties = new iText.Kernel.Pdf.ReaderProperties();
                using (PdfReader reader = new PdfReader(filePath, readerProperties))
                using (PdfDocument document = new PdfDocument(reader))
                {
                    if (document.GetNumberOfPages() == 0)
                        return false;

                    PdfPage page = document.GetPage(1);

                    // 获取所有页面框用于调试
                    Rectangle mediaBox = page.GetMediaBox();
                    Rectangle cropBox = page.GetCropBox();
                    Rectangle trimBox = page.GetTrimBox();
                    Rectangle bleedBox = page.GetBleedBox();
                    Rectangle artBox = page.GetArtBox();

                    LogHelper.Debug($"页面框信息 - 文件: {filePath}");
                    LogHelper.Debug($"  MediaBox: {mediaBox?.GetWidth()}x{mediaBox?.GetHeight()}");
                    LogHelper.Debug($"  CropBox: {cropBox?.GetWidth()}x{cropBox?.GetHeight()}");
                    LogHelper.Debug($"  TrimBox: {trimBox?.GetWidth()}x{trimBox?.GetHeight()}");
                    LogHelper.Debug($"  BleedBox: {bleedBox?.GetWidth()}x{bleedBox?.GetHeight()}");
                    LogHelper.Debug($"  ArtBox: {artBox?.GetWidth()}x{artBox?.GetHeight()}");

                    // Adobe Acrobat优先显示CropBox，但会验证其有效性
                    Rectangle pageSize = null;

                    if (cropBox != null &&
                        cropBox.GetWidth() > 0 && cropBox.GetHeight() > 0 &&
                        cropBox.GetLeft() >= mediaBox?.GetLeft() && cropBox.GetTop() <= mediaBox?.GetTop() &&
                        cropBox.GetRight() <= mediaBox?.GetRight() && cropBox.GetBottom() >= mediaBox?.GetBottom())
                    {
                        pageSize = cropBox;
                        LogHelper.Debug($"使用有效的CropBox - 文件: {filePath}");
                    }
                    else
                    {
                        pageSize = mediaBox;
                        if (cropBox == null || cropBox.GetWidth() <= 0 || cropBox.GetHeight() <= 0)
                        {
                            LogHelper.Debug($"CropBox无效或为空，使用MediaBox - 文件: {filePath}");
                        }
                        else
                        {
                            LogHelper.Debug($"CropBox超出MediaBox范围，使用MediaBox - 文件: {filePath}");
                        }
                    }

                    if (pageSize == null)
                    {
                        LogHelper.Error($"无法获取页面尺寸 - 文件: {filePath}");
                        return false;
                    }

                    // 检查页面旋转并调整尺寸
                    int rotation = page.GetRotation();
                    double rawWidth = pageSize.GetWidth();
                    double rawHeight = pageSize.GetHeight();

                    if (rotation == 90 || rotation == 270)
                    {
                        // 页面旋转90度或270度时，需要交换宽高
                        width = Math.Round(rawHeight / 72 * 25.4, 2);
                        height = Math.Round(rawWidth / 72 * 25.4, 2);
                        LogHelper.Debug($"检测到页面旋转 {rotation} 度，已调整宽高 - 文件: {filePath}");
                    }
                    else
                    {
                        // iText 7中尺寸单位为点，需要转换为毫米（1点=1/72英寸，1英寸=25.4毫米）
                        width = Math.Round(rawWidth / 72 * 25.4, 2);
                        height = Math.Round(rawHeight / 72 * 25.4, 2);
                    }

                    LogHelper.Debug($"iText7获取PDF尺寸成功 - 文件: {filePath}, 尺寸: {width}x{height}mm, 使用Adobe Acrobat兼容逻辑");
                    return true;
                }
            }
            catch (iText.Kernel.Exceptions.PdfException pdfEx)
            {
                LogHelper.Error($"iText7 PDF异常获取页面尺寸失败 - 文件: {filePath}, 错误: {pdfEx.Message}");
                return false;
            }
            catch (System.IO.IOException ioEx)
            {
                LogHelper.Error($"iText7 IO异常获取页面尺寸失败 - 文件: {filePath}, 错误: {ioEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"iText7获取PDF页面尺寸失败 - 文件: {filePath}, 错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取PDF文件第一页的尺寸（毫米）- 兼容旧版本
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="width">输出参数：页面宽度（毫米）</param>
        /// <param name="height">输出参数：页面高度（毫米）</param>
        /// <param name="useCropBox">是否使用CropBox而不是MediaBox</param>
        /// <returns>是否成功获取尺寸</returns>
        [Obsolete("请使用GetFirstPageSize(string, out double, out double)")]
        public static bool GetFirstPageSize(string filePath, out double width, out double height, bool useCropBox = true)
        {
            // 忽略useCropBox参数，新API始终使用Adobe Acrobat兼容逻辑
            LogHelper.Debug($"调用过时API GetFirstPageSize(filePath, width, height, useCropBox={useCropBox})，重定向到新API");
            return GetFirstPageSize(filePath, out width, out height);
        }

        /// <summary>
        /// 获取PDF所有页面的尺寸信息
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>页面尺寸列表，每个元素包含页码和尺寸信息</returns>
        public static List<PageSizeInfo> GetAllPageSizes(string filePath)
        {
            var pageSizes = new List<PageSizeInfo>();

            try
            {
                if (!File.Exists(filePath))
                    return pageSizes;

                using (PdfDocument document = new PdfDocument(new PdfReader(filePath)))
                {
                    int pageCount = document.GetNumberOfPages();

                    for (int i = 1; i <= pageCount; i++)
                    {
                        PdfPage page = document.GetPage(i);

                        // 获取各种页面框的尺寸
                        Rectangle mediaBox = page.GetMediaBox();
                        Rectangle cropBox = page.GetCropBox();
                        Rectangle trimBox = page.GetTrimBox();
                        Rectangle bleedBox = page.GetBleedBox();
                        Rectangle artBox = page.GetArtBox();

                        var pageInfo = new PageSizeInfo
                        {
                            PageNumber = i,
                            MediaBox = ConvertToMillimeters(mediaBox),
                            CropBox = ConvertToMillimeters(cropBox ?? mediaBox),
                            TrimBox = ConvertToMillimeters(trimBox ?? cropBox ?? mediaBox),
                            BleedBox = ConvertToMillimeters(bleedBox ?? cropBox ?? mediaBox),
                            ArtBox = ConvertToMillimeters(artBox ?? cropBox ?? mediaBox),
                            Rotation = page.GetRotation()
                        };

                        pageSizes.Add(pageInfo);
                    }
                }

                LogHelper.Debug($"iText7获取所有页面尺寸成功 - 文件: {filePath}, 页数: {pageSizes.Count}");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"iText7获取所有页面尺寸失败 - 文件: {filePath}, 错误: {ex.Message}");
            }

            return pageSizes;
        }

        /// <summary>
        /// 设置所有页面的页面框为指定尺寸
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="boxType">要设置的页面框类型</param>
        /// <param name="width">宽度（毫米）</param>
        /// <param name="height">高度（毫米）</param>
        /// <returns>是否设置成功</returns>
        public static bool SetAllPageBoxes(string filePath, PdfBoxType boxType, double width, double height)
        {
            try
            {
                if (!File.Exists(filePath))
                    return false;

                string tempFilePath = SystemPath.Combine(SystemPath.GetTempPath(), $"temp_{Guid.NewGuid()}.pdf");

                using (PdfReader reader = new PdfReader(filePath))
                using (PdfWriter writer = new PdfWriter(tempFilePath))
                using (PdfDocument document = new PdfDocument(reader, writer))
                {
                    // 将毫米转换为点
                    float widthPt = (float)(width / 25.4 * 72);
                    float heightPt = (float)(height / 25.4 * 72);
                    Rectangle newBox = new Rectangle(0, 0, widthPt, heightPt);

                    int pageCount = document.GetNumberOfPages();
                    for (int i = 1; i <= pageCount; i++)
                    {
                        PdfPage page = document.GetPage(i);

                        switch (boxType)
                        {
                            case PdfBoxType.MediaBox:
                                page.SetMediaBox(newBox);
                                break;
                            case PdfBoxType.CropBox:
                                page.SetCropBox(newBox);
                                break;
                            case PdfBoxType.TrimBox:
                                page.SetTrimBox(newBox);
                                break;
                            case PdfBoxType.BleedBox:
                                page.SetBleedBox(newBox);
                                break;
                            case PdfBoxType.ArtBox:
                                page.SetArtBox(newBox);
                                break;
                        }
                    }
                }

                // 替换原文件
                File.Delete(filePath);
                File.Move(tempFilePath, filePath);

                LogHelper.Debug($"iText7设置页面框成功 - 文件: {filePath}, 框类型: {boxType}, 尺寸: {width}x{height}mm");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"iText7设置页面框失败 - 文件: {filePath}, 错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 将所有页面的CropBox设置为与MediaBox相同
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>是否设置成功</returns>
        public static bool SetAllCropBoxToMediaBox(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return false;

                string tempFilePath = SystemPath.Combine(SystemPath.GetTempPath(), $"temp_{Guid.NewGuid()}.pdf");

                using (PdfReader reader = new PdfReader(filePath))
                using (PdfWriter writer = new PdfWriter(tempFilePath))
                using (PdfDocument document = new PdfDocument(reader, writer))
                {
                    int pageCount = document.GetNumberOfPages();
                    int modifiedCount = 0;

                    for (int i = 1; i <= pageCount; i++)
                    {
                        PdfPage page = document.GetPage(i);
                        Rectangle mediaBox = page.GetMediaBox();
                        Rectangle cropBox = page.GetCropBox();

                        // 检查是否需要修改
                        if (cropBox == null || !IsEqualRectangle(mediaBox, cropBox))
                        {
                            page.SetCropBox(mediaBox);
                            modifiedCount++;

                            LogHelper.Debug($"iText7设置第{i}页CropBox为MediaBox - 尺寸: {ConvertToMillimeters(mediaBox).Width}x{ConvertToMillimeters(mediaBox).Height}mm");
                        }
                    }

                    LogHelper.Debug($"iText7设置CropBox完成 - 总页数: {pageCount}, 修改页数: {modifiedCount}");
                }

                // 替换原文件
                File.Delete(filePath);
                File.Move(tempFilePath, filePath);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"iText7设置CropBox为MediaBox失败 - 文件: {filePath}, 错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 检查PDF页面是否存在异常的页面框（如0x0尺寸）
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>异常页面信息列表</returns>
        public static List<PageBoxError> CheckPageBoxErrors(string filePath)
        {
            var errors = new List<PageBoxError>();

            try
            {
                if (!File.Exists(filePath))
                    return errors;

                using (PdfDocument document = new PdfDocument(new PdfReader(filePath)))
                {
                    int pageCount = document.GetNumberOfPages();

                    for (int i = 1; i <= pageCount; i++)
                    {
                        PdfPage page = document.GetPage(i);
                        Rectangle mediaBox = page.GetMediaBox();
                        Rectangle cropBox = page.GetCropBox();

                        // 检查MediaBox
                        if (mediaBox == null || mediaBox.GetWidth() <= 0.1 || mediaBox.GetHeight() <= 0.1)
                        {
                            errors.Add(new PageBoxError
                            {
                                PageNumber = i,
                                BoxType = "MediaBox",
                                Error = "MediaBox为空或尺寸无效",
                                Width = mediaBox?.GetWidth() ?? 0,
                                Height = mediaBox?.GetHeight() ?? 0
                            });
                        }

                        // 检查CropBox
                        if (cropBox == null || cropBox.GetWidth() <= 0.1 || cropBox.GetHeight() <= 0.1)
                        {
                            errors.Add(new PageBoxError
                            {
                                PageNumber = i,
                                BoxType = "CropBox",
                                Error = "CropBox为空或尺寸无效",
                                Width = cropBox?.GetWidth() ?? 0,
                                Height = cropBox?.GetHeight() ?? 0
                            });
                        }
                    }
                }

                if (errors.Count > 0)
                {
                    LogHelper.Warn($"iText7检测到页面框异常 - 文件: {filePath}, 异常数量: {errors.Count}");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"iText7检查页面框异常失败 - 文件: {filePath}, 错误: {ex.Message}");
            }

            return errors;
        }

        #region 私有辅助方法

        /// <summary>
        /// 将iText 7的Rectangle转换为毫米单位的尺寸
        /// </summary>
        /// <param name="rect">iText 7 Rectangle对象</param>
        /// <returns>毫米单位的尺寸</returns>
        private static PageSize ConvertToMillimeters(Rectangle rect)
        {
            if (rect == null)
                return new PageSize(0, 0);

            double width = Math.Round(rect.GetWidth() / 72 * 25.4, 2);
            double height = Math.Round(rect.GetHeight() / 72 * 25.4, 2);

            return new PageSize(width, height);
        }

        /// <summary>
        /// 比较两个Rectangle是否相等（允许小的误差）
        /// </summary>
        /// <param name="rect1">第一个矩形</param>
        /// <param name="rect2">第二个矩形</param>
        /// <returns>是否相等</returns>
        private static bool IsEqualRectangle(Rectangle rect1, Rectangle rect2)
        {
            if (rect1 == null || rect2 == null)
                return false;

            const float tolerance = 0.1f; // 允许0.1点的误差

            return Math.Abs(rect1.GetLeft() - rect2.GetLeft()) < tolerance &&
                   Math.Abs(rect1.GetBottom() - rect2.GetBottom()) < tolerance &&
                   Math.Abs(rect1.GetRight() - rect2.GetRight()) < tolerance &&
                   Math.Abs(rect1.GetTop() - rect2.GetTop()) < tolerance;
        }

        #endregion
    }

    #region 数据结构定义

    /// <summary>
    /// 页面尺寸信息
    /// </summary>
    public class PageSizeInfo
    {
        public int PageNumber { get; set; }
        public PageSize MediaBox { get; set; }
        public PageSize CropBox { get; set; }
        public PageSize TrimBox { get; set; }
        public PageSize BleedBox { get; set; }
        public PageSize ArtBox { get; set; }
        public int Rotation { get; set; }

        public override string ToString()
        {
            return $"Page {PageNumber}: {CropBox.Width}x{CropBox.Height}mm (Rotation: {Rotation}°)";
        }
    }

    /// <summary>
    /// 页面尺寸（毫米）
    /// </summary>
    public class PageSize
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public PageSize() { }

        public PageSize(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public bool IsLandscape => Width > Height;
        public bool IsPortrait => Width <= Height;

        public override string ToString()
        {
            return $"{Width}x{Height}mm";
        }
    }

    /// <summary>
    /// 页面框错误信息
    /// </summary>
    public class PageBoxError
    {
        public int PageNumber { get; set; }
        public string BoxType { get; set; }
        public string Error { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public override string ToString()
        {
            return $"Page {PageNumber} {BoxType}: {Error} ({Width}x{Height} points)";
        }
    }

    /// <summary>
    /// PDF页面框类型
    /// </summary>
    public enum PdfBoxType
    {
        MediaBox,
        CropBox,
        TrimBox,
        BleedBox,
        ArtBox
    }

    #endregion
}