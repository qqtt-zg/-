using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Moq;
// 使用别名解决命名冲突
using IExcelImportService = WindowsFormsApp3.Services.IExcelImportService;
using IFileMonitor = WindowsFormsApp3.Services.IFileMonitor;
using IFileRenameService = WindowsFormsApp3.Services.IFileRenameService;
using IPdfProcessingService = WindowsFormsApp3.Services.IPdfProcessingService;
using ILogger = WindowsFormsApp3.Interfaces.ILogger;
// 引入LogHelper
using LogHelper = WindowsFormsApp3.Utils.LogHelper;
// 直接使用ServiceLocator的完全限定名
using SL = WindowsFormsApp3.Services.ServiceLocator;
// 导入具体服务类
using FileMonitor = WindowsFormsApp3.Services.FileMonitor;
using BatchProcessingService = WindowsFormsApp3.Services.BatchProcessingService;
using ExcelImportHelper = WindowsFormsApp3.Services.ExcelImportHelper;
using BatchCompleteEventArgs = WindowsFormsApp3.Models.BatchCompleteEventArgs;
using FileRenameInfo = WindowsFormsApp3.FileRenameInfo;
using FileRenameService = WindowsFormsApp3.Services.FileRenameService;

namespace WindowsFormsApp3.Tests
{
    /// <summary>
    /// 综合单元测试，测试服务定位器、异常处理和服务交互
    /// </summary>
    public class UnitTest1
    {
        private readonly string _tempTestDir;

        public UnitTest1()
        {
            // 创建临时测试目录
            _tempTestDir = Path.Combine(Path.GetTempPath(), "ToolboxTests_", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempTestDir);
        }

