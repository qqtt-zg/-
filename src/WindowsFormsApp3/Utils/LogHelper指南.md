# LogHelper使用指南

## 概述

本指南旨在帮助您将项目中所有用于调试输出的代码归类到统一的日志系统中，通过使用新创建的`LogHelper`工具类替换所有直接的`System.Diagnostics.Debug.WriteLine`调用。

## 为什么需要统一日志管理

1. **集中控制**：统一管理所有日志输出，方便启用/禁用调试信息
2. **分级管理**：支持不同日志级别（Debug、Info、Warn、Error、Critical）
3. **持久化存储**：自动将日志保存到文件中，便于后期分析
4. **格式统一**：统一日志格式，包含时间戳、日志级别等信息
5. **易于维护**：后续如需修改日志行为，只需修改一处

## LogHelper类介绍

`LogHelper`是一个静态工具类，提供了简单易用的方法来记录不同级别的日志：

```csharp
// 记录调试信息
LogHelper.Debug("调试信息");

// 记录一般信息
LogHelper.Info("一般信息");

// 记录警告信息
LogHelper.Warn("警告信息");

// 记录错误信息
LogHelper.Error("错误信息");

// 记录带异常的错误信息
LogHelper.Error("错误信息", exception);

// 记录严重错误信息
LogHelper.Critical("严重错误信息");

// 记录带异常的严重错误信息
LogHelper.Critical("严重错误信息", exception);
```

## 替换步骤

### 1. 添加引用

在需要使用日志的文件中添加以下命名空间引用：

```csharp
using WindowsFormsApp3.Utils;
```

### 2. 替换调试输出代码

将所有直接使用`System.Diagnostics.Debug.WriteLine`的代码替换为`LogHelper`对应的方法。

#### 示例1：简单调试信息

原代码：
```csharp
Debug.WriteLine("开始处理文件: " + fileName);
```

替换为：
```csharp
LogHelper.Debug("开始处理文件: " + fileName);
```

#### 示例2：错误信息

原代码：
```csharp
Debug.WriteLine("处理文件失败: " + ex.Message);
```

替换为：
```csharp
LogHelper.Error("处理文件失败: " + ex.Message, ex);
```

### 3. 移除不需要的引用

替换完成后，可以移除文件中对`System.Diagnostics`命名空间的引用（如果不再使用其他相关类）。

## 批量替换建议

对于项目中大量的调试输出代码，建议使用以下批量替换策略：

1. **按文件类型处理**：优先处理日志密集的文件，如`PdfTools.cs`、`Form1Presenter.cs`、`FileRenameService.cs`等
2. **按日志级别分类**：根据消息的重要性选择合适的日志级别
   - 临时调试信息 -> `Debug`
   - 正常流程信息 -> `Info`
   - 可能的问题但不影响功能 -> `Warn`
   - 操作失败但程序可以继续 -> `Error`
   - 致命错误导致程序无法继续 -> `Critical`

## 日志配置

日志文件默认保存在应用数据目录下，按日期生成，格式为`Log_yyyyMMdd.txt`。

如需修改日志行为，可以调整`FileLogger`类中的相关配置。

## 注意事项

1. **避免日志泛滥**：不要记录过多的低价值信息，以免影响性能和日志可读性
2. **敏感信息保护**：不要在日志中记录密码、密钥等敏感信息
3. **异常处理**：记录异常时，请使用带异常参数的重载方法，以便记录完整的异常信息
4. **性能考虑**：在性能敏感的代码路径中，谨慎使用日志记录

## 常见问题解答

**Q: 为什么有时候看不到调试输出？**
**A:** 请确保在调试模式下运行，并且没有禁用Debug级别日志。

**Q: 日志文件保存在哪里？**
**A:** 默认保存在`Application.UserAppDataPath`目录下，通常是`C:\Users\用户名\AppData\Local\大诚工具箱`。

**Q: 如何禁用特定级别的日志？**
**A:** 可以通过修改`FileLogger`类中的日志级别过滤逻辑来实现。

## 总结

通过统一使用`LogHelper`类来管理项目中的所有日志输出，您可以获得更规范、更灵活、更易于维护的日志系统，同时保留原有的调试功能。建议您尽快完成所有调试输出代码的替换工作，以提升项目质量。