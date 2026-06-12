using System;
using System.IO;
using System.Collections.Generic;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Layer;
using iText.Kernel.Colors;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 异形PDF处理器 - 为PDF添加裁切路径图层和出血线图层
    /// 从模板页（最后一页）提取裁切路径，应用到所有内容页
    /// </summary>
    public static class PdfSpecialShapeProcessor
    {
        /// <summary>
        /// 处理异形PDF文件：提取模板页裁切路径，添加图层，删除模板页
        /// </summary>
        public static bool ProcessSpecialShapePdf(string filePath)
        {
            LogHelper.Debug("ProcessSpecialShapePdf: " + filePath);
            try
            {
                if (!File.Exists(filePath))
                {
                    LogHelper.Debug("文件不存在: " + filePath);
                    return false;
                }

                if (!System.IO.Path.GetExtension(filePath).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                string tempFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "PDFToolCache");
                Directory.CreateDirectory(tempFolder);
                string tempFilePath = System.IO.Path.Combine(tempFolder, System.IO.Path.GetRandomFileName() + ".pdf");

                PdfReader reader = null;
                PdfWriter writer = null;
                PdfDocument document = null;

                try
                {
                    reader = new PdfReader(filePath);
                    writer = new PdfWriter(tempFilePath);
                    document = new PdfDocument(reader, writer);

                    if (document.GetNumberOfPages() < 2)
                    {
                        LogHelper.Debug("文档页数不足2页，无法执行异形处理");
                        return false;
                    }

                    List<int> originalPageRotations = new List<int>();
                    for (int i = 1; i <= document.GetNumberOfPages(); i++)
                    {
                        PdfPage page = document.GetPage(i);
                        originalPageRotations.Add(page.GetRotation());
                    }

                    int lastPageIndex = document.GetNumberOfPages();
                    PdfPage lastPage = document.GetPage(lastPageIndex);
                    int originalLastPageRotation = lastPage.GetRotation();
                    lastPage.SetRotation(0);

                    PdfLayer addCounterLayer = new PdfLayer("Dots_AddCounter", document);
                    PdfLayer bleedLayer = new PdfLayer("Dots_L_B_出血线", document);

                    Rectangle lastPageSize = lastPage.GetCropBox() ?? lastPage.GetMediaBox();
                    float templateCropLeft = (float)(lastPageSize.GetLeft());
                    float templateCropBottom = (float)(lastPageSize.GetBottom());
                    LogHelper.Debug("模板页CropBox原点: (" + templateCropLeft + ", " + templateCropBottom + ")");

                    float[] cutPathBounds;
                    byte[] convertedCutPath = PdfCutPathExtractor.ExtractAndConvertCutPath(lastPage, document, out cutPathBounds);
                    if (convertedCutPath == null || convertedCutPath.Length == 0)
                    {
                        LogHelper.Debug("无法提取裁切路径，跳过异形处理");
                        return false;
                    }

                    for (int i = 1; i <= document.GetNumberOfPages(); i++)
                    {
                        PdfPage currentPage = document.GetPage(i);
                        int originalCurrentPageRotation = currentPage.GetRotation();
                        currentPage.SetRotation(0);

                        Rectangle currentPageSize = currentPage.GetCropBox() ?? currentPage.GetMediaBox();
                        float centerX = (float)((currentPageSize.GetWidth() - lastPageSize.GetWidth()) / 2);
                        float centerY = (float)((currentPageSize.GetHeight() - lastPageSize.GetHeight()) / 2);
                        LogHelper.Debug("第" + i + "页居中位置: X=" + centerX + ", Y=" + centerY);

                        // 1. 出血线（底层）
                        {
                            float bleedOffsetX = (float)(currentPageSize.GetLeft() - templateCropLeft + centerX);
                            float bleedOffsetY = (float)(currentPageSize.GetBottom() - templateCropBottom + centerY);
                            PdfCanvas bleedCanvas = new PdfCanvas(currentPage.NewContentStreamAfter(), currentPage.GetResources(), document);
                            bleedCanvas.BeginLayer(bleedLayer);
                            bleedCanvas.ConcatMatrix(1, 0, 0, 1, bleedOffsetX, bleedOffsetY);
                            bleedCanvas.SetLineWidth(0.01f);
                            bleedCanvas.SetStrokeColor(ColorConstants.GREEN);
                            bleedCanvas.Rectangle((float)lastPageSize.GetLeft(), (float)lastPageSize.GetBottom(), (float)lastPageSize.GetWidth(), (float)lastPageSize.GetHeight());
                            bleedCanvas.Stroke();
                            bleedCanvas.EndLayer();
                            bleedCanvas.Release();
                        }

                        // 2. 裁切路径（顶层，坐标已在CropBox空间中）
                        if (convertedCutPath != null && convertedCutPath.Length > 0)
                        {
                            PdfCanvas addCounterCanvas = new PdfCanvas(currentPage.NewContentStreamAfter(), currentPage.GetResources(), document);
                            addCounterCanvas.BeginLayer(addCounterLayer);
                            PdfStream contentStream = addCounterCanvas.GetContentStream();
                            string bdcLine = System.Text.Encoding.ASCII.GetString(contentStream.GetBytes()).Trim();
                            using (var ms = new MemoryStream())
                            {
                                ms.Write(System.Text.Encoding.ASCII.GetBytes(bdcLine + "\n"), 0, bdcLine.Length + 1);
                                ms.Write(convertedCutPath, 0, convertedCutPath.Length);
                                contentStream.SetData(ms.ToArray());
                            }
                            addCounterCanvas.EndLayer();
                            addCounterCanvas.Release();
                            LogHelper.Debug("第" + i + "页 Dots_AddCounter 图层写入完成");
                        }

                        currentPage.SetRotation(originalCurrentPageRotation);
                    }

                    lastPage.SetRotation(originalLastPageRotation);
                    document.RemovePage(lastPageIndex);
                    LogHelper.Debug("删除最后一页，剩余页数: " + document.GetNumberOfPages());

                    for (int i = 1; i <= document.GetNumberOfPages(); i++)
                    {
                        document.GetPage(i).SetRotation(originalPageRotations[i - 1]);
                    }

                    LogHelper.Debug("保存修改后的文档到临时文件：" + tempFilePath);
                }
                finally
                {
                    document?.Close();
                    writer?.Close();
                    reader?.Close();
                }

                File.Delete(filePath);
                File.Move(tempFilePath, filePath);

                LogHelper.Debug("ProcessSpecialShapePdf执行GC清理，处理完成");
                GC.Collect();
                GC.WaitForPendingFinalizers();

                return true;
            }
            catch (iText.Kernel.Exceptions.PdfException pdfEx)
            {
                LogHelper.Debug("iText 7 PDF异常: " + pdfEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Debug("处理异形PDF失败: " + ex.Message);
                return false;
            }
        }
    }
}
