namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// 重命名验证结果数据传输对象
    /// 用于封装文件重命名操作的验证结果和相关信息
    /// </summary>
    public class RenameValidationResult
    {
        /// <summary>
        /// 验证是否通过
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 错误类型
        /// </summary>
        public ValidationErrorType ErrorType { get; set; }

        /// <summary>
        /// 是否为临时文件
        /// </summary>
        public bool IsTemporaryFile { get; set; }

        /// <summary>
        /// 正则表达式选择是否有效
        /// </summary>
        public bool IsRegexSelectionValid { get; set; }

        /// <summary>
        /// 导出路径是否有效
        /// </summary>
        public bool IsExportPathValid { get; set; }

        /// <summary>
        /// 正则表达式模式名称
        /// </summary>
        public string PatternName { get; set; }

        /// <summary>
        /// 正则表达式模式
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// 验证的详细信息
        /// </summary>
        public string ValidationDetails { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RenameValidationResult()
        {
            IsValid = false;
            ErrorMessage = string.Empty;
            ErrorType = ValidationErrorType.Unknown;
            IsTemporaryFile = false;
            IsRegexSelectionValid = false;
            IsExportPathValid = false;
            PatternName = string.Empty;
            Pattern = string.Empty;
            ValidationDetails = string.Empty;
        }

        /// <summary>
        /// 创建成功的验证结果
        /// </summary>
        /// <param name="patternName">正则表达式模式名称</param>
        /// <param name="pattern">正则表达式模式</param>
        /// <param name="details">验证详细信息</param>
        /// <returns>成功的验证结果</returns>
        public static RenameValidationResult Success(string patternName, string pattern, string details = "")
        {
            return new RenameValidationResult
            {
                IsValid = true,
                ErrorMessage = string.Empty,
                ErrorType = ValidationErrorType.None,
                IsTemporaryFile = false,
                IsRegexSelectionValid = true,
                IsExportPathValid = true,
                PatternName = patternName ?? string.Empty,
                Pattern = pattern ?? string.Empty,
                ValidationDetails = details ?? "重命名验证通过"
            };
        }

        /// <summary>
        /// 创建失败的验证结果
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorType">错误类型</param>
        /// <param name="details">详细信息</param>
        /// <returns>失败的验证结果</returns>
        public static RenameValidationResult Failure(string errorMessage, ValidationErrorType errorType, string details = "")
        {
            return new RenameValidationResult
            {
                IsValid = false,
                ErrorMessage = errorMessage ?? "未知错误",
                ErrorType = errorType,
                IsTemporaryFile = false,
                IsRegexSelectionValid = false,
                IsExportPathValid = false,
                PatternName = string.Empty,
                Pattern = string.Empty,
                ValidationDetails = details ?? errorMessage ?? "验证失败"
            };
        }

        /// <summary>
        /// 创建临时文件的验证结果
        /// </summary>
        /// <param name="details">详细信息</param>
        /// <returns>临时文件验证结果</returns>
        public static RenameValidationResult TemporaryFile(string details = "")
        {
            return new RenameValidationResult
            {
                IsValid = false,
                ErrorMessage = "跳过临时文件处理",
                ErrorType = ValidationErrorType.TemporaryFile,
                IsTemporaryFile = true,
                IsRegexSelectionValid = false,
                IsExportPathValid = false,
                PatternName = string.Empty,
                Pattern = string.Empty,
                ValidationDetails = details ?? "检测到临时文件，跳过处理"
            };
        }

        /// <summary>
        /// 创建正则表达式选择无效的验证结果
        /// </summary>
        /// <param name="details">详细信息</param>
        /// <returns>正则选择无效验证结果</returns>
        public static RenameValidationResult InvalidRegexSelection(string details = "")
        {
            return new RenameValidationResult
            {
                IsValid = false,
                ErrorMessage = "请先选择正则表达式",
                ErrorType = ValidationErrorType.InvalidRegexSelection,
                IsTemporaryFile = false,
                IsRegexSelectionValid = false,
                IsExportPathValid = false,
                PatternName = string.Empty,
                Pattern = string.Empty,
                ValidationDetails = details ?? "未选择有效的正则表达式"
            };
        }

        /// <summary>
        /// 创建导出路径无效的验证结果
        /// </summary>
        /// <param name="exportPath">导出路径</param>
        /// <param name="details">详细信息</param>
        /// <returns>导出路径无效验证结果</returns>
        public static RenameValidationResult InvalidExportPath(string exportPath, string details = "")
        {
            return new RenameValidationResult
            {
                IsValid = false,
                ErrorMessage = "请选择有效的导出目录",
                ErrorType = ValidationErrorType.InvalidExportPath,
                IsTemporaryFile = false,
                IsRegexSelectionValid = true,
                IsExportPathValid = false,
                PatternName = string.Empty,
                Pattern = string.Empty,
                ValidationDetails = details ?? $"导出路径无效: {exportPath}"
            };
        }

        /// <summary>
        /// 获取调试信息
        /// </summary>
        /// <returns>调试信息字符串</returns>
        public string GetDebugInfo()
        {
            return $"RenameValidation: Valid={IsValid}, Error='{ErrorMessage}', Type={ErrorType}, " +
                   $"TempFile={IsTemporaryFile}, RegexValid={IsRegexSelectionValid}, " +
                   $"PathValid={IsExportPathValid}, Pattern='{PatternName}:{Pattern}', Details='{ValidationDetails}'";
        }
    }
}