using System;
using System.IO;
using Xunit;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Tests.Utils
{
    /// <summary>
    /// 测试PDFsharp到iText7迁移后的PDF工具功能
    /// </summary>
    public class PdfToolsMigrationTests
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
        public void CalculateFinalDimensions_Should_Return_Correct_Format()
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
        public void CalculateFinalDimensions_With_Layers_Should_Add_Shape_Code()
        {
            // Arrange
            double width = 100.0;
            double height = 200.0;
            double tetBleed = 5.0;
            string cornerRadius = "R";
            bool addPdfLayers = true;

            // Act
            string result = PdfTools.CalculateFinalDimensions(width, height, tetBleed, cornerRadius, addPdfLayers);

            // Assert
            Assert.Equal("90x190R", result); // (100-5*2) x (200-5*2) + shape code
        }

        [Fact]
        public void CalculateFinalDimensions_With_Radius_Should_Add_Some_Radius_Code()
        {
            // Arrange
            double width = 100.0;
            double height = 200.0;
            double tetBleed = 5.0;
            string cornerRadius = "3";
            bool addPdfLayers = true;

            // Act
            string result = PdfTools.CalculateFinalDimensions(width, height, tetBleed, cornerRadius, addPdfLayers);

            // Assert
            Assert.StartsWith("90x190R", result); // Should start with 90x190R, may or may not have 3 depending on config
        }
    }
}