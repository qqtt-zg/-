using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using WindowsFormsApp3.Forms.Dialogs;
using WindowsFormsApp3.Models;
using WindowsFormsApp3.Services;

namespace WindowsFormsApp3.Tests.Services
{
    /// <summary>
    /// 批量处理服务测试类
    /// </summary>
    public class BatchProcessingServiceTests : IDisposable
    {
        private readonly Mock<WindowsFormsApp3.Interfaces.IFileRenameService> _mockFileRenameService;
        private readonly Mock<WindowsFormsApp3.Services.IPdfProcessingService> _mockPdfProcessingService;
        private readonly Mock<WindowsFormsApp3.Interfaces.ILogger> _mockLogger;
        private readonly BatchProcessingService _batchProcessingService;
        private readonly string _testDir;
        private bool _disposed;

        /// <summary>
        /// 构造函数，初始化测试环境
        /// </summary>
        public BatchProcessingServiceTests()
        {
            // 创建mock对象
            _mockFileRenameService = new Mock<WindowsFormsApp3.Interfaces.IFileRenameService>();
            _mockPdfProcessingService = new Mock<WindowsFormsApp3.Services.IPdfProcessingService>();
            _mockLogger = new Mock<WindowsFormsApp3.Interfaces.ILogger>();
            var mockEventBus = new Mock<WindowsFormsApp3.Services.IEventBus>();

            // 创建服务实例
            _batchProcessingService = new BatchProcessingService(
                _mockFileRenameService.Object,
                _mockPdfProcessingService.Object,
                _mockLogger.Object,
                mockEventBus.Object
            );

            // 创建测试目录
            _testDir = Path.Combine(Path.GetTempPath(), "BatchProcessingServiceTests_", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDir);

            _disposed = false;
        }

