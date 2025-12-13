using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3.UI
{
    /// <summary>
    /// 进度条助手类，封装进度显示相关的UI操作
    /// </summary>
    public static class ProgressHelper
    {
        /// <summary>
        /// 显示进度对话框并执行任务
        /// </summary>
        /// <param name="owner">拥有进度对话框的窗口</param>
        /// <param name="taskAction">要执行的任务</param>
        /// <param name="taskName">任务名称</param>
        /// <param name="onTaskCompleted">任务完成回调</param>
        /// <param name="onError">错误处理回调</param>
        public static void ShowProgressDialog(Form owner, Action<IProgressReporter> taskAction, string taskName = "处理中...", 
            Action onTaskCompleted = null, Action<Exception> onError = null)
        {
            using (var progressForm = new ProgressForm())
            {
                progressForm.Title = taskName;
                progressForm.CancelEnabled = true;

                // 创建进度报告器
                var progressReporter = new ProgressReporter(progressForm);

                // 启动任务
                var task = Task.Run(() =>
                {
                    try
                    {
                        taskAction(progressReporter);
                    }
                    catch (Exception ex)
                    {
                        // 如果有错误回调，则调用错误回调；否则显示错误消息
                        if (onError != null)
                        {
                            owner.Invoke((MethodInvoker)(() => onError(ex)));
                        }
                        else
                        {
                            owner.Invoke((MethodInvoker)(() =>
                            {
                                MessageBox.Show("任务执行出错: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }));
                        }
                    }
                });

                // 显示进度对话框
                var dialogResult = progressForm.ShowDialog(owner);

                // 检查任务是否已完成
                if (task.IsCompleted && dialogResult != DialogResult.Cancel && onTaskCompleted != null)
                {
                    onTaskCompleted();
                }
            }
        }

        /// <summary>
        /// 显示无取消选项的进度对话框
        /// </summary>
        /// <param name="owner">拥有进度对话框的窗口</param>
        /// <param name="taskAction">要执行的任务</param>
        /// <param name="taskName">任务名称</param>
        /// <param name="onTaskCompleted">任务完成回调</param>
        public static void ShowNonCancelableProgressDialog(Form owner, Action<IProgressReporter> taskAction, string taskName = "处理中...", 
            Action onTaskCompleted = null)
        {
            ShowProgressDialog(owner, taskAction, taskName, onTaskCompleted, null);
        }

        /// <summary>
        /// 进度报告器接口
        /// </summary>
        public interface IProgressReporter
        {
            /// <summary>
            /// 更新进度
            /// </summary>
            /// <param name="percentage">进度百分比(0-100)</param>
            /// <param name="statusText">状态文本</param>
            void ReportProgress(int percentage, string statusText = "");

            /// <summary>
            /// 报告当前正在处理的项目
            /// </summary>
            /// <param name="currentItem">当前项目名称</param>
            /// <param name="currentIndex">当前索引</param>
            /// <param name="totalCount">总项目数</param>
            void ReportCurrentItem(string currentItem, int currentIndex, int totalCount);

            /// <summary>
            /// 获取是否已取消
            /// </summary>
            bool IsCancelled { get; }
        }

        /// <summary>
        /// 进度报告器实现
        /// </summary>
        private class ProgressReporter : IProgressReporter
        {
            private ProgressForm _progressForm;

            /// <summary>
            /// 获取是否已取消
            /// </summary>
            public bool IsCancelled => _progressForm.IsCanceled;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="progressForm">进度表单</param>
            public ProgressReporter(ProgressForm progressForm)
            {
                _progressForm = progressForm;
            }

            /// <summary>
            /// 更新进度
            /// </summary>
            /// <param name="percentage">进度百分比(0-100)</param>
            /// <param name="statusText">状态文本</param>
            public void ReportProgress(int percentage, string statusText = "")
            {
                percentage = Math.Max(0, Math.Min(100, percentage));
                _progressForm.SafeUpdate(() =>
                {
                    _progressForm.ProgressPercentage = percentage;
                    if (!string.IsNullOrEmpty(statusText))
                    {
                        _progressForm.StatusText = statusText;
                    }
                });
            }

            /// <summary>
            /// 报告当前正在处理的项目
            /// </summary>
            /// <param name="currentItem">当前项目名称</param>
            /// <param name="currentIndex">当前索引</param>
            /// <param name="totalCount">总项目数</param>
            public void ReportCurrentItem(string currentItem, int currentIndex, int totalCount)
            {
                var percentage = totalCount > 0 ? (int)((double)currentIndex / totalCount * 100) : 0;
                var statusText = string.IsNullOrEmpty(currentItem) 
                    ? $"处理中: {currentIndex}/{totalCount}"
                    : $"处理中: {currentItem} ({currentIndex}/{totalCount})";

                ReportProgress(percentage, statusText);
            }
        }

        /// <summary>
        /// 扩展方法：安全更新进度表单
        /// </summary>
        /// <param name="progressForm">进度表单</param>
        /// <param name="updateAction">更新操作</param>
        private static void SafeUpdate(this ProgressForm progressForm, Action updateAction)
        {
            if (progressForm.InvokeRequired)
            {
                progressForm.Invoke(new Action(updateAction));
            }
            else
            {
                updateAction();
            }
        }
    }
}