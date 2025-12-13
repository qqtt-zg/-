using System;
using WindowsFormsApp3.Interfaces;
using WindowsFormsApp3.Services;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// 日志适配器类，用于适配Services.ILogger和Interfaces.ILogger接口
    /// </summary>
    public class LoggerAdapter : Interfaces.ILogger
    {
        private readonly Interfaces.ILogger _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">Services.ILogger实例</param>
        public LoggerAdapter(Interfaces.ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 记录指定级别的日志
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="message">日志消息</param>
        public void Log(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _logger.LogDebug(message);
                    break;
                case LogLevel.Information:
                    _logger.LogInformation(message);
                    break;
                case LogLevel.Warning:
                    _logger.LogWarning(message);
                    break;
                case LogLevel.Error:
                    _logger.LogError(message);
                    break;
                case LogLevel.Critical:
                    _logger.LogCritical(message);
                    break;
                default:
                    _logger.LogInformation(message);
                    break;
            }
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void LogError(string message)
        {
            _logger.LogError(message);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="message">日志消息</param>
        public void LogError(Exception ex, string message)
        {
            _logger.LogError(ex, message);
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void LogDebug(string message)
        {
            _logger.LogDebug(message);
        }

        /// <summary>
        /// 记录严重错误日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void LogCritical(string message)
        {
            _logger.LogCritical(message);
        }

        /// <summary>
        /// 记录严重错误日志
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="message">日志消息</param>
        public void LogCritical(Exception ex, string message)
        {
            _logger.LogCritical(ex, message);
        }
    }
}