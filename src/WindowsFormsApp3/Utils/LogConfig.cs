using System;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// 日志配置类，用于存储日志系统的配置信息
    /// </summary>
    public class LogConfig
    {
        /// <summary>
        /// 日志级别（Debug、Information、Warning、Error、Critical）
        /// </summary>
        public string LogLevel { get; set; } = "Information";

        /// <summary>
        /// 日志文件目录
        /// </summary>
        public string LogDirectory { get; set; } = "logs";

        /// <summary>
        /// 日志文件名格式
        /// </summary>
        public string LogFileNameFormat { get; set; } = "app_{0:yyyy-MM-dd}.log";

        /// <summary>
        /// 单个日志文件的最大大小（字节）
        /// </summary>
        public long MaxFileSizeBytes { get; set; } = 10485760; // 10MB

        /// <summary>
        /// 保留的最大日志文件数量
        /// </summary>
        public int MaxRetainedFiles { get; set; } = 7;

        /// <summary>
        /// 是否启用控制台日志
        /// </summary>
        public bool EnableConsoleLogging { get; set; } = true;

        /// <summary>
        /// 日志格式模板
        /// </summary>
        public string LogFormat { get; set; } = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level}] {Message}{NewLine}{Exception}";
        
        /// <summary>
        /// 是否启用敏感信息加密
        /// </summary>
        public bool EnableSensitiveInfoEncryption { get; set; } = false;
        
        /// <summary>
        /// 加密密钥（注意：实际部署时应考虑更安全的存储方式）
        /// </summary>
        public string EncryptionKey { get; set; } = "87b94e5f-0a3d-41c2-b7bc-f18d6c4a9d82";
        
        /// <summary>
        /// 日志缓冲区大小
        /// </summary>
        public int LogBufferSize { get; set; } = 50;
    }
}