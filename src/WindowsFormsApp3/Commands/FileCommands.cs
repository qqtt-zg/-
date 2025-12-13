using System;
using System.IO;
using WindowsFormsApp3.Helpers;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Commands
{
    /// <summary>
    /// 文件重命名命令
    /// </summary>
    public class FileRenameCommand : CommandBase
    {
        private readonly string _sourcePath;
        private readonly string _targetPath;
        private readonly bool _overwrite;
        private string _backupPath;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <param name="overwrite">是否覆盖现有文件</param>
        public FileRenameCommand(string sourcePath, string targetPath, bool overwrite = false)
            : base($"重命名文件: {Path.GetFileName(sourcePath)} -> {Path.GetFileName(targetPath)}")
        {
            _sourcePath = sourcePath ?? throw new ArgumentNullException(nameof(sourcePath));
            _targetPath = targetPath ?? throw new ArgumentNullException(nameof(targetPath));
            _overwrite = overwrite;
        }

        /// <summary>
        /// 执行重命名操作
        /// </summary>
        protected override void OnExecute()
        {
            // 如果目标文件已存在且不允许覆盖，先备份
            if (File.Exists(_targetPath) && !_overwrite)
            {
                _backupPath = Path.Combine(Path.GetDirectoryName(_targetPath),
                    $"backup_{Path.GetFileNameWithoutExtension(_targetPath)}_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(_targetPath)}");
                FileOperationHelper.SafeMoveFile(_targetPath, _backupPath);
            }

            // 执行重命名
            FileOperationHelper.SafeMoveFile(_sourcePath, _targetPath, _overwrite);
        }

        /// <summary>
        /// 撤销重命名操作
        /// </summary>
        protected override void OnUndo()
        {
            // 将文件重命名回原始名称
            FileOperationHelper.SafeMoveFile(_targetPath, _sourcePath);

            // 如果有备份文件，恢复它
            if (!string.IsNullOrEmpty(_backupPath) && File.Exists(_backupPath))
            {
                FileOperationHelper.SafeMoveFile(_backupPath, _targetPath);
            }
        }

        /// <summary>
        /// 检查是否可以撤销
        /// </summary>
        /// <returns>是否可以撤销</returns>
        public override bool CanUndo()
        {
            return base.CanUndo() && File.Exists(_targetPath) && !File.Exists(_sourcePath);
        }
    }

    /// <summary>
    /// 批量文件重命名命令
    /// </summary>
    public class BatchFileRenameCommand : CommandBase
    {
        private readonly FileRenameOperation[] _operations;
        private readonly string[] _backupPaths;

        /// <summary>
        /// 文件重命名操作
        /// </summary>
        public class FileRenameOperation
        {
            public string SourcePath { get; set; }
            public string TargetPath { get; set; }
            public bool Overwrite { get; set; }
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }

            public FileRenameOperation(string sourcePath, string targetPath, bool overwrite = false)
            {
                SourcePath = sourcePath;
                TargetPath = targetPath;
                Overwrite = overwrite;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="operations">重命名操作列表</param>
        public BatchFileRenameCommand(FileRenameOperation[] operations)
            : base($"批量重命名 {operations.Length} 个文件")
        {
            _operations = operations ?? throw new ArgumentNullException(nameof(operations));
            _backupPaths = new string[operations.Length];
        }

        /// <summary>
        /// 执行批量重命名操作
        /// </summary>
        protected override void OnExecute()
        {
            int successCount = 0;
            int failureCount = 0;

            for (int i = 0; i < _operations.Length; i++)
            {
                var operation = _operations[i];
                try
                {
                    // 如果目标文件已存在且不允许覆盖，先备份
                    if (File.Exists(operation.TargetPath) && !operation.Overwrite)
                    {
                        _backupPaths[i] = Path.Combine(Path.GetDirectoryName(operation.TargetPath),
                            $"backup_{Path.GetFileNameWithoutExtension(operation.TargetPath)}_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(operation.TargetPath)}");
                        FileOperationHelper.SafeMoveFile(operation.TargetPath, _backupPaths[i]);
                    }

                    // 执行重命名
                    FileOperationHelper.SafeMoveFile(operation.SourcePath, operation.TargetPath, operation.Overwrite);
                    operation.Success = true;
                    successCount++;
                }
                catch (Exception ex)
                {
                    operation.Success = false;
                    operation.ErrorMessage = ex.Message;
                    failureCount++;
                }
            }

            if (failureCount > 0)
            {
                throw new InvalidOperationException($"批量重命名部分失败：成功 {successCount} 个，失败 {failureCount} 个");
            }
        }

        /// <summary>
        /// 撤销批量重命名操作
        /// </summary>
        protected override void OnUndo()
        {
            // 逆序撤销操作
            for (int i = _operations.Length - 1; i >= 0; i--)
            {
                var operation = _operations[i];
                if (!operation.Success)
                    continue;

                try
                {
                    // 将文件重命名回原始名称
                    FileOperationHelper.SafeMoveFile(operation.TargetPath, operation.SourcePath);

                    // 如果有备份文件，恢复它
                    if (!string.IsNullOrEmpty(_backupPaths[i]) && File.Exists(_backupPaths[i]))
                    {
                        FileOperationHelper.SafeMoveFile(_backupPaths[i], operation.TargetPath);
                    }
                }
                catch (Exception ex)
                {
                    // 记录撤销失败，但继续撤销其他操作
                    System.Diagnostics.Debug.WriteLine($"撤销重命名失败: {operation.TargetPath} -> {operation.SourcePath}, 错误: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 检查是否可以撤销
        /// </summary>
        /// <returns>是否可以撤销</returns>
        public override bool CanUndo()
        {
            if (!base.CanUndo())
                return false;

            // 检查是否至少有一个成功的操作可以撤销
            return Array.Exists(_operations, op => op.Success && File.Exists(op.TargetPath));
        }

        /// <summary>
        /// 获取操作结果摘要
        /// </summary>
        /// <returns>操作结果摘要</returns>
        public string GetOperationSummary()
        {
            int successCount = System.Linq.Enumerable.Count(_operations, op => op.Success);
            int failureCount = _operations.Length - successCount;

            if (failureCount == 0)
                return $"成功重命名 {successCount} 个文件";
            else
                return $"重命名完成：成功 {successCount} 个，失败 {failureCount} 个";
        }
    }

    /// <summary>
    /// 文件复制命令
    /// </summary>
    public class FileCopyCommand : CommandBase
    {
        private readonly string _sourcePath;
        private readonly string _targetPath;
        private readonly bool _overwrite;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <param name="overwrite">是否覆盖现有文件</param>
        public FileCopyCommand(string sourcePath, string targetPath, bool overwrite = false)
            : base($"复制文件: {Path.GetFileName(sourcePath)} -> {Path.GetFileName(targetPath)}")
        {
            _sourcePath = sourcePath ?? throw new ArgumentNullException(nameof(sourcePath));
            _targetPath = targetPath ?? throw new ArgumentNullException(nameof(targetPath));
            _overwrite = overwrite;
        }

        /// <summary>
        /// 执行复制操作
        /// </summary>
        protected override void OnExecute()
        {
            FileOperationHelper.SafeCopyFile(_sourcePath, _targetPath, _overwrite);
        }

        /// <summary>
        /// 撤销复制操作（删除复制的文件）
        /// </summary>
        protected override void OnUndo()
        {
            if (File.Exists(_targetPath))
            {
                File.Delete(_targetPath);
            }
        }

        /// <summary>
        /// 检查是否可以撤销
        /// </summary>
        /// <returns>是否可以撤销</returns>
        public override bool CanUndo()
        {
            return base.CanUndo() && File.Exists(_targetPath);
        }
    }

    /// <summary>
    /// 文件删除命令（移动到回收站或备份）
    /// </summary>
    public class FileDeleteCommand : CommandBase
    {
        private readonly string _filePath;
        private readonly bool _moveToRecycleBin;
        private string _backupPath;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="moveToRecycleBin">是否移动到回收站</param>
        public FileDeleteCommand(string filePath, bool moveToRecycleBin = true)
            : base($"删除文件: {Path.GetFileName(filePath)}")
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            _moveToRecycleBin = moveToRecycleBin;
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        protected override void OnExecute()
        {
            if (_moveToRecycleBin)
            {
                // 移动到回收站（这里简化实现，实际可以使用Shell API）
                _backupPath = Path.Combine(Path.GetTempPath(), "DeletedFiles",
                    Path.GetFileName(_filePath));
                var backupDir = Path.GetDirectoryName(_backupPath);
                IOHelper.EnsureDirectoryExists(backupDir);

                FileOperationHelper.SafeMoveFile(_filePath, _backupPath);
            }
            else
            {
                // 创建备份然后删除
                _backupPath = Path.Combine(Path.GetTempPath(), "Backup",
                    $"{Path.GetFileNameWithoutExtension(_filePath)}_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(_filePath)}");
                var backupDir = Path.GetDirectoryName(_backupPath);
                IOHelper.EnsureDirectoryExists(backupDir);

                FileOperationHelper.SafeCopyFile(_filePath, _backupPath);
                File.Delete(_filePath);
            }
        }

        /// <summary>
        /// 撤销删除操作（恢复文件）
        /// </summary>
        protected override void OnUndo()
        {
            if (!string.IsNullOrEmpty(_backupPath) && File.Exists(_backupPath))
            {
                FileOperationHelper.SafeMoveFile(_backupPath, _filePath);
            }
        }

        /// <summary>
        /// 检查是否可以撤销
        /// </summary>
        /// <returns>是否可以撤销</returns>
        public override bool CanUndo()
        {
            return base.CanUndo() &&
                   !string.IsNullOrEmpty(_backupPath) &&
                   File.Exists(_backupPath) &&
                   !File.Exists(_filePath);
        }
    }
}