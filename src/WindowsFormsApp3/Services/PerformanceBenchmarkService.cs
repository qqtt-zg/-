using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp3.Interfaces;
using WindowsFormsApp3.Services;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 性能基准测试服务接口
    /// </summary>
    public interface IPerformanceBenchmarkService
    {
        /// <summary>
        /// 运行文件重命名性能测试
        /// </summary>
        /// <param name="testFiles">测试文件列表</param>
        /// <param name="iterations">迭代次数</param>
        /// <returns>测试结果</returns>
        BenchmarkResult RunFileRenameBenchmark(List<string> testFiles, int iterations = 1);

        /// <summary>
        /// 运行批量文件操作性能测试
        /// </summary>
        /// <param name="testFiles">测试文件列表</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="parallel">是否并行处理</param>
        /// <returns>测试结果</returns>
        BenchmarkResult RunBatchOperationBenchmark(List<string> testFiles, string operationType, bool parallel = false);

        /// <summary>
        /// 运行内存压力测试
        /// </summary>
        /// <param name="largeFilesPath">大文件路径</param>
        /// <param name="operation">操作类型</param>
        /// <returns>内存使用结果</returns>
        MemoryStressResult RunMemoryStressTest(string largeFilesPath, string operation);

        /// <summary>
        /// 获取性能基准数据
        /// </summary>
        /// <returns>基准数据列表</returns>
        List<BenchmarkData> GetBenchmarkData();

        /// <summary>
        /// 清理基准测试数据
        /// </summary>
        /// <param name="olderThan">清理早于此时间的记录</param>
        void CleanupBenchmarkData(DateTime olderThan);

        /// <summary>
        /// 导出性能报告
        /// </summary>
        /// <param name="outputPath">输出路径</param>
        /// <returns>报告文件路径</returns>
        string ExportPerformanceReport(string outputPath);
    }

    /// <summary>
    /// 基准测试结果
    /// </summary>
    public class BenchmarkResult
    {
        public string TestName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int TotalFiles { get; set; }
        public int SuccessfulFiles { get; set; }
        public int FailedFiles { get; set; }
        public long TotalBytes { get; set; }
        public double ThroughputMBps { get; set; }
        public long PeakMemoryUsageMB { get; set; }
        public List<OperationMetric> Operations { get; set; } = new List<OperationMetric>();
        public List<string> Errors { get; set; } = new List<string>();
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 内存压力测试结果
    /// </summary>
    public class MemoryStressResult
    {
        public string TestName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public long InitialMemoryMB { get; set; }
        public long PeakMemoryMB { get; set; }
        public long FinalMemoryMB { get; set; }
        public long MemoryDeltaMB { get; set; }
        public int GCCollections { get; set; }
        public bool MemoryLeakDetected { get; set; }
        public List<MemorySnapshot> Snapshots { get; set; } = new List<MemorySnapshot>();
        public List<string> Errors { get; set; } = new List<string>();
    }

    
    /// <summary>
    /// 操作指标
    /// </summary>
    public class OperationMetric
    {
        public string Operation { get; set; }
        public string FilePath { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public long FileSize { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public long MemoryBeforeMB { get; set; }
        public long MemoryAfterMB { get; set; }
    }

    /// <summary>
    /// 基准数据
    /// </summary>
    public class BenchmarkData
    {
        public DateTime Timestamp { get; set; }
        public string TestType { get; set; }
        public string TestName { get; set; }
        public double DurationMs { get; set; }
        public int FileCount { get; set; }
        public double ThroughputMBps { get; set; }
        public long PeakMemoryMB { get; set; }
        public double SuccessRate { get; set; }
        public Dictionary<string, object> Metrics { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 性能基准测试服务实现
    /// </summary>
    public class PerformanceBenchmarkService : IPerformanceBenchmarkService
    {
        private readonly ILogger _logger;
        private readonly IMemoryMonitorService _memoryMonitor;
        private readonly List<BenchmarkData> _benchmarkHistory = new List<BenchmarkData>();
        private readonly object _historyLock = new object();
        private const int MaxHistorySize = 500;

        public PerformanceBenchmarkService(ILogger logger, IMemoryMonitorService memoryMonitor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memoryMonitor = memoryMonitor ?? throw new ArgumentNullException(nameof(memoryMonitor));
        }

        public BenchmarkResult RunFileRenameBenchmark(List<string> testFiles, int iterations = 1)
        {
            var result = new BenchmarkResult
            {
                TestName = $"文件重命名基准测试 ({iterations}次迭代)",
                StartTime = DateTime.Now,
                TotalFiles = testFiles.Count * iterations,
                TotalBytes = testFiles.Sum(f => new FileInfo(f).Length) * iterations
            };

            _logger.LogInformation($"开始文件重命名基准测试 - 文件数: {testFiles.Count}, 迭代次数: {iterations}");

            var memoryBefore = GC.GetTotalMemory(false) / (1024 * 1024);

            try
            {
                for (int iteration = 0; iteration < iterations; iteration++)
                {
                    _logger.LogInformation($"执行第 {iteration + 1}/{iterations} 次迭代");

                    foreach (var filePath in testFiles)
                    {
                        var metric = new OperationMetric
                        {
                            Operation = "重命名",
                            FilePath = filePath,
                            StartTime = DateTime.Now,
                            MemoryBeforeMB = GC.GetTotalMemory(false) / (1024 * 1024)
                        };

                        try
                        {
                            var fileInfo = new FileInfo(filePath);
                            metric.FileSize = fileInfo.Length;

                            // 模拟重命名操作（创建临时重命名）
                            var tempPath = filePath + ".benchmark_temp";
                            File.Move(filePath, tempPath);
                            Thread.Sleep(10); // 模拟处理时间
                            File.Move(tempPath, filePath);

                            metric.EndTime = DateTime.Now;
                            metric.Duration = metric.EndTime - metric.StartTime;
                            metric.Success = true;
                            metric.MemoryAfterMB = GC.GetTotalMemory(false) / (1024 * 1024);

                            result.Operations.Add(metric);
                            result.SuccessfulFiles++;
                        }
                        catch (Exception ex)
                        {
                            metric.EndTime = DateTime.Now;
                            metric.Duration = metric.EndTime - metric.StartTime;
                            metric.Success = false;
                            metric.ErrorMessage = ex.Message;
                            metric.MemoryAfterMB = GC.GetTotalMemory(false) / (1024 * 1024);

                            result.Operations.Add(metric);
                            result.FailedFiles++;
                            result.Errors.Add($"文件 {Path.GetFileName(filePath)} 重命名失败: {ex.Message}");
                        }
                    }

                    // 每次迭代后强制垃圾回收以获得准确的内存测量
                    if (iteration % 10 == 0)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                }

                var memoryAfter = GC.GetTotalMemory(false) / (1024 * 1024);
                result.PeakMemoryUsageMB = Math.Max(memoryBefore, memoryAfter);

                // 计算吞吐量
                var totalSeconds = result.Duration.TotalSeconds;
                if (totalSeconds > 0)
                {
                    result.ThroughputMBps = (result.TotalBytes / (1024.0 * 1024.0)) / totalSeconds;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"基准测试执行异常: {ex.Message}");
                result.Errors.Add($"基准测试异常: {ex.Message}");
            }
            finally
            {
                result.EndTime = DateTime.Now;
                result.Duration = result.EndTime - result.StartTime;

                // 记录基准数据
                RecordBenchmarkData("文件重命名", result.TestName, result);
            }

            _logger.LogInformation($"文件重命名基准测试完成 - 耗时: {result.Duration.TotalMilliseconds:F0}ms, 成功: {result.SuccessfulFiles}, 失败: {result.FailedFiles}, 吞吐量: {result.ThroughputMBps:F2}MB/s");

            return result;
        }

        public BenchmarkResult RunBatchOperationBenchmark(List<string> testFiles, string operationType, bool parallel = false)
        {
            var result = new BenchmarkResult
            {
                TestName = $"批量{operationType}基准测试 ({(parallel ? "并行" : "串行")})",
                StartTime = DateTime.Now,
                TotalFiles = testFiles.Count,
                TotalBytes = testFiles.Sum(f => new FileInfo(f).Length)
            };

            _logger.LogInformation($"开始批量{operationType}基准测试 - 文件数: {testFiles.Count}, 并行: {parallel}");

            var memoryBefore = GC.GetTotalMemory(false) / (1024 * 1024);

            try
            {
                if (parallel)
                {
                    var tasks = testFiles.Select(file => Task.Run(() => ProcessFile(file, operationType, result))).ToArray();
                    Task.WaitAll(tasks);
                }
                else
                {
                    foreach (var file in testFiles)
                    {
                        ProcessFile(file, operationType, result);
                    }
                }

                var memoryAfter = GC.GetTotalMemory(false) / (1024 * 1024);
                result.PeakMemoryUsageMB = Math.Max(memoryBefore, memoryAfter);

                // 计算吞吐量
                var totalSeconds = result.Duration.TotalSeconds;
                if (totalSeconds > 0)
                {
                    result.ThroughputMBps = (result.TotalBytes / (1024.0 * 1024.0)) / totalSeconds;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"批量操作基准测试异常: {ex.Message}");
                result.Errors.Add($"基准测试异常: {ex.Message}");
            }
            finally
            {
                result.EndTime = DateTime.Now;
                result.Duration = result.EndTime - result.StartTime;

                // 记录基准数据
                RecordBenchmarkData($"批量{operationType}", result.TestName, result);
            }

            _logger.LogInformation($"批量{operationType}基准测试完成 - 耗时: {result.Duration.TotalMilliseconds:F0}ms, 成功: {result.SuccessfulFiles}, 失败: {result.FailedFiles}");

            return result;
        }

        public MemoryStressResult RunMemoryStressTest(string largeFilesPath, string operation)
        {
            var result = new MemoryStressResult
            {
                TestName = $"{operation}内存压力测试",
                StartTime = DateTime.Now,
                InitialMemoryMB = GC.GetTotalMemory(false) / (1024 * 1024)
            };

            _logger.LogInformation($"开始内存压力测试 - 路径: {largeFilesPath}, 操作: {operation}");

            try
            {
                var largeFiles = Directory.GetFiles(largeFilesPath, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(f => new FileInfo(f).Length > 10 * 1024 * 1024) // 大于10MB的文件
                    .Take(5) // 最多测试5个大文件
                    .ToList();

                if (largeFiles.Count == 0)
                {
                    result.Errors.Add("未找到适合进行内存压力测试的大文件（>10MB）");
                    return result;
                }

                var gcCollectionsBefore = new
                {
                    Gen0 = GC.CollectionCount(0),
                    Gen1 = GC.CollectionCount(1),
                    Gen2 = GC.CollectionCount(2)
                };

                // 开始内存监控
                _memoryMonitor.StartMonitoring(1000, 1024, 2048);

                foreach (var filePath in largeFiles)
                {
                    var snapshot = new MemorySnapshot
                    {
                        Timestamp = DateTime.Now,
                        MemoryInfo = _memoryMonitor.GetCurrentMemoryInfo(),
                        Context = $"处理文件: {Path.GetFileName(filePath)}"
                    };

                    result.Snapshots.Add(snapshot);

                    // 记录峰值内存
                    if (snapshot.MemoryInfo.ProcessMemoryMB > result.PeakMemoryMB)
                    {
                        result.PeakMemoryMB = snapshot.MemoryInfo.ProcessMemoryMB;
                    }

                    try
                    {
                        switch (operation.ToLower())
                        {
                            case "读取":
                                ProcessLargeFileRead(filePath);
                                break;
                            case "复制":
                                ProcessLargeFileCopy(filePath);
                                break;
                            case "重命名":
                                ProcessLargeFileRename(filePath);
                                break;
                            default:
                                throw new ArgumentException($"不支持的操作类型: {operation}");
                        }

                        // 处理后的内存快照
                        var afterSnapshot = new MemorySnapshot
                        {
                            Timestamp = DateTime.Now,
                            MemoryInfo = _memoryMonitor.GetCurrentMemoryInfo(),
                            Context = $"完成处理: {Path.GetFileName(filePath)}"
                        };
                        result.Snapshots.Add(afterSnapshot);
                    }
                    catch (Exception ex)
                    {
                        result.Errors.Add($"处理文件 {Path.GetFileName(filePath)} 失败: {ex.Message}");
                    }
                }

                // 停止内存监控
                _memoryMonitor.StopMonitoring();

                // 强制垃圾回收并测量内存泄漏
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                var gcCollectionsAfter = new
                {
                    Gen0 = GC.CollectionCount(0),
                    Gen1 = GC.CollectionCount(1),
                    Gen2 = GC.CollectionCount(2)
                };

                result.FinalMemoryMB = GC.GetTotalMemory(false) / (1024 * 1024);
                result.MemoryDeltaMB = result.FinalMemoryMB - result.InitialMemoryMB;
                result.GCCollections = (gcCollectionsAfter.Gen0 - gcCollectionsBefore.Gen0) +
                                     (gcCollectionsAfter.Gen1 - gcCollectionsBefore.Gen1) +
                                     (gcCollectionsAfter.Gen2 - gcCollectionsBefore.Gen2);

                // 检测内存泄漏（内存增长超过50MB）
                result.MemoryLeakDetected = result.MemoryDeltaMB > 50;
            }
            catch (Exception ex)
            {
                _logger.LogError($"内存压力测试异常: {ex.Message}");
                result.Errors.Add($"内存压力测试异常: {ex.Message}");
            }
            finally
            {
                result.EndTime = DateTime.Now;
                result.Duration = result.EndTime - result.StartTime;
            }

            _logger.LogInformation($"内存压力测试完成 - 初始内存: {result.InitialMemoryMB}MB, 峰值内存: {result.PeakMemoryMB}MB, 最终内存: {result.FinalMemoryMB}MB, 内存变化: {result.MemoryDeltaMB:+#;-#;0}MB, 垃圾回收次数: {result.GCCollections}");

            return result;
        }

        public List<BenchmarkData> GetBenchmarkData()
        {
            lock (_historyLock)
            {
                return _benchmarkHistory.OrderByDescending(d => d.Timestamp).ToList();
            }
        }

        public void CleanupBenchmarkData(DateTime olderThan)
        {
            lock (_historyLock)
            {
                var itemsToRemove = _benchmarkHistory.Where(d => d.Timestamp < olderThan).ToList();
                foreach (var item in itemsToRemove)
                {
                    _benchmarkHistory.Remove(item);
                }
            }
        }

        public string ExportPerformanceReport(string outputPath)
        {
            try
            {
                var reportData = new
                {
                    GeneratedAt = DateTime.Now,
                    Summary = GenerateSummary(),
                    RecentBenchmarks = GetBenchmarkData().Take(20).ToList(),
                    Recommendations = GenerateRecommendations()
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(reportData, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(outputPath, json);

                _logger.LogInformation($"性能报告已导出: {outputPath}");
                return outputPath;
            }
            catch (Exception ex)
            {
                _logger.LogError($"导出性能报告失败: {ex.Message}");
                return null;
            }
        }

        private void ProcessFile(string filePath, string operationType, BenchmarkResult result)
        {
            var metric = new OperationMetric
            {
                Operation = operationType,
                FilePath = filePath,
                StartTime = DateTime.Now,
                MemoryBeforeMB = GC.GetTotalMemory(false) / (1024 * 1024)
            };

            try
            {
                var fileInfo = new FileInfo(filePath);
                metric.FileSize = fileInfo.Length;

                // 模拟不同的操作
                switch (operationType.ToLower())
                {
                    case "读取":
                        using (var stream = File.OpenRead(filePath))
                        {
                            var buffer = new byte[4096];
                            while (stream.Read(buffer, 0, buffer.Length) > 0)
                            {
                                // 模拟读取操作
                            }
                        }
                        break;
                    case "复制":
                        var copyPath = filePath + ".benchmark_copy";
                        File.Copy(filePath, copyPath, true);
                        File.Delete(copyPath);
                        break;
                    case "重命名":
                        var renamePath = filePath + ".benchmark_rename";
                        File.Move(filePath, renamePath);
                        File.Move(renamePath, filePath);
                        break;
                    default:
                        throw new ArgumentException($"不支持的操作类型: {operationType}");
                }

                metric.EndTime = DateTime.Now;
                metric.Duration = metric.EndTime - metric.StartTime;
                metric.Success = true;
                metric.MemoryAfterMB = GC.GetTotalMemory(false) / (1024 * 1024);

                result.Operations.Add(metric);
                result.SuccessfulFiles++;
            }
            catch (Exception ex)
            {
                metric.EndTime = DateTime.Now;
                metric.Duration = metric.EndTime - metric.StartTime;
                metric.Success = false;
                metric.ErrorMessage = ex.Message;
                metric.MemoryAfterMB = GC.GetTotalMemory(false) / (1024 * 1024);

                result.Operations.Add(metric);
                result.FailedFiles++;
                result.Errors.Add($"文件 {Path.GetFileName(filePath)} {operationType}失败: {ex.Message}");
            }
        }

        private void ProcessLargeFileRead(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                var buffer = new byte[64 * 1024]; // 64KB buffer
                long totalRead = 0;

                while (stream.Read(buffer, 0, buffer.Length) > 0)
                {
                    totalRead += buffer.Length;
                    // 模拟一些处理逻辑
                    if (totalRead % (10 * 1024 * 1024) == 0) // 每10MB检查一次
                    {
                        // 定期检查内存使用
                        var currentMemory = GC.GetTotalMemory(false) / (1024 * 1024);
                        if (currentMemory > 1024) // 如果超过1GB，触发垃圾回收
                        {
                            GC.Collect(0, GCCollectionMode.Optimized);
                        }
                    }
                }
            }
        }

        private void ProcessLargeFileCopy(string filePath)
        {
            var copyPath = Path.Combine(Path.GetDirectoryName(filePath), "benchmark_" + Path.GetFileName(filePath));

            using (var source = File.OpenRead(filePath))
            using (var destination = File.Create(copyPath))
            {
                var buffer = new byte[64 * 1024]; // 64KB buffer
                int bytesRead;

                while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    destination.Write(buffer, 0, bytesRead);
                }
            }

            File.Delete(copyPath);
        }

        private void ProcessLargeFileRename(string filePath)
        {
            var tempPath = filePath + ".benchmark_temp";
            File.Move(filePath, tempPath);
            File.Move(tempPath, filePath);
        }

        private void RecordBenchmarkData(string testType, string testName, BenchmarkResult result)
        {
            lock (_historyLock)
            {
                var data = new BenchmarkData
                {
                    Timestamp = DateTime.Now,
                    TestType = testType,
                    TestName = testName,
                    DurationMs = result.Duration.TotalMilliseconds,
                    FileCount = result.TotalFiles,
                    ThroughputMBps = result.ThroughputMBps,
                    PeakMemoryMB = result.PeakMemoryUsageMB,
                    SuccessRate = result.TotalFiles > 0 ? (double)result.SuccessfulFiles / result.TotalFiles : 0,
                    Metrics = new Dictionary<string, object>
                    {
                        ["FailedFiles"] = result.FailedFiles,
                        ["ErrorCount"] = result.Errors.Count,
                        ["TotalBytes"] = result.TotalBytes
                    }
                };

                _benchmarkHistory.Add(data);

                // 限制历史记录大小
                if (_benchmarkHistory.Count > MaxHistorySize)
                {
                    var itemsToRemove = _benchmarkHistory.Count - MaxHistorySize;
                    _benchmarkHistory.RemoveRange(0, itemsToRemove);
                }
            }
        }

        private object GenerateSummary()
        {
            var data = GetBenchmarkData();
            if (!data.Any())
                return null;

            return new
            {
                TotalTests = data.Count,
                AverageThroughput = data.Average(d => d.ThroughputMBps),
                PeakMemoryUsage = data.Max(d => d.PeakMemoryMB),
                AverageSuccessRate = data.Average(d => d.SuccessRate),
                LastTestDate = data.Max(d => d.Timestamp),
                TestTypes = data.GroupBy(d => d.TestType)
                    .Select(g => new { Type = g.Key, Count = g.Count() })
                    .ToList()
            };
        }

        private List<string> GenerateRecommendations()
        {
            var recommendations = new List<string>();
            var data = GetBenchmarkData();

            if (!data.Any())
            {
                recommendations.Add("暂无基准测试数据，建议先运行性能测试");
                return recommendations;
            }

            // 内存使用建议
            var avgMemory = data.Average(d => d.PeakMemoryMB);
            if (avgMemory > 512)
            {
                recommendations.Add($"平均内存使用较高 ({avgMemory:F1}MB)，建议优化内存管理");
            }

            // 成功率建议
            var avgSuccessRate = data.Average(d => d.SuccessRate);
            if (avgSuccessRate < 0.95)
            {
                recommendations.Add($"操作成功率较低 ({avgSuccessRate:P1})，建议检查错误处理机制");
            }

            // 吞吐量建议
            var avgThroughput = data.Average(d => d.ThroughputMBps);
            if (avgThroughput < 10)
            {
                recommendations.Add($"平均吞吐量较低 ({avgThroughput:F2}MB/s)，建议考虑并行处理");
            }

            return recommendations;
        }
    }
}