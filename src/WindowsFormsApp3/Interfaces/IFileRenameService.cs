using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp3.Models;

namespace WindowsFormsApp3.Interfaces
{
    public interface IFileRenameService
    {
        event EventHandler<FileRenamedEventArgs> FileRenamed;
        event EventHandler<RenameBatchCompletedEventArgs> BatchRenameCompleted;
        event EventHandler<RenameErrorEventArgs> RenameError;

        bool RenameFileImmediately(string sourcePath, string newName);
        bool RenameFileImmediately(FileRenameInfo fileInfo, string exportPath, bool isCopyMode);
        bool RenameFileImmediately(FileRenameInfo fileInfo, string exportPath, bool isCopyMode, PdfProcessingOptions pdfOptions);

        // 异步版本
        Task<bool> RenameFileImmediatelyAsync(string sourcePath, string newName, CancellationToken cancellationToken = default);
        Task<bool> RenameFileImmediatelyAsync(FileRenameInfo fileInfo, string exportPath, bool isCopyMode, CancellationToken cancellationToken = default);
        Task<bool> RenameFileImmediatelyAsync(FileRenameInfo fileInfo, string exportPath, bool isCopyMode, PdfProcessingOptions pdfOptions, CancellationToken cancellationToken = default);
        Task<int> BatchRenameFilesAsync(List<FileRenameInfo> fileInfos, string exportPath, bool isCopyMode, Action<int, int> progressCallback = null, CancellationToken cancellationToken = default);
        Task<int> BatchRenameFilesAsync(List<FileRenameInfo> fileInfos, string exportPath, bool isCopyMode, PdfProcessingOptions pdfOptions, Action<int, int> progressCallback = null, CancellationToken cancellationToken = default);

        // 保留原有方法签名
        Task<bool> BatchRenameFilesAsync(List<FileRenameInfo> renameInfos);
        string GenerateNewFileName(string baseName, string extension, string pattern, int sequenceNumber = 1);
        List<string> ValidateRenamePattern(string pattern);
        
        // 添加新方法声明 - 更新验证方法以支持Excel序号来源判断
        ValidationResult ValidateFileGridInput(FileInfo fileInfo, string material, string orderNumber, string quantity, string serialNumber, bool isSerialNumberFromExcel = false);
        bool AddFileToGrid(FileInfo fileInfo, ProcessedFileData processedData, BindingList<FileRenameInfo> bindingList);

        /// <summary>
        /// 处理文件名冲突
        /// </summary>
        /// <param name="existingPath">已存在的文件路径</param>
        /// <param name="newPath">新的文件路径</param>
        /// <returns>解决冲突后的文件路径</returns>
        string HandleFileNameConflict(string existingPath, string newPath);
    }

    public class FileRenamedEventArgs : EventArgs
    {
        public string OldPath { get; set; }
        public string NewPath { get; set; }
    }

    public class RenameBatchCompletedEventArgs : EventArgs
    {
        public int TotalFiles { get; set; }
        public int SuccessfulRenames { get; set; }
        public int FailedRenames { get; set; }
    }

    public class RenameErrorEventArgs : EventArgs
    {
        public string FilePath { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
    }
}