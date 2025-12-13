using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp3.Interfaces;
using WindowsFormsApp3.Models;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Presenters
{
    /// <summary>
    /// PDF处理Presenter - MVP模式的Presenter层
    /// 负责协调View和Model，处理业务逻辑
    /// </summary>
    public class PdfProcessingPresenter
    {
        #region 私有字段

        private IPdfProcessingView _view;
        private readonly PdfProcessingService _processingService;
        private readonly Dictionary<string, CancellationTokenSource> _activeTasks;
        private readonly PdfProcessingStatistics _statistics;
        private PdfProcessingConfig _currentConfig;

        #endregion

        #region 构造函数

        public PdfProcessingPresenter()
        {
            _processingService = new PdfProcessingService();
            _activeTasks = new Dictionary<string, CancellationTokenSource>();
            _statistics = new PdfProcessingStatistics();
            _currentConfig = new PdfProcessingConfig();
        }

        /// <summary>
        /// 依赖注入构造函数
        /// </summary>
        /// <param name="processingService">PDF处理服务</param>
        public PdfProcessingPresenter(PdfProcessingService processingService)
        {
            _processingService = processingService ?? throw new ArgumentNullException(nameof(processingService));
            _activeTasks = new Dictionary<string, CancellationTokenSource>();
            _statistics = new PdfProcessingStatistics();
            _currentConfig = new PdfProcessingConfig();
        }

        #endregion

        #region View管理

        /// <summary>
        /// 设置View并订阅事件
        /// </summary>
        /// <param name="view">PDF处理视图</param>
        public void SetView(IPdfProcessingView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));

            // 订阅View事件
            SubscribeToViewEvents();

            // 初始化View
            InitializeView();
        }

        private void SubscribeToViewEvents()
        {
            _view.ProcessPdfRequested += OnProcessPdfRequested;
            _view.GetPageInfoRequested += OnGetPageInfoRequested;
            _view.CompareLibrariesRequested += OnCompareLibrariesRequested;
            _view.GenerateReportRequested += OnGenerateReportRequested;
            _view.CancelProcessingRequested += OnCancelProcessingRequested;
            _view.ConfigUpdated += OnConfigUpdated;
        }

        private void InitializeView()
        {
            // 加载默认配置
            var defaultConfig = new PdfProcessingConfig();
            _view.LoadConfig(defaultConfig);
            _currentConfig = defaultConfig;

            // 初始化统计信息
            UpdateStatisticsDisplay();
        }

        #endregion

        #region 事件处理器

        private async void OnProcessPdfRequested(object sender, string filePath)
        {
            await ProcessPdfAsync(filePath, PdfProcessingOperation.GetAllPageSizes);
        }

        private async void OnGetPageInfoRequested(object sender, string filePath)
        {
            await ProcessPdfAsync(filePath, PdfProcessingOperation.GetFirstPageSize);
        }

        private async void OnCompareLibrariesRequested(object sender, string filePath)
        {
            await ProcessPdfAsync(filePath, PdfProcessingOperation.CompareLibraries);
        }

        private async void OnGenerateReportRequested(object sender, string filePath)
        {
            await ProcessPdfAsync(filePath, PdfProcessingOperation.GenerateReport);
        }

        private void OnCancelProcessingRequested(object sender, string requestId)
        {
            CancelProcessing(requestId);
        }

        private void OnConfigUpdated(object sender, PdfProcessingConfig config)
        {
            if (ValidateConfig(config))
            {
                _currentConfig = config;
                _view.AddLogMessage($"配置已更新: {config.PreferredLibrary} 库优先", LogLevel.Information);
            }
            else
            {
                _view.ShowError("配置验证失败，已恢复到默认配置");
                _view.ResetConfigToDefault();
            }
        }

        #endregion

        #region 核心处理方法

        /// <summary>
        /// 异步处理PDF文件
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="operation">操作类型</param>
        /// <returns>异步任务</returns>
        private async Task ProcessPdfAsync(string filePath, PdfProcessingOperation operation)
        {
            // 验证输入
            var validationResult = ValidateProcessingRequest(filePath, operation);
            if (!validationResult.IsValid)
            {
                _view.ShowError(validationResult.ErrorMessage);
                return;
            }

            // 创建请求ID
            var requestId = Guid.NewGuid().ToString();
            var request = new PdfProcessingRequest
            {
                FilePath = filePath,
                Operation = operation,
                RequestId = requestId,
                Parameters = _view.GetProcessingParameters()
            };

            // 创建取消令牌
            var cts = new CancellationTokenSource();
            _activeTasks[requestId] = cts;

            try
            {
                // 更新UI状态
                _view.SetProcessingState(true);
                _view.ClearResults();
                _view.AddLogMessage($"开始处理: {Path.GetFileName(filePath)} ({operation})", LogLevel.Information);

                // 执行处理
                var stopwatch = Stopwatch.StartNew();
                var response = await ExecuteProcessingAsync(request, cts.Token);
                stopwatch.Stop();

                // 更新响应信息
                response.ProcessingTimeMs = stopwatch.ElapsedMilliseconds;
                response.RequestId = requestId;

                // 更新统计信息
                UpdateStatistics(response, operation);

                // 显示结果
                DisplayResults(response, operation);

                _view.AddLogMessage($"处理完成，耗时: {response.ProcessingTimeMs} ms", LogLevel.Information);
            }
            catch (OperationCanceledException)
            {
                _view.AddLogMessage("处理已取消", LogLevel.Warning);
                _view.ShowWarning("处理已取消");
            }
            catch (Exception ex)
            {
                _view.AddLogMessage($"处理异常: {ex.Message}", LogLevel.Error);
                _view.ShowError($"处理失败: {ex.Message}");

                // 更新统计信息
                _statistics.Failed++;
                _statistics.LastUpdated = DateTime.Now;
            }
            finally
            {
                // 清理资源
                _activeTasks.Remove(requestId);
                _view.SetProcessingState(false);
                UpdateStatisticsDisplay();
            }
        }

        /// <summary>
        /// 执行具体的PDF处理操作
        /// </summary>
        /// <param name="request">处理请求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理响应</returns>
        private async Task<PdfProcessingResponse> ExecuteProcessingAsync(
            PdfProcessingRequest request,
            CancellationToken cancellationToken)
        {
            var startTime = DateTime.Now;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                switch (request.Operation)
                {
                    case PdfProcessingOperation.GetPageCount:
                        return await GetPageCountAsync(request, cancellationToken);

                    case PdfProcessingOperation.GetFirstPageSize:
                        return await GetFirstPageSizeAsync(request, cancellationToken);

                    case PdfProcessingOperation.GetAllPageSizes:
                        return await GetAllPageSizesAsync(request, cancellationToken);

                    case PdfProcessingOperation.CompareLibraries:
                        return await CompareLibrariesAsync(request, cancellationToken);

                    case PdfProcessingOperation.GenerateReport:
                        return await GenerateReportAsync(request, cancellationToken);

                    default:
                        throw new ArgumentException($"不支持的操作类型: {request.Operation}");
                }
            }
            catch (Exception ex)
            {
                return new PdfProcessingResponse
                {
                    Success = false,
                    Message = "处理失败",
                    Exception = ex,
                    RequestId = request.RequestId,
                    ProcessingTimeMs = (long)(DateTime.Now - startTime).TotalMilliseconds
                };
            }
        }

        #endregion

        #region 具体操作实现

        private async Task<PdfProcessingResponse> GetPageCountAsync(
            PdfProcessingRequest request,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            UpdateProgress(0, "正在获取页数...");

            var pageCount = await Task.Run(() => _processingService.GetPdfPageCount(request.FilePath), cancellationToken);

            UpdateProgress(100, "页数获取完成");

            return new PdfProcessingResponse
            {
                Success = pageCount > 0,
                Message = pageCount > 0 ? $"成功获取页数: {pageCount}" : "获取页数失败",
                Data = pageCount,
                RequestId = request.RequestId
            };
        }

        private async Task<PdfProcessingResponse> GetFirstPageSizeAsync(
            PdfProcessingRequest request,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            UpdateProgress(0, "正在获取首页尺寸...");

            var useCropBox = request.Parameters.ContainsKey("UseCropBox") &&
                           (bool)request.Parameters["UseCropBox"];

            var result = await Task.Run(() =>
            {
                bool success = _processingService.GetFirstPageSize(
                    request.FilePath,
                    out double width,
                    out double height);
                return (Success: success, Width: width, Height: height);
            }, cancellationToken);

            UpdateProgress(100, "首页尺寸获取完成");

            if (result.Success)
            {
                var pageSize = new PageSize(result.Width, result.Height);
                return new PdfProcessingResponse
                {
                    Success = true,
                    Message = $"首页尺寸: {result.Width} x {result.Height} mm",
                    Data = pageSize,
                    RequestId = request.RequestId
                };
            }

            return new PdfProcessingResponse
            {
                Success = false,
                Message = "获取首页尺寸失败",
                RequestId = request.RequestId
            };
        }

        private async Task<PdfProcessingResponse> GetAllPageSizesAsync(
            PdfProcessingRequest request,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // 获取文件信息
            UpdateProgress(10, "正在读取文件信息...");
            var fileInfo = await GetPdfFileInfoAsync(request.FilePath, cancellationToken);

            UpdateProgress(100, "文件信息获取完成");

            return new PdfProcessingResponse
            {
                Success = fileInfo != null,
                Message = fileInfo != null ? "文件信息获取成功" : "文件信息获取失败",
                Data = fileInfo,
                RequestId = request.RequestId
            };
        }

        private async Task<PdfProcessingResponse> CompareLibrariesAsync(
            PdfProcessingRequest request,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            UpdateProgress(0, "正在使用iText 7获取尺寸...");

            // 使用IText7获取尺寸（PDFsharp尺寸功能已迁移至iText 7）
            var itext7Result = await Task.Run(() =>
            {
                bool success = IText7PdfTools.GetFirstPageSize(request.FilePath, out double width, out double height);
                return (Success: success, Width: width, Height: height);
            }, cancellationToken);

            UpdateProgress(100, "尺寸获取完成");

            // 构建结果（现在只使用iText 7）
            var comparison = new PdfLibraryComparisonResult
            {
                FileName = Path.GetFileName(request.FilePath),
                IText7Result = new PdfLibraryResult
                {
                    LibraryName = "IText7",
                    Success = itext7Result.Success,
                    Width = itext7Result.Width,
                    Height = itext7Result.Height
                },
                ResultsMatch = true, // 只有一个库，默认匹配
                Recommendation = "PDF尺寸功能已统一使用iText 7"
            };

            return new PdfProcessingResponse
            {
                Success = itext7Result.Success,
                Message = itext7Result.Success ? $"尺寸获取完成: {itext7Result.Width}x{itext7Result.Height}mm" : "尺寸获取失败",
                Data = comparison,
                RequestId = request.RequestId
            };
        }

        private async Task<PdfProcessingResponse> GenerateReportAsync(
            PdfProcessingRequest request,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            UpdateProgress(0, "正在生成报告...");

            // 使用IText7示例类生成报告 - 使用Task.Run包装CPU密集操作
            var report = await Task.Run(() => IText7PdfToolsExample.GetPdfReport(request.FilePath), cancellationToken);

            UpdateProgress(100, "报告生成完成");

            return new PdfProcessingResponse
            {
                Success = !string.IsNullOrEmpty(report),
                Message = "报告生成完成",
                Data = report,
                RequestId = request.RequestId
            };
        }

        #endregion

        #region 辅助方法

        private async Task<PdfFileInfo> GetPdfFileInfoAsync(string filePath, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                var fileInfo = new FileInfo(filePath);
                var pdfFileInfo = new PdfFileInfo
                {
                    FilePath = filePath,
                    FileName = fileInfo.Name,
                    FileSize = fileInfo.Length,
                    LastModified = fileInfo.LastWriteTime
                };

                // 获取页数
                pdfFileInfo.PageCount = IText7PdfTools.GetPageCount(filePath);

                // 获取首页尺寸
                if (IText7PdfTools.GetFirstPageSize(filePath, out double width, out double height))
                {
                    pdfFileInfo.FirstPageSize = new PageSize(width, height);
                }

                // 获取所有页面尺寸
                pdfFileInfo.AllPageSizes = IText7PdfTools.GetAllPageSizes(filePath);

                // 检查页面框错误
                pdfFileInfo.Errors = IText7PdfTools.CheckPageBoxErrors(filePath);

                return pdfFileInfo;
            }, cancellationToken);
        }

        private void UpdateProgress(int percentage, string message)
        {
            var progress = new PdfProcessingProgress
            {
                ProgressPercentage = percentage,
                CurrentOperation = message,
                Message = message,
                Status = percentage == 100 ? PdfProcessingStatus.Completed : PdfProcessingStatus.Processing
            };

            _view.ShowProcessingProgress(progress);
        }

        private void UpdateStatistics(PdfProcessingResponse response, PdfProcessingOperation operation)
        {
            _statistics.TotalProcessed++;

            if (response.Success)
            {
                _statistics.Successful++;
            }
            else
            {
                _statistics.Failed++;

                // 记录错误类型
                var errorType = response.Exception?.GetType().Name ?? "UnknownError";
                if (_statistics.ErrorTypes.ContainsKey(errorType))
                {
                    _statistics.ErrorTypes[errorType]++;
                }
                else
                {
                    _statistics.ErrorTypes[errorType] = 1;
                }
            }

            // 记录操作统计
            var operationName = operation.ToString();
            if (_statistics.OperationCounts.ContainsKey(operationName))
            {
                _statistics.OperationCounts[operationName]++;
            }
            else
            {
                _statistics.OperationCounts[operationName] = 1;
            }

            // 更新平均处理时间
            var totalProcessingTime = _statistics.AverageProcessingTimeMs * (_statistics.TotalProcessed - 1) + response.ProcessingTimeMs;
            _statistics.AverageProcessingTimeMs = totalProcessingTime / _statistics.TotalProcessed;

            _statistics.LastUpdated = DateTime.Now;
        }

        private void UpdateStatisticsDisplay()
        {
            _view.ShowStatistics(_statistics);
        }

        private void DisplayResults(PdfProcessingResponse response, PdfProcessingOperation operation)
        {
            switch (operation)
            {
                case PdfProcessingOperation.GetPageCount:
                    if (response.Success && response.Data is int pageCount)
                    {
                        _view.ShowSuccess($"PDF页数: {pageCount}");
                    }
                    break;

                case PdfProcessingOperation.GetFirstPageSize:
                    if (response.Success && response.Data is PageSize pageSize)
                    {
                        _view.ShowSuccess($"首页尺寸: {pageSize.Width} x {pageSize.Height} mm");
                    }
                    break;

                case PdfProcessingOperation.GetAllPageSizes:
                    if (response.Success && response.Data is PdfFileInfo fileInfo)
                    {
                        _view.ShowPdfFileInfo(fileInfo);
                    }
                    break;

                case PdfProcessingOperation.CompareLibraries:
                    if (response.Success && response.Data is PdfLibraryComparisonResult comparison)
                    {
                        _view.ShowLibraryComparison(comparison);
                    }
                    break;

                case PdfProcessingOperation.GenerateReport:
                    if (response.Success && response.Data is string report)
                    {
                        _view.ShowProcessingReport(report);
                    }
                    break;
            }

            // 总是显示基本处理结果
            _view.ShowProcessingResult(response);
        }

        private void CancelProcessing(string requestId)
        {
            if (_activeTasks.TryGetValue(requestId, out var cts))
            {
                cts.Cancel();
                _activeTasks.Remove(requestId);
                _view.AddLogMessage($"已取消处理任务: {requestId}", LogLevel.Warning);
            }
        }

        #endregion

        #region 验证方法

        private PdfProcessingValidationResult ValidateProcessingRequest(string filePath, PdfProcessingOperation operation)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return new PdfProcessingValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "请选择PDF文件",
                    ErrorType = ValidationErrorType.InvalidFilePath,
                    ValidationContext = "ProcessPdf"
                };
            }

            if (!File.Exists(filePath))
            {
                return new PdfProcessingValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "文件不存在",
                    ErrorType = ValidationErrorType.FileNotFound,
                    ValidationContext = "ProcessPdf"
                };
            }

            if (!Path.GetExtension(filePath).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                return new PdfProcessingValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "请选择PDF文件",
                    ErrorType = ValidationErrorType.InvalidFileFormat,
                    ValidationContext = "ProcessPdf"
                };
            }

            if (_view.HasActiveProcessing())
            {
                return new PdfProcessingValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "已有处理任务正在进行中",
                    ErrorType = ValidationErrorType.General,
                    ValidationContext = "ProcessPdf"
                };
            }

            return new PdfProcessingValidationResult { IsValid = true };
        }

        private bool ValidateConfig(PdfProcessingConfig config)
        {
            var validationResult = _view.ValidateConfig(config);
            if (!validationResult.IsValid)
            {
                _view.ShowError(validationResult.ErrorMessage);
                return false;
            }

            return true;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 取消所有正在进行的处理
        /// </summary>
        public void CancelAllProcessing()
        {
            foreach (var kvp in _activeTasks.ToList())
            {
                kvp.Value.Cancel();
                _activeTasks.Remove(kvp.Key);
            }

            _view.SetProcessingState(false);
            _view.AddLogMessage("已取消所有处理任务", LogLevel.Warning);
        }

        /// <summary>
        /// 获取当前统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        public PdfProcessingStatistics GetStatistics()
        {
            return _statistics;
        }

        /// <summary>
        /// 清除统计信息
        /// </summary>
        public void ClearStatistics()
        {
            _statistics.TotalProcessed = 0;
            _statistics.Successful = 0;
            _statistics.Failed = 0;
            _statistics.AverageProcessingTimeMs = 0;
            _statistics.ErrorTypes.Clear();
            _statistics.OperationCounts.Clear();
            _statistics.LastUpdated = DateTime.Now;

            UpdateStatisticsDisplay();
        }

        #endregion

        #region 资源释放

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            CancelAllProcessing();
            _activeTasks.Clear();
        }

        #endregion
    }

    #region 服务类

    /// <summary>
    /// PDF处理服务 - 简化版，主要用于演示MVP模式
    /// </summary>
    public class PdfProcessingService
    {
        public int GetPdfPageCount(string filePath)
        {
            return IText7PdfTools.GetPageCount(filePath) ?? 0;
        }

        public bool GetFirstPageSize(string filePath, out double width, out double height)
        {
            return IText7PdfTools.GetFirstPageSize(filePath, out width, out height);
        }
    }

    #endregion
}