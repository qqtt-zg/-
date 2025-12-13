using System;

namespace WindowsFormsApp3.Exceptions
{
    /// <summary>
    /// 文件重命名异常类
    /// </summary>
    public class FileRenameException : Exception
    {
        public string FilePath { get; }
        public string TargetPath { get; }

        public FileRenameException() : base("文件重命名操作失败")
        {
        }

        public FileRenameException(string message) : base(message)
        {
        }

        public FileRenameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FileRenameException(string filePath, string targetPath, string message) : base(message)
        {
            FilePath = filePath;
            TargetPath = targetPath;
        }

        public FileRenameException(string filePath, string targetPath, string message, Exception innerException) : base(message, innerException)
        {
            FilePath = filePath;
            TargetPath = targetPath;
        }
    }

    /// <summary>
    /// PDF处理异常类
    /// </summary>
    public class PdfProcessingException : Exception
    {
        public string FilePath { get; }
        public int? PageNumber { get; }

        public PdfProcessingException() : base("PDF处理操作失败")
        {
        }

        public PdfProcessingException(string message) : base(message)
        {
        }

        public PdfProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PdfProcessingException(string filePath, string message) : base(message)
        {
            FilePath = filePath;
        }

        public PdfProcessingException(string filePath, int pageNumber, string message) : base(message)
        {
            FilePath = filePath;
            PageNumber = pageNumber;
        }

        public PdfProcessingException(string filePath, string message, Exception innerException) : base(message, innerException)
        {
            FilePath = filePath;
        }
    }

    /// <summary>
    /// 配置异常类
    /// </summary>
    public class ConfigurationException : Exception
    {
        public string ConfigKey { get; }
        public string ConfigPath { get; }

        public ConfigurationException() : base("配置操作失败")
        {
        }

        public ConfigurationException(string message) : base(message)
        {
        }

        public ConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ConfigurationException(string configKey, string message) : base(message)
        {
            ConfigKey = configKey;
        }

        public ConfigurationException(string configKey, string configPath, string message) : base(message)
        {
            ConfigKey = configKey;
            ConfigPath = configPath;
        }

        public ConfigurationException(string configKey, string message, Exception innerException) : base(message, innerException)
        {
            ConfigKey = configKey;
        }
    }

    /// <summary>
    /// Excel处理异常类
    /// </summary>
    public class ExcelProcessingException : Exception
    {
        public string FilePath { get; }
        public string WorksheetName { get; }
        public int? RowNumber { get; }

        public ExcelProcessingException() : base("Excel处理操作失败")
        {
        }

        public ExcelProcessingException(string message) : base(message)
        {
        }

        public ExcelProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ExcelProcessingException(string filePath, string message) : base(message)
        {
            FilePath = filePath;
        }

        public ExcelProcessingException(string filePath, string worksheetName, string message) : base(message)
        {
            FilePath = filePath;
            WorksheetName = worksheetName;
        }

        public ExcelProcessingException(string filePath, string worksheetName, int rowNumber, string message) : base(message)
        {
            FilePath = filePath;
            WorksheetName = worksheetName;
            RowNumber = rowNumber;
        }
    }

    /// <summary>
    /// 批量处理异常类
    /// </summary>
    public class BatchProcessingException : Exception
    {
        public int ProcessedCount { get; }
        public int TotalCount { get; }
        public string FailedItem { get; }

        public BatchProcessingException() : base("批量处理操作失败")
        {
        }

        public BatchProcessingException(string message) : base(message)
        {
        }

        public BatchProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BatchProcessingException(int processedCount, int totalCount, string message) : base(message)
        {
            ProcessedCount = processedCount;
            TotalCount = totalCount;
        }

        public BatchProcessingException(int processedCount, int totalCount, string failedItem, string message) : base(message)
        {
            ProcessedCount = processedCount;
            TotalCount = totalCount;
            FailedItem = failedItem;
        }

        public BatchProcessingException(int processedCount, int totalCount, string failedItem, string message, Exception innerException) : base(message, innerException)
        {
            ProcessedCount = processedCount;
            TotalCount = totalCount;
            FailedItem = failedItem;
        }
    }

    /// <summary>
    /// 验证异常类
    /// </summary>
    public class ValidationException : Exception
    {
        public string PropertyName { get; }
        public object PropertyValue { get; }

        public ValidationException() : base("验证失败")
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ValidationException(string propertyName, object propertyValue, string message) : base(message)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public ValidationException(string propertyName, object propertyValue, string message, Exception innerException) : base(message, innerException)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }
    }
}