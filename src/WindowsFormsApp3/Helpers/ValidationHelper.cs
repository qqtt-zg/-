using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsFormsApp3.Exceptions;

namespace WindowsFormsApp3.Helpers
{
    /// <summary>
    /// 验证帮助类
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// 验证正则表达式
        /// </summary>
        /// <param name="pattern">正则表达式模式</param>
        /// <returns>验证结果</returns>
        public static ValidationResult ValidateRegex(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return new ValidationResult(false, "正则表达式不能为空");

            try
            {
                new Regex(pattern);
                return new ValidationResult(true, "正则表达式有效");
            }
            catch (ArgumentException ex)
            {
                return new ValidationResult(false, $"正则表达式格式无效: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证数字字符串
        /// </summary>
        /// <param name="value">要验证的值</param>
        /// <param name="allowDecimal">是否允许小数</param>
        /// <param name="allowNegative">是否允许负数</param>
        /// <returns>验证结果</returns>
        public static ValidationResult ValidateNumber(string value, bool allowDecimal = false, bool allowNegative = false)
        {
            if (string.IsNullOrEmpty(value))
                return new ValidationResult(false, "数值不能为空");

            var pattern = allowDecimal ? @"^-?\d*\.?\d+$" : @"^-?\d+$";
            if (!allowNegative)
            {
                pattern = pattern.Replace("-?", "");
            }

            if (!Regex.IsMatch(value, pattern))
            {
                var message = allowDecimal
                    ? (allowNegative ? "请输入有效的数字（可包含小数和负号）" : "请输入有效的正数（可包含小数）")
                    : (allowNegative ? "请输入有效的整数（可包含负号）" : "请输入有效的正整数");

                return new ValidationResult(false, message);
            }

            if (allowDecimal && value.ToCharArray().Count(c => c == '.') > 1)
            {
                return new ValidationResult(false, "小数点只能出现一次");
            }

            return new ValidationResult(true, "数值格式正确");
        }

        /// <summary>
        /// 验证电子邮件地址
        /// </summary>
        /// <param name="email">电子邮件地址</param>
        /// <returns>验证结果</returns>
        public static ValidationResult ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return new ValidationResult(false, "电子邮件地址不能为空");

            if (email.Length > 254)
                return new ValidationResult(false, "电子邮件地址长度不能超过254个字符");

            var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, pattern))
            {
                return new ValidationResult(false, "电子邮件地址格式无效");
            }

            return new ValidationResult(true, "电子邮件地址格式正确");
        }

