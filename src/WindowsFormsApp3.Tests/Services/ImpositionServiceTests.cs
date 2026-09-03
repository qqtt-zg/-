using System.Threading.Tasks;
using WindowsFormsApp3.Models;
using WindowsFormsApp3.Services;
using Xunit;

namespace WindowsFormsApp3.Tests.Services
{
    public class ImpositionServiceTests
    {
        [Fact]
        public async Task CalculateFlatSheetLayoutAsync_ShouldFail_WhenRequestedLayoutDoesNotFit()
        {
            var config = new FlatSheetConfiguration
            {
                PaperWidth = 220,
                PaperHeight = 120,
                MarginTop = 10,
                MarginBottom = 10,
                MarginLeft = 10,
                MarginRight = 10,
                Rows = 2,
                Columns = 2
            };

            ImpositionResult result = await new ImpositionService().CalculateFlatSheetLayoutAsync(config, CreatePdfInfo(100, 100));

            Assert.False(result.Success);
            Assert.Contains("无法容纳", result.ErrorMessage);
        }

        [Fact]
        public async Task CalculateRollMaterialLayoutAsync_ShouldUseMaximumOfRequestedRowsAndMinimumLengthRows()
        {
            var config = new RollMaterialConfiguration
            {
                FixedWidth = 220,
                MinLength = 350,
                MarginTop = 10,
                MarginBottom = 10,
                MarginLeft = 10,
                MarginRight = 10,
                Rows = 2,
                Columns = 2
            };

            ImpositionResult result = await new ImpositionService().CalculateRollMaterialLayoutAsync(
                config, CreatePdfInfo(100, 100), rotationMode: RollRotationMode.Force0Degree);

            Assert.True(result.Success);
            Assert.Equal(4, result.Rows);
            Assert.Equal(2, result.Columns);
            Assert.Equal(420f, result.ActualMaterialLength.GetValueOrDefault());
        }

        [Fact]
        public async Task CalculateRollMaterialLayoutAsync_ShouldFail_WhenRequestedColumnsExceedFixedWidth()
        {
            var config = new RollMaterialConfiguration
            {
                FixedWidth = 220,
                MinLength = 100,
                MarginLeft = 10,
                MarginRight = 10,
                Columns = 3
            };

            ImpositionResult result = await new ImpositionService().CalculateRollMaterialLayoutAsync(
                config, CreatePdfInfo(100, 100), rotationMode: RollRotationMode.Force0Degree);

            Assert.False(result.Success);
            Assert.Contains("固定宽度", result.ErrorMessage);
        }

        private static ImpositionPdfInfo CreatePdfInfo(float width, float height)
        {
            return new ImpositionPdfInfo
            {
                CropBoxWidth = width,
                CropBoxHeight = height,
                HasCropBox = true
            };
        }
    }
}
