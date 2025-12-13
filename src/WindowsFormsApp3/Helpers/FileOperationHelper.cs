using System;
using System.IO;
using System.Security;
using System.Text.RegularExpressions;
using System.Linq;
using WindowsFormsApp3.Exceptions;

namespace WindowsFormsApp3.Helpers
{
    /// <summary>
    /// 文件操作帮助类
    /// </summary>
    public static class FileOperationHelper
    {
        /// <summary>
        /// 安全地移动文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="destinationPath">目标文件路径</param>
        /// <param name="overwrite">是否覆盖现有文件</param>
        /// <exception cref="FileRenameException">文件重命名失败时抛出</exception>
        public static void SafeMoveFile(string sourcePath, string destinationPath, bool overwrite = false)
        {
            if (string.IsNullOrEmpty(sourcePath))
                throw new ArgumentException("源文件路径不能为空", nameof(sourcePath));

            if (string.IsNullOrEmpty(destinationPath))
                throw new ArgumentException("目标文件路径不能为空", nameof(destinationPath));

            if (!File.Exists(sourcePath))
                throw new FileRenameException(sourcePath, destinationPath, "源文件不存在");

            try
            {
                // 确保目标目录存在
                var destinationDir = Path.GetDirectoryName(destinationPath);
                if (!string.IsNullOrEmpty(destinationDir) && !Directory.Exists(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir);
                }

                // 检查目标文件是否存在
                if (File.Exists(destinationPath) && !overwrite)
                {
                    throw new FileRenameException(sourcePath, destinationPath, "目标文件已存在且不允许覆盖");
                }

                // 检查文件是否被占用
                ValidateFileAccess(sourcePath, destinationPath);

                // 执行移动操作
                File.Move(sourcePath, destinationPath);
            }
            catch (FileRenameException)
            {
                // 重新抛出我们自己的异常
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new FileRenameException(sourcePath, destinationPath, $"没有文件访问权限: {ex.Message}", ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new FileRenameException(sourcePath, destinationPath, $"目录不存在: {ex.Message}", ex);
            }
            catch (IOException ex)
            {
                throw new FileRenameException(sourcePath, destinationPath, $"文件IO操作失败: {ex.Message}", ex);
            }
            catch (SecurityException ex)
            {
                throw new FileRenameException(sourcePath, destinationPath, $"安全权限不足: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new FileRenameException(sourcePath, destinationPath, $"文件重命名失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 安全地复制文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="destinationPath">目标文件路径</param>
        /// <param name="overwrite">是否覆盖现有文件</param>
        /// <exception cref="FileRenameException">文件复制失败时抛出</exception>
        public static void SafeCopyFile(string sourcePath, string destinationPath, bool overwrite = false)
        {
            if (string.IsNullOrEmpty(sourcePath))
                throw new ArgumentException("源文件路径不能为空", nameof(sourcePath));

            if (string.IsNullOrEmpty(destinationPath))
                throw new ArgumentException("目标文件路径不能为空", nameof(destinationPath));

            if (!File.Exists(sourcePath))
                throw new FileRenameException(sourcePath, destinationPath, "源文件不存在");

            try
            {
                // 确保目标目录存在
                var destinationDir = Path.GetDirectoryName(destinationPath);
                if (!string.IsNullOrEmpty(destinationDir) && !Directory.Exists(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir);
                }

                // 检查目标文件是否存在
                if (File.Exists(destinationPath) && !overwrite)
                {
                    throw new FileRenameException(sourcePath, destinationPath, "目标文件已存在且不允许覆盖");
                }

                // 执行复制操作
                File.Copy(sourcePath, destinationPath, overwrite);
            }
            catch (FileRenameException)
            {
                // 重新抛出我们自己的异常
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new FileRenameException(sourcePath, destinationPath, $"没有文件访问权限: {ex.Message}", ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new FileRenameException(sourcePath, destinationPath, $"目录不存在: {ex.Message}", ex);
            }
            catch (IOException ex)
            {
                throw new FileRenameException(sourcePath, destinationPath, $"文件IO操作失败: {ex.Message}", ex);
            }
            catch (SecurityException ex)
            {
                throw new FileRenameException(sourcePath, destinationPath, $"安全权限不足: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new FileRenameException(sourcePath, destinationPath, $"文件复制失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 验证文件名是否有效
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>验证结果</returns>
        public static FileValidationResult ValidateFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return new FileValidationResult(false, "文件名不能为空");

            try
            {
                // 检查是否包含无效字符
                var invalidChars = Path.GetInvalidFileNameChars();
                var foundInvalidChars = fileName.Where(c => invalidChars.Contains(c)).ToArray();
                if (foundInvalidChars.Length > 0)
                {
                    return new FileValidationResult(false, $"文件名包含无效字符: {string.Join(", ", foundInvalidChars.Select(c => $"'{c}'"))}。请移除这些字符后重试。");
                }

                // 检查是否为保留名称（Windows）
                var reservedNames = new[] { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };
                var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName).ToUpperInvariant();
                if (System.Linq.Enumerable.Contains(reservedNames, nameWithoutExtension))
                {
                    return new FileValidationResult(false, $"文件名不能使用保留名称: {nameWithoutExtension}");
                }

                // 检查文件名长度
                if (fileName.Length > 255)
                {
                    return new FileValidationResult(false, "文件名长度不能超过255个字符");
                }

                return new FileValidationResult(true, "文件名有效");
            }
            catch (Exception ex)
            {
                return new FileValidationResult(false, $"文件名验证失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证文件路径是否有效
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>验证结果</returns>
        public static FileValidationResult ValidateFilePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return new FileValidationResult(false, "文件路径不能为空");

            try
            {
                // 检查路径格式
                var fullPath = Path.GetFullPath(path);

                // 检查路径长度
                if (fullPath.Length > 260) // MAX_PATH in Windows
                {
                    return new FileValidationResult(false, "文件路径长度不能超过260个字符");
                }

                // 检查是否包含无效字符
                var invalidChars = Path.GetInvalidPathChars();
                var foundInvalidChars = path.Where(c => invalidChars.Contains(c)).ToArray();
                if (foundInvalidChars.Length > 0)
                {
                    return new FileValidationResult(false, $"文件路径包含无效字符: {string.Join(", ", foundInvalidChars.Select(c => $"'{c}'"))}。请移除这些字符后重试。");
                }

                return new FileValidationResult(true, "文件路径有效");
            }
            catch (ArgumentException ex)
            {
                return new FileValidationResult(false, $"文件路径格式无效: {ex.Message}");
            }
            catch (Exception ex)
            {
                return new FileValidationResult(false, $"文件路径验证失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 生成唯一的文件名
        /// </summary>
        /// <param name="directory">目录</param>
        /// <param name="fileName">文件名</param>
        /// <returns>唯一的文件名</returns>
        public static string GenerateUniqueFileName(string directory, string fileName)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentException("目录不能为空", nameof(directory));

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("文件名不能为空", nameof(fileName));

            var path = Path.Combine(directory, fileName);

            if (!File.Exists(path))
                return path;

            var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var extension = Path.GetExtension(fileName);
            var counter = 1;

            do
            {
                var newFileName = $"{nameWithoutExtension}_{counter}{extension}";
                path = Path.Combine(directory, newFileName);
                counter++;
            }
            while (File.Exists(path));

            return path;
        }

        /// <summary>
        /// 清理文件名，移除或替换无效字符
        /// </summary>
        /// <param name="fileName">原始文件名</param>
        /// <param name="replacementChar">替换字符</param>
        /// <returns>清理后的文件名</returns>
        public static string SanitizeFileName(string fileName, char replacementChar = '_')
        {
            if (string.IsNullOrEmpty(fileName))
                return fileName;

            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = fileName;

            foreach (var invalidChar in invalidChars)
            {
                sanitized = sanitized.Replace(invalidChar, replacementChar);
            }

            // 移除或替换其他可能有问题的字符
            sanitized = Regex.Replace(sanitized, @"[^\w\s-]", replacementChar.ToString());

            // 移除连续的替换字符
            sanitized = Regex.Replace(sanitized, $@"{replacementChar}+", replacementChar.ToString());

            // 确保文件名不以点开头或结尾
            sanitized = sanitized.Trim('.');

            // 确保文件名不为空
            if (string.IsNullOrEmpty(sanitized))
            {
                sanitized = $"file_{DateTime.Now:yyyyMMdd_HHmmss}";
            }

            return sanitized;
        }

        /// <summary>
        /// 验证文件访问权限
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="destinationPath">目标文件路径</param>
        private static void ValidateFileAccess(string sourcePath, string destinationPath)
        {
            try
            {
                // 检查源文件是否可以被读取
                using (var sourceStream = File.Open(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    // 如果可以打开文件，说明有读取权限
                }

                // 检查目标路径是否可以写入
                var destinationDir = Path.GetDirectoryName(destinationPath);
                if (!string.IsNullOrEmpty(destinationDir))
                {
                    var testFile = Path.Combine(destinationDir, $"test_{Guid.NewGuid():N}.tmp");
                    try
                    {
                        using (var testStream = File.Create(testFile))
                        {
                            // 测试写入权限
                        }
                        File.Delete(testFile);
                    }
                    catch (Exception ex)
                    {
                        throw new FileRenameException(sourcePath, destinationPath, $"没有目标目录的写入权限: {ex.Message}", ex);
                    }
                }
            }
            catch (FileRenameException)
            {
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new FileRenameException(sourcePath, destinationPath, $"文件访问权限不足: {ex.Message}", ex);
            }
            catch (IOException ex)
            {
                throw new FileRenameException(sourcePath, destinationPath, $"文件可能被占用: {ex.Message}", ex);
            }
        }
    }

    /// <summary>
    /// 文件验证结果
    /// </summary>
    public class FileValidationResult
    {
        public bool IsValid { get; }
        public string Message { get; }

        public FileValidationResult(bool isValid, string message)
        {
            IsValid = isValid;
            Message = message;
        }
    }
}