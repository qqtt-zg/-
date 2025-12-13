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

namespace WindowsFormsApp3.Tests.Services
{
    /// <summary>
    /// 自动保存功能专门单元测试
    /// 测试 PerformAutoSave 方法和相关的JSON文件保存逻辑
    /// </summary>
    public class AutoSaveTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly string _testJsonDirectory;
        private readonly Mock<ComboBox> _mockCmbJsonFiles;
        private readonly Mock<DataGridView> _mockDgvFiles;
        private readonly BindingList<FileRenameInfo> _testFileList;

        public AutoSaveTests()
        {
            // 创建测试目录
            _testDirectory = Path.Combine(Path.GetTempPath(), "AutoSaveTests_", DateTime.Now.Ticks.ToString());
            _testJsonDirectory = Path.Combine(_testDirectory, "SavedGrids");
            Directory.CreateDirectory(_testDirectory);
            Directory.CreateDirectory(_testJsonDirectory);

            // 创建模拟控件
            _mockCmbJsonFiles = new Mock<ComboBox>();
            _mockDgvFiles = new Mock<DataGridView>();

            // 创建测试数据
            _testFileList = new BindingList<FileRenameInfo>
            {
                new FileRenameInfo
                {
                    SerialNumber = "001",
                    OriginalName = "test1.pdf",
                    NewName = "renamed1.pdf",
                    FullPath = @"C:\test\test1.pdf",
                    Material = "Steel",
                    Quantity = "10",
                    Dimensions = "100x200",
                    Process = "Test file 1",
                    Time = DateTime.Now.ToString("MM-dd")
                },
                new FileRenameInfo
                {
                    SerialNumber = "002",
                    OriginalName = "test2.pdf",
                    NewName = "renamed2.pdf",
                    FullPath = @"C:\test\test2.pdf",
                    Material = "Aluminum",
                    Quantity = "5",
                    Dimensions = "50x100",
                    Process = "Test file 2",
                    Time = DateTime.Now.ToString("MM-dd")
                }
            };
        }

        [Fact]
        public void PerformAutoSave_Should_Save_Json_When_ValidData_And_SelectedFile()
        {
            // Arrange
            string testFileName = "test_config";
            string expectedJsonPath = Path.Combine(_testJsonDirectory, $"{testFileName}.json");
            
            var autoSaveService = new TestableAutoSaveService(_testJsonDirectory);
            
            // Act
            bool result = autoSaveService.PerformAutoSave(testFileName, _testFileList);
            
            // Assert
            Assert.True(result);
            Assert.True(File.Exists(expectedJsonPath));
            
            // 验证保存的JSON内容
            string savedJson = File.ReadAllText(expectedJsonPath);
            var deserializedData = JsonConvert.DeserializeObject<List<FileRenameInfo>>(savedJson);
            
            Assert.NotNull(deserializedData);
            Assert.Equal(2, deserializedData.Count);
            Assert.Equal("001", deserializedData[0].SerialNumber);
            Assert.Equal("test1.pdf", deserializedData[0].OriginalName);
            Assert.Equal("Steel", deserializedData[0].Material);
        }