        /// <summary>
        /// 验证路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="checkExists">是否检查路径是否存在</param>
        /// <param name="isDirectory">是否为目录路径</param>
        /// <returns>验证结果</returns>
        public static ValidationResult ValidatePath(string path, bool checkExists = true, bool isDirectory = false)
        {
            if (string.IsNullOrEmpty(path))
                return new ValidationResult(false, "路径不能为空");

            try
            {
                var fullPath = Path.GetFullPath(path);

                if (fullPath.Length > 260)
                {
                    return new ValidationResult(false, "路径长度不能超过260个字符");
                }

                if (checkExists)
                {
                    if (isDirectory)
                    {
                        if (!Directory.Exists(fullPath))
                            return new ValidationResult(false, "目录不存在");
                    }
                    else
                    {
                        if (!File.Exists(fullPath))
                            return new ValidationResult(false, "文件不存在");
                    }
                }

                return new ValidationResult(true, isDirectory ? "目录路径有效" : "文件路径有效");
            }
            catch (ArgumentException ex)
            {
                return new ValidationResult(false, $"路径格式无效: {ex.Message}");
            }
            catch (PathTooLongException ex)
            {
                return new ValidationResult(false, $"路径过长: {ex.Message}");
            }
            catch (NotSupportedException ex)
            {
                return new ValidationResult(false, $"路径格式不支持: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证字符串长度
        /// </summary>
        /// <param name="value">要验证的值</param>
        /// <param name="minLength">最小长度</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns>验证结果</returns>
        public static ValidationResult ValidateStringLength(string value, int minLength = 0, int maxLength = int.MaxValue, string fieldName = "字段")
        {
            if (value == null)
                value = string.Empty;

            if (value.Length < minLength)
            {
                return new ValidationResult(false, $"{fieldName}长度不能少于{minLength}个字符");
            }

            if (value.Length > maxLength)
            {
                return new ValidationResult(false, $"{fieldName}长度不能超过{maxLength}个字符");
            }

            return new ValidationResult(true, $"{fieldName}长度符合要求");
        }

        /// <summary>
        /// 验证日期范围
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="minDate">最小日期</param>
        /// <param name="maxDate">最大日期</param>
        /// <returns>验证结果</returns>
        public static ValidationResult ValidateDateRange(DateTime date, DateTime? minDate = null, DateTime? maxDate = null)
        {
            if (minDate.HasValue && date < minDate.Value)
            {
                return new ValidationResult(false, $"日期不能早于{minDate.Value:yyyy-MM-dd}");
            }

            if (maxDate.HasValue && date > maxDate.Value)
            {
                return new ValidationResult(false, $"日期不能晚于{maxDate.Value:yyyy-MM-dd}");
            }

            return new ValidationResult(true, "日期在有效范围内");
        }

        /// <summary>
        /// 验证集合不为空
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="collectionName">集合名称</param>
        /// <returns>验证结果</returns>
        public static ValidationResult ValidateCollectionNotEmpty<T>(IEnumerable<T> collection, string collectionName = "集合")
        {
            if (collection == null)
                return new ValidationResult(false, $"{collectionName}不能为null");

            if (!collection.GetEnumerator().MoveNext())
                return new ValidationResult(false, $"{collectionName}不能为空");

            return new ValidationResult(true, $"{collectionName}包含元素");
        }

        /// <summary>
        /// 验证对象不为null
        /// </summary>
        /// <param name="obj">要验证的对象</param>
        /// <param name="parameterName">参数名称</param>
        /// <exception cref="ValidationException">验证失败时抛出</exception>
        public static void ValidateNotNull(object obj, string parameterName)
        {
            if (obj == null)
            {
                throw new ValidationException(parameterName, null, $"{parameterName}不能为null");
            }
        }

        /// <summary>
        /// 验证字符串不为空或空白
        /// </summary>
        /// <param name="value">要验证的字符串</param>
        /// <param name="parameterName">参数名称</param>
        /// <exception cref="ValidationException">验证失败时抛出</exception>
        public static void ValidateNotNullOrWhiteSpace(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ValidationException(parameterName, value, $"{parameterName}不能为空或空白");
            }
        }

        /// <summary>
        /// 验证条件是否为真
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="message">错误消息</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="parameterValue">参数值</param>
        /// <exception cref="ValidationException">验证失败时抛出</exception>
        public static void ValidateCondition(bool condition, string message, string parameterName = null, object parameterValue = null)
        {
            if (!condition)
            {
                throw new ValidationException(parameterName, parameterValue, message);
            }
        }

        /// <summary>
        /// 验证并确保文件扩展名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="expectedExtension">期望的扩展名（包含点号）</param>
        /// <param name="autoAdd">是否自动添加扩展名</param>
        /// <returns>验证后的文件名</returns>
        /// <exception cref="ValidationException">验证失败时抛出</exception>
        public static string ValidateAndEnsureFileExtension(string fileName, string expectedExtension, bool autoAdd = true)
        {
            ValidateNotNullOrWhiteSpace(fileName, nameof(fileName));
            ValidateNotNullOrWhiteSpace(expectedExtension, nameof(expectedExtension));

            if (!expectedExtension.StartsWith("."))
            {
                expectedExtension = "." + expectedExtension;
            }

            var currentExtension = Path.GetExtension(fileName);

            if (string.IsNullOrEmpty(currentExtension))
            {
                if (autoAdd)
                {
                    return fileName + expectedExtension;
                }
                else
                {
                    throw new ValidationException(nameof(fileName), fileName, $"文件名必须包含扩展名: {expectedExtension}");
                }
            }

            if (!currentExtension.Equals(expectedExtension, StringComparison.OrdinalIgnoreCase))
            {
                if (autoAdd)
                {
                    return Path.ChangeExtension(fileName, expectedExtension);
                }
                else
                {
                    throw new ValidationException(nameof(fileName), fileName, $"文件扩展名必须是: {expectedExtension}，当前是: {currentExtension}");
                }
            }

            return fileName;
        }

        /// <summary>
        /// 批量验证
        /// </summary>
        /// <param name="validations">验证函数列表</param>
        /// <returns>批量验证结果</returns>
        public static BatchValidationResult ValidateBatch(params Func<ValidationResult>[] validations)
        {
            var result = new BatchValidationResult();

            foreach (var validation in validations)
            {
                try
                {
                    var validationResult = validation();
                    if (!validationResult.IsValid)
                    {
                        result.AddError(validationResult.Message);
                    }
                }
                catch (Exception ex)
                {
                    result.AddError($"验证过程中发生错误: {ex.Message}");
                }
            }

            return result;
        }
    }

    /// <summary>
    /// 验证结果
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; }
        public string Message { get; }

        public ValidationResult(bool isValid, string message)
        {
            IsValid = isValid;
            Message = message;
        }

        public static ValidationResult Success => new ValidationResult(true, "验证通过");
        public static ValidationResult Failure(string message) => new ValidationResult(false, message);
    }

    /// <summary>
    /// 批量验证结果
    /// </summary>
    public class BatchValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors { get; } = new List<string>();

        public void AddError(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                Errors.Add(error);
            }
        }

        public string GetErrorMessage()
        {
            return IsValid ? "验证通过" : string.Join("; ", Errors);
        }
    }
}