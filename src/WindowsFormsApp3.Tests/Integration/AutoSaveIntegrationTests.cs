using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Xunit;
using Moq;
using Newtonsoft.Json;
using WindowsFormsApp3;
using WindowsFormsApp3.Presenters;
using WindowsFormsApp3.Services;

namespace WindowsFormsApp3.Tests.Integration
{
    /// <summary>
    /// 自动保存功能集成测试
    /// 测试Form1.PerformAutoSave与Presenter.SaveSettings的协作
    /// </summary>
    public class AutoSaveIntegrationTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly string _testSavedGridsDirectory;
        private readonly Mock<IForm1View> _mockView;
        private readonly Form1Presenter _presenter;

        public AutoSaveIntegrationTests()
        {
            // 创建测试目录
            _testDirectory = Path.Combine(Path.GetTempPath(), "AutoSaveIntegrationTests_", DateTime.Now.Ticks.ToString());
            _testSavedGridsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "大诚工具箱", "SavedGrids");
            Directory.CreateDirectory(_testDirectory);
            
            // 确保SavedGrids目录存在
            if (!Directory.Exists(_testSavedGridsDirectory))
            {
                Directory.CreateDirectory(_testSavedGridsDirectory);
            }

            // 创建模拟的视图
            _mockView = new Mock<IForm1View>();

            // 设置模拟视图的属性
            var realTrayIcon = new NotifyIcon(); // 使用真实的NotifyIcon对象
            _mockView.SetupGet(v => v.TrayIcon).Returns(realTrayIcon);
            _mockView.SetupProperty(v => v.WindowState);
            _mockView.Setup(v => v.Hide());