        [Fact]
        public void PerformAutoSave_Should_Return_False_When_FileName_Is_Null()
        {
            // Arrange
            var autoSaveService = new TestableAutoSaveService(_testJsonDirectory);
            
            // Act
            bool result = autoSaveService.PerformAutoSave(null, _testFileList);
            
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void PerformAutoSave_Should_Return_False_When_FileName_Is_Empty()
        {
            // Arrange
            var autoSaveService = new TestableAutoSaveService(_testJsonDirectory);
            
            // Act
            bool result = autoSaveService.PerformAutoSave("", _testFileList);
            
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void PerformAutoSave_Should_Return_False_When_DataList_Is_Null()
        {
            // Arrange
            var autoSaveService = new TestableAutoSaveService(_testJsonDirectory);
            
            // Act
            bool result = autoSaveService.PerformAutoSave("test_config", null);
            
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void PerformAutoSave_Should_Return_False_When_DataList_Is_Empty()
        {
            // Arrange
            var autoSaveService = new TestableAutoSaveService(_testJsonDirectory);
            var emptyList = new BindingList<FileRenameInfo>();
            
            // Act
            bool result = autoSaveService.PerformAutoSave("test_config", emptyList);
            
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void PerformAutoSave_Should_Handle_Json_Serialization_Error()
        {
            // Arrange
            var autoSaveService = new TestableAutoSaveService(_testJsonDirectory);
            
            // 使用不存在的目录来模拟写入失败（无法创建目录）
            var invalidPath = "Z:\\NonExistent\\Path\\That\\Cannot\\Be\\Created";
            var invalidAutoSaveService = new TestableAutoSaveService(invalidPath);
            
            // Act & Assert
            // 在无效路径中尝试保存应该返回false
            bool result = invalidAutoSaveService.PerformAutoSave("test_config", _testFileList);
            Assert.False(result);
        }

        [Fact]
        public void PerformAutoSave_Should_Create_Directory_If_Not_Exists()
        {
            // Arrange
            string nonExistentDir = Path.Combine(_testDirectory, "new_directory");
            var autoSaveService = new TestableAutoSaveService(nonExistentDir);
            
            // Act
            bool result = autoSaveService.PerformAutoSave("test_config", _testFileList);
            
            // Assert
            Assert.True(result);
            Assert.True(Directory.Exists(nonExistentDir));
            Assert.True(File.Exists(Path.Combine(nonExistentDir, "test_config.json")));
        }

        [Fact]
        public void PerformAutoSave_Should_Use_UTF8_Encoding()
        {
            // Arrange
            string testFileName = "utf8_test";
            string expectedJsonPath = Path.Combine(_testJsonDirectory, $"{testFileName}.json");
            
            // 添加包含中文字符的测试数据
            _testFileList[0].Process = "测试文件工艺 - UTF8编码";
            _testFileList[1].Material = "不锈钢材料";
            
            var autoSaveService = new TestableAutoSaveService(_testJsonDirectory);
            
            // Act
            bool result = autoSaveService.PerformAutoSave(testFileName, _testFileList);
            
            // Assert
            Assert.True(result);
            
            // 验证文件使用UTF8编码保存
            byte[] fileBytes = File.ReadAllBytes(expectedJsonPath);
            string fileContent = System.Text.Encoding.UTF8.GetString(fileBytes);
            
            Assert.Contains("测试文件工艺 - UTF8编码", fileContent);
            Assert.Contains("不锈钢材料", fileContent);
        }

        [Fact]
        public void PerformAutoSave_Should_Use_Indented_Json_Format()
        {
            // Arrange
            string testFileName = "indented_test";
            string expectedJsonPath = Path.Combine(_testJsonDirectory, $"{testFileName}.json");
            
            var autoSaveService = new TestableAutoSaveService(_testJsonDirectory);
            
            // Act
            bool result = autoSaveService.PerformAutoSave(testFileName, _testFileList);
            
            // Assert
            Assert.True(result);
            
            // 验证JSON格式是缩进的（包含换行符和空格）
            string savedJson = File.ReadAllText(expectedJsonPath);
            Assert.Contains("\n", savedJson); // 包含换行符
            Assert.Contains("  ", savedJson); // 包含缩进空格
            
            // 验证JSON结构正确
            var deserializedData = JsonConvert.DeserializeObject<List<FileRenameInfo>>(savedJson);
            Assert.NotNull(deserializedData);
            Assert.Equal(2, deserializedData.Count);
        }

        [Fact]
        public void PerformAutoSave_Should_Overwrite_Existing_File()
        {
            // Arrange
            string testFileName = "overwrite_test";
            string expectedJsonPath = Path.Combine(_testJsonDirectory, $"{testFileName}.json");
            
            // 先创建一个现有文件
            File.WriteAllText(expectedJsonPath, "existing content");
            Assert.True(File.Exists(expectedJsonPath));
            
            var autoSaveService = new TestableAutoSaveService(_testJsonDirectory);
            
            // Act
            bool result = autoSaveService.PerformAutoSave(testFileName, _testFileList);
            
            // Assert
            Assert.True(result);
            
            // 验证文件被正确覆写
            string savedJson = File.ReadAllText(expectedJsonPath);
            Assert.DoesNotContain("existing content", savedJson);
            
            var deserializedData = JsonConvert.DeserializeObject<List<FileRenameInfo>>(savedJson);
            Assert.NotNull(deserializedData);
            Assert.Equal(2, deserializedData.Count);
        }

        [Fact]
        public void PerformAutoSave_Should_Handle_Special_Characters_In_FileName()
        {
            // Arrange
            // 使用包含特殊字符但合法的文件名
            string testFileName = "test_file_2025-09-03_14-30-00";
            string expectedJsonPath = Path.Combine(_testJsonDirectory, $"{testFileName}.json");
            
            var autoSaveService = new TestableAutoSaveService(_testJsonDirectory);
            
            // Act
            bool result = autoSaveService.PerformAutoSave(testFileName, _testFileList);
            
            // Assert
            Assert.True(result);
            Assert.True(File.Exists(expectedJsonPath));
        }

        [Fact]
        public void PerformAutoSave_Should_Preserve_All_FileRenameInfo_Properties()
        {
            // Arrange
            string testFileName = "complete_properties_test";
            string expectedJsonPath = Path.Combine(_testJsonDirectory, $"{testFileName}.json");
            
            // 创建包含所有属性的测试数据
            var completeFileInfo = new FileRenameInfo
            {
                SerialNumber = "999",
                OriginalName = "complete.pdf",
                NewName = "complete_renamed.pdf",
                FullPath = @"C:\complete\complete.pdf",
                RegexResult = "regex_match",
                OrderNumber = "ORDER-999",
                Material = "Carbon Steel",
                Quantity = "25",
                Dimensions = "200x300x50",
                Process = "Complete test file with all properties",
                Time = "09-03",
                Status = "Processed",
                ErrorMessage = "",
                PageCount = 15
            };
            
            var completeList = new BindingList<FileRenameInfo> { completeFileInfo };
            var autoSaveService = new TestableAutoSaveService(_testJsonDirectory);
            
            // Act
            bool result = autoSaveService.PerformAutoSave(testFileName, completeList);
            
            // Assert
            Assert.True(result);
            
            // 验证所有属性都被正确保存
            string savedJson = File.ReadAllText(expectedJsonPath);
            var deserializedData = JsonConvert.DeserializeObject<List<FileRenameInfo>>(savedJson);
            
            Assert.NotNull(deserializedData);
            Assert.Single(deserializedData);
            
            var savedFileInfo = deserializedData[0];
            Assert.Equal("999", savedFileInfo.SerialNumber);
            Assert.Equal("complete.pdf", savedFileInfo.OriginalName);
            Assert.Equal("complete_renamed.pdf", savedFileInfo.NewName);
            Assert.Equal(@"C:\complete\complete.pdf", savedFileInfo.FullPath);
            Assert.Equal("regex_match", savedFileInfo.RegexResult);
            Assert.Equal("ORDER-999", savedFileInfo.OrderNumber);
            Assert.Equal("Carbon Steel", savedFileInfo.Material);
            Assert.Equal("25", savedFileInfo.Quantity);
            Assert.Equal("200x300x50", savedFileInfo.Dimensions);
            Assert.Equal("Complete test file with all properties", savedFileInfo.Process);
            Assert.Equal("09-03", savedFileInfo.Time);
            Assert.Equal("Processed", savedFileInfo.Status);
            Assert.Equal("", savedFileInfo.ErrorMessage);
            Assert.Equal(15, savedFileInfo.PageCount);
        }

        // 清理测试资源
        public void Dispose()
        {
            if (Directory.Exists(_testDirectory))
            {
                try
                {
                    // 先移除只读属性
                    foreach (var file in Directory.GetFiles(_testDirectory, "*", SearchOption.AllDirectories))
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                    }
                    foreach (var dir in Directory.GetDirectories(_testDirectory, "*", SearchOption.AllDirectories))
                    {
                        var dirInfo = new DirectoryInfo(dir);
                        dirInfo.Attributes = FileAttributes.Normal;
                    }
                    var testDirInfo = new DirectoryInfo(_testDirectory);
                    testDirInfo.Attributes = FileAttributes.Normal;
                    
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
    /// 可测试的自动保存服务类
    /// 封装了PerformAutoSave的核心逻辑，便于单元测试
    /// </summary>
    public class TestableAutoSaveService
    {
        private readonly string _saveDirectory;

        public TestableAutoSaveService(string saveDirectory)
        {
            _saveDirectory = saveDirectory ?? throw new ArgumentNullException(nameof(saveDirectory));
        }

        /// <summary>
        /// 执行自动保存逻辑（模拟Form1.PerformAutoSave方法的核心功能）
        /// </summary>
        /// <param name="fileName">JSON文件名（不含扩展名）</param>
        /// <param name="data">要保存的文件重命名信息列表</param>
        /// <returns>保存是否成功</returns>
        public bool PerformAutoSave(string fileName, BindingList<FileRenameInfo> data)
        {
            try
            {
                // 验证输入参数
                if (string.IsNullOrEmpty(fileName) || data == null || data.Count == 0)
                {
                    return false;
                }

                // 确保保存目录存在
                if (!Directory.Exists(_saveDirectory))
                {
                    Directory.CreateDirectory(_saveDirectory);
                }

                // 构建JSON文件路径
                string jsonPath = Path.Combine(_saveDirectory, $"{fileName}.json");

                // 序列化数据为JSON格式（使用缩进格式提高可读性）
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);

                // 使用UTF-8编码保存文件
                File.WriteAllText(jsonPath, json, System.Text.Encoding.UTF8);

                return true;
            }
            catch (Exception)
            {
                // 保存失败时返回false
                return false;
            }
        }
    }
}