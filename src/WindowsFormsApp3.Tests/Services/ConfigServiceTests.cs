using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using WindowsFormsApp3.Services;
using WindowsFormsApp3.Models;
using WindowsFormsApp3.Interfaces;

namespace WindowsFormsApp3.Tests.Services
{
    public class ConfigServiceTests : IDisposable
    {
        private readonly WindowsFormsApp3.Services.IConfigService _configService;
        private readonly string _testDirectory;
        private readonly string _testConfigFile;

        public ConfigServiceTests()
        {
            // 创建测试目录
            _testDirectory = Path.Combine(Path.GetTempPath(), "ConfigServiceTests_", DateTime.Now.Ticks.ToString());
            Directory.CreateDirectory(_testDirectory);
            _testConfigFile = Path.Combine(_testDirectory, "test_config.json");

            // 创建配置服务，使用Mock的ILogger和ICacheService
            var mockLogger = new Mock<WindowsFormsApp3.Interfaces.ILogger>();
            var mockCacheService = new Mock<WindowsFormsApp3.Interfaces.ICacheService>();
            
            // 设置mock缓存服务的行为
            mockCacheService
                .Setup(x => x.GetOrCreate(It.IsAny<string>(), It.IsAny<Func<Dictionary<string, AppConfig>>>(), It.IsAny<TimeSpan>()))
                .Returns((string key, Func<Dictionary<string, AppConfig>> factory, TimeSpan expiration) => factory());
            
            _configService = new ConfigService(mockLogger.Object, mockCacheService.Object);
            
            // 清除所有配置，确保测试隔离
            if (_configService is ConfigService concreteConfigService && File.Exists(concreteConfigService.ConfigFilePath))
            {
                File.Delete(concreteConfigService.ConfigFilePath);
            }
        }

        [Fact]
        public void ExportConfigsToFile_Should_Save_Config_To_File()
        {
            // 准备测试配置数据
            var testConfig = new AppConfig
            {
                Separator = "_",
                Unit = "mm"
            };
            var configs = new Dictionary<string, AppConfig>
            {
                { "TestConfig", testConfig }
            };
            
            // 先保存配置到ConfigService
            _configService.SaveAllConfigs(configs);

            // 导出配置
            bool result = _configService.ExportConfigsToFile(_testConfigFile);

            // 验证结果和文件存在
            Assert.True(result);
            Assert.True(File.Exists(_testConfigFile));
        }

        [Fact]
        public void ImportConfigsFromFile_Should_Return_Null_When_File_Not_Exists()
        {
            // 导入不存在的配置文件
            var configData = _configService.ImportConfigsFromFile(Path.Combine(_testDirectory, "non_existent_config.json"));

            // 验证结果为null
            Assert.Null(configData);
        }

        [Fact]
        public void ImportConfigsFromFile_Should_Return_Null_When_File_Is_Empty()
        {
            // 创建空配置文件
            File.WriteAllText(_testConfigFile, string.Empty);

            // 导入空配置文件
            var configData = _configService.ImportConfigsFromFile(_testConfigFile);

            // 验证结果为null
            Assert.Null(configData);
        }

        [Fact]
        public void ImportConfigsFromFile_Should_Return_Null_When_File_Has_Invalid_Json()
        {
            // 创建无效的JSON配置文件
            File.WriteAllText(_testConfigFile, "Invalid JSON content");

            // 导入无效的配置文件
            var configData = _configService.ImportConfigsFromFile(_testConfigFile);

            // 验证结果为null
            Assert.Null(configData);
        }

        [Fact]
        public void LoadAllConfigs_Should_Return_Empty_Dictionary_When_No_Configs()
        {
            // 加载配置（此时应该没有配置）
            var configs = _configService.LoadAllConfigs();

            // 验证结果为空字典
            Assert.NotNull(configs);
            Assert.Empty(configs);
        }

        [Fact]
        public void SaveConfig_Should_Save_Single_Config()
        {
            // 准备测试配置
            var testConfig = new AppConfig { Separator = "-", Unit = "cm" };
            string configName = "NewTestConfig";
            
            // 保存配置
            _configService.SaveConfig(configName, testConfig);
            
            // 重新加载配置并验证
            var loadedConfig = _configService.GetConfig(configName);
            Assert.NotNull(loadedConfig);
            Assert.Equal("-", loadedConfig.Separator);
            Assert.Equal("cm", loadedConfig.Unit);
        }

        // 清理测试资源
        public void Dispose()
        {
            if (Directory.Exists(_testDirectory))
            {
                try
                {
                    Directory.Delete(_testDirectory, true);
                }
                catch (IOException)
                {
                    // 如果文件被锁定，忽略异常
                }
            }
        }

        [Fact]
        public void GetConfigNames_Should_Return_All_Config_Names()
        {
            // 准备测试配置
            var config1 = new AppConfig { Separator = "_" };
            var config2 = new AppConfig { Separator = "-" };
            
            _configService.SaveConfig("Config1", config1);
            _configService.SaveConfig("Config2", config2);

            // 获取配置名称列表
            List<string> configNames = _configService.GetConfigNames();

            // 验证结果
            Assert.NotNull(configNames);
            Assert.Contains("Config1", configNames);
            Assert.Contains("Config2", configNames);
            Assert.Equal(2, configNames.Count);
        }

