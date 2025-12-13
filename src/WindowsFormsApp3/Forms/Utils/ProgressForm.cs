using System;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    /// <summary>
    /// 进度对话框
    /// </summary>
    public partial class ProgressForm : Form
    {
        /// <summary>
        /// 是否已取消
        /// </summary>
        private bool isCanceled = false;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProgressForm()
        {
            InitializeComponent();
            // 设置取消按钮事件
            cancelButton.Click += CancelButton_Click;
        }
        
        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            isCanceled = true;
            cancelButton.Enabled = false;
        }
        
        /// <summary>
        /// 更新进度
        /// </summary>
        /// <param name="percentage">进度百分比</param>
        /// <param name="statusText">状态文本</param>
        public void UpdateProgress(double percentage, string statusText)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateProgress(percentage, statusText)));
                return;
            }
            
            progressBar.Value = Math.Min(100, Math.Max(0, (int)percentage));
            statusLabel.Text = statusText;
        }
        
        /// <summary>
        /// 获取或设置窗口标题
        /// </summary>
        public string Title
        {
            get { return this.Text; }
            set { this.Text = value; }
        }
        
        /// <summary>
        /// 获取或设置取消按钮是否启用
        /// </summary>
        public bool CancelEnabled
        {
            get { return cancelButton.Visible; }
            set 
            { 
                cancelButton.Visible = value;
                // 调整窗体大小以适应取消按钮
                if (value)
                {
                    this.Size = new System.Drawing.Size(380, 130);
                }
                else
                {
                    this.Size = new System.Drawing.Size(380, 100);
                }
            }
        }
        
        /// <summary>
        /// 获取是否已取消
        /// </summary>
        public bool IsCanceled
        {
            get { return isCanceled; }
        }
        
        /// <summary>
        /// 获取或设置进度百分比
        /// </summary>
        public int ProgressPercentage
        {
            get { return progressBar.Value; }
            set { progressBar.Value = Math.Min(100, Math.Max(0, value)); }
        }
        
        /// <summary>
        /// 获取或设置状态文本
        /// </summary>
        public string StatusText
        {
            get { return statusLabel.Text; }
            set { statusLabel.Text = value; }
        }

        /// <summary>
        /// 添加取消按钮并设置其点击事件
        /// </summary>
        /// <param name="cancelAction">取消操作时要执行的动作</param>
        public void AddCancelButton(Action cancelAction)
        {
            // 移除现有的事件处理器
            cancelButton.Click -= CancelButton_Click;

            // 添加新的事件处理器
            cancelButton.Click += (sender, e) =>
            {
                isCanceled = true;
                cancelButton.Enabled = false;
                cancelAction?.Invoke();
            };

            // 显示取消按钮
            CancelEnabled = true;
        }
    }
}