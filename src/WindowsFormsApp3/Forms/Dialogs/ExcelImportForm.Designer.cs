namespace WindowsFormsApp3 {
    partial class ExcelImportForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbReturnColumn = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSearchColumn = new System.Windows.Forms.ComboBox();
            this.cmbNewColumn = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkImportSerialColumn = new System.Windows.Forms.CheckBox();
            this.dgvPreview = new System.Windows.Forms.DataGridView();
            this.txtReturnColumnParams = new System.Windows.Forms.TextBox();
            this.txtSearchColumnParams = new System.Windows.Forms.TextBox();
            this.txtNewColumnParams = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbRegex2 = new System.Windows.Forms.ComboBox();
            this.clbCompositeColumns = new System.Windows.Forms.CheckedListBox();
            this.txtCompositeSeparator = new System.Windows.Forms.TextBox();
            this.btnSelectAllColumns = new System.Windows.Forms.Button();
            this.btnClearAllColumns = new System.Windows.Forms.Button();
            this.labelCompositeColumns = new System.Windows.Forms.Label();
            this.labelSeparator = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "已导入Excel数据：";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(21, 559);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(113, 559);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 468);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "数量列选择    ：";
            // 
            // cmbReturnColumn
            // 
            this.cmbReturnColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReturnColumn.FormattingEnabled = true;
            this.cmbReturnColumn.Location = new System.Drawing.Point(127, 465);
            this.cmbReturnColumn.Name = "cmbReturnColumn";
            this.cmbReturnColumn.Size = new System.Drawing.Size(175, 20);
            this.cmbReturnColumn.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 431);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "搜索列选择    ：";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // cmbSearchColumn
            // 
            this.cmbSearchColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchColumn.FormattingEnabled = true;
            this.cmbSearchColumn.Location = new System.Drawing.Point(127, 428);
            this.cmbSearchColumn.Name = "cmbSearchColumn";
            this.cmbSearchColumn.Size = new System.Drawing.Size(175, 20);
            this.cmbSearchColumn.TabIndex = 3;
            // 
            // cmbNewColumn
            // 
            this.cmbNewColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNewColumn.FormattingEnabled = true;
            this.cmbNewColumn.Location = new System.Drawing.Point(127, 387);
            this.cmbNewColumn.Name = "cmbNewColumn";
            this.cmbNewColumn.Size = new System.Drawing.Size(175, 20);
            this.cmbNewColumn.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 390);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "序号列选择    ：";
            // 
            // chkImportSerialColumn
            // 
            this.chkImportSerialColumn.AutoSize = true;
            this.chkImportSerialColumn.Checked = true;
            this.chkImportSerialColumn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImportSerialColumn.Location = new System.Drawing.Point(127, 359);
            this.chkImportSerialColumn.Name = "chkImportSerialColumn";
            this.chkImportSerialColumn.Size = new System.Drawing.Size(108, 16);
            this.chkImportSerialColumn.TabIndex = 6;
            this.chkImportSerialColumn.Text = "导入序号列数据";
            this.chkImportSerialColumn.UseVisualStyleBackColor = true;
            // 
            // dgvPreview
            // 
            this.dgvPreview.AllowUserToAddRows = false;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            this.dgvPreview.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvPreview.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPreview.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvPreview.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvPreview.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(115)))), ((int)(((byte)(186)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPreview.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPreview.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvPreview.EnableHeadersVisualStyles = false;
            this.dgvPreview.GridColor = System.Drawing.SystemColors.Control;
            this.dgvPreview.Location = new System.Drawing.Point(12, 41);
            this.dgvPreview.Margin = new System.Windows.Forms.Padding(0);
            this.dgvPreview.Name = "dgvPreview";
            this.dgvPreview.ReadOnly = true;
            this.dgvPreview.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.ButtonFace;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPreview.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvPreview.RowHeadersWidth = 30;
            this.dgvPreview.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvPreview.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvPreview.RowTemplate.Height = 23;
            this.dgvPreview.Size = new System.Drawing.Size(649, 300);
            this.dgvPreview.TabIndex = 7;
            // 
            // txtReturnColumnParams
            // 
            this.txtReturnColumnParams.Location = new System.Drawing.Point(308, 465);
            this.txtReturnColumnParams.Name = "txtReturnColumnParams";
            this.txtReturnColumnParams.Size = new System.Drawing.Size(150, 21);
            this.txtReturnColumnParams.TabIndex = 9;
            // 
            // txtSearchColumnParams
            // 
            this.txtSearchColumnParams.Location = new System.Drawing.Point(308, 428);
            this.txtSearchColumnParams.Name = "txtSearchColumnParams";
            this.txtSearchColumnParams.Size = new System.Drawing.Size(150, 21);
            this.txtSearchColumnParams.TabIndex = 8;
            // 
            // txtNewColumnParams
            // 
            this.txtNewColumnParams.Location = new System.Drawing.Point(308, 387);
            this.txtNewColumnParams.Name = "txtNewColumnParams";
            this.txtNewColumnParams.Size = new System.Drawing.Size(150, 21);
            this.txtNewColumnParams.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 510);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "正则表达式选择：";
            // 
            // cmbRegex2
            // 
            this.cmbRegex2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRegex2.FormattingEnabled = true;
            this.cmbRegex2.Location = new System.Drawing.Point(127, 507);
            this.cmbRegex2.Name = "cmbRegex2";
            this.cmbRegex2.Size = new System.Drawing.Size(175, 20);
            this.cmbRegex2.TabIndex = 11;
            // 
            // clbCompositeColumns
            // 
            this.clbCompositeColumns.FormattingEnabled = true;
            this.clbCompositeColumns.Location = new System.Drawing.Point(480, 387);
            this.clbCompositeColumns.Name = "clbCompositeColumns";
            this.clbCompositeColumns.Size = new System.Drawing.Size(168, 132);
            this.clbCompositeColumns.TabIndex = 10;
            // 
            // txtCompositeSeparator
            // 
            this.txtCompositeSeparator.Location = new System.Drawing.Point(367, 507);
            this.txtCompositeSeparator.Name = "txtCompositeSeparator";
            this.txtCompositeSeparator.Size = new System.Drawing.Size(91, 21);
            this.txtCompositeSeparator.TabIndex = 11;
            this.txtCompositeSeparator.Text = ",";
            // 
            // btnSelectAllColumns
            // 
            this.btnSelectAllColumns.Location = new System.Drawing.Point(571, 527);
            this.btnSelectAllColumns.Name = "btnSelectAllColumns";
            this.btnSelectAllColumns.Size = new System.Drawing.Size(77, 23);
            this.btnSelectAllColumns.TabIndex = 12;
            this.btnSelectAllColumns.Text = "全选";
            this.btnSelectAllColumns.UseVisualStyleBackColor = true;
            this.btnSelectAllColumns.Click += new System.EventHandler(this.btnSelectAllColumns_Click);
            // 
            // btnClearAllColumns
            // 
            this.btnClearAllColumns.Location = new System.Drawing.Point(571, 559);
            this.btnClearAllColumns.Name = "btnClearAllColumns";
            this.btnClearAllColumns.Size = new System.Drawing.Size(77, 23);
            this.btnClearAllColumns.TabIndex = 13;
            this.btnClearAllColumns.Text = "取消全选";
            this.btnClearAllColumns.UseVisualStyleBackColor = true;
            this.btnClearAllColumns.Click += new System.EventHandler(this.btnClearAllColumns_Click);
            // 
            // labelCompositeColumns
            // 
            this.labelCompositeColumns.AutoSize = true;
            this.labelCompositeColumns.Location = new System.Drawing.Point(480, 367);
            this.labelCompositeColumns.Name = "labelCompositeColumns";
            this.labelCompositeColumns.Size = new System.Drawing.Size(77, 12);
            this.labelCompositeColumns.TabIndex = 14;
            this.labelCompositeColumns.Text = "选择组合列：";
            // 
            // labelSeparator
            // 
            this.labelSeparator.AutoSize = true;
            this.labelSeparator.Location = new System.Drawing.Point(308, 510);
            this.labelSeparator.Name = "labelSeparator";
            this.labelSeparator.Size = new System.Drawing.Size(53, 12);
            this.labelSeparator.TabIndex = 15;
            this.labelSeparator.Text = "分隔符：";
            // 
            // ExcelImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 609);
            this.Controls.Add(this.dgvPreview);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cmbSearchColumn);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbReturnColumn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbNewColumn);
            this.Controls.Add(this.chkImportSerialColumn);
            this.Controls.Add(this.txtNewColumnParams);
            this.Controls.Add(this.txtSearchColumnParams);
            this.Controls.Add(this.txtReturnColumnParams);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbRegex2);
            this.Controls.Add(this.clbCompositeColumns);
            this.Controls.Add(this.txtCompositeSeparator);
            this.Controls.Add(this.btnSelectAllColumns);
            this.Controls.Add(this.btnClearAllColumns);
            this.Controls.Add(this.labelCompositeColumns);
            this.Controls.Add(this.labelSeparator);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExcelImportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Excel导入设置";
            this.Load += new System.EventHandler(this.ExcelImportForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbReturnColumn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbSearchColumn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbNewColumn;
        private System.Windows.Forms.CheckBox chkImportSerialColumn;
        private System.Windows.Forms.DataGridView dgvPreview;
        private System.Windows.Forms.TextBox txtNewColumnParams;
        private System.Windows.Forms.TextBox txtSearchColumnParams;
        private System.Windows.Forms.TextBox txtReturnColumnParams;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbRegex2;
        // 列组合控件声明
        private System.Windows.Forms.CheckedListBox clbCompositeColumns;
        private System.Windows.Forms.TextBox txtCompositeSeparator;
        private System.Windows.Forms.Button btnSelectAllColumns;
        private System.Windows.Forms.Button btnClearAllColumns;
        private System.Windows.Forms.Label labelCompositeColumns;
        private System.Windows.Forms.Label labelSeparator;
    }
}