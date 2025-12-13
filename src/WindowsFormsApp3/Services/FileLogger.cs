using System;
using System.IO;
using System.Text;
using System.Linq;
using WindowsFormsApp3.Utils;


using WindowsFormsApp3.Interfaces;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 文件日志记录器实现，将日志写入文件
    /// </summary>
    public class FileLogger : Interfaces.ILogger
    {
        private string _logFolderPath;
        private object _lockObject = new object();
        private Interfaces.LogLevel _currentLogLevel = Interfaces.LogLevel.Information;
        private long _maxLogSizeKB = 10240; // 10MB
        private int _maxLogFiles = 7; // 保留7天的日志
        private string _logFileNameFormat = "{0:yyyy-MM-dd}.log";
        private string _logFormat = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level}] {Message}{NewLine}{Exception}";
        private bool _enableConsoleLogging = true;

        /// <summary>
        /// 更新日志级别
        /// </summary>
        private void UpdateLogLevel(string logLevelText)
        {
            if (!string.IsNullOrEmpty(logLevelText))
            {
                switch (logLevelText.ToLower())
                {
                    case "debug":
                        _currentLogLevel = Interfaces.LogLevel.Debug;
                        break;
                    case "info":
                    case "information":
                        _currentLogLevel = Interfaces.LogLevel.Information;
                        break;
                    case "warn":
                    case "warning":
                        _currentLogLevel = Interfaces.LogLevel.Warning;
                        break;
                    case "error":
                        _currentLogLevel = Interfaces.LogLevel.Error;
                        break;
                    case "critical":
                    case "fatal":
                        _currentLogLevel = Interfaces.LogLevel.Critical;
                        break;
                    default:
                        _currentLogLevel = Interfaces.LogLevel.Information;
                        break;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logFolderPath">日志文件目录路径</param>
        public FileLogger(string logFolderPath = null)
        {
            try
            {
                // 初始化配置
                UpdateFromConfig();

                // 如果指定了日志文件夹路径，则覆盖配置中的路径
                if (!string.IsNullOrEmpty(logFolderPath))
                {
                    _logFolderPath = logFolderPath;
                }
                
                // 确保日志目录存在
                Directory.CreateDirectory(_logFolderPath);
            }
            catch (Exception ex)
            {
                // 使用Console.WriteLine而不是LogHelper.Debug，避免循环依赖
                Console.WriteLine($"创建日志目录失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 从LogConfigManager更新配置
        /// </summary>
        public void UpdateFromConfig()
        {
            try
            {
                var config = LogConfigManager.GetConfig();
                
                // 更新日志级别
                UpdateLogLevel(config.LogLevel);
                
                // 更新日志目录
                if (!string.IsNullOrEmpty(config.LogDirectory))
                {
                    // 检查是否是绝对路径
                    if (Path.IsPathRooted(config.LogDirectory))
                    {
                        _logFolderPath = config.LogDirectory;
                    }
                    else
                    {
                        // 如果是相对路径，使用应用程序数据目录作为基础
                        _logFolderPath = Path.Combine(AppDataPathManager.AppRootDirectory, config.LogDirectory);
                    }
                }
                else
                {
                    // 使用默认路径
                    _logFolderPath = AppDataPathManager.LogsDirectory;
                }
                
                // 更新其他配置项
                _maxLogSizeKB = config.MaxFileSizeBytes / 1024; // 转换为KB
                _maxLogFiles = config.MaxRetainedFiles;
                
                if (!string.IsNullOrEmpty(config.LogFileNameFormat))
                {
                    _logFileNameFormat = config.LogFileNameFormat;
                }
                
                if (!string.IsNullOrEmpty(config.LogFormat))
                {
                    _logFormat = config.LogFormat;
                }
                
                _enableConsoleLogging = config.EnableConsoleLogging;
                
                // 使用Console.WriteLine而不是LogHelper.Debug，避免循环依赖
                Console.WriteLine("日志配置已从LogConfigManager更新");
            }
            catch (Exception ex)
            {
                // 使用Console.WriteLine而不是LogHelper.Error，避免循环依赖
                Console.WriteLine($"从LogConfigManager更新日志配置失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 获取当前日期的日志文件路径
        /// </summary>
        private string GetLogFilePath()
        {
            string fileName = string.Format(_logFileNameFormat, DateTime.Now);
            return Path.Combine(_logFolderPath, fileName);
        }
        
        /// <summary>
        /// 根据日志字符串获取日志级别枚举值
        /// </summary>
        private Interfaces.LogLevel GetLogLevelEnum(string level)
        {
            switch (level.ToUpper())
            {
                case "DEBUG":
                    return Interfaces.LogLevel.Debug;
                case "INFO":
                case "INFORMATION":
                    return Interfaces.LogLevel.Information;
                case "WARN":
                case "WARNING":
                    return Interfaces.LogLevel.Warning;
                case "ERROR":
                    return Interfaces.LogLevel.Error;
                case "CRITICAL":
                case "FATAL":
                    return Interfaces.LogLevel.Critical;
                default:
                    return Interfaces.LogLevel.Information; // 默认返回Information级别
            }
        }

        /// <summary>
        /// 写入日志到文件
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="message">日志消息</param>
        /// <param name="exception">异常对象</param>
        private void WriteLog(string level, string message, Exception exception = null)
        {
            try
            {
                // 检查日志级别是否需要记录
                Interfaces.LogLevel logLevel = GetLogLevelEnum(level);
                if (logLevel < _currentLogLevel)
                {
                    // 如果日志级别低于当前设置的级别，不记录
                    return;
                }

                StringBuilder logBuilder = new StringBuilder();
                logBuilder.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] ");
                logBuilder.AppendLine(message);
                
                if (exception != null)
                {
                    logBuilder.AppendLine($"Exception: {exception.Message}");
                    logBuilder.AppendLine($"Stack Trace: {exception.StackTrace}");
                    
                    if (exception.InnerException != null)
                    {
                        logBuilder.AppendLine($"Inner Exception: {exception.InnerException.Message}");
                        logBuilder.AppendLine($"Inner Stack Trace: {exception.InnerException.StackTrace}");
                    }
                }
                
                lock (_lockObject)
                {
                    string logFilePath = GetLogFilePath();
                    
                    // 检查日志文件大小
                    CheckLogFileSize(logFilePath);
                    // 确保日志目录存在（配置可能已更改）
                    Directory.CreateDirectory(_logFolderPath);
                    
                    // 写入日志内容
                    File.AppendAllText(logFilePath, logBuilder.ToString());
                    
                    // 清理过期日志文件
                    CleanupOldLogFiles();
                }
                
                // 同时输出到调试窗口
                if (_enableConsoleLogging && (logLevel <= Interfaces.LogLevel.Debug || level.Equals("DEBUG", StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine(logBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                // 如果日志记录失败，输出到调试窗口
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 检查日志文件大小，超过阈值时创建新文件
        /// </summary>
        /// <param name="logFilePath">日志文件路径</param>
        private void CheckLogFileSize(string logFilePath)
        {
            try
            {
                if (File.Exists(logFilePath))
                {
                    FileInfo fileInfo = new FileInfo(logFilePath);
                    if (fileInfo.Length / 1024 > _maxLogSizeKB)
                    {
                        // 创建带时间戳的新日志文件
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        string newFileName = Path.GetFileNameWithoutExtension(logFilePath) + "_" + timestamp + Path.GetExtension(logFilePath);
                        string newFilePath = Path.Combine(Path.GetDirectoryName(logFilePath), newFileName);
                        
                        // 如果新文件已存在，添加计数器
                        int counter = 1;
                        while (File.Exists(newFilePath))
                        {
                            newFileName = Path.GetFileNameWithoutExtension(logFilePath) + "_" + timestamp + "_" + counter + Path.GetExtension(logFilePath);
                            newFilePath = Path.Combine(Path.GetDirectoryName(logFilePath), newFileName);
                            counter++;
                        }
                        
                        // 重命名当前日志文件
                        File.Move(logFilePath, newFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                // 忽略文件检查时的错误，不影响日志记录
                Console.WriteLine($"Failed to check log file size: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 清理过期的日志文件
        /// </summary>
        private void CleanupOldLogFiles()
        {
            try
            {
                string logDirectory = Path.GetDirectoryName(GetLogFilePath());
                if (Directory.Exists(logDirectory))
                {
                    // 获取所有日志文件并按创建时间排序
                    var logFiles = Directory.GetFiles(logDirectory, "*.log")
                        .Select(f => new FileInfo(f))
                        .OrderBy(f => f.CreationTime)
                        .ToList();
                    
                    // 删除超过保留数量的旧文件
                    while (logFiles.Count > _maxLogFiles)
                    {
                        try
                        {
                            File.Delete(logFiles[0].FullName);
                        }
                        catch (Exception)
                        {
                            // 忽略单个文件删除错误
                        }
                        logFiles.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
                // 忽略清理时的错误，不影响日志记录
                Console.WriteLine($"Failed to cleanup old log files: {ex.Message}");
            }
        }
        
        #region ILogger 接口实现

        public void Log(Interfaces.LogLevel level, string message)
        {
            string levelText = GetLevelText(level);
            WriteLog(levelText, message);
        }

        public void LogCritical(string message)
        {
            WriteLog("CRITICAL", message);
        }

        public void LogCritical(Exception ex, string message)
        {
            WriteLog("CRITICAL", message, ex);
        }

        public void LogDebug(string message)
        {
            WriteLog("DEBUG", message);
        }

        public void LogError(string message)
        {
            WriteLog("ERROR", message);
        }

        public void LogError(Exception ex, string message)
        {
            WriteLog("ERROR", message, ex);
        }

        public void LogInformation(string message)
        {
            WriteLog("INFO", message);
        }

        public void LogWarning(string message)
        {
            WriteLog("WARN", message);
        }

        #endregion

        /// <summary>
        /// 获取日志级别的文本表示
        /// </summary>
        private string GetLevelText(Interfaces.LogLevel level)
        {
            switch (level)
            {
                case Interfaces.LogLevel.Debug:
                    return "DEBUG";
                case Interfaces.LogLevel.Information:
                    return "INFO";
                case Interfaces.LogLevel.Warning:
                    return "WARN";
                case Interfaces.LogLevel.Error:
                    return "ERROR";
                case Interfaces.LogLevel.Critical:
                    return "CRITICAL";
                default:
                    return "UNKNOWN";
            }
        }
    }
}