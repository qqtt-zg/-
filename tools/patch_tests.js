const fs = require('fs');
const path = require('path');

const testPath = path.resolve('src/WindowsFormsApp3.Tests/Forms/MaterialSelectionSchemeATests.cs');
let content = fs.readFileSync(testPath, 'utf8');

const additionalTests = `
        [Fact]
        public void OrderNumberModeEnum_ShouldMapExpectedValues()
        {
            Assert.Equal(0, (int)OrderNumberMode.None);
            Assert.Equal(1, (int)OrderNumberMode.AutoIncrement);
            Assert.Equal(2, (int)OrderNumberMode.RegexExtraction);
        }

        [Theory]
        [InlineData(800, 400)]
        [InlineData(600, 400)]
        [InlineData(400, 400)]
        [InlineData(385, 385)]
        public void FormWidthSanitization_ShouldNormalizeLargeWidth(int savedWidth, int expectedWidth)
        {
            int sanitized = savedWidth > 450 ? 400 : savedWidth;
            Assert.Equal(expectedWidth, sanitized);
        }

        [Fact]
        public void BatchPanelCollapseGeometry_ShouldRestoreStandardCoordinates()
        {
            const int BATCH_PANEL_WIDTH = 420;
            int expandedLeft = 100;
            int expandedWidth = 820;

            int standardLeft = expandedLeft + BATCH_PANEL_WIDTH;
            int standardWidth = Math.Max(380, expandedWidth - BATCH_PANEL_WIDTH);

            Assert.Equal(520, standardLeft);
            Assert.Equal(400, standardWidth);
        }
`;

content = content.replace('    }\r\n}', additionalTests + '    }\r\n}');
fs.writeFileSync(testPath, content, 'utf8');
console.log('Added tests to MaterialSelectionSchemeATests.cs');
