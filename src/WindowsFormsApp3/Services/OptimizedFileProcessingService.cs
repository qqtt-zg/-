using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp3.Interfaces;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 优化文件处理服务接口
    /// </summary>
    public interface IOptimizedFileProcessingService
    {
        /// <summary>
        /// 内存优化的文件批量重命名
        /// </summary>
        /// <param name="files">文件列表</param>
        /// <param name="nameGenerator">新名称生成器</param>
        /// <param name="options">处理选项</param>
        /// <param name="progress">进度报告</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        Task<BatchProcessResult> OptimizedBatchRenameAsync(
            List<FileOperation> files,
            Func<string, string> nameGenerator,
            OptimizedProcessingOptions options = null,
            IProgress<BatchProgress> progress = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 流式大文件复制
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="destinationPath">目标文件路径</param>
        /// <param name="bufferSize">缓冲区大小</param>
        /// <param name="progress">进度报告</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>复制结果</returns>
        Task<FileOperationResult> StreamCopyFileAsync(
            string sourcePath,
            string destinationPath,
            int bufferSize = 64 * 1024,
            IProgress<long> progress = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 分块文件处理
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="chunkSize">块大小</param>
        /// <param name="processor">块处理器</param>
        /// <param name="progress">进度报告</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        Task<FileOperationResult> ProcessFileInChunksAsync(
            string filePath,
            int chunkSize = 4 * 1024 * 1024, // 4MB
            Func<byte[], long, bool> processor = null,
            IProgress<long> progress = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 内存压力监控的批量操作
        /// </summary>
        /// <param name="operations">操作列表</param>
        /// <param name="memoryThresholdMB">内存阈值</param>
        /// <param name="progress">进度报告</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        Task<BatchProcessResult> MemoryAwareBatchProcessAsync(
            List<FileOperation> operations,
            long memoryThresholdMB = 512,
            IProgress<BatchProgress> progress = null,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 文件操作
    /// </summary>
    public class FileOperation
    {
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public string OperationType { get; set; } // "rename", "copy", "move"
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 优化处理选项
    /// </summary>
    public class OptimizedProcessingOptions
    {
        public int MaxConcurrency { get; set; } = Environment.ProcessorCount;
        public long MemoryThresholdMB { get; set; } = 512;
        public bool EnableMemoryMonitoring { get; set; } = true;
        public int BatchSize { get; set; } = 50;
        public int BufferSize { get; set; } = 64 * 1024; // 64KB
        public bool ForceGarbageCollection { get; set; } = true;
        public int GCInterval { get; set; } = 10; // 每处理N个文件进行一次GC
    }

    /// <summary>
    /// 文件操作结果
    /// </summary>
    public class FileOperationResult
    {
        public bool Success { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public string ErrorMessage { get; set; }
        public long FileSize { get; set; }
        public TimeSpan ProcessingTime { get; set; }
        public long BytesProcessed { get; set; }
        public Dictionary<string, object> Metrics { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 批量处理进度
    /// </summary>
    public class BatchProgress
    {
        public int CompletedCount { get; set; }
        public int TotalCount { get; set; }
        public long CompletedBytes { get; set; }
        public long TotalBytes { get; set; }
        public double ProgressPercentage => TotalCount > 0 ? (double)CompletedCount / TotalCount * 100 : 0;
        public double BytesProgressPercentage => TotalBytes > 0 ? (double)CompletedBytes / TotalBytes * 100 : 0;
        public string CurrentFile { get; set; }
        public long CurrentMemoryUsageMB { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public TimeSpan EstimatedTimeRemaining { get; set; }
    }

    /// <summary>
    /// 批量处理结果
    /// </summary>
    public class BatchProcessResult
    {
        public bool Success { get; set; }
        public int TotalFiles { get; set; }
        public int SuccessfulFiles { get; set; }
        public int FailedFiles { get; set; }
        public long TotalBytes { get; set; }
        public long ProcessedBytes { get; set; }
        public TimeSpan TotalTime { get; set; }
        public double ThroughputMBps { get; set; }
        public List<FileOperationResult> Results { get; set; } = new List<FileOperationResult>();
        public List<string> Errors { get; set; } = new List<string>();
        public Dictionary<string, object> Metrics { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 优化文件处理服务实现
    /// </summary>
    public class OptimizedFileProcessingService : IOptimizedFileProcessingService
    {
        private readonly ILogger _logger;
        private readonly IMemoryMonitorService _memoryMonitor;
        private readonly IErrorRecoveryService _errorRecovery;

        public OptimizedFileProcessingService(
            ILogger logger,
            IMemoryMonitorService memoryMonitor,
            IErrorRecoveryService errorRecovery)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memoryMonitor = memoryMonitor ?? throw new ArgumentNullException(nameof(memoryMonitor));
            _errorRecovery = errorRecovery ?? throw new ArgumentNullException(nameof(errorRecovery));
        }

        public async Task<BatchProcessResult> OptimizedBatchRenameAsync(
            List<FileOperation> files,
            Func<string, string> nameGenerator,
            OptimizedProcessingOptions options = null,
            IProgress<BatchProgress> progress = null,
            CancellationToken cancellationToken = default)
        {
            options = options ?? new OptimizedProcessingOptions();
            var result = new BatchProcessResult();
            var startTime = DateTime.Now;

            _logger.LogInformation($"开始优化批量重命名 - 文件数: {files.Count}, 并发度: {options.MaxConcurrency}, 内存阈值: {options.MemoryThresholdMB}MB");

            result.TotalFiles = files.Count;
            result.TotalBytes = files.Sum(f => new FileInfo(f.SourcePath).Length);

            var batchProgress = new BatchProgress
            {
                TotalCount = files.Count,
                TotalBytes = result.TotalBytes
            };

            try
            {
                // 启用内存监控
                if (options.EnableMemoryMonitoring)
                {
                    _memoryMonitor.StartMonitoring(2000, options.MemoryThresholdMB, options.MemoryThresholdMB * 2);
                }

                // 分批处理以控制内存使用
                var batches = files.Chunk(options.BatchSize);
                var processedCount = 0;

                foreach (var batch in batches)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    // 检查内存使用
                    if (options.EnableMemoryMonitoring)
                    {
                        var currentMemory = GC.GetTotalMemory(false) / (1024 * 1024);
                        batchProgress.CurrentMemoryUsageMB = currentMemory;

                        if (currentMemory > options.MemoryThresholdMB)
                        {
                            _logger.LogWarning($"内存使用过高 ({currentMemory}MB)，触发垃圾回收");
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            GC.Collect();

                            // 如果内存仍然过高，降低并发度
                            if (GC.GetTotalMemory(false) / (1024 * 1024) > options.MemoryThresholdMB)
                            {
                                options.MaxConcurrency = Math.Max(1, options.MaxConcurrency / 2);
                                _logger.LogInformation($"降低并发度至: {options.MaxConcurrency}");
                            }
                        }
                    }

                    // 处理当前批次
                    var semaphore = new SemaphoreSlim(options.MaxConcurrency, options.MaxConcurrency);
                    var tasks = batch.Select(async file =>
                    {
                        await semaphore.WaitAsync(cancellationToken);
                        try
                        {
                            return await ProcessFileOperationAsync(file, nameGenerator, options, cancellationToken);
                        }
                        finally
                        {
                            semaphore.Release();
                            processedCount++;

                            // 更新进度
                            batchProgress.CompletedCount = processedCount;
                            batchProgress.CompletedBytes = result.Results.Where(r => r.Success).Sum(r => r.BytesProcessed);
                            batchProgress.ElapsedTime = DateTime.Now - startTime;
                            batchProgress.CurrentFile = file.SourcePath;

                            // 估算剩余时间
                            if (processedCount > 0)
                            {
                                var avgTimePerFile = batchProgress.ElapsedTime.TotalMilliseconds / processedCount;
                                var remainingFiles = files.Count - processedCount;
                                batchProgress.EstimatedTimeRemaining = TimeSpan.FromMilliseconds(avgTimePerFile * remainingFiles);
                            }

                            progress?.Report(batchProgress);
                        }
                    });

                    var batchResults = await Task.WhenAll(tasks);
                    result.Results.AddRange(batchResults);

                    // 统计结果
                    result.SuccessfulFiles += batchResults.Count(r => r.Success);
                    result.FailedFiles += batchResults.Count(r => !r.Success);
                    result.ProcessedBytes += batchResults.Where(r => r.Success).Sum(r => r.BytesProcessed);

                    // 定期垃圾回收
                    if (options.ForceGarbageCollection && processedCount % options.GCInterval == 0)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }

                    // 批次间短暂休息，让系统有时间回收资源
                    await Task.Delay(100, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("批量重命名操作已取消");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"优化批量重命名异常: {ex.Message}");
                result.Errors.Add($"批量处理异常: {ex.Message}");
            }
            finally
            {
                if (options.EnableMemoryMonitoring)
                {
                    _memoryMonitor.StopMonitoring();
                }

                result.TotalTime = DateTime.Now - startTime;

                // 计算吞吐量
                if (result.TotalTime.TotalSeconds > 0)
                {
                    result.ThroughputMBps = (result.ProcessedBytes / (1024.0 * 1024.0)) / result.TotalTime.TotalSeconds;
                }

                result.Success = result.FailedFiles == 0;

                _logger.LogInformation($"优化批量重命名完成 - 成功: {result.SuccessfulFiles}, 失败: {result.FailedFiles}, 耗时: {result.TotalTime.TotalMilliseconds:F0}ms, 吞吐量: {result.ThroughputMBps:F2}MB/s");
            }

            return result;
        }

        public async Task<FileOperationResult> StreamCopyFileAsync(
            string sourcePath,
            string destinationPath,
            int bufferSize = 64 * 1024,
            IProgress<long> progress = null,
            CancellationToken cancellationToken = default)
        {
            var result = new FileOperationResult
            {
                SourcePath = sourcePath,
                DestinationPath = destinationPath
            };

            var startTime = DateTime.Now;

            try
            {
                var fileInfo = new FileInfo(sourcePath);
                result.FileSize = fileInfo.Length;

                // 确保目标目录存在
                var destinationDir = Path.GetDirectoryName(destinationPath);
                if (!string.IsNullOrEmpty(destinationDir) && !Directory.Exists(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir);
                }

                // 使用流式复制以减少内存使用
                using (var sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, useAsync: true))
                using (var destinationStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, useAsync: true))
                {
                    var buffer = new byte[bufferSize];
                    long totalBytesRead = 0;
                    int bytesRead;

                    while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                    {
                        await destinationStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                        totalBytesRead += bytesRead;
                        result.BytesProcessed = totalBytesRead;

                        // 报告进度
                        progress?.Report(totalBytesRead);

                        // 定期检查内存使用
                        if (totalBytesRead % (10 * 1024 * 1024) == 0) // 每10MB检查一次
                        {
                            var currentMemory = GC.GetTotalMemory(false) / (1024 * 1024);
                            if (currentMemory > 1024) // 如果内存使用超过1GB
                            {
                                GC.Collect(0, GCCollectionMode.Optimized);
                            }
                        }
                    }

                    // 确保所有数据都写入磁盘
                    await destinationStream.FlushAsync(cancellationToken);
                }

                result.Success = true;
                result.ProcessingTime = DateTime.Now - startTime;

                _logger.LogDebug($"流式复制完成 - 源文件: {sourcePath}, 大小: {result.FileSize / (1024.0 * 1024.0):F2}MB, 耗时: {result.ProcessingTime.TotalMilliseconds:F0}ms");
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.ProcessingTime = DateTime.Now - startTime;

                _logger.LogError($"流式复制失败 - 源文件: {sourcePath}, 错误: {ex.Message}");
            }

            return result;
        }

        public async Task<FileOperationResult> ProcessFileInChunksAsync(
            string filePath,
            int chunkSize = 4 * 1024 * 1024,
            Func<byte[], long, bool> processor = null,
            IProgress<long> progress = null,
            CancellationToken cancellationToken = default)
        {
            var result = new FileOperationResult
            {
                SourcePath = filePath
            };

            var startTime = DateTime.Now;

            try
            {
                var fileInfo = new FileInfo(filePath);
                result.FileSize = fileInfo.Length;

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 8192, useAsync: true))
                {
                    var buffer = new byte[chunkSize];
                    long totalBytesRead = 0;
                    int bytesRead;
                    int chunkIndex = 0;

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var chunkData = new byte[bytesRead];
                        Array.Copy(buffer, 0, chunkData, 0, bytesRead);

                        // 处理数据块
                        bool processResult = true;
                        if (processor != null)
                        {
                            processResult = processor(chunkData, chunkIndex * chunkSize);
                        }

                        if (!processResult)
                        {
                            result.ErrorMessage = $"数据块处理失败，索引: {chunkIndex}";
                            break;
                        }

                        totalBytesRead += bytesRead;
                        result.BytesProcessed = totalBytesRead;
                        chunkIndex++;

                        // 报告进度
                        progress?.Report(totalBytesRead);

                        // 每处理一个数据块后进行垃圾回收以控制内存
                        if (chunkIndex % 5 == 0)
                        {
                            GC.Collect(0, GCCollectionMode.Optimized);
                        }
                    }
                }

                result.Success = string.IsNullOrEmpty(result.ErrorMessage);
                result.ProcessingTime = DateTime.Now - startTime;

                _logger.LogDebug($"分块处理完成 - 文件: {filePath}, 大小: {result.FileSize / (1024.0 * 1024.0):F2}MB, 耗时: {result.ProcessingTime.TotalMilliseconds:F0}ms");
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.ProcessingTime = DateTime.Now - startTime;

                _logger.LogError($"分块处理失败 - 文件: {filePath}, 错误: {ex.Message}");
            }

            return result;
        }

        public async Task<BatchProcessResult> MemoryAwareBatchProcessAsync(
            List<FileOperation> operations,
            long memoryThresholdMB = 512,
            IProgress<BatchProgress> progress = null,
            CancellationToken cancellationToken = default)
        {
            var result = new BatchProcessResult();
            var startTime = DateTime.Now;

            _logger.LogInformation($"开始内存感知批量处理 - 操作数: {operations.Count}, 内存阈值: {memoryThresholdMB}MB");

            result.TotalFiles = operations.Count;
            result.TotalBytes = operations.Sum(op => new FileInfo(op.SourcePath).Length);

            var batchProgress = new BatchProgress
            {
                TotalCount = operations.Count,
                TotalBytes = result.TotalBytes
            };

            try
            {
                // 启动内存监控
                _memoryMonitor.StartMonitoring(1000, memoryThresholdMB / 2, memoryThresholdMB);

                var processedCount = 0;

                foreach (var operation in operations)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    // 检查内存使用
                    var currentMemory = GC.GetTotalMemory(false) / (1024 * 1024);
                    batchProgress.CurrentMemoryUsageMB = currentMemory;

                    if (currentMemory > memoryThresholdMB)
                    {
                        _logger.LogWarning($"内存使用超过阈值 ({currentMemory}MB > {memoryThresholdMB}MB)，执行垃圾回收");

                        // 强制垃圾回收
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();

                        // 等待一段时间让内存释放
                        await Task.Delay(1000, cancellationToken);

                        // 再次检查内存
                        currentMemory = GC.GetTotalMemory(false) / (1024 * 1024);
                        if (currentMemory > memoryThresholdMB)
                        {
                            _logger.LogError($"垃圾回收后内存仍然过高 ({currentMemory}MB)，暂停处理");
                            await Task.Delay(5000, cancellationToken);
                        }
                    }

                    // 处理单个操作
                    var operationResult = await ProcessSingleOperationAsync(operation, cancellationToken);
                    result.Results.Add(operationResult);

                    if (operationResult.Success)
                    {
                        result.SuccessfulFiles++;
                        result.ProcessedBytes += operationResult.BytesProcessed;
                    }
                    else
                    {
                        result.FailedFiles++;
                        result.Errors.Add(operationResult.ErrorMessage);
                    }

                    processedCount++;

                    // 更新进度
                    batchProgress.CompletedCount = processedCount;
                    batchProgress.CompletedBytes = result.ProcessedBytes;
                    batchProgress.ElapsedTime = DateTime.Now - startTime;
                    batchProgress.CurrentFile = operation.SourcePath;

                    if (processedCount > 0)
                    {
                        var avgTimePerOperation = batchProgress.ElapsedTime.TotalMilliseconds / processedCount;
                        var remainingOperations = operations.Count - processedCount;
                        batchProgress.EstimatedTimeRemaining = TimeSpan.FromMilliseconds(avgTimePerOperation * remainingOperations);
                    }

                    progress?.Report(batchProgress);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("内存感知批量处理已取消");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"内存感知批量处理异常: {ex.Message}");
                result.Errors.Add($"批量处理异常: {ex.Message}");
            }
            finally
            {
                _memoryMonitor.StopMonitoring();

                result.TotalTime = DateTime.Now - startTime;

                if (result.TotalTime.TotalSeconds > 0)
                {
                    result.ThroughputMBps = (result.ProcessedBytes / (1024.0 * 1024.0)) / result.TotalTime.TotalSeconds;
                }

                result.Success = result.FailedFiles == 0;

                _logger.LogInformation($"内存感知批量处理完成 - 成功: {result.SuccessfulFiles}, 失败: {result.FailedFiles}, 耗时: {result.TotalTime.TotalMilliseconds:F0}ms");
            }

            return result;
        }

        private Task<FileOperationResult> ProcessFileOperationAsync(
            FileOperation operation,
            Func<string, string> nameGenerator,
            OptimizedProcessingOptions options,
            CancellationToken cancellationToken)
        {
            var result = new FileOperationResult
            {
                SourcePath = operation.SourcePath,
                DestinationPath = operation.DestinationPath
            };

            var startTime = DateTime.Now;

            try
            {
                // 生成新的文件名
                var newName = nameGenerator(operation.SourcePath);
                var newPath = Path.Combine(Path.GetDirectoryName(operation.DestinationPath), newName);

                // 使用错误恢复服务执行操作
                bool success = false;
                if (operation.OperationType.ToLower() == "rename")
                {
                    success = _errorRecovery.TryRecoverFileOperation(
                        "重命名文件",
                        operation.SourcePath,
                        () => {
                            File.Move(operation.SourcePath, newPath);
                        },
                        maxRetries: 3);
                }
                else if (operation.OperationType.ToLower() == "copy")
                {
                    success = _errorRecovery.TryRecoverFileOperation(
                        "复制文件",
                        operation.SourcePath,
                        () => {
                            File.Copy(operation.SourcePath, newPath, true);
                        },
                        maxRetries: 3);
                }

                result.Success = success;
                result.DestinationPath = newPath;
                result.BytesProcessed = new FileInfo(operation.SourcePath).Length;
                result.ProcessingTime = DateTime.Now - startTime;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.ProcessingTime = DateTime.Now - startTime;
            }

            return Task.FromResult(result);
        }

        private async Task<FileOperationResult> ProcessSingleOperationAsync(
            FileOperation operation,
            CancellationToken cancellationToken)
        {
            var result = new FileOperationResult
            {
                SourcePath = operation.SourcePath,
                DestinationPath = operation.DestinationPath
            };

            var startTime = DateTime.Now;

            try
            {
                var fileInfo = new FileInfo(operation.SourcePath);
                result.FileSize = fileInfo.Length;

                // 根据操作类型执行相应的处理
                switch (operation.OperationType.ToLower())
                {
                    case "rename":
                        File.Move(operation.SourcePath, operation.DestinationPath);
                        break;
                    case "copy":
                        await StreamCopyFileAsync(operation.SourcePath, operation.DestinationPath, 32 * 1024, null, cancellationToken);
                        break;
                    default:
                        throw new ArgumentException($"不支持的操作类型: {operation.OperationType}");
                }

                result.Success = true;
                result.BytesProcessed = result.FileSize;
                result.ProcessingTime = DateTime.Now - startTime;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.ProcessingTime = DateTime.Now - startTime;
            }

            return result;
        }
    }

    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 将集合分割成指定大小的块
        /// </summary>
        public static IEnumerable<T[]> Chunk<T>(this IEnumerable<T> source, int size)
        {
            var list = source as T[] ?? source.ToArray();
            for (int i = 0; i < list.Length; i += size)
            {
                yield return list.Skip(i).Take(size).ToArray();
            }
        }
    }
}