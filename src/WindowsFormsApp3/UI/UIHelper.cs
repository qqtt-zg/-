using System;
using System.Windows.Forms;
using Ookii.Dialogs.WinForms;

namespace WindowsFormsApp3.UI
{
    /// <summary>
    /// UI辅助类，提供常用的UI操作方法
    /// </summary>
    public static class UIHelper
    {
        /// <summary>
        /// 显示信息消息框
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="title">消息框标题</param>
        public static void ShowInfoMessage(string message, string title = "提示")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示错误消息框
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="title">消息框标题</param>
        public static void ShowErrorMessage(string message, string title = "错误")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示警告消息框
        /// </summary>
        /// <param name="message">警告消息</param>
        /// <param name="title">消息框标题</param>
        public static void ShowWarningMessage(string message, string title = "警告")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 显示确认消息框
        /// </summary>
        /// <param name="message">确认消息</param>
        /// <param name="title">消息框标题</param>
        /// <returns>用户选择的结果</returns>
        public static DialogResult ShowConfirmMessage(string message, string title = "确认")
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 显示确认对话框
        /// </summary>
        /// <param name="message">确认消息</param>
        /// <param name="title">消息框标题</param>
        /// <returns>用户选择的结果</returns>
        public static DialogResult ShowConfirmDialog(string message, string title = "确认")
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 显示成功消息框
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <param name="title">消息框标题</param>
        public static void ShowSuccessMessage(string message, string title = "成功")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示文件夹浏览器对话框
        /// </summary>
        /// <param name="description">对话框描述</param>
        /// <param name="initialPath">初始路径</param>
        /// <returns>用户选择的文件夹路径，如果取消则返回空字符串</returns>
        public static string ShowFolderBrowserDialog(string description = "请选择文件夹", string initialPath = null)
        {
            using (var dialog = new VistaFolderBrowserDialog())
            {
                dialog.Description = description;
                dialog.UseDescriptionForTitle = true;

                if (!string.IsNullOrEmpty(initialPath) && System.IO.Directory.Exists(initialPath))
                {
                    dialog.SelectedPath = initialPath;
                }

                return dialog.ShowDialog() == DialogResult.OK ? dialog.SelectedPath : string.Empty;
            }
        }

        /// <summary>
        /// 显示文件打开对话框
        /// </summary>
        /// <param name="filter">文件过滤器</param>
        /// <param name="title">对话框标题</param>
        /// <param name="multiselect">是否允许选择多个文件</param>
        /// <returns>用户选择的文件路径数组，如果取消则返回null</returns>
        public static string[] ShowOpenFileDialog(string filter = "所有文件 (*.*)|*.*", string title = "打开文件", bool multiselect = false)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = filter;
                dialog.Title = title;
                dialog.Multiselect = multiselect;

                return dialog.ShowDialog() == DialogResult.OK ? dialog.FileNames : null;
            }
        }

        /// <summary>
        /// 显示文件保存对话框
        /// </summary>
        /// <param name="filter">文件过滤器</param>
        /// <param name="title">对话框标题</param>
        /// <param name="defaultFileName">默认文件名</param>
        /// <returns>用户选择的保存路径，如果取消则返回空字符串</returns>
        public static string ShowSaveFileDialog(string filter = "所有文件 (*.*)|*.*", string title = "保存文件", string defaultFileName = null)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = filter;
                dialog.Title = title;
                if (!string.IsNullOrEmpty(defaultFileName))
                {
                    dialog.FileName = defaultFileName;
                }

                return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : string.Empty;
            }
        }

        /// <summary>
        /// 在UI线程上执行操作
        /// </summary>
        /// <param name="control">UI控件</param>
        /// <param name="action">要执行的操作</param>
        public static void InvokeOnUIThread(this Control control, MethodInvoker action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// 安全地更新控件属性
        /// </summary>
        /// <typeparam name="T">控件类型</typeparam>
        /// <param name="control">控件实例</param>
        /// <param name="action">更新操作</param>
        public static void SafeUpdate<T>(this T control, Action<T> action) where T : Control
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action(() => action(control)));
            }
            else
            {
                action(control);
            }
        }
    }
}