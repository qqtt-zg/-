using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Moq;
using WindowsFormsApp3.Services;
using Logger = WindowsFormsApp3.Interfaces.ILogger;

namespace WindowsFormsApp3.Tests.Services
{
    public class FileMonitorTests : IDisposable
    {
        private readonly WindowsFormsApp3.Services.IFileMonitor _fileMonitor;
        private readonly Mock<Logger> _loggerMock;
        private readonly string _testDirectory;
        private bool _fileCreatedEventRaised;
        private bool _fileRenamedEventRaised;

        public FileMonitorTests()
        {
            // 创建测试目录
            _testDirectory = Path.Combine(Path.GetTempPath(), "FileMonitorTests_", DateTime.Now.Ticks.ToString());
            Directory.CreateDirectory(_testDirectory);

            // 创建模拟日志服务
            _loggerMock = new Mock<Logger>();

            // 创建FileMonitor实例
            _fileMonitor = new FileMonitor();
            if (_fileMonitor is FileMonitor fileMonitorImpl)
            {
                fileMonitorImpl.SetLogger(_loggerMock.Object);
            }

            // 订阅事件
            _fileMonitor.FileCreated += (sender, e) => _fileCreatedEventRaised = true;
            _fileMonitor.FileRenamed += (sender, e) => _fileRenamedEventRaised = true;
        }

        [Fact]
        public void StartMonitoring_Should_Start_Monitoring_Successfully()
        {
            // 执行开始监控
            _fileMonitor.StartMonitoring(_testDirectory);

            // 验证监控状态
            Assert.True(_fileMonitor.IsMonitoring);
        }

        [Fact]
        public void StartMonitoring_Should_Throw_Exception_When_Directory_Is_Null()
        {
            // 验证异常
            Assert.Throws<ArgumentException>(() => _fileMonitor.StartMonitoring(null));
            Assert.Throws<ArgumentException>(() => _fileMonitor.StartMonitoring(string.Empty));
        }

        [Fact]
        public void StartMonitoring_Should_Throw_Exception_When_Directory_Does_Not_Exist()
        {
            // 验证异常
            Assert.Throws<DirectoryNotFoundException>(() => 
                _fileMonitor.StartMonitoring(Path.Combine(_testDirectory, "non_existent_directory"))
            );
        }

        [Fact]
        public void StopMonitoring_Should_Stop_Monitoring_Successfully()
        {
            // 先开始监控
            _fileMonitor.StartMonitoring(_testDirectory);
            Assert.True(_fileMonitor.IsMonitoring);

            // 执行停止监控
            _fileMonitor.StopMonitoring();

            // 验证监控状态
            Assert.False(_fileMonitor.IsMonitoring);
        }

        [Fact]
        public async Task FileCreatedEvent_Should_Be_Raised_When_File_Is_Created()
        {
            // 先开始监控
            _fileMonitor.StartMonitoring(_testDirectory);

            // 创建测试文件
            string testFilePath = Path.Combine(_testDirectory, "test_file.txt");
            File.WriteAllText(testFilePath, "Test content");

            // 等待事件触发
            await Task.Delay(1000);

            // 验证事件是否触发
            Assert.True(_fileCreatedEventRaised);
        }

        [Fact]
        public async Task FileRenamedEvent_Should_Be_Raised_When_File_Is_Renamed()
        {
            // 创建测试文件
            string originalFilePath = Path.Combine(_testDirectory, "original_file.txt");
            string newFilePath = Path.Combine(_testDirectory, "renamed_file.txt");
            File.WriteAllText(originalFilePath, "Test content");

            // 开始监控
            _fileMonitor.StartMonitoring(_testDirectory);

            // 重命名文件
            File.Move(originalFilePath, newFilePath);

            // 等待事件触发
            await Task.Delay(1000);

            // 验证事件是否触发
            Assert.True(_fileRenamedEventRaised);
        }

        [Fact]
        public void StartMonitoring_Should_Stop_Previous_Monitoring()
        {
            // 创建第二个测试目录
            string secondTestDirectory = Path.Combine(Path.GetTempPath(), "FileMonitorTests_Second_", DateTime.Now.Ticks.ToString());
            Directory.CreateDirectory(secondTestDirectory);

            try
            {
                // 先监控第一个目录
                _fileMonitor.StartMonitoring(_testDirectory);
                Assert.True(_fileMonitor.IsMonitoring);

                // 再监控第二个目录，这应该会停止对第一个目录的监控
                _fileMonitor.StartMonitoring(secondTestDirectory);
                Assert.True(_fileMonitor.IsMonitoring);
            }
            finally
            {
                // 清理第二个测试目录
                if (Directory.Exists(secondTestDirectory))
                {
                    Directory.Delete(secondTestDirectory, true);
                }
            }
        }

        // 清理测试资源
        public void Dispose()
        {
            // 停止监控
            _fileMonitor.StopMonitoring();

            // 清理测试目录
            if (Directory.Exists(_testDirectory))
            {
                try
                {
                    Directory.Delete(_testDirectory, true);
                }
                catch (IOException)
                {
                    // 如果文件被锁定，忽略异常
                }
            }
        }
    }
}