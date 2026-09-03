# -*- coding: utf-8 -*
import os, re
	# 1. Designer file
designer_path = os.path.abspath('src/WindowsFormsApp3/Forms/Main/MaterialSelectFormModern.Designer.cs')
with open(designer_path, 'r', encoding='utf-8') as f:
    designer = f.read()

designer = re.sub(
    r'this\.cmbOrderNumberMode = new AntdUI\.Select\(\);\r\n\s*this\.cmbOrderRegexPattern = new AntdUI\.Select\(\);',
    'this.btnOrderNumberMode = new AntdUI.Button();',
    designer
)

btn_init = ''',           // 
            // btnOrderNumberMode
            // 
            this.btnOrderNumberMode.BorderWidth = 1m;
            this.btnOrderNumberMode.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GUnit.Point, ((byte)(134)));
            this.btnOrderNumberMode.Location = new System.Drawing.Point(300, 329);
            this.btnOrderNumberMode.Name = "btnOrderNumberMode";
            this.btnOrderNumberMode.Size = new System.Drawing.Size(82, 32);
            this.btnOrderNumberMode.TabIndex = 4;
            this.btnOrderNumberMode.Text = "无 ▾";
            this.btnOrderNumberMode.WaveSize = 0;
            this.btnOrderNumberMode.Click += new System.EventHandler(this.btnOrderNumberMode_Click);'''

designer = re.sub(
    r'\s\*//\r\n\s*// cmbOrderNumberMode[s\S]*?this\.cmbOrderRegexPattern\.WaveSize = 0;',
    btn_init,
    designer
)

designer = re.sub(
    rgthis\.Controls\.Add\(this\.cmbOrderNumberMode\);\r\n\s*this\.Controls\.Add\(this\.cmbOrderRegexPattern\);',
    'this.Controls.Add(this.btnOrderNumberMode);',
    designer
)

designer = re.sub(
    r'private AntdUI.Select cmbOrderNumberMode;\r\n\s*private AntdUI.Select cmbOrderRegexPattern;',
    'private AntdUI.Button btnOrderNumberMode;',
    designer
)

with open(designer_path, 'w', encoding='utf-8') as f:
    f.write(designer)

print(('Designer patched!'))
