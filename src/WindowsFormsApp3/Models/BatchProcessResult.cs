using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// 批量处理结果
    /// </summary>
    public class BatchProcessResult
    {
        private readonly Stopwatch _stopwatch;
        private readonly List<BatchProcessError> _errors;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchProcessResult()
        {
            _stopwatch = Stopwatch.StartNew();
            _errors = new List<BatchProcessError>();
        }

        /// <summary>
        /// 成功处理的文件数量
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 错误数量
        /// </summary>
        public int ErrorCount => _errors.Count;

        /// <summary>
        /// 是否被取消
        /// </summary>
        public bool IsCanceled { get; set; }

        /// <summary>
        /// 错误列表
        /// </summary>
        public IReadOnlyList<BatchProcessError> Errors => _errors.AsReadOnly();

        /// <summary>
        /// 总耗时（毫秒）
        /// </summary>
        public long ElapsedTimeMs => _stopwatch.ElapsedMilliseconds;

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="errorMessage">错误信息</param>
        public void AddError(string filePath, string errorMessage)
        {
            _errors.Add(new BatchProcessError
            {
                FilePath = filePath,
                ErrorMessage = errorMessage,
                Timestamp = DateTime.Now
            });
        }

        /// <summary>
        /// 停止计时
        /// </summary>
        public void Stop()
        {
            _stopwatch.Stop();
        }
    }

    /// <summary>
    /// 批量处理错误信息
    /// </summary>
    public class BatchProcessError
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 错误发生时间
        /// </summary>
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 批量处理进度信息
    /// </summary>
    public class BatchProgress
    {
        /// <summary>
        /// 已处理的文件数量
        /// </summary>
        public int ProcessedCount { get; set; }

        /// <summary>
        /// 总文件数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 当前正在处理的文件名
        /// </summary>
        public string CurrentFile { get; set; }

        /// <summary>
        /// 进度百分比（0-100）
        /// </summary>
        public double Percentage => TotalCount > 0 ? (double)ProcessedCount / TotalCount * 100 : 0;

        /// <summary>
        /// 预估剩余时间（毫秒）
        /// </summary>
        public long? EstimatedRemainingTimeMs { get; set; }

        /// <summary>
        /// 处理速度（文件/秒）
        /// </summary>
        public double ProcessRate { get; set; }
    }
}