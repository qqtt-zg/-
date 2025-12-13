using WindowsFormsApp3.Models;

namespace WindowsFormsApp3.Interfaces
{
    /// <summary>
    /// PDF信息提供者接口
    /// 提供PDF文件分析功能
    /// </summary>
    public interface IPdfInfoProvider
    {
        /// <summary>
        /// 分析PDF文件信息
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>PDF文件信息</returns>
        PdfFileInfo AnalyzePdf(string filePath);
    }
}