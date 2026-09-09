# 右键菜单与短确认弹框迁移测试规格（待 Critic 审查）

## 自动化

- 请求到 AntdUI 菜单项的映射：顺序、分隔、子菜单、图标、危险语义、禁用/勾选态、稳定 ID 与回调。
- Modal 映射：标准 Yes/No、Button2 默认 Yes/No、OK/Cancel、Abort/Retry/Ignore；分别验证确认、取消、Enter、Esc、关闭按钮。
- 主题映射：深色、浅色、护眼绿、经典蓝的背景、文字、边框、主色、危险色、悬停和禁用态。
- 宿主回归：DataGridView、树、PDF 标签页的右键目标；Ctrl+C/X/V 与 Shift+F10/Application 键；动态菜单状态。

## 手工矩阵

- 四套主题 × 所有纳入范围菜单表面。
- 四套主题 × 常规确认、危险确认、三按钮恢复确认。
- 100%、125%、150%、200% DPI；长中文文本、嵌套菜单、屏幕边缘、多显示器、置顶悬浮窗。
- FileRename 隐藏列：连续多选、保存配置、恢复原始、位置稳定、无闪烁或重复执行。

## 最终命令

```powershell
dotnet build WindowsFormsApp3.sln -c Debug
dotnet test src/WindowsFormsApp3.Tests/WindowsFormsApp3.Tests.csproj
```

## 静态核查

```powershell
rg -n --glob '*.cs' "new\s+(System\.Windows\.Forms\.)?ContextMenuStrip" src/WindowsFormsApp3
rg -n --glob '*.cs' "MessageBoxButtons\.(YesNo|AbortRetryIgnore)" src/WindowsFormsApp3
```

允许的原生例外必须仅为系统托盘、文件选择器、复杂窗体及规格明确排除的长更新流程。
