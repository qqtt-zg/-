using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Kernel.Pdf;
using WindowsFormsApp3.Interfaces;
using WindowsFormsApp3.Models;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// PDF信息提供者实现
    /// 使用iText7库分析PDF文件
    /// </summary>
    public class PdfInfoProvider : IPdfInfoProvider
    {
        /// <summary>
        /// 分析PDF文件信息
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>PDF文件信息</returns>
        public PdfFileInfo AnalyzePdf(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"PDF文件不存在: {filePath}");

            try
            {
                var fileInfo = new FileInfo(filePath);
                var pdfInfo = new PdfFileInfo
                {
                    FilePath = filePath,
                    FileName = Path.GetFileName(filePath),
                    FileSize = fileInfo.Length,
                    LastModified = fileInfo.LastWriteTime,
                    Errors = new List<PageBoxError>()
                };

                using (var reader = new PdfReader(filePath))
                using (var document = new PdfDocument(reader))
                {
                    // 获取页面数量
                    var pageCount = document.GetNumberOfPages();
                    pdfInfo.PageCount = pageCount;

                    if (pageCount > 0)
                    {
                        // 获取第一页尺寸
                        var firstPage = document.GetFirstPage();
                        var pageSize = firstPage.GetPageSize();

                        pdfInfo.FirstPageSize = new PageSize
                        {
                            Width = pageSize.GetWidth() * 0.3528f, // 转换为毫米
                            Height = pageSize.GetHeight() * 0.3528f
                        };

                        // 收集所有页面尺寸信息
                        pdfInfo.AllPageSizes = new List<PageSizeInfo>();

                        // 只检查前100页以提高性能
                        int pagesToCheck = Math.Min(pageCount, 100);
                        bool uniformSize = true;
                        var firstSize = pdfInfo.FirstPageSize;

                        for (int i = 1; i <= pagesToCheck; i++)
                        {
                            var page = document.GetPage(i);
                            var pagePageSize = page.GetPageSize();

                            var sizeInfo = new PageSizeInfo
                            {
                                PageNumber = i,
                                MediaBox = new PageSize
                                {
                                    Width = pagePageSize.GetWidth() * 0.3528f,
                                    Height = pagePageSize.GetHeight() * 0.3528f
                                },
                                CropBox = new PageSize
                                {
                                    Width = pagePageSize.GetWidth() * 0.3528f,
                                    Height = pagePageSize.GetHeight() * 0.3528f
                                }
                            };

                            pdfInfo.AllPageSizes.Add(sizeInfo);

                            // 检查页面尺寸是否一致
                            if (uniformSize &&
                                (Math.Abs(sizeInfo.CropBox.Width - firstSize.Width) > 0.1 ||
                                 Math.Abs(sizeInfo.CropBox.Height - firstSize.Height) > 0.1))
                            {
                                uniformSize = false;
                            }
                        }

                        // 如果页面数量超过100页，且前100页尺寸一致，假设所有页面尺寸一致
                        if (pageCount > 100 && uniformSize)
                        {
                            // 剩余页面使用第一页的尺寸
                            for (int i = 101; i <= pageCount; i++)
                            {
                                pdfInfo.AllPageSizes.Add(new PageSizeInfo
                                {
                                    PageNumber = i,
                                    MediaBox = new PageSize
                                    {
                                        Width = firstSize.Width,
                                        Height = firstSize.Height
                                    },
                                    CropBox = new PageSize
                                    {
                                        Width = firstSize.Width,
                                        Height = firstSize.Height
                                    }
                                });
                            }
                        }

                        // 检查是否有不同尺寸的页面
                        if (!uniformSize)
                        {
                            pdfInfo.Errors.Add(new PageBoxError
                            {
                                PageNumber = 1,
                                BoxType = "SizeConsistency",
                                Error = "PDF文件包含不同尺寸的页面，可能影响排版效果"
                            });
                        }
                    }
                    else
                    {
                        pdfInfo.Errors.Add(new PageBoxError
                        {
                            PageNumber = 1,
                            BoxType = "PageCount",
                            Error = "PDF文件没有页面"
                        });
                    }
                }

                return pdfInfo;
            }
            catch (Exception ex)
            {
                return new PdfFileInfo
                {
                    FilePath = filePath,
                    FileName = Path.GetFileName(filePath),
                    Errors = new List<PageBoxError>
                    {
                        new PageBoxError
                        {
                            PageNumber = 1,
                            BoxType = "AnalysisError",
                            Error = $"分析PDF文件失败: {ex.Message}"
                        }
                    }
                };
            }
        }
    }
}