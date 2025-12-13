using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using WindowsFormsApp3.Interfaces;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 内存监控服务接口
    /// </summary>
    public interface IMemoryMonitorService
    {
        /// <summary>
        /// 获取当前内存使用情况
        /// </summary>
        /// <returns>内存信息</returns>
        MemoryInfo GetCurrentMemoryInfo();

        /// <summary>
        /// 开始监控内存使用
        /// </summary>
        /// <param name="checkIntervalMs">检查间隔（毫秒）</param>
        /// <param name="warningThresholdMB">警告阈值（MB）</param>
        /// <param name="criticalThresholdMB">严重阈值（MB）</param>
        void StartMonitoring(int checkIntervalMs = 5000, long warningThresholdMB = 512, long criticalThresholdMB = 1024);

        /// <summary>
        /// 停止监控内存使用
        /// </summary>
        void StopMonitoring();

        /// <summary>
        /// 获取内存使用历史
        /// </summary>
        /// <returns>内存使用历史</returns>
        List<MemorySnapshot> GetMemoryHistory();

        /// <summary>
        /// 清理内存使用历史
        /// </summary>
        /// <param name="olderThan">清理早于此时间的记录</param>
        void CleanupHistory(DateTime olderThan);

        /// <summary>
        /// 触发垃圾回收
        /// </summary>
        /// <param name="generation">垃圾回收代数</param>
        /// <param name="mode">垃圾回收模式</param>
        void TriggerGarbageCollection(int generation = 0, GCCollectionMode mode = GCCollectionMode.Default);

        /// <summary>
        /// 内存使用警告事件
        /// </summary>
        event EventHandler<MemoryWarningEventArgs> MemoryWarning;
    }

    /// <summary>
    /// 内存信息
    /// </summary>
    public class MemoryInfo
    {
        public long TotalMemoryMB { get; set; }
        public long AvailableMemoryMB { get; set; }
        public long UsedMemoryMB { get; set; }
        public long ProcessMemoryMB { get; set; }
        public double MemoryUsagePercent { get; set; }
        public long GCMemoryMB { get; set; }
        public int Gen0Collections { get; set; }
        public int Gen1Collections { get; set; }
        public int Gen2Collections { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 内存快照
    /// </summary>
    public class MemorySnapshot
    {
        public DateTime Timestamp { get; set; }
        public MemoryInfo MemoryInfo { get; set; }
        public string Context { get; set; }
    }

    /// <summary>
    /// 内存警告事件参数
    /// </summary>
    public class MemoryWarningEventArgs : EventArgs
    {
        public MemoryWarningLevel Level { get; set; }
        public MemoryInfo MemoryInfo { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// 内存警告级别
    /// </summary>
    public enum MemoryWarningLevel
    {
        Normal,
        Warning,
        Critical
    }

    /// <summary>
    /// 内存监控服务实现
    /// </summary>
    public class MemoryMonitorService : IMemoryMonitorService
    {
        private readonly ILogger _logger;
        private System.Timers.Timer _monitorTimer;
        private readonly List<MemorySnapshot> _memoryHistory = new List<MemorySnapshot>();
        private readonly object _historyLock = new object();
        private long _warningThresholdMB = 512;
        private long _criticalThresholdMB = 1024;
        private const int MaxHistorySize = 1000;

        public event EventHandler<MemoryWarningEventArgs> MemoryWarning;

        public MemoryMonitorService(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public MemoryInfo GetCurrentMemoryInfo()
        {
            var process = Process.GetCurrentProcess();
            var memCounter = new PerformanceCounter("Memory", "Available MBytes");
            var processMemCounter = new PerformanceCounter("Process", "Working Set", process.ProcessName);

            long totalMemory = GetTotalPhysicalMemory();
            long availableMemory = (long)memCounter.NextValue();
            long usedMemory = totalMemory - availableMemory;
            long processMemory = (long)processMemCounter.NextValue() / (1024 * 1024); // Convert to MB

            // Get GC memory info
            long gcMemory = GC.GetTotalMemory(false) / (1024 * 1024);

            return new MemoryInfo
            {
                TotalMemoryMB = totalMemory,
                AvailableMemoryMB = availableMemory,
                UsedMemoryMB = usedMemory,
                ProcessMemoryMB = processMemory,
                MemoryUsagePercent = (double)usedMemory / totalMemory * 100,
                GCMemoryMB = gcMemory,
                Gen0Collections = GC.CollectionCount(0),
                Gen1Collections = GC.CollectionCount(1),
                Gen2Collections = GC.CollectionCount(2),
                Timestamp = DateTime.Now
            };
        }

        public void StartMonitoring(int checkIntervalMs = 5000, long warningThresholdMB = 512, long criticalThresholdMB = 1024)
        {
            StopMonitoring(); // Stop any existing monitoring

            _warningThresholdMB = warningThresholdMB;
            _criticalThresholdMB = criticalThresholdMB;

            _monitorTimer = new System.Timers.Timer();
            _monitorTimer.Interval = checkIntervalMs;
            _monitorTimer.Elapsed += MonitorTimer_Elapsed;
            _monitorTimer.Start();

            _logger.LogInformation($"内存监控已启动 - 检查间隔: {checkIntervalMs}ms, 警告阈值: {warningThresholdMB}MB, 严重阈值: {criticalThresholdMB}MB");
        }

        public void StopMonitoring()
        {
            if (_monitorTimer != null)
            {
                _monitorTimer.Stop();
                _monitorTimer.Dispose();
                _monitorTimer = null;
                _logger.LogInformation("内存监控已停止");
            }
        }

        public List<MemorySnapshot> GetMemoryHistory()
        {
            lock (_historyLock)
            {
                return _memoryHistory.OrderByDescending(s => s.Timestamp).ToList();
            }
        }

        public void CleanupHistory(DateTime olderThan)
        {
            lock (_historyLock)
            {
                var itemsToRemove = _memoryHistory.Where(s => s.Timestamp < olderThan).ToList();
                foreach (var item in itemsToRemove)
                {
                    _memoryHistory.Remove(item);
                }
            }
        }

        public void TriggerGarbageCollection(int generation = 0, GCCollectionMode mode = GCCollectionMode.Default)
        {
            var beforeMemory = GC.GetTotalMemory(false) / (1024 * 1024);

            GC.Collect(generation, mode);
            GC.WaitForPendingFinalizers();

            var afterMemory = GC.GetTotalMemory(true) / (1024 * 1024);
            var savedMemory = beforeMemory - afterMemory;

            _logger.LogInformation($"垃圾回收完成 - 代数: {generation}, 释放内存: {savedMemory}MB (前: {beforeMemory}MB, 后: {afterMemory}MB)");
        }

        private void MonitorTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                var memoryInfo = GetCurrentMemoryInfo();
                var snapshot = new MemorySnapshot
                {
                    Timestamp = memoryInfo.Timestamp,
                    MemoryInfo = memoryInfo,
                    Context = "定期监控"
                };

                // 记录到历史
                RecordMemorySnapshot(snapshot);

                // 检查阈值并触发警告
                CheckMemoryThresholds(memoryInfo);

                // 如果内存使用过高，建议垃圾回收
                if (memoryInfo.MemoryUsagePercent > 80 || memoryInfo.ProcessMemoryMB > _warningThresholdMB)
                {
                    _logger.LogWarning($"内存使用较高 - 进程内存: {memoryInfo.ProcessMemoryMB}MB, 系统内存使用率: {memoryInfo.MemoryUsagePercent:F1}%");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"内存监控发生错误: {ex.Message}");
            }
        }

        private void CheckMemoryThresholds(MemoryInfo memoryInfo)
        {
            MemoryWarningLevel level = MemoryWarningLevel.Normal;
            string message = string.Empty;

            if (memoryInfo.ProcessMemoryMB >= _criticalThresholdMB)
            {
                level = MemoryWarningLevel.Critical;
                message = $"进程内存使用严重过高: {memoryInfo.ProcessMemoryMB}MB (阈值: {_criticalThresholdMB}MB)";
            }
            else if (memoryInfo.ProcessMemoryMB >= _warningThresholdMB)
            {
                level = MemoryWarningLevel.Warning;
                message = $"进程内存使用较高: {memoryInfo.ProcessMemoryMB}MB (阈值: {_warningThresholdMB}MB)";
            }
            else if (memoryInfo.MemoryUsagePercent >= 90)
            {
                level = MemoryWarningLevel.Critical;
                message = $"系统内存使用严重过高: {memoryInfo.MemoryUsagePercent:F1}%";
            }
            else if (memoryInfo.MemoryUsagePercent >= 75)
            {
                level = MemoryWarningLevel.Warning;
                message = $"系统内存使用较高: {memoryInfo.MemoryUsagePercent:F1}%";
            }

            if (level != MemoryWarningLevel.Normal)
            {
                var args = new MemoryWarningEventArgs
                {
                    Level = level,
                    MemoryInfo = memoryInfo,
                    Message = message
                };

                OnMemoryWarning(args);
            }
        }

        private void RecordMemorySnapshot(MemorySnapshot snapshot)
        {
            lock (_historyLock)
            {
                _memoryHistory.Add(snapshot);

                // 限制历史记录大小
                if (_memoryHistory.Count > MaxHistorySize)
                {
                    var itemsToRemove = _memoryHistory.Count - MaxHistorySize;
                    _memoryHistory.RemoveRange(0, itemsToRemove);
                }
            }
        }

        protected virtual void OnMemoryWarning(MemoryWarningEventArgs e)
        {
            MemoryWarning?.Invoke(this, e);

            // 记录到日志
            var logLevel = e.Level == MemoryWarningLevel.Critical ? "严重" : "警告";
            _logger.LogWarning($"内存{logLevel}警告: {e.Message}");
        }

        private long GetTotalPhysicalMemory()
        {
            try
            {
                var memStatus = new MEMORYSTATUSEX();
                memStatus.Initialize();
                if (GlobalMemoryStatusEx(ref memStatus))
                {
                    return (long)(memStatus.ullTotalPhys / (1024 * 1024)); // Convert to MB
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"获取总物理内存失败: {ex.Message}");
            }

            // 如果获取失败，返回默认值
            return 8192; // 8GB
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;

            public void Initialize()
            {
                dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);
    }
}