namespace WindowsFormsApp3
{
    partial class PdfProcessingForm
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
            this.grpFileSelection = new System.Windows.Forms.GroupBox();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.grpConfiguration = new System.Windows.Forms.GroupBox();
            this.chkUseCropBox = new System.Windows.Forms.CheckBox();
            this.chkEnableDetailedLogging = new System.Windows.Forms.CheckBox();
            this.lblPreferredLibrary = new System.Windows.Forms.Label();
            this.cmbPreferredLibrary = new System.Windows.Forms.ComboBox();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnGetPageCount = new System.Windows.Forms.Button();
            this.btnGetFirstPageSize = new System.Windows.Forms.Button();
            this.btnGetAllPageSizes = new System.Windows.Forms.Button();
            this.btnCompareLibraries = new System.Windows.Forms.Button();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tabResults = new System.Windows.Forms.TabControl();
            this.tabResultsPage = new System.Windows.Forms.TabPage();
            this.txtResults = new System.Windows.Forms.RichTextBox();
            this.tabPageInfo = new System.Windows.Forms.TabPage();
            this.dgvPageInfo = new System.Windows.Forms.DataGridView();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.grpFileSelection.SuspendLayout();
            this.grpConfiguration.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tabResults.SuspendLayout();
            this.tabResultsPage.SuspendLayout();
            this.tabPageInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPageInfo)).BeginInit();
            this.tabLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpFileSelection
            // 
            this.grpFileSelection.Controls.Add(this.txtFilePath);
            this.grpFileSelection.Controls.Add(this.btnBrowseFile);
            this.grpFileSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFileSelection.Location = new System.Drawing.Point(0, 0);
            this.grpFileSelection.Name = "grpFileSelection";
            this.grpFileSelection.Size = new System.Drawing.Size(784, 80);
            this.grpFileSelection.TabIndex = 0;
            this.grpFileSelection.TabStop = false;
            this.grpFileSelection.Text = "文件选择";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilePath.Location = new System.Drawing.Point(3, 17);
            this.txtFilePath.Multiline = false;
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(698, 60);
            this.txtFilePath.TabIndex = 0;
            // 
            // btnBrowseFile
            // 
            this.btnBrowseFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnBrowseFile.Location = new System.Drawing.Point(701, 17);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(80, 60);
            this.btnBrowseFile.TabIndex = 1;
            this.btnBrowseFile.Text = "浏览...";
            this.btnBrowseFile.UseVisualStyleBackColor = true;
            // 
            // grpConfiguration
            // 
            this.grpConfiguration.Controls.Add(this.chkUseCropBox);
            this.grpConfiguration.Controls.Add(this.chkEnableDetailedLogging);
            this.grpConfiguration.Controls.Add(this.lblPreferredLibrary);
            this.grpConfiguration.Controls.Add(this.cmbPreferredLibrary);
            this.grpConfiguration.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpConfiguration.Location = new System.Drawing.Point(0, 80);
            this.grpConfiguration.Name = "grpConfiguration";
            this.grpConfiguration.Size = new System.Drawing.Size(784, 120);
            this.grpConfiguration.TabIndex = 1;
            this.grpConfiguration.TabStop = false;
            this.grpConfiguration.Text = "处理配置";
            // 
            // chkUseCropBox
            // 
            this.chkUseCropBox.AutoSize = true;
            this.chkUseCropBox.Checked = true;
            this.chkUseCropBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseCropBox.Location = new System.Drawing.Point(10, 25);
            this.chkUseCropBox.Name = "chkUseCropBox";
            this.chkUseCropBox.Size = new System.Drawing.Size(84, 21);
            this.chkUseCropBox.TabIndex = 0;
            this.chkUseCropBox.Text = "使用CropBox";
            this.chkUseCropBox.UseVisualStyleBackColor = true;
            // 
            // chkEnableDetailedLogging
            // 
            this.chkEnableDetailedLogging.AutoSize = true;
            this.chkEnableDetailedLogging.Checked = true;
            this.chkEnableDetailedLogging.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableDetailedLogging.Location = new System.Drawing.Point(150, 25);
            this.chkEnableDetailedLogging.Name = "chkEnableDetailedLogging";
            this.chkEnableDetailedLogging.Size = new System.Drawing.Size(108, 21);
            this.chkEnableDetailedLogging.TabIndex = 1;
            this.chkEnableDetailedLogging.Text = "启用详细日志";
            this.chkEnableDetailedLogging.UseVisualStyleBackColor = true;
            // 
            // lblPreferredLibrary
            // 
            this.lblPreferredLibrary.AutoSize = true;
            this.lblPreferredLibrary.Location = new System.Drawing.Point(10, 55);
            this.lblPreferredLibrary.Name = "lblPreferredLibrary";
            this.lblPreferredLibrary.Size = new System.Drawing.Size(53, 17);
            this.lblPreferredLibrary.TabIndex = 2;
            this.lblPreferredLibrary.Text = "首选库:";
            // 
            // cmbPreferredLibrary
            // 
            this.cmbPreferredLibrary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPreferredLibrary.FormattingEnabled = true;
            this.cmbPreferredLibrary.Items.AddRange(new object[] {
            "IText7",
            "PdfTools"});
            this.cmbPreferredLibrary.Location = new System.Drawing.Point(70, 52);
            this.cmbPreferredLibrary.Name = "cmbPreferredLibrary";
            this.cmbPreferredLibrary.Size = new System.Drawing.Size(120, 25);
            this.cmbPreferredLibrary.TabIndex = 3;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnGetPageCount);
            this.pnlButtons.Controls.Add(this.btnGetFirstPageSize);
            this.pnlButtons.Controls.Add(this.btnGetAllPageSizes);
            this.pnlButtons.Controls.Add(this.btnCompareLibraries);
            this.pnlButtons.Controls.Add(this.btnGenerateReport);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlButtons.Location = new System.Drawing.Point(0, 200);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(784, 50);
            this.pnlButtons.TabIndex = 2;
            // 
            // btnGetPageCount
            // 
            this.btnGetPageCount.Location = new System.Drawing.Point(10, 10);
            this.btnGetPageCount.Name = "btnGetPageCount";
            this.btnGetPageCount.Size = new System.Drawing.Size(100, 30);
            this.btnGetPageCount.TabIndex = 0;
            this.btnGetPageCount.Text = "获取页数";
            this.btnGetPageCount.UseVisualStyleBackColor = true;
            // 
            // btnGetFirstPageSize
            // 
            this.btnGetFirstPageSize.Location = new System.Drawing.Point(120, 10);
            this.btnGetFirstPageSize.Name = "btnGetFirstPageSize";
            this.btnGetFirstPageSize.Size = new System.Drawing.Size(120, 30);
            this.btnGetFirstPageSize.TabIndex = 1;
            this.btnGetFirstPageSize.Text = "获取首页尺寸";
            this.btnGetFirstPageSize.UseVisualStyleBackColor = true;
            // 
            // btnGetAllPageSizes
            // 
            this.btnGetAllPageSizes.Location = new System.Drawing.Point(250, 10);
            this.btnGetAllPageSizes.Name = "btnGetAllPageSizes";
            this.btnGetAllPageSizes.Size = new System.Drawing.Size(140, 30);
            this.btnGetAllPageSizes.TabIndex = 2;
            this.btnGetAllPageSizes.Text = "获取所有页面尺寸";
            this.btnGetAllPageSizes.UseVisualStyleBackColor = true;
            // 
            // btnCompareLibraries
            // 
            this.btnCompareLibraries.Location = new System.Drawing.Point(400, 10);
            this.btnCompareLibraries.Name = "btnCompareLibraries";
            this.btnCompareLibraries.Size = new System.Drawing.Size(100, 30);
            this.btnCompareLibraries.TabIndex = 3;
            this.btnCompareLibraries.Text = "比较PDF库";
            this.btnCompareLibraries.UseVisualStyleBackColor = true;
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.Location = new System.Drawing.Point(510, 10);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(100, 30);
            this.btnGenerateReport.TabIndex = 4;
            this.btnGenerateReport.Text = "生成报告";
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(620, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar.Location = new System.Drawing.Point(0, 250);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(784, 20);
            this.progressBar.TabIndex = 3;
            this.progressBar.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatus.Location = new System.Drawing.Point(0, 270);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(784, 20);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "就绪";
            // 
            // tabResults
            // 
            this.tabResults.Controls.Add(this.tabResultsPage);
            this.tabResults.Controls.Add(this.tabPageInfo);
            this.tabResults.Controls.Add(this.tabLog);
            this.tabResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabResults.Location = new System.Drawing.Point(0, 290);
            this.tabResults.Name = "tabResults";
            this.tabResults.SelectedIndex = 0;
            this.tabResults.Size = new System.Drawing.Size(784, 370);
            this.tabResults.TabIndex = 5;
            // 
            // tabResultsPage
            // 
            this.tabResultsPage.Controls.Add(this.txtResults);
            this.tabResultsPage.Location = new System.Drawing.Point(4, 25);
            this.tabResultsPage.Name = "tabResultsPage";
            this.tabResultsPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabResultsPage.Size = new System.Drawing.Size(776, 341);
            this.tabResultsPage.TabIndex = 0;
            this.tabResultsPage.Text = "处理结果";
            this.tabResultsPage.UseVisualStyleBackColor = true;
            // 
            // txtResults
            // 
            this.txtResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResults.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtResults.Location = new System.Drawing.Point(3, 3);
            this.txtResults.Name = "txtResults";
            this.txtResults.ReadOnly = true;
            this.txtResults.Size = new System.Drawing.Size(770, 335);
            this.txtResults.TabIndex = 0;
            this.txtResults.Text = "";
            // 
            // tabPageInfo
            // 
            this.tabPageInfo.Controls.Add(this.dgvPageInfo);
            this.tabPageInfo.Location = new System.Drawing.Point(4, 25);
            this.tabPageInfo.Name = "tabPageInfo";
            this.tabPageInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInfo.Size = new System.Drawing.Size(776, 341);
            this.tabPageInfo.TabIndex = 1;
            this.tabPageInfo.Text = "页面信息";
            this.tabPageInfo.UseVisualStyleBackColor = true;
            // 
            // dgvPageInfo
            // 
            this.dgvPageInfo.AllowUserToAddRows = false;
            this.dgvPageInfo.AllowUserToDeleteRows = false;
            this.dgvPageInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPageInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPageInfo.Location = new System.Drawing.Point(3, 3);
            this.dgvPageInfo.Name = "dgvPageInfo";
            this.dgvPageInfo.ReadOnly = true;
            this.dgvPageInfo.RowHeadersVisible = false;
            this.dgvPageInfo.Size = new System.Drawing.Size(770, 335);
            this.dgvPageInfo.TabIndex = 0;
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.txtLog);
            this.tabLog.Location = new System.Drawing.Point(4, 25);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(776, 341);
            this.tabLog.TabIndex = 2;
            this.tabLog.Text = "处理日志";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 8F);
            this.txtLog.ForeColor = System.Drawing.Color.Lime;
            this.txtLog.Location = new System.Drawing.Point(3, 3);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(770, 335);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            // 
            // PdfProcessingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 660);
            this.Controls.Add(this.tabResults);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.grpConfiguration);
            this.Controls.Add(this.grpFileSelection);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "PdfProcessingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PDF处理工具";
            this.grpFileSelection.ResumeLayout(false);
            this.grpFileSelection.PerformLayout();
            this.grpConfiguration.ResumeLayout(false);
            this.grpConfiguration.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.tabResults.ResumeLayout(false);
            this.tabResultsPage.ResumeLayout(false);
            this.tabPageInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPageInfo)).EndInit();
            this.tabLog.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFileSelection;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowseFile;
        private System.Windows.Forms.GroupBox grpConfiguration;
        private System.Windows.Forms.CheckBox chkUseCropBox;
        private System.Windows.Forms.CheckBox chkEnableDetailedLogging;
        private System.Windows.Forms.Label lblPreferredLibrary;
        private System.Windows.Forms.ComboBox cmbPreferredLibrary;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnGetPageCount;
        private System.Windows.Forms.Button btnGetFirstPageSize;
        private System.Windows.Forms.Button btnGetAllPageSizes;
        private System.Windows.Forms.Button btnCompareLibraries;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TabControl tabResults;
        private System.Windows.Forms.TabPage tabResultsPage;
        private System.Windows.Forms.RichTextBox txtResults;
        private System.Windows.Forms.TabPage tabPageInfo;
        private System.Windows.Forms.DataGridView dgvPageInfo;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.RichTextBox txtLog;
    }
}