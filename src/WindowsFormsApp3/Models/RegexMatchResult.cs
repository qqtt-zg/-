using System;
using System.Text.RegularExpressions;

namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// 正则表达式匹配结果数据传输对象
    /// 用于封装正则表达式处理的结果信息
    /// </summary>
    public class RegexMatchResult
    {
        /// <summary>
        /// 正则匹配是否成功
        /// </summary>
        public bool IsMatch { get; set; }

        /// <summary>
        /// 匹配的结果文本
        /// </summary>
        public string MatchedText { get; set; }

        /// <summary>
        /// 使用的正则表达式模式
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// 原始输入文本
        /// </summary>
        public string InputText { get; set; }

        /// <summary>
        /// 正则表达式处理过程中的错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 匹配的捕获组数量
        /// </summary>
        public int CaptureGroupCount { get; set; }

        /// <summary>
        /// 是否使用了捕获组的结果
        /// </summary>
        public bool UsedCaptureGroup { get; set; }

        /// <summary>
        /// 原始Match对象（用于需要详细匹配信息的场景）
        /// </summary>
        public Match OriginalMatch { get; set; }

        /// <summary>
        /// 正则表达式名称（来自配置）
        /// </summary>
        public string PatternName { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RegexMatchResult()
        {
            IsMatch = false;
            MatchedText = string.Empty;
            Pattern = string.Empty;
            InputText = string.Empty;
            ErrorMessage = string.Empty;
            CaptureGroupCount = 0;
            UsedCaptureGroup = false;
            PatternName = string.Empty;
        }

        /// <summary>
        /// 创建成功匹配的结果
        /// </summary>
        /// <param name="inputText">输入文本</param>
        /// <param name="pattern">正则模式</param>
        /// <param name="match">匹配对象</param>
        /// <param name="patternName">模式名称</param>
        /// <returns>匹配结果</returns>
        public static RegexMatchResult Success(string inputText, string pattern, Match match, string patternName = "")
        {
            string matchedText = string.Empty;
            bool usedCaptureGroup = false;

            // 根据捕获组数量决定使用哪个结果
            if (match.Groups.Count > 1)
            {
                // 有捕获组，使用第一个捕获组的内容
                matchedText = match.Groups[1].Value.Trim();
                usedCaptureGroup = true;
            }
            else
            {
                // 没有捕获组，使用整个匹配结果
                matchedText = match.Value.Trim();
            }

            return new RegexMatchResult
            {
                IsMatch = true,
                MatchedText = matchedText,
                Pattern = pattern,
                InputText = inputText,
                CaptureGroupCount = match.Groups.Count,
                UsedCaptureGroup = usedCaptureGroup,
                OriginalMatch = match,
                PatternName = patternName
            };
        }

        /// <summary>
        /// 创建匹配失败的结果
        /// </summary>
        /// <param name="inputText">输入文本</param>
        /// <param name="pattern">正则模式</param>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="patternName">模式名称</param>
        /// <returns>失败的匹配结果</returns>
        public static RegexMatchResult Failure(string inputText, string pattern, string errorMessage = "", string patternName = "")
        {
            return new RegexMatchResult
            {
                IsMatch = false,
                InputText = inputText,
                Pattern = pattern,
                ErrorMessage = string.IsNullOrEmpty(errorMessage) ? "正则表达式匹配失败" : errorMessage,
                PatternName = patternName
            };
        }

        /// <summary>
        /// 创建正则表达式异常的结果
        /// </summary>
        /// <param name="inputText">输入文本</param>
        /// <param name="pattern">正则模式</param>
        /// <param name="exception">异常信息</param>
        /// <param name="patternName">模式名称</param>
        /// <returns>异常的匹配结果</returns>
        public static RegexMatchResult Exception(string inputText, string pattern, Exception exception, string patternName = "")
        {
            return new RegexMatchResult
            {
                IsMatch = false,
                InputText = inputText,
                Pattern = pattern,
                ErrorMessage = $"正则表达式处理异常: {exception.Message}",
                PatternName = patternName
            };
        }

        /// <summary>
        /// 获取用于调试的详细信息
        /// </summary>
        /// <returns>调试信息字符串</returns>
        public string GetDebugInfo()
        {
            return $"Pattern: '{PatternName}' ({Pattern}), Input: '{InputText}', Match: {IsMatch}, Result: '{MatchedText}', CaptureGroups: {CaptureGroupCount}";
        }
    }
}