using System;
using System.IO;
using Xunit;
using WindowsFormsApp3.Services;

namespace WindowsFormsApp3.Tests.Utils
{
    public class PdfCutPathExtractorTests
    {
        private readonly string _testDirectory;

        public PdfCutPathExtractorTests()
        {
            _testDirectory = Path.Combine(Path.GetTempPath(), "PdfCutPathExtractorTests_" + DateTime.Now.Ticks);
            Directory.CreateDirectory(_testDirectory);
        }

        public void Dispose()
        {
            try { if (Directory.Exists(_testDirectory)) Directory.Delete(_testDirectory, true); } catch { }
        }

        [Fact]
        public void CanExtractCutPathFromLastPage_NullFile_ReturnsFalse()
        {
            bool result = PdfCutPathExtractor.CanExtractCutPathFromLastPage(null);
            Assert.False(result);
        }

        [Fact]
        public void CanExtractCutPathFromLastPage_EmptyString_ReturnsFalse()
        {
            bool result = PdfCutPathExtractor.CanExtractCutPathFromLastPage("");
            Assert.False(result);
        }

        [Fact]
        public void CanExtractCutPathFromLastPage_NonExistentFile_ReturnsFalse()
        {
            bool result = PdfCutPathExtractor.CanExtractCutPathFromLastPage(Path.Combine(_testDirectory, "nonexistent.pdf"));
            Assert.False(result);
        }

        [Fact]
        public void CanExtractCutPathFromLastPage_NonPdfFile_ReturnsFalse()
        {
            string txtFile = Path.Combine(_testDirectory, "test.txt");
            File.WriteAllText(txtFile, "not a pdf");
            bool result = PdfCutPathExtractor.CanExtractCutPathFromLastPage(txtFile);
            Assert.False(result);
        }

        [Fact]
        public void ExtractAndConvertCutPath_NullPage_ReturnsNull()
        {
            float[] bounds;
            byte[] result = PdfCutPathExtractor.ExtractAndConvertCutPath(null, null, out bounds);
            Assert.Null(result);
        }

        [Fact]
        public void ConvertLineCoords_SimpleLine_ReturnsSameLine()
        {
            string result = PdfCutPathExtractor.ConvertLineCoords("0 0 m", 0, 0);
            Assert.Equal("0 0 m", result);
        }

        [Fact]
        public void ConvertLineCoords_WithCmOffset_AddsOffset()
        {
            string result = PdfCutPathExtractor.ConvertLineCoords("100 200 m", 10.5f, -5.3f);
            Assert.Equal("110.5 194.7 m", result);
        }

        [Fact]
        public void ConvertLineCoords_HOperator_ReturnsUnchanged()
        {
            string result = PdfCutPathExtractor.ConvertLineCoords("h", 10, 20);
            Assert.Equal("h", result);
        }

        [Fact]
        public void UpdateBounds_SinglePoint_UpdatesCorrectly()
        {
            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
            PdfCutPathExtractor.UpdateBounds("100 200 m", ref minX, ref minY, ref maxX, ref maxY);
            Assert.Equal(100f, minX);
            Assert.Equal(200f, minY);
            Assert.Equal(100f, maxX);
            Assert.Equal(200f, maxY);
        }

        [Fact]
        public void UpdateBounds_MultiplePoints_UpdatesBounds()
        {
            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
            PdfCutPathExtractor.UpdateBounds("10 20 m", ref minX, ref minY, ref maxX, ref maxY);
            PdfCutPathExtractor.UpdateBounds("100 200 l", ref minX, ref minY, ref maxX, ref maxY);
            Assert.Equal(10f, minX);
            Assert.Equal(20f, minY);
            Assert.Equal(100f, maxX);
            Assert.Equal(200f, maxY);
        }

        [Fact]
        public void UpdateBounds_HOperator_DoesNotUpdate()
        {
            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
            PdfCutPathExtractor.UpdateBounds("h", ref minX, ref minY, ref maxX, ref maxY);
            Assert.Equal(float.MaxValue, minX);
        }
    }
}
