using System;
using System.Collections.Generic;
using WindowsFormsApp3.Models;

namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// 批量处理完成事件参数类，包含批量处理完成后的统计信息
    /// </summary>
    public class BatchCompleteEventArgs : EventArgs
    {
        /// <summary>
        /// 获取成功处理的文件数量
        /// </summary>
        public int SuccessCount { get; }

        /// <summary>
        /// 获取处理失败的文件数量
        /// </summary>
        public int FailedCount { get; }

        /// <summary>
        /// 获取总处理的文件数量
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// 获取处理失败的文件信息列表
        /// </summary>
        public List<FileRenameInfo> FailedFiles { get; }

        /// <summary>
        /// 获取一个值，指示批量处理是否被取消
        /// </summary>
        public bool IsCanceled { get; }

        /// <summary>
        /// 获取处理总耗时（毫秒）
        /// </summary>
        public long ElapsedTimeMs { get; }

        /// <summary>
        /// 初始化 BatchCompleteEventArgs 类的新实例
        /// </summary>
        /// <param name="successCount">成功处理的文件数量</param>
        /// <param name="failedCount">处理失败的文件数量</param>
        /// <param name="failedFiles">处理失败的文件信息列表</param>
        /// <param name="isCanceled">是否被取消</param>
        /// <param name="elapsedTimeMs">处理总耗时（毫秒）</param>
        public BatchCompleteEventArgs(
            int successCount,
            int failedCount,
            List<FileRenameInfo> failedFiles,
            bool isCanceled = false,
            long elapsedTimeMs = 0)
        {
            SuccessCount = successCount;
            FailedCount = failedCount;
            TotalCount = successCount + failedCount;
            FailedFiles = failedFiles ?? new List<FileRenameInfo>();
            IsCanceled = isCanceled;
            ElapsedTimeMs = elapsedTimeMs;
        }
    }
}