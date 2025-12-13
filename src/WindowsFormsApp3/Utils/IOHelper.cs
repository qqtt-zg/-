using System;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// IO操作辅助类，提供文件和目录操作的公共方法
    /// </summary>
    public static class IOHelper
    {
        /// <summary>
        /// 确保目录存在，如果不存在则创建
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <exception cref="ArgumentException">路径为空或无效时抛出</exception>
        /// <exception cref="SecurityException">没有权限时抛出</exception>
        public static void EnsureDirectoryExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("目录路径不能为空", nameof(path));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                LogHelper.Debug($"创建目录: {path}");
            }
        }

        /// <summary>
        /// 异步确保目录存在，如果不存在则创建
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>异步任务</returns>
        /// <exception cref="ArgumentException">路径为空或无效时抛出</exception>
        /// <exception cref="SecurityException">没有权限时抛出</exception>
        public static async Task EnsureDirectoryExistsAsync(string path, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("目录路径不能为空", nameof(path));

            if (!Directory.Exists(path))
            {
                await Task.Run(() =>
                {
                    Directory.CreateDirectory(path);
                    LogHelper.Debug($"异步创建目录: {path}");
                }, cancellationToken);
            }
        }

        /// <summary>
        /// 安全地移动文件，自动处理文件名冲突
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="destinationPath">目标文件路径</param>
        /// <returns>实际的目标文件路径</returns>
        /// <exception cref="FileNotFoundException">源文件不存在时抛出</exception>
        /// <exception cref="IOException">IO操作失败时抛出</exception>
        public static string SafeMoveFile(string sourcePath, string destinationPath)
        {
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException($"源文件不存在: {sourcePath}");

            // 处理文件名冲突
            string finalPath = HandleFileNameConflict(destinationPath);

            File.Move(sourcePath, finalPath);
            LogHelper.Debug($"文件移动: {sourcePath} -> {finalPath}");

            return finalPath;
        }

        /// <summary>
        /// 安全地复制文件，自动处理文件名冲突
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="destinationPath">目标文件路径</param>
        /// <param name="overwrite">是否覆盖现有文件</param>
        /// <returns>实际的目标文件路径</returns>
        /// <exception cref="FileNotFoundException">源文件不存在时抛出</exception>
        /// <exception cref="IOException">IO操作失败时抛出</exception>
        public static string SafeCopyFile(string sourcePath, string destinationPath, bool overwrite = false)
        {
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException($"源文件不存在: {sourcePath}");

            // 处理文件名冲突（除非允许覆盖）
            string finalPath = overwrite ? destinationPath : HandleFileNameConflict(destinationPath);

            File.Copy(sourcePath, finalPath, overwrite);
            LogHelper.Debug($"文件复制: {sourcePath} -> {finalPath}");

            return finalPath;
        }

        /// <summary>
        /// 处理文件名冲突，生成唯一的文件名
        /// </summary>
        /// <param name="filePath">原始文件路径</param>
        /// <returns>无冲突的文件路径</returns>
        public static string HandleFileNameConflict(string filePath)
        {
            if (!File.Exists(filePath))
                return filePath;

            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);

            int counter = 1;
            string newFilePath;

            do
            {
                newFilePath = Path.Combine(directory, $"{fileName}_{counter}{extension}");
                counter++;
            }
            while (File.Exists(newFilePath));

            LogHelper.Debug($"解决文件名冲突: {filePath} -> {newFilePath}");
            return newFilePath;
        }

        /// <summary>
        /// 检查文件是否可访问
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件是否可访问</returns>
        public static bool IsFileAccessible(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            try
            {
                using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    fileStream.Close();
                }
                return true;
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取文件大小的友好显示格式
        /// </summary>
        /// <param name="bytes">字节数</param>
        /// <returns>格式化的文件大小</returns>
        public static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }
}