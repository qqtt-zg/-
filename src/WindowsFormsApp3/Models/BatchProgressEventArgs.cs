using System;

namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// 批量处理进度事件参数类，包含批量处理过程中的进度信息
    /// </summary>
    public class BatchProgressEventArgs : EventArgs
    {
        /// <summary>
        /// 获取已处理的文件数量
        /// </summary>
        public int ProcessedCount { get; }

        /// <summary>
        /// 获取总文件数量
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// 获取当前处理进度百分比（0-100）
        /// </summary>
        public int ProgressPercentage { get; }

        /// <summary>
        /// 获取当前正在处理的文件信息
        /// </summary>
        public string CurrentFileName { get; }

        /// <summary>
        /// 初始化 BatchProgressEventArgs 类的新实例
        /// </summary>
        /// <param name="processedCount">已处理的文件数量</param>
        /// <param name="totalCount">总文件数量</param>
        /// <param name="currentFileName">当前正在处理的文件名称</param>
        public BatchProgressEventArgs(int processedCount, int totalCount, string currentFileName = "")
        {
            ProcessedCount = processedCount;
            TotalCount = totalCount;
            CurrentFileName = currentFileName;
            ProgressPercentage = totalCount > 0 ? (int)Math.Round((double)processedCount / totalCount * 100) : 0;
        }
    }
}