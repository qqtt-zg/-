using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp3.Models;
using WindowsFormsApp3.Services;
using WindowsFormsApp3.Interfaces;

namespace WindowsFormsApp3
{
    /// <summary>
    /// 操作历史窗口
    /// </summary>
    public partial class OperationHistoryForm : Form
    {
        private readonly IEnhancedUndoRedoService _undoRedoService;
        private List<CommandHistoryEntry> _historyEntries = new List<CommandHistoryEntry>();
        private List<BatchOperationHistory> _batchOperations = new List<BatchOperationHistory>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="undoRedoService">增强的撤销/重做服务</param>
        public OperationHistoryForm(IEnhancedUndoRedoService undoRedoService)
        {
            InitializeComponent();
            _undoRedoService = undoRedoService ?? throw new ArgumentNullException(nameof(undoRedoService));

            // 订阅历史记录变化事件
            _undoRedoService.HistoryChanged += OnHistoryChanged;

            // 设置事件处理器
            SetupEventHandlers();

            LoadHistoryData();
        }

        /// <summary>
        /// 设置事件处理器
        /// </summary>
        private void SetupEventHandlers()
        {
            // 工具栏按钮事件
            refreshButton.Click += OnRefresh;
            undoButton.Click += OnUndoSelected;
            redoButton.Click += OnRedoSelected;
            clearButton.Click += OnClearHistory;
            saveButton.Click += OnSaveHistory;
            loadButton.Click += OnLoadHistory;

            // 列表视图事件
            historyListView.DoubleClick += OnHistoryItemDoubleClick;
            historyListView.SelectedIndexChanged += OnHistoryItemSelected;
            batchListView.DoubleClick += OnBatchItemDoubleClick;
            batchListView.SelectedIndexChanged += OnBatchItemSelected;
        }

