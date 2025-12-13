using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Xunit;
using Moq;
using WindowsFormsApp3;
using WindowsFormsApp3.Presenters;

namespace WindowsFormsApp3.Tests.UIAutomation
{
    /// <summary>
    /// FormClosing事件与自动保存功能的UI级别测试
    /// 测试窗口关闭时的自动保存行为
    /// </summary>
    public class FormClosingAutoSaveTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly Mock<IForm1View> _mockView;
        private readonly Form1Presenter _presenter;
        private readonly TestFormClosingHandler _formClosingHandler;

        public FormClosingAutoSaveTests()
        {
            // 创建测试目录
            _testDirectory = Path.Combine(Path.GetTempPath(), "FormClosingAutoSaveTests_", DateTime.Now.Ticks.ToString());
            Directory.CreateDirectory(_testDirectory);

            // 创建模拟的视图
            _mockView = new Mock<IForm1View>();

            // ConfigService已移除，不需要模拟

            // 创建Presenter实例
            _presenter = new Form1Presenter(_mockView.Object);

            // 创建FormClosing处理器
            _formClosingHandler = new TestFormClosingHandler();
        }

        [Fact]
        public void FormClosing_UserClosing_Should_Trigger_AutoSave()
        {
            // Arrange
            var formClosingEventArgs = new FormClosingEventArgs(CloseReason.UserClosing, false);
            bool autoSaveCalled = false;

            _mockView.Setup(v => v.PerformAutoSave()).Callback(() => autoSaveCalled = true);

            // Act
            _presenter.HandleFormClosing(formClosingEventArgs);

            // Assert
            Assert.True(autoSaveCalled, "PerformAutoSave should be called when user closes the form");
            _mockView.Verify(v => v.PerformAutoSave(), Times.Once);
        }

        [Fact]
        public void FormClosing_WindowsShutDown_Should_Trigger_AutoSave()
        {
            // Arrange
            var formClosingEventArgs = new FormClosingEventArgs(CloseReason.WindowsShutDown, false);
            bool autoSaveCalled = false;

            _mockView.Setup(v => v.PerformAutoSave()).Callback(() => autoSaveCalled = true);

            // Act
            _presenter.HandleFormClosing(formClosingEventArgs);

            // Assert
            Assert.True(autoSaveCalled, "PerformAutoSave should be called when Windows is shutting down");
            _mockView.Verify(v => v.PerformAutoSave(), Times.Once);
        }

        [Fact]
        public void FormClosing_ApplicationExit_Should_Trigger_AutoSave()
        {
            // Arrange
            var formClosingEventArgs = new FormClosingEventArgs(CloseReason.ApplicationExitCall, false);
            bool autoSaveCalled = false;

            _mockView.Setup(v => v.PerformAutoSave()).Callback(() => autoSaveCalled = true);

            // Act
            _presenter.HandleFormClosing(formClosingEventArgs);

            // Assert
            Assert.True(autoSaveCalled, "PerformAutoSave should be called when application exits");
            _mockView.Verify(v => v.PerformAutoSave(), Times.Once);
        }

        [Fact]
        public void FormClosing_Should_Handle_AutoSave_Exception_Gracefully()
        {
            // Arrange
            var formClosingEventArgs = new FormClosingEventArgs(CloseReason.UserClosing, false);
            
            _mockView.Setup(v => v.PerformAutoSave()).Throws(new IOException("Disk full"));
            _mockView.Setup(v => v.ShowMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButtons>(), It.IsAny<MessageBoxIcon>()));

            // Act & Assert (should not throw exception)
            _presenter.HandleFormClosing(formClosingEventArgs);

