using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using iText.Kernel.Pdf;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// PDF裁切路径提取器 - 从PDF最后一页提取裁切路径
    /// 支持的颜色操作符：K(CMYK)、RG(RGB)、G(灰度)、CS+SCN(命名颜色空间)
    /// 裁切路径坐标已在CropBox可视区域空间中，无需额外坐标变换
    /// </summary>
    public static class PdfCutPathExtractor
    {
        /// <summary>
        /// 检测PDF文件最后一页是否有可提取的裁切路径
        /// </summary>
        public static bool CanExtractCutPathFromLastPage(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return false;
                using (var reader = new PdfReader(filePath))
                using (var doc = new PdfDocument(reader))
                {
                    if (doc.GetNumberOfPages() < 2) return false;
                    var lastPage = doc.GetPage(doc.GetNumberOfPages());
                    float[] bounds;
                    byte[] pathBytes = ExtractAndConvertCutPath(lastPage, doc, out bounds);
                    return pathBytes != null && pathBytes.Length > 0;
                }
            }
            catch (Exception ex) { LogHelper.Debug("CanExtractCutPathFromLastPage: " + ex.Message); return false; }
        }

        /// <summary>
        /// 从PDF页面中提取裁切路径，并将坐标转换到页面坐标系
        /// 坐标已在可视区域(CropBox)空间中，无需ConcatMatrix偏移
        /// </summary>
        public static byte[] ExtractAndConvertCutPath(PdfPage page, PdfDocument doc, out float[] pageBounds)
        {
            pageBounds = null;
            try
            {
                PdfDictionary pageDict = page.GetPdfObject();
                PdfObject contents = pageDict.Get(PdfName.Contents);
                byte[] contentBytes = null;
                if (contents is PdfStream singleStream)
                    contentBytes = singleStream.GetBytes();
                else if (contents is PdfArray arr)
                {
                    using (var ms = new MemoryStream())
                    {
                        for (int j = 0; j < arr.Size(); j++)
                        {
                            var obj = arr.Get(j);
                            PdfStream cs = obj is PdfStream s ? s :
                                (obj is PdfIndirectReference indRef && indRef.GetRefersTo() is PdfStream indS) ? indS : null;
                            if (cs != null) { byte[] csBytes = cs.GetBytes(); ms.Write(csBytes, 0, csBytes.Length); }
                        }
                        contentBytes = ms.ToArray();
                    }
                }
                if (contentBytes == null || contentBytes.Length == 0) { LogHelper.Debug("ExtractAndConvertCutPath: content stream empty"); return null; }
                string content = System.Text.Encoding.ASCII.GetString(contentBytes);
                if (!content.Contains(" m\n") && !content.Contains(" m\r") && !content.Contains(" c\n") && !content.Contains(" c\r"))
                { LogHelper.Debug("ExtractAndConvertCutPath: no path operators"); return null; }
                string[] contentLines = content.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.None);
                float cmTx = 0, cmTy = 0; int pathColorLine = -1;
                for (int li = 0; li < contentLines.Length; li++)
                {
                    string line = contentLines[li].Trim();
                    if (string.IsNullOrEmpty(line)) continue;
                    if (line.EndsWith(" cm"))
                    {
                        string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 7)
                        {
                            float.TryParse(parts[4], NumberStyles.Float, CultureInfo.InvariantCulture, out float e);
                            float.TryParse(parts[5], NumberStyles.Float, CultureInfo.InvariantCulture, out float f);
                            cmTx += e; cmTy += f;
                        }
                    }
                    if ((line.EndsWith(" K") || line.EndsWith(" RG") || line.EndsWith(" G") || (line.Contains(" CS") && line.EndsWith(" SCN"))) && pathColorLine < 0)
                    {
                        bool hasW = false, hasM = false;
                        for (int j = Math.Max(0, li - 6); j < Math.Min(li + 6, contentLines.Length); j++)
                        { string next = contentLines[j].Trim(); if (next.EndsWith(" w")) hasW = true; if (next.EndsWith(" m")) hasM = true; }
                        if (hasW && hasM) pathColorLine = li;
                    }
                }
                if (pathColorLine < 0) { LogHelper.Debug("ExtractAndConvertCutPath: no color+w+m pattern found"); return null; }
                string colorLine = "", widthLine = ""; int pathStartLine = -1;
                for (int li = pathColorLine; li < contentLines.Length; li++)
                {
                    string line = contentLines[li].Trim();
                    if (line.EndsWith(" K") || line.EndsWith(" RG") || line.EndsWith(" G") || (line.Contains(" CS") && line.EndsWith(" SCN"))) { colorLine = line; continue; }
                    if (line.EndsWith(" w")) { widthLine = line; continue; }
                    if (line.EndsWith(" m") || line.EndsWith(" c")) { pathStartLine = li; break; }
                }
                if (pathStartLine < 0) return null;
                var pathLineList = new List<string>();
                float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
                for (int li = pathStartLine; li < contentLines.Length; li++)
                {
                    string line = contentLines[li].Trim();
                    if (string.IsNullOrEmpty(line)) continue;
                    if (line.EndsWith(" m") || line.EndsWith(" c") || line.EndsWith(" l") || line == "h")
                    { string converted = ConvertLineCoords(line, cmTx, cmTy); pathLineList.Add(converted); UpdateBounds(converted, ref minX, ref minY, ref maxX, ref maxY); }
                    else if (line == "S" || line == "s" || line == "f" || line == "f*" || line == "B" || line == "B*")
                    { pathLineList.Add(line); break; }
                    else break;
                }
                if (pathLineList.Count == 0) return null;
                pageBounds = new float[] { minX, minY, maxX, maxY };
                using (var ms = new MemoryStream())
                {
                    ms.Write(System.Text.Encoding.ASCII.GetBytes("q\n"), 0, 2);
                    byte[] colorBytes = System.Text.Encoding.ASCII.GetBytes("1 0 0 RG\n"); ms.Write(colorBytes, 0, colorBytes.Length);
                    byte[] widthBytes = System.Text.Encoding.ASCII.GetBytes("0.01 w\n"); ms.Write(widthBytes, 0, widthBytes.Length);
                    foreach (string pl in pathLineList) { byte[] plBytes = System.Text.Encoding.ASCII.GetBytes(pl + "\n"); ms.Write(plBytes, 0, plBytes.Length); }
                    ms.Write(System.Text.Encoding.ASCII.GetBytes("Q\n"), 0, 2);
                    LogHelper.Debug("ExtractAndConvertCutPath: cm=(" + cmTx + "," + cmTy + "), lines=" + pathLineList.Count + ", bounds=(" + minX + "," + minY + ")-(" + maxX + "," + maxY + ")");
                    return ms.ToArray();
                }
            }
            catch (Exception ex) { LogHelper.Debug("ExtractAndConvertCutPath: " + ex.Message); return null; }
        }

        internal static string ConvertLineCoords(string line, float cmTx, float cmTy)
        {
            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string op = parts[parts.Length - 1];
            if (op == "h") return line;
            var nums = new List<string>();
            for (int i = 0; i < parts.Length - 1; i++)
            {
                if (float.TryParse(parts[i], NumberStyles.Float, CultureInfo.InvariantCulture, out float val))
                { if (i % 2 == 0) val += cmTx; else val += cmTy; nums.Add(val.ToString(CultureInfo.InvariantCulture)); }
                else nums.Add(parts[i]);
            }
            return string.Join(" ", nums) + " " + op;
        }

        internal static void UpdateBounds(string line, ref float minX, ref float minY, ref float maxX, ref float maxY)
        {
            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string op = parts[parts.Length - 1];
            if (op == "h") return;
            for (int i = 0; i < parts.Length - 1; i++)
            {
                if (float.TryParse(parts[i], NumberStyles.Float, CultureInfo.InvariantCulture, out float val))
                { if (i % 2 == 0) { if (val < minX) minX = val; if (val > maxX) maxX = val; } else { if (val < minY) minY = val; if (val > maxY) maxY = val; } }
            }
        }
    }
}