        [Fact]
        public void ServiceLocator_Should_Return_Same_Instance_For_Multiple_Calls()
        {
            // 多次获取服务定位器实例
            var instance1 = SL.Instance;
            var instance2 = SL.Instance;
            
            // 验证返回的是同一个实例
            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void ServiceLocator_Should_Provide_All_Registered_Services()
        {
            var services = SL.Instance.GetAllServices();
            
            // 验证返回的服务字典不为空
            Assert.NotNull(services);
            Assert.NotEmpty(services);
            
            // 确保至少有6个服务被注册（已移除ConfigService）
            Assert.True(services.Count >= 6);
        }

        [Fact]
        public void ServiceLocator_Should_Allow_Registering_Custom_Services()
        {
            var locator = SL.Instance;
            var mockLogger = new Mock<WindowsFormsApp3.Interfaces.ILogger>();
            
            // 注册自定义服务
            locator.RegisterLogger(mockLogger.Object);
            
            // 验证自定义服务是否被正确注册
            Assert.Same(mockLogger.Object, locator.Logger);
        }

        [Fact]
        public void FileRenameService_Should_Handle_Null_Parameters_Gracefully()
        {
            // 创建服务实例（移除版本控制服务参数）
            var mockEventBus = new Mock<WindowsFormsApp3.Services.IEventBus>();
            var renameService = new FileRenameService(mockEventBus.Object);
            
            // 测试空参数
            bool result = renameService.RenameFileImmediately(null, "testPath", false);
            
            // 验证结果
            Assert.False(result);
        }

        [Fact]
        public void ServiceLocator_InitializeServices_Should_Prevent_Recursive_Initialization()
        {
            // 这个测试确保InitializeServices方法不会导致递归初始化
            // 如果发生递归初始化，这个测试会抛出StackOverflowException
            var locator = SL.Instance;
            
            // 验证能够成功获取依赖于其他服务的服务
            var renameService = locator.GetFileRenameService();
            // 移除对版本控制服务的验证，因为我们已经删除了它
            
            // 验证服务不为null
            Assert.NotNull(renameService);
        }

        [Fact]
        public void FileRenameService_With_No_VersionControlService_Should_Handle_Rename()
        {
            // 创建没有版本控制服务的文件重命名服务
            var mockEventBus = new Mock<WindowsFormsApp3.Services.IEventBus>();
            var renameService = new FileRenameService(mockEventBus.Object);
            
            // 创建测试文件
            string testFileName = "test.txt";
            string testFilePath = Path.Combine(_tempTestDir, testFileName);
            File.WriteAllText(testFilePath, "test content");
            
            // 创建目标目录
            string targetDir = Path.Combine(_tempTestDir, "target");
            Directory.CreateDirectory(targetDir);
            
            // 创建文件重命名信息
            var fileInfo = new FileRenameInfo {
                FullPath = testFilePath,
                OriginalName = testFileName,
                NewName = "renamed.txt"
            };
            
            // 执行重命名（使用复制模式避免文件锁定问题）
            bool result = renameService.RenameFileImmediately(fileInfo, targetDir, true);
            
            // 验证结果
            Assert.True(result);
            Assert.True(File.Exists(Path.Combine(targetDir, "renamed.txt")));
        }

        [Fact]
        public void ExcelImportHelperTests_Should_HasValidData_When_DataTable_Has_Rows()
        {
            // 创建Excel导入助手
            var importHelper = new ExcelImportHelper();
            
            // 创建有数据的DataTable
            var dataTable = new System.Data.DataTable();
            dataTable.Columns.Add("Column1");
            dataTable.Rows.Add("Row1");
            
            // 设置导入的数据
            importHelper.ImportedData = dataTable;
            
            // 验证HasValidData返回true
            Assert.True(importHelper.HasValidData());
        }

        [Fact]
        public void FileMonitor_Should_Start_Monitoring_Without_Logger()
        {
            // 创建没有日志服务的文件监控器
            var fileMonitor = new FileMonitor();
            
            // 验证可以启动监控而不会抛出异常
            fileMonitor.StartMonitoring(_tempTestDir);
            
            // 验证监控状态
            Assert.True(fileMonitor.IsMonitoring);
            
            // 停止监控
            fileMonitor.StopMonitoring();
        }

        [Fact]
        public async Task BatchProcessingService_Should_Handle_Empty_File_List()
        {
            // 创建模拟服务
            var mockRenameService = new Mock<WindowsFormsApp3.Interfaces.IFileRenameService>();
            var mockPdfService = new Mock<WindowsFormsApp3.Services.IPdfProcessingService>();
            var mockLogger = new Mock<WindowsFormsApp3.Interfaces.ILogger>();
            var mockEventBus = new Mock<WindowsFormsApp3.Services.IEventBus>();

            // 创建批量处理服务
            var batchService = new BatchProcessingService(mockRenameService.Object, mockPdfService.Object, mockLogger.Object, mockEventBus.Object);
            
            // 定义完成事件处理程序
            bool processingComplete = false;
            BatchCompleteEventArgs completeArgs = null;
            
            batchService.ProcessingComplete += (sender, e) =>
            {
                processingComplete = true;
                completeArgs = e;
            };
            
            // 启动处理空文件列表
            await batchService.StartBatchProcessingAsync(new System.Collections.Generic.List<FileRenameInfo>(), _tempTestDir, true);
            
            // 验证结果
            Assert.True(processingComplete);
            Assert.NotNull(completeArgs);
            Assert.Equal(0, completeArgs.TotalCount);
            Assert.Equal(0, completeArgs.SuccessCount);
            Assert.Equal(0, completeArgs.FailedCount);
        }

        // 清理临时文件和目录
        [Fact]
        public void Cleanup_Test_Directory()
        {
            try
            {
                if (Directory.Exists(_tempTestDir))
                {
                    // 尝试清理临时目录
                    // 注意：有些文件可能被锁定，所以需要特殊处理
                    Directory.Delete(_tempTestDir, true);
                }
            }
            catch (Exception ex)
            {
                // 记录清理失败但不影响测试结果
                LogHelper.Error("清理测试目录失败: " + ex.Message);
            }
        }
    }
}
