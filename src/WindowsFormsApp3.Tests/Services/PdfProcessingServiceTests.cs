using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using WindowsFormsApp3.Services;
using WindowsFormsApp3.Models;
using System.Reflection;

namespace WindowsFormsApp3.Tests.Services
{
    public class PdfProcessingServiceTests : IDisposable
    {
        private readonly WindowsFormsApp3.Services.IPdfProcessingService _pdfService;
        private readonly string _testDirectory;
        private readonly string _testPdfPath;

        public PdfProcessingServiceTests()
        {
            // 创建测试目录
            _testDirectory = Path.Combine(Path.GetTempPath(), "PdfProcessingTests_", DateTime.Now.Ticks.ToString());
            Directory.CreateDirectory(_testDirectory);

            // 创建一个空的PDF文件模拟
            _testPdfPath = CreateEmptyTestFile("test.pdf");

            // 创建PdfProcessingService实例
            _pdfService = new PdfProcessingService();
        }

        [Fact]
        public void AddLayerToPdf_Should_Return_False_When_File_Not_Exists()
        {
            // 准备不存在的文件路径
            string nonExistentFilePath = Path.Combine(_testDirectory, "non_existent.pdf");
            var layerInfo = new PdfLayerInfo { LayerName = "TestLayer", Content = "TestContent", X = 10, Y = 10, FontSize = 12 };

            // 执行方法并验证结果
            bool result = _pdfService.AddLayerToPdf(nonExistentFilePath, nonExistentFilePath, layerInfo);
            Assert.False(result);
        }

        [Fact]
        public void AddLayerToPdf_Should_Return_False_When_FilePath_Is_Null_Or_Empty()
        {
            var layerInfo = new PdfLayerInfo { LayerName = "TestLayer", Content = "TestContent", X = 10, Y = 10, FontSize = 12 };

            // 执行方法并验证结果
            Assert.False(_pdfService.AddLayerToPdf(null, null, layerInfo));
            Assert.False(_pdfService.AddLayerToPdf(string.Empty, string.Empty, layerInfo));
        }

        [Fact]
        public void AddLayerToPdf_Should_Return_False_When_LayerInfo_Is_Null()
        {
            // 创建一个简单的PDF文件用于测试
            string testFilePath = CreateEmptyTestFile("test.pdf");

            try
            {
                // 执行方法并验证结果
                bool result = _pdfService.AddLayerToPdf(testFilePath, testFilePath, null);
                Assert.False(result);
            }
            finally
            {
                // 清理测试文件
                if (File.Exists(testFilePath))
                {
                    File.Delete(testFilePath);
                }
            }
        }

        [Fact]
        public void AddLayerToPdf_Should_Return_False_When_PdfFile_Is_Invalid()
        {
            // 准备测试数据
            string invalidPdfPath = CreateEmptyTestFile("invalid.pdf");
            var layerInfo = new PdfLayerInfo
            {
                LayerName = "TestLayer",
                Content = "TestContent",
                X = 10,
                Y = 10,
                FontSize = 12
            };

            // 执行方法
            bool result = _pdfService.AddLayerToPdf(invalidPdfPath, invalidPdfPath, layerInfo);
            
            // 验证结果
            Assert.False(result);
        }

        [Fact]
        public void AddLayerToPdf_Should_Handle_Valid_Input_Without_Crashing()
        {
            // 准备测试数据
            var layerInfo = new PdfLayerInfo
            {
                LayerName = "TestLayer",
                Content = "TestContent",
                X = 10,
                Y = 10,
                FontSize = 12
            };

            // 执行方法（这里我们不期望成功，只是验证它不会崩溃）
            bool result = _pdfService.AddLayerToPdf(_testPdfPath, _testPdfPath, layerInfo);
            
            // 验证文件仍然存在
            Assert.True(File.Exists(_testPdfPath));
        }

        [Fact]
        public void MergePdfFiles_Should_Return_False_When_SourceFiles_Is_Null_Or_Empty()
        {
            string outputFile = Path.Combine(_testDirectory, "merged.pdf");

            // 执行方法并验证结果
            Assert.False(_pdfService.MergePdfFiles(null, outputFile));
            Assert.False(_pdfService.MergePdfFiles(new List<string>(), outputFile));
        }

