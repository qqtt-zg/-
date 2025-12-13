using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Xunit;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using WindowsFormsApp3;

namespace WindowsFormsApp3.Tests.UIAutomation
{
    public class MainWindowTests : IDisposable
    {
        private Application _app;
        private UIA3Automation _automation;
        private Window _mainWindow;
        private readonly string _testDir;

        public MainWindowTests()
        {
            // 创建测试目录
            _testDir = Path.Combine(Path.GetTempPath(), "MainWindowTests_" + DateTime.Now.Ticks);
            Directory.CreateDirectory(_testDir);

            // 启动应用程序
            var appPath = Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\WindowsFormsApp3\bin\Debug\大诚重命名工具.exe");
            if (!File.Exists(appPath))
            {
                appPath = Path.Combine(Environment.CurrentDirectory, "大诚重命名工具.exe");
                if (!File.Exists(appPath))
                {
                    throw new FileNotFoundException("无法找到应用程序可执行文件", appPath);
                }
            }

            // 初始化自动化
            _automation = new UIA3Automation();
            _app = Application.Launch(appPath);
            Thread.Sleep(2000); // 等待应用启动

            // 获取主窗口
            _mainWindow = _app.GetMainWindow(_automation);
        }

        public void Dispose()
        {
            // 关闭应用程序
            _app?.Close();
            _app?.Dispose();
            _automation?.Dispose();

            // 清理测试目录
            if (Directory.Exists(_testDir))
            {
                try
                {
                    Directory.Delete(_testDir, true);
                }
                catch { }
            }
        }

        [Fact(Skip = "需要先构建应用程序")]
        public void MainWindow_Should_Open_And_Display_Title()
        {
            // 验证窗口标题
            Assert.Equal("大诚工具箱", _mainWindow.Title);
            Assert.True(_mainWindow.IsEnabled);
            Assert.False(_mainWindow.IsOffscreen); // 在FlaUI中，IsOffscreen为false表示窗口可见
        }

        [Fact(Skip = "需要先构建应用程序")]
        public void MainWindow_Should_Have_Main_Controls()
        {
            // 验证主要控件存在
            var btnImmediateRename = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnImmediateRename")) as Button;
            var btnStopImmediateRename = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnStopImmediateRename")) as Button;
            var btnBatchProcess = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnBatchProcess")) as Button;

            Assert.NotNull(btnImmediateRename);
            Assert.NotNull(btnStopImmediateRename);
            Assert.NotNull(btnBatchProcess);
        }
    }
}