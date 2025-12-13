namespace WindowsFormsApp3
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ToolTip toolTip1;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed.</param>
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkHideRadiusValue = new System.Windows.Forms.CheckBox();
            this.txtSeparator = new System.Windows.Forms.TextBox();
            this.labelSeparator = new System.Windows.Forms.Label();
            this.lblUnit = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.labelTetBleed = new System.Windows.Forms.Label();
            this.txtTetBleed = new System.Windows.Forms.TextBox();
            this.lblMaterial = new System.Windows.Forms.Label();
            this.txtMaterial = new System.Windows.Forms.TextBox();
            this.lblZeroShapeCode = new System.Windows.Forms.Label();
            this.lblRoundShapeCode = new System.Windows.Forms.Label();
            this.lblEllipseShapeCode = new System.Windows.Forms.Label();
            this.lblCircleShapeCode = new System.Windows.Forms.Label();
            this.txtZeroShapeCode = new System.Windows.Forms.TextBox();
            this.txtRoundShapeCode = new System.Windows.Forms.TextBox();
            this.txtEllipseShapeCode = new System.Windows.Forms.TextBox();
            this.txtCircleShapeCode = new System.Windows.Forms.TextBox();
            this.trackBarOpacity = new System.Windows.Forms.TrackBar();
            this.lblOrderSettings = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnTestRegex = new System.Windows.Forms.Button();
            this.txtRegexTestResult = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRegexPattern = new System.Windows.Forms.TextBox();
            this.txtRegexName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRegexTestInput = new System.Windows.Forms.TextBox();
            this.labelHotkeyToggle = new System.Windows.Forms.Label();
            this.txtHotkeyToggle = new System.Windows.Forms.TextBox();
            this.btnRecordToggle = new System.Windows.Forms.Button();
            this.dgvRegex = new System.Windows.Forms.DataGridView();
            this.nudMaxVersions = new System.Windows.Forms.NumericUpDown();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpExportPaths = new System.Windows.Forms.GroupBox();
            this.dgvExportPaths = new System.Windows.Forms.DataGridView();
            this.btnAddExportPath = new System.Windows.Forms.Button();
            this.btnEditExportPath = new System.Windows.Forms.Button();
            this.btnDeleteExportPath = new System.Windows.Forms.Button();
            this.btnMoveUpExportPath = new System.Windows.Forms.Button();
            this.btnMoveDownExportPath = new System.Windows.Forms.Button();
            this.lblExportPathsStatus = new System.Windows.Forms.Label();
            this.grpTextSettings = new System.Windows.Forms.GroupBox();
            this.chkLstTextItems = new System.Windows.Forms.CheckedListBox();
            this.lblTextSettings = new System.Windows.Forms.Label();
            this.btnMoveUpText = new System.Windows.Forms.Button();
            this.btnMoveDownText = new System.Windows.Forms.Button();
            this.lblTextCombo = new System.Windows.Forms.Label();
            this.txtTextCombo = new System.Windows.Forms.TextBox();
            this.cboTextPresets = new System.Windows.Forms.ComboBox();
            this.lblTextPresets = new System.Windows.Forms.Label();
            this.btnSaveTextPreset = new System.Windows.Forms.Button();
            this.btnDeleteTextPreset = new System.Windows.Forms.Button();
            this.grpImpositionSettings = new System.Windows.Forms.GroupBox();
            this.chkEnableImposition = new System.Windows.Forms.CheckBox();
            this.grpFlatSheetSettings = new System.Windows.Forms.GroupBox();
            this.lblPaperSize = new System.Windows.Forms.Label();
            this.txtPaperWidth = new System.Windows.Forms.TextBox();
            this.lblWidthUnit = new System.Windows.Forms.Label();
            this.txtPaperHeight = new System.Windows.Forms.TextBox();
            this.lblHeightUnit = new System.Windows.Forms.Label();
            this.lblMargins = new System.Windows.Forms.Label();
            this.txtMarginTop = new System.Windows.Forms.TextBox();
            this.lblMarginTop = new System.Windows.Forms.Label();
            this.txtMarginBottom = new System.Windows.Forms.TextBox();
            this.lblMarginBottom = new System.Windows.Forms.Label();
            this.txtMarginLeft = new System.Windows.Forms.TextBox();
            this.lblMarginLeft = new System.Windows.Forms.Label();
            this.txtMarginRight = new System.Windows.Forms.TextBox();
            this.lblMarginRight = new System.Windows.Forms.Label();
            this.lblLayout = new System.Windows.Forms.Label();
            this.txtRows = new System.Windows.Forms.TextBox();
            this.lblRowsHint = new System.Windows.Forms.Label();
            this.txtColumns = new System.Windows.Forms.TextBox();
            this.lblColumnsHint = new System.Windows.Forms.Label();
            this.grpRollMaterialSettings = new System.Windows.Forms.GroupBox();
            this.lblMaterialSpec = new System.Windows.Forms.Label();
            this.lblFixedWidth = new System.Windows.Forms.Label();
            this.txtFixedWidth = new System.Windows.Forms.TextBox();
            this.lblFixedWidthUnit = new System.Windows.Forms.Label();
            this.lblMinLength = new System.Windows.Forms.Label();
            this.txtMinLength = new System.Windows.Forms.TextBox();
            this.lblMinLengthUnit = new System.Windows.Forms.Label();
            this.lblRollMargins = new System.Windows.Forms.Label();
            this.lblRollMarginTop = new System.Windows.Forms.Label();
            this.txtRollMarginTop = new System.Windows.Forms.TextBox();
            this.lblRollMarginBottom = new System.Windows.Forms.Label();
            this.txtRollMarginBottom = new System.Windows.Forms.TextBox();
            this.lblRollMarginLeft = new System.Windows.Forms.Label();
            this.txtRollMarginLeft = new System.Windows.Forms.TextBox();
            this.lblRollMarginRight = new System.Windows.Forms.Label();
            this.txtRollMarginRight = new System.Windows.Forms.TextBox();
            this.treeViewEvents = new WindowsFormsApp3.Controls.EventGroupsTreeView();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxVersions)).BeginInit();
            this.grpExportPaths.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExportPaths)).BeginInit();
            this.grpTextSettings.SuspendLayout();
            this.grpImpositionSettings.SuspendLayout();
            this.grpFlatSheetSettings.SuspendLayout();
            this.grpRollMaterialSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkHideRadiusValue
            // 
            this.chkHideRadiusValue.AutoSize = true;
            this.chkHideRadiusValue.Location = new System.Drawing.Point(866, 574);
            this.chkHideRadiusValue.Name = "chkHideRadiusValue";
            this.chkHideRadiusValue.Size = new System.Drawing.Size(15, 14);
            this.chkHideRadiusValue.TabIndex = 73;
            this.toolTip1.SetToolTip(this.chkHideRadiusValue, "隐藏半径数值");
            this.chkHideRadiusValue.UseVisualStyleBackColor = true;
            // 
            // txtSeparator
            // 
            this.txtSeparator.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSeparator.Location = new System.Drawing.Point(789, 474);
            this.txtSeparator.Name = "txtSeparator";
            this.txtSeparator.Size = new System.Drawing.Size(92, 23);
            this.txtSeparator.TabIndex = 28;
            // 
            // labelSeparator
            // 
            this.labelSeparator.AutoSize = true;
            this.labelSeparator.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelSeparator.Location = new System.Drawing.Point(718, 477);
            this.labelSeparator.Name = "labelSeparator";
            this.labelSeparator.Size = new System.Drawing.Size(64, 17);
            this.labelSeparator.TabIndex = 27;
            this.labelSeparator.Text = "分隔符  ：";
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(578, 103);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(41, 12);
            this.lblUnit.TabIndex = 14;
            this.lblUnit.Text = "单位：";
            // 
            // txtUnit
            // 
            this.txtUnit.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUnit.Location = new System.Drawing.Point(789, 442);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(92, 23);
            this.txtUnit.TabIndex = 15;
            // 
            // labelTetBleed
            // 
            this.labelTetBleed.AutoSize = true;
            this.labelTetBleed.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTetBleed.Location = new System.Drawing.Point(718, 509);
            this.labelTetBleed.Name = "labelTetBleed";
            this.labelTetBleed.Size = new System.Drawing.Size(64, 17);
            this.labelTetBleed.TabIndex = 23;
            this.labelTetBleed.Text = "出血值  ：";
            this.labelTetBleed.Click += new System.EventHandler(this.LabelTetBleed_Click);
            // 
            // txtTetBleed
            // 
            this.txtTetBleed.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTetBleed.Location = new System.Drawing.Point(789, 506);
            this.txtTetBleed.Name = "txtTetBleed";
            this.txtTetBleed.Size = new System.Drawing.Size(92, 23);
            this.txtTetBleed.TabIndex = 24;
            this.txtTetBleed.TextChanged += new System.EventHandler(this.TxtTetBleed_TextChanged);
            // 
            // lblMaterial
            // 
            this.lblMaterial.AutoSize = true;
            this.lblMaterial.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMaterial.Location = new System.Drawing.Point(18, 55);
            this.lblMaterial.Name = "lblMaterial";
            this.lblMaterial.Size = new System.Drawing.Size(68, 17);
            this.lblMaterial.TabIndex = 29;
            this.lblMaterial.Text = "材料设置：";
            // 
            // txtMaterial
            // 
            this.txtMaterial.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMaterial.Location = new System.Drawing.Point(92, 49);
            this.txtMaterial.Name = "txtMaterial";
            this.txtMaterial.Size = new System.Drawing.Size(406, 23);
            this.txtMaterial.TabIndex = 30;
            // 
            // lblZeroShapeCode
            // 
            this.lblZeroShapeCode.AutoSize = true;
            this.lblZeroShapeCode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblZeroShapeCode.Location = new System.Drawing.Point(718, 541);
            this.lblZeroShapeCode.Name = "lblZeroShapeCode";
            this.lblZeroShapeCode.Size = new System.Drawing.Size(68, 17);
            this.lblZeroShapeCode.TabIndex = 66;
            this.lblZeroShapeCode.Text = "直角代号：";
            // 
            // lblRoundShapeCode
            // 
            this.lblRoundShapeCode.AutoSize = true;
            this.lblRoundShapeCode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRoundShapeCode.Location = new System.Drawing.Point(718, 573);
            this.lblRoundShapeCode.Name = "lblRoundShapeCode";
            this.lblRoundShapeCode.Size = new System.Drawing.Size(68, 17);
            this.lblRoundShapeCode.TabIndex = 68;
            this.lblRoundShapeCode.Text = "圆角代号：";
            // 
            // lblEllipseShapeCode
            // 
            this.lblEllipseShapeCode.AutoSize = true;
            this.lblEllipseShapeCode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblEllipseShapeCode.Location = new System.Drawing.Point(718, 605);
            this.lblEllipseShapeCode.Name = "lblEllipseShapeCode";
            this.lblEllipseShapeCode.Size = new System.Drawing.Size(68, 17);
            this.lblEllipseShapeCode.TabIndex = 70;
            this.lblEllipseShapeCode.Text = "异形代号：";
            // 
            // lblCircleShapeCode
            // 
            this.lblCircleShapeCode.AutoSize = true;
            this.lblCircleShapeCode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCircleShapeCode.Location = new System.Drawing.Point(718, 637);
            this.lblCircleShapeCode.Name = "lblCircleShapeCode";
            this.lblCircleShapeCode.Size = new System.Drawing.Size(68, 17);
            this.lblCircleShapeCode.TabIndex = 72;
            this.lblCircleShapeCode.Text = "圆形代号：";
            // 
            // txtZeroShapeCode
            // 
            this.txtZeroShapeCode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtZeroShapeCode.Location = new System.Drawing.Point(789, 538);
            this.txtZeroShapeCode.Name = "txtZeroShapeCode";
            this.txtZeroShapeCode.Size = new System.Drawing.Size(92, 23);
            this.txtZeroShapeCode.TabIndex = 67;
            // 
            // txtRoundShapeCode
            // 
            this.txtRoundShapeCode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRoundShapeCode.Location = new System.Drawing.Point(789, 570);
            this.txtRoundShapeCode.Name = "txtRoundShapeCode";
            this.txtRoundShapeCode.Size = new System.Drawing.Size(72, 23);
            this.txtRoundShapeCode.TabIndex = 69;
            // 
            // txtEllipseShapeCode
            // 
            this.txtEllipseShapeCode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtEllipseShapeCode.Location = new System.Drawing.Point(789, 602);
            this.txtEllipseShapeCode.Name = "txtEllipseShapeCode";
            this.txtEllipseShapeCode.Size = new System.Drawing.Size(92, 23);
            this.txtEllipseShapeCode.TabIndex = 71;
            // 
            // txtCircleShapeCode
            // 
            this.txtCircleShapeCode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCircleShapeCode.Location = new System.Drawing.Point(789, 634);
            this.txtCircleShapeCode.Name = "txtCircleShapeCode";
            this.txtCircleShapeCode.Size = new System.Drawing.Size(92, 23);
            this.txtCircleShapeCode.TabIndex = 71;
            // 
            // trackBarOpacity
            // 
            this.trackBarOpacity.Location = new System.Drawing.Point(23, 695);
            this.trackBarOpacity.Maximum = 100;
            this.trackBarOpacity.Minimum = 50;
            this.trackBarOpacity.Name = "trackBarOpacity";
            this.trackBarOpacity.Size = new System.Drawing.Size(707, 45);
            this.trackBarOpacity.TabIndex = 10;
            this.trackBarOpacity.TickFrequency = 100;
            this.trackBarOpacity.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarOpacity.Value = 100;
            this.trackBarOpacity.Scroll += new System.EventHandler(this.TrackBarOpacity_Scroll);
            // 
            // lblOrderSettings
            // 
            this.lblOrderSettings.AutoSize = true;
            this.lblOrderSettings.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblOrderSettings.Location = new System.Drawing.Point(515, 31);
            this.lblOrderSettings.Name = "lblOrderSettings";
            this.lblOrderSettings.Size = new System.Drawing.Size(80, 17);
            this.lblOrderSettings.TabIndex = 42;
            this.lblOrderSettings.Text = "重命名规则：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(21, 356);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 56;
            this.label3.Text = "测试输入：";
            // 
            // btnTestRegex
            // 
            this.btnTestRegex.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTestRegex.Location = new System.Drawing.Point(445, 378);
            this.btnTestRegex.Name = "btnTestRegex";
            this.btnTestRegex.Size = new System.Drawing.Size(53, 23);
            this.btnTestRegex.TabIndex = 58;
            this.btnTestRegex.Text = "测试";
            this.btnTestRegex.UseVisualStyleBackColor = true;
            this.btnTestRegex.Click += new System.EventHandler(this.BtnTestRegex_Click);
            // 
            // txtRegexTestResult
            // 
            this.txtRegexTestResult.Location = new System.Drawing.Point(145, 378);
            this.txtRegexTestResult.Multiline = true;
            this.txtRegexTestResult.Name = "txtRegexTestResult";
            this.txtRegexTestResult.ReadOnly = true;
            this.txtRegexTestResult.Size = new System.Drawing.Size(281, 23);
            this.txtRegexTestResult.TabIndex = 59;
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDelete.Location = new System.Drawing.Point(445, 319);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(53, 23);
            this.btnDelete.TabIndex = 53;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.Location = new System.Drawing.Point(372, 319);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(54, 23);
            this.btnAdd.TabIndex = 51;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(145, 300);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 17);
            this.label2.TabIndex = 50;
            this.label2.Text = "正则表达式：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(20, 300);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 49;
            this.label1.Text = "正则名称：";
            // 
            // txtRegexPattern
            // 
            this.txtRegexPattern.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRegexPattern.Location = new System.Drawing.Point(145, 319);
            this.txtRegexPattern.Name = "txtRegexPattern";
            this.txtRegexPattern.Size = new System.Drawing.Size(203, 23);
            this.txtRegexPattern.TabIndex = 48;
            // 
            // txtRegexName
            // 
            this.txtRegexName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRegexName.Location = new System.Drawing.Point(20, 319);
            this.txtRegexName.Name = "txtRegexName";
            this.txtRegexName.Size = new System.Drawing.Size(109, 23);
            this.txtRegexName.TabIndex = 47;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(718, 445);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 17);
            this.label4.TabIndex = 60;
            this.label4.Text = "数量单位：";
            // 
            // txtRegexTestInput
            // 
            this.txtRegexTestInput.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRegexTestInput.Location = new System.Drawing.Point(21, 378);
            this.txtRegexTestInput.Name = "txtRegexTestInput";
            this.txtRegexTestInput.Size = new System.Drawing.Size(108, 23);
            this.txtRegexTestInput.TabIndex = 61;
            // 
            // labelHotkeyToggle
            // 
            this.labelHotkeyToggle.AutoSize = true;
            this.labelHotkeyToggle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelHotkeyToggle.Location = new System.Drawing.Point(718, 669);
            this.labelHotkeyToggle.Name = "labelHotkeyToggle";
            this.labelHotkeyToggle.Size = new System.Drawing.Size(56, 17);
            this.labelHotkeyToggle.TabIndex = 63;
            this.labelHotkeyToggle.Text = "最小化：";
            // 
            // txtHotkeyToggle
            // 
            this.txtHotkeyToggle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtHotkeyToggle.Location = new System.Drawing.Point(789, 666);
            this.txtHotkeyToggle.Name = "txtHotkeyToggle";
            this.txtHotkeyToggle.ReadOnly = true;
            this.txtHotkeyToggle.Size = new System.Drawing.Size(92, 23);
            this.txtHotkeyToggle.TabIndex = 64;
            // 
            // btnRecordToggle
            // 
            this.btnRecordToggle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRecordToggle.Location = new System.Drawing.Point(789, 696);
            this.btnRecordToggle.Name = "btnRecordToggle";
            this.btnRecordToggle.Size = new System.Drawing.Size(92, 23);
            this.btnRecordToggle.TabIndex = 65;
            this.btnRecordToggle.Text = "录制";
            this.btnRecordToggle.UseVisualStyleBackColor = true;
            this.btnRecordToggle.Click += new System.EventHandler(this.BtnRecordToggle_Click);
            // 
            // dgvRegex
            // 
            this.dgvRegex.AllowUserToAddRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvRegex.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRegex.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvRegex.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvRegex.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvRegex.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(115)))), ((int)(((byte)(186)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.Window;
            this.dgvRegex.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRegex.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRegex.EnableHeadersVisualStyles = false;
            this.dgvRegex.GridColor = System.Drawing.SystemColors.Control;
            this.dgvRegex.Location = new System.Drawing.Point(20, 85);
            this.dgvRegex.Margin = new System.Windows.Forms.Padding(0);
            this.dgvRegex.Name = "dgvRegex";
            this.dgvRegex.ReadOnly = true;
            this.dgvRegex.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.Window;
            this.dgvRegex.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvRegex.RowHeadersWidth = 30;
            this.dgvRegex.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            this.dgvRegex.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvRegex.Size = new System.Drawing.Size(478, 200);
            this.dgvRegex.TabIndex = 67;
            // 
            // nudMaxVersions
            // 
            this.nudMaxVersions.Location = new System.Drawing.Point(0, 0);
            this.nudMaxVersions.Name = "nudMaxVersions";
            this.nudMaxVersions.Size = new System.Drawing.Size(100, 21);
            this.nudMaxVersions.TabIndex = 0;
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSettings.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveSettings.Location = new System.Drawing.Point(962, 648);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(90, 35);
            this.btnSaveSettings.TabIndex = 84;
            this.btnSaveSettings.Text = "保存设置";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.BtnSaveSettings_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(1058, 648);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 35);
            this.btnCancel.TabIndex = 85;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // grpExportPaths
            // 
            this.grpExportPaths.Controls.Add(this.dgvExportPaths);
            this.grpExportPaths.Controls.Add(this.btnAddExportPath);
            this.grpExportPaths.Controls.Add(this.btnEditExportPath);
            this.grpExportPaths.Controls.Add(this.btnDeleteExportPath);
            this.grpExportPaths.Controls.Add(this.btnMoveUpExportPath);
            this.grpExportPaths.Controls.Add(this.btnMoveDownExportPath);
            this.grpExportPaths.Controls.Add(this.lblExportPathsStatus);
            this.grpExportPaths.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpExportPaths.Location = new System.Drawing.Point(20, 418);
            this.grpExportPaths.Name = "grpExportPaths";
            this.grpExportPaths.Size = new System.Drawing.Size(489, 271);
            this.grpExportPaths.TabIndex = 79;
            this.grpExportPaths.TabStop = false;
            this.grpExportPaths.Text = "导出路径管理";
            // 
            // dgvExportPaths
            // 
            this.dgvExportPaths.AllowUserToAddRows = false;
            this.dgvExportPaths.AllowUserToDeleteRows = false;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            this.dgvExportPaths.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvExportPaths.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvExportPaths.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvExportPaths.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvExportPaths.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(115)))), ((int)(((byte)(186)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(115)))), ((int)(((byte)(186)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.Window;
            this.dgvExportPaths.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvExportPaths.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvExportPaths.EnableHeadersVisualStyles = false;
            this.dgvExportPaths.GridColor = System.Drawing.SystemColors.Control;
            this.dgvExportPaths.Location = new System.Drawing.Point(7, 57);
            this.dgvExportPaths.Margin = new System.Windows.Forms.Padding(0);
            this.dgvExportPaths.Name = "dgvExportPaths";
            this.dgvExportPaths.ReadOnly = true;
            this.dgvExportPaths.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.Window;
            this.dgvExportPaths.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvExportPaths.RowHeadersVisible = false;
            this.dgvExportPaths.RowHeadersWidth = 30;
            this.dgvExportPaths.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White;
            this.dgvExportPaths.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvExportPaths.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvExportPaths.Size = new System.Drawing.Size(471, 208);
            this.dgvExportPaths.TabIndex = 1;
            // 
            // btnAddExportPath
            // 
            this.btnAddExportPath.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddExportPath.Location = new System.Drawing.Point(9, 22);
            this.btnAddExportPath.Name = "btnAddExportPath";
            this.btnAddExportPath.Size = new System.Drawing.Size(60, 23);
            this.btnAddExportPath.TabIndex = 2;
            this.btnAddExportPath.Text = "添加";
            this.btnAddExportPath.UseVisualStyleBackColor = true;
            this.btnAddExportPath.Click += new System.EventHandler(this.BtnAddExportPath_Click);
            // 
            // btnEditExportPath
            // 
            this.btnEditExportPath.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEditExportPath.Location = new System.Drawing.Point(82, 22);
            this.btnEditExportPath.Name = "btnEditExportPath";
            this.btnEditExportPath.Size = new System.Drawing.Size(60, 23);
            this.btnEditExportPath.TabIndex = 3;
            this.btnEditExportPath.Text = "编辑";
            this.btnEditExportPath.UseVisualStyleBackColor = true;
            this.btnEditExportPath.Click += new System.EventHandler(this.BtnEditExportPath_Click);
            // 
            // btnDeleteExportPath
            // 
            this.btnDeleteExportPath.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDeleteExportPath.Location = new System.Drawing.Point(155, 22);
            this.btnDeleteExportPath.Name = "btnDeleteExportPath";
            this.btnDeleteExportPath.Size = new System.Drawing.Size(60, 23);
            this.btnDeleteExportPath.TabIndex = 4;
            this.btnDeleteExportPath.Text = "删除";
            this.btnDeleteExportPath.UseVisualStyleBackColor = true;
            this.btnDeleteExportPath.Click += new System.EventHandler(this.BtnDeleteExportPath_Click);
            // 
            // btnMoveUpExportPath
            // 
            this.btnMoveUpExportPath.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMoveUpExportPath.Location = new System.Drawing.Point(228, 22);
            this.btnMoveUpExportPath.Name = "btnMoveUpExportPath";
            this.btnMoveUpExportPath.Size = new System.Drawing.Size(45, 23);
            this.btnMoveUpExportPath.TabIndex = 5;
            this.btnMoveUpExportPath.Text = "上移";
            this.btnMoveUpExportPath.UseVisualStyleBackColor = true;
            this.btnMoveUpExportPath.Click += new System.EventHandler(this.BtnMoveUpExportPath_Click);
            // 
            // btnMoveDownExportPath
            // 
            this.btnMoveDownExportPath.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMoveDownExportPath.Location = new System.Drawing.Point(286, 22);
            this.btnMoveDownExportPath.Name = "btnMoveDownExportPath";
            this.btnMoveDownExportPath.Size = new System.Drawing.Size(45, 23);
            this.btnMoveDownExportPath.TabIndex = 6;
            this.btnMoveDownExportPath.Text = "下移";
            this.btnMoveDownExportPath.UseVisualStyleBackColor = true;
            this.btnMoveDownExportPath.Click += new System.EventHandler(this.BtnMoveDownExportPath_Click);
            // 
            // lblExportPathsStatus
            // 
            this.lblExportPathsStatus.AutoSize = true;
            this.lblExportPathsStatus.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblExportPathsStatus.Location = new System.Drawing.Point(7, 270);
            this.lblExportPathsStatus.Name = "lblExportPathsStatus";
            this.lblExportPathsStatus.Size = new System.Drawing.Size(0, 16);
            this.lblExportPathsStatus.TabIndex = 7;
            // 
            // grpTextSettings
            // 
            this.grpTextSettings.Controls.Add(this.chkLstTextItems);
            this.grpTextSettings.Controls.Add(this.lblTextSettings);
            this.grpTextSettings.Controls.Add(this.btnMoveUpText);
            this.grpTextSettings.Controls.Add(this.btnMoveDownText);
            this.grpTextSettings.Controls.Add(this.lblTextCombo);
            this.grpTextSettings.Controls.Add(this.txtTextCombo);
            this.grpTextSettings.Controls.Add(this.cboTextPresets);
            this.grpTextSettings.Controls.Add(this.lblTextPresets);
            this.grpTextSettings.Controls.Add(this.btnSaveTextPreset);
            this.grpTextSettings.Controls.Add(this.btnDeleteTextPreset);
            this.grpTextSettings.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpTextSettings.Location = new System.Drawing.Point(738, 19);
            this.grpTextSettings.Name = "grpTextSettings";
            this.grpTextSettings.Size = new System.Drawing.Size(143, 413);
            this.grpTextSettings.TabIndex = 80;
            this.grpTextSettings.TabStop = false;
            this.grpTextSettings.Text = "PDF文字添加";
            // 
            // chkLstTextItems
            // 
            this.chkLstTextItems.AllowDrop = true;
            this.chkLstTextItems.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkLstTextItems.FormattingEnabled = true;
            this.chkLstTextItems.Items.AddRange(new object[] {
            "正则结果",
            "订单号",
            "材料",
            "数量",
            "工艺",
            "尺寸",
            "序号",
            "列组合"});
            this.chkLstTextItems.Location = new System.Drawing.Point(7, 40);
            this.chkLstTextItems.Name = "chkLstTextItems";
            this.chkLstTextItems.Size = new System.Drawing.Size(111, 166);
            this.chkLstTextItems.TabIndex = 0;
            this.chkLstTextItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChkLstTextItems_ItemCheck);
            // 
            // lblTextSettings
            // 
            this.lblTextSettings.AutoSize = true;
            this.lblTextSettings.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTextSettings.Location = new System.Drawing.Point(7, 19);
            this.lblTextSettings.Name = "lblTextSettings";
            this.lblTextSettings.Size = new System.Drawing.Size(56, 17);
            this.lblTextSettings.TabIndex = 1;
            this.lblTextSettings.Text = "文字内容";
            // 
            // btnMoveUpText
            // 
            this.btnMoveUpText.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMoveUpText.Location = new System.Drawing.Point(7, 216);
            this.btnMoveUpText.Name = "btnMoveUpText";
            this.btnMoveUpText.Size = new System.Drawing.Size(60, 23);
            this.btnMoveUpText.TabIndex = 2;
            this.btnMoveUpText.Text = "上移";
            this.btnMoveUpText.UseVisualStyleBackColor = true;
            this.btnMoveUpText.Click += new System.EventHandler(this.BtnMoveUpText_Click);
            // 
            // btnMoveDownText
            // 
            this.btnMoveDownText.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMoveDownText.Location = new System.Drawing.Point(73, 216);
            this.btnMoveDownText.Name = "btnMoveDownText";
            this.btnMoveDownText.Size = new System.Drawing.Size(60, 23);
            this.btnMoveDownText.TabIndex = 3;
            this.btnMoveDownText.Text = "下移";
            this.btnMoveDownText.UseVisualStyleBackColor = true;
            this.btnMoveDownText.Click += new System.EventHandler(this.BtnMoveDownText_Click);
            // 
            // lblTextCombo
            // 
            this.lblTextCombo.AutoSize = true;
            this.lblTextCombo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTextCombo.Location = new System.Drawing.Point(7, 250);
            this.lblTextCombo.Name = "lblTextCombo";
            this.lblTextCombo.Size = new System.Drawing.Size(56, 17);
            this.lblTextCombo.TabIndex = 4;
            this.lblTextCombo.Text = "组合内容";
            // 
            // txtTextCombo
            // 
            this.txtTextCombo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTextCombo.Location = new System.Drawing.Point(7, 270);
            this.txtTextCombo.Multiline = true;
            this.txtTextCombo.Name = "txtTextCombo";
            this.txtTextCombo.Size = new System.Drawing.Size(126, 60);
            this.txtTextCombo.TabIndex = 5;
            this.txtTextCombo.TextChanged += new System.EventHandler(this.TxtTextCombo_TextChanged);
            // 
            // cboTextPresets
            // 
            this.cboTextPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTextPresets.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboTextPresets.FormattingEnabled = true;
            this.cboTextPresets.Location = new System.Drawing.Point(7, 360);
            this.cboTextPresets.Name = "cboTextPresets";
            this.cboTextPresets.Size = new System.Drawing.Size(126, 25);
            this.cboTextPresets.TabIndex = 6;
            this.cboTextPresets.SelectedIndexChanged += new System.EventHandler(this.cboTextPresets_SelectedIndexChanged);
            // 
            // lblTextPresets
            // 
            this.lblTextPresets.AutoSize = true;
            this.lblTextPresets.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTextPresets.Location = new System.Drawing.Point(7, 336);
            this.lblTextPresets.Name = "lblTextPresets";
            this.lblTextPresets.Size = new System.Drawing.Size(51, 16);
            this.lblTextPresets.TabIndex = 7;
            this.lblTextPresets.Text = "预设方案";
            // 
            // btnSaveTextPreset
            // 
            this.btnSaveTextPreset.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveTextPreset.Location = new System.Drawing.Point(7, 391);
            this.btnSaveTextPreset.Name = "btnSaveTextPreset";
            this.btnSaveTextPreset.Size = new System.Drawing.Size(60, 23);
            this.btnSaveTextPreset.TabIndex = 8;
            this.btnSaveTextPreset.Text = "另存为";
            this.btnSaveTextPreset.UseVisualStyleBackColor = true;
            this.btnSaveTextPreset.Click += new System.EventHandler(this.btnSaveTextPreset_Click);
            // 
            // btnDeleteTextPreset
            // 
            this.btnDeleteTextPreset.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDeleteTextPreset.Location = new System.Drawing.Point(73, 391);
            this.btnDeleteTextPreset.Name = "btnDeleteTextPreset";
            this.btnDeleteTextPreset.Size = new System.Drawing.Size(60, 23);
            this.btnDeleteTextPreset.TabIndex = 9;
            this.btnDeleteTextPreset.Text = "删除";
            this.btnDeleteTextPreset.UseVisualStyleBackColor = true;
            this.btnDeleteTextPreset.Click += new System.EventHandler(this.btnDeleteTextPreset_Click);
            // 
            // grpImpositionSettings
            // 
            this.grpImpositionSettings.Controls.Add(this.chkEnableImposition);
            this.grpImpositionSettings.Controls.Add(this.grpFlatSheetSettings);
            this.grpImpositionSettings.Controls.Add(this.grpRollMaterialSettings);
            this.grpImpositionSettings.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpImpositionSettings.Location = new System.Drawing.Point(900, 19);
            this.grpImpositionSettings.Name = "grpImpositionSettings";
            this.grpImpositionSettings.Size = new System.Drawing.Size(250, 700);
            this.grpImpositionSettings.TabIndex = 81;
            this.grpImpositionSettings.TabStop = false;
            this.grpImpositionSettings.Text = "印刷排版设置";
            // 
            // chkEnableImposition
            // 
            this.chkEnableImposition.AutoSize = true;
            this.chkEnableImposition.Checked = true;
            this.chkEnableImposition.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableImposition.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkEnableImposition.Location = new System.Drawing.Point(15, 25);
            this.chkEnableImposition.Name = "chkEnableImposition";
            this.chkEnableImposition.Size = new System.Drawing.Size(123, 21);
            this.chkEnableImposition.TabIndex = 0;
            this.chkEnableImposition.Text = "启用印刷排版功能";
            this.chkEnableImposition.UseVisualStyleBackColor = true;
            this.chkEnableImposition.CheckedChanged += new System.EventHandler(this.OnImpositionSettingsChanged);
            // 
            // grpFlatSheetSettings
            // 
            this.grpFlatSheetSettings.Controls.Add(this.lblPaperSize);
            this.grpFlatSheetSettings.Controls.Add(this.txtPaperWidth);
            this.grpFlatSheetSettings.Controls.Add(this.lblWidthUnit);
            this.grpFlatSheetSettings.Controls.Add(this.txtPaperHeight);
            this.grpFlatSheetSettings.Controls.Add(this.lblHeightUnit);
            this.grpFlatSheetSettings.Controls.Add(this.lblMargins);
            this.grpFlatSheetSettings.Controls.Add(this.txtMarginTop);
            this.grpFlatSheetSettings.Controls.Add(this.lblMarginTop);
            this.grpFlatSheetSettings.Controls.Add(this.txtMarginBottom);
            this.grpFlatSheetSettings.Controls.Add(this.lblMarginBottom);
            this.grpFlatSheetSettings.Controls.Add(this.txtMarginLeft);
            this.grpFlatSheetSettings.Controls.Add(this.lblMarginLeft);
            this.grpFlatSheetSettings.Controls.Add(this.txtMarginRight);
            this.grpFlatSheetSettings.Controls.Add(this.lblMarginRight);
            this.grpFlatSheetSettings.Controls.Add(this.lblLayout);
            this.grpFlatSheetSettings.Controls.Add(this.txtRows);
            this.grpFlatSheetSettings.Controls.Add(this.lblRowsHint);
            this.grpFlatSheetSettings.Controls.Add(this.txtColumns);
            this.grpFlatSheetSettings.Controls.Add(this.lblColumnsHint);
            this.grpFlatSheetSettings.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpFlatSheetSettings.Location = new System.Drawing.Point(15, 63);
            this.grpFlatSheetSettings.Name = "grpFlatSheetSettings";
            this.grpFlatSheetSettings.Size = new System.Drawing.Size(220, 260);
            this.grpFlatSheetSettings.TabIndex = 3;
            this.grpFlatSheetSettings.TabStop = false;
            this.grpFlatSheetSettings.Text = "平张设置";
            // 
            // lblPaperSize
            // 
            this.lblPaperSize.AutoSize = true;
            this.lblPaperSize.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPaperSize.Location = new System.Drawing.Point(10, 25);
            this.lblPaperSize.Name = "lblPaperSize";
            this.lblPaperSize.Size = new System.Drawing.Size(59, 17);
            this.lblPaperSize.TabIndex = 0;
            this.lblPaperSize.Text = "纸张尺寸:";
            // 
            // txtPaperWidth
            // 
            this.txtPaperWidth.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPaperWidth.Location = new System.Drawing.Point(75, 22);
            this.txtPaperWidth.Name = "txtPaperWidth";
            this.txtPaperWidth.Size = new System.Drawing.Size(50, 23);
            this.txtPaperWidth.TabIndex = 1;
            this.txtPaperWidth.TextChanged += new System.EventHandler(this.OnImpositionParametersChanged);
            // 
            // lblWidthUnit
            // 
            this.lblWidthUnit.AutoSize = true;
            this.lblWidthUnit.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWidthUnit.Location = new System.Drawing.Point(130, 25);
            this.lblWidthUnit.Name = "lblWidthUnit";
            this.lblWidthUnit.Size = new System.Drawing.Size(30, 17);
            this.lblWidthUnit.TabIndex = 2;
            this.lblWidthUnit.Text = "mm";
            // 
            // txtPaperHeight
            // 
            this.txtPaperHeight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPaperHeight.Location = new System.Drawing.Point(165, 22);
            this.txtPaperHeight.Name = "txtPaperHeight";
            this.txtPaperHeight.Size = new System.Drawing.Size(50, 23);
            this.txtPaperHeight.TabIndex = 3;
            this.txtPaperHeight.TextChanged += new System.EventHandler(this.OnImpositionParametersChanged);
            // 
            // lblHeightUnit
            // 
            this.lblHeightUnit.AutoSize = true;
            this.lblHeightUnit.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblHeightUnit.Location = new System.Drawing.Point(220, 25);
            this.lblHeightUnit.Name = "lblHeightUnit";
            this.lblHeightUnit.Size = new System.Drawing.Size(30, 17);
            this.lblHeightUnit.TabIndex = 4;
            this.lblHeightUnit.Text = "mm";
            // 
            // lblMargins
            // 
            this.lblMargins.AutoSize = true;
            this.lblMargins.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMargins.Location = new System.Drawing.Point(10, 66);
            this.lblMargins.Name = "lblMargins";
            this.lblMargins.Size = new System.Drawing.Size(47, 17);
            this.lblMargins.TabIndex = 5;
            this.lblMargins.Text = "页边距:";
            // 
            // txtMarginTop
            // 
            this.txtMarginTop.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMarginTop.Location = new System.Drawing.Point(40, 97);
            this.txtMarginTop.Name = "txtMarginTop";
            this.txtMarginTop.Size = new System.Drawing.Size(40, 23);
            this.txtMarginTop.TabIndex = 6;
            this.txtMarginTop.TextChanged += new System.EventHandler(this.OnImpositionParametersChanged);
            // 
            // lblMarginTop
            // 
            this.lblMarginTop.AutoSize = true;
            this.lblMarginTop.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMarginTop.Location = new System.Drawing.Point(14, 100);
            this.lblMarginTop.Name = "lblMarginTop";
            this.lblMarginTop.Size = new System.Drawing.Size(20, 17);
            this.lblMarginTop.TabIndex = 7;
            this.lblMarginTop.Text = "上";
            // 
            // txtMarginBottom
            // 
            this.txtMarginBottom.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMarginBottom.Location = new System.Drawing.Point(130, 97);
            this.txtMarginBottom.Name = "txtMarginBottom";
            this.txtMarginBottom.Size = new System.Drawing.Size(40, 23);
            this.txtMarginBottom.TabIndex = 8;
            this.txtMarginBottom.TextChanged += new System.EventHandler(this.OnImpositionParametersChanged);
            // 
            // lblMarginBottom
            // 
            this.lblMarginBottom.AutoSize = true;
            this.lblMarginBottom.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMarginBottom.Location = new System.Drawing.Point(104, 100);
            this.lblMarginBottom.Name = "lblMarginBottom";
            this.lblMarginBottom.Size = new System.Drawing.Size(20, 17);
            this.lblMarginBottom.TabIndex = 9;
            this.lblMarginBottom.Text = "下";
            // 
            // txtMarginLeft
            // 
            this.txtMarginLeft.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMarginLeft.Location = new System.Drawing.Point(40, 127);
            this.txtMarginLeft.Name = "txtMarginLeft";
            this.txtMarginLeft.Size = new System.Drawing.Size(40, 23);
            this.txtMarginLeft.TabIndex = 10;
            this.txtMarginLeft.TextChanged += new System.EventHandler(this.OnImpositionParametersChanged);
            // 
            // lblMarginLeft
            // 
            this.lblMarginLeft.AutoSize = true;
            this.lblMarginLeft.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMarginLeft.Location = new System.Drawing.Point(14, 130);
            this.lblMarginLeft.Name = "lblMarginLeft";
            this.lblMarginLeft.Size = new System.Drawing.Size(20, 17);
            this.lblMarginLeft.TabIndex = 11;
            this.lblMarginLeft.Text = "左";
            // 
            // txtMarginRight
            // 
            this.txtMarginRight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMarginRight.Location = new System.Drawing.Point(130, 127);
            this.txtMarginRight.Name = "txtMarginRight";
            this.txtMarginRight.Size = new System.Drawing.Size(40, 23);
            this.txtMarginRight.TabIndex = 12;
            this.txtMarginRight.TextChanged += new System.EventHandler(this.OnImpositionParametersChanged);
            // 
            // lblMarginRight
            // 
            this.lblMarginRight.AutoSize = true;
            this.lblMarginRight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMarginRight.Location = new System.Drawing.Point(104, 130);
            this.lblMarginRight.Name = "lblMarginRight";
            this.lblMarginRight.Size = new System.Drawing.Size(20, 17);
            this.lblMarginRight.TabIndex = 13;
            this.lblMarginRight.Text = "右";
            // 
            // lblLayout
            // 
            this.lblLayout.AutoSize = true;
            this.lblLayout.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLayout.Location = new System.Drawing.Point(10, 170);
            this.lblLayout.Name = "lblLayout";
            this.lblLayout.Size = new System.Drawing.Size(59, 17);
            this.lblLayout.TabIndex = 14;
            this.lblLayout.Text = "布局设置:";
            // 
            // txtRows
            // 
            this.txtRows.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRows.Location = new System.Drawing.Point(75, 167);
            this.txtRows.Name = "txtRows";
            this.txtRows.Size = new System.Drawing.Size(40, 23);
            this.txtRows.TabIndex = 15;
            this.txtRows.TextChanged += new System.EventHandler(this.OnImpositionParametersChanged);
            // 
            // lblRowsHint
            // 
            this.lblRowsHint.AutoSize = true;
            this.lblRowsHint.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRowsHint.Location = new System.Drawing.Point(120, 170);
            this.lblRowsHint.Name = "lblRowsHint";
            this.lblRowsHint.Size = new System.Drawing.Size(65, 16);
            this.lblRowsHint.TabIndex = 16;
            this.lblRowsHint.Text = "行 (0=自动)";
            // 
            // txtColumns
            // 
            this.txtColumns.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtColumns.Location = new System.Drawing.Point(75, 197);
            this.txtColumns.Name = "txtColumns";
            this.txtColumns.Size = new System.Drawing.Size(40, 23);
            this.txtColumns.TabIndex = 17;
            this.txtColumns.TextChanged += new System.EventHandler(this.OnImpositionParametersChanged);
            // 
            // lblColumnsHint
            // 
            this.lblColumnsHint.AutoSize = true;
            this.lblColumnsHint.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblColumnsHint.Location = new System.Drawing.Point(120, 200);
            this.lblColumnsHint.Name = "lblColumnsHint";
            this.lblColumnsHint.Size = new System.Drawing.Size(65, 16);
            this.lblColumnsHint.TabIndex = 18;
            this.lblColumnsHint.Text = "列 (0=自动)";
            // 
            // grpRollMaterialSettings
            // 
            this.grpRollMaterialSettings.Controls.Add(this.lblMaterialSpec);
            this.grpRollMaterialSettings.Controls.Add(this.lblFixedWidth);
            this.grpRollMaterialSettings.Controls.Add(this.txtFixedWidth);
            this.grpRollMaterialSettings.Controls.Add(this.lblFixedWidthUnit);
            this.grpRollMaterialSettings.Controls.Add(this.lblMinLength);
            this.grpRollMaterialSettings.Controls.Add(this.txtMinLength);
            this.grpRollMaterialSettings.Controls.Add(this.lblMinLengthUnit);
            this.grpRollMaterialSettings.Controls.Add(this.lblRollMargins);
            this.grpRollMaterialSettings.Controls.Add(this.lblRollMarginTop);
            this.grpRollMaterialSettings.Controls.Add(this.txtRollMarginTop);
            this.grpRollMaterialSettings.Controls.Add(this.lblRollMarginBottom);
            this.grpRollMaterialSettings.Controls.Add(this.txtRollMarginBottom);
            this.grpRollMaterialSettings.Controls.Add(this.lblRollMarginLeft);
            this.grpRollMaterialSettings.Controls.Add(this.txtRollMarginLeft);
            this.grpRollMaterialSettings.Controls.Add(this.lblRollMarginRight);
            this.grpRollMaterialSettings.Controls.Add(this.txtRollMarginRight);
            this.grpRollMaterialSettings.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpRollMaterialSettings.Location = new System.Drawing.Point(15, 346);
            this.grpRollMaterialSettings.Name = "grpRollMaterialSettings";
            this.grpRollMaterialSettings.Size = new System.Drawing.Size(220, 180);
            this.grpRollMaterialSettings.TabIndex = 4;
            this.grpRollMaterialSettings.TabStop = false;
            this.grpRollMaterialSettings.Text = "卷装设置";
            // 
            // lblMaterialSpec
            // 
            this.lblMaterialSpec.AutoSize = true;
            this.lblMaterialSpec.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMaterialSpec.Location = new System.Drawing.Point(10, 25);
            this.lblMaterialSpec.Name = "lblMaterialSpec";
            this.lblMaterialSpec.Size = new System.Drawing.Size(59, 17);
            this.lblMaterialSpec.TabIndex = 0;
            this.lblMaterialSpec.Text = "材料规格:";
            // 
            // lblFixedWidth
            // 
            this.lblFixedWidth.AutoSize = true;
            this.lblFixedWidth.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFixedWidth.Location = new System.Drawing.Point(10, 50);
            this.lblFixedWidth.Name = "lblFixedWidth";
            this.lblFixedWidth.Size = new System.Drawing.Size(59, 17);
            this.lblFixedWidth.TabIndex = 1;
            this.lblFixedWidth.Text = "固定宽度:";
            // 
            // txtFixedWidth
            // 
            this.txtFixedWidth.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFixedWidth.Location = new System.Drawing.Point(75, 47);
            this.txtFixedWidth.Name = "txtFixedWidth";
            this.txtFixedWidth.Size = new System.Drawing.Size(50, 23);
            this.txtFixedWidth.TabIndex = 2;
            this.txtFixedWidth.TextChanged += new System.EventHandler(this.OnImpositionParametersChanged);
            // 
            // lblFixedWidthUnit
            // 
            this.lblFixedWidthUnit.AutoSize = true;
            this.lblFixedWidthUnit.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFixedWidthUnit.Location = new System.Drawing.Point(130, 50);
            this.lblFixedWidthUnit.Name = "lblFixedWidthUnit";
            this.lblFixedWidthUnit.Size = new System.Drawing.Size(30, 17);
            this.lblFixedWidthUnit.TabIndex = 3;
            this.lblFixedWidthUnit.Text = "mm";
            // 
            // lblMinLength
            // 
            this.lblMinLength.AutoSize = true;
            this.lblMinLength.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMinLength.Location = new System.Drawing.Point(10, 77);
            this.lblMinLength.Name = "lblMinLength";
            this.lblMinLength.Size = new System.Drawing.Size(59, 17);
            this.lblMinLength.TabIndex = 4;
            this.lblMinLength.Text = "最小长度:";
            // 
            // txtMinLength
            // 
            this.txtMinLength.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMinLength.Location = new System.Drawing.Point(75, 74);
            this.txtMinLength.Name = "txtMinLength";
            this.txtMinLength.Size = new System.Drawing.Size(50, 23);
            this.txtMinLength.TabIndex = 5;
            this.txtMinLength.TextChanged += new System.EventHandler(this.OnImpositionParametersChanged);
            // 
            // lblMinLengthUnit
            // 
            this.lblMinLengthUnit.AutoSize = true;
            this.lblMinLengthUnit.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMinLengthUnit.Location = new System.Drawing.Point(130, 77);
            this.lblMinLengthUnit.Name = "lblMinLengthUnit";
            this.lblMinLengthUnit.Size = new System.Drawing.Size(30, 17);
            this.lblMinLengthUnit.TabIndex = 6;
            this.lblMinLengthUnit.Text = "mm";
            // 
            // lblRollMargins
            // 
            this.lblRollMargins.AutoSize = true;
            this.lblRollMargins.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRollMargins.Location = new System.Drawing.Point(10, 105);
            this.lblRollMargins.Name = "lblRollMargins";
            this.lblRollMargins.Size = new System.Drawing.Size(47, 17);
            this.lblRollMargins.TabIndex = 7;
            this.lblRollMargins.Text = "页边距:";
            // 
            // lblRollMarginTop
            // 
            this.lblRollMarginTop.AutoSize = true;
            this.lblRollMarginTop.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRollMarginTop.Location = new System.Drawing.Point(10, 127);
            this.lblRollMarginTop.Name = "lblRollMarginTop";
            this.lblRollMarginTop.Size = new System.Drawing.Size(23, 17);
            this.lblRollMarginTop.TabIndex = 8;
            this.lblRollMarginTop.Text = "上:";
            // 
            // txtRollMarginTop
            // 
            this.txtRollMarginTop.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRollMarginTop.Location = new System.Drawing.Point(40, 127);
            this.txtRollMarginTop.Name = "txtRollMarginTop";
            this.txtRollMarginTop.Size = new System.Drawing.Size(35, 23);
            this.txtRollMarginTop.TabIndex = 16;
            this.txtRollMarginTop.TextChanged += new System.EventHandler(this.OnImpositionParametersChanged);
            // 
            // lblRollMarginBottom
            // 
            this.lblRollMarginBottom.AutoSize = true;
            this.lblRollMarginBottom.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRollMarginBottom.Location = new System.Drawing.Point(75, 127);
            this.lblRollMarginBottom.Name = "lblRollMarginBottom";
            this.lblRollMarginBottom.Size = new System.Drawing.Size(23, 17);
            this.lblRollMarginBottom.TabIndex = 10;
            this.lblRollMarginBottom.Text = "下:";
            // 
            // txtRollMarginBottom
            // 
            this.txtRollMarginBottom.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRollMarginBottom.Location = new System.Drawing.Point(105, 127);
            this.txtRollMarginBottom.Name = "txtRollMarginBottom";
            this.txtRollMarginBottom.Size = new System.Drawing.Size(35, 23);
            this.txtRollMarginBottom.TabIndex = 17;
            this.txtRollMarginBottom.TextChanged += new System.EventHandler(this.OnImpositionParametersChanged);
            // 
            // lblRollMarginLeft
            // 
            this.lblRollMarginLeft.AutoSize = true;
            this.lblRollMarginLeft.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRollMarginLeft.Location = new System.Drawing.Point(10, 152);
            this.lblRollMarginLeft.Name = "lblRollMarginLeft";
            this.lblRollMarginLeft.Size = new System.Drawing.Size(23, 17);
            this.lblRollMarginLeft.TabIndex = 12;
            this.lblRollMarginLeft.Text = "左:";
            // 
            // txtRollMarginLeft
            // 
            this.txtRollMarginLeft.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRollMarginLeft.Location = new System.Drawing.Point(40, 152);
            this.txtRollMarginLeft.Name = "txtRollMarginLeft";
            this.txtRollMarginLeft.Size = new System.Drawing.Size(35, 23);
            this.txtRollMarginLeft.TabIndex = 18;
            this.txtRollMarginLeft.TextChanged += new System.EventHandler(this.OnImpositionParametersChanged);
            // 
            // lblRollMarginRight
            // 
            this.lblRollMarginRight.AutoSize = true;
            this.lblRollMarginRight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRollMarginRight.Location = new System.Drawing.Point(75, 152);
            this.lblRollMarginRight.Name = "lblRollMarginRight";
            this.lblRollMarginRight.Size = new System.Drawing.Size(23, 17);
            this.lblRollMarginRight.TabIndex = 14;
            this.lblRollMarginRight.Text = "右:";
            // 
            // txtRollMarginRight
            // 
            this.txtRollMarginRight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRollMarginRight.Location = new System.Drawing.Point(105, 152);
            this.txtRollMarginRight.Name = "txtRollMarginRight";
            this.txtRollMarginRight.Size = new System.Drawing.Size(35, 23);
            this.txtRollMarginRight.TabIndex = 19;
            this.txtRollMarginRight.TextChanged += new System.EventHandler(this.OnImpositionParametersChanged);
            // 
            // treeViewEvents
            // 
            this.treeViewEvents.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeViewEvents.Location = new System.Drawing.Point(515, 51);
            this.treeViewEvents.Name = "treeViewEvents";
            this.treeViewEvents.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.treeViewEvents.SelectedPreset = null;
            this.treeViewEvents.Size = new System.Drawing.Size(197, 632);
            this.treeViewEvents.TabIndex = 45;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1162, 752);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.grpExportPaths);
            this.Controls.Add(this.grpTextSettings);
            this.Controls.Add(this.grpImpositionSettings);
            this.Controls.Add(this.lblZeroShapeCode);
            this.Controls.Add(this.txtZeroShapeCode);
            this.Controls.Add(this.lblRoundShapeCode);
            this.Controls.Add(this.txtRoundShapeCode);
            this.Controls.Add(this.lblEllipseShapeCode);
            this.Controls.Add(this.txtEllipseShapeCode);
            this.Controls.Add(this.lblCircleShapeCode);
            this.Controls.Add(this.txtCircleShapeCode);
            this.Controls.Add(this.chkHideRadiusValue);
            this.Controls.Add(this.dgvRegex);
            this.Controls.Add(this.labelHotkeyToggle);
            this.Controls.Add(this.txtHotkeyToggle);
            this.Controls.Add(this.btnRecordToggle);
            this.Controls.Add(this.txtRegexTestInput);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnTestRegex);
            this.Controls.Add(this.txtRegexTestResult);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRegexPattern);
            this.Controls.Add(this.txtRegexName);
            this.Controls.Add(this.treeViewEvents);
            this.Controls.Add(this.lblOrderSettings);
            this.Controls.Add(this.trackBarOpacity);
            this.Controls.Add(this.lblMaterial);
            this.Controls.Add(this.txtMaterial);
            this.Controls.Add(this.txtSeparator);
            this.Controls.Add(this.labelSeparator);
            this.Controls.Add(this.txtUnit);
            this.Controls.Add(this.labelTetBleed);
            this.Controls.Add(this.txtTetBleed);
            this.Name = "SettingsForm";
            this.Text = "设置";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxVersions)).EndInit();
            this.grpExportPaths.ResumeLayout(false);
            this.grpExportPaths.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExportPaths)).EndInit();
            this.grpTextSettings.ResumeLayout(false);
            this.grpTextSettings.PerformLayout();
            this.grpImpositionSettings.ResumeLayout(false);
            this.grpImpositionSettings.PerformLayout();
            this.grpFlatSheetSettings.ResumeLayout(false);
            this.grpFlatSheetSettings.PerformLayout();
            this.grpRollMaterialSettings.ResumeLayout(false);
            this.grpRollMaterialSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label labelTetBleed;
        private System.Windows.Forms.TextBox txtTetBleed;
        private System.Windows.Forms.Label labelSeparator;
        private System.Windows.Forms.TextBox txtSeparator;
        private System.Windows.Forms.Label lblMaterial;
        private System.Windows.Forms.TextBox txtMaterial;
        private System.Windows.Forms.TrackBar trackBarOpacity;
        private WindowsFormsApp3.Controls.EventGroupsTreeView treeViewEvents; // 重命名规则树形控件
        private System.Windows.Forms.Label lblOrderSettings; // 重命名规则标签
        private System.Windows.Forms.TextBox txtRegexName;
        private System.Windows.Forms.TextBox txtRegexPattern;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnTestRegex;
        private System.Windows.Forms.TextBox txtRegexTestResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRegexTestInput;
        private System.Windows.Forms.Label labelHotkeyToggle;
        private System.Windows.Forms.TextBox txtHotkeyToggle;
        private System.Windows.Forms.Button btnRecordToggle;
        private System.Windows.Forms.DataGridView dgvRegex;
          private System.Windows.Forms.GroupBox grpExportPaths;
        private System.Windows.Forms.DataGridView dgvExportPaths;
        private System.Windows.Forms.Button btnAddExportPath;
        private System.Windows.Forms.Button btnEditExportPath;
        private System.Windows.Forms.Button btnDeleteExportPath;
        private System.Windows.Forms.Button btnMoveUpExportPath;
        private System.Windows.Forms.Button btnMoveDownExportPath;
        private System.Windows.Forms.Label lblExportPathsStatus;
        private System.Windows.Forms.CheckBox chkHideRadiusValue;
        private System.Windows.Forms.TextBox txtZeroShapeCode;
        private System.Windows.Forms.TextBox txtRoundShapeCode;
        private System.Windows.Forms.TextBox txtEllipseShapeCode;
        private System.Windows.Forms.TextBox txtCircleShapeCode;
        private System.Windows.Forms.Label lblZeroShapeCode;
        private System.Windows.Forms.Label lblRoundShapeCode;
        private System.Windows.Forms.Label lblEllipseShapeCode;
        private System.Windows.Forms.Label lblCircleShapeCode;
        private System.Windows.Forms.NumericUpDown nudMaxVersions;
                  private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpTextSettings;
        private System.Windows.Forms.GroupBox grpImpositionSettings;
        private System.Windows.Forms.CheckBox chkEnableImposition;
            private System.Windows.Forms.GroupBox grpFlatSheetSettings;
        private System.Windows.Forms.Label lblPaperSize;
        private System.Windows.Forms.TextBox txtPaperWidth;
        private System.Windows.Forms.Label lblWidthUnit;
        private System.Windows.Forms.TextBox txtPaperHeight;
        private System.Windows.Forms.Label lblHeightUnit;
        private System.Windows.Forms.Label lblMargins;
        private System.Windows.Forms.TextBox txtMarginTop;
        private System.Windows.Forms.Label lblMarginTop;
        private System.Windows.Forms.TextBox txtMarginBottom;
        private System.Windows.Forms.Label lblMarginBottom;
        private System.Windows.Forms.TextBox txtMarginLeft;
        private System.Windows.Forms.Label lblMarginLeft;
        private System.Windows.Forms.TextBox txtMarginRight;
        private System.Windows.Forms.Label lblMarginRight;
        private System.Windows.Forms.Label lblLayout;
        private System.Windows.Forms.TextBox txtRows;
        private System.Windows.Forms.Label lblRowsHint;
        private System.Windows.Forms.TextBox txtColumns;
        private System.Windows.Forms.Label lblColumnsHint;
        private System.Windows.Forms.GroupBox grpRollMaterialSettings;
        private System.Windows.Forms.Label lblMaterialSpec;
        private System.Windows.Forms.Label lblFixedWidth;
        private System.Windows.Forms.TextBox txtFixedWidth;
        private System.Windows.Forms.Label lblFixedWidthUnit;
        private System.Windows.Forms.Label lblMinLength;
        private System.Windows.Forms.TextBox txtMinLength;
        private System.Windows.Forms.Label lblMinLengthUnit;
        private System.Windows.Forms.Label lblRollMargins;
        private System.Windows.Forms.Label lblRollMarginTop;
        private System.Windows.Forms.Label lblRollMarginBottom;
        private System.Windows.Forms.Label lblRollMarginLeft;
        private System.Windows.Forms.Label lblRollMarginRight;
        private System.Windows.Forms.TextBox txtRollMarginTop;
        private System.Windows.Forms.TextBox txtRollMarginBottom;
        private System.Windows.Forms.TextBox txtRollMarginLeft;
        private System.Windows.Forms.TextBox txtRollMarginRight;

        private System.Windows.Forms.CheckedListBox chkLstTextItems;
        private System.Windows.Forms.Label lblTextSettings;
        private System.Windows.Forms.Button btnMoveUpText;
        private System.Windows.Forms.Button btnMoveDownText;
        private System.Windows.Forms.Label lblTextCombo;
        private System.Windows.Forms.TextBox txtTextCombo;
        private System.Windows.Forms.ComboBox cboTextPresets;
        private System.Windows.Forms.Label lblTextPresets;
        private System.Windows.Forms.Button btnSaveTextPreset;
        private System.Windows.Forms.Button btnDeleteTextPreset;
    }
}