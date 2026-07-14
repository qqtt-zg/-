# Release Rules
<!-- last-analyzed: 2026-07-01T02:12:17.7307313Z -->

## Version Sources
- `src/WindowsFormsApp3/Properties/AssemblyInfo.cs` - `AssemblyVersion`, `AssemblyFileVersion`
- `installers/Setup.iss` - `AppVersion`, `AppVerName`, `OutputBaseFilename`, `AppComments`

## Release Trigger
- 本地手动运行 Inno Setup 编译器生成安装包
- 无 GitHub Actions / CI 自动发布流程

## Test Gate
- `dotnet test src/WindowsFormsApp3.Tests/WindowsFormsApp3.Tests.csproj`
- 发布前至少确认相关服务测试通过

## Registry / Distribution
- Windows 安装包：`installers/Setup.iss` 编译后输出到 `installers/安装包/`

## Release Notes Strategy
- 以 `Setup.iss` 的 `AppComments` 作为安装包内版本说明
- 目前没有统一的 `CHANGELOG.md`

## CI Workflow Files
- 无

## First-Time Setup Gaps
- 缺少自动化发布工作流
- 缺少统一 changelog
