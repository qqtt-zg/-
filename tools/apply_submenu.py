# -*- coding: utf-8 -*
-import re, os

designer_path = 'src/WindowsFormsApp3/Forms/Main/MaterialSelectFormModern.Designer.cs'
with open(designer_path, 'r', encoding='utf-8') as f:
    designer = f.read()
