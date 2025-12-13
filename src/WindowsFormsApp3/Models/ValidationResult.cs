using System;
using System.Collections.Generic;

namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// 验证结果数据传输对象
    /// 用于封装文件添加到网格时的验证结果信息
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// 验证是否成功
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 验证失败时的错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 验证失败的具体错误类型
        /// </summary>
        public ValidationErrorType ErrorType { get; set; }

        /// <summary>
        /// 验证过程中的警告消息列表
        /// </summary>
        public List<string> Warnings { get; set; }

        /// <summary>
        /// 验证的上下文信息，用于调试
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ValidationResult()
        {
            IsValid = true;
            ErrorMessage = string.Empty;
            ErrorType = ValidationErrorType.None;
            Warnings = new List<string>();
            Context = string.Empty;
        }

        /// <summary>
        /// 创建成功的验证结果
        /// </summary>
        /// <param name="context">验证上下文</param>
        /// <returns>成功的验证结果</returns>
        public static ValidationResult Success(string context = "")
        {
            return new ValidationResult
            {
                IsValid = true,
                Context = context
            };
        }

        /// <summary>
        /// 创建失败的验证结果
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorType">错误类型</param>
        /// <param name="context">验证上下文</param>
        /// <returns>失败的验证结果</returns>
        public static ValidationResult Failure(string errorMessage, ValidationErrorType errorType = ValidationErrorType.General, string context = "")
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = errorMessage,
                ErrorType = errorType,
                Context = context
            };
        }

        /// <summary>
        /// 添加警告消息
        /// </summary>
        /// <param name="warning">警告消息</param>
        public void AddWarning(string warning)
        {
            if (!string.IsNullOrEmpty(warning))
            {
                Warnings.Add(warning);
            }
        }
    }

    /// <summary>
    /// 验证错误类型枚举
    /// </summary>
    public enum ValidationErrorType
    {
        /// <summary>
        /// 无错误
        /// </summary>
        None,

        /// <summary>
        /// 通用错误
        /// </summary>
        General,

        /// <summary>
        /// 未知错误
        /// </summary>
        Unknown,

        /// <summary>
        /// 文件信息无效
        /// </summary>
        InvalidFileInfo,

        /// <summary>
        /// 参数为空或无效
        /// </summary>
        InvalidParameters,

        /// <summary>
        /// 序号无效或重复
        /// </summary>
        InvalidSerialNumber,

        /// <summary>
        /// 数量格式无效
        /// </summary>
        InvalidQuantity,

        /// <summary>
        /// 材料信息缺失
        /// </summary>
        MissingMaterial,

        /// <summary>
        /// 文件已存在
        /// </summary>
        FileAlreadyExists,

        /// <summary>
        /// 空引用错误
        /// </summary>
        NullReference,

        /// <summary>
        /// 文件未找到
        /// </summary>
        FileNotFound,

        /// <summary>
        /// 目录未找到
        /// </summary>
        DirectoryNotFound,

        /// <summary>
        /// 临时文件
        /// </summary>
        TemporaryFile,

        /// <summary>
        /// 正则表达式选择无效
        /// </summary>
        InvalidRegexSelection,

        /// <summary>
        /// 导出路径无效
        /// </summary>
        InvalidExportPath,

        /// <summary>
        /// 文件路径无效
        /// </summary>
        InvalidFilePath,

        /// <summary>
        /// 文件格式无效
        /// </summary>
        InvalidFileFormat
    }
}