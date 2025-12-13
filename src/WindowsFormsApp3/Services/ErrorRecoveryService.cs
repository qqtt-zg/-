using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WindowsFormsApp3.Interfaces;
using WindowsFormsApp3.Exceptions;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 错误恢复服务接口
    /// </summary>
    public interface IErrorRecoveryService
    {
        /// <summary>
        /// 尝试恢复文件操作
        /// </summary>
        /// <param name="operation">操作名称</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="action">要执行的操作</param>
        /// <param name="maxRetries">最大重试次数</param>
        /// <returns>操作是否成功</returns>
        bool TryRecoverFileOperation(string operation, string filePath, Action action, int maxRetries = 3);

        /// <summary>
        /// 尝试恢复文件操作（带备选路径）
        /// </summary>
        /// <param name="operation">操作名称</param>
        /// <param name="filePath">主文件路径</param>
        /// <param name="alternativePaths">备选路径列表</param>
        /// <param name="action">要执行的操作</param>
        /// <param name="maxRetries">最大重试次数</param>
        /// <returns>操作是否成功及使用的路径</returns>
        (bool Success, string UsedPath) TryRecoverFileOperationWithAlternatives(string operation, string filePath, List<string> alternativePaths, Action<string> action, int maxRetries = 3);

        /// <summary>
        /// 尝试恢复文件操作（带返回值）
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="operation">操作名称</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="func">要执行的函数</param>
        /// <param name="maxRetries">最大重试次数</param>
        /// <returns>操作结果</returns>
        RecoveryResult<T> TryRecoverFileOperation<T>(string operation, string filePath, Func<T> func, int maxRetries = 3);

        /// <summary>
        /// 创建错误报告
        /// </summary>
        /// <param name="exception">异常信息</param>
        /// <param name="context">上下文信息</param>
        /// <returns>错误报告ID</returns>
        string CreateErrorReport(Exception exception, Dictionary<string, object> context = null);

        /// <summary>
        /// 获取操作历史
        /// </summary>
        /// <returns>操作历史列表</returns>
        List<OperationHistory> GetOperationHistory();

        /// <summary>
        /// 清理操作历史
        /// </summary>
        /// <param name="olderThan">清理早于此时间的记录</param>
        void CleanupHistory(DateTime olderThan);
    }

    /// <summary>
    /// 恢复结果
    /// </summary>
    /// <typeparam name="T">结果类型</typeparam>
    public class RecoveryResult<T>
    {
        public bool Success { get; set; }
        public T Result { get; set; }
        public string ErrorMessage { get; set; }
        public int AttemptsCount { get; set; }
        public List<Exception> Exceptions { get; set; } = new List<Exception>();
    }

    /// <summary>
    /// 操作历史记录
    /// </summary>
    public class OperationHistory
    {
        public DateTime Timestamp { get; set; }
        public string Operation { get; set; }
        public string FilePath { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int AttemptsCount { get; set; }
    }

    /// <summary>
    /// 错误恢复服务实现
    /// </summary>
    public class ErrorRecoveryService : IErrorRecoveryService
    {
        private readonly WindowsFormsApp3.Interfaces.ILogger _logger;
        private readonly List<OperationHistory> _operationHistory = new List<OperationHistory>();
        private readonly object _historyLock = new object();
        private const int MaxHistorySize = 1000;

        public ErrorRecoveryService(WindowsFormsApp3.Interfaces.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool TryRecoverFileOperation(string operation, string filePath, Action action, int maxRetries = 3)
        {
            if (string.IsNullOrEmpty(operation))
                throw new ArgumentException("操作名称不能为空", nameof(operation));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var startTime = DateTime.Now;
            var attempts = 0;
            var exceptions = new List<Exception>();

            _logger.LogInformation($"开始执行操作: {operation} - 文件: {filePath}");

            while (attempts <= maxRetries)
            {
                attempts++;
                try
                {
                    // 检查文件状态（如果涉及文件操作）
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        ValidateFileState(filePath);
                    }

                    action();

                    var duration = DateTime.Now - startTime;
                    _logger.LogInformation($"操作成功: {operation} - 耗时: {duration.TotalMilliseconds:F0}ms, 重试次数: {attempts - 1}");

                    // 记录成功的操作历史
                    RecordOperationHistory(operation, filePath, true, null, attempts);

                    return true;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    _logger.LogWarning($"操作失败 (尝试 {attempts}/{maxRetries + 1}): {operation} - 错误: {ex.Message}");

                    // 如果不是最后一次尝试，等待一段时间后重试
                    if (attempts <= maxRetries)
                    {
                        var delay = CalculateRetryDelay(attempts);
                        System.Threading.Thread.Sleep(delay);
                    }
                }
            }

            var totalDuration = DateTime.Now - startTime;
            _logger.LogError($"操作最终失败: {operation} - 总耗时: {totalDuration.TotalMilliseconds:F0}ms, 总尝试次数: {attempts}");

            // 记录失败的操作历史
            var errorMessage = string.Join("; ", exceptions.Select(e => e.Message));
            RecordOperationHistory(operation, filePath, false, errorMessage, attempts);

            // 创建错误报告
            CreateErrorReport(new AggregateException($"操作 '{operation}' 在 {attempts} 次尝试后失败", exceptions),
                new Dictionary<string, object>
                {
                    ["Operation"] = operation,
                    ["FilePath"] = filePath,
                    ["MaxRetries"] = maxRetries,
                    ["TotalDuration"] = totalDuration,
                    ["Attempts"] = attempts
                });

            return false;
        }

        public (bool Success, string UsedPath) TryRecoverFileOperationWithAlternatives(string operation, string filePath, List<string> alternativePaths, Action<string> action, int maxRetries = 3)
        {
            if (string.IsNullOrEmpty(operation))
                throw new ArgumentException("操作名称不能为空", nameof(operation));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var allPaths = new List<string> { filePath };
            if (alternativePaths != null)
            {
                allPaths.AddRange(alternativePaths);
            }

            _logger.LogInformation($"开始执行操作（带备选路径）: {operation} - 主路径: {filePath}");

            foreach (var currentPath in allPaths)
            {
                var attempts = 0;
                var exceptions = new List<Exception>();
                var startTime = DateTime.Now;

                _logger.LogInformation($"尝试路径: {currentPath}");

                while (attempts <= maxRetries)
                {
                    attempts++;
                    try
                    {
                        // 检查文件状态
                        if (!string.IsNullOrEmpty(currentPath))
                        {
                            ValidateFileState(currentPath);
                        }

                        action(currentPath);

                        var duration = DateTime.Now - startTime;
                        _logger.LogInformation($"操作成功: {operation} - 路径: {currentPath} - 耗时: {duration.TotalMilliseconds:F0}ms, 重试次数: {attempts - 1}");

                        // 记录成功的操作历史
                        RecordOperationHistory($"{operation} (路径: {currentPath})", currentPath, true, null, attempts);

                        return (true, currentPath);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                        _logger.LogWarning($"操作失败 (尝试 {attempts}/{maxRetries + 1}): {operation} - 路径: {currentPath} - 错误: {ex.Message}");

                        // 如果不是最后一次尝试，等待一段时间后重试
                        if (attempts <= maxRetries)
                        {
                            var delay = CalculateRetryDelay(attempts);
                            System.Threading.Thread.Sleep(delay);
                        }
                    }
                }

                var totalDuration = DateTime.Now - startTime;
                _logger.LogError($"路径 {currentPath} 最终失败: {operation} - 总耗时: {totalDuration.TotalMilliseconds:F0}ms, 总尝试次数: {attempts}");

                // 记录失败的操作历史
                var errorMessage = string.Join("; ", exceptions.Select(e => e.Message));
                RecordOperationHistory($"{operation} (路径: {currentPath})", currentPath, false, errorMessage, attempts);

                // 如果不是最后一个路径，继续尝试下一个路径
                if (currentPath != allPaths.Last())
                {
                    _logger.LogInformation($"当前路径失败，尝试下一个备选路径...");
                    continue;
                }
            }

            // 所有路径都失败了
            _logger.LogError($"所有路径都失败了: {operation}");
            return (false, null);
        }

        public RecoveryResult<T> TryRecoverFileOperation<T>(string operation, string filePath, Func<T> func, int maxRetries = 3)
        {
            var result = new RecoveryResult<T>();
            var startTime = DateTime.Now;
            var exceptions = new List<Exception>();

            if (string.IsNullOrEmpty(operation))
                throw new ArgumentException("操作名称不能为空", nameof(operation));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            _logger.LogInformation($"开始执行操作: {operation} - 文件: {filePath}");

            for (int attempt = 1; attempt <= maxRetries + 1; attempt++)
            {
                try
                {
                    // 检查文件状态（如果涉及文件操作）
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        ValidateFileState(filePath);
                    }

                    result.Result = func();
                    result.Success = true;
                    result.AttemptsCount = attempt;

                    var duration = DateTime.Now - startTime;
                    _logger.LogInformation($"操作成功: {operation} - 耗时: {duration.TotalMilliseconds:F0}ms, 重试次数: {attempt - 1}");

                    // 记录成功的操作历史
                    RecordOperationHistory(operation, filePath, true, null, attempt);

                    return result;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    result.Exceptions.Add(ex);
                    result.AttemptsCount = attempt;

                    _logger.LogWarning($"操作失败 (尝试 {attempt}/{maxRetries + 1}): {operation} - 错误: {ex.Message}");

                    // 如果不是最后一次尝试，等待一段时间后重试
                    if (attempt <= maxRetries)
                    {
                        var delay = CalculateRetryDelay(attempt);
                        System.Threading.Thread.Sleep(delay);
                    }
                }
            }

            var totalDuration = DateTime.Now - startTime;
            _logger.LogError($"操作最终失败: {operation} - 总耗时: {totalDuration.TotalMilliseconds:F0}ms, 总尝试次数: {result.AttemptsCount}");

            result.ErrorMessage = string.Join("; ", exceptions.Select(e => e.Message));

            // 记录失败的操作历史
            RecordOperationHistory(operation, filePath, false, result.ErrorMessage, result.AttemptsCount);

            // 创建错误报告
            CreateErrorReport(new AggregateException($"操作 '{operation}' 在 {result.AttemptsCount} 次尝试后失败", exceptions),
                new Dictionary<string, object>
                {
                    ["Operation"] = operation,
                    ["FilePath"] = filePath,
                    ["MaxRetries"] = maxRetries,
                    ["TotalDuration"] = totalDuration,
                    ["Attempts"] = result.AttemptsCount
                });

            return result;
        }

        public string CreateErrorReport(Exception exception, Dictionary<string, object> context = null)
        {
            try
            {
                var reportId = Guid.NewGuid().ToString("N").Substring(0, 8);
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var reportPath = Path.Combine(GetErrorReportsDirectory(), $"error_report_{reportId}.json");

                var report = new
                {
                    ReportId = reportId,
                    Timestamp = timestamp,
                    Exception = new
                    {
                        Type = exception.GetType().Name,
                        Message = exception.Message,
                        StackTrace = exception.StackTrace,
                        Source = exception.Source
                    },
                    Context = context ?? new Dictionary<string, object>(),
                    SystemInfo = new
                    {
                        OSVersion = Environment.OSVersion.ToString(),
                        MachineName = Environment.MachineName,
                        UserName = Environment.UserName,
                        WorkingDirectory = Environment.CurrentDirectory
                    }
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(report, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(reportPath, json);

                _logger.LogInformation($"错误报告已创建: {reportPath}");
                return reportId;
            }
            catch (Exception ex)
            {
                _logger.LogError($"创建错误报告失败: {ex.Message}");
                return null;
            }
        }

        public List<OperationHistory> GetOperationHistory()
        {
            lock (_historyLock)
            {
                return _operationHistory.OrderByDescending(h => h.Timestamp).ToList();
            }
        }

        public void CleanupHistory(DateTime olderThan)
        {
            lock (_historyLock)
            {
                var itemsToRemove = _operationHistory.Where(h => h.Timestamp < olderThan).ToList();
                foreach (var item in itemsToRemove)
                {
                    _operationHistory.Remove(item);
                }
            }
        }

        private void ValidateFileState(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                // 检查文件是否存在
                if (File.Exists(filePath))
                {
                    // 检查文件是否被占用
                    using (var fs = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        // 文件可以正常访问
                    }
                }
            }
            catch (IOException ex)
            {
                throw new FileRenameException(filePath, null, $"文件被占用或无法访问: {ex.Message}", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new FileRenameException(filePath, null, $"没有文件访问权限: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new FileRenameException(filePath, null, $"文件状态检查失败: {ex.Message}", ex);
            }
        }

        private int CalculateRetryDelay(int attempt)
        {
            // 指数退避算法：基础延迟100ms，每次尝试延迟翻倍
            var baseDelay = 100;
            var maxDelay = 5000; // 最大延迟5秒
            var delay = Math.Min(baseDelay * (int)Math.Pow(2, attempt - 1), maxDelay);

            // 添加随机抖动，避免多个操作同时重试
            var random = new Random();
            var jitter = random.Next(0, delay / 4);

            return delay + jitter;
        }

        private void RecordOperationHistory(string operation, string filePath, bool success, string errorMessage, int attempts)
        {
            lock (_historyLock)
            {
                var history = new OperationHistory
                {
                    Timestamp = DateTime.Now,
                    Operation = operation,
                    FilePath = filePath,
                    Success = success,
                    ErrorMessage = errorMessage,
                    AttemptsCount = attempts
                };

                _operationHistory.Add(history);

                // 限制历史记录大小
                if (_operationHistory.Count > MaxHistorySize)
                {
                    var itemsToRemove = _operationHistory.Count - MaxHistorySize;
                    _operationHistory.RemoveRange(0, itemsToRemove);
                }
            }
        }

        private string GetErrorReportsDirectory()
        {
            return AppDataPathManager.ErrorReportsDirectory;
        }
    }
}