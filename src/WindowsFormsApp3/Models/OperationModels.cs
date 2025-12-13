using System;

namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// 命令状态枚举
    /// </summary>
    public enum CommandStatus
    {
        /// <summary>
        /// 等待执行
        /// </summary>
        Pending,
        
        /// <summary>
        /// 执行成功
        /// </summary>
        Success,
        
        /// <summary>
        /// 执行失败
        /// </summary>
        Failed,
        
        /// <summary>
        /// 已取消
        /// </summary>
        Cancelled
    }

    /// <summary>
    /// 批量操作状态枚举
    /// </summary>
    public enum BatchOperationStatus
    {
        /// <summary>
        /// 正在运行
        /// </summary>
        Running,
        
        /// <summary>
        /// 已完成
        /// </summary>
        Completed,
        
        /// <summary>
        /// 执行失败
        /// </summary>
        Failed,
        
        /// <summary>
        /// 已取消
        /// </summary>
        Cancelled
    }
}