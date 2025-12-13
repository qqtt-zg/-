using System;
using System.Collections.Generic;
using System.IO;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// IText7PdfTools使用示例类
    /// 演示如何使用iText 7进行PDF尺寸处理
    /// </summary>
    public static class IText7PdfToolsExample
    {
        /// <summary>
        /// 完整的PDF尺寸处理示例
        /// </summary>
        /// <param name="pdfFilePath">PDF文件路径</param>
        /// <returns>处理结果信息</returns>
        public static string ProcessPdfExample(string pdfFilePath)
        {
            try
            {
                if (!File.Exists(pdfFilePath))
                    return "文件不存在";

                LogHelper.Info($"开始处理PDF文件: {pdfFilePath}");

                // 1. 获取页数
                int? pageCount = IText7PdfTools.GetPageCount(pdfFilePath);
                if (pageCount == null)
                    return "无法读取PDF页数";

                LogHelper.Info($"PDF页数: {pageCount}");

                // 2. 获取第一页尺寸
                if (IText7PdfTools.GetFirstPageSize(pdfFilePath, out double width, out double height))
                {
                    LogHelper.Info($"第一页尺寸: {width} x {height} mm");
                }
                else
                {
                    LogHelper.Error("无法获取第一页尺寸");
                }

                // 3. 获取所有页面尺寸信息
                List<PageSizeInfo> allPageSizes = IText7PdfTools.GetAllPageSizes(pdfFilePath);
                LogHelper.Info($"获取到 {allPageSizes.Count} 页的尺寸信息");

                // 显示每页信息
                foreach (var pageInfo in allPageSizes)
                {
                    LogHelper.Debug($"第{pageInfo.PageNumber}页: {pageInfo.CropBox}, 旋转: {pageInfo.Rotation}°");
                }

                // 4. 检查页面框错误
                List<PageBoxError> errors = IText7PdfTools.CheckPageBoxErrors(pdfFilePath);
                if (errors.Count > 0)
                {
                    LogHelper.Warn($"发现 {errors.Count} 个页面框错误:");
                    foreach (var error in errors)
                    {
                        LogHelper.Warn($"  - {error}");
                    }
                }
                else
                {
                    LogHelper.Info("未发现页面框错误");
                }

                // 5. 设置所有页面的CropBox为MediaBox（如果需要的话）
                if (errors.Count > 0)
                {
                    LogHelper.Info("尝试修复页面框问题...");
                    if (IText7PdfTools.SetAllCropBoxToMediaBox(pdfFilePath))
                    {
                        LogHelper.Info("页面框修复成功");
                    }
                    else
                    {
                        LogHelper.Error("页面框修复失败");
                    }
                }

                return $"处理完成: {pageCount}页, 首页尺寸: {width}x{height}mm";
            }
            catch (Exception ex)
            {
                LogHelper.Error("PDF处理过程中发生错误", ex);
                return $"处理失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 获取PDF尺寸信息（原比较PDFsharp和iText 7的方法，现统一使用iText 7）
        /// </summary>
        /// <param name="pdfFilePath">PDF文件路径</param>
        /// <returns>PDF尺寸信息</returns>
        public static string ComparePdfLibraries(string pdfFilePath)
        {
            try
            {
                LogHelper.Info("获取PDF尺寸信息（使用iText 7）");

                // 使用iText 7获取尺寸
                double itextWidth = 0, itextHeight = 0;
                bool itextSuccess = IText7PdfTools.GetFirstPageSize(pdfFilePath, out itextWidth, out itextHeight);

                string result = $"PDF文件: {Path.GetFileName(pdfFilePath)}\n";
                result += $"iText 7: {(itextSuccess ? $"{itextWidth}x{itextHeight}mm" : "失败")}\n";
                result += "注意: PDFsharp尺寸功能已迁移至iText 7";

                LogHelper.Info(result);
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取PDF尺寸信息时发生错误", ex);
                return $"获取失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 获取PDF文件的详细报告
        /// </summary>
        /// <param name="pdfFilePath">PDF文件路径</param>
        /// <returns>详细的PDF信息报告</returns>
        public static string GetPdfReport(string pdfFilePath)
        {
            try
            {
                var report = new System.Text.StringBuilder();
                report.AppendLine("=== PDF文件详细报告 ===");
                report.AppendLine($"文件路径: {pdfFilePath}");
                report.AppendLine($"文件大小: {new FileInfo(pdfFilePath).Length / 1024.0:F1} KB");
                report.AppendLine($"生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                report.AppendLine();

                // 基本信息
                int? pageCount = IText7PdfTools.GetPageCount(pdfFilePath);
                report.AppendLine($"页数: {pageCount ?? 0}");

                // 第一页信息
                if (IText7PdfTools.GetFirstPageSize(pdfFilePath, out double width, out double height))
                {
                    report.AppendLine($"首页尺寸: {width:F2} x {height:F2} mm");
                    report.AppendLine($"首页方向: {(width > height ? "横向" : "纵向")}");
                }

                // 所有页面信息
                List<PageSizeInfo> allPages = IText7PdfTools.GetAllPageSizes(pdfFilePath);
                if (allPages.Count > 0)
                {
                    report.AppendLine();
                    report.AppendLine("=== 页面详细信息 ===");

                    var portraitCount = 0;
                    var landscapeCount = 0;

                    foreach (var page in allPages)
                    {
                        report.AppendLine($"第{page.PageNumber}页: {page.CropBox.Width:F2}x{page.CropBox.Height:F2}mm " +
                                        $"({(page.CropBox.IsLandscape ? "横向" : "纵向")}, 旋转{page.Rotation}°)");

                        if (page.CropBox.IsLandscape)
                            landscapeCount++;
                        else
                            portraitCount++;
                    }

                    report.AppendLine();
                    report.AppendLine($"统计: {portraitCount}页纵向, {landscapeCount}页横向");
                }

                // 错误检查
                List<PageBoxError> errors = IText7PdfTools.CheckPageBoxErrors(pdfFilePath);
                if (errors.Count > 0)
                {
                    report.AppendLine();
                    report.AppendLine("=== 发现的页面框错误 ===");
                    foreach (var error in errors)
                    {
                        report.AppendLine($"- {error}");
                    }
                }
                else
                {
                    report.AppendLine();
                    report.AppendLine("✓ 未发现页面框错误");
                }

                report.AppendLine();
                report.AppendLine("=== 报告结束 ===");

                return report.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Error("生成PDF报告时发生错误", ex);
                return $"生成报告失败: {ex.Message}";
            }
        }
    }
}