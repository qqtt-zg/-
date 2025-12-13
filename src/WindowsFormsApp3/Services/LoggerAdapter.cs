using System;
using WindowsFormsApp3.Interfaces;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 日志适配器类，用于将Services.ILogger适配到Interfaces.ILogger接口
    /// </summary>
    public class LoggerAdapter : Interfaces.ILogger
    {
        private readonly Interfaces.ILogger _innerLogger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="innerLogger">内部日志记录器实例</param>
        public LoggerAdapter(Interfaces.ILogger innerLogger)
        {
            _innerLogger = innerLogger ?? throw new ArgumentNullException(nameof(innerLogger));
        }

        /// <summary>
        /// 根据日志级别记录日志
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="message">日志消息</param>
        public void Log(WindowsFormsApp3.Interfaces.LogLevel level, string message)
        {
            switch (level)
            {
                case WindowsFormsApp3.Interfaces.LogLevel.Debug:
                    _innerLogger.LogDebug(message);
                    break;
                case WindowsFormsApp3.Interfaces.LogLevel.Information:
                    _innerLogger.LogInformation(message);
                    break;
                case WindowsFormsApp3.Interfaces.LogLevel.Warning:
                    _innerLogger.LogWarning(message);
                    break;
                case WindowsFormsApp3.Interfaces.LogLevel.Error:
                    _innerLogger.LogError(message);
                    break;
                case WindowsFormsApp3.Interfaces.LogLevel.Critical:
                    _innerLogger.LogCritical(message);
                    break;
            }
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void LogInformation(string message)
        {
            _innerLogger.LogInformation(message);
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void LogWarning(string message)
        {
            _innerLogger.LogWarning(message);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void LogError(string message)
        {
            _innerLogger.LogError(message);
        }

        /// <summary>
        /// 记录带异常的错误日志
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="message">日志消息</param>
        public void LogError(Exception ex, string message)
        {
            _innerLogger.LogError(ex, message);
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void LogDebug(string message)
        {
            _innerLogger.LogDebug(message);
        }

        /// <summary>
        /// 记录严重错误日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void LogCritical(string message)
        {
            _innerLogger.LogCritical(message);
        }

        /// <summary>
        /// 记录带异常的严重错误日志
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="message">日志消息</param>
        public void LogCritical(Exception ex, string message)
        {
            _innerLogger.LogCritical(ex, message);
        }
    }
}