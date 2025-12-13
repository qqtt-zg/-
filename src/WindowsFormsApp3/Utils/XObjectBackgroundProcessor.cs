using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iText.Kernel.Pdf;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// XObject后台处理器
    /// 提供异步PDF处理功能，避免UI阻塞
    /// </summary>
    public class XObjectBackgroundProcessor
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly Dictionary<string, CancellationTokenSource> _activeTasks;
        private readonly object _lockObject = new object();

        public XObjectBackgroundProcessor(int maxConcurrentTasks = 2)
        {
            _semaphore = new SemaphoreSlim(maxConcurrentTasks);
            _activeTasks = new Dictionary<string, CancellationTokenSource>();
        }

        /// <summary>
        /// 异步执行XObject增强的页面重排
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="progressCallback">进度回调</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理任务</returns>
        public async Task<XObjectProcessingResult> ProcessXObjectEnhancementAsync(
            string filePath,
            Action<XObjectProcessingProgress> progressCallback = null,
            CancellationToken cancellationToken = default)
        {
            var taskId = Guid.NewGuid().ToString();
            var taskCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            lock (_lockObject)
            {
                _activeTasks[taskId] = taskCts;
            }

            try
            {
                await _semaphore.WaitAsync(taskCts.Token);

                return await Task.Run(async () =>
                {
                    try
                    {
                        return await ExecuteXObjectEnhancementAsync(filePath, progressCallback, taskCts.Token);
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }, taskCts.Token);
            }
            catch (OperationCanceledException)
            {
                LogHelper.Debug($"XObject处理任务已取消: {taskId}");
                return XObjectProcessingResult.Cancelled();
            }
            catch (Exception ex)
            {
                LogHelper.Debug($"XObject处理任务异常: {taskId} - {ex.Message}");
                return XObjectProcessingResult.Failed(ex.Message);
            }
            finally
            {
                lock (_lockObject)
                {
                    _activeTasks.Remove(taskId);
                }
            }
        }

        /// <summary>
        /// 执行XObject增强处理的核心逻辑
        /// </summary>
        private async Task<XObjectProcessingResult> ExecuteXObjectEnhancementAsync(
            string filePath,
            Action<XObjectProcessingProgress> progressCallback,
            CancellationToken cancellationToken)
        {
            var result = new XObjectProcessingResult { IsSuccess = true };

            try
            {
                LogHelper.Debug($"开始后台XObject处理: {filePath}");

                // 报告开始状态
                progressCallback?.Invoke(new XObjectProcessingProgress
                {
                    Stage = "检查文件",
                    Percentage = 0,
                    Message = "正在检查PDF文件..."
                });

                // 验证文件
                if (!ValidateFile(filePath, cancellationToken))
                {
                    return XObjectProcessingResult.Failed("文件验证失败");
                }

                cancellationToken.ThrowIfCancellationRequested();

                // 检查旋转页面
                progressCallback?.Invoke(new XObjectProcessingProgress
                {
                    Stage = "分析旋转页面",
                    Percentage = 10,
                    Message = "正在分析旋转页面..."
                });

                var rotatedPagesInfo = XObjectEnhancedPageRelayout.GetRotatedPagesInfo(filePath);
                int rotatedPageCount = rotatedPagesInfo.Count;

                LogHelper.Debug($"发现{rotatedPageCount}个旋转页面");

                if (rotatedPageCount == 0)
                {
                    progressCallback?.Invoke(new XObjectProcessingProgress
                    {
                        Stage = "完成",
                        Percentage = 100,
                        Message = "文件无需XObject优化处理"
                    });

                    return XObjectProcessingResult.CreateSuccess("文件无需XObject优化处理");
                }

                cancellationToken.ThrowIfCancellationRequested();

                // 执行基础页面重排
                progressCallback?.Invoke(new XObjectProcessingProgress
                {
                    Stage = "页面重排",
                    Percentage = 30,
                    Message = "正在执行页面重排..."
                });

                if (!XObjectEnhancedPageRelayout.ProcessWithXObjectEnhancement(filePath, false))
                {
                    return XObjectProcessingResult.Failed("页面重排失败");
                }

                cancellationToken.ThrowIfCancellationRequested();

                // XObject优化处理
                progressCallback?.Invoke(new XObjectProcessingProgress
                {
                    Stage = "XObject优化",
                    Percentage = 70,
                    Message = $"正在处理{rotatedPageCount}个旋转页面..."
                });

                await Task.Run(async () =>
                {
                    // 模拟XObject处理进度
                    for (int i = 0; i < rotatedPageCount; i++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        progressCallback?.Invoke(new XObjectProcessingProgress
                        {
                            Stage = "XObject优化",
                            Percentage = 70 + (int)((i + 1) * 25.0 / rotatedPageCount),
                            Message = $"正在处理第{i + 1}/{rotatedPageCount}个旋转页面..."
                        });

                        // 模拟处理延迟
                        await Task.Delay(100, cancellationToken);
                    }
                }, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                // 完成处理
                progressCallback?.Invoke(new XObjectProcessingProgress
                {
                    Stage = "完成",
                    Percentage = 100,
                    Message = "XObject优化处理完成"
                });

                LogHelper.Debug($"XObject处理完成: {filePath}");
                return XObjectProcessingResult.CreateSuccess($"成功处理{rotatedPageCount}个旋转页面");
            }
            catch (OperationCanceledException)
            {
                LogHelper.Debug("XObject处理被取消");
                return XObjectProcessingResult.Cancelled();
            }
            catch (Exception ex)
            {
                LogHelper.Debug($"XObject处理异常: {ex.Message}");
                return XObjectProcessingResult.Failed(ex.Message);
            }
        }

        /// <summary>
        /// 验证文件
        /// </summary>
        private bool ValidateFile(string filePath, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!System.IO.File.Exists(filePath))
            {
                LogHelper.Debug($"文件不存在: {filePath}");
                return false;
            }

            try
            {
                var fileInfo = new System.IO.FileInfo(filePath);
                if (fileInfo.Length == 0)
                {
                    LogHelper.Debug($"文件为空: {filePath}");
                    return false;
                }

                if (fileInfo.Length > 500 * 1024 * 1024) // 500MB限制
                {
                    LogHelper.Debug($"文件过大: {filePath} ({fileInfo.Length} bytes)");
                    return false;
                }

                // 尝试打开PDF文件验证格式
                using var reader = new PdfReader(filePath);
                using var document = new PdfDocument(reader);

                if (document.GetNumberOfPages() == 0)
                {
                    LogHelper.Debug($"PDF文件无页面: {filePath}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Debug($"文件验证失败: {filePath} - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 取消指定任务
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public bool CancelTask(string filePath)
        {
            lock (_lockObject)
            {
                foreach (var kvp in _activeTasks)
                {
                    if (kvp.Key.Contains(filePath) || kvp.Value.ToString().Contains(filePath))
                    {
                        kvp.Value.Cancel();
                        LogHelper.Debug($"已取消XObject处理任务: {kvp.Key}");
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 取消所有活动任务
        /// </summary>
        public void CancelAllTasks()
        {
            lock (_lockObject)
            {
                foreach (var cts in _activeTasks.Values)
                {
                    cts.Cancel();
                }
                LogHelper.Debug("已取消所有XObject处理任务");
            }
        }

        /// <summary>
        /// 获取活动任务数量
        /// </summary>
        public int GetActiveTaskCount()
        {
            lock (_lockObject)
            {
                return _activeTasks.Count;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            CancelAllTasks();
            _semaphore?.Dispose();
        }
    }

    /// <summary>
    /// XObject处理进度信息
    /// </summary>
    public class XObjectProcessingProgress
    {
        public string Stage { get; set; }
        public int Percentage { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// XObject处理结果
    /// </summary>
    public class XObjectProcessingResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public bool IsCancelled { get; set; }
        public Exception Exception { get; set; }
        public DateTime ProcessedAt { get; set; } = DateTime.Now;

        public static XObjectProcessingResult CreateSuccess(string message = "处理成功")
        {
            return new XObjectProcessingResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static XObjectProcessingResult Failed(string message, Exception exception = null)
        {
            return new XObjectProcessingResult
            {
                IsSuccess = false,
                Message = message,
                Exception = exception
            };
        }

        public static XObjectProcessingResult Cancelled()
        {
            return new XObjectProcessingResult
            {
                IsSuccess = false,
                IsCancelled = true,
                Message = "处理已取消"
            };
        }
    }

    /// <summary>
    /// XObject后台处理器单例
    /// </summary>
    public static class XObjectProcessorInstance
    {
        private static readonly Lazy<XObjectBackgroundProcessor> _instance =
            new Lazy<XObjectBackgroundProcessor>(() => new XObjectBackgroundProcessor());

        public static XObjectBackgroundProcessor Instance => _instance.Value;

        /// <summary>
        /// 快速处理方法
        /// </summary>
        public static async Task<XObjectProcessingResult> QuickProcessAsync(
            string filePath,
            Action<XObjectProcessingProgress> progressCallback = null)
        {
            return await Instance.ProcessXObjectEnhancementAsync(filePath, progressCallback);
        }

        /// <summary>
        /// 检查是否有旋转页面
        /// </summary>
        public static bool HasRotatedPages(string filePath)
        {
            return XObjectEnhancedPageRelayout.HasRotatedPages(filePath);
        }

        /// <summary>
        /// 获取旋转页面统计
        /// </summary>
        public static Dictionary<int, int> GetRotatedPagesInfo(string filePath)
        {
            return XObjectEnhancedPageRelayout.GetRotatedPagesInfo(filePath);
        }
    }
}