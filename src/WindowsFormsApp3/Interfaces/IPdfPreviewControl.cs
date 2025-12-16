using System;

namespace WindowsFormsApp3.Interfaces
{
    /// <summary>
    /// PDF预览控件接口
    /// 定义PDF预览控件的基本功能
    /// </summary>
    public interface IPdfPreviewControl
    {
        /// <summary>
        /// 当前页码（从1开始）
        /// </summary>
        int CurrentPage { get; }

        /// <summary>
        /// 总页数
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// 当前PDF文件路径
        /// </summary>
        string CurrentFilePath { get; }

        /// <summary>
        /// 页面变化事件
        /// </summary>
        event EventHandler PageChanged;

        /// <summary>
        /// PDF加载完成事件
        /// </summary>
        event EventHandler PageLoaded;

        /// <summary>
        /// PDF加载错误事件
        /// </summary>
        event EventHandler<string> LoadError;

        /// <summary>
        /// 加载PDF文件
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>是否加载成功</returns>
        bool LoadPdf(string filePath);

        /// <summary>
        /// 重新加载当前PDF
        /// </summary>
        void Refresh();

        /// <summary>
        /// 导航到第一页
        /// </summary>
        void FirstPage();

        /// <summary>
        /// 导航到上一页
        /// </summary>
        void PreviousPage();

        /// <summary>
        /// 导航到下一页
        /// </summary>
        void NextPage();

        /// <summary>
        /// 导航到最后一页
        /// </summary>
        void LastPage();

        /// <summary>
        /// 导航到指定页
        /// </summary>
        /// <param name="pageNumber">页码（从1开始）</param>
        /// <returns>是否导航成功</returns>
        System.Threading.Tasks.Task<bool> GoToPage(int pageNumber);
    }
}