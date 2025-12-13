namespace WindowsFormsApp3
{
    partial class OperationHistoryForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.refreshButton = new System.Windows.Forms.ToolStripButton();
            this.undoButton = new System.Windows.Forms.ToolStripButton();
            this.redoButton = new System.Windows.Forms.ToolStripButton();
            this.clearButton = new System.Windows.Forms.ToolStripButton();
            this.saveButton = new System.Windows.Forms.ToolStripButton();
            this.loadButton = new System.Windows.Forms.ToolStripButton();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.historyPage = new System.Windows.Forms.TabPage();
            this.historyListView = new System.Windows.Forms.ListView();
            this.columnHeaderTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDuration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTags = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.batchPage = new System.Windows.Forms.TabPage();
            this.batchListView = new System.Windows.Forms.ListView();
            this.columnHeaderBatchTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBatchDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBatchCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBatchSuccessRate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBatchDuration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBatchStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statsPage = new System.Windows.Forms.TabPage();
            this.statsLabel = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl.SuspendLayout();
            this.historyPage.SuspendLayout();
            this.batchPage.SuspendLayout();
            this.statsPage.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshButton,
            this.undoButton,
            this.redoButton,
            this.clearButton,
            this.saveButton,
            this.loadButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(784, 27);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // refreshButton
            // 
            this.refreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(44, 24);
            this.refreshButton.Text = "刷新";
            this.refreshButton.ToolTipText = "刷新历史记录";
            // 
            // undoButton
            // 
            this.undoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.undoButton.Name = "undoButton";
            this.undoButton.Size = new System.Drawing.Size(44, 24);
            this.undoButton.Text = "撤销";
            this.undoButton.ToolTipText = "撤销选中项";
            // 
            // redoButton
            // 
            this.redoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.redoButton.Name = "redoButton";
            this.redoButton.Size = new System.Drawing.Size(44, 24);
            this.redoButton.Text = "重做";
            this.redoButton.ToolTipText = "重做选中项";
            // 
            // clearButton
            // 
            this.clearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(68, 24);
            this.clearButton.Text = "清空历史";
            this.clearButton.ToolTipText = "清空所有历史记录";
            // 
            // saveButton
            // 
            this.saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(68, 24);
            this.saveButton.Text = "保存历史";
            this.saveButton.ToolTipText = "保存历史记录到文件";
            // 
            // loadButton
            // 
            this.loadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(68, 24);
            this.loadButton.Text = "加载历史";
            this.loadButton.ToolTipText = "从文件加载历史记录";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.historyPage);
            this.tabControl.Controls.Add(this.batchPage);
            this.tabControl.Controls.Add(this.statsPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 27);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(784, 536);
            this.tabControl.TabIndex = 1;
            // 
            // historyPage
            // 
            this.historyPage.Controls.Add(this.historyListView);
            this.historyPage.Location = new System.Drawing.Point(4, 25);
            this.historyPage.Name = "historyPage";
            this.historyPage.Padding = new System.Windows.Forms.Padding(3);
            this.historyPage.Size = new System.Drawing.Size(776, 507);
            this.historyPage.TabIndex = 0;
            this.historyPage.Text = "单个操作";
            this.historyPage.UseVisualStyleBackColor = true;
            // 
            // historyListView
            // 
            this.historyListView.CheckBoxes = true;
            this.historyListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTime,
            this.columnHeaderDescription,
            this.columnHeaderType,
            this.columnHeaderDuration,
            this.columnHeaderStatus,
            this.columnHeaderTags});
            this.historyListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historyListView.FullRowSelect = true;
            this.historyListView.GridLines = true;
            this.historyListView.Location = new System.Drawing.Point(3, 3);
            this.historyListView.MultiSelect = true;
            this.historyListView.Name = "historyListView";
            this.historyListView.Size = new System.Drawing.Size(770, 501);
            this.historyListView.TabIndex = 0;
            this.historyListView.UseCompatibleStateImageBehavior = false;
            this.historyListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderTime
            // 
            this.columnHeaderTime.Text = "时间";
            this.columnHeaderTime.Width = 120;
            // 
            // columnHeaderDescription
            // 
            this.columnHeaderDescription.Text = "操作描述";
            this.columnHeaderDescription.Width = 200;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "类型";
            this.columnHeaderType.Width = 100;
            // 
            // columnHeaderDuration
            // 
            this.columnHeaderDuration.Text = "耗时(ms)";
            this.columnHeaderDuration.Width = 80;
            // 
            // columnHeaderStatus
            // 
            this.columnHeaderStatus.Text = "状态";
            this.columnHeaderStatus.Width = 80;
            // 
            // columnHeaderTags
            // 
            this.columnHeaderTags.Text = "标签";
            this.columnHeaderTags.Width = 150;
            // 
            // batchPage
            // 
            this.batchPage.Controls.Add(this.batchListView);
            this.batchPage.Location = new System.Drawing.Point(4, 25);
            this.batchPage.Name = "batchPage";
            this.batchPage.Padding = new System.Windows.Forms.Padding(3);
            this.batchPage.Size = new System.Drawing.Size(776, 507);
            this.batchPage.TabIndex = 1;
            this.batchPage.Text = "批量操作";
            this.batchPage.UseVisualStyleBackColor = true;
            // 
            // batchListView
            // 
            this.batchListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderBatchTime,
            this.columnHeaderBatchDescription,
            this.columnHeaderBatchCount,
            this.columnHeaderBatchSuccessRate,
            this.columnHeaderBatchDuration,
            this.columnHeaderBatchStatus});
            this.batchListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.batchListView.FullRowSelect = true;
            this.batchListView.GridLines = true;
            this.batchListView.Location = new System.Drawing.Point(3, 3);
            this.batchListView.MultiSelect = true;
            this.batchListView.Name = "batchListView";
            this.batchListView.Size = new System.Drawing.Size(770, 501);
            this.batchListView.TabIndex = 0;
            this.batchListView.UseCompatibleStateImageBehavior = false;
            this.batchListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderBatchTime
            // 
            this.columnHeaderBatchTime.Text = "开始时间";
            this.columnHeaderBatchTime.Width = 120;
            // 
            // columnHeaderBatchDescription
            // 
            this.columnHeaderBatchDescription.Text = "操作描述";
            this.columnHeaderBatchDescription.Width = 200;
            // 
            // columnHeaderBatchCount
            // 
            this.columnHeaderBatchCount.Text = "命令数量";
            this.columnHeaderBatchCount.Width = 80;
            // 
            // columnHeaderBatchSuccessRate
            // 
            this.columnHeaderBatchSuccessRate.Text = "成功率";
            this.columnHeaderBatchSuccessRate.Width = 80;
            // 
            // columnHeaderBatchDuration
            // 
            this.columnHeaderBatchDuration.Text = "总耗时(ms)";
            this.columnHeaderBatchDuration.Width = 100;
            // 
            // columnHeaderBatchStatus
            // 
            this.columnHeaderBatchStatus.Text = "状态";
            this.columnHeaderBatchStatus.Width = 80;
            // 
            // statsPage
            // 
            this.statsPage.Controls.Add(this.statsLabel);
            this.statsPage.Location = new System.Drawing.Point(4, 25);
            this.statsPage.Name = "statsPage";
            this.statsPage.Padding = new System.Windows.Forms.Padding(3);
            this.statsPage.Size = new System.Drawing.Size(776, 507);
            this.statsPage.TabIndex = 2;
            this.statsPage.Text = "统计信息";
            this.statsPage.UseVisualStyleBackColor = true;
            // 
            // statsLabel
            // 
            this.statsLabel.AutoSize = false;
            this.statsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsLabel.Font = new System.Drawing.Font("Microsoft YaHei", 10F);
            this.statsLabel.Location = new System.Drawing.Point(3, 3);
            this.statsLabel.Name = "statsLabel";
            this.statsLabel.Padding = new System.Windows.Forms.Padding(20);
            this.statsLabel.Size = new System.Drawing.Size(770, 501);
            this.statsLabel.TabIndex = 0;
            this.statsLabel.Text = "正在加载统计信息...";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 563);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(784, 24);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(41, 19);
            this.statusLabel.Text = "就绪";
            // 
            // OperationHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 587);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "OperationHistoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "操作历史";
            this.tabControl.ResumeLayout(false);
            this.historyPage.ResumeLayout(false);
            this.batchPage.ResumeLayout(false);
            this.statsPage.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton refreshButton;
        private System.Windows.Forms.ToolStripButton undoButton;
        private System.Windows.Forms.ToolStripButton redoButton;
        private System.Windows.Forms.ToolStripButton clearButton;
        private System.Windows.Forms.ToolStripButton saveButton;
        private System.Windows.Forms.ToolStripButton loadButton;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage historyPage;
        private System.Windows.Forms.ListView historyListView;
        private System.Windows.Forms.ColumnHeader columnHeaderTime;
        private System.Windows.Forms.ColumnHeader columnHeaderDescription;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderDuration;
        private System.Windows.Forms.ColumnHeader columnHeaderStatus;
        private System.Windows.Forms.ColumnHeader columnHeaderTags;
        private System.Windows.Forms.TabPage batchPage;
        private System.Windows.Forms.ListView batchListView;
        private System.Windows.Forms.ColumnHeader columnHeaderBatchTime;
        private System.Windows.Forms.ColumnHeader columnHeaderBatchDescription;
        private System.Windows.Forms.ColumnHeader columnHeaderBatchCount;
        private System.Windows.Forms.ColumnHeader columnHeaderBatchSuccessRate;
        private System.Windows.Forms.ColumnHeader columnHeaderBatchDuration;
        private System.Windows.Forms.ColumnHeader columnHeaderBatchStatus;
        private System.Windows.Forms.TabPage statsPage;
        private System.Windows.Forms.Label statsLabel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
    }
}