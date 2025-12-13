using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using WindowsFormsApp3.Models;
using WindowsFormsApp3.Interfaces;
using WindowsFormsApp3.Utils;
using System.Linq;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 配置管理服务实现
    /// </summary>
    public class ConfigService : IConfigService
    {
        public string ConfigFilePath => AppDataPathManager.ConfigFilePath;
        private readonly WindowsFormsApp3.Interfaces.ILogger _logger;
        private readonly ICacheService _cacheService;
        
        // 缓存相关常量
        private const string ALL_CONFIGS_CACHE_KEY = "all_configs";
        private static readonly TimeSpan CONFIG_CACHE_EXPIRATION = TimeSpan.FromMinutes(5);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志服务</param>
        /// <param name="cacheService">缓存服务</param>
        public ConfigService(WindowsFormsApp3.Interfaces.ILogger logger, ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        /// <summary>
        /// 加载指定键的配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="configKey">配置键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置对象</returns>
        public T LoadConfig<T>(string configKey, T defaultValue = default(T)) where T : class
        {
            try
            {
                var configs = LoadAllConfigs();
                if (configs.TryGetValue(configKey, out AppConfig appConfig))
                {
                    // 如果类型匹配，直接返回
                    if (appConfig is T)
                    {
                        return appConfig as T;
                    }
                    // 否则尝试序列化/反序列化转换类型
                    string json = JsonConvert.SerializeObject(appConfig);
                    return JsonConvert.DeserializeObject<T>(json);
                }
                return defaultValue;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载配置失败");
                return defaultValue;
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="configKey">配置键</param>
        /// <param name="config">配置对象</param>
        public void SaveConfig<T>(string configKey, T config) where T : class
        {
            try
            {
                // 如果是AppConfig类型，直接保存
                if (config is AppConfig appConfig)
                {
                    SaveConfig(configKey, appConfig);
                }
                else
                {
                    // 对于非AppConfig类型的配置，我们创建一个新的AppConfig对象
                    // 并使用JSON序列化来存储配置数据
                    string json = JsonConvert.SerializeObject(config);
                    SaveRawConfig(configKey, json);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存配置失败");
                throw;
            }
        }
        
        /// <summary>
        /// 保存原始配置JSON字符串
        /// </summary>
        /// <param name="configKey">配置键</param>
        /// <param name="jsonContent">JSON内容</param>
        private void SaveRawConfig(string configKey, string jsonContent)
        {
            try
            {
                var configs = LoadAllConfigs();
                // 创建一个空的AppConfig对象来存储配置键
                configs[configKey] = new AppConfig();
                // 保存配置
                SaveAllConfigs(configs);
                // 将原始JSON保存到单独的文件
                string rawConfigPath = Path.Combine(Path.GetDirectoryName(ConfigFilePath), $"{configKey}_raw.json");
                File.WriteAllText(rawConfigPath, jsonContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存原始配置失败");
                throw;
            }
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="configKey">配置键</param>
        /// <returns>删除是否成功</returns>
        public bool DeleteConfig(string configKey)
        {
            try
            {
                // 加载现有配置
                var configs = LoadAllConfigs();
                
                // 检查配置是否存在
                if (configs.ContainsKey(configKey))
                {
                    // 删除配置
                    configs.Remove(configKey);
                    
                    // 保存更新后的配置
                    SaveAllConfigs(configs);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除配置失败");
                return false;
            }
        }

        /// <summary>
        /// 检查配置是否存在
        /// </summary>
        /// <param name="configKey">配置键</param>
        /// <returns>配置是否存在</returns>
        public bool ConfigExists(string configKey)
        {
            try
            {
                var configs = LoadAllConfigs();
                return configs.ContainsKey(configKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查配置是否存在失败");
                return false;
            }
        }

        /// <summary>
        /// 加载所有配置
        /// </summary>
        /// <returns>配置字典，键为配置名称，值为配置对象</returns>
        public Dictionary<string, AppConfig> LoadAllConfigs()
        {
            // 使用缓存获取配置，如果缓存中没有则从文件加载
            return _cacheService.GetOrCreate(ALL_CONFIGS_CACHE_KEY, () =>
            {
                try
                {
                    if (!File.Exists(ConfigFilePath))
                        return new Dictionary<string, AppConfig>();

                    var json = File.ReadAllText(ConfigFilePath);
                    return JsonConvert.DeserializeObject<Dictionary<string, AppConfig>>(json) ?? new Dictionary<string, AppConfig>();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "加载配置失败");
                    return new Dictionary<string, AppConfig>();
                }
            }, CONFIG_CACHE_EXPIRATION);
        }

        /// <summary>
        /// 保存所有配置
        /// </summary>
        /// <param name="configs">要保存的配置字典</param>
        public void SaveAllConfigs(Dictionary<string, AppConfig> configs)
        {
            try
            {
                // 确保目录存在
                IOHelper.EnsureDirectoryExists(Path.GetDirectoryName(ConfigFilePath));
                
                // 序列化并保存配置
                var json = JsonConvert.SerializeObject(configs, Formatting.Indented);
                File.WriteAllText(ConfigFilePath, json);
                
                // 清除缓存以确保数据一致性
                _cacheService.Remove(ALL_CONFIGS_CACHE_KEY);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存配置失败");
                throw;
            }
        }

        /// <summary>
        /// 保存单个配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="config">配置对象</param>
        public void SaveConfig(string configName, AppConfig config)
        {
            try
            {
                // 加载现有配置
                var configs = LoadAllConfigs();
                
                // 确保配置字典不为null
                if (configs == null)
                {
                    configs = new Dictionary<string, AppConfig>();
                }
                
                // 添加或更新配置
                if (configs.ContainsKey(configName))
                {
                    configs[configName] = config;
                }
                else
                {
                    configs.Add(configName, config);
                }
                
                // 保存所有配置
                SaveAllConfigs(configs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存单个配置失败");
                throw;
            }
        }

        /// <summary>
        /// 删除配置并返回删除结果
        /// </summary>
        /// <param name="configName">要删除的配置名称</param>
        /// <returns>删除是否成功</returns>
        private bool DeleteConfigInternal(string configName)
        {
            try
            {
                // 加载现有配置
                var configs = LoadAllConfigs();
                
                // 检查配置是否存在
                if (configs.ContainsKey(configName))
                {
                    // 删除配置
                    configs.Remove(configName);
                    
                    // 保存更新后的配置
                    SaveAllConfigs(configs);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除配置失败");
                return false;
            }
        }

        /// <summary>
        /// 获取指定名称的配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns>配置对象，如果不存在则返回null</returns>
        public AppConfig GetConfig(string configName)
        {
            try
            {
                var configs = LoadAllConfigs();
                
                // 确保配置字典不为null
                if (configs == null)
                {
                    return null;
                }
                
                if (configs.TryGetValue(configName, out var config))
                {
                    return config;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取配置失败");
                return null;
            }
        }

        /// <summary>
        /// 获取配置名称列表
        /// </summary>
        /// <returns>配置名称列表</returns>
        public List<string> GetConfigNames()
        {
            try
            {
                var configs = LoadAllConfigs();
                
                // 确保配置字典不为null
                if (configs == null)
                {
                    return new List<string>();
                }
                
                return new List<string>(configs.Keys);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取配置名称列表失败");
                return new List<string>();
            }
        }

        /// <summary>
        /// 获取默认配置
        /// </summary>
        /// <returns>默认配置对象</returns>
        /// <summary>
        /// 获取默认配置
        /// </summary>
        /// <returns>默认配置对象</returns>
        public AppConfig GetDefaultConfig()
        {
            return new AppConfig
            {
                Separator = "_",
                Unit = "",
                TetBleed = "3,5,10",
                Material = "",
                Opacity = 100,
                HotkeyToggle = "",
                FixedFieldPresets = "",
                RegexPatterns = new Dictionary<string, string>(),
#pragma warning disable CS0618 // 保留向后兼容性
                EventItems = new List<string>(),
#pragma warning restore CS0618
                AddPdfLayers = true,
                ZeroShapeCode = "Z",
                RoundShapeCode = "R",
                EllipseShapeCode = "Y",
                CircleShapeCode = "C",
                HideRadiusValue = false
            };
        }

        /// <summary>
        /// 从文件导入配置
        /// </summary>
        /// <param name="filePath">导入文件路径</param>
        /// <returns>导入的配置字典</returns>
        public Dictionary<string, AppConfig> ImportConfigsFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _logger.LogError("导入文件不存在: " + filePath);
                    return null;
                }

                var json = File.ReadAllText(filePath);
                var importedConfigs = JsonConvert.DeserializeObject<Dictionary<string, AppConfig>>(json);

                if (importedConfigs == null)
                {
                    _logger.LogError("导入配置格式无效: " + filePath);
                    return null;
                }

                _logger.LogInformation("成功导入配置文件: " + filePath);
                return importedConfigs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "导入配置失败");
                return null;
            }
        }

        /// <summary>
        /// 导出所有配置到文件
        /// </summary>
        /// <param name="filePath">导出文件路径</param>
        /// <returns>导出是否成功</returns>
        public bool ExportConfigsToFile(string filePath)
        {
            try
            {
                var configs = LoadAllConfigs();
                
                // 确保配置字典不为null
                if (configs == null)
                {
                    configs = new Dictionary<string, AppConfig>();
                }
                
                var json = JsonConvert.SerializeObject(configs, Formatting.Indented);
                
                // 确保目录存在
                IOHelper.EnsureDirectoryExists(Path.GetDirectoryName(filePath));
                File.WriteAllText(filePath, json);
                
                _logger.LogInformation("成功导出配置文件: " + filePath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "导出配置失败");
                return false;
            }
        }

        /// <summary>
        /// 导出单个配置到文件
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="filePath">导出文件路径</param>
        /// <returns>导出是否成功</returns>
        public bool ExportSingleConfig(string configName, string filePath)
        {
            try
            {
                var config = GetConfig(configName);
                if (config == null)
                {
                    _logger.LogError("配置不存在: " + configName);
                    return false;
                }

                var singleConfigDict = new Dictionary<string, AppConfig> { { configName, config } };
                var json = JsonConvert.SerializeObject(singleConfigDict, Formatting.Indented);
                
                // 确保目录存在
                IOHelper.EnsureDirectoryExists(Path.GetDirectoryName(filePath));
                File.WriteAllText(filePath, json);
                
                _logger.LogInformation("成功导出配置: " + configName + " 到 " + filePath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "导出单个配置失败");
                return false;
            }
        }

        private Dictionary<string, object> _runtimeConfig = new Dictionary<string, object>();

        /// <summary>
        /// 获取配置项的值
        /// </summary>
        /// <param name="key">配置项的键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置项的值</returns>
        public object GetValue(string key, object defaultValue = null)
        {
            if (_runtimeConfig.TryGetValue(key, out object value))
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// 设置配置项的值
        /// </summary>
        /// <param name="key">配置项的键</param>
        /// <param name="value">配置项的值</param>
        public void SetValue(string key, object value)
        {
            _runtimeConfig[key] = value;
        }

        /// <summary>
        /// 保存当前配置
        /// </summary>
        public void SaveConfig()
        {
            try
            {
                // 将运行时配置保存到一个特殊的配置项中
                var configs = LoadAllConfigs();
                if (configs.TryGetValue("RuntimeConfig", out AppConfig runtimeConfig))
                {
                    // 更新现有配置
                    foreach (var kvp in _runtimeConfig)
                    {
                        // 根据键值设置对应的属性
                        SetProperty(runtimeConfig, kvp.Key, kvp.Value);
                    }
                }
                else
                {
                    // 创建新的运行时配置
                    runtimeConfig = new AppConfig();
                    foreach (var kvp in _runtimeConfig)
                    {
                        SetProperty(runtimeConfig, kvp.Key, kvp.Value);
                    }
                }

                SaveConfig("RuntimeConfig", runtimeConfig);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存运行时配置失败");
                throw;
            }
        }

        /// <summary>
        /// 设置AppConfig对象的属性
        /// </summary>
        private void SetProperty(AppConfig config, string key, object value)
        {
            try
            {
                var property = config.GetType().GetProperty(key);
                if (property != null && property.CanWrite)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(config, value?.ToString());
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        property.SetValue(config, Convert.ToInt32(value));
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        property.SetValue(config, Convert.ToBoolean(value));
                    }
                    else if (property.PropertyType == typeof(Dictionary<string, string>))
                    {
                        property.SetValue(config, value as Dictionary<string, string>);
                    }
                    else if (property.PropertyType == typeof(List<string>))
                    {
                        property.SetValue(config, value as List<string>);
                    }
                }
            }
            catch
            {
                // 如果设置失败，忽略该属性
            }
        }
    }
}