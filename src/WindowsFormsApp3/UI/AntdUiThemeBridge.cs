using System;
using System.Drawing;
using AntdUI;
using WindowsFormsApp3.Models;

namespace WindowsFormsApp3.UI
{
    /// <summary>
    /// 将应用主题映射到 AntdUI 的全局语义颜色。
    /// 该桥接仅在应用主题切换时调用，菜单和弹框渲染时只读取 CurrentTokens，
    /// 避免在每次显示浮层时重复修改 AntdUI 全局状态。
    /// </summary>
    public static class AntdUiThemeBridge
    {
        private static readonly object SyncRoot = new object();
        private static string _appliedFingerprint;
        private static PopupThemeTokens _currentTokens;

        /// <summary>
        /// 当前主题对应的浮层语义颜色。主题尚未初始化时返回 null。
        /// </summary>
        public static PopupThemeTokens CurrentTokens
        {
            get
            {
                lock (SyncRoot)
                {
                    return _currentTokens;
                }
            }
        }

        /// <summary>
        /// 应用主题到 AntdUI 全局配色。传入相同主题时不会重复写入全局状态。
        /// </summary>
        public static void Apply(ThemeDefinition theme)
        {
            if (theme == null)
            {
                return;
            }

            var tokens = PopupThemeTokens.FromTheme(theme);
            var fingerprint = tokens.CreateFingerprint();

            lock (SyncRoot)
            {
                if (string.Equals(_appliedFingerprint, fingerprint, StringComparison.Ordinal))
                {
                    return;
                }

                Config.Mode = tokens.IsDark ? TMode.Dark : TMode.Light;
                Style.SetPrimary(tokens.Primary);
                Style.SetSuccess(tokens.Success);
                Style.SetWarning(tokens.Warning);
                Style.SetError(tokens.Danger);
                Style.SetInfo(tokens.Info);
                Style.Set(Colour.BgBase, tokens.Surface);
                Style.Set(Colour.BgContainer, tokens.Surface);
                Style.Set(Colour.BgElevated, tokens.Surface);
                Style.Set(Colour.BgLayout, tokens.Surface);
                Style.Set(Colour.TextBase, tokens.Foreground);
                Style.Set(Colour.Text, tokens.Foreground);
                Style.Set(Colour.TextSecondary, tokens.SecondaryForeground);
                Style.Set(Colour.TextTertiary, tokens.DisabledForeground);
                Style.Set(Colour.TextQuaternary, tokens.DisabledForeground);
                Style.Set(Colour.BorderColor, tokens.Border);
                Style.Set(Colour.BorderSecondary, tokens.Border);
                Style.Set(Colour.BorderColorDisable, tokens.DisabledBackground);
                Style.Set(Colour.HoverBg, tokens.Hover);
                Style.Set(Colour.HoverColor, tokens.Active);
                Style.Set(Colour.Fill, tokens.Active);
                Style.Set(Colour.FillSecondary, tokens.Hover);
                Style.Set(Colour.FillTertiary, tokens.DisabledBackground);
                Style.Set(Colour.ErrorColor, tokens.Danger);

                // 这些是 AntdUI 浮层共享的视觉基线，而不是单个菜单或 Modal 的配置。
                Config.ShadowEnabled = true;
                Config.ShadowSize = 8;
                Config.ShadowOpacity = 0.18F;

                _currentTokens = tokens;
                _appliedFingerprint = fingerprint;
            }
        }
    }

    /// <summary>
    /// 右键菜单和短确认弹框共用的语义颜色。
    /// </summary>
    public sealed class PopupThemeTokens
    {
        private PopupThemeTokens()
        {
        }

        public bool IsDark { get; private set; }
        public Color Surface { get; private set; }
        public Color Foreground { get; private set; }
        public Color SecondaryForeground { get; private set; }
        public Color Border { get; private set; }
        public Color Hover { get; private set; }
        public Color Active { get; private set; }
        public Color Primary { get; private set; }
        public Color Success { get; private set; }
        public Color Warning { get; private set; }
        public Color Danger { get; private set; }
        public Color Info { get; private set; }
        public Color DisabledBackground { get; private set; }
        public Color DisabledForeground { get; private set; }

        /// <summary>
        /// 从 ThemeDefinition 推导所有浮层颜色，因此内置四套主题和自定义主题遵循同一规则。
        /// </summary>
        public static PopupThemeTokens FromTheme(ThemeDefinition theme)
        {
            if (theme == null)
            {
                throw new ArgumentNullException(nameof(theme));
            }

            var surface = FirstUsable(theme.Surface, theme.SurfaceLight, theme.Background, SystemColors.Window);
            var foreground = FirstUsable(theme.TextPrimary, SystemColors.WindowText);
            var secondary = FirstUsable(theme.TextSecondary, Blend(foreground, surface, 0.55F));
            var border = FirstUsable(theme.Border, Blend(foreground, surface, 0.15F));
            var primary = FirstUsable(theme.Primary, theme.AccentColor1, foreground);
            var success = FirstUsable(theme.Success, theme.AccentColor2, primary);
            var warning = FirstUsable(theme.Warning, theme.AccentColor3, primary);
            var danger = FirstUsable(theme.Error, theme.AccentColor4, warning);
            var info = FirstUsable(theme.AccentColor1, primary);

            return new PopupThemeTokens
            {
                IsDark = IsDarkBackground(theme.Background),
                Surface = surface,
                Foreground = foreground,
                SecondaryForeground = secondary,
                Border = border,
                Hover = FirstUsable(theme.BackHover, Blend(surface, primary, 0.08F)),
                Active = FirstUsable(theme.BackActive, Blend(surface, primary, 0.14F)),
                Primary = primary,
                Success = success,
                Warning = warning,
                Danger = danger,
                Info = info,
                DisabledBackground = Blend(surface, theme.Background, 0.35F),
                DisabledForeground = Blend(secondary, surface, 0.45F)
            };
        }

        internal string CreateFingerprint()
        {
            return string.Join("|", new[]
            {
                IsDark.ToString(), ToArgb(Surface), ToArgb(Foreground), ToArgb(SecondaryForeground),
                ToArgb(Border), ToArgb(Hover), ToArgb(Active), ToArgb(Primary), ToArgb(Success),
                ToArgb(Warning), ToArgb(Danger), ToArgb(Info), ToArgb(DisabledBackground), ToArgb(DisabledForeground)
            });
        }

        private static bool IsDarkBackground(Color color)
        {
            return color.GetBrightness() < 0.5F;
        }

        private static Color FirstUsable(params Color[] colors)
        {
            foreach (var color in colors)
            {
                if (!color.IsEmpty)
                {
                    return color;
                }
            }

            return SystemColors.Window;
        }

        private static Color Blend(Color foreground, Color background, float foregroundWeight)
        {
            var weight = Math.Max(0F, Math.Min(1F, foregroundWeight));
            var backgroundWeight = 1F - weight;
            return Color.FromArgb(
                (int)(foreground.R * weight + background.R * backgroundWeight),
                (int)(foreground.G * weight + background.G * backgroundWeight),
                (int)(foreground.B * weight + background.B * backgroundWeight));
        }

        private static string ToArgb(Color color)
        {
            return color.ToArgb().ToString();
        }
    }
}
