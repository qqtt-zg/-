using System;
using System.Collections.Generic;
using System.IO;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// 外部SVG图标库 - 从外部文件或资源加载SVG图标
    /// </summary>
    public static class ExternalSvgIconLibrary
    {
        /// <summary>
        /// 加载颜色模式图标
        /// </summary>
        /// <param name="iconName">图标名称</param>
        /// <returns>SVG路径字符串</returns>
        public static string LoadColorMode(string iconName)
        {
            switch (iconName.ToLower())
            {
                case "rainbow":
                return SvgIconLibrary.Rainbow;
                case "sun":
                    return SvgIconLibrary.Sun;
                case "black-circle":
                    return SvgIconLibrary.BlackCircle;
                case "sparkle":
                    return SvgIconLibrary.Sparkle;
                default:
                    return SvgIconLibrary.Rainbow; // 默认返回彩虹图标
            }
        }

        /// <summary>
        /// 从文件加载SVG内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>SVG内容</returns>
        public static string LoadFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load SVG from {filePath}: {ex.Message}");
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取可用的颜色模式列表
        /// </summary>
        /// <returns>颜色模式列表</returns>
        public static List<string> GetAvailableColorModes()
        {
            return new List<string>
            {
                "rainbow",
                "sun",
                "black-circle",
                "sparkle"
            };
        }

        /// <summary>
        /// 验证SVG内容是否有效
        /// </summary>
        /// <param name="svgContent">SVG内容</param>
        /// <returns>是否有效</returns>
        public static bool IsValidSvg(string svgContent)
        {
            if (string.IsNullOrEmpty(svgContent))
                return false;

            return svgContent.Contains("<svg") &&
                   svgContent.Contains("</svg>") &&
                   (svgContent.Contains("viewBox") || svgContent.Contains("width="));
        }
    }
}