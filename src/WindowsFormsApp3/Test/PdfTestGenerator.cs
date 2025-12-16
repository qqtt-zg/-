using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

namespace WindowsFormsApp3.Test
{
    /// <summary>
    /// PDF测试文件生成器
    /// 用于创建测试PDF文件以验证CefSharp预览功能
    /// </summary>
    public static class PdfTestGenerator
    {
        /// <summary>
        /// 创建一个简单的测试PDF文件
        /// </summary>
        /// <param name="filePath">PDF文件保存路径</param>
        /// <param name="pageCount">页数</param>
        public static void CreateTestPdf(string filePath, int pageCount = 3)
        {
            try
            {
                // 创建输出目录
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 创建PDF文档
                using (var writer = new PdfWriter(filePath))
                using (var pdf = new PdfDocument(writer))
                using (var document = new Document(pdf))
                {
                    // 设置字体
                    var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                    // 创建多个页面
                    for (int page = 1; page <= pageCount; page++)
                    {
                        // 添加标题
                        var title = new Paragraph($"测试PDF文档 - 第{page}页")
                            .SetFont(font)
                            .SetFontSize(24)
                            .SetFontColor(ColorConstants.BLUE)
                            .SetMarginBottom(20);
                        document.Add(title);

                        // 添加内容
                        var content = new Paragraph($"这是第{page}页的内容。\n\n" +
                            $"本PDF文件用于测试CefSharp PDF预览功能。\n" +
                            $"创建时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}\n\n" +
                            $"功能测试要点：\n" +
                            $"1. 单页模式显示\n" +
                            $"2. 适应页面缩放\n" +
                            $"3. 页面导航功能\n" +
                            $"4. CefSharp渲染质量")
                            .SetFontSize(14)
                            .SetMarginBottom(20);
                        document.Add(content);

                        // 添加一些测试内容
                        var testLine = new Paragraph("=== 测试内容区域 ===")
                            .SetFont(font)
                            .SetFontSize(16)
                            .SetFontColor(ColorConstants.DARK_GRAY)
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .SetMarginTop(20)
                            .SetMarginBottom(20);
                        document.Add(testLine);

                        // 添加多行文本
                        for (int line = 1; line <= 5; line++)
                        {
                            var lineContent = new Paragraph($"这是第 {line} 行测试内容。CefSharp PDF预览功能测试。")
                                .SetFontSize(12)
                                .SetMarginLeft(20);
                            document.Add(lineContent);
                        }

                        // 添加页面底部的页码
                        var pageNumber = new Paragraph($"页码: {page} / {pageCount}")
                            .SetFontSize(10)
                            .SetFontColor(ColorConstants.GRAY)
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .SetMarginTop(30);
                        document.Add(pageNumber);

                        // 如果不是最后一页，添加分页
                        if (page < pageCount)
                        {
                            document.Add(new AreaBreak());
                        }
                    }
                }

                Console.WriteLine($"测试PDF文件已创建: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建测试PDF失败: {ex.Message}");
                throw;
            }
        }
    }
}