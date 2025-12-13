using System;

namespace WindowsFormsApp3.Services.Events
{
    /// <summary>
    /// 应用状态事件基类
    /// </summary>
    public abstract class AppStateEvent
    {
        /// <summary>
        /// 事件时间
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// 应用版本
        /// </summary>
        public string AppVersion { get; set; }
        
        protected AppStateEvent()
        {
            Timestamp = DateTime.Now;
            AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
    
    /// <summary>
    /// 应用启动事件
    /// </summary>
    public class AppStartedEvent : AppStateEvent
    {
        /// <summary>
        /// 启动参数
        /// </summary>
        public string[] StartupArgs { get; set; }
        
        /// <summary>
        /// 启动时间（毫秒）
        /// </summary>
        public long StartupTimeMs { get; set; }
    }
    
    /// <summary>
    /// 应用关闭事件
    /// </summary>
    public class AppShutdownEvent : AppStateEvent
    {
        /// <summary>
        /// 运行时长（毫秒）
        /// </summary>
        public long RunTimeMs { get; set; }
        
        /// <summary>
        /// 关闭原因
        /// </summary>
        public string ShutdownReason { get; set; }
    }
    
    /// <summary>
    /// 批量处理开始事件
    /// </summary>
    public class BatchProcessingStartedEvent : AppStateEvent
    {
        /// <summary>
        /// 处理类型
        /// </summary>
        public string ProcessingType { get; set; }
        
        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileCount { get; set; }
        
        /// <summary>
        /// 目标路径
        /// </summary>
        public string TargetPath { get; set; }
    }
    
    /// <summary>
    /// 批量处理完成事件
    /// </summary>
    public class BatchProcessingCompletedEvent : AppStateEvent
    {
        /// <summary>
        /// 处理类型
        /// </summary>
        public string ProcessingType { get; set; }
        
        /// <summary>
        /// 成功数量
        /// </summary>
        public int SuccessCount { get; set; }
        
        /// <summary>
        /// 失败数量
        /// </summary>
        public int FailedCount { get; set; }
        
        /// <summary>
        /// 总耗时（毫秒）
        /// </summary>
        public long TotalTimeMs { get; set; }
        
        /// <summary>
        /// 平均处理时间（毫秒）
        /// </summary>
        public double AverageTimePerFileMs { get; set; }
    }
}