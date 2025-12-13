namespace WindowsFormsApp3
{
    partial class PerformanceMonitorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageMemory = new System.Windows.Forms.TabPage();
            this.groupBoxMemoryMonitoring = new System.Windows.Forms.GroupBox();
            this.labelCurrentMemory = new System.Windows.Forms.Label();
            this.labelPeakMemory = new System.Windows.Forms.Label();
            this.labelGCCollections = new System.Windows.Forms.Label();
            this.buttonStartMonitoring = new System.Windows.Forms.Button();
            this.buttonStopMonitoring = new System.Windows.Forms.Button();
            this.buttonTriggerGC = new System.Windows.Forms.Button();
            this.textBoxMemoryHistory = new System.Windows.Forms.TextBox();
            this.tabPageBenchmark = new System.Windows.Forms.TabPage();
            this.groupBoxBenchmark = new System.Windows.Forms.GroupBox();
            this.labelTestFiles = new System.Windows.Forms.Label();
            this.textBoxTestFiles = new System.Windows.Forms.TextBox();
            this.buttonSelectFiles = new System.Windows.Forms.Button();
            this.numericUpDownIterations = new System.Windows.Forms.NumericUpDown();
            this.buttonRunRenameBenchmark = new System.Windows.Forms.Button();
            this.buttonRunBatchBenchmark = new System.Windows.Forms.Button();
            this.buttonRunMemoryStress = new System.Windows.Forms.Button();
            this.textBoxBenchmarkResults = new System.Windows.Forms.TextBox();
            this.progressBarBenchmark = new System.Windows.Forms.ProgressBar();
            this.tabPageOptimization = new System.Windows.Forms.TabPage();
            this.groupBoxOptimization = new System.Windows.Forms.GroupBox();
            this.labelOptimizeFiles = new System.Windows.Forms.Label();
            this.textBoxOptimizeFiles = new System.Windows.Forms.TextBox();
            this.buttonSelectOptimizeFiles = new System.Windows.Forms.Button();
            this.checkBoxParallelProcessing = new System.Windows.Forms.CheckBox();
            this.numericUpDownMemoryThreshold = new System.Windows.Forms.NumericUpDown();
            this.buttonRunOptimized = new System.Windows.Forms.Button();
            this.textBoxOptimizationResults = new System.Windows.Forms.TextBox();
            this.progressBarOptimization = new System.Windows.Forms.ProgressBar();
            this.tabControl1.SuspendLayout();
            this.tabPageMemory.SuspendLayout();
            this.groupBoxMemoryMonitoring.SuspendLayout();
            this.tabPageBenchmark.SuspendLayout();
            this.groupBoxBenchmark.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIterations)).BeginInit();
            this.tabPageOptimization.SuspendLayout();
            this.groupBoxOptimization.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMemoryThreshold)).BeginInit();
            this.SuspendLayout();
            //
            // tabControl1
            //
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageMemory);
            this.tabControl1.Controls.Add(this.tabPageBenchmark);
            this.tabControl1.Controls.Add(this.tabPageOptimization);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(860, 510);
            this.tabControl1.TabIndex = 0;
            //
            // tabPageMemory
            //
            this.tabPageMemory.Controls.Add(this.groupBoxMemoryMonitoring);
            this.tabPageMemory.Location = new System.Drawing.Point(4, 22);
            this.tabPageMemory.Name = "tabPageMemory";
            this.tabPageMemory.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMemory.Size = new System.Drawing.Size(852, 484);
            this.tabPageMemory.TabIndex = 0;
            this.tabPageMemory.Text = "内存监控";
            this.tabPageMemory.UseVisualStyleBackColor = true;
            //
            // groupBoxMemoryMonitoring
            //
            this.groupBoxMemoryMonitoring.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMemoryMonitoring.Controls.Add(this.labelCurrentMemory);
            this.groupBoxMemoryMonitoring.Controls.Add(this.labelPeakMemory);
            this.groupBoxMemoryMonitoring.Controls.Add(this.labelGCCollections);
            this.groupBoxMemoryMonitoring.Controls.Add(this.buttonStartMonitoring);
            this.groupBoxMemoryMonitoring.Controls.Add(this.buttonStopMonitoring);
            this.groupBoxMemoryMonitoring.Controls.Add(this.buttonTriggerGC);
            this.groupBoxMemoryMonitoring.Controls.Add(this.textBoxMemoryHistory);
            this.groupBoxMemoryMonitoring.Location = new System.Drawing.Point(6, 6);
            this.groupBoxMemoryMonitoring.Name = "groupBoxMemoryMonitoring";
            this.groupBoxMemoryMonitoring.Size = new System.Drawing.Size(840, 472);
            this.groupBoxMemoryMonitoring.TabIndex = 0;
            this.groupBoxMemoryMonitoring.TabStop = false;
            this.groupBoxMemoryMonitoring.Text = "内存监控";
            //
            // labelCurrentMemory
            //
            this.labelCurrentMemory.AutoSize = true;
            this.labelCurrentMemory.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCurrentMemory.Location = new System.Drawing.Point(17, 28);
            this.labelCurrentMemory.Name = "labelCurrentMemory";
            this.labelCurrentMemory.Size = new System.Drawing.Size(140, 17);
            this.labelCurrentMemory.TabIndex = 0;
            this.labelCurrentMemory.Text = "当前内存使用: 0 MB";
            //
            // labelPeakMemory
            //
            this.labelPeakMemory.AutoSize = true;
            this.labelPeakMemory.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelPeakMemory.Location = new System.Drawing.Point(17, 55);
            this.labelPeakMemory.Name = "labelPeakMemory";
            this.labelPeakMemory.Size = new System.Drawing.Size(140, 17);
            this.labelPeakMemory.TabIndex = 1;
            this.labelPeakMemory.Text = "峰值内存使用: 0 MB";
            //
            // labelGCCollections
            //
            this.labelGCCollections.AutoSize = true;
            this.labelGCCollections.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelGCCollections.Location = new System.Drawing.Point(17, 82);
            this.labelGCCollections.Name = "labelGCCollections";
            this.labelGCCollections.Size = new System.Drawing.Size(158, 17);
            this.labelGCCollections.TabIndex = 2;
            this.labelGCCollections.Text = "GC回收次数: Gen0=0";
            //
            // buttonStartMonitoring
            //
            this.buttonStartMonitoring.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonStartMonitoring.Location = new System.Drawing.Point(17, 115);
            this.buttonStartMonitoring.Name = "buttonStartMonitoring";
            this.buttonStartMonitoring.Size = new System.Drawing.Size(100, 30);
            this.buttonStartMonitoring.TabIndex = 3;
            this.buttonStartMonitoring.Text = "开始监控";
            this.buttonStartMonitoring.UseVisualStyleBackColor = true;
            //
            // buttonStopMonitoring
            //
            this.buttonStopMonitoring.Enabled = false;
            this.buttonStopMonitoring.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonStopMonitoring.Location = new System.Drawing.Point(123, 115);
            this.buttonStopMonitoring.Name = "buttonStopMonitoring";
            this.buttonStopMonitoring.Size = new System.Drawing.Size(100, 30);
            this.buttonStopMonitoring.TabIndex = 4;
            this.buttonStopMonitoring.Text = "停止监控";
            this.buttonStopMonitoring.UseVisualStyleBackColor = true;
            //
            // buttonTriggerGC
            //
            this.buttonTriggerGC.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonTriggerGC.Location = new System.Drawing.Point(229, 115);
            this.buttonTriggerGC.Name = "buttonTriggerGC";
            this.buttonTriggerGC.Size = new System.Drawing.Size(100, 30);
            this.buttonTriggerGC.TabIndex = 5;
            this.buttonTriggerGC.Text = "手动GC";
            this.buttonTriggerGC.UseVisualStyleBackColor = true;
            //
            // textBoxMemoryHistory
            //
            this.textBoxMemoryHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMemoryHistory.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMemoryHistory.Location = new System.Drawing.Point(17, 151);
            this.textBoxMemoryHistory.Multiline = true;
            this.textBoxMemoryHistory.Name = "textBoxMemoryHistory";
            this.textBoxMemoryHistory.ReadOnly = true;
            this.textBoxMemoryHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMemoryHistory.Size = new System.Drawing.Size(807, 315);
            this.textBoxMemoryHistory.TabIndex = 6;
            //
            // tabPageBenchmark
            //
            this.tabPageBenchmark.Controls.Add(this.groupBoxBenchmark);
            this.tabPageBenchmark.Location = new System.Drawing.Point(4, 22);
            this.tabPageBenchmark.Name = "tabPageBenchmark";
            this.tabPageBenchmark.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBenchmark.Size = new System.Drawing.Size(852, 484);
            this.tabPageBenchmark.TabIndex = 1;
            this.tabPageBenchmark.Text = "性能基准测试";
            this.tabPageBenchmark.UseVisualStyleBackColor = true;
            //
            // groupBoxBenchmark
            //
            this.groupBoxBenchmark.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxBenchmark.Controls.Add(this.labelTestFiles);
            this.groupBoxBenchmark.Controls.Add(this.textBoxTestFiles);
            this.groupBoxBenchmark.Controls.Add(this.buttonSelectFiles);
            this.groupBoxBenchmark.Controls.Add(this.numericUpDownIterations);
            this.groupBoxBenchmark.Controls.Add(this.buttonRunRenameBenchmark);
            this.groupBoxBenchmark.Controls.Add(this.buttonRunBatchBenchmark);
            this.groupBoxBenchmark.Controls.Add(this.buttonRunMemoryStress);
            this.groupBoxBenchmark.Controls.Add(this.textBoxBenchmarkResults);
            this.groupBoxBenchmark.Controls.Add(this.progressBarBenchmark);
            this.groupBoxBenchmark.Location = new System.Drawing.Point(6, 6);
            this.groupBoxBenchmark.Name = "groupBoxBenchmark";
            this.groupBoxBenchmark.Size = new System.Drawing.Size(840, 472);
            this.groupBoxBenchmark.TabIndex = 0;
            this.groupBoxBenchmark.TabStop = false;
            this.groupBoxBenchmark.Text = "性能基准测试";
            //
            // labelTestFiles
            //
            this.labelTestFiles.AutoSize = true;
            this.labelTestFiles.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTestFiles.Location = new System.Drawing.Point(17, 28);
            this.labelTestFiles.Name = "labelTestFiles";
            this.labelTestFiles.Size = new System.Drawing.Size(70, 17);
            this.labelTestFiles.TabIndex = 0;
            this.labelTestFiles.Text = "测试文件:";
            //
            // textBoxTestFiles
            //
            this.textBoxTestFiles.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxTestFiles.Location = new System.Drawing.Point(93, 25);
            this.textBoxTestFiles.Name = "textBoxTestFiles";
            this.textBoxTestFiles.ReadOnly = true;
            this.textBoxTestFiles.Size = new System.Drawing.Size(550, 23);
            this.textBoxTestFiles.TabIndex = 1;
            //
            // buttonSelectFiles
            //
            this.buttonSelectFiles.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonSelectFiles.Location = new System.Drawing.Point(649, 23);
            this.buttonSelectFiles.Name = "buttonSelectFiles";
            this.buttonSelectFiles.Size = new System.Drawing.Size(75, 28);
            this.buttonSelectFiles.TabIndex = 2;
            this.buttonSelectFiles.Text = "选择文件";
            this.buttonSelectFiles.UseVisualStyleBackColor = true;
            //
            // numericUpDownIterations
            //
            this.numericUpDownIterations.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDownIterations.Location = new System.Drawing.Point(93, 57);
            this.numericUpDownIterations.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownIterations.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownIterations.Name = "numericUpDownIterations";
            this.numericUpDownIterations.Size = new System.Drawing.Size(80, 23);
            this.numericUpDownIterations.TabIndex = 3;
            this.numericUpDownIterations.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            //
            // buttonRunRenameBenchmark
            //
            this.buttonRunRenameBenchmark.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonRunRenameBenchmark.Location = new System.Drawing.Point(17, 90);
            this.buttonRunRenameBenchmark.Name = "buttonRunRenameBenchmark";
            this.buttonRunRenameBenchmark.Size = new System.Drawing.Size(120, 30);
            this.buttonRunRenameBenchmark.TabIndex = 4;
            this.buttonRunRenameBenchmark.Text = "重命名基准测试";
            this.buttonRunRenameBenchmark.UseVisualStyleBackColor = true;
            //
            // buttonRunBatchBenchmark
            //
            this.buttonRunBatchBenchmark.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonRunBatchBenchmark.Location = new System.Drawing.Point(143, 90);
            this.buttonRunBatchBenchmark.Name = "buttonRunBatchBenchmark";
            this.buttonRunBatchBenchmark.Size = new System.Drawing.Size(120, 30);
            this.buttonRunBatchBenchmark.TabIndex = 5;
            this.buttonRunBatchBenchmark.Text = "批量处理基准测试";
            this.buttonRunBatchBenchmark.UseVisualStyleBackColor = true;
            //
            // buttonRunMemoryStress
            //
            this.buttonRunMemoryStress.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonRunMemoryStress.Location = new System.Drawing.Point(269, 90);
            this.buttonRunMemoryStress.Name = "buttonRunMemoryStress";
            this.buttonRunMemoryStress.Size = new System.Drawing.Size(120, 30);
            this.buttonRunMemoryStress.TabIndex = 6;
            this.buttonRunMemoryStress.Text = "内存压力测试";
            this.buttonRunMemoryStress.UseVisualStyleBackColor = true;
            //
            // textBoxBenchmarkResults
            //
            this.textBoxBenchmarkResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBenchmarkResults.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBenchmarkResults.Location = new System.Drawing.Point(17, 126);
            this.textBoxBenchmarkResults.Multiline = true;
            this.textBoxBenchmarkResults.Name = "textBoxBenchmarkResults";
            this.textBoxBenchmarkResults.ReadOnly = true;
            this.textBoxBenchmarkResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxBenchmarkResults.Size = new System.Drawing.Size(807, 310);
            this.textBoxBenchmarkResults.TabIndex = 7;
            //
            // progressBarBenchmark
            //
            this.progressBarBenchmark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBarBenchmark.Location = new System.Drawing.Point(17, 442);
            this.progressBarBenchmark.Name = "progressBarBenchmark";
            this.progressBarBenchmark.Size = new System.Drawing.Size(300, 23);
            this.progressBarBenchmark.TabIndex = 8;
            //
            // tabPageOptimization
            //
            this.tabPageOptimization.Controls.Add(this.groupBoxOptimization);
            this.tabPageOptimization.Location = new System.Drawing.Point(4, 22);
            this.tabPageOptimization.Name = "tabPageOptimization";
            this.tabPageOptimization.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOptimization.Size = new System.Drawing.Size(852, 484);
            this.tabPageOptimization.TabIndex = 2;
            this.tabPageOptimization.Text = "优化测试";
            this.tabPageOptimization.UseVisualStyleBackColor = true;
            //
            // groupBoxOptimization
            //
            this.groupBoxOptimization.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxOptimization.Controls.Add(this.labelOptimizeFiles);
            this.groupBoxOptimization.Controls.Add(this.textBoxOptimizeFiles);
            this.groupBoxOptimization.Controls.Add(this.buttonSelectOptimizeFiles);
            this.groupBoxOptimization.Controls.Add(this.checkBoxParallelProcessing);
            this.groupBoxOptimization.Controls.Add(this.numericUpDownMemoryThreshold);
            this.groupBoxOptimization.Controls.Add(this.buttonRunOptimized);
            this.groupBoxOptimization.Controls.Add(this.textBoxOptimizationResults);
            this.groupBoxOptimization.Controls.Add(this.progressBarOptimization);
            this.groupBoxOptimization.Location = new System.Drawing.Point(6, 6);
            this.groupBoxOptimization.Name = "groupBoxOptimization";
            this.groupBoxOptimization.Size = new System.Drawing.Size(840, 472);
            this.groupBoxOptimization.TabIndex = 0;
            this.groupBoxOptimization.TabStop = false;
            this.groupBoxOptimization.Text = "优化测试";
            //
            // labelOptimizeFiles
            //
            this.labelOptimizeFiles.AutoSize = true;
            this.labelOptimizeFiles.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelOptimizeFiles.Location = new System.Drawing.Point(17, 28);
            this.labelOptimizeFiles.Name = "labelOptimizeFiles";
            this.labelOptimizeFiles.Size = new System.Drawing.Size(70, 17);
            this.labelOptimizeFiles.TabIndex = 0;
            this.labelOptimizeFiles.Text = "优化文件:";
            //
            // textBoxOptimizeFiles
            //
            this.textBoxOptimizeFiles.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxOptimizeFiles.Location = new System.Drawing.Point(93, 25);
            this.textBoxOptimizeFiles.Name = "textBoxOptimizeFiles";
            this.textBoxOptimizeFiles.ReadOnly = true;
            this.textBoxOptimizeFiles.Size = new System.Drawing.Size(550, 23);
            this.textBoxOptimizeFiles.TabIndex = 1;
            //
            // buttonSelectOptimizeFiles
            //
            this.buttonSelectOptimizeFiles.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonSelectOptimizeFiles.Location = new System.Drawing.Point(649, 23);
            this.buttonSelectOptimizeFiles.Name = "buttonSelectOptimizeFiles";
            this.buttonSelectOptimizeFiles.Size = new System.Drawing.Size(75, 28);
            this.buttonSelectOptimizeFiles.TabIndex = 2;
            this.buttonSelectOptimizeFiles.Text = "选择文件";
            this.buttonSelectOptimizeFiles.UseVisualStyleBackColor = true;
            //
            // checkBoxParallelProcessing
            //
            this.checkBoxParallelProcessing.AutoSize = true;
            this.checkBoxParallelProcessing.Checked = true;
            this.checkBoxParallelProcessing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxParallelProcessing.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxParallelProcessing.Location = new System.Drawing.Point(93, 57);
            this.checkBoxParallelProcessing.Name = "checkBoxParallelProcessing";
            this.checkBoxParallelProcessing.Size = new System.Drawing.Size(120, 21);
            this.checkBoxParallelProcessing.TabIndex = 3;
            this.checkBoxParallelProcessing.Text = "并行处理";
            this.checkBoxParallelProcessing.UseVisualStyleBackColor = true;
            //
            // numericUpDownMemoryThreshold
            //
            this.numericUpDownMemoryThreshold.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDownMemoryThreshold.Location = new System.Drawing.Point(250, 55);
            this.numericUpDownMemoryThreshold.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.numericUpDownMemoryThreshold.Minimum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numericUpDownMemoryThreshold.Name = "numericUpDownMemoryThreshold";
            this.numericUpDownMemoryThreshold.Size = new System.Drawing.Size(80, 23);
            this.numericUpDownMemoryThreshold.TabIndex = 4;
            this.numericUpDownMemoryThreshold.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            //
            // buttonRunOptimized
            //
            this.buttonRunOptimized.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonRunOptimized.Location = new System.Drawing.Point(17, 90);
            this.buttonRunOptimized.Name = "buttonRunOptimized";
            this.buttonRunOptimized.Size = new System.Drawing.Size(120, 30);
            this.buttonRunOptimized.TabIndex = 5;
            this.buttonRunOptimized.Text = "运行优化测试";
            this.buttonRunOptimized.UseVisualStyleBackColor = true;
            //
            // textBoxOptimizationResults
            //
            this.textBoxOptimizationResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOptimizationResults.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxOptimizationResults.Location = new System.Drawing.Point(17, 126);
            this.textBoxOptimizationResults.Multiline = true;
            this.textBoxOptimizationResults.Name = "textBoxOptimizationResults";
            this.textBoxOptimizationResults.ReadOnly = true;
            this.textBoxOptimizationResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOptimizationResults.Size = new System.Drawing.Size(807, 310);
            this.textBoxOptimizationResults.TabIndex = 6;
            //
            // progressBarOptimization
            //
            this.progressBarOptimization.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBarOptimization.Location = new System.Drawing.Point(17, 442);
            this.progressBarOptimization.Name = "progressBarOptimization";
            this.progressBarOptimization.Size = new System.Drawing.Size(300, 23);
            this.progressBarOptimization.TabIndex = 7;
            //
            // PerformanceMonitorForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 534);
            this.Controls.Add(this.tabControl1);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "PerformanceMonitorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "性能监控工具";
            this.tabControl1.ResumeLayout(false);
            this.tabPageMemory.ResumeLayout(false);
            this.groupBoxMemoryMonitoring.ResumeLayout(false);
            this.groupBoxMemoryMonitoring.PerformLayout();
            this.tabPageBenchmark.ResumeLayout(false);
            this.groupBoxBenchmark.ResumeLayout(false);
            this.groupBoxBenchmark.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIterations)).EndInit();
            this.tabPageOptimization.ResumeLayout(false);
            this.groupBoxOptimization.ResumeLayout(false);
            this.groupBoxOptimization.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMemoryThreshold)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageMemory;
        private System.Windows.Forms.TabPage tabPageBenchmark;
        private System.Windows.Forms.TabPage tabPageOptimization;
        private System.Windows.Forms.GroupBox groupBoxMemoryMonitoring;
        private System.Windows.Forms.Label labelCurrentMemory;
        private System.Windows.Forms.Label labelPeakMemory;
        private System.Windows.Forms.Label labelGCCollections;
        private System.Windows.Forms.Button buttonStartMonitoring;
        private System.Windows.Forms.Button buttonStopMonitoring;
        private System.Windows.Forms.Button buttonTriggerGC;
        private System.Windows.Forms.TextBox textBoxMemoryHistory;
        private System.Windows.Forms.GroupBox groupBoxBenchmark;
        private System.Windows.Forms.Label labelTestFiles;
        private System.Windows.Forms.TextBox textBoxTestFiles;
        private System.Windows.Forms.Button buttonSelectFiles;
        private System.Windows.Forms.NumericUpDown numericUpDownIterations;
        private System.Windows.Forms.Button buttonRunRenameBenchmark;
        private System.Windows.Forms.Button buttonRunBatchBenchmark;
        private System.Windows.Forms.Button buttonRunMemoryStress;
        private System.Windows.Forms.TextBox textBoxBenchmarkResults;
        private System.Windows.Forms.ProgressBar progressBarBenchmark;
        private System.Windows.Forms.GroupBox groupBoxOptimization;
        private System.Windows.Forms.Label labelOptimizeFiles;
        private System.Windows.Forms.TextBox textBoxOptimizeFiles;
        private System.Windows.Forms.Button buttonSelectOptimizeFiles;
        private System.Windows.Forms.CheckBox checkBoxParallelProcessing;
        private System.Windows.Forms.NumericUpDown numericUpDownMemoryThreshold;
        private System.Windows.Forms.Button buttonRunOptimized;
        private System.Windows.Forms.TextBox textBoxOptimizationResults;
        private System.Windows.Forms.ProgressBar progressBarOptimization;
    }
}