using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WindowsFormsApp3.Commands;
using WindowsFormsApp3.Interfaces;
using WindowsFormsApp3.Models;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 增强的撤销/重做服务接口
    /// </summary>
    public interface IEnhancedUndoRedoService : IUndoRedoService
    {
        /// <summary>
        /// 开始批量操作
        /// </summary>
        /// <param name="description">批量操作描述</param>
        /// <returns>批量操作ID</returns>
        Guid BeginBatchOperation(string description);

        /// <summary>
        /// 结束批量操作
        /// </summary>
        /// <param name="batchId">批量操作ID</param>
        void EndBatchOperation(Guid batchId);

        /// <summary>
        /// 执行批量操作中的命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="batchId">批量操作ID</param>
        void ExecuteCommandInBatch(CommandBase command, Guid batchId);

        /// <summary>
        /// 撤销到指定历史记录
        /// </summary>
        /// <param name="historyEntryId">历史记录ID</param>
        /// <returns>撤销的命令数量</returns>
        int UndoTo(Guid historyEntryId);

        /// <summary>
        /// 重做到指定历史记录
        /// </summary>
        /// <param name="historyEntryId">历史记录ID</param>
        /// <returns>重做的命令数量</returns>
        int RedoTo(Guid historyEntryId);

        /// <summary>
        /// 撤销批量操作
        /// </summary>
        /// <param name="batchId">批量操作ID</param>
        /// <returns>撤销的命令数量</returns>
        int UndoBatchOperation(Guid batchId);

        /// <summary>
        /// 获取完整的历史记录
        /// </summary>
        /// <returns>历史记录列表</returns>
        List<CommandHistoryEntry> GetFullHistory();

        /// <summary>
        /// 获取批量操作历史
        /// </summary>
        /// <returns>批量操作历史列表</returns>
        List<BatchOperationHistory> GetBatchOperationHistory();

        /// <summary>
        /// 保存历史记录到文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        Task SaveHistoryToFileAsync(string filePath);

        /// <summary>
        /// 从文件加载历史记录
        /// </summary>
        /// <param name="filePath">文件路径</param>
        Task LoadHistoryFromFileAsync(string filePath);

        /// <summary>
        /// 清除指定时间之前的历史记录
        /// </summary>
        /// <param name="beforeTime">时间点</param>
        int ClearHistoryBefore(DateTime beforeTime);

        /// <summary>
        /// 获取历史记录统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        UndoRedoStatistics GetStatistics();

        /// <summary>
        /// 历史记录变化事件
        /// </summary>
        event EventHandler<HistoryChangedEventArgs> HistoryChanged;
    }

    /// <summary>
    /// 历史记录变化事件参数
    /// </summary>
    public class HistoryChangedEventArgs : EventArgs
    {
        public CommandHistoryEntry ChangedEntry { get; set; }
        public HistoryChangeType ChangeType { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// 历史记录变化类型
    /// </summary>
    public enum HistoryChangeType
    {
        CommandExecuted,
        CommandUndone,
        CommandRedone,
        BatchStarted,
        BatchCompleted,
        HistoryCleared,
        HistoryLoaded
    }

    /// <summary>
    /// 撤销/重做统计信息
    /// </summary>
    public class UndoRedoStatistics
    {
        public int TotalCommands { get; set; }
        public int UndoCommands { get; set; }
        public int RedoCommands { get; set; }
        public int BatchOperations { get; set; }
        public TimeSpan TotalExecutionTime { get; set; }
        public Dictionary<string, int> CommandTypeCount { get; set; }
        public DateTime OldestCommand { get; set; }
        public DateTime NewestCommand { get; set; }
    }

    /// <summary>
    /// 增强的撤销/重做服务实现
    /// </summary>
    public class EnhancedUndoRedoService : UndoRedoService, IEnhancedUndoRedoService
    {
        private readonly Dictionary<Guid, BatchOperationHistory> _activeBatchOperations = new Dictionary<Guid, BatchOperationHistory>();
        private readonly List<CommandHistoryEntry> _historyEntries = new List<CommandHistoryEntry>();
        private readonly List<BatchOperationHistory> _completedBatchOperations = new List<BatchOperationHistory>();
        private readonly object _historyLock = new object();
        private readonly string _historyFilePath;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志服务</param>
        /// <param name="maxHistorySize">最大历史记录大小</param>
        /// <param name="historyFilePath">历史记录文件路径</param>
        public EnhancedUndoRedoService(Interfaces.ILogger logger, int maxHistorySize = 500, string historyFilePath = null)
            : base(logger, maxHistorySize)
        {
            _historyFilePath = historyFilePath ?? AppDataPathManager.CommandHistoryPath;
        }

        /// <summary>
        /// 历史记录变化事件
        /// </summary>
        public event EventHandler<HistoryChangedEventArgs> HistoryChanged;

        /// <summary>
        /// 开始批量操作
        /// </summary>
        /// <param name="description">批量操作描述</param>
        /// <returns>批量操作ID</returns>
        public Guid BeginBatchOperation(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("批量操作描述不能为空", nameof(description));

            var batchOperation = new BatchOperationHistory
            {
                Description = description
            };

            lock (_historyLock)
            {
                _activeBatchOperations[batchOperation.Id] = batchOperation;
                _logger.LogDebug($"开始批量操作: {description} (ID: {batchOperation.Id})");
            }

            OnHistoryChanged(new HistoryChangedEventArgs
            {
                ChangeType = HistoryChangeType.BatchStarted,
                Description = description
            });

            return batchOperation.Id;
        }

        /// <summary>
        /// 结束批量操作
        /// </summary>
        /// <param name="batchId">批量操作ID</param>
        public void EndBatchOperation(Guid batchId)
        {
            lock (_historyLock)
            {
                if (_activeBatchOperations.TryGetValue(batchId, out var batchOperation))
                {
                    batchOperation.EndTime = DateTime.Now;

                    _activeBatchOperations.Remove(batchId);
                    _completedBatchOperations.Add(batchOperation);

                    _logger.LogDebug($"结束批量操作: {batchOperation.Description} (ID: {batchId}), " +
                                  $"执行了 {batchOperation.Entries.Count} 个命令，耗时 {batchOperation.TotalExecutionTimeMs}ms");

                    OnHistoryChanged(new HistoryChangedEventArgs
                    {
                        ChangeType = HistoryChangeType.BatchCompleted,
                        Description = $"批量操作完成: {batchOperation.Description}"
                    });
                }
                else
                {
                    _logger.LogWarning($"尝试结束不存在的批量操作: {batchId}");
                }
            }
        }

        /// <summary>
        /// 在批量操作中执行命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="batchId">批量操作ID</param>
        public void ExecuteCommandInBatch(CommandBase command, Guid batchId)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            lock (_historyLock)
            {
                if (_activeBatchOperations.TryGetValue(batchId, out var batchOperation))
                {
                    var startTime = DateTime.Now;

                    // 执行命令
                    base.ExecuteCommand(command);

                    var executionTime = (DateTime.Now - startTime).Milliseconds;

                    // 创建历史记录条目
                    var historyEntry = new CommandHistoryEntry
                    {
                        Description = command.Description,
                        CommandType = command.GetType(),
                        IsBatchOperation = true,
                        BatchGroupId = batchId,
                        BatchIndex = batchOperation.Entries.Count,
                        BatchCount = -1, // 结束时更新
                        ExecutionTimeMs = executionTime,
                        Tags = new List<string> { "批量操作" }
                    };

                    batchOperation.Entries.Add(historyEntry);
                    _historyEntries.Add(historyEntry);

                    _logger.LogDebug($"在批量操作中执行命令: {command.Description} (批量ID: {batchId})");

                    OnHistoryChanged(new HistoryChangedEventArgs
                    {
                        ChangedEntry = historyEntry,
                        ChangeType = HistoryChangeType.CommandExecuted,
                        Description = $"批量操作命令: {command.Description}"
                    });
                }
                else
                {
                    throw new InvalidOperationException($"批量操作 {batchId} 不存在或已结束");
                }
            }
        }

        /// <summary>
        /// 执行单个命令（非批量操作）
        /// </summary>
        /// <param name="command">命令</param>
        public override void ExecuteCommand(CommandBase command)
        {
            var startTime = DateTime.Now;

            // 执行命令
            base.ExecuteCommand(command);

            var executionTime = (DateTime.Now - startTime).Milliseconds;

            // 创建历史记录条目
            var historyEntry = new CommandHistoryEntry
            {
                Description = command.Description,
                CommandType = command.GetType(),
                IsBatchOperation = false,
                ExecutionTimeMs = executionTime,
                Tags = new List<string> { "单个操作" }
            };

            lock (_historyLock)
            {
                _historyEntries.Add(historyEntry);
            }

            _logger.LogDebug($"执行命令: {command.Description}，耗时 {executionTime}ms");

            OnHistoryChanged(new HistoryChangedEventArgs
            {
                ChangedEntry = historyEntry,
                ChangeType = HistoryChangeType.CommandExecuted,
                Description = command.Description
            });
        }

        /// <summary>
        /// 撤销命令
        /// </summary>
        /// <returns>撤销的命令描述</returns>
        public override string Undo()
        {
            var result = base.Undo();

            if (!string.IsNullOrEmpty(result))
            {
                lock (_historyLock)
                {
                    var entry = _historyEntries.LastOrDefault(e => !e.IsUndone);
                    if (entry != null)
                    {
                        entry.IsUndone = true;
                        entry.UndoTimestamp = DateTime.Now;

                        OnHistoryChanged(new HistoryChangedEventArgs
                        {
                            ChangedEntry = entry,
                            ChangeType = HistoryChangeType.CommandUndone,
                            Description = $"撤销: {entry.Description}"
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 重做命令
        /// </summary>
        /// <returns>重做的命令描述</returns>
        public override string Redo()
        {
            var result = base.Redo();

            if (!string.IsNullOrEmpty(result))
            {
                lock (_historyLock)
                {
                    var entry = _historyEntries.LastOrDefault(e => e.IsUndone && !e.IsRedone);
                    if (entry != null)
                    {
                        entry.IsRedone = true;
                        entry.RedoTimestamp = DateTime.Now;

                        OnHistoryChanged(new HistoryChangedEventArgs
                        {
                            ChangedEntry = entry,
                            ChangeType = HistoryChangeType.CommandRedone,
                            Description = $"重做: {entry.Description}"
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 撤销到指定历史记录
        /// </summary>
        /// <param name="historyEntryId">历史记录ID</param>
        /// <returns>撤销的命令数量</returns>
        public int UndoTo(Guid historyEntryId)
        {
            lock (_historyLock)
            {
                var targetIndex = _historyEntries.FindIndex(e => e.Id == historyEntryId);
                if (targetIndex == -1)
                    throw new ArgumentException($"历史记录 {historyEntryId} 不存在");

                int undoCount = 0;
                for (int i = _historyEntries.Count - 1; i >= targetIndex; i--)
                {
                    if (!_historyEntries[i].IsUndone && CanUndo())
                    {
                        Undo();
                        undoCount++;
                    }
                }

                return undoCount;
            }
        }

        /// <summary>
        /// 重做到指定历史记录
        /// </summary>
        /// <param name="historyEntryId">历史记录ID</param>
        /// <returns>重做的命令数量</returns>
        public int RedoTo(Guid historyEntryId)
        {
            lock (_historyLock)
            {
                var targetIndex = _historyEntries.FindIndex(e => e.Id == historyEntryId);
                if (targetIndex == -1)
                    throw new ArgumentException($"历史记录 {historyEntryId} 不存在");

                int redoCount = 0;
                for (int i = 0; i <= targetIndex; i++)
                {
                    if (_historyEntries[i].IsUndone && !_historyEntries[i].IsRedone && CanRedo())
                    {
                        Redo();
                        redoCount++;
                    }
                }

                return redoCount;
            }
        }

        /// <summary>
        /// 撤销批量操作
        /// </summary>
        /// <param name="batchId">批量操作ID</param>
        /// <returns>撤销的命令数量</returns>
        public int UndoBatchOperation(Guid batchId)
        {
            lock (_historyLock)
            {
                var batchOperation = _completedBatchOperations.FirstOrDefault(b => b.Id == batchId);
                if (batchOperation == null)
                    throw new ArgumentException($"批量操作 {batchId} 不存在");

                int undoCount = 0;
                // 撤销批量操作中的所有命令（从后往前）
                for (int i = batchOperation.Entries.Count - 1; i >= 0; i--)
                {
                    var entry = batchOperation.Entries[i];
                    if (!entry.IsUndone && CanUndo())
                    {
                        Undo();
                        undoCount++;
                    }
                }

                _logger.LogDebug($"撤销批量操作: {batchOperation.Description}，撤销了 {undoCount} 个命令");
                return undoCount;
            }
        }

        /// <summary>
        /// 获取完整的历史记录
        /// </summary>
        /// <returns>历史记录列表</returns>
        public List<CommandHistoryEntry> GetFullHistory()
        {
            lock (_historyLock)
            {
                return _historyEntries.Select(e => e.Clone()).ToList();
            }
        }

        /// <summary>
        /// 获取批量操作历史
        /// </summary>
        /// <returns>批量操作历史列表</returns>
        public List<BatchOperationHistory> GetBatchOperationHistory()
        {
            lock (_historyLock)
            {
                var allBatches = new List<BatchOperationHistory>();
                allBatches.AddRange(_activeBatchOperations.Values);
                allBatches.AddRange(_completedBatchOperations);
                return allBatches.OrderByDescending(b => b.StartTime).ToList();
            }
        }

        /// <summary>
        /// 保存历史记录到文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public async Task SaveHistoryToFileAsync(string filePath)
        {
            try
            {
                var historyData = new
                {
                    Version = "1.0",
                    SavedAt = DateTime.Now,
                    HistoryEntries = _historyEntries,
                    BatchOperations = _completedBatchOperations
                };

                var json = JsonConvert.SerializeObject(historyData, Formatting.Indented);

                await Task.Run(() =>
                {
                    IOHelper.EnsureDirectoryExists(Path.GetDirectoryName(filePath));
                    File.WriteAllText(filePath, json);
                });

                _logger.LogDebug($"历史记录已保存到: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"保存历史记录失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 从文件加载历史记录
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public async Task LoadHistoryFromFileAsync(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning($"历史记录文件不存在: {filePath}");
                    return;
                }

                var json = await Task.Run(() => File.ReadAllText(filePath));
                var historyData = JsonConvert.DeserializeAnonymousType(json, new
                {
                    Version = "",
                    SavedAt = DateTime.MinValue,
                    HistoryEntries = new List<CommandHistoryEntry>(),
                    BatchOperations = new List<BatchOperationHistory>()
                });

                lock (_historyLock)
                {
                    _historyEntries.Clear();
                    _completedBatchOperations.Clear();

                    if (historyData.HistoryEntries != null)
                        _historyEntries.AddRange(historyData.HistoryEntries);

                    if (historyData.BatchOperations != null)
                        _completedBatchOperations.AddRange(historyData.BatchOperations);
                }

                _logger.LogDebug($"从文件加载历史记录: {filePath}");

                OnHistoryChanged(new HistoryChangedEventArgs
                {
                    ChangeType = HistoryChangeType.HistoryLoaded,
                    Description = "历史记录已从文件加载"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"加载历史记录失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 清除指定时间之前的历史记录
        /// </summary>
        /// <param name="beforeTime">时间点</param>
        public int ClearHistoryBefore(DateTime beforeTime)
        {
            lock (_historyLock)
            {
                var entriesToRemove = _historyEntries.Where(e => e.Timestamp < beforeTime).ToList();

                foreach (var entry in entriesToRemove)
                {
                    _historyEntries.Remove(entry);
                }

                var batchesToRemove = _completedBatchOperations
                    .Where(b => b.EndTime.HasValue && b.EndTime.Value < beforeTime)
                    .ToList();

                foreach (var batch in batchesToRemove)
                {
                    _completedBatchOperations.Remove(batch);
                }

                _logger.LogDebug($"清除了 {entriesToRemove.Count} 个历史记录和 {batchesToRemove.Count} 个批量操作记录");
                return entriesToRemove.Count;
            }
        }

        /// <summary>
        /// 获取历史记录统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        public UndoRedoStatistics GetStatistics()
        {
            lock (_historyLock)
            {
                var stats = new UndoRedoStatistics
                {
                    TotalCommands = _historyEntries.Count,
                    UndoCommands = _historyEntries.Count(e => e.IsUndone),
                    RedoCommands = _historyEntries.Count(e => e.IsRedone),
                    BatchOperations = _completedBatchOperations.Count,
                    CommandTypeCount = _historyEntries
                        .Where(e => e.CommandType != null)
                        .GroupBy(e => e.CommandType.Name ?? "Unknown")
                        .ToDictionary(g => g.Key, g => g.Count()),
                    OldestCommand = _historyEntries.Any() ? _historyEntries.Min(e => e.Timestamp) : DateTime.MinValue,
                    NewestCommand = _historyEntries.Any() ? _historyEntries.Max(e => e.Timestamp) : DateTime.MinValue,
                    TotalExecutionTime = TimeSpan.FromMilliseconds(_historyEntries.Sum(e => e.ExecutionTimeMs))
                };

                return stats;
            }
        }

        /// <summary>
        /// 触发历史记录变化事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnHistoryChanged(HistoryChangedEventArgs e)
        {
            HistoryChanged?.Invoke(this, e);
        }
    }
}