        [Fact]
        public void GetConfigNames_Should_Return_Empty_List_When_No_Configs()
        {
            // 确保没有配置文件
            if (_configService is ConfigService concreteConfigService && File.Exists(concreteConfigService.ConfigFilePath))
            {
                File.Delete(concreteConfigService.ConfigFilePath);
            }

            // 获取配置名称列表
            List<string> configNames = _configService.GetConfigNames();

            // 验证结果为空列表
            Assert.NotNull(configNames);
            Assert.Empty(configNames);
        }

        [Fact]
        public void GetDefaultConfig_Should_Return_Valid_Default_Config()
        {
            // Act
            var defaultConfig = _configService.GetDefaultConfig();

            // Assert
            Assert.NotNull(defaultConfig);
            Assert.Equal("_", defaultConfig.Separator);
            Assert.Equal("", defaultConfig.Unit);
            Assert.Equal("3,5,10", defaultConfig.TetBleed);
            Assert.Equal("", defaultConfig.Material);
            Assert.Equal(100, defaultConfig.Opacity);
            Assert.True(defaultConfig.AddPdfLayers);
            Assert.Equal("Z", defaultConfig.ZeroShapeCode);
            Assert.Equal("R", defaultConfig.RoundShapeCode);
            Assert.Equal("Y", defaultConfig.EllipseShapeCode);
            Assert.Equal("C", defaultConfig.CircleShapeCode);
            Assert.False(defaultConfig.HideRadiusValue);
            Assert.NotNull(defaultConfig.RegexPatterns);
#pragma warning disable CS0618 // 保留向后兼容性测试
            Assert.NotNull(defaultConfig.EventItems);
#pragma warning restore CS0618
        }

        [Fact]
        public void ExportSingleConfig_Should_Save_Single_Config_To_File()
        {
            // 准备测试配置
            var testConfig = new AppConfig { Separator = "_", Unit = "mm" };
            string configName = "SingleExportConfig";
            
            // 保存配置
            _configService.SaveConfig(configName, testConfig);

            // 导出单个配置
            string exportFilePath = Path.Combine(_testDirectory, "single_config.json");
            bool result = _configService.ExportSingleConfig(configName, exportFilePath);

            // 验证导出成功且文件存在
            Assert.True(result);
            Assert.True(File.Exists(exportFilePath));

            // 验证导出的内容
            string fileContent = File.ReadAllText(exportFilePath);
            Assert.Contains(configName, fileContent);
            Assert.Contains("_", fileContent);
            Assert.Contains("mm", fileContent);
        }

        [Fact]
        public void ImportConfigsFromFile_Should_Return_Valid_Configs_When_File_Has_Valid_Json()
        {
            // 创建有效的配置文件
            var testConfigs = new Dictionary<string, AppConfig>
            {
                { "ImportedConfig1", new AppConfig { Separator = "_", Unit = "mm" } },
                { "ImportedConfig2", new AppConfig { Separator = "-", Unit = "cm" } }
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(testConfigs, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(_testConfigFile, json);

            // 导入有效的配置文件
            var importedConfigs = _configService.ImportConfigsFromFile(_testConfigFile);

            // 验证结果包含预期的配置
            Assert.NotNull(importedConfigs);
            Assert.Equal(2, importedConfigs.Count);
            Assert.Contains("ImportedConfig1", importedConfigs.Keys);
            Assert.Contains("ImportedConfig2", importedConfigs.Keys);
            Assert.Equal("_", importedConfigs["ImportedConfig1"].Separator);
            Assert.Equal("-", importedConfigs["ImportedConfig2"].Separator);
        }

        [Fact]
        public void GetConfig_Should_Return_Null_When_Config_Not_Exists()
        {
            // 获取不存在的配置
            var config = _configService.GetConfig("NonExistentConfig");

            // 验证结果为null
            Assert.Null(config);
        }

        [Fact]
        public void ExportSingleConfig_Should_Return_False_When_Config_Not_Exists()
        {
            // 导出不存在的配置
            string exportFilePath = Path.Combine(_testDirectory, "non_existent_config.json");
            bool result = _configService.ExportSingleConfig("NonExistentConfig", exportFilePath);

            // 验证导出失败
            Assert.False(result);
            Assert.False(File.Exists(exportFilePath));
        }

        [Fact]
        public void DeleteConfig_Should_Remove_Config_Successfully()
        {
            // 准备测试配置
            var testConfig = new AppConfig { Separator = "_" };
            string configName = "DeleteTestConfig";
            
            // 保存配置
            _configService.SaveConfig(configName, testConfig);

            // 验证配置存在
            var savedConfig = _configService.GetConfig(configName);
            Assert.NotNull(savedConfig);

            // 删除配置
            bool result = _configService.DeleteConfig(configName);

            // 验证删除成功
            Assert.True(result);
            var deletedConfig = _configService.GetConfig(configName);
            Assert.Null(deletedConfig);
        }

        [Fact]
        public void DeleteConfig_Should_Return_False_When_Config_Not_Exists()
        {
            // 删除不存在的配置
            bool result = _configService.DeleteConfig("NonExistentConfig");

            // 验证删除失败
            Assert.False(result);
        }
    }
}