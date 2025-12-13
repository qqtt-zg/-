using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// 命令历史记录条目
    /// </summary>
    public class CommandHistoryEntry
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CommandHistoryEntry()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.Now;
            Tags = new List<string>();
            Metadata = new Dictionary<string, object>();
        }

        /// <summary>
        /// 唯一标识符
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 命令描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 命令类型
        /// </summary>
        public Type CommandType { get; set; }

        /// <summary>
        /// 执行时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 是否为批量操作的一部分
        /// </summary>
        public bool IsBatchOperation { get; set; }

        /// <summary>
        /// 批量操作组ID（如果适用）
        /// </summary>
        public Guid? BatchGroupId { get; set; }

        /// <summary>
        /// 批量操作中的索引（从0开始）
        /// </summary>
        public int BatchIndex { get; set; }

        /// <summary>
        /// 批量操作总数
        /// </summary>
        public int BatchCount { get; set; }

        /// <summary>
        /// 操作标签
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// 扩展元数据
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; }

        /// <summary>
        /// 操作执行耗时（毫秒）
        /// </summary>
        public long ExecutionTimeMs { get; set; }

        /// <summary>
        /// 是否已撤销
        /// </summary>
        public bool IsUndone { get; set; }

        /// <summary>
        /// 撤销时间戳
        /// </summary>
        public DateTime? UndoTimestamp { get; set; }

        /// <summary>
        /// 是否已重做
        /// </summary>
        public bool IsRedone { get; set; }

        /// <summary>
        /// 重做时间戳
        /// </summary>
        public DateTime? RedoTimestamp { get; set; }

        /// <summary>
        /// 操作状态
        /// </summary>
        public CommandStatus Status { get; set; } = CommandStatus.Success;

        /// <summary>
        /// 获取格式化的显示文本
        /// </summary>
        /// <returns>显示文本</returns>
        public string GetDisplayText()
        {
            if (IsBatchOperation)
            {
                return $"[{BatchIndex + 1}/{BatchCount}] {Description}";
            }
            return Description;
        }

        /// <summary>
        /// 获取状态显示文本
        /// </summary>
        /// <returns>状态文本</returns>
        public string GetStatusText()
        {
            if (IsUndone && !IsRedone)
                return "已撤销";
            if (IsRedone)
                return "已重做";
            return "已执行";
        }

        /// <summary>
        /// 克隆历史记录条目
        /// </summary>
        /// <returns>克隆的条目</returns>
        public CommandHistoryEntry Clone()
        {
            return new CommandHistoryEntry
            {
                Id = Id,
                Description = Description,
                CommandType = CommandType,
                Timestamp = Timestamp,
                IsBatchOperation = IsBatchOperation,
                BatchGroupId = BatchGroupId,
                BatchIndex = BatchIndex,
                BatchCount = BatchCount,
                Tags = new List<string>(Tags),
                Metadata = new Dictionary<string, object>(Metadata),
                ExecutionTimeMs = ExecutionTimeMs,
                IsUndone = IsUndone,
                UndoTimestamp = UndoTimestamp,
                IsRedone = IsRedone,
                RedoTimestamp = RedoTimestamp,
                Status = Status
            };
        }
    }

    /// <summary>
    /// 批量操作历史记录
    /// </summary>
    public class BatchOperationHistory
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchOperationHistory()
        {
            Id = Guid.NewGuid();
            Entries = new List<CommandHistoryEntry>();
            StartTime = DateTime.Now;
        }

        /// <summary>
        /// 批量操作ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 批量操作描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 包含的命令条目
        /// </summary>
        public List<CommandHistoryEntry> Entries { get; set; }

        /// <summary>
        /// 操作标签
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// 总执行时间（毫秒）
        /// </summary>
        public long TotalExecutionTimeMs => EndTime.HasValue ?
            (long)(EndTime.Value - StartTime).TotalMilliseconds : 0;

        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool IsCompleted => EndTime.HasValue;

        /// <summary>
        /// 成功执行的操作数量
        /// </summary>
        public int SuccessCount => Entries.Count(e => !e.IsUndone || e.IsRedone);

        /// <summary>
        /// 失败的操作数量
        /// </summary>
        public int FailureCount => Entries.Count - SuccessCount;

        /// <summary>
        /// 总命令数量
        /// </summary>
        public int TotalCommands => Entries.Count;

        /// <summary>
        /// 成功执行的操作数量
        /// </summary>
        public int SuccessfulCommands => SuccessCount;

        /// <summary>
        /// 批量操作状态
        /// </summary>
        public BatchOperationStatus Status { get; set; } = BatchOperationStatus.Completed;

        /// <summary>
        /// 获取成功率
        /// </summary>
        /// <returns>成功率（0-1之间）</returns>
        public double GetSuccessRate()
        {
            if (Entries.Count == 0) return 1.0;
            return (double)SuccessCount / Entries.Count;
        }
    }
}