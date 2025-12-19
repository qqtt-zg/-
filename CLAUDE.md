# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 项目概述

这是一个基于 .NET Framework 4.8 的 Windows Forms 桌面应用程序，名为"大诚重命名工具"。该工具专为生产环境设计，提供文件批量重命名、Excel数据导入、PDF处理等高级功能。

## 构建和运行命令

### 基本构建

```bash
# 还原 NuGet 包
dotnet restore WindowsFormsApp3.sln

# 构建 Debug 版本
dotnet build WindowsFormsApp3.sln --configuration Debug

# 构建 Release 版本
dotnet build WindowsFormsApp3.sln --configuration Release

# 清理解决方案
dotnet clean WindowsFormsApp3.sln
```

### 运行应用

```bash
# 运行主程序
dotnet run --project src/WindowsFormsApp3/WindowsFormsApp3.csproj

# 或直接运行编译后的 exe
src/WindowsFormsApp3/bin/Debug/net48/大诚重命名工具.exe
```

### 测试命令

```bash
# 运行所有测试
dotnet test src/WindowsFormsApp3.Tests/WindowsFormsApp3.Tests.csproj

# 运行测试并生成覆盖率报告
dotnet test src/WindowsFormsApp3.Tests/WindowsFormsApp3.Tests.csproj --collect:"XPlat Code Coverage"
```

### 发布命令

```bash
# 发布为依赖框架的可执行文件
dotnet publish src/WindowsFormsApp3/WindowsFormsApp3.csproj --configuration Release --output ./publish

# 发布为自包含应用（包含 .NET 运行时）
dotnet publish src/WindowsFormsApp3/WindowsFormsApp3.csproj --configuration Release --self-contained true --runtime win-x64 --output ./publish-sc
```

## 核心架构

### 分层架构设计

项目采用清晰的分层架构：

- **表示层 (Presentation Layer)**: Forms、Controls、Presenters - 负责 UI 展示和用户交互
- **应用层 (Application Layer)**: Services、Commands - 包含业务逻辑和服务协调
- **领域层 (Domain Layer)**: Models、Interfaces - 核心数据模型和业务规则
- **基础设施层 (Infrastructure Layer)**: Utils、Helpers - 通用工具和外部依赖

### 关键设计模式

1. **服务定位器模式 (Service Locator)**
   - 使用 `ServiceLocator` 管理所有服务的生命周期
   - 基于 Microsoft.Extensions.DependencyInjection 容器
   - 支持单例和瞬态服务注册

2. **MVP 模式 (Model-View-Presenter)**
   - PDF 处理模块采用 MVP 模式
   - View 通过接口定义实现解耦
   - Presenter 协调 View 和 Model 之间的交互

3. **命令模式 (Command Pattern)**
   - 实现完整的撤销/重做功能
   - 支持批量操作和复合命令
   - 命令历史可持久化

4. **事件驱动架构**
   - 基于 `IEventBus` 的发布-订阅模式
   - 支持同步和异步事件处理
   - 用于组件间的解耦通信

### 核心服务组件

- **FileRenameService**: 文件重命名核心服务
- **ExcelImportService**: Excel 数据导入服务
- **PdfProcessingService**: PDF 处理服务（支持 CefSharp 和 Pdfium 双引擎）
- **BatchProcessingService**: 批量处理服务
- **UndoRedoService**: 撤销重做服务
- **EventBus**: 事件总线

### 重要技术细节

#### PDF 双引擎设计
- **CefSharp 引擎**: 基于 Chrome 渲染，功能强大但占用内存较高
- **Pdfium 引擎**: 轻量级渲染，性能优先
- 通过 `IPdfPreviewControl` 接口实现统一抽象

#### 命令系统
- 所有可撤销操作都实现 `ICommand` 接口
- 支持批量操作的事务性
- 命令历史记录可导出和恢复

#### 事件系统
- 使用强类型事件
- 支持事件过滤和优先级
- 统一的异常处理机制

## 项目注意事项

### 平台要求
- 必须在 Windows 平台上运行
- 目标框架为 .NET Framework 4.8
- 构建平台必须为 x64（CefSharp 要求）

### 特殊依赖
- **CefSharp**: 需要 win-x64 运行时标识符
- **Spire.Pdf**: 使用本地 DLL 引用（位于 Spire.Office Platinum v9.9.0 目录）
- **字体资源**: 项目嵌入了多个中文字体文件

### 开发建议
1. 修改服务时优先实现接口
2. 新增可撤销操作时继承 `CommandBase`
3. 使用事件总线进行组件间通信
4. 遵循现有的命名约定和代码风格
5. 所有异步方法都应以 Async 后缀结尾

### 测试策略
- 单元测试使用 xUnit 和 MSTest 框架
- UI 测试使用 FlaUI.UIA3
- 模拟框架使用 Moq
- 测试覆盖核心业务逻辑和服务层

## 开发环境配置

推荐使用 Visual Studio 2019/2022 进行开发：
1. 打开 `WindowsFormsApp3.sln` 解决方案
2. 确保安装了 .NET Framework 4.8 开发工具
3. 还原 NuGet 包后即可编译运行