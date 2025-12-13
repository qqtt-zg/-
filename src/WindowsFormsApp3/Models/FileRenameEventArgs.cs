using System;

namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// 文件重命名事件参数类
    /// </summary>
    public class FileRenameEventArgs : EventArgs
    {
        /// <summary>
        /// 获取文件重命名信息
        /// </summary>
        public FileRenameInfo FileInfo { get; }

        /// <summary>
        /// 获取错误消息（如果有）
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 初始化 FileRenameEventArgs 类的新实例
        /// </summary>
        /// <param name="fileInfo">文件重命名信息</param>
        public FileRenameEventArgs(FileRenameInfo fileInfo)
        {
            FileInfo = fileInfo;
        }
    }
}