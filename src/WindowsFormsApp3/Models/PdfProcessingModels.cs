using System;
using System.Collections.Generic;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Models
{
    #region 数据模型

    /// <summary>
    /// PDF处理请求模型
    /// </summary>
    public class PdfProcessingRequest
    {
        public string FilePath { get; set; }
        public PdfProcessingOperation Operation { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        public DateTime RequestTime { get; set; } = DateTime.Now;
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// PDF处理响应模型
    /// </summary>
    public class PdfProcessingResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public Exception Exception { get; set; }
        public DateTime ResponseTime { get; set; } = DateTime.Now;
        public string RequestId { get; set; }
        public long ProcessingTimeMs { get; set; }
    }

    /// <summary>
    /// PDF文件信息模型
    /// </summary>
    public class PdfFileInfo
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public int? PageCount { get; set; }
        public PageSize FirstPageSize { get; set; }
        public List<PageSizeInfo> AllPageSizes { get; set; } = new List<PageSizeInfo>();
        public List<PageBoxError> Errors { get; set; } = new List<PageBoxError>();
        public DateTime LastModified { get; set; }
    }

    /// <summary>
    /// PDF处理操作类型
    /// </summary>
    public enum PdfProcessingOperation
    {
        GetPageCount,
        GetFirstPageSize,
        GetAllPageSizes,
        SetPageBox,
        SetCropBoxToMediaBox,
        CheckPageBoxErrors,
        GenerateReport,
        CompareLibraries
    }

    /// <summary>
    /// PDF处理状态
    /// </summary>
    public enum PdfProcessingStatus
    {
        Idle,
        Processing,
        Completed,
        Failed,
        Cancelled
    }

    /// <summary>
    /// PDF处理进度信息
    /// </summary>
    public class PdfProcessingProgress
    {
        public string RequestId { get; set; }
        public PdfProcessingStatus Status { get; set; }
        public int ProgressPercentage { get; set; }
        public string CurrentOperation { get; set; }
        public string Message { get; set; }
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// PDF处理配置
    /// </summary>
    public class PdfProcessingConfig
    {
        public bool UseCropBoxByDefault { get; set; } = true;
        public bool EnableDetailedLogging { get; set; } = true;
        public int MaxRetryCount { get; set; } = 3;
        public int RetryDelayMs { get; set; } = 100;
        public bool EnableErrorRecovery { get; set; } = true;
        public string PreferredLibrary { get; set; } = "IText7"; // "PdfTools" or "IText7"
    }

    #endregion

    #region 结果模型

    /// <summary>
    /// PDF库比较结果
    /// </summary>
    public class PdfLibraryComparisonResult
    {
        public string FileName { get; set; }
        public PdfLibraryResult PdfToolsResult { get; set; }
        public PdfLibraryResult IText7Result { get; set; }
        public bool ResultsMatch { get; set; }
        public double WidthDifference { get; set; }
        public double HeightDifference { get; set; }
        public string Recommendation { get; set; }
    }

    /// <summary>
    /// 单个PDF库的处理结果
    /// </summary>
    public class PdfLibraryResult
    {
        public string LibraryName { get; set; }
        public bool Success { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string ErrorMessage { get; set; }
        public long ProcessingTimeMs { get; set; }
    }

    /// <summary>
    /// PDF处理统计信息
    /// </summary>
    public class PdfProcessingStatistics
    {
        public int TotalProcessed { get; set; }
        public int Successful { get; set; }
        public int Failed { get; set; }
        public double AverageProcessingTimeMs { get; set; }
        public Dictionary<string, int> ErrorTypes { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> OperationCounts { get; set; } = new Dictionary<string, int>();
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

    #endregion

    #region 事件参数

    /// <summary>
    /// PDF处理开始事件参数
    /// </summary>
    public class PdfProcessingStartedEventArgs : EventArgs
    {
        public string RequestId { get; set; }
        public PdfProcessingOperation Operation { get; set; }
        public string FilePath { get; set; }
    }

    /// <summary>
    /// PDF处理完成事件参数
    /// </summary>
    public class PdfProcessingCompletedEventArgs : EventArgs
    {
        public string RequestId { get; set; }
        public PdfProcessingOperation Operation { get; set; }
        public PdfProcessingResponse Response { get; set; }
    }

    /// <summary>
    /// PDF处理进度更新事件参数
    /// </summary>
    public class PdfProcessingProgressEventArgs : EventArgs
    {
        public PdfProcessingProgress Progress { get; set; }
    }

    #endregion

    #region 验证结果

    /// <summary>
    /// PDF处理验证结果
    /// </summary>
    public class PdfProcessingValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
        public ValidationErrorType ErrorType { get; set; }
        public string ValidationContext { get; set; }
    }

    #endregion
}