using System;
using System.IO;
using Xunit;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Tests.Utils
{
    /// <summary>
    /// 简单的PDF迁移测试，不依赖应用程序设置
    /// </summary>
    public class SimplePdfMigrationTests
    {
        [Fact]
        public void SetAllPageBoxesToCropBox_Should_Handle_Non_Existent_File()
        {
            // Arrange
            string nonExistentFile = @"C:\temp\non_existent.pdf";

            // Act
            bool result = PdfTools.SetAllPageBoxesToCropBox(nonExistentFile);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SetAllPageBoxesToCropBox_Should_Handle_Non_PDF_File()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFile, "This is not a PDF file");

                // Act
                bool result = PdfTools.SetAllPageBoxesToCropBox(tempFile);

                // Assert
                Assert.False(result);
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        [Fact]
        public void CheckPdfLayersExist_Should_Handle_Non_Existent_File()
        {
            // Arrange
            string nonExistentFile = @"C:\temp\non_existent.pdf";

            // Act
            bool result = PdfTools.CheckPdfLayersExist(nonExistentFile, "TestLayer");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CheckPdfLayersExist_Should_Handle_Non_PDF_File()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFile, "This is not a PDF file");

                // Act
                bool result = PdfTools.CheckPdfLayersExist(tempFile, "TestLayer");

                // Assert
                Assert.False(result);
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        [Fact]
        public void CalculateFinalDimensions_Basic_Should_Return_Correct_Format()
        {
            // Arrange
            double width = 100.0;
            double height = 200.0;
            double tetBleed = 5.0;

            // Act
            string result = PdfTools.CalculateFinalDimensions(width, height, tetBleed);

            // Assert
            Assert.Equal("90x190", result); // (100-5*2) x (200-5*2)
        }

        [Fact]
        public void CalculateFinalDimensions_With_Zero_Bleed_Should_Return_Original_Dimensions()
        {
            // Arrange
            double width = 100.0;
            double height = 200.0;
            double tetBleed = 0.0;

            // Act
            string result = PdfTools.CalculateFinalDimensions(width, height, tetBleed);

            // Assert
            Assert.Equal("100x200", result); // No change when tetBleed is 0
        }
    }
}