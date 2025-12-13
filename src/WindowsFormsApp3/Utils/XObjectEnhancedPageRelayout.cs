using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// XObject增强的页面重排处理器
    /// 提供高级的PDF页面处理功能，特别是针对旋转页面的XObject优化
    /// </summary>
    public static class XObjectEnhancedPageRelayout
    {
        /// <summary>
        /// 使用XObject增强处理PDF文件
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="preserveOriginalRotation">是否保留原始旋转信息</param>
        /// <returns>处理是否成功</returns>
        public static bool ProcessWithXObjectEnhancement(string filePath, bool preserveOriginalRotation = false)
        {
            try
            {
                LogHelper.Debug($"开始XObject增强页面重排: {filePath}, 保留旋转: {preserveOriginalRotation}");

                string tempFilePath = filePath + ".xobject_relayout";
                bool success = false;

                using (var reader = new PdfReader(filePath))
                using (var writer = new PdfWriter(tempFilePath))
                using (var document = new PdfDocument(reader, writer))
                {
                    var xobjectCache = new Dictionary<int, PdfFormXObject>();
                    var rotationMap = new Dictionary<int, int>();

                    // 第一遍：分析和处理所有页面
                    for (int i = 1; i <= document.GetNumberOfPages(); i++)
                    {
                        var page = document.GetPage(i);
                        int rotation = page.GetRotation();

                        if (rotation != 0)
                        {
                            LogHelper.Debug($"页面{i}有{rotation}度旋转，创建XObject");

                            // 创建XObject以保留原始内容
                            var xobject = page.CopyAsFormXObject(document);
                            if (xobject != null)
                            {
                                xobjectCache[i] = xobject;
                                rotationMap[i] = rotation;

                                // 清除页面旋转，XObject将负责旋转显示
                                page.SetRotation(0);
                                LogHelper.Debug($"页面{i}旋转已清除，XObject已创建");
                            }
                        }
                    }

                    // 第二遍：应用XObject到需要旋转的页面
                    foreach (var kvp in xobjectCache)
                    {
                        int pageIndex = kvp.Key;
                        var xobject = kvp.Value;
                        int rotation = rotationMap[pageIndex];
                        var page = document.GetPage(pageIndex);

                        // 在页面内容流之前添加XObject
                        var canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), document);

                        // 应用正确的变换矩阵
                        ApplyRotationTransformation(canvas, rotation, page);

                        // 添加XObject
                        canvas.AddXObject(xobject);

                        LogHelper.Debug($"页面{pageIndex}XObject应用完成，旋转{rotation}度");
                    }

                    document.Close();
                    success = true;
                }

                // 替换原文件
                if (success && File.Exists(tempFilePath))
                {
                    File.Delete(filePath);
                    File.Move(tempFilePath, filePath);
                    LogHelper.Debug("XObject增强页面重排完成");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Debug($"XObject增强页面重排失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 应用旋转变换矩阵到Canvas
        /// </summary>
        /// <param name="canvas">PDF Canvas</param>
        /// <param name="rotation">旋转角度</param>
        /// <param name="page">PDF页面</param>
        private static void ApplyRotationTransformation(PdfCanvas canvas, int rotation, PdfPage page)
        {
            try
            {
                float pageWidth = page.GetPageSize().GetWidth();
                float pageHeight = page.GetPageSize().GetHeight();

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

                LogHelper.Debug($"应用{rotation}度旋转变换矩阵完成");
            }
            catch (Exception ex)
            {
                LogHelper.Debug($"应用旋转变换矩阵失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 检查PDF文件是否包含旋转页面
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>是否包含旋转页面</returns>
        public static bool HasRotatedPages(string filePath)
        {
            try
            {
                using var reader = new PdfReader(filePath);
                using var document = new PdfDocument(reader);

                for (int i = 1; i <= document.GetNumberOfPages(); i++)
                {
                    if (document.GetPage(i).GetRotation() != 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Debug($"检查旋转页面失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取PDF文件中所有旋转页面的信息
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>旋转页面信息字典，键为页码，值为旋转角度</returns>
        public static Dictionary<int, int> GetRotatedPagesInfo(string filePath)
        {
            var rotationInfo = new Dictionary<int, int>();

            try
            {
                using var reader = new PdfReader(filePath);
                using var document = new PdfDocument(reader);

                for (int i = 1; i <= document.GetNumberOfPages(); i++)
                {
                    int rotation = document.GetPage(i).GetRotation();
                    if (rotation != 0)
                    {
                        rotationInfo[i] = rotation;
                    }
                }

                LogHelper.Debug($"发现{rotationInfo.Count}个旋转页面");
            }
            catch (Exception ex)
            {
                LogHelper.Debug($"获取旋转页面信息失败: {ex.Message}");
            }

            return rotationInfo;
        }

        /// <summary>
        /// 分析PDF文件的旋转页面情况
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>分析报告</returns>
        public static string AnalyzeRotatedPages(string filePath)
        {
            try
            {
                var report = new System.Text.StringBuilder();
                report.AppendLine("=== XObject增强页面分析报告 ===");
                report.AppendLine($"文件路径: {filePath}");
                report.AppendLine($"分析时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                report.AppendLine();

                var rotationInfo = GetRotatedPagesInfo(filePath);

                if (rotationInfo.Count == 0)
                {
                    report.AppendLine("✅ 该文件不包含旋转页面");
                    report.AppendLine("建议: 无需使用XObject增强处理");
                }
                else
                {
                    report.AppendLine($"⚠️ 发现 {rotationInfo.Count} 个旋转页面:");

                    foreach (var kvp in rotationInfo)
                    {
                        report.AppendLine($"  第{kvp.Key}页: {kvp.Value}° 旋转");
                    }

                    report.AppendLine();
                    report.AppendLine("建议: 使用XObject增强处理方法");
                    report.AppendLine("优势: ");
                    report.AppendLine("  • 更好的旋转兼容性");
                    report.AppendLine("  • 保持原始内容质量");
                    report.AppendLine("  • 支持所有标准旋转角度");
                    report.AppendLine("  • 自动变换矩阵计算");
                }

                return report.ToString();
            }
            catch (Exception ex)
            {
                return $"分析失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 验证XObject处理结果
        /// </summary>
        /// <param name="filePath">处理后的PDF文件路径</param>
        /// <returns>验证结果</returns>
        public static bool ValidateXObjectProcessing(string filePath)
        {
            try
            {
                using var reader = new PdfReader(filePath);
                using var document = new PdfDocument(reader);

                // 检查文档是否有页面
                if (document.GetNumberOfPages() == 0)
                {
                    LogHelper.Debug("验证失败: 文档没有页面");
                    return false;
                }

                // 检查页面是否都没有旋转（XObject处理后应该清除所有页面旋转）
                for (int i = 1; i <= document.GetNumberOfPages(); i++)
                {
                    int rotation = document.GetPage(i).GetRotation();
                    if (rotation != 0)
                    {
                        LogHelper.Debug($"验证失败: 第{i}页仍有{rotation}度旋转");
                        return false;
                    }
                }

                LogHelper.Debug("XObject处理验证通过");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Debug($"XObject处理验证失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 批量处理多个PDF文件
        /// </summary>
        /// <param name="filePaths">PDF文件路径列表</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>处理结果字典，键为文件路径，值为是否成功</returns>
        public static Dictionary<string, bool> BatchProcessWithXObjectEnhancement(
            IEnumerable<string> filePaths,
            Action<int, int, string> progressCallback = null)
        {
            var results = new Dictionary<string, bool>();
            var fileList = filePaths.ToList();
            int total = fileList.Count;
            int processed = 0;

            foreach (var filePath in fileList)
            {
                try
                {
                    progressCallback?.Invoke(processed, total, $"正在处理: {System.IO.Path.GetFileName(filePath)}");

                    bool success = ProcessWithXObjectEnhancement(filePath);
                    results[filePath] = success;

                    processed++;
                    progressCallback?.Invoke(processed, total,
                        success ? $"完成: {System.IO.Path.GetFileName(filePath)}" : $"失败: {System.IO.Path.GetFileName(filePath)}");
                }
                catch (Exception ex)
                {
                    LogHelper.Debug($"批量处理失败: {System.IO.Path.GetFileName(filePath)} - {ex.Message}");
                    results[filePath] = false;
                    processed++;
                }
            }

            LogHelper.Debug($"批量XObject处理完成，成功: {results.Values.Count(v => v)}/{total}");
            return results;
        }
    }
}