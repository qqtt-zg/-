using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using WindowsFormsApp3;
using WindowsFormsApp3.Services;
using WindowsFormsApp3.Models;

namespace WindowsFormsApp3.Tests.Integration
{
    public class EndToEndTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly ServiceLocator _serviceLocator;
        // 移除对版本控制服务的引用

        public EndToEndTests()
        {
            // 创建临时测试目录
            _testDirectory = Path.Combine(Path.GetTempPath(), "EndToEndTests_", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDirectory);
            
            // 获取服务定位器实例
            _serviceLocator = ServiceLocator.Instance;
            // 移除对版本控制服务的初始化
        }

        public void Dispose()
        {
            // 清理测试目录
            if (Directory.Exists(_testDirectory))
            {
                try
                {
                    Directory.Delete(_testDirectory, true);
                }
                catch (Exception)
                {
                    // 忽略删除失败的情况
                }
            }
        }

        [Fact]
        public async Task EndToEnd_FileProcessing_Workflow_Should_Work_Correctly()
        {
            // 创建测试文件
            var testFiles = CreateTestFiles(5);
            
            // 获取服务
            var fileRenameService = _serviceLocator.GetFileRenameService();
            var batchProcessingService = _serviceLocator.GetBatchProcessingService();
            
            // 验证服务不为null
            Assert.NotNull(fileRenameService);
            Assert.NotNull(batchProcessingService);
            
            // 创建文件重命名信息列表
            var fileRenameInfos = testFiles.Select(f => new FileRenameInfo
            {
                FullPath = f,
                OriginalName = Path.GetFileName(f),
                NewName = $"renamed_{Path.GetFileName(f)}",
                Material = "TestMaterial",
                OrderNumber = "TestOrder",
                Quantity = "10",
                Dimensions = "100x200",
                Process = "Fixed", // 使用Process属性替代FixedField
                SerialNumber = "001"
            }).ToList();
            
            // 执行批量处理
            await batchProcessingService.StartBatchProcessingAsync(
                fileRenameInfos,
                _testDirectory,
                isCopyMode: true,
                batchSize: 3,
                maxDegreeOfParallelism: 2);
            
            // 验证结果
            var renamedFiles = Directory.GetFiles(_testDirectory, "renamed_*");
            Assert.Equal(testFiles.Count, renamedFiles.Length);
        }

        [Fact]
        public void ServiceLocator_Should_Provide_All_Required_Services()
        {
            // 获取所有服务（ConfigService已移除）
            var excelImportService = _serviceLocator.GetExcelImportService();
            var fileMonitor = _serviceLocator.GetFileMonitor();
            var fileRenameService = _serviceLocator.GetFileRenameService();
            var pdfProcessingService = _serviceLocator.GetPdfProcessingService();
            var batchProcessingService = _serviceLocator.GetBatchProcessingService();
            var eventBus = _serviceLocator.GetEventBus();
            
            // 验证所有服务都不为null
            Assert.NotNull(excelImportService);
            Assert.NotNull(fileMonitor);
            Assert.NotNull(fileRenameService);
            Assert.NotNull(pdfProcessingService);
            Assert.NotNull(batchProcessingService);
            Assert.NotNull(eventBus);
        }

        private List<string> CreateTestFiles(int count)
        {
            var files = new List<string>();
            
            for (int i = 1; i <= count; i++)
            {
                string fileName = $"test_file_{i}.txt";
                string filePath = Path.Combine(_testDirectory, fileName);
                
                // 创建测试文件内容
                File.WriteAllText(filePath, $"This is test file {i}");
                files.Add(filePath);
            }
            
            return files;
        }
    }
}