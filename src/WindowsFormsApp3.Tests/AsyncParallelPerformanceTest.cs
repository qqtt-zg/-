using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WindowsFormsApp3.Models;
using WindowsFormsApp3.Services;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Tests
{
    /// <summary>
    /// 异步和并行处理性能测试
    /// </summary>
    public class AsyncParallelPerformanceTest
    {
        private readonly string _testDirectory;
        private readonly List<FileRenameInfo> _testFiles;
        private readonly FileRenameService _fileRenameService;
        private readonly BatchProcessingService _batchProcessingService;

        public AsyncParallelPerformanceTest()
        {
            // 设置测试目录
            _testDirectory = Path.Combine(Path.GetTempPath(), "AsyncParallelTest");

            // 初始化服务
            _fileRenameService = new FileRenameService();
            _batchProcessingService = new BatchProcessingService();

            // 准备测试文件
            _testFiles = new List<FileRenameInfo>();
            CreateTestFiles();
        }

        /// <summary>
        /// 创建测试文件
        /// </summary>
        private void CreateTestFiles()
        {
            // 确保测试目录存在
            IOHelper.EnsureDirectoryExists(_testDirectory);

            // 创建100个测试文件
            for (int i = 1; i <= 100; i++)
            {
                string fileName = $"test_file_{i:D3}.txt";
                string filePath = Path.Combine(_testDirectory, fileName);

                // 创建测试文件内容
                File.WriteAllText(filePath, $"测试文件 {i} 的内容");

                // 创建FileRenameInfo对象
                var fileInfo = new FileRenameInfo
                {
                    OriginalName = fileName,
                    FullPath = filePath,
                    OrderNumber = $"ORD{i:D4}",
                    Material = $"MAT{i % 10 + 1}",
                    Quantity = $"{(i % 5) + 1}",
                    Dimensions = $"100x200",
                    SerialNumber = $"{i:D2}",
                    NewName = $"processed_file_{i:D3}.txt"
                };

                _testFiles.Add(fileInfo);
            }
        }

        /// <summary>
        /// 测试同步文件重命名性能
        /// </summary>
        public Task<PerformanceResult> TestSyncFileRename()
        {
            var sw = Stopwatch.StartNew();
            int successCount = 0;
            int errorCount = 0;

            string exportPath = Path.Combine(_testDirectory, "sync_export");
            IOHelper.EnsureDirectoryExists(exportPath);

            // 同步处理所有文件
            foreach (var file in _testFiles)
            {
                try
                {
                    bool success = _fileRenameService.RenameFileImmediately(
                        file, exportPath, false);
                    if (success)
                        successCount++;
                    else
                        errorCount++;
                }
                catch (Exception ex)
                {
                    errorCount++;
                    Console.WriteLine($"同步处理文件 {file.OriginalName} 时出错: {ex.Message}");
                }
            }

            sw.Stop();

            return Task.FromResult(new PerformanceResult
            {
                MethodName = "同步文件重命名",
                TotalFiles = _testFiles.Count,
                SuccessCount = successCount,
                ErrorCount = errorCount,
                ElapsedTimeMs = sw.ElapsedMilliseconds,
                FilesPerSecond = successCount / sw.Elapsed.TotalSeconds
            });
        }

        /// <summary>
        /// 测试异步文件重命名性能
        /// </summary>
        public async Task<PerformanceResult> TestAsyncFileRename()
        {
            var sw = Stopwatch.StartNew();
            int successCount = 0;
            int errorCount = 0;

            string exportPath = Path.Combine(_testDirectory, "async_export");
            await IOHelper.EnsureDirectoryExistsAsync(exportPath);

            // 异步处理所有文件
            var tasks = _testFiles.Select(async file =>
            {
                try
                {
                    bool success = await _fileRenameService.RenameFileImmediatelyAsync(
                        file, exportPath, false);
                    return success ? 1 : 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"异步处理文件 {file.OriginalName} 时出错: {ex.Message}");
                    return 0;
                }
            });

            var results = await Task.WhenAll(tasks);
            successCount = results.Sum();
            errorCount = _testFiles.Count - successCount;

            sw.Stop();

            return new PerformanceResult
            {
                MethodName = "异步文件重命名",
                TotalFiles = _testFiles.Count,
                SuccessCount = successCount,
                ErrorCount = errorCount,
                ElapsedTimeMs = sw.ElapsedMilliseconds,
                FilesPerSecond = successCount / sw.Elapsed.TotalSeconds
            };
        }

        /// <summary>
        /// 测试并行批量处理性能
        /// </summary>
        public async Task<PerformanceResult> TestParallelBatchProcessing()
        {
            var sw = Stopwatch.StartNew();

            string exportPath = Path.Combine(_testDirectory, "parallel_export");
            await IOHelper.EnsureDirectoryExistsAsync(exportPath);

            // 使用并行批量处理
            var result = await _batchProcessingService.ProcessFilesAsync(
                _testFiles,
                exportPath,
                false, // 不使用复制模式
                batchSize: 10,
                maxDegreeOfParallelism: Environment.ProcessorCount);

            sw.Stop();

            return new PerformanceResult
            {
                MethodName = "并行批量处理",
                TotalFiles = _testFiles.Count,
                SuccessCount = result.SuccessCount,
                ErrorCount = result.ErrorCount,
                ElapsedTimeMs = sw.ElapsedMilliseconds,
                FilesPerSecond = result.SuccessCount / sw.Elapsed.TotalSeconds
            };
        }

        /// <summary>
        /// 运行所有性能测试
        /// </summary>
        public async Task<List<PerformanceResult>> RunAllTests()
        {
            var results = new List<PerformanceResult>();

            Console.WriteLine("开始异步和并行处理性能测试...\n");

            // 1. 测试同步处理
            Console.WriteLine("1. 测试同步文件重命名...");
            var syncResult = await TestSyncFileRename();
            results.Add(syncResult);
            PrintResult(syncResult);

            // 2. 测试异步处理
            Console.WriteLine("\n2. 测试异步文件重命名...");
            var asyncResult = await TestAsyncFileRename();
            results.Add(asyncResult);
            PrintResult(asyncResult);

            // 3. 测试并行处理
            Console.WriteLine("\n3. 测试并行批量处理...");
            var parallelResult = await TestParallelBatchProcessing();
            results.Add(parallelResult);
            PrintResult(parallelResult);

            // 4. 比较结果
            Console.WriteLine("\n=== 性能对比总结 ===");
            CompareResults(results);

            return results;
        }

        /// <summary>
        /// 打印测试结果
        /// </summary>
        private void PrintResult(PerformanceResult result)
        {
            Console.WriteLine($"方法: {result.MethodName}");
            Console.WriteLine($"总文件数: {result.TotalFiles}");
            Console.WriteLine($"成功数: {result.SuccessCount}");
            Console.WriteLine($"失败数: {result.ErrorCount}");
            Console.WriteLine($"耗时: {result.ElapsedTimeMs} ms");
            Console.WriteLine($"处理速度: {result.FilesPerSecond:F2} 文件/秒");
            Console.WriteLine($"成功率: {(double)result.SuccessCount / result.TotalFiles * 100:F1}%");
        }

        /// <summary>
        /// 比较测试结果
        /// </summary>
        private void CompareResults(List<PerformanceResult> results)
        {
            if (results.Count < 2) return;

            var baseline = results[0]; // 同步处理作为基准
            Console.WriteLine($"以 '{baseline.MethodName}' 为基准:");

            for (int i = 1; i < results.Count; i++)
            {
                var current = results[i];
                double speedImprovement = (current.FilesPerSecond / baseline.FilesPerSecond - 1) * 100;
                double timeReduction = (1 - (double)current.ElapsedTimeMs / baseline.ElapsedTimeMs) * 100;

                Console.WriteLine($"\n{current.MethodName} vs {baseline.MethodName}:");
                Console.WriteLine($"  速度提升: {speedImprovement:+F1}%");
                Console.WriteLine($"  时间减少: {timeReduction:+F1}%");
                Console.WriteLine($"  效率比: {current.FilesPerSecond / baseline.FilesPerSecond:F2}x");
            }
        }

        /// <summary>
        /// 清理测试文件
        /// </summary>
        public void Cleanup()
        {
            try
            {
                if (Directory.Exists(_testDirectory))
                {
                    Directory.Delete(_testDirectory, true);
                    Console.WriteLine($"已清理测试目录: {_testDirectory}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"清理测试目录时出错: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 性能测试结果
    /// </summary>
    public class PerformanceResult
    {
        public string MethodName { get; set; }
        public int TotalFiles { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public long ElapsedTimeMs { get; set; }
        public double FilesPerSecond { get; set; }
    }
}