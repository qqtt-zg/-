using System;
using Xunit;
using Moq;
using WindowsFormsApp3.Services;

namespace WindowsFormsApp3.Tests.Services
{
    public class ServiceLocatorTests
    {
        public ServiceLocatorTests()
        {
            // 重置ServiceLocator实例以确保测试隔离
            ServiceLocator.Reset();
        }

        [Fact]
        public void Instance_Should_Return_Singleton_Instance()
        {
            // Act
            var instance1 = ServiceLocator.Instance;
            var instance2 = ServiceLocator.Instance;

            // Assert
            Assert.NotNull(instance1);
            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void GetServices_Should_Return_Correct_Service_Instances()
        {
            // Arrange
            var serviceLocator = ServiceLocator.Instance;

            // Act
            var excelImportService = serviceLocator.GetExcelImportService();
            var fileMonitor = serviceLocator.GetFileMonitor();
            var fileRenameService = serviceLocator.GetFileRenameService();
            var pdfProcessingService = serviceLocator.GetPdfProcessingService();
            var batchProcessingService = serviceLocator.GetBatchProcessingService();
            var eventBus = serviceLocator.GetEventBus();

            // Assert
            Assert.NotNull(excelImportService);
            Assert.NotNull(fileMonitor);
            Assert.NotNull(fileRenameService);
            Assert.NotNull(pdfProcessingService);
            Assert.NotNull(batchProcessingService);
            Assert.NotNull(eventBus);
        }

        [Fact]
        public void RegisterCustomService_Should_Replace_Existing_Service()
        {
            // Arrange
            var serviceLocator = ServiceLocator.Instance;
            var mockExcelImportService = new Mock<IExcelImportService>();
            
            // 获取原始服务
            var originalService = serviceLocator.GetExcelImportService();
            
            // Act
            serviceLocator.RegisterExcelImportService(mockExcelImportService.Object);
            
            // 获取新服务
            var newService = serviceLocator.GetExcelImportService();

            // Assert
            Assert.NotNull(originalService);
            Assert.NotNull(newService);
            Assert.NotSame(originalService, newService);
            Assert.Same(mockExcelImportService.Object, newService);
        }

        [Fact]
        public void RegisterCustomFileMonitorService_Should_Replace_Existing_Service()
        {
            // Arrange
            var serviceLocator = ServiceLocator.Instance;
            var mockFileMonitor = new Mock<WindowsFormsApp3.Services.IFileMonitor>(); // 明确指定命名空间
            
            // 获取原始服务
            var originalService = serviceLocator.GetFileMonitor();
            
            // Act
            serviceLocator.RegisterFileMonitor(mockFileMonitor.Object);
            
            // 获取新服务
            var newService = serviceLocator.GetFileMonitor();

            // Assert
            Assert.NotNull(originalService);
            Assert.NotNull(newService);
            Assert.NotSame(originalService, newService);
            Assert.Same(mockFileMonitor.Object, newService);
        }

        [Fact]
        public void RegisterCustomFileRenameService_Should_Replace_Existing_Service()
        {
            // Arrange
            var serviceLocator = ServiceLocator.Instance;
            var mockFileRenameService = new Mock<WindowsFormsApp3.Services.IFileRenameService>(); // 明确指定命名空间
            
            // 获取原始服务
            var originalService = serviceLocator.GetFileRenameService();
            
            // Act
            serviceLocator.RegisterFileRenameService(mockFileRenameService.Object);
            
            // 获取新服务
            var newService = serviceLocator.GetFileRenameService();

            // Assert
            Assert.NotNull(originalService);
            Assert.NotNull(newService);
            Assert.NotSame(originalService, newService);
            Assert.Same(mockFileRenameService.Object, newService);
        }

[Fact]
        public void Logger_Property_Should_Return_Correct_Instance()
        {
            // Arrange
            var serviceLocator = ServiceLocator.Instance;

            // Act
            var logger = serviceLocator.Logger;

            // Assert
            Assert.NotNull(logger);
        }

        [Fact]
        public void GetAllServices_Should_Return_All_Services()
        {
            // Arrange
            var serviceLocator = ServiceLocator.Instance;

            // Act
            var allServices = serviceLocator.GetAllServices();

            // Assert
            Assert.NotNull(allServices);
            Assert.Equal(7, allServices.Count); // 6个服务 + 1个EventBus
        }

        [Fact]
        public void IsServiceRegistered_Should_Return_Correct_Result()
        {
            // Arrange
            var serviceLocator = ServiceLocator.Instance;

            // Act
            var isExcelImportServiceRegistered = serviceLocator.IsServiceRegistered<IExcelImportService>();
            var isUnknownServiceRegistered = serviceLocator.IsServiceRegistered<string>();

            // Assert
            Assert.True(isExcelImportServiceRegistered);
            Assert.False(isUnknownServiceRegistered);
        }

        [Fact]
        public void GetServiceCount_Should_Return_Correct_Count()
        {
            // Arrange
            var serviceLocator = ServiceLocator.Instance;

            // Act
            var serviceCount = serviceLocator.GetServiceCount();

            // Assert
            Assert.True(serviceCount > 0);
        }

        [Fact]
        public void RegisterCustomPdfProcessingService_Should_Replace_Existing_Service()
        {
            // Arrange
            var serviceLocator = ServiceLocator.Instance;
            var mockPdfProcessingService = new Mock<WindowsFormsApp3.Services.IPdfProcessingService>(); // 明确指定命名空间
            
            // 获取原始服务
            var originalService = serviceLocator.GetPdfProcessingService();
            
            // Act
            serviceLocator.RegisterPdfProcessingService(mockPdfProcessingService.Object);
            
            // 获取新服务
            var newService = serviceLocator.GetPdfProcessingService();

            // Assert
            Assert.NotNull(originalService);
            Assert.NotNull(newService);
            Assert.NotSame(originalService, newService);
            Assert.Same(mockPdfProcessingService.Object, newService);
        }

[Fact]
        public void RegisterCustomBatchProcessingService_Should_Replace_Existing_Service()
        {
            // Arrange
            var serviceLocator = ServiceLocator.Instance;
            var mockBatchProcessingService = new Mock<IBatchProcessingService>();
            
            // 获取原始服务
            var originalService = serviceLocator.GetBatchProcessingService();
            
            // Act
            serviceLocator.RegisterBatchProcessingService(mockBatchProcessingService.Object);
            
            // 获取新服务
            var newService = serviceLocator.GetBatchProcessingService();

            // Assert
            Assert.NotNull(originalService);
            Assert.NotNull(newService);
            Assert.NotSame(originalService, newService);
            Assert.Same(mockBatchProcessingService.Object, newService);
        }

        [Fact]
        public void RegisterCustomLogger_Should_Replace_Existing_Service()
        {
            // Arrange
            var serviceLocator = ServiceLocator.Instance;
            var mockLogger = new Mock<WindowsFormsApp3.Interfaces.ILogger>();
            
            // 获取原始服务
            var originalService = serviceLocator.Logger; // 使用Logger属性而不是GetLogger方法
            
            // Act
            serviceLocator.RegisterLogger(mockLogger.Object);
            
            // 获取新服务
            var newService = serviceLocator.Logger; // 使用Logger属性而不是GetLogger方法

            // Assert
            Assert.NotNull(originalService);
            Assert.NotNull(newService);
            Assert.NotSame(originalService, newService);
            Assert.Same(mockLogger.Object, newService);
        }
    }
}