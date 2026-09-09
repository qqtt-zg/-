# 右键菜单与短确认弹框 AntdUI 迁移计划（待 Critic 审查）

## 状态

- Planner：已修订
- Architect：APPROVE
- Critic：BLOCKED（当前账户不支持该角色所配置的模型，未生成 Critic 结论）
- 执行交接：未授权；不得进入代码实施

## 方案

采用薄渲染适配层，而不是把业务命令集中到新服务中。

- `UI/ContextMenuRequest.cs` 与 `UI/AntdUiContextMenuRenderer.cs`：将调用方即时生成的菜单请求映射为 AntdUI 弹出菜单。
- `UI/ModalRequest.cs` 与 `UI/AntdUiModalRenderer.cs`：显式声明按钮、默认按钮、关闭策略和精确 `DialogResult` 映射。
- `Utils/AntdUiThemeBridge.cs`：在主题的非递归入口幂等地将 `ThemeDefinition` 同步到 AntdUI 全局语义色；渲染器不得在每次打开时改全局主题。
- 宿主保留右键定位、行/列/节点/标签选择、动态菜单状态、快捷键和命令执行。

## 实施顺序

1. 先为请求模型、渲染器、主题桥接建立单元测试；锁定菜单项顺序、层级、勾选/禁用态、危险色、命令 ID 和弹框返回值。
2. 完成 Modal 闸门：验证 `YesNo`、Button2 默认、`OKCancel`、`AbortRetryIgnore` 的 Enter、Esc、关闭按钮及精确返回值；所有调用均传入 UI 线程上的 owner。
3. 完成 FileRename 列头“隐藏列”闸门：验证多选项点击后菜单可继续使用。若重开菜单不可用，改为专用 AntdUI 风格清单弹层；两者均不能保证等价时停止该表面迁移。
4. 在 `ThemeHelper` 主题应用入口接入主题桥接，并验证四套内置主题。
5. 迁移简单菜单：`MainShellForm.ShowMoreMenu`、`TabbedPdfPreviewControl`、`PdfiumPdfPreviewControl`。保留 `MainShellForm` 的托盘菜单原生实现。
6. 迁移数据菜单：`DataGridViewContextMenu`、`DgvContextMenu`、`FileRenamePanel`、`DatabasePanel`；保留宿主 Ctrl+C/X/V 与命中目标逻辑。
7. 迁移动态菜单：`MaterialSelectFormModern`、`MaterialSelectFormModern.BatchGroupWorkbench`、`EventGroupsTreeView`、`FloatingDropZoneForm`。
8. 迁移短确认弹框：共享确认辅助方法及已识别的删除、覆盖、继续等调用点；不迁移单按钮提示、输入窗、文件选择器、进度窗和复杂业务窗体。
9. 完成四主题、DPI、键盘与全部回归验证。

## 不可改变的业务边界

- 菜单项、顺序、启用/勾选条件、快捷键、命令目标与业务结果保持不变。
- 托盘菜单、文件选择器、复杂窗体、进度窗和 `UpdateManager` 更新流程不在范围内。
- 不增加第三方依赖。

## 关键风险

- AntdUI 菜单不是可赋值给 `Control.ContextMenuStrip` 的 WinForms 组件，所有宿主必须显式打开并维持自身上下文。
- AntdUI 菜单快捷键文本不等于键盘命令，需要由宿主继续处理。
- AntdUI Modal 的原始返回值与当前 Yes/No、Button2 默认、Abort/Retry/Ignore 的语义不直接等价，必须规范化。
- FileRename 列头菜单当前依赖阻止菜单关闭的多选交互，需独立验证。