        [Fact]
        public void MergePdfFiles_Should_Return_False_When_OutputFile_Is_Null_Or_Empty()
        {
            var sourceFiles = new List<string> { CreateEmptyTestFile("test1.pdf") };

            try
            {
                // 执行方法并验证结果
                Assert.False(_pdfService.MergePdfFiles(sourceFiles, null));
                Assert.False(_pdfService.MergePdfFiles(sourceFiles, string.Empty));
            }
            finally
            {
                // 清理测试文件
                foreach (string file in sourceFiles)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        [Fact]
        public void MergePdfFiles_Should_Handle_Valid_Input_Without_Crashing()
        {
            // 准备测试数据
            string testFile1 = CreateEmptyTestFile("test1.pdf");
            string testFile2 = CreateEmptyTestFile("test2.pdf");
            List<string> sourceFiles = new List<string> { testFile1, testFile2 };
            string outputFile = Path.Combine(_testDirectory, "merged.pdf");

            try
            {
                // 执行方法
                bool result = _pdfService.MergePdfFiles(sourceFiles, outputFile);
                
                // 验证文件仍然存在
                Assert.True(File.Exists(testFile1));
                Assert.True(File.Exists(testFile2));
            }
            finally
            {
                // 清理额外的测试文件
                try
                {
                    if (File.Exists(testFile1)) File.Delete(testFile1);
                    if (File.Exists(testFile2)) File.Delete(testFile2);
                    if (File.Exists(outputFile)) File.Delete(outputFile);
                }
                catch (IOException) { }
            }
        }

        [Fact]
        public void MergePdfFiles_Should_Return_False_When_OutputDirectory_DoesNotExist_AndCannotBeCreated()
        {
            // 准备测试数据 - 使用无效的输出路径格式
            string testFile1 = CreateEmptyTestFile("test1.pdf");
            List<string> sourceFiles = new List<string> { testFile1 };
            
            // 使用无效的目录路径
            string invalidOutputPath = "C:\\invalid_path_that_cannot_be_created\\merged.pdf";

            try
            {
                // 执行方法
                bool result = _pdfService.MergePdfFiles(sourceFiles, invalidOutputPath);
                
                // 验证结果
                Assert.False(result);
            }
            finally
            {
                // 清理测试文件
                if (File.Exists(testFile1)) File.Delete(testFile1);
            }
        }

        [Fact]
        public void GetPdfPageCount_Should_Return_Zero_When_File_Not_Exists()
        {
            // 准备不存在的文件路径
            string nonExistentFilePath = Path.Combine(_testDirectory, "non_existent.pdf");

            // 执行方法并验证结果
            int pageCount = _pdfService.GetPdfPageCount(nonExistentFilePath);
            Assert.Equal(0, pageCount);
        }

        [Fact]
        public void GetPdfPageCount_Should_Return_Zero_When_File_Is_Not_Pdf()
        {
            // 创建一个非PDF文件用于测试
            string nonPdfFilePath = CreateEmptyTestFile("test.txt");

            try
            {
                // 执行方法并验证结果
                int pageCount = _pdfService.GetPdfPageCount(nonPdfFilePath);
                Assert.Equal(0, pageCount);
            }
            finally
            {
                // 清理测试文件
                if (File.Exists(nonPdfFilePath))
                {
                    File.Delete(nonPdfFilePath);
                }
            }
        }

        [Fact]
        public void GetPdfPageCount_Should_Return_Zero_When_Exception_Occurs()
        {
            // 创建一个无效的PDF内容文件
            string invalidFilePath = Path.Combine(_testDirectory, "invalid_pdf.pdf");
            File.WriteAllText(invalidFilePath, "This is not a valid PDF content");

            try
            {
                // 执行方法
                int pageCount = _pdfService.GetPdfPageCount(invalidFilePath);
                
                // 验证结果
                Assert.Equal(0, pageCount);
            }
            finally
            {
                // 清理测试文件
                if (File.Exists(invalidFilePath)) File.Delete(invalidFilePath);
            }
        }

        // 辅助方法：创建空的测试文件
        private string CreateEmptyTestFile(string fileName)
        {
            string filePath = Path.Combine(_testDirectory, fileName);
            File.Create(filePath).Dispose();
            return filePath;
        }

        // 清理测试资源
        public void Dispose()
        {
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