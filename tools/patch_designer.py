# -*- coding: utf-8 -*-
import re

with open('src/WindowsFormsApp3/Forms/Main/MaterialSelectFormModern.Designer.cs', 'r', encoding='utf-8') as f:
    text = f.read()

text = text.replace(
    'this.autoIncrementCheckbox = new AntdUI.Checkbox();',
    'this.cmbOrderNumberMode = new AntdUI.Select();\n            this.cmbOrderRegexPattern = new AntdUI.Select();\n            this.btnApplyToAll = new AntdUI.Button();'
)

# Replace autoIncrementCheckbox setup
text = re.sub(
    r'          // \r?\n           // autoIncrementCheckbox\r?\n            // \r?\n            this\.autoIncrementCheckbox\..*?this\.autoIncrementCheckbox\.CheckedChanged.*?;',
    '''            // 
            // cmbOrderNumberMode
            // 
            this.cmbOrderNumberMode.BorderWidth = 2F;
            this.cmbOrderNumberMode.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbOrderNumberMode.Location = new System.Drawing.Point(290, 329);
            this.cmbOrderNumberMode.Name = "cmbOrderNumberMode";
            this.cmbOrderNumberMode.Size = new System.Drawing.Size(80, 32);
            this.cmbOrderNumberMode.TabIndex = 4;
            this.cmbOrderNumberMode.WaveSize = 0;
            // 
            // cmbOrderRegexPattern
            // 
            this.cmbOrderRegexPattern.BorderWidth = 2F;
            this.cmbOrderRegexPattern.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbOrderRegexPattern.Location = new System.Drawing.Point(255, 329);
            this.cmbOrderRegexPattern.Name = "cmbOrderRegexPattern";
            this.cmbOrderRegexPattern.Size = new System.Drawing.Size(56, 32);
            this.cmbOrderRegexPattern.TabIndex = 5;
            this.cmbOrderRegexPattern.Visible = false;
            this.cmbOrderRegexPattern.WaveSize = 0;''',
    text,
    flags=re.DOTALL
)

# Replace confirmButton / cancelButton
text = re.sub(
    r'          // \r?\n           // confirmButton\r?\n.*?this\.cancelButton\.Click \+= new System\.EventHandler\(this\.cancelButton_Click_1\);',
    '''            // 
            // btnApplyToAll
            // 
            this.btnApplyToAll.BorderWidth = 2F;
            this.btnApplyToAll.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnApplyToAll.Location = new System.Drawing.Point(150, 598);
            this.btnApplyToAll.Name = "btnApplyToAll";
            this.btnApplyToAll.Size = new System.Drawing.Size(85, 28);
            this.btnApplyToAll.TabIndex = 19;
            this.btnApplyToAll.Text = "宔用e全";
            this.btnApplyToAll.WaveSize = 0;
            this.btnApplyToAll.Click += new System.EventHandler(this.BtnApplyToAll_Click);
            // 
            // confirmButton
            // 
            this.confirmButton.BorderWidth = 2F;
            this.confirmButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.confirmButton.Location = new System.Drawing.Point(240, 598);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(64, 28);
            this.confirmButton.TabIndex = 20;
            this.confirmButton.Text = "型认";
            this.confirmButton.WaveSize = 0;
            this.confirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BorderWidth = 2F;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cancelButton.Ghost = true;
            this.cancelButton.Location = new System.Drawing.Point(309, 598);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(64, 28);
            this.cancelButton.TabIndex = 21;
            this.cancelButton.Text = "取消";
            this.cancelButton.WaveSize = 0;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click_1);''',
    text,
    flags=re.DOTALL
)

# 4. Replace Controls.Add
text = text.replace(
    'this.Controls.Add(this.autoIncrementCheckbox);\n',
    'this.Controls.Add(this.cmbOrderNumberMode);\n            this.Controls.Add(this.cmbOrderRegexPattern);\n            this.Controls.Add(this.btnApplyToAll);\n'
)

	# 5. Replace declarations
text = text.replace(
    'private AntdUI.Checkbox autoIncrementCheckbox;',
    'private AntdUI.Select cmbOrderNumberMode;\n        private AntdUI.Select cmbOrderRegexPattern;\n        private AntdUI.Button btnApplyToAll;'
)

with open('src/WindowsFormsApp3/Forms/Main/MaterialSelectFormModern.Designer.cs', 'w', encoding='-tf-8') as f:
    f.write(text)

print('Designer successfully patched!')
