using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using WindowsFormsApp3.Interfaces;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 数据完整性验证和错误处理服务
    /// 用于验证数据文件的完整性和处理各种错误情况
    /// </summary>
    public class DataIntegrityService
    {
        private readonly ILogger _logger;
        private readonly Dictionary<string, ValidationResult> _validationResults;

        public DataIntegrityService(ILogger logger)
        {
            _logger = logger;
            _validationResults = new Dictionary<string, ValidationResult>();
        }

        /// <summary>
        /// 执行完整的数据完整性检查
        /// </summary>
        /// <returns>完整性检查报告</returns>
        public IntegrityReport PerformIntegrityCheck()
        {
            var report = new IntegrityReport();

            try
            {
                _logger.LogInformation("开始执行数据完整性检查");

                // 检查关键数据文件
                ValidateConfigsJson(report);
                ValidateApplicationSettings(report);
                ValidateMaterialSelectSettings(report);
                ValidateLogConfig(report);
                ValidateSavedGrids(report);
                ValidateCommandHistory(report);

                // 检查目录结构
                ValidateDirectoryStructure(report);

                // 检查数据格式完整性
                ValidateDataFormat(report);

                // 生成报告
                GenerateIntegrityReport(report);

                _logger.LogInformation($"数据完整性检查完成: 正常 {report.ValidCount}, 警告 {report.WarningCount}, 错误 {report.ErrorCount}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "数据完整性检查过程中发生严重错误");
                report.HasCriticalError = true;
                report.ErrorMessage = ex.Message;
            }

            return report;
        }

        /// <summary>
        /// 验证configs.json文件
        /// </summary>
        private void ValidateConfigsJson(IntegrityReport report)
        {
            try
            {
                var configPath = AppDataPathManager.ConfigFilePath;

                if (!File.Exists(configPath))
                {
                    report.AddError("configs.json", "文件不存在");
                    return;
                }

                var fileInfo = new FileInfo(configPath);
                if (fileInfo.Length == 0)
                {
                    report.AddError("configs.json", "文件为空");
                    return;
                }

                // 尝试解析JSON
                var json = File.ReadAllText(configPath);
                var configs = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                if (configs == null)
                {
                    report.AddError("configs.json", "JSON格式无效");
                    return;
                }

                report.AddValid("configs.json", $"包含 {configs.Count} 个配置项");
            }
            catch (JsonException ex)
            {
                report.AddError("configs.json", $"JSON解析失败: {ex.Message}");
            }
            catch (Exception ex)
            {
                report.AddError("configs.json", $"验证失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证application_settings.json文件
        /// </summary>
        private void ValidateApplicationSettings(IntegrityReport report)
        {
            try
            {
                var settingsPath = Path.Combine(AppDataPathManager.AppRootDirectory, "application_settings.json");

                if (!File.Exists(settingsPath))
                {
                    report.AddWarning("application_settings.json", "文件不存在，将使用默认设置");
                    return;
                }

                var fileInfo = new FileInfo(settingsPath);
                if (fileInfo.Length == 0)
                {
                    report.AddError("application_settings.json", "文件为空");
                    return;
                }

                // 尝试解析JSON
                var json = File.ReadAllText(settingsPath);
                var settings = JsonConvert.DeserializeObject<ApplicationSettingsService.ApplicationSettings>(json);

                if (settings == null)
                {
                    report.AddError("application_settings.json", "JSON格式无效");
                    return;
                }

                report.AddValid("application_settings.json", "应用程序设置文件格式正确");
            }
            catch (JsonException ex)
            {
                report.AddError("application_settings.json", $"JSON解析失败: {ex.Message}");
            }
            catch (Exception ex)
            {
                report.AddError("application_settings.json", $"验证失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证MaterialSelectSettings.json文件
        /// </summary>
        private void ValidateMaterialSelectSettings(IntegrityReport report)
        {
            try
            {
                var settingsPath = AppDataPathManager.MaterialSelectSettingsPath;

                if (!File.Exists(settingsPath))
                {
                    report.AddWarning("MaterialSelectSettings.json", "文件不存在，将使用默认设置");
                    return;
                }

                var fileInfo = new FileInfo(settingsPath);
                if (fileInfo.Length == 0)
                {
                    report.AddWarning("MaterialSelectSettings.json", "文件为空，将使用默认设置");
                    return;
                }

                // 尝试解析JSON
                var json = File.ReadAllText(settingsPath);
                var settings = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                if (settings == null)
                {
                    report.AddError("MaterialSelectSettings.json", "JSON格式无效");
                    return;
                }

                report.AddValid("MaterialSelectSettings.json", $"材料选择设置文件格式正确");
            }
            catch (JsonException ex)
            {
                report.AddError("MaterialSelectSettings.json", $"JSON解析失败: {ex.Message}");
            }
            catch (Exception ex)
            {
                report.AddError("MaterialSelectSettings.json", $"验证失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证LogConfig.json文件
        /// </summary>
        private void ValidateLogConfig(IntegrityReport report)
        {
            try
            {
                var configPath = AppDataPathManager.LogConfigPath;

                if (!File.Exists(configPath))
                {
                    report.AddWarning("LogConfig.json", "文件不存在，将使用默认日志配置");
                    return;
                }

                var fileInfo = new FileInfo(configPath);
                if (fileInfo.Length == 0)
                {
                    report.AddError("LogConfig.json", "文件为空");
                    return;
                }

                // 尝试解析JSON
                var json = File.ReadAllText(configPath);
                var config = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                if (config == null)
                {
                    report.AddError("LogConfig.json", "JSON格式无效");
                    return;
                }

                // 验证必要的配置项
                if (!config.ContainsKey("LogLevel"))
                {
                    report.AddWarning("LogConfig.json", "缺少LogLevel配置项");
                }

                if (!config.ContainsKey("LogDirectory"))
                {
                    report.AddWarning("LogConfig.json", "缺少LogDirectory配置项");
                }

                report.AddValid("LogConfig.json", "日志配置文件格式正确");
            }
            catch (JsonException ex)
            {
                report.AddError("LogConfig.json", $"JSON解析失败: {ex.Message}");
            }
            catch (Exception ex)
            {
                report.AddError("LogConfig.json", $"验证失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证SavedGrids目录
        /// </summary>
        private void ValidateSavedGrids(IntegrityReport report)
        {
            try
            {
                var gridsDir = AppDataPathManager.SavedGridsDirectory;

                if (!Directory.Exists(gridsDir))
                {
                    report.AddWarning("SavedGrids", "目录不存在");
                    return;
                }

                var jsonFiles = Directory.GetFiles(gridsDir, "*.json", SearchOption.TopDirectoryOnly);
                var corruptedFiles = 0;
                var validFiles = 0;

                foreach (var file in jsonFiles)
                {
                    try
                    {
                        var json = File.ReadAllText(file);
                        JsonConvert.DeserializeObject(json);
                        validFiles++;
                    }
                    catch (JsonException)
                    {
                        corruptedFiles++;
                        report.AddError("SavedGrids", $"文件损坏: {Path.GetFileName(file)}");
                    }
                }

                if (corruptedFiles == 0 && validFiles > 0)
                {
                    report.AddValid("SavedGrids", $"包含 {validFiles} 个有效的网格数据文件");
                }
                else if (corruptedFiles > 0)
                {
                    report.AddError("SavedGrids", $"发现 {corruptedFiles} 个损坏的文件，{validFiles} 个有效文件");
                }
                else
                {
                    report.AddWarning("SavedGrids", "目录为空");
                }
            }
            catch (Exception ex)
            {
                report.AddError("SavedGrids", $"验证失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证命令历史文件
        /// </summary>
        private void ValidateCommandHistory(IntegrityReport report)
        {
            try
            {
                var historyPath = AppDataPathManager.CommandHistoryPath;

                if (!File.Exists(historyPath))
                {
                    report.AddWarning("command_history.json", "文件不存在");
                    return;
                }

                var fileInfo = new FileInfo(historyPath);
                if (fileInfo.Length == 0)
                {
                    report.AddWarning("command_history.json", "文件为空");
                    return;
                }

                // 尝试解析JSON
                var json = File.ReadAllText(historyPath);
                var history = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                if (history == null)
                {
                    report.AddError("command_history.json", "JSON格式无效");
                    return;
                }

                report.AddValid("command_history.json", "命令历史文件格式正确");
            }
            catch (JsonException ex)
            {
                report.AddError("command_history.json", $"JSON解析失败: {ex.Message}");
            }
            catch (Exception ex)
            {
                report.AddError("command_history.json", $"验证失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证目录结构
        /// </summary>
        private void ValidateDirectoryStructure(IntegrityReport report)
        {
            try
            {
                var directories = new[]
                {
                    AppDataPathManager.AppRootDirectory,
                    AppDataPathManager.SavedGridsDirectory,
                    AppDataPathManager.ErrorReportsDirectory,
                    AppDataPathManager.HistoryDirectory,
                    AppDataPathManager.LogsDirectory,
                    AppDataPathManager.ExcelExportDirectory
                };

                foreach (var dir in directories)
                {
                    if (!Directory.Exists(dir))
                    {
                        report.AddError("目录结构", $"目录不存在: {dir}");
                    }
                    else
                    {
                        report.AddValid("目录结构", $"目录存在: {dir}");
                    }
                }
            }
            catch (Exception ex)
            {
                report.AddError("目录结构", $"验证失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证数据格式
        /// </summary>
        private void ValidateDataFormat(IntegrityReport report)
        {
            try
            {
                // 检查配置文件格式一致性
                var configPath = AppDataPathManager.ConfigFilePath;
                if (File.Exists(configPath))
                {
                    var json = File.ReadAllText(configPath);
                    var configs = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                    if (configs != null)
                    {
                        foreach (var config in configs)
                        {
                            try
                            {
                                var configJson = JsonConvert.SerializeObject(config.Value);
                                JsonConvert.DeserializeObject(configJson);
                                report.AddValid($"配置格式", $"配置项 '{config.Key}' 格式正确");
                            }
                            catch (JsonException)
                            {
                                report.AddError($"配置格式", $"配置项 '{config.Key}' 格式无效");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                report.AddError("数据格式", $"验证失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 生成完整性检查报告
        /// </summary>
        private void GenerateIntegrityReport(IntegrityReport report)
        {
            try
            {
                var reportPath = Path.Combine(AppDataPathManager.AppRootDirectory, "integrity_report.json");
                var reportJson = JsonConvert.SerializeObject(report, Formatting.Indented);
                File.WriteAllText(reportPath, reportJson);

                _logger.LogInformation($"数据完整性报告已保存到: {reportPath}");
                report.ReportPath = reportPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成数据完整性报告失败");
            }
        }

        /// <summary>
        /// 修复损坏的数据文件
        /// </summary>
        /// <param name="itemName">要修复的项目名称</param>
        /// <returns>修复是否成功</returns>
        public bool RepairDataFile(string itemName)
        {
            try
            {
                switch (itemName)
                {
                    case "configs.json":
                        return RepairConfigsJson();
                    case "application_settings.json":
                        return RepairApplicationSettings();
                    case "MaterialSelectSettings.json":
                        return RepairMaterialSelectSettings();
                    case "LogConfig.json":
                        return RepairLogConfig();
                    default:
                        _logger.LogWarning($"未知的修复项目: {itemName}");
                        return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"修复数据文件失败: {itemName}");
                return false;
            }
        }

        /// <summary>
        /// 修复configs.json文件
        /// </summary>
        private bool RepairConfigsJson()
        {
            try
            {
                var configPath = AppDataPathManager.ConfigFilePath;

                // 创建默认配置
                var defaultConfigs = new Dictionary<string, object>
                {
                    ["默认配置"] = new
                    {
                        Separator = "_",
                        Unit = "",
                        TetBleed = "3,5,10",
                        Material = "",
                        Opacity = 100,
                        HotkeyToggle = "",
                        FixedFieldPresets = "",
                        RegexPatterns = new Dictionary<string, string>(),
                        EventItems = new List<string>(),
                        AddPdfLayers = true
                    }
                };

                var json = JsonConvert.SerializeObject(defaultConfigs, Formatting.Indented);
                File.WriteAllText(configPath, json);

                _logger.LogInformation("configs.json文件已修复");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "修复configs.json文件失败");
                return false;
            }
        }

        /// <summary>
        /// 修复application_settings.json文件
        /// </summary>
        private bool RepairApplicationSettings()
        {
            try
            {
                var settingsPath = Path.Combine(AppDataPathManager.AppRootDirectory, "application_settings.json");

                // 使用ApplicationSettingsService创建默认设置
                var settingsService = new ApplicationSettingsService(_logger);
                var settings = settingsService.LoadSettingsInternal();
                settingsService.SaveSettingsInternal(settings);

                _logger.LogInformation("application_settings.json文件已修复");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "修复application_settings.json文件失败");
                return false;
            }
        }

        /// <summary>
        /// 修复MaterialSelectSettings.json文件
        /// </summary>
        private bool RepairMaterialSelectSettings()
        {
            try
            {
                var settingsPath = AppDataPathManager.MaterialSelectSettingsPath;

                // 创建默认材料选择设置
                var defaultSettings = new Dictionary<string, object>
                {
                    ["lastSelectedMaterial"] = "",
                    ["lastColorMode"] = "彩色",
                    ["lastFilmType"] = "光膜"
                };

                var json = JsonConvert.SerializeObject(defaultSettings, Formatting.Indented);
                File.WriteAllText(settingsPath, json);

                _logger.LogInformation("MaterialSelectSettings.json文件已修复");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "修复MaterialSelectSettings.json文件失败");
                return false;
            }
        }

        /// <summary>
        /// 修复LogConfig.json文件
        /// </summary>
        private bool RepairLogConfig()
        {
            try
            {
                var configPath = AppDataPathManager.LogConfigPath;

                // 创建默认日志配置
                var defaultConfig = new Dictionary<string, object>
                {
                    ["LogLevel"] = "Debug",
                    ["LogDirectory"] = "logs",
                    ["LogFileNameFormat"] = "app_{0:yyyy-MM-dd}.log",
                    ["MaxFileSizeBytes"] = 10485760,
                    ["MaxRetainedFiles"] = 7,
                    ["EnableConsoleLogging"] = true,
                    ["LogFormat"] = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level}] {Message}{NewLine}{Exception}"
                };

                var json = JsonConvert.SerializeObject(defaultConfig, Formatting.Indented);
                File.WriteAllText(configPath, json);

                _logger.LogInformation("LogConfig.json文件已修复");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "修复LogConfig.json文件失败");
                return false;
            }
        }
    }

    /// <summary>
    /// 数据完整性检查报告
    /// </summary>
    public class IntegrityReport
    {
        public DateTime CheckTime { get; set; } = DateTime.Now;
        public List<ValidationResult> Items { get; set; } = new List<ValidationResult>();
        public bool HasCriticalError { get; set; }
        public string ErrorMessage { get; set; }
        public string ReportPath { get; set; }

        public int ValidCount => Items.Count(i => i.Status == ValidationStatus.Valid);
        public int WarningCount => Items.Count(i => i.Status == ValidationStatus.Warning);
        public int ErrorCount => Items.Count(i => i.Status == ValidationStatus.Error);

        public void AddValid(string item, string message)
        {
            Items.Add(new ValidationResult { Item = item, Status = ValidationStatus.Valid, Message = message });
        }

        public void AddWarning(string item, string message)
        {
            Items.Add(new ValidationResult { Item = item, Status = ValidationStatus.Warning, Message = message });
        }

        public void AddError(string item, string message)
        {
            Items.Add(new ValidationResult { Item = item, Status = ValidationStatus.Error, Message = message });
        }
    }

    /// <summary>
    /// 验证结果
    /// </summary>
    public class ValidationResult
    {
        public string Item { get; set; }
        public ValidationStatus Status { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// 验证状态
    /// </summary>
    public enum ValidationStatus
    {
        Valid,
        Warning,
        Error
    }
}