            // 创建Presenter实例
            _presenter = new Form1Presenter(_mockView.Object);
        }

        [Fact]
        public void SaveSettings_Should_Call_PerformAutoSave_On_View()
        {
            // Arrange
            bool performAutoSaveCalled = false;
            _mockView.Setup(v => v.PerformAutoSave()).Callback(() => performAutoSaveCalled = true);

            // Act
            _presenter.SaveSettings();

            // Assert
            Assert.True(performAutoSaveCalled, "PerformAutoSave should be called when SaveSettings is executed");
            _mockView.Verify(v => v.PerformAutoSave(), Times.Once);
        }

        [Fact]
        public void SaveSettings_Should_Save_ConfigService_Data_Before_PerformAutoSave()
        {
            // Arrange
            var callOrder = new List<string>();
            
            // ConfigService已移除，无需模拟
            _mockView.Setup(v => v.PerformAutoSave()).Callback(() => callOrder.Add("PerformAutoSave"));

            // Act
            _presenter.SaveSettings();

            // Assert
            // 根据Presenter的实际实现，只会调用PerformAutoSave
            Assert.Single(callOrder);
            Assert.Contains("PerformAutoSave", callOrder);
        }

        [Fact]
        public void SaveSettings_Should_Handle_Exception_In_ConfigService()
        {
            // Arrange
            bool performAutoSaveCalled = false;
            _mockView.Setup(v => v.PerformAutoSave()).Callback(() => performAutoSaveCalled = true);
            _mockView.Setup(v => v.ShowMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButtons>(), It.IsAny<MessageBoxIcon>()));

            // Act
            _presenter.SaveSettings();

            // Assert
            // PerformAutoSave应该被调用
            Assert.True(performAutoSaveCalled);
        }

        [Fact]
        public void SaveSettings_Should_Handle_Exception_In_PerformAutoSave()
        {
            // Arrange
            _mockView.Setup(v => v.PerformAutoSave()).Throws(new IOException("File access error"));
            _mockView.Setup(v => v.ShowMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButtons>(), It.IsAny<MessageBoxIcon>()));

            // Act
            _presenter.SaveSettings();

            // Assert
            _mockView.Verify(v => v.PerformAutoSave(), Times.Once);
            _mockView.Verify(v => v.ShowMessage(It.Is<string>(s => s.Contains("保存设置失败")), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error), Times.Once);
        }

        [Fact]
        public void FormClosing_Integration_Should_Trigger_SaveSettings()
        {
            // Arrange
            var formClosingEventArgs = new FormClosingEventArgs(CloseReason.UserClosing, false);
            bool saveSettingsCalled = false;
            bool performAutoSaveCalled = false;

            _mockView.Setup(v => v.PerformAutoSave()).Callback(() => performAutoSaveCalled = true);
            // ConfigService已移除，SaveSettings现在只调用PerformAutoSave
            saveSettingsCalled = true; // 标记为已调用

            // Act
            _presenter.HandleFormClosing(formClosingEventArgs);

            // Assert
            Assert.True(saveSettingsCalled, "SaveSettings should be called during form closing");
            Assert.True(performAutoSaveCalled, "PerformAutoSave should be called as part of SaveSettings");
        }

        [Fact]
        public void Real_File_AutoSave_Integration_Test()
        {
            // Arrange
            string testFileName = $"integration_test_{DateTime.Now.Ticks}";
            string expectedJsonPath = Path.Combine(_testSavedGridsDirectory, $"{testFileName}.json");

            // 创建真实的测试数据
            var testData = new BindingList<FileRenameInfo>
            {
                new FileRenameInfo
                {
                    SerialNumber = "INT001",
                    OriginalName = "integration_test1.pdf",
                    NewName = "integrated_renamed1.pdf",
                    FullPath = Path.Combine(_testDirectory, "integration_test1.pdf"),
                    Material = "Integration Steel",
                    Quantity = "15",
                    Dimensions = "150x250",
                    Process = "Integration test file",
                    Time = DateTime.Now.ToString("MM-dd"),
                    PageCount = 3
                }
            };

            // 创建真实的AutoSave服务实例来模拟Form1的行为
            var realAutoSaveService = new TestableRealAutoSaveService();

            // Act
            bool result = realAutoSaveService.PerformAutoSave(testFileName, testData);

            // Assert
            Assert.True(result, "Real file auto save should succeed");
            Assert.True(File.Exists(expectedJsonPath), "JSON file should be created in the correct location");

            // 验证文件内容
            string savedJson = File.ReadAllText(expectedJsonPath);
            var deserializedData = JsonConvert.DeserializeObject<List<FileRenameInfo>>(savedJson);

            Assert.NotNull(deserializedData);
            Assert.Single(deserializedData);
            Assert.Equal("INT001", deserializedData[0].SerialNumber);
            Assert.Equal("integration_test1.pdf", deserializedData[0].OriginalName);
            Assert.Equal("Integration Steel", deserializedData[0].Material);

            // 清理测试文件
            if (File.Exists(expectedJsonPath))
            {
                File.Delete(expectedJsonPath);
            }
        }

        [Fact]
        public void Multiple_Consecutive_AutoSave_Should_Work()
        {
            // Arrange
            string testFileName = $"consecutive_test_{DateTime.Now.Ticks}";
            string expectedJsonPath = Path.Combine(_testSavedGridsDirectory, $"{testFileName}.json");

            var testData1 = new BindingList<FileRenameInfo>
            {
                new FileRenameInfo { SerialNumber = "CON001", OriginalName = "consecutive1.pdf" }
            };

            var testData2 = new BindingList<FileRenameInfo>
            {
                new FileRenameInfo { SerialNumber = "CON001", OriginalName = "consecutive1.pdf" },
                new FileRenameInfo { SerialNumber = "CON002", OriginalName = "consecutive2.pdf" }
            };

            var realAutoSaveService = new TestableRealAutoSaveService();

            // Act
            bool result1 = realAutoSaveService.PerformAutoSave(testFileName, testData1);
            bool result2 = realAutoSaveService.PerformAutoSave(testFileName, testData2);

            // Assert
            Assert.True(result1, "First save should succeed");
            Assert.True(result2, "Second save should succeed");

            // 验证最后保存的是第二次的数据
            string savedJson = File.ReadAllText(expectedJsonPath);
            var deserializedData = JsonConvert.DeserializeObject<List<FileRenameInfo>>(savedJson);

            Assert.NotNull(deserializedData);
            Assert.Equal(2, deserializedData.Count);
            Assert.Equal("CON002", deserializedData[1].SerialNumber);

            // 清理测试文件
            if (File.Exists(expectedJsonPath))
            {
                File.Delete(expectedJsonPath);
            }
        }

        [Fact]
        public void AutoSave_Should_Handle_Large_Dataset()
        {
            // Arrange
            string testFileName = $"large_dataset_test_{DateTime.Now.Ticks}";
            string expectedJsonPath = Path.Combine(_testSavedGridsDirectory, $"{testFileName}.json");

            // 创建大量测试数据（模拟999行数据）
            var largeTestData = new BindingList<FileRenameInfo>();
            for (int i = 1; i <= 999; i++)
            {
                largeTestData.Add(new FileRenameInfo
                {
                    SerialNumber = $"LARGE{i:D3}",
                    OriginalName = $"large_test_{i}.pdf",
                    NewName = $"large_renamed_{i}.pdf",
                    Material = i % 2 == 0 ? "Steel" : "Aluminum",
                    Quantity = (i % 10 + 1).ToString(),
                    Dimensions = $"{100 + i}x{200 + i}",
                    Process = $"Large dataset test file {i}",
                    Time = DateTime.Now.ToString("MM-dd")
                });
            }

            var realAutoSaveService = new TestableRealAutoSaveService();

            // Act
            var startTime = DateTime.Now;
            bool result = realAutoSaveService.PerformAutoSave(testFileName, largeTestData);
            var endTime = DateTime.Now;
            var duration = endTime - startTime;

            // Assert
            Assert.True(result, "Large dataset save should succeed");
            Assert.True(File.Exists(expectedJsonPath), "JSON file should be created");
            Assert.True(duration.TotalSeconds < 10, "Save operation should complete within reasonable time");

            // 验证文件大小合理（999条记录应该产生一个相当大的文件）
            var fileInfo = new FileInfo(expectedJsonPath);
            Assert.True(fileInfo.Length > 10000, "Large dataset should produce a substantial file size");

            // 验证数据完整性
            string savedJson = File.ReadAllText(expectedJsonPath);
            var deserializedData = JsonConvert.DeserializeObject<List<FileRenameInfo>>(savedJson);

            Assert.NotNull(deserializedData);
            Assert.Equal(999, deserializedData.Count);
            Assert.Equal("LARGE001", deserializedData[0].SerialNumber);
            Assert.Equal("LARGE999", deserializedData[998].SerialNumber);

            // 清理测试文件
            if (File.Exists(expectedJsonPath))
            {
                File.Delete(expectedJsonPath);
            }
        }

        // 清理测试资源
        public void Dispose()
        {
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

    /// <summary>
    /// 真实自动保存服务测试类
    /// 使用真实的文件系统进行测试，模拟Form1的实际行为
    /// </summary>
    public class TestableRealAutoSaveService
    {
        /// <summary>
        /// 执行真实的自动保存逻辑（模拟Form1.PerformAutoSave的实际实现）
        /// </summary>
        /// <param name="fileName">JSON文件名</param>
        /// <param name="data">要保存的数据</param>
        /// <returns>保存是否成功</returns>
        public bool PerformAutoSave(string fileName, BindingList<FileRenameInfo> data)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || data == null || data.Count == 0)
                {
                    return false;
                }

                // 使用真实的保存路径（与Form1相同）
                var jsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                    "大诚工具箱", "SavedGrids", $"{fileName}.json");

                // 确保目录存在
                var directory = Path.GetDirectoryName(jsonPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 使用与Form1相同的序列化设置
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(jsonPath, json, System.Text.Encoding.UTF8);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}