            // Verify error message was shown
            _mockView.Verify(v => v.ShowMessage(It.Is<string>(s => s.Contains("保存设置失败")), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error), Times.Once);
        }

        [Fact]
        public void FormClosing_Multiple_Calls_Should_Handle_Gracefully()
        {
            // Arrange
            var formClosingEventArgs1 = new FormClosingEventArgs(CloseReason.UserClosing, false);
            var formClosingEventArgs2 = new FormClosingEventArgs(CloseReason.UserClosing, false);
            int autoSaveCallCount = 0;

            _mockView.Setup(v => v.PerformAutoSave()).Callback(() => autoSaveCallCount++);

            // Act
            _presenter.HandleFormClosing(formClosingEventArgs1);
            _presenter.HandleFormClosing(formClosingEventArgs2);

            // Assert
            Assert.Equal(2, autoSaveCallCount);
            _mockView.Verify(v => v.PerformAutoSave(), Times.Exactly(2));
        }

        [Fact]
        public void TestFormClosingEvent_Should_Trigger_Event_Properly()
        {
            // Arrange
            bool eventTriggered = false;
            FormClosingEventArgs capturedArgs = null;

            _formClosingHandler.FormClosing += (sender, e) =>
            {
                eventTriggered = true;
                capturedArgs = e;
            };

            // Act
            _formClosingHandler.TestFormClosingEvent();

            // Assert
            Assert.True(eventTriggered, "FormClosing event should be triggered");
            Assert.NotNull(capturedArgs);
            Assert.Equal(CloseReason.UserClosing, capturedArgs.CloseReason);
            Assert.False(capturedArgs.Cancel);
        }

        [Fact]
        public void FormClosing_With_Cancel_Should_Still_Trigger_AutoSave()
        {
            // Arrange
            var formClosingEventArgs = new FormClosingEventArgs(CloseReason.UserClosing, true); // Cancel = true
            bool autoSaveCalled = false;

            _mockView.Setup(v => v.PerformAutoSave()).Callback(() => autoSaveCalled = true);

            // Act
            _presenter.HandleFormClosing(formClosingEventArgs);

            // Assert
            Assert.True(autoSaveCalled, "PerformAutoSave should be called even when form closing is cancelled");
            _mockView.Verify(v => v.PerformAutoSave(), Times.Once);
        }

        [Fact]
        public void FormClosing_Performance_Test()
        {
            // Arrange
            var formClosingEventArgs = new FormClosingEventArgs(CloseReason.UserClosing, false);
            _mockView.Setup(v => v.PerformAutoSave());

            // Act
            var startTime = DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                _presenter.HandleFormClosing(formClosingEventArgs);
            }
            var endTime = DateTime.Now;
            var duration = endTime - startTime;

            // Assert
            Assert.True(duration.TotalMilliseconds < 1000, "100 form closing events should complete within 1 second");
            _mockView.Verify(v => v.PerformAutoSave(), Times.Exactly(100));
        }

        [Fact]
        public void FormClosing_Should_Preserve_Event_Args_Properties()
        {
            // Arrange
            var originalEventArgs = new FormClosingEventArgs(CloseReason.TaskManagerClosing, false);
            FormClosingEventArgs processedEventArgs = null;

            // 模拟事件处理过程中对事件参数的访问
            _mockView.Setup(v => v.PerformAutoSave()).Callback(() => {
                // 在这里，事件参数应该保持原始状态
                processedEventArgs = originalEventArgs;
            });

            // Act
            _presenter.HandleFormClosing(originalEventArgs);

            // Assert
            Assert.NotNull(processedEventArgs);
            Assert.Equal(CloseReason.TaskManagerClosing, processedEventArgs.CloseReason);
            Assert.False(processedEventArgs.Cancel);
        }

        [Fact]
        public void AutoSave_Integration_With_Real_FormClosing_Scenario()
        {
            // Arrange
            string testFileName = $"formclosing_test_{DateTime.Now.Ticks}";
            string expectedJsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                "大诚工具箱", "SavedGrids", $"{testFileName}.json");

            var testData = new BindingList<FileRenameInfo>
            {
                new FileRenameInfo
                {
                    SerialNumber = "FC001",
                    OriginalName = "formclosing_test.pdf",
                    NewName = "formclosing_renamed.pdf",
                    Process = "FormClosing integration test",
                    Time = DateTime.Now.ToString("MM-dd")
                }
            };

            // 创建一个真实的自动保存行为模拟
            _mockView.Setup(v => v.PerformAutoSave()).Callback(() =>
            {
                // 模拟真实的PerformAutoSave行为
                var realAutoSaveService = new TestableRealAutoSaveService();
                realAutoSaveService.PerformAutoSave(testFileName, testData);
            });

            var formClosingEventArgs = new FormClosingEventArgs(CloseReason.UserClosing, false);

            // Act
            _presenter.HandleFormClosing(formClosingEventArgs);

            // Assert
            _mockView.Verify(v => v.PerformAutoSave(), Times.Once);
            
            // 验证文件确实被保存了
            Assert.True(File.Exists(expectedJsonPath), "JSON file should be created during form closing");

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
    /// 测试用的FormClosing事件处理器
    /// 模拟Form1中的TestFormClosingEvent方法
    /// </summary>
    public class TestFormClosingHandler
    {
        public event FormClosingEventHandler FormClosing;

        /// <summary>
        /// 手动测试FormClosing事件（模拟Form1.TestFormClosingEvent方法）
        /// </summary>
        public void TestFormClosingEvent()
        {
            // 创建测试事件参数
            var testEventArgs = new FormClosingEventArgs(CloseReason.UserClosing, false);
            
            // 手动触发FormClosing事件
            FormClosing?.Invoke(this, testEventArgs);
        }
    }

    /// <summary>
    /// 真实自动保存服务测试类（重用集成测试中的类）
    /// </summary>
    public class TestableRealAutoSaveService
    {
        public bool PerformAutoSave(string fileName, BindingList<FileRenameInfo> data)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || data == null || data.Count == 0)
                {
                    return false;
                }

                var jsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                    "大诚工具箱", "SavedGrids", $"{fileName}.json");

                var directory = Path.GetDirectoryName(jsonPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
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