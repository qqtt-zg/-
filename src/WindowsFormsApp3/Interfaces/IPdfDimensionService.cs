using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Interfaces
{
    /// <summary>
    /// PDF尺寸服务统一接口
    /// 项目中所有PDF尺寸相关的操作都应该通过这个接口进行
    /// 统一使用IText7PdfTools作为底层实现
    /// </summary>
    public interface IPdfDimensionService
    {
        /// <summary>
        /// 获取PDF文件的页数
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>PDF页数，如果文件不是PDF或无法读取则返回null</returns>
        int? GetPageCount(string filePath);

        /// <summary>
        /// 异步获取PDF文件的页数
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>PDF页数，如果文件不是PDF或无法读取则返回null</returns>
        Task<int?> GetPageCountAsync(string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取PDF文件第一页的尺寸（毫米）
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="width">输出参数：页面宽度（毫米）</param>
        /// <param name="height">输出参数：页面高度（毫米）</param>
        /// <param name="useCropBox">是否使用CropBox而不是MediaBox</param>
        /// <returns>是否成功获取尺寸</returns>
        bool GetFirstPageSize(string filePath, out double width, out double height, bool useCropBox = true);

        /// <summary>
        /// 异步获取PDF文件第一页的尺寸（毫米）
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="useCropBox">是否使用CropBox而不是MediaBox</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>页面尺寸，如果获取失败则返回null</returns>
        Task<(double Width, double Height)?> GetFirstPageSizeAsync(
            string filePath,
            bool useCropBox = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取PDF所有页面的尺寸信息
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>页面尺寸列表，每个元素包含页码和尺寸信息</returns>
        List<PageSizeInfo> GetAllPageSizes(string filePath);

        /// <summary>
        /// 异步获取PDF所有页面的尺寸信息
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>页面尺寸列表，每个元素包含页码和尺寸信息</returns>
        Task<List<PageSizeInfo>> GetAllPageSizesAsync(
            string filePath,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 设置所有页面的页面框为指定尺寸
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="boxType">要设置的页面框类型</param>
        /// <param name="width">宽度（毫米）</param>
        /// <param name="height">高度（毫米）</param>
        /// <returns>是否设置成功</returns>
        bool SetAllPageBoxes(string filePath, PdfBoxType boxType, double width, double height);

        /// <summary>
        /// 异步设置所有页面的页面框为指定尺寸
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="boxType">要设置的页面框类型</param>
        /// <param name="width">宽度（毫米）</param>
        /// <param name="height">高度（毫米）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetAllPageBoxesAsync(
            string filePath,
            PdfBoxType boxType,
            double width,
            double height,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 将所有页面的CropBox设置为与MediaBox相同
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>是否设置成功</returns>
        bool SetAllCropBoxToMediaBox(string filePath);

        /// <summary>
        /// 异步将所有页面的CropBox设置为与MediaBox相同
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetAllCropBoxToMediaBoxAsync(
            string filePath,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 检查PDF页面是否存在异常的页面框（如0x0尺寸）
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>异常页面信息列表</returns>
        List<PageBoxError> CheckPageBoxErrors(string filePath);

        /// <summary>
        /// 异步检查PDF页面是否存在异常的页面框（如0x0尺寸）
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>异常页面信息列表</returns>
        Task<List<PageBoxError>> CheckPageBoxErrorsAsync(
            string filePath,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 生成PDF文件的详细报告
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>详细的PDF信息报告</returns>
        string GeneratePdfReport(string filePath);

        /// <summary>
        /// 异步生成PDF文件的详细报告
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>详细的PDF信息报告</returns>
        Task<string> GeneratePdfReportAsync(
            string filePath,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 比较两个PDF库获取的PDF尺寸
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>比较结果</returns>
        string ComparePdfLibraries(string filePath);

        /// <summary>
        /// 检查PDF文件中是否存在指定的图层
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="layerNames">要检查的图层名称列表</param>
        /// <returns>如果所有指定图层都存在则返回true，否则返回false</returns>
        bool CheckPdfLayersExist(string filePath, params string[] layerNames);

        /// <summary>
        /// 异步比较两个PDF库获取的PDF尺寸
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>比较结果</returns>
        Task<string> ComparePdfLibrariesAsync(
            string filePath,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// PDF尺寸服务的默认实现，基于IText7PdfTools
    /// </summary>
    public class PdfDimensionService : IPdfDimensionService
    {
        /// <summary>
        /// 获取PDF文件的页数
        /// </summary>
        public int? GetPageCount(string filePath)
        {
            return IText7PdfTools.GetPageCount(filePath);
        }

        /// <summary>
        /// 异步获取PDF文件的页数
        /// </summary>
        public async Task<int?> GetPageCountAsync(string filePath, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => GetPageCount(filePath), cancellationToken);
        }

        /// <summary>
        /// 获取PDF文件第一页的尺寸（毫米）
        /// </summary>
        public bool GetFirstPageSize(string filePath, out double width, out double height, bool useCropBox = true)
        {
            // 使用新的API，忽略useCropBox参数（新API默认使用Adobe Acrobat兼容的逻辑）
            return IText7PdfTools.GetFirstPageSize(filePath, out width, out height);
        }

        /// <summary>
        /// 异步获取PDF文件第一页的尺寸（毫米）
        /// </summary>
        public async Task<(double Width, double Height)?> GetFirstPageSizeAsync(
            string filePath,
            bool useCropBox = true,
            CancellationToken cancellationToken = default)
        {
            return await Task.Run<(double Width, double Height)?>(() =>
            {
                if (GetFirstPageSize(filePath, out double width, out double height, useCropBox))
                {
                    return (width, height);
                }
                return null;
            }, cancellationToken);
        }

        /// <summary>
        /// 获取PDF所有页面的尺寸信息
        /// </summary>
        public List<PageSizeInfo> GetAllPageSizes(string filePath)
        {
            return IText7PdfTools.GetAllPageSizes(filePath);
        }

        /// <summary>
        /// 异步获取PDF所有页面的尺寸信息
        /// </summary>
        public async Task<List<PageSizeInfo>> GetAllPageSizesAsync(
            string filePath,
            CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => GetAllPageSizes(filePath), cancellationToken);
        }

        /// <summary>
        /// 设置所有页面的页面框为指定尺寸
        /// </summary>
        public bool SetAllPageBoxes(string filePath, PdfBoxType boxType, double width, double height)
        {
            return IText7PdfTools.SetAllPageBoxes(filePath, boxType, width, height);
        }

        /// <summary>
        /// 异步设置所有页面的页面框为指定尺寸
        /// </summary>
        public async Task<bool> SetAllPageBoxesAsync(
            string filePath,
            PdfBoxType boxType,
            double width,
            double height,
            CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => SetAllPageBoxes(filePath, boxType, width, height), cancellationToken);
        }

        /// <summary>
        /// 将所有页面的CropBox设置为与MediaBox相同
        /// </summary>
        public bool SetAllCropBoxToMediaBox(string filePath)
        {
            return IText7PdfTools.SetAllCropBoxToMediaBox(filePath);
        }

        /// <summary>
        /// 异步将所有页面的CropBox设置为与MediaBox相同
        /// </summary>
        public async Task<bool> SetAllCropBoxToMediaBoxAsync(
            string filePath,
            CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => SetAllCropBoxToMediaBox(filePath), cancellationToken);
        }

        /// <summary>
        /// 检查PDF页面是否存在异常的页面框（如0x0尺寸）
        /// </summary>
        public List<PageBoxError> CheckPageBoxErrors(string filePath)
        {
            return IText7PdfTools.CheckPageBoxErrors(filePath);
        }

        /// <summary>
        /// 异步检查PDF页面是否存在异常的页面框（如0x0尺寸）
        /// </summary>
        public async Task<List<PageBoxError>> CheckPageBoxErrorsAsync(
            string filePath,
            CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => CheckPageBoxErrors(filePath), cancellationToken);
        }

        /// <summary>
        /// 生成PDF文件的详细报告
        /// </summary>
        public string GeneratePdfReport(string filePath)
        {
            return IText7PdfToolsExample.GetPdfReport(filePath);
        }

        /// <summary>
        /// 异步生成PDF文件的详细报告
        /// </summary>
        public async Task<string> GeneratePdfReportAsync(
            string filePath,
            CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => GeneratePdfReport(filePath), cancellationToken);
        }

        /// <summary>
        /// 比较两个PDF库获取的PDF尺寸
        /// </summary>
        public string ComparePdfLibraries(string filePath)
        {
            return IText7PdfToolsExample.ComparePdfLibraries(filePath);
        }

        /// <summary>
        /// 异步比较两个PDF库获取的PDF尺寸
        /// </summary>
        public async Task<string> ComparePdfLibrariesAsync(
            string filePath,
            CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => ComparePdfLibraries(filePath), cancellationToken);
        }

        /// <summary>
        /// 检查PDF文件中是否存在指定的图层
        /// 注意：此方法使用PdfTools实现，因为它需要处理PDF图层（非尺寸测量功能）
        /// </summary>
        public bool CheckPdfLayersExist(string filePath, params string[] layerNames)
        {
            return PdfTools.CheckPdfLayersExist(filePath, layerNames);
        }
    }

    /// <summary>
    /// PDF尺寸服务工厂
    /// </summary>
    public static class PdfDimensionServiceFactory
    {
        private static IPdfDimensionService _instance;

        /// <summary>
        /// 获取PDF尺寸服务实例（单例模式）
        /// </summary>
        /// <returns>PDF尺寸服务实例</returns>
        public static IPdfDimensionService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PdfDimensionService();
            }
            return _instance;
        }

        /// <summary>
        /// 设置自定义的PDF尺寸服务实例（用于测试或依赖注入）
        /// </summary>
        /// <param name="service">自定义服务实例</param>
        public static void SetInstance(IPdfDimensionService service)
        {
            _instance = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// 重置服务实例（主要用于测试）
        /// </summary>
        public static void ResetInstance()
        {
            _instance = null;
        }
    }
}