        /// <summary>
        /// 测试批量处理服务的基本功能
        /// </summary>
        [Fact]
        public async Task StartBatchProcessingAsync_ShouldProcessFilesSuccessfully()
        {
            // 准备测试数据
            var fileInfos = new List<FileRenameInfo>
            {
                new FileRenameInfo { FullPath = Path.Combine(_testDir, "file1.pdf"), OriginalName = "file1.pdf" },
                new FileRenameInfo { FullPath = Path.Combine(_testDir, "file2.pdf"), OriginalName = "file2.pdf" }
            };

            // 创建测试文件
            foreach (var fileInfo in fileInfos)
            {
                File.WriteAllText(fileInfo.FullPath, "test content");
            }

            // 设置mock行为
            _mockFileRenameService.Setup(s => s.RenameFileImmediately(It.IsAny<FileRenameInfo>(), It.IsAny<string>(), false)).Returns(true);
            _mockFileRenameService.Setup(s => s.GenerateNewFileName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns<string, string, string, int>((b, e, p, s) => $"{b}_{s}{e}");

            // 记录进度变化
            var progressChangedCount = 0;
            _batchProcessingService.ProgressChanged += (sender, e) => progressChangedCount++;

            // 记录完成事件
            bool processingComplete = false;
            BatchCompleteEventArgs completeEventArgs = null;
            _batchProcessingService.ProcessingComplete += (sender, e) =>
            {
                processingComplete = true;
                completeEventArgs = e;
            };

            // 执行测试
            await _batchProcessingService.StartBatchProcessingAsync(fileInfos, _testDir, false, 10, 2);

            // 验证结果
            Assert.True(processingComplete);
            Assert.Equal(2, completeEventArgs.SuccessCount);
            Assert.Equal(0, completeEventArgs.FailedCount);
            Assert.True(progressChangedCount > 0);

            // 验证服务调用
            _mockFileRenameService.Verify(s => s.RenameFileImmediately(It.IsAny<FileRenameInfo>(), _testDir, false), Times.Exactly(2));
            _mockLogger.Verify(l => l.LogInformation(It.Is<string>(s => s.Contains("开始批量处理"))), Times.Once);
            _mockLogger.Verify(l => l.LogInformation(It.Is<string>(s => s.Contains("批量处理完成"))), Times.Once);
        }

        /// <summary>
        /// 测试批量处理服务在处理失败文件时的行为
        /// </summary>
        [Fact]
        public async Task StartBatchProcessingAsync_ShouldHandleFailedFiles()
        {
            // 准备测试数据
            var fileInfos = new List<FileRenameInfo>
            {
                new FileRenameInfo { FullPath = Path.Combine(_testDir, "file1.pdf"), OriginalName = "file1.pdf" },
                new FileRenameInfo { FullPath = Path.Combine(_testDir, "file2.pdf"), OriginalName = "file2.pdf" }
            };

            // 创建一个测试文件，另一个不存在
            File.WriteAllText(fileInfos[0].FullPath, "test content");

            // 设置mock行为
            _mockFileRenameService.Setup(s => s.RenameFileImmediately(It.Is<FileRenameInfo>(f => f.OriginalName == "file1.pdf"), _testDir, false)).Returns(true);
            _mockFileRenameService.Setup(s => s.RenameFileImmediately(It.Is<FileRenameInfo>(f => f.OriginalName == "file2.pdf"), _testDir, false)).Returns(false);
            _mockFileRenameService.Setup(s => s.GenerateNewFileName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns<string, string, string, int>((b, e, p, s) => $"{b}_{s}{e}");

            // 记录完成事件
            BatchCompleteEventArgs completeEventArgs = null;
            _batchProcessingService.ProcessingComplete += (sender, e) => completeEventArgs = e;

            // 执行测试
            await _batchProcessingService.StartBatchProcessingAsync(fileInfos, _testDir, false, 10, 2);

            // 验证结果
            Assert.NotNull(completeEventArgs);
            Assert.Equal(1, completeEventArgs.SuccessCount);
            Assert.Equal(1, completeEventArgs.FailedCount);
            Assert.Single(completeEventArgs.FailedFiles);
            Assert.Equal(fileInfos[1].FullPath, completeEventArgs.FailedFiles[0].FullPath);
        }

        /// <summary>
        /// 测试取消批量处理功能
        /// </summary>
        [Fact]
        public async Task CancelProcessing_ShouldStopProcessing()
        {
            // 准备测试数据
            var fileInfos = new List<FileRenameInfo>
            {
                new FileRenameInfo { FullPath = Path.Combine(_testDir, "file1.pdf"), OriginalName = "file1.pdf" },
                new FileRenameInfo { FullPath = Path.Combine(_testDir, "file2.pdf"), OriginalName = "file2.pdf" },
                new FileRenameInfo { FullPath = Path.Combine(_testDir, "file3.pdf"), OriginalName = "file3.pdf" }
            };

            // 创建测试文件
            foreach (var fileInfo in fileInfos)
            {
                File.WriteAllText(fileInfo.FullPath, "test content");
            }

            // 设置mock行为，使第一个文件处理成功，第二个文件处理时模拟延迟
            int renameCount = 0;
            _mockFileRenameService.Setup(s => s.RenameFileImmediately(It.IsAny<FileRenameInfo>(), _testDir, false))
                .Returns(() =>
                {
                    renameCount++;
                    if (renameCount == 2) // 第二个文件处理时模拟延迟
                    {
                        Task.Delay(100).Wait(); // 给取消操作留出时间
                    }
                    return true;
                });

            // 创建取消任务
            var cancelTask = Task.Run(async () =>
            {
                await Task.Delay(50); // 延迟取消，确保已经开始处理
                _batchProcessingService.CancelProcessing();
            });

            // 记录完成事件
            BatchCompleteEventArgs completeEventArgs = null;
            _batchProcessingService.ProcessingComplete += (sender, e) => completeEventArgs = e;

            // 执行测试
            await Task.WhenAll(
                _batchProcessingService.StartBatchProcessingAsync(fileInfos, _testDir, false, 1, 1), // 使用串行处理
                cancelTask
            );

            // 验证结果
            Assert.NotNull(completeEventArgs);
            Assert.True(completeEventArgs.IsCanceled);
            Assert.True(renameCount <= 3); // 可能处理了1-3个文件，但应该被取消
        }

        /// <summary>
        /// 测试PDF文件添加图层功能
        /// </summary>
        [Fact]
        public async Task ProcessSingleFile_ShouldAddLayerToPdf()
        {
            // 准备测试数据
            var fileInfo = new FileRenameInfo
            {
                FullPath = Path.Combine(_testDir, "test.pdf"),
                OriginalName = "test.pdf",
                Material = "Test Material" // 设置Material属性，使needAddLayer条件为true
            };

            // 创建测试文件
            File.WriteAllText(fileInfo.FullPath, "test content");

            // 设置mock行为
            _mockPdfProcessingService.Setup(s => s.AddLayerToPdf(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PdfLayerInfo>())).Returns(true);
            _mockFileRenameService.Setup(s => s.RenameFileImmediately(It.IsAny<FileRenameInfo>(), _testDir, false)).Returns(true);
            _mockFileRenameService.Setup(s => s.GenerateNewFileName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(fileInfo.OriginalName);

            // 执行测试
            await _batchProcessingService.StartBatchProcessingAsync(new List<FileRenameInfo> { fileInfo }, _testDir, false, 10, 1);

            // 验证结果
            _mockPdfProcessingService.Verify(s => s.AddLayerToPdf(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PdfLayerInfo>()), Times.Once);
            _mockFileRenameService.Verify(s => s.RenameFileImmediately(It.IsAny<FileRenameInfo>(), _testDir, false), Times.Once);
        }

        /// <summary>
        /// 测试异常处理
        /// </summary>
        [Fact]
        public async Task StartBatchProcessingAsync_ShouldHandleExceptions()
        {
            // 准备测试数据
            var fileInfos = new List<FileRenameInfo>
            {
                new FileRenameInfo { FullPath = Path.Combine(_testDir, "file1.pdf"), OriginalName = "file1.pdf" }
            };

            // 创建测试文件
            File.WriteAllText(fileInfos[0].FullPath, "test content");

            // 设置mock行为，抛出异常
            _mockFileRenameService.Setup(s => s.RenameFileImmediately(It.IsAny<FileRenameInfo>(), _testDir, false))
                .Throws(new Exception("Test exception"));

            // 记录完成事件
            BatchCompleteEventArgs completeEventArgs = null;
            _batchProcessingService.ProcessingComplete += (sender, e) => completeEventArgs = e;

            // 执行测试
            await _batchProcessingService.StartBatchProcessingAsync(fileInfos, _testDir, false, 10, 1);

            // 验证结果
            Assert.NotNull(completeEventArgs);
            Assert.Equal(0, completeEventArgs.SuccessCount);
            Assert.Equal(1, completeEventArgs.FailedCount);
            Assert.Single(completeEventArgs.FailedFiles);
            Assert.Contains("Test exception", completeEventArgs.FailedFiles[0].ErrorMessage);

            // 验证日志记录
            _mockLogger.Verify(l => l.LogError(It.IsAny<Exception>(), It.Is<string>(s => s.Contains("处理文件 file1.pdf 时出错"))), Times.Once);
        }

        /// <summary>
        /// 测试空文件列表
        /// </summary>
        [Fact]
        public async Task StartBatchProcessingAsync_WithEmptyFileList_ShouldDoNothing()
        {
            // 执行测试，传入空文件列表
            await _batchProcessingService.StartBatchProcessingAsync(new List<FileRenameInfo>(), _testDir, false, 10, 2);

            // 验证没有调用任何服务
            _mockFileRenameService.Verify(s => s.RenameFileImmediately(It.IsAny<FileRenameInfo>(), _testDir, false), Times.Never);
            _mockPdfProcessingService.Verify(s => s.AddLayerToPdf(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PdfLayerInfo>()), Times.Never);
        }

        /// <summary>
        /// 测试创建不存在的导出目录
        /// </summary>
        [Fact]
        public async Task StartBatchProcessingAsync_ShouldCreateExportDirectory()
        {
            // 准备测试数据
            var fileInfos = new List<FileRenameInfo>
            {
                new FileRenameInfo { FullPath = Path.Combine(_testDir, "file1.pdf"), OriginalName = "file1.pdf" }
            };

            // 创建测试文件
            File.WriteAllText(fileInfos[0].FullPath, "test content");

            // 设置mock行为
            _mockFileRenameService.Setup(s => s.RenameFileImmediately(It.IsAny<FileRenameInfo>(), It.IsAny<string>(), false)).Returns(true);
            _mockFileRenameService.Setup(s => s.GenerateNewFileName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns<string, string, string, int>((b, e, p, s) => $"{b}_{s}{e}");

            // 创建不存在的导出目录路径
            string newExportDir = Path.Combine(_testDir, "NewExportDir");
            Assert.False(Directory.Exists(newExportDir));

            // 执行测试
            await _batchProcessingService.StartBatchProcessingAsync(fileInfos, newExportDir, false, 10, 1);

            // 验证目录已创建
            Assert.True(Directory.Exists(newExportDir));
        }

        /// <summary>
        /// 测试批处理大小参数
        /// </summary>
        [Fact]
        public async Task StartBatchProcessingAsync_ShouldRespectBatchSize()
        {
            // 准备测试数据 - 5个文件
            var fileInfos = new List<FileRenameInfo>();
            for (int i = 1; i <= 5; i++)
            {
                string filePath = Path.Combine(_testDir, $"file{i}.pdf");
                File.WriteAllText(filePath, "test content");
                fileInfos.Add(new FileRenameInfo { FullPath = filePath, OriginalName = $"file{i}.pdf" });
            }

            // 设置mock行为
            _mockFileRenameService.Setup(s => s.RenameFileImmediately(It.IsAny<FileRenameInfo>(), _testDir, false)).Returns(true);
            _mockFileRenameService.Setup(s => s.GenerateNewFileName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns<string, string, string, int>((b, e, p, s) => $"{b}_{s}{e}");

            // 记录批次处理的日志
            int batchProcessCount = 0;
            _mockLogger.Setup(l => l.LogDebug(It.Is<string>(s => s.StartsWith("开始处理批次"))))
                .Callback(() => batchProcessCount++);

            // 执行测试，设置批次大小为2
            await _batchProcessingService.StartBatchProcessingAsync(fileInfos, _testDir, false, 2, 1);

            // 验证结果 - 应该有3个批次(2+2+1)
            Assert.Equal(3, batchProcessCount);
        }

        /// <summary>
        /// 测试并行度参数
        /// </summary>
        [Fact]
        public async Task StartBatchProcessingAsync_ShouldRespectMaxDegreeOfParallelism()
        {
            // 准备测试数据 - 4个文件
            var fileInfos = new List<FileRenameInfo>();
            for (int i = 1; i <= 4; i++)
            {
                string filePath = Path.Combine(_testDir, $"file{i}.pdf");
                File.WriteAllText(filePath, "test content");
                fileInfos.Add(new FileRenameInfo { FullPath = filePath, OriginalName = $"file{i}.pdf" });
            }

            // 设置mock行为，模拟处理时间
            _mockFileRenameService.Setup(s => s.RenameFileImmediately(It.IsAny<FileRenameInfo>(), It.IsAny<string>(), false))
                .Callback(() => Task.Delay(50).Wait()) // 模拟处理延迟
                .Returns(true);
            _mockFileRenameService.Setup(s => s.GenerateNewFileName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns<string, string, string, int>((b, e, p, s) => $"{b}_{s}{e}");

            // 记录完成事件
            BatchCompleteEventArgs completeEventArgs = null;
            _batchProcessingService.ProcessingComplete += (sender, e) => completeEventArgs = e;

            // 执行测试，设置最大并行度为2
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            await _batchProcessingService.StartBatchProcessingAsync(fileInfos, _testDir, false, 10, 2);
            stopwatch.Stop();

            // 验证结果
            Assert.NotNull(completeEventArgs);
            Assert.Equal(4, completeEventArgs.SuccessCount);

            // 验证並行处理确实提高了速度
            // 对于4个文件，每个需要50ms，如果是串行处理大约需要200ms，並行处理应该明显少于200ms
            Assert.True(stopwatch.ElapsedMilliseconds < 150);
        }
        
        /// <summary>
        /// 测试保留字段恢复功能
        /// </summary>
        [Fact]
        public void RestorePreservedFields_ShouldRestoreBackupDataToProperties()
        {
            // 准备测试数据
            var fileInfos = new List<FileRenameInfo>
            {
                new FileRenameInfo
                {
                    FullPath = Path.Combine(_testDir, "file1.pdf"),
                    OriginalName = "file1.pdf",
                    IsPreserveMode = true,
                    OrderNumber = "new-order-001",
                    Material = "new-material",
                    Process = "new-process",
                    RegexResult = "new-regex",
                    BackupData = new Dictionary<string, string>
                    {
                        { "订单号", "old-order-001" },
                        { "材料", "old-material" },
                        { "工艺", "old-process" },
                        { "正则结果", "old-regex" }
                    }
                }
            };
        
            // 执行恢复操作
            _batchProcessingService.RestorePreservedFields(fileInfos);
        
            // 验证结果 - 备份数据应该被恢复到对应的属性
            var fileInfo = fileInfos[0];
            Assert.Equal("old-order-001", fileInfo.OrderNumber);
            Assert.Equal("old-material", fileInfo.Material);
            Assert.Equal("old-process", fileInfo.Process);
            Assert.Equal("old-regex", fileInfo.RegexResult);
        }
        
        /// <summary>
        /// 测试空备份数据情况下的恢复功能
        /// </summary>
        [Fact]
        public void RestorePreservedFields_WithEmptyBackupData_ShouldNotThrowException()
        {
            // 准备测试数据
            var fileInfos = new List<FileRenameInfo>
            {
                new FileRenameInfo
                {
                    FullPath = Path.Combine(_testDir, "file1.pdf"),
                    OriginalName = "file1.pdf",
                    IsPreserveMode = true,
                    BackupData = new Dictionary<string, string>()
                }
            };
        
            // 应该不抛出异常
            _batchProcessingService.RestorePreservedFields(fileInfos);
        
            // 验证文件对象仍然有效
            Assert.NotNull(fileInfos[0]);
            Assert.Equal("file1.pdf", fileInfos[0].OriginalName);
        }
        
        /// <summary>
        /// 测试恢复操作时的日志记录
        /// </summary>
        [Fact]
        public void RestorePreservedFields_ShouldLogRestoreInformation()
        {
            // 准备测试数据
            var fileInfos = new List<FileRenameInfo>
            {
                new FileRenameInfo
                {
                    FullPath = Path.Combine(_testDir, "file1.pdf"),
                    OriginalName = "file1.pdf",
                    IsPreserveMode = true,
                    BackupData = new Dictionary<string, string>
                    {
                        { "订单号", "backup-order" }
                    }
                }
            };
        
            // 执行恢复操作
            _batchProcessingService.RestorePreservedFields(fileInfos);
        
            // 验证日志被记录
            _mockLogger.Verify(
                l => l.LogInformation(It.Is<string>(s => s.Contains("已恢复") || s.Contains("保留"))),
                Times.AtLeastOnce);
        }

        /// <summary>
        /// 测试动态字段（正则结果、尺寸）的备份功能
        /// </summary>
        [Fact]
        public void BackupFieldFromOriginalName_ShouldHandleDynamicFields()
        {
            // 准备测试数据
            var fileInfo = new FileRenameInfo
            {
                OriginalName = "&ID-11-228&MT-hc&DP-彩色光膜&MK-68x41Z&Row-9&Col-4.pdf",
                RegexResult = "test-regex-result",
                Dimensions = "71x44"
            };

            // 测试备份"尺寸"（从原文件名提取）
            fileInfo.BackupFieldFromOriginalName("尺寸");
            Assert.True(fileInfo.BackupData.ContainsKey("尺寸"));
            // 尺寸格式 "68x41Z" 应该被提取
            Assert.NotEmpty(fileInfo.BackupData["尺寸"]);

            // 测试备份"正则结果"（从当前属性值返回）
            fileInfo.BackupFieldFromOriginalName("正则结果");
            Assert.True(fileInfo.BackupData.ContainsKey("正则结果"));
            Assert.Equal("test-regex-result", fileInfo.BackupData["正则结果"]);
        }

        /// <summary>
        /// 测试所有保留字段的完整备份流程
        /// </summary>
        [Fact]
        public void ApplyPreserveModeToFileList_ShouldBackupAllFields()
        {
            // 准备测试数据
            var fileInfos = new List<FileRenameInfo>
            {
                new FileRenameInfo
                {
                    OriginalName = "&ID-11-228&MT-hc&DP-彩色光膜&MK-68x41Z&Row-9&Col-4.pdf",
                    RegexResult = "regex-result",
                    Dimensions = "71x44"
                }
            };

            // 模拟保留分组配置
            var preserveGroupConfigs = new List<EventGroupConfig>
            {
                new EventGroupConfig
                {
                    Items = new List<string> { "订单号", "材料", "工艺", "尺寸", "正则结果", "行数", "列数" },
                    IsPreserved = true
                }
            };

            // 设置保留分组配置
            _batchProcessingService.SetPreserveGroupConfigs(preserveGroupConfigs);

            // 执行备份
            _batchProcessingService.ApplyPreserveModeToFileList(fileInfos);

            // 验证结果
            var fileInfo = fileInfos[0];
            Assert.True(fileInfo.IsPreserveMode);
            Assert.NotEmpty(fileInfo.BackupData);

            // 验证各个字段都有备份（除了无法从原文件名提取的）
            Assert.True(fileInfo.BackupData.ContainsKey("订单号"));
            Assert.True(fileInfo.BackupData.ContainsKey("材料"));
            Assert.True(fileInfo.BackupData.ContainsKey("工艺"));
            // 尺寸应该从"68x41Z"提取
            Assert.True(fileInfo.BackupData.ContainsKey("尺寸"));
            // 正则结果应该从属性值返回
            Assert.True(fileInfo.BackupData.ContainsKey("正则结果"));
            // 行数应该从"&Row-9"提取
            Assert.True(fileInfo.BackupData.ContainsKey("行数"));
            // 列数应该从"&Col-4"提取
            Assert.True(fileInfo.BackupData.ContainsKey("列数"));
        }

        /// <summary>
        /// 清理测试资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 清理测试资源
        /// </summary>
        /// <param name="disposing">是否释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 清理测试目录
                    if (Directory.Exists(_testDir))
                    {
                        try
                        {
                            // 确保所有文件都被释放
                            foreach (var file in Directory.GetFiles(_testDir))
                            {
                                try
                                {
                                    File.Delete(file);
                                }
                                catch (Exception) { /* 忽略无法删除的文件 */ }
                            }
                            Directory.Delete(_testDir, true);
                        }
                        catch (Exception) { /* 忽略删除目录时的异常 */ }
                    }
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~BatchProcessingServiceTests()
        {
            Dispose(false);
        }
    }
}