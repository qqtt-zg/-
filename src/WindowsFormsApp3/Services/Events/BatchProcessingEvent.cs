using System;

namespace WindowsFormsApp3.Services.Events
{
    /// <summary>
    /// 批量处理事件基类
    /// </summary>
    public abstract class BatchProcessingEvent
    {
        /// <summary>
        /// 处理类型
        /// </summary>
        public string ProcessingType { get; set; }

        /// <summary>
        /// 事件时间
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 目标路径
        /// </summary>
        public string TargetPath { get; set; }

        protected BatchProcessingEvent()
        {
            Timestamp = DateTime.Now;
        }
    }
}