using System; using WindowsFormsApp3.Models;
using System.Collections.Generic;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 文件重命名服务接口，定义文件重命名相关的核心功能
    /// 提供文件重命名、批量处理、文件名生成和冲突处理等功能
    /// </summary>
    public interface IFileRenameService
    {
        /// <summary>
        /// 立即重命名单个文件，执行实际的文件系统操作
        /// </summary>
        /// <param name="fileInfo">包含原始文件名、新文件名和其他重命名信息的对象</param>
        /// <param name="exportPath">文件重命名后的保存路径</param>
        /// <param name="isCopyMode">是否为复制模式，为true时保留原始文件</param>
        /// <returns>重命名操作是否成功完成</returns>
        /// <exception cref="ArgumentNullException">当fileInfo为null或exportPath为空时抛出</exception>
        /// <exception cref="IOException">当文件操作失败时抛出</exception>
        bool RenameFileImmediately(FileRenameInfo fileInfo, string exportPath, bool isCopyMode);

        /// <summary>
        /// 批量重命名多个文件，支持进度报告和错误处理
        /// </summary>
        /// <param name="fileInfos">包含多个文件重命名信息的列表</param>
        /// <param name="exportPath">文件重命名后的保存路径</param>
        /// <param name="isCopyMode">是否为复制模式，为true时保留原始文件</param>
        /// <param name="progressCallback">可选的进度回调函数，接收当前进度和总进度</param>
        /// <returns>成功重命名的文件数量</returns>
        /// <exception cref="ArgumentNullException">当fileInfos为null或exportPath为空时抛出</exception>
        int BatchRenameFiles(List<FileRenameInfo> fileInfos, string exportPath, bool isCopyMode, Action<int, int> progressCallback = null);

        /// <summary>
        /// 根据文件重命名信息和指定的分隔符生成新的文件名
        /// </summary>
        /// <param name="fileInfo">包含原始文件名、前缀、后缀等信息的对象</param>
        /// <param name="separator">用于连接文件名各部分的分隔符字符串</param>
        /// <returns>生成的新文件名（不包含路径）</returns>
        /// <exception cref="ArgumentNullException">当fileInfo为null时抛出</exception>
        string GenerateNewFileName(FileRenameInfo fileInfo, string separator);

        /// <summary>
        /// 检查并处理指定文件路径的命名冲突
        /// </summary>
        /// <param name="filePath">需要检查的文件完整路径</param>
        /// <returns>处理冲突后的唯一文件路径，如果无冲突则返回原路径</returns>
        /// <exception cref="ArgumentNullException">当filePath为null或空时抛出</exception>
        string HandleFileNameConflict(string filePath);

        /// <summary>
        /// 当单个文件重命名操作成功完成时触发的事件
        /// 提供文件重命名信息和操作结果
        /// </summary>
        event EventHandler<FileRenameEventArgs> FileRenamedSuccessfully;

        /// <summary>
        /// 当单个文件重命名操作失败时触发的事件
        /// 提供文件重命名信息和错误详情
        /// </summary>
        event EventHandler<FileRenameEventArgs> FileRenameFailed;

        /// <summary>
        /// 当批量重命名操作的进度发生变化时触发的事件
        /// 提供当前已处理文件数量和总文件数量
        /// </summary>
        event EventHandler<BatchRenameProgressEventArgs> BatchRenameProgressChanged;
    }

    /// <summary>
    /// 文件重命名事件参数
    /// </summary>
    public class FileRenameEventArgs : EventArgs
    {
        /// <summary>
        /// 获取或设置文件重命名信息
        /// </summary>
        public FileRenameInfo FileInfo { get; set; }

        /// <summary>
        /// 获取或设置错误信息（如果有）
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileInfo">文件重命名信息</param>
        public FileRenameEventArgs(FileRenameInfo fileInfo)
        {
            FileInfo = fileInfo;
        }
    }

    /// <summary>
    /// 批量重命名进度事件参数
    /// </summary>
    public class BatchRenameProgressEventArgs : EventArgs
    {
        /// <summary>
        /// 当前已处理的文件数量
        /// </summary>
        public int CurrentCount { get; set; }

        /// <summary>
        /// 总文件数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 当前正在处理的文件信息
        /// </summary>
        public FileRenameInfo CurrentFileInfo { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentCount">当前已处理的文件数量</param>
        /// <param name="totalCount">总文件数量</param>
        /// <param name="currentFileInfo">当前正在处理的文件信息</param>
        public BatchRenameProgressEventArgs(int currentCount, int totalCount, FileRenameInfo currentFileInfo = null)
        {
            CurrentCount = currentCount;
            TotalCount = totalCount;
            CurrentFileInfo = currentFileInfo;
        }
    }
}