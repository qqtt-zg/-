using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WindowsFormsApp3.Helpers
{
    /// <summary>
    /// 字符串操作帮助类
    /// </summary>
    public static class StringManipulationHelper
    {
        /// <summary>
        /// 格式化文件大小
        /// </summary>
        /// <param name="bytes">字节数</param>
        /// <returns>格式化的文件大小字符串</returns>
        public static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="useNumbers">是否包含数字</param>
        /// <param name="useLowercase">是否包含小写字母</param>
        /// <param name="useUppercase">是否包含大写字母</param>
        /// <param name="useSpecialChars">是否包含特殊字符</param>
        /// <returns>随机字符串</returns>
        public static string GenerateRandomString(int length, bool useNumbers = true, bool useLowercase = true,
            bool useUppercase = true, bool useSpecialChars = false)
        {
            if (length <= 0)
                return string.Empty;

            var chars = new StringBuilder();
            if (useLowercase) chars.Append("abcdefghijklmnopqrstuvwxyz");
            if (useUppercase) chars.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            if (useNumbers) chars.Append("0123456789");
            if (useSpecialChars) chars.Append("!@#$%^&*()_+-=[]{}|;:,.<>?");

            var random = new Random();
            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }

        /// <summary>
        /// 截断字符串并在末尾添加省略号
        /// </summary>
        /// <param name="text">原始文本</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="suffix">后缀</param>
        /// <returns>截断后的字符串</returns>
        public static string TruncateWithEllipsis(string text, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text ?? string.Empty;

            if (maxLength <= suffix.Length)
                return suffix.Substring(0, maxLength);

            return text.Substring(0, maxLength - suffix.Length) + suffix;
        }

        /// <summary>
        /// 移除字符串中的所有空白字符
        /// </summary>
        /// <param name="text">原始文本</param>
        /// <returns>移除空白字符后的字符串</returns>
        public static string RemoveAllWhitespace(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return Regex.Replace(text, @"\s+", "");
        }

        /// <summary>
        /// 标准化字符串中的空白字符（将连续空白替换为单个空格）
        /// </summary>
        /// <param name="text">原始文本</param>
        /// <returns>标准化后的字符串</returns>
        public static string NormalizeWhitespace(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return Regex.Replace(text.Trim(), @"\s+", " ");
        }

        /// <summary>
        /// 安全地转换字符串为整数
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>转换结果</returns>
        public static int SafeParseInt(string value, int defaultValue = 0)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            return int.TryParse(value, out int result) ? result : defaultValue;
        }

        /// <summary>
        /// 安全地转换字符串为双精度浮点数
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>转换结果</returns>
        public static double SafeParseDouble(string value, double defaultValue = 0.0)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            return double.TryParse(value, out double result) ? result : defaultValue;
        }

        /// <summary>
        /// 安全地转换字符串为布尔值
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>转换结果</returns>
        public static bool SafeParseBool(string value, bool defaultValue = false)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            var normalizedValue = value.Trim().ToLowerInvariant();

            if (normalizedValue == "true" || normalizedValue == "yes" || normalizedValue == "1" || normalizedValue == "on")
                return true;

            if (normalizedValue == "false" || normalizedValue == "no" || normalizedValue == "0" || normalizedValue == "off")
                return false;

            return bool.TryParse(value, out bool result) ? result : defaultValue;
        }
    }
}