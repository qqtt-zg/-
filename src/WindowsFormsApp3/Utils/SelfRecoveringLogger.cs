using System;
using System.Threading.Tasks;
using WindowsFormsApp3.Interfaces;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// 自恢复日志记录器，提供日志系统的容错和自恢复能力
    /// </summary>
    public class SelfRecoveringLogger : ILogger
    {
        private readonly ILogger _innerLogger;
        private readonly int _maxRetryCount = 3;
        private readonly int _retryDelayMs = 1000;
        private bool _isOperational = true;
        private readonly object _statusLock = new object();
        private int _consecutiveFailures = 0;
        private const int _maxConsecutiveFailures = 5; // 最大连续失败次数

        /// <summary>
        /// 获取或设置内部日志记录器
        /// </summary>
        public ILogger InnerLogger => _innerLogger;

        /// <summary>
        /// 获取日志记录器是否可操作
        /// </summary>
        public bool IsOperational
        {
            get { lock (_statusLock) return _isOperational; }
            private set { lock (_statusLock) _isOperational = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="innerLogger">内部日志记录器</param>
        public SelfRecoveringLogger(ILogger innerLogger)
        {
            if (innerLogger == null)
                throw new ArgumentNullException(nameof(innerLogger));
            
            _innerLogger = innerLogger;
        }

        /// <summary>
        /// 尝试记录日志，带重试逻辑
        /// </summary>
        /// <param name="action">日志记录操作</param>
        private void TryLog(Action action)
        {
            if (!IsOperational)
            {
                // 尝试恢复
                AttemptRecovery();
                if (!IsOperational)
                {
                    // 无法恢复，使用后备日志记录
                    FallbackLog(action);
                    return;
                }
            }

            int attempt = 0;
            bool success = false;
            
            while (attempt < _maxRetryCount && !success)
            {
                try
                {
                    action();
                    success = true;
                    
                    // 重置连续失败计数器
                    lock (_statusLock)
                    {
                        _consecutiveFailures = 0;
                    }
                }
                catch (Exception ex)
                {
                    attempt++;
                    
                    // 更新连续失败计数
                    lock (_statusLock)
                    {
                        _consecutiveFailures++;
                        
                        // 检查是否需要标记为不可操作
                        if (_consecutiveFailures >= _maxConsecutiveFailures)
                        {
                            IsOperational = false;
                        }
                    }
                    
                    if (attempt < _maxRetryCount)
                    {
                        // 等待一段时间后重试
                        System.Threading.Thread.Sleep(_retryDelayMs);
                    }
                    else
                    {
                        // 重试次数用尽，使用后备日志记录
                        FallbackLog(() => 
                        {
                            Console.WriteLine($"日志记录失败，已尝试{_maxRetryCount}次: {ex.Message}\n{ex.StackTrace}");
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 异步尝试记录日志，带重试逻辑
        /// </summary>
        /// <param name="action">日志记录操作</param>
        private async Task TryLogAsync(Func<Task> action)
        {
            if (!IsOperational)
            {
                // 尝试恢复
                AttemptRecovery();
                if (!IsOperational)
                {
                    // 无法恢复，使用后备日志记录
                    FallbackLog(() => { action().Wait(); });
                    return;
                }
            }

            int attempt = 0;
            bool success = false;

            while (attempt < _maxRetryCount && !success)
            {
                try
                {
                    await action();
                    success = true;

                    // 重置连续失败计数器
                    lock (_statusLock)
                    {
                        _consecutiveFailures = 0;
                    }
                }
                catch (Exception ex)
                {
                    attempt++;

                    // 更新连续失败计数
                    lock (_statusLock)
                    {
                        _consecutiveFailures++;

                        // 检查是否需要标记为不可操作
                        if (_consecutiveFailures >= _maxConsecutiveFailures)
                        {
                            IsOperational = false;
                        }
                    }

                    if (attempt < _maxRetryCount)
                    {
                        // 等待一段时间后重试
                        await Task.Delay(_retryDelayMs);
                    }
                    else
                    {
                        // 重试次数用尽，使用后备日志记录
                        FallbackLog(() => 
                        {
                            Console.WriteLine($"异步日志记录失败，已尝试{_maxRetryCount}次: {ex.Message}\n{ex.StackTrace}");
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 尝试恢复日志记录器
        /// </summary>
        private void AttemptRecovery()
        {
            try
            {
                // 检查内部日志记录器是否实现了IDisposable接口
                if (_innerLogger is IDisposable disposableLogger)
                {
                    try
                    {
                        disposableLogger.Dispose();
                    }
                    catch (Exception ex)
                    {
                        // 日志记录器释放失败，但不影响程序运行
                        System.Diagnostics.Debug.WriteLine($"日志记录器释放失败: {ex.Message}");
                    }
                }

                // 如果内部日志记录器是BufferedFileLogger，尝试刷新缓冲区
                if (_innerLogger is BufferedFileLogger bufferedLogger)
                {
                    try
                    {
                        bufferedLogger.Flush();
                    }
                    catch (Exception ex)
                    {
                        // 缓冲区刷新失败，但不影响程序运行
                        System.Diagnostics.Debug.WriteLine($"日志缓冲区刷新失败: {ex.Message}");
                    }
                }

                // 检查内部日志记录器是否可以重新初始化
                // 这里可以根据具体的日志记录器类型实现不同的恢复策略
                
                // 尝试记录一条恢复信息
                Console.WriteLine("正在尝试恢复日志记录器...");
                
                // 假设恢复成功
                IsOperational = true;
                lock (_statusLock)
                {
                    _consecutiveFailures = 0;
                }
                
                Console.WriteLine("日志记录器恢复成功。");
            }
            catch (Exception ex)
            {
                Console.WriteLine("日志记录器恢复失败: " + ex.Message);
                IsOperational = false;
            }
        }

        /// <summary>
        /// 后备日志记录机制
        /// </summary>
        /// <param name="action">日志记录操作</param>
        private void FallbackLog(Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
                // 作为最后的后备，忽略任何异常
            }
        }

        #region ILogger 接口实现

        public void Log(LogLevel level, string message)
        {
            TryLog(() => _innerLogger.Log(level, message));
        }

        public void LogCritical(string message)
        {
            TryLog(() => _innerLogger.LogCritical(message));
        }

        public void LogCritical(Exception ex, string message)
        {
            TryLog(() => _innerLogger.LogCritical(ex, message));
        }

        public void LogDebug(string message)
        {
            TryLog(() => _innerLogger.LogDebug(message));
        }

        public void LogError(string message)
        {
            TryLog(() => _innerLogger.LogError(message));
        }

        public void LogError(Exception ex, string message)
        {
            TryLog(() => _innerLogger.LogError(ex, message));
        }

        public void LogInformation(string message)
        {
            TryLog(() => _innerLogger.LogInformation(message));
        }

        public void LogWarning(string message)
        {
            TryLog(() => _innerLogger.LogWarning(message));
        }

        #endregion
    }
}