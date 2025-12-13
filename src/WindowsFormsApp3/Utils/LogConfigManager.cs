using System;
using System.IO;
using Newtonsoft.Json;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// 日志配置管理器，负责加载和管理日志配置
    /// </summary>
    public class LogConfigManager
    {
        private static LogConfig _config;
        private static readonly object _syncLock = new object();

        /// <summary>
        /// 获取日志配置实例（单例模式）
        /// </summary>
        public static LogConfig GetConfig()
        {
            if (_config == null)
            {
                lock (_syncLock)
                {
                    if (_config == null)
                    {
                        LoadConfig();
                    }
                }
            }
            return _config;
        }

        /// <summary>
        /// 加载日志配置
        /// </summary>
        private static void LoadConfig()
        {
            try
            {
                string configPath = AppDataPathManager.LogConfigPath;

                if (File.Exists(configPath))
                {
                    string json = File.ReadAllText(configPath);
                    _config = JsonConvert.DeserializeObject<LogConfig>(json);
                }
                else
                {
                    // 使用默认配置
                    _config = new LogConfig
                    {
                        LogLevel = "Information",
                        LogDirectory = "Logs",
                        LogFileNameFormat = "app_{0:yyyy-MM-dd}.log",
                        MaxFileSizeBytes = 10485760, // 10MB
                        MaxRetainedFiles = 7,
                        EnableConsoleLogging = true,
                        LogFormat = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level}] {Message}{NewLine}{Exception}"
                    };

                    // 保存默认配置
                    string defaultJson = JsonConvert.SerializeObject(_config, Formatting.Indented);
                    File.WriteAllText(configPath, defaultJson);
                }
            }
            catch (Exception ex)
            {
                // 使用最低限度的错误处理，避免配置加载失败导致应用程序无法启动
                try
                {
                    var logger = LogHelper.GetLogger();
                    logger.LogError("加载日志配置失败: " + ex.Message);
                }
                catch
                {
                    // 如果日志系统也失败，就静默处理，确保应用程序能够启动
                }

                // 回退到硬编码的默认配置
                _config = new LogConfig
                {
                    LogLevel = "Information",
                    LogDirectory = "Logs",
                    MaxFileSizeBytes = 10485760,
                    MaxRetainedFiles = 7
                };
            }
        }

        /// <summary>
        /// 保存日志配置
        /// </summary>
        public static void SaveConfig(LogConfig config)
        {
            try
            {
                string configPath = AppDataPathManager.LogConfigPath;
                string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(configPath, json);
                _config = config;
            }
            catch (Exception ex)
            {
                try
                {
                    var logger = LogHelper.GetLogger();
                    logger.LogError("保存日志配置失败: " + ex.Message);
                }
                catch
                {
                    // 静默处理日志系统的错误
                }
            }
        }

        /// <summary>
        /// 确保目录存在，如果不存在则创建
        /// </summary>
        private static void EnsureDirectoryExists(string directoryPath)
        {
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch (Exception ex)
                {
                    try
                    {
                        var logger = LogHelper.GetLogger();
                        logger.LogError("创建日志目录失败: " + ex.Message);
                    }
                    catch
                    {
                        // 静默处理日志系统的错误
                    }
                }
            }
        }

        /// <summary>
        /// 重置配置为默认值
        /// </summary>
        public static void ResetToDefault()
        {
            _config = new LogConfig
            {
                LogLevel = "Information",
                LogDirectory = "Logs",
                LogFileNameFormat = "app_{0:yyyy-MM-dd}.log",
                MaxFileSizeBytes = 10485760, // 10MB
                MaxRetainedFiles = 7,
                EnableConsoleLogging = true,
                LogFormat = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level}] {Message}{NewLine}{Exception}"
            };
            SaveConfig(_config);
        }
    }
}