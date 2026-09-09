using System.Drawing;
using WindowsFormsApp3.Models;
using WindowsFormsApp3.UI;

namespace WindowsFormsApp3.Tests.UI
{
    public class AntdUiThemeBridgeTests
    {
        [Theory]
        [InlineData(40, 42, 46, true)]
        [InlineData(248, 249, 250, false)]
        [InlineData(199, 237, 204, false)]
        [InlineData(225, 235, 245, false)]
        public void FromTheme_Derives_Popup_Semantics_For_All_BuiltIn_Color_Families(
            int red,
            int green,
            int blue,
            bool expectedDark)
        {
            var theme = CreateTheme(Color.FromArgb(red, green, blue));

            var tokens = PopupThemeTokens.FromTheme(theme);

            Assert.Equal(expectedDark, tokens.IsDark);
            Assert.Equal(theme.Surface, tokens.Surface);
            Assert.Equal(theme.TextPrimary, tokens.Foreground);
            Assert.Equal(theme.TextSecondary, tokens.SecondaryForeground);
            Assert.Equal(theme.Border, tokens.Border);
            Assert.Equal(theme.BackHover, tokens.Hover);
            Assert.Equal(theme.BackActive, tokens.Active);
            Assert.Equal(theme.Primary, tokens.Primary);
            Assert.Equal(theme.Error, tokens.Danger);
        }

        [Fact]
        public void FromTheme_Uses_Fallbacks_For_Custom_Themes_With_Optional_Colors_Omitted()
        {
            var theme = new ThemeDefinition
            {
                Background = Color.FromArgb(30, 30, 30),
                TextPrimary = Color.White,
                Primary = Color.MediumPurple
            };

            var tokens = PopupThemeTokens.FromTheme(theme);

            Assert.True(tokens.IsDark);
            Assert.Equal(theme.Background, tokens.Surface);
            Assert.Equal(theme.Primary, tokens.Info);
            Assert.False(tokens.DisabledForeground.IsEmpty);
            Assert.False(tokens.DisabledBackground.IsEmpty);
        }

        [Fact]
        public void Apply_Is_Idempotent_For_An_Unchanged_Theme()
        {
            var theme = CreateTheme(Color.FromArgb(248, 249, 250));

            AntdUiThemeBridge.Apply(theme);
            var firstTokens = AntdUiThemeBridge.CurrentTokens;
            AntdUiThemeBridge.Apply(theme);

            Assert.Same(firstTokens, AntdUiThemeBridge.CurrentTokens);
            Assert.Equal(AntdUI.TMode.Light, AntdUI.Config.Mode);
            Assert.Equal(theme.Primary, AntdUI.Style.Db.Primary);
        }

        private static ThemeDefinition CreateTheme(Color background)
        {
            return new ThemeDefinition
            {
                Background = background,
                Surface = Color.FromArgb(240, 241, 242),
                SurfaceLight = Color.White,
                TextPrimary = Color.FromArgb(20, 30, 40),
                TextSecondary = Color.FromArgb(80, 90, 100),
                Border = Color.FromArgb(180, 190, 200),
                Primary = Color.FromArgb(20, 100, 220),
                Success = Color.ForestGreen,
                Warning = Color.Goldenrod,
                Error = Color.Crimson,
                AccentColor1 = Color.DodgerBlue,
                BackHover = Color.FromArgb(230, 235, 240),
                BackActive = Color.FromArgb(210, 220, 230)
            };
        }
    }
}
