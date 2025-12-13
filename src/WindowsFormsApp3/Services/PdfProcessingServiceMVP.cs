#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp3.Models;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// PDF处理服务 - MVP模式的服务层
    /// 提供PDF处理的核心业务逻辑，支持异步处理和错误恢复
    /// </summary>
    public class PdfProcessingServiceMVP : IPdfProcessingServiceMVP
    {
        #region 私有字段

        private readonly PdfProcessingConfig _config;
        private readonly Dictionary<string, IPdfLibraryProvider> _libraryProviders;
        private readonly IPdfLibraryProvider _defaultProvider;

        #endregion

        #region 构造函数

        public PdfProcessingServiceMVP() : this(new PdfProcessingConfig())
        {
        }

        public PdfProcessingServiceMVP(PdfProcessingConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _libraryProviders = new Dictionary<string, IPdfLibraryProvider>
            {
                ["IText7"] = new IText7LibraryProvider(),
                ["PdfTools"] = new PdfToolsLibraryProvider()
            };
            _defaultProvider = _libraryProviders.ContainsKey(config.PreferredLibrary)
                ? _libraryProviders[config.PreferredLibrary]
                : _libraryProviders["IText7"];
        }

        #endregion

        #region IPdfProcessingServiceMVP 实现

        /// <summary>
        /// 异步获取PDF页数
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>页数</returns>
        public async Task<int?> GetPageCountAsync(string filePath, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithRetryAsync(
                () => _defaultProvider.GetPageCount(filePath),
                cancellationToken,
                $"获取页数失败: {Path.GetFileName(filePath)}"
            );
        }

        /// <summary>
        /// 异步获取PDF首页尺寸
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="useCropBox">是否使用CropBox</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>页面尺寸</returns>
        public async Task<PageSize?> GetFirstPageSizeAsync(
            string filePath,
            bool useCropBox = true,
            CancellationToken cancellationToken = default)
        {
            return await ExecuteWithRetryAsync(
                () => _defaultProvider.GetFirstPageSize(filePath, useCropBox),
                cancellationToken,
                $"获取首页尺寸失败: {Path.GetFileName(filePath)}"
            );
        }

        /// <summary>
        /// 异步获取所有页面尺寸信息
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>页面尺寸信息列表</returns>
        public async Task<List<PageSizeInfo>> GetAllPageSizesAsync(
            string filePath,
            CancellationToken cancellationToken = default)
        {
            return await ExecuteWithRetryAsync(
                () => _defaultProvider.GetAllPageSizes(filePath),
                cancellationToken,
                $"获取所有页面尺寸失败: {Path.GetFileName(filePath)}"
            );
        }

        /// <summary>
        /// 异步设置页面框
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="boxType">页面框类型</param>
        /// <param name="width">宽度（毫米）</param>
        /// <param name="height">高度（毫米）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否成功</returns>
        public async Task<bool> SetPageBoxAsync(
            string filePath,
            PdfBoxType boxType,
            double width,
            double height,
            CancellationToken cancellationToken = default)
        {
            return await ExecuteWithRetryAsync(
                () => _defaultProvider.SetPageBox(filePath, boxType, width, height),
                cancellationToken,
                $"设置页面框失败: {Path.GetFileName(filePath)}"
            );
        }

        /// <summary>
        /// 异步将所有页面的CropBox设置为MediaBox
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否成功</returns>
        public async Task<bool> SetAllCropBoxToMediaBoxAsync(
            string filePath,
            CancellationToken cancellationToken = default)
        {
            return await ExecuteWithRetryAsync(
                () => _defaultProvider.SetAllCropBoxToMediaBox(filePath),
                cancellationToken,
                $"设置CropBox失败: {Path.GetFileName(filePath)}"
            );
        }

        /// <summary>
        /// 异步检查页面框错误
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>页面框错误列表</returns>
        public async Task<List<PageBoxError>> CheckPageBoxErrorsAsync(
            string filePath,
            CancellationToken cancellationToken = default)
        {
            return await ExecuteWithRetryAsync(
                () => _defaultProvider.CheckPageBoxErrors(filePath),
                cancellationToken,
                $"检查页面框错误失败: {Path.GetFileName(filePath)}"
            );
        }

        /// <summary>
        /// 异步生成详细报告
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>报告内容</returns>
        public async Task<string> GenerateDetailedReportAsync(
            string filePath,
            CancellationToken cancellationToken = default)
        {
            return await ExecuteWithRetryAsync(
                () => _defaultProvider.GenerateReport(filePath),
                cancellationToken,
                $"生成报告失败: {Path.GetFileName(filePath)}"
            );
        }

        /// <summary>
        /// 异步比较PDF库的结果
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>比较结果</returns>
        public async Task<PdfLibraryComparisonResult> CompareLibrariesAsync(
            string filePath,
            CancellationToken cancellationToken = default)
        {
            var comparison = new PdfLibraryComparisonResult
            {
                FileName = Path.GetFileName(filePath)
            };

            // 并行获取两个库的结果
            var tasks = new[]
            {
                GetLibraryResultAsync("PdfTools", filePath, cancellationToken),
                GetLibraryResultAsync("IText7", filePath, cancellationToken)
            };

            var results = await Task.WhenAll(tasks);

            comparison.PdfToolsResult = results[0];
            comparison.IText7Result = results[1];

            // 分析比较结果
            AnalyzeComparisonResults(comparison);

            return comparison;
        }

        /// <summary>
        /// 获取PDF文件信息
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>PDF文件信息</returns>
        public async Task<PdfFileInfo> GetPdfFileInfoAsync(
            string filePath,
            CancellationToken cancellationToken = default)
        {
            var fileInfo = new FileInfo(filePath);
            var pdfFileInfo = new PdfFileInfo
            {
                FilePath = filePath,
                FileName = fileInfo.Name,
                FileSize = fileInfo.Length,
                LastModified = fileInfo.LastWriteTime
            };

            // 并行获取各种信息
            var task1 = GetPageCountAsync(filePath, cancellationToken);
            var task2 = GetFirstPageSizeAsync(filePath, _config.UseCropBoxByDefault, cancellationToken);
            var task3 = GetAllPageSizesAsync(filePath, cancellationToken);
            var task4 = CheckPageBoxErrorsAsync(filePath, cancellationToken);

            await Task.WhenAll(task1, task2, task3, task4);

            pdfFileInfo.PageCount = await task1;
            pdfFileInfo.FirstPageSize = await task2;
            var allPageSizes = await task3;
            pdfFileInfo.AllPageSizes = allPageSizes ?? new List<PageSizeInfo>();
            var errors = await task4;
            pdfFileInfo.Errors = errors ?? new List<PageBoxError>();

            return pdfFileInfo;
        }

        /// <summary>
        /// 批量处理PDF文件
        /// </summary>
        /// <param name="filePaths">PDF文件路径列表</param>
        /// <param name="operation">处理操作</param>
        /// <param name="progressCallback">进度回调</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果列表</returns>
        public async Task<List<PdfProcessingResponse>> ProcessBatchAsync(
            IEnumerable<string> filePaths,
            PdfProcessingOperation operation,
            Action<int, int>? progressCallback = null,
            CancellationToken cancellationToken = default)
        {
            var filePathList = filePaths.ToList();
            var results = new List<PdfProcessingResponse>();
            var totalCount = filePathList.Count;

            for (int i = 0; i < totalCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var filePath = filePathList[i];
                var response = await ProcessFileAsync(filePath, operation, cancellationToken);
                results.Add(response);

                // 报告进度
                progressCallback?.Invoke(i + 1, totalCount);
            }

            return results;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 执行带重试的操作
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="operation">要执行的操作</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>操作结果</returns>
        private async Task<T> ExecuteWithRetryAsync<T>(
            Func<T> operation,
            CancellationToken cancellationToken,
            string? errorMessage = null)
        {
            Exception? lastException = null;

            for (int attempt = 1; attempt <= _config.MaxRetryCount; attempt++)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (_config.EnableDetailedLogging)
                    {
                        LogHelper.Debug($"执行操作，尝试 {attempt}/{_config.MaxRetryCount}");
                    }

                    var result = operation();
                    return result;
                }
                catch (Exception ex) when (_config.EnableErrorRecovery && attempt < _config.MaxRetryCount)
                {
                    lastException = ex;
                    LogHelper.Warn($"操作失败，将在 { _config.RetryDelayMs }ms 后重试: {ex.Message}");

                    await Task.Delay(_config.RetryDelayMs, cancellationToken);
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    break;
                }
            }

            var finalMessage = errorMessage ?? "操作失败";
            LogHelper.Error($"{finalMessage}: {lastException?.Message}");
            throw new Exception(finalMessage, lastException);
        }

        /// <summary>
        /// 获取指定库的处理结果
        /// </summary>
        /// <param name="libraryName">库名称</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>库处理结果</returns>
        private async Task<PdfLibraryResult> GetLibraryResultAsync(
            string libraryName,
            string filePath,
            CancellationToken cancellationToken)
        {
            if (!_libraryProviders.TryGetValue(libraryName, out var provider))
            {
                return new PdfLibraryResult
                {
                    LibraryName = libraryName,
                    Success = false,
                    ErrorMessage = $"未找到库: {libraryName}"
                };
            }

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var pageSize = await Task.Run(() => provider.GetFirstPageSize(filePath, true), cancellationToken);
                stopwatch.Stop();

                return new PdfLibraryResult
                {
                    LibraryName = libraryName,
                    Success = pageSize != null,
                    Width = pageSize?.Width ?? 0,
                    Height = pageSize?.Height ?? 0,
                    ProcessingTimeMs = stopwatch.ElapsedMilliseconds
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                return new PdfLibraryResult
                {
                    LibraryName = libraryName,
                    Success = false,
                    ErrorMessage = ex.Message,
                    ProcessingTimeMs = stopwatch.ElapsedMilliseconds
                };
            }
        }

        /// <summary>
        /// 分析比较结果
        /// </summary>
        /// <param name="comparison">比较结果对象</param>
        private void AnalyzeComparisonResults(PdfLibraryComparisonResult comparison)
        {
            if (comparison.PdfToolsResult.Success && comparison.IText7Result.Success)
            {
                comparison.WidthDifference = Math.Abs(comparison.PdfToolsResult.Width - comparison.IText7Result.Width);
                comparison.HeightDifference = Math.Abs(comparison.PdfToolsResult.Height - comparison.IText7Result.Height);
                comparison.ResultsMatch = comparison.WidthDifference < 0.1 && comparison.HeightDifference < 0.1;

                if (comparison.ResultsMatch)
                {
                    comparison.Recommendation = "两个库结果一致，可任选其一";
                }
                else
                {
                    // 基于处理时间和精确度给出建议
                    if (comparison.IText7Result.ProcessingTimeMs <= comparison.PdfToolsResult.ProcessingTimeMs * 1.5)
                    {
                        comparison.Recommendation = "建议使用iText 7（更精确且性能相当）";
                    }
                    else
                    {
                        comparison.Recommendation = "建议使用PdfTools（性能更好）";
                    }
                }
            }
            else if (comparison.PdfToolsResult.Success)
            {
                comparison.Recommendation = "建议使用PdfTools（IText 7处理失败）";
            }
            else if (comparison.IText7Result.Success)
            {
                comparison.Recommendation = "建议使用iText 7（PdfTools处理失败）";
            }
            else
            {
                comparison.Recommendation = "两个库都处理失败，请检查文件完整性";
            }
        }

        /// <summary>
        /// 处理单个文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="operation">操作类型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理响应</returns>
        private async Task<PdfProcessingResponse> ProcessFileAsync(
            string filePath,
            PdfProcessingOperation operation,
            CancellationToken cancellationToken)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                object? data = null;
                bool success = false;
                string message = "";

                switch (operation)
                {
                    case PdfProcessingOperation.GetPageCount:
                        data = await GetPageCountAsync(filePath, cancellationToken);
                        success = data != null;
                        message = success ? $"页数: {data}" : "获取页数失败";
                        break;

                    case PdfProcessingOperation.GetFirstPageSize:
                        data = await GetFirstPageSizeAsync(filePath, _config.UseCropBoxByDefault, cancellationToken);
                        success = data != null;
                        message = success ? $"尺寸: {data}" : "获取尺寸失败";
                        break;

                    case PdfProcessingOperation.GetAllPageSizes:
                        data = await GetAllPageSizesAsync(filePath, cancellationToken);
                        if (data is List<PageSizeInfo> pageSizeList)
                        {
                            success = pageSizeList.Count > 0;
                            message = success ? $"获取到 {pageSizeList.Count} 页信息" : "获取页面信息失败";
                        }
                        else
                        {
                            success = false;
                            message = "获取页面信息失败";
                        }
                        break;

                    default:
                        throw new ArgumentException($"不支持的操作: {operation}");
                }

                stopwatch.Stop();

                return new PdfProcessingResponse
                {
                    Success = success,
                    Message = message,
                    Data = data,
                    ProcessingTimeMs = stopwatch.ElapsedMilliseconds
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                return new PdfProcessingResponse
                {
                    Success = false,
                    Message = "处理失败",
                    Exception = ex,
                    ProcessingTimeMs = stopwatch.ElapsedMilliseconds
                };
            }
        }

        #endregion

        #region 配置管理

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="config">新配置</param>
        public void UpdateConfig(PdfProcessingConfig config)
        {
            // 这里可以更新配置逻辑
            // 为了简单起见，暂时只是记录日志
            LogHelper.Info($"配置已更新: {config.PreferredLibrary}");
        }

        /// <summary>
        /// 获取当前配置
        /// </summary>
        /// <returns>当前配置</returns>
        public PdfProcessingConfig GetCurrentConfig()
        {
            return _config;
        }

        #endregion
    }

    #region 服务接口

    /// <summary>
    /// PDF处理服务接口
    /// </summary>
    public interface IPdfProcessingServiceMVP
    {
        Task<int?> GetPageCountAsync(string filePath, CancellationToken cancellationToken = default);
        Task<PageSize?> GetFirstPageSizeAsync(string filePath, bool useCropBox = true, CancellationToken cancellationToken = default);
        Task<List<PageSizeInfo>> GetAllPageSizesAsync(string filePath, CancellationToken cancellationToken = default);
        Task<bool> SetPageBoxAsync(string filePath, PdfBoxType boxType, double width, double height, CancellationToken cancellationToken = default);
        Task<bool> SetAllCropBoxToMediaBoxAsync(string filePath, CancellationToken cancellationToken = default);
        Task<List<PageBoxError>> CheckPageBoxErrorsAsync(string filePath, CancellationToken cancellationToken = default);
        Task<string> GenerateDetailedReportAsync(string filePath, CancellationToken cancellationToken = default);
        Task<PdfLibraryComparisonResult> CompareLibrariesAsync(string filePath, CancellationToken cancellationToken = default);
        Task<PdfFileInfo> GetPdfFileInfoAsync(string filePath, CancellationToken cancellationToken = default);
        Task<List<PdfProcessingResponse>> ProcessBatchAsync(IEnumerable<string> filePaths, PdfProcessingOperation operation, Action<int, int>? progressCallback = null, CancellationToken cancellationToken = default);
    }

    #endregion

    #region 库提供者接口

    /// <summary>
    /// PDF库提供者接口
    /// </summary>
    public interface IPdfLibraryProvider
    {
        int? GetPageCount(string filePath);
        PageSize? GetFirstPageSize(string filePath, bool useCropBox = true);
        List<PageSizeInfo> GetAllPageSizes(string filePath);
        bool SetPageBox(string filePath, PdfBoxType boxType, double width, double height);
        bool SetAllCropBoxToMediaBox(string filePath);
        List<PageBoxError> CheckPageBoxErrors(string filePath);
        string GenerateReport(string filePath);
    }

    /// <summary>
    /// iText 7库提供者
    /// </summary>
    public class IText7LibraryProvider : IPdfLibraryProvider
    {
        public int? GetPageCount(string filePath) => IText7PdfTools.GetPageCount(filePath);

        public PageSize? GetFirstPageSize(string filePath, bool useCropBox = true)
        {
            // 使用新的API，忽略useCropBox参数（新API默认使用Adobe Acrobat兼容的逻辑）
            if (IText7PdfTools.GetFirstPageSize(filePath, out double width, out double height))
            {
                return new PageSize(width, height);
            }
            return null;
        }

        public List<PageSizeInfo> GetAllPageSizes(string filePath) => IText7PdfTools.GetAllPageSizes(filePath);

        public bool SetPageBox(string filePath, PdfBoxType boxType, double width, double height)
            => IText7PdfTools.SetAllPageBoxes(filePath, boxType, width, height);

        public bool SetAllCropBoxToMediaBox(string filePath) => IText7PdfTools.SetAllCropBoxToMediaBox(filePath);

        public List<PageBoxError> CheckPageBoxErrors(string filePath) => IText7PdfTools.CheckPageBoxErrors(filePath);

        public string GenerateReport(string filePath) => IText7PdfToolsExample.GetPdfReport(filePath);
    }

    /// <summary>
    /// PdfTools库提供者（注意：PDFsharp尺寸功能已迁移至iText 7）
    /// </summary>
    public class PdfToolsLibraryProvider : IPdfLibraryProvider
    {
        public int? GetPageCount(string filePath) => IText7PdfTools.GetPageCount(filePath);

        public PageSize? GetFirstPageSize(string filePath, bool useCropBox = true)
        {
            // 使用新的API，忽略useCropBox参数（新API默认使用Adobe Acrobat兼容的逻辑）
            if (IText7PdfTools.GetFirstPageSize(filePath, out double width, out double height))
            {
                return new PageSize(width, height);
            }
            return null;
        }

        public List<PageSizeInfo> GetAllPageSizes(string filePath)
        {
            // 现在使用iText 7获取所有页面尺寸（PDFsharp功能已迁移）
            return IText7PdfTools.GetAllPageSizes(filePath);
        }

        public bool SetPageBox(string filePath, PdfBoxType boxType, double width, double height)
        {
            // PdfTools不支持设置页面框
            return false;
        }

        public bool SetAllCropBoxToMediaBox(string filePath)
        {
            // PdfTools不支持设置页面框
            return false;
        }

        public List<PageBoxError> CheckPageBoxErrors(string filePath)
        {
            // PdfTools不支持检查页面框错误
            return new List<PageBoxError>();
        }

        public string GenerateReport(string filePath)
        {
            // 简单的报告生成
            if (GetPageCount(filePath) is int pageCount && GetFirstPageSize(filePath, true) is PageSize pageSize)
            {
                return $"=== PDF文件信息 ===\n文件: {Path.GetFileName(filePath)}\n页数: {pageCount}\n尺寸: {pageSize}\n=== 报告结束 ===";
            }
            return "无法生成报告";
        }
    }

    #endregion
}