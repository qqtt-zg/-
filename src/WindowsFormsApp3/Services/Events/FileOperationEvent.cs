using System;

namespace WindowsFormsApp3.Services.Events
{
    /// <summary>
    /// 文件操作事件基类
    /// </summary>
    public abstract class FileOperationEvent
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        
        /// <summary>
        /// 错误信息（如果操作失败）
        /// </summary>
        public string ErrorMessage { get; set; }
        
        protected FileOperationEvent()
        {
            Timestamp = DateTime.Now;
        }
    }
    
    /// <summary>
    /// 文件重命名事件
    /// </summary>
    public class FileRenamedEvent : FileOperationEvent
    {
        /// <summary>
        /// 原文件名
        /// </summary>
        public string OriginalFileName { get; set; }
        
        /// <summary>
        /// 新文件名
        /// </summary>
        public string NewFileName { get; set; }
        
        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }
    }
    
    /// <summary>
    /// 文件删除事件
    /// </summary>
    public class FileDeletedEvent : FileOperationEvent
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }
    }
    
    /// <summary>
    /// 文件创建事件
    /// </summary>
    public class FileCreatedEvent : FileOperationEvent
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }
    }
}