using System;
using System.IO;
using Xunit;
using WindowsFormsApp3.Utils;
using WindowsFormsApp3;

namespace WindowsFormsApp3.Tests.Utils
{
    public class PdfToolsTests : IDisposable
    {
        private readonly string _testDirectory;

        public PdfToolsTests()
        {
            // 创建测试目录
            _testDirectory = Path.Combine(Path.GetTempPath(), "PdfToolsTests_", DateTime.Now.Ticks.ToString());
            Directory.CreateDirectory(_testDirectory);
        }

        [Fact]
        public void IText7PdfTools_GetPageCount_ShouldReturnNull_WhenFileDoesNotExist()
        {
            // Arrange
            string nonExistentFile = Path.Combine(_testDirectory, "non_existent.pdf");

            // Act
            int? result = IText7PdfTools.GetPageCount(nonExistentFile);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void IText7PdfTools_GetPageCount_ShouldReturnNull_WhenFileIsNotPdf()
        {
            // Arrange
            string nonPdfFile = CreateEmptyTestFile("test.txt");

            // Act
            int? result = IText7PdfTools.GetPageCount(nonPdfFile);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CheckPdfLayersExist_ShouldReturnFalse_WhenFileDoesNotExist()
        {
            // Arrange
            string nonExistentFile = Path.Combine(_testDirectory, "non_existent.pdf");

            // Act
            bool result = PdfTools.CheckPdfLayersExist(nonExistentFile, "AnyLayer");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CheckPdfLayersExist_ShouldReturnFalse_WhenFileIsNotPdf()
        {
            // Arrange
            string nonPdfFile = CreateEmptyTestFile("test.txt");

            // Act
            bool result = PdfTools.CheckPdfLayersExist(nonPdfFile, "AnyLayer");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IText7PdfTools_GetFirstPageSize_ShouldReturnFalse_WhenFileDoesNotExist()
        {
            // Arrange
            string nonExistentFile = Path.Combine(_testDirectory, "non_existent.pdf");
            double width = 0;
            double height = 0;

            // Act
            bool result = IText7PdfTools.GetFirstPageSize(nonExistentFile, out width, out height);

            // Assert
            Assert.False(result);
            Assert.Equal(0, width);
            Assert.Equal(0, height);
        }

        [Fact]
        public void IText7PdfTools_GetFirstPageSize_ShouldReturnFalse_WhenFileIsNotPdf()
        {
            // Arrange
            string nonPdfFile = CreateEmptyTestFile("test.txt");
            double width = 0;
            double height = 0;

            // Act
            bool result = IText7PdfTools.GetFirstPageSize(nonPdfFile, out width, out height);

            // Assert
            Assert.False(result);
            Assert.Equal(0, width);
            Assert.Equal(0, height);
        }

        [Fact]
        public void AddDotsAddCounterLayer_ShouldReturnFalse_WhenFileDoesNotExist()
        {
            // Arrange
            string nonExistentFile = Path.Combine(_testDirectory, "non_existent.pdf");

            // Act
            bool result = PdfTools.AddDotsAddCounterLayer(nonExistentFile, "100x150", ShapeType.RightAngle, 0);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddDotsAddCounterLayer_ShouldReturnFalse_WhenFileIsNotPdf()
        {
            // Arrange
            string nonPdfFile = CreateEmptyTestFile("test.txt");

            // Act
            bool result = PdfTools.AddDotsAddCounterLayer(nonPdfFile, "100x150", ShapeType.RightAngle, 0);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddDotsAddCounterLayer_ShouldHandleInvalidNumericParameters()
        {
            // Arrange
            string testFile = CreateEmptyTestFile("test.pdf");

            // Act & Assert - 验证方法不会崩溃
            bool result = PdfTools.AddDotsAddCounterLayer(testFile, "invalid", ShapeType.RightAngle, 0);
            
            // 验证文件仍然存在
            Assert.True(File.Exists(testFile));
        }

        [Fact]
        public void ProcessSpecialShapePdf_ShouldReturnFalse_WhenFileDoesNotExist()
        {
            // Arrange
            string nonExistentFile = Path.Combine(_testDirectory, "non_existent.pdf");

            // Act
            bool result = PdfTools.ProcessSpecialShapePdf(nonExistentFile);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ProcessSpecialShapePdf_ShouldReturnFalse_WhenFileIsNotPdf()
        {
            // Arrange
            string nonPdfFile = CreateEmptyTestFile("test.txt");

            // Act
            bool result = PdfTools.ProcessSpecialShapePdf(nonPdfFile);

            // Assert
            Assert.False(result);
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
                catch { }
            }
        }
    }
}