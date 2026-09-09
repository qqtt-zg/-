# 应用内右键菜单与短确认弹框的 AntdUI 迁移规格

## 元数据

- 来源：deep-interview
- 项目类型：既有 .NET Framework 4.8 WinForms 应用
- AntdUI 版本：2.4.8
- 需求状态：可进入架构/实施规划
- 访谈记录：`.omx/interviews/context-menu-modal-migration-20260907T172853Z.md`
- 上下文快照：`.omx/context/context-menu-modal-migration-20260907T172853Z.md`

## 意图与目标

统一应用内高频临时交互的视觉语言，使右键菜单与简短确认弹框在全部内置主题中拥有一致的图标、圆角、阴影、语义色和禁用态，同时保持既有业务行为不变。

## 范围

### 纳入

1. 所有已识别的应用内业务右键菜单，包括文件重命名表格和列头、材料选择的预设/批处理/订单号菜单、PDF 标签页和预览、事件分组树、数据库及悬浮拖放窗口菜单。
2. 所有仅负责确认、警告或简单选择的短 `Form.ShowDialog()` 对话框，重点位于 `UI/DgvContextMenu.cs`、`Forms/Panels/FileRenamePanel.cs`、`Forms/Main/MaterialSelectFormModern.cs` 与 PDF 操作面板。
3. 已有 `AntdUI.Modal.open` 调用的统一风格和复用模式。
4. 四主题验证：深色、浅色、护眼绿、经典蓝。

### 排除

1. `NotifyIcon` 系统托盘菜单。
2. 系统文件/文件夹选择器。
3. 含复杂表单、预览、多步骤配置或独立业务生命周期的窗口，例如材料选择、PDF 处理和进度窗体。
4. 业务命令、数据模型、服务逻辑、权限/启用规则、菜单排序和快捷键的重设计。
5. 新增第三方依赖。

## 决策边界

- 实施方可决定迁移顺序、提取的复用工具和具体 AntdUI 2.4.8 菜单/弹框 API。
- 若 AntdUI 的菜单 API 不可直接绑定到 WinForms `ContextMenuStrip` 属性，实施方可采用适配层或事件展示方式；不得改变菜单命令语义。
- 对删除、覆盖、剪切等危险操作，可增强视觉语义，但不得改变既有默认行为、快捷键、命令触发条件或确认结果。

## 技术事实与影响面

- 项目目前没有直接构造 `AntdUI.ContextMenuStrip`；业务菜单均使用 `System.Windows.Forms.ContextMenuStrip`。
- `AntdUI.Modal.open` 当前只在 `Forms/Panels/PdfOperationsPanel.cs` 用于“另存后是否打开文件”的确认。
- 主要菜单定义分布在：
  - `UI/DgvContextMenu.cs`
  - `UI/DataGridViewContextMenu.cs`
  - `Forms/Panels/FileRenamePanel.cs`
  - `Forms/Main/MaterialSelectFormModern.cs`
  - `Controls/EventGroupsTreeView.cs`
  - `Controls/PdfiumPdfPreviewControl.cs`
  - `Controls/TabbedPdfPreviewControl.cs`
  - `Forms/Panels/DatabasePanel.cs`
  - `Forms/Utils/FloatingDropZoneForm.cs`
  - `Forms/Main/MainShellForm.cs`（仅保留其托盘菜单原生实现）
- 主题规则由 `Utils/ThemeHelper.cs` 和主题系统文档定义；迁移后的新表面必须接入现有主题颜色语义。

## 可测试验收标准

1. 每个纳入范围的业务入口仍能显示对应菜单/确认弹框，并执行原有全部命令。
2. 既有快捷键、菜单项顺序、可用/禁用判断、取消和确认后的业务结果不变。
3. 四套内置主题下，菜单与短确认弹框均具备一致的圆角、阴影、图标、主操作色、危险操作色和禁用态，且没有文字截断、定位错误或残留的原生浅色外观。
4. 对 DataGridView、树和 PDF 预览等控件，右键菜单的定位、当前选中项识别和上下文命令目标不变。
5. 系统托盘菜单、文件选择器和复杂业务窗体保持原样。
6. `dotnet build WindowsFormsApp3.sln -c Debug` 与 `dotnet test src/WindowsFormsApp3.Tests/WindowsFormsApp3.Tests.csproj` 通过；为抽取的菜单/弹框映射逻辑补充针对性回归测试。

## 推荐后续路径

先使用 `$ralplan` 校验 AntdUI 2.4.8 的实际菜单 API 与适配方案，再用 `$ultragoal` 执行分批迁移和四主题视觉验证。
