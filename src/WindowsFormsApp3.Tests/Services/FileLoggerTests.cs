using System;
using System.IO;
using System.Linq;
using Xunit;
using WindowsFormsApp3.Services;
using WindowsFormsApp3.Interfaces;

namespace WindowsFormsApp3.Tests.Services
{
    public class FileLoggerTests : IDisposable
    {
        private readonly WindowsFormsApp3.Interfaces.ILogger _logger;
        private readonly string _testLogFolder;

        public FileLoggerTests()
        {
            // 创建测试日志文件夹
            _testLogFolder = Path.Combine(Path.GetTempPath(), "FileLoggerTests_", DateTime.Now.Ticks.ToString());
            Directory.CreateDirectory(_testLogFolder);

            // 创建FileLogger实例，使用测试日志文件夹
            _logger = new FileLogger(_testLogFolder);
        }

        [Fact]
        public void LogDebug_Should_Write_Debug_Log()
        {
            // 由于FileLogger类没有公共的SetLogLevel方法，我们不能直接设置日志级别
            // 但我们可以通过调用LogDebug方法来测试调试日志记录

            // 执行操作
            string testMessage = "Test debug message";
            _logger.LogDebug(testMessage);

            // 验证结果
            string logFilePath = GetCurrentLogFilePath();
            Assert.True(File.Exists(logFilePath));
            string logContent = File.ReadAllText(logFilePath);
            Assert.Contains("[DEBUG]", logContent);
            Assert.Contains(testMessage, logContent);
        }

        [Fact]
        public void LogInformation_Should_Write_Info_Log()
        {
            // 执行操作
            string testMessage = "Test info message";
            _logger.LogInformation(testMessage);

            // 验证结果
            string logFilePath = GetCurrentLogFilePath();
            Assert.True(File.Exists(logFilePath));
            string logContent = File.ReadAllText(logFilePath);
            Assert.Contains("[INFO]", logContent);
            Assert.Contains(testMessage, logContent);
        }

        [Fact]
        public void LogWarning_Should_Write_Warn_Log()
        {
            // 执行操作
            string testMessage = "Test warning message";
            _logger.LogWarning(testMessage);

            // 验证结果
            string logFilePath = GetCurrentLogFilePath();
            Assert.True(File.Exists(logFilePath));
            string logContent = File.ReadAllText(logFilePath);
            Assert.Contains("[WARN]", logContent);
            Assert.Contains(testMessage, logContent);
        }

        [Fact]
        public void LogError_Should_Write_Error_Log()
        {
            // 执行操作
            string testMessage = "Test error message";
            _logger.LogError(testMessage);

            // 验证结果
            string logFilePath = GetCurrentLogFilePath();
            Assert.True(File.Exists(logFilePath));
            string logContent = File.ReadAllText(logFilePath);
            Assert.Contains("[ERROR]", logContent);
            Assert.Contains(testMessage, logContent);
        }

        [Fact]
        public void LogCritical_Should_Write_Critical_Log()
        {
            // 执行操作
            string testMessage = "Test critical message";
            _logger.LogCritical(testMessage);

            // 验证结果
            string logFilePath = GetCurrentLogFilePath();
            Assert.True(File.Exists(logFilePath));
            string logContent = File.ReadAllText(logFilePath);
            Assert.Contains("[CRITICAL]", logContent);
            Assert.Contains(testMessage, logContent);
        }

        [Fact]
        public void LogError_With_Exception_Should_Write_Exception_Details()
        {
            // 准备数据
            string testMessage = "Test message with exception";
            var testException = new Exception("Test exception message");

            // 执行操作
            _logger.LogError(testException, testMessage);

            // 验证结果
            string logFilePath = GetCurrentLogFilePath();
            Assert.True(File.Exists(logFilePath));
            string logContent = File.ReadAllText(logFilePath);
            Assert.Contains(testMessage, logContent);
            Assert.Contains("Exception: Test exception message", logContent);
            Assert.Contains("Stack Trace:", logContent);
        }

        [Fact]
        public void Log_Directory_Should_Be_Created_If_Not_Exists()
        {
            // 准备数据
            string newLogFolder = Path.Combine(Path.GetTempPath(), "NewLogFolder_", DateTime.Now.Ticks.ToString());
            Assert.False(Directory.Exists(newLogFolder));

            try
            {
                // 执行操作
                var newLogger = new FileLogger(newLogFolder);
                newLogger.LogInformation("Test message");

                // 验证结果
                Assert.True(Directory.Exists(newLogFolder));
                string logFilePath = Path.Combine(newLogFolder, $"{DateTime.Now:yyyy-MM-dd}.log");
                Assert.True(File.Exists(logFilePath));
            }
            finally
            {
                // 清理
                if (Directory.Exists(newLogFolder))
                {
                    Directory.Delete(newLogFolder, true);
                }
            }
        }

        [Fact]
        public void Multiple_Log_Entries_Should_Be_Appended()
        {
            // 执行多次日志记录
            _logger.LogInformation("First log entry");
            _logger.LogInformation("Second log entry");
            _logger.LogInformation("Third log entry");

            // 验证结果
            string logFilePath = GetCurrentLogFilePath();
            string logContent = File.ReadAllText(logFilePath);
            
            // 计算包含"[INFO]"的行数
            int infoCount = logContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .Count(line => line.Contains("[INFO]"));

            Assert.Equal(3, infoCount);
        }

        private string GetCurrentLogFilePath()
        {
            string dateStr = DateTime.Now.ToString("yyyy-MM-dd");
            return Path.Combine(_testLogFolder, $"{dateStr}.log");
        }

        // 清理测试资源
        public void Dispose()
        {
            if (Directory.Exists(_testLogFolder))
            {
                try
                {
                    Directory.Delete(_testLogFolder, true);
                }
                catch (IOException)
                {
                    // 如果文件被锁定，忽略异常
                }
            }
        }
    }
}