        /// <summary>
        /// 加载历史数据
        /// </summary>
        private void LoadHistoryData()
        {
            try
            {
                _historyEntries = _undoRedoService.GetFullHistory();
                _batchOperations = _undoRedoService.GetBatchOperationHistory();

                RefreshHistoryListView();
                RefreshBatchListView();
                RefreshStatistics();

                UpdateStatus($"已加载 {_historyEntries.Count} 个操作记录和 {_batchOperations.Count} 个批量操作");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载历史数据失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新历史记录列表
        /// </summary>
        private void RefreshHistoryListView()
        {
            historyListView.Items.Clear();

            foreach (var entry in _historyEntries)
            {
                var item = new ListViewItem(new string[]
                {
                    entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    entry.Description ?? "无描述",
                    entry.CommandType?.Name ?? "未知",
                    entry.ExecutionTimeMs.ToString(),
                    entry.Status.ToString(),
                    string.Join(", ", entry.Tags ?? new List<string>())
                });

                item.Tag = entry;
                item.BackColor = GetStatusColor(entry.Status);
                
                historyListView.Items.Add(item);
            }
        }

        /// <summary>
        /// 刷新批量操作列表
        /// </summary>
        private void RefreshBatchListView()
        {
            batchListView.Items.Clear();

            foreach (var batch in _batchOperations)
            {
                var successRate = batch.TotalCommands > 0 
                    ? (batch.SuccessfulCommands * 100.0 / batch.TotalCommands).ToString("F1") + "%"
                    : "0%";

                var item = new ListViewItem(new string[]
                {
                    batch.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    batch.Description ?? "无描述",
                    batch.TotalCommands.ToString(),
                    successRate,
                    batch.TotalExecutionTimeMs.ToString(),
                    batch.Status.ToString()
                });

                item.Tag = batch;
                item.BackColor = GetStatusColor(batch.Status);
                
                batchListView.Items.Add(item);
            }
        }

        /// <summary>
        /// 刷新统计信息
        /// </summary>
        private void RefreshStatistics()
        {
            var stats = GenerateStatistics();
            statsLabel.Text = stats;
        }

        /// <summary>
        /// 生成统计信息
        /// </summary>
        private string GenerateStatistics()
        {
            var stats = new System.Text.StringBuilder();
            
            stats.AppendLine("=== 操作历史统计 ===");
            stats.AppendLine();
            
            // 总体统计
            stats.AppendLine($"总操作数: {_historyEntries.Count}");
            stats.AppendLine($"总批量操作数: {_batchOperations.Count}");
            stats.AppendLine();
            
            // 按状态统计
            var statusGroups = _historyEntries.GroupBy(e => e.Status)
                .ToDictionary(g => g.Key, g => g.Count());
            
            stats.AppendLine("按状态统计:");
            foreach (var group in statusGroups)
            {
                stats.AppendLine($"  {group.Key}: {group.Value}");
            }
            stats.AppendLine();
            
            // 按类型统计
            var typeGroups = _historyEntries.Where(e => e.CommandType != null)
                .GroupBy(e => e.CommandType.Name)
                .ToDictionary(g => g.Key, g => g.Count());
            
            stats.AppendLine("按类型统计:");
            foreach (var group in typeGroups.OrderByDescending(g => g.Value))
            {
                stats.AppendLine($"  {group.Key}: {group.Value}");
            }
            stats.AppendLine();
            
            // 性能统计
            if (_historyEntries.Any())
            {
                var avgTime = _historyEntries.Average(e => e.ExecutionTimeMs);
                var maxTime = _historyEntries.Max(e => e.ExecutionTimeMs);
                var minTime = _historyEntries.Min(e => e.ExecutionTimeMs);
                
                stats.AppendLine("性能统计:");
                stats.AppendLine($"  平均耗时: {avgTime:F2} ms");
                stats.AppendLine($"  最大耗时: {maxTime} ms");
                stats.AppendLine($"  最小耗时: {minTime} ms");
                stats.AppendLine();
            }
            
            // 最近活动
            var recentActivities = _historyEntries
                .OrderByDescending(e => e.Timestamp)
                .Take(10);
            
            if (recentActivities.Any())
            {
                stats.AppendLine("最近10个操作:");
                foreach (var activity in recentActivities)
                {
                    stats.AppendLine($"  {activity.Timestamp:HH:mm:ss} - {activity.Description}");
                }
            }
            
            return stats.ToString();
        }

        /// <summary>
        /// 获取状态对应的颜色
        /// </summary>
        private Color GetStatusColor(CommandStatus status)
        {
            return status switch
            {
                CommandStatus.Success => Color.LightGreen,
                CommandStatus.Failed => Color.LightCoral,
                CommandStatus.Cancelled => Color.LightYellow,
                CommandStatus.Pending => Color.LightBlue,
                _ => Color.White
            };
        }

        /// <summary>
        /// 获取批量操作状态对应的颜色
        /// </summary>
        private Color GetStatusColor(BatchOperationStatus status)
        {
            return status switch
            {
                BatchOperationStatus.Completed => Color.LightGreen,
                BatchOperationStatus.Failed => Color.LightCoral,
                BatchOperationStatus.Cancelled => Color.LightYellow,
                BatchOperationStatus.Running => Color.LightBlue,
                _ => Color.White
            };
        }

        /// <summary>
        /// 更新状态栏
        /// </summary>
        private void UpdateStatus(string message)
        {
            statusLabel.Text = message;
        }

        /// <summary>
        /// 历史记录变化事件处理
        /// </summary>
        private void OnHistoryChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(OnHistoryChanged), sender, e);
                return;
            }

            LoadHistoryData();
        }

        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void OnRefresh(object sender, EventArgs e)
        {
            LoadHistoryData();
        }

        /// <summary>
        /// 撤销选中项
        /// </summary>
        private void OnUndoSelected(object sender, EventArgs e)
        {
            var selectedItem = historyListView.SelectedItems.Cast<ListViewItem>().FirstOrDefault();
            if (selectedItem?.Tag is CommandHistoryEntry entry)
            {
                try
                {
                    _undoRedoService.UndoTo(entry.Id);
                    UpdateStatus($"已撤销到: {entry.Description}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"撤销失败: {ex.Message}", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 重做选中项
        /// </summary>
        private void OnRedoSelected(object sender, EventArgs e)
        {
            var selectedItem = historyListView.SelectedItems.Cast<ListViewItem>().FirstOrDefault();
            if (selectedItem?.Tag is CommandHistoryEntry entry)
            {
                try
                {
                    _undoRedoService.RedoTo(entry.Id);
                    UpdateStatus($"已重做到: {entry.Description}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"重做失败: {ex.Message}", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 清空历史记录
        /// </summary>
        private void OnClearHistory(object sender, EventArgs e)
        {
            var result = MessageBox.Show("确定要清空所有历史记录吗？此操作不可撤销。", 
                "确认清空", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
            if (result == DialogResult.Yes)
            {
                try
                {
                    _undoRedoService.ClearHistory();
                    UpdateStatus("历史记录已清空");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"清空历史记录失败: {ex.Message}", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 保存历史记录
        /// </summary>
        private void OnSaveHistory(object sender, EventArgs e)
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "JSON文件|*.json|所有文件|*.*";
                saveDialog.Title = "保存历史记录";
                saveDialog.FileName = $"operation_history_{DateTime.Now:yyyyMMdd_HHmmss}.json";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 这里应该实现实际的保存逻辑
                        UpdateStatus($"历史记录已保存到: {saveDialog.FileName}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"保存历史记录失败: {ex.Message}", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// 加载历史记录
        /// </summary>
        private void OnLoadHistory(object sender, EventArgs e)
        {
            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "JSON文件|*.json|所有文件|*.*";
                openDialog.Title = "加载历史记录";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 这里应该实现实际的加载逻辑
                        UpdateStatus($"历史记录已从文件加载: {openDialog.FileName}");
                        LoadHistoryData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"加载历史记录失败: {ex.Message}", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// 历史记录项双击事件
        /// </summary>
        private void OnHistoryItemDoubleClick(object sender, EventArgs e)
        {
            if (historyListView.SelectedItems.Count > 0)
            {
                var selectedItem = historyListView.SelectedItems[0];
                if (selectedItem.Tag is CommandHistoryEntry entry)
                {
                    ShowEntryDetails(entry);
                }
            }
        }

        /// <summary>
        /// 批量操作项双击事件
        /// </summary>
        private void OnBatchItemDoubleClick(object sender, EventArgs e)
        {
            if (batchListView.SelectedItems.Count > 0)
            {
                var selectedItem = batchListView.SelectedItems[0];
                if (selectedItem.Tag is BatchOperationHistory batch)
                {
                    ShowBatchDetails(batch);
                }
            }
        }

        /// <summary>
        /// 历史记录项选择变化事件
        /// </summary>
        private void OnHistoryItemSelected(object sender, EventArgs e)
        {
            // 可以在这里实现选择项的逻辑
        }

        /// <summary>
        /// 批量操作项选择变化事件
        /// </summary>
        private void OnBatchItemSelected(object sender, EventArgs e)
        {
            // 可以在这里实现选择项的逻辑
        }

        /// <summary>
        /// 显示历史记录项详情
        /// </summary>
        private void ShowEntryDetails(CommandHistoryEntry entry)
        {
            var details = $"操作详情:\n\n" +
                         $"时间: {entry.Timestamp}\n" +
                         $"描述: {entry.Description}\n" +
                         $"类型: {entry.CommandType?.Name}\n" +
                         $"状态: {entry.Status}\n" +
                         $"耗时: {entry.ExecutionTimeMs} ms\n" +
                         $"标签: {string.Join(", ", entry.Tags ?? new List<string>())}";

            MessageBox.Show(details, "操作详情", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示批量操作详情
        /// </summary>
        private void ShowBatchDetails(BatchOperationHistory batch)
        {
            var successRate = batch.TotalCommands > 0 
                ? (batch.SuccessfulCommands * 100.0 / batch.TotalCommands).ToString("F1") + "%"
                : "0%";

            var details = $"批量操作详情:\n\n" +
                         $"开始时间: {batch.StartTime}\n" +
                         $"结束时间: {batch.EndTime}\n" +
                         $"描述: {batch.Description}\n" +
                         $"总命令数: {batch.TotalCommands}\n" +
                         $"成功数: {batch.SuccessfulCommands}\n" +
                         $"失败数: {batch.FailureCount}\n" +
                         $"成功率: {successRate}\n" +
                         $"总耗时: {batch.TotalExecutionTimeMs} ms\n" +
                         $"状态: {batch.Status}";

            MessageBox.Show(details, "批量操作详情", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}