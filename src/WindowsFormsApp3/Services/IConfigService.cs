using System.Collections.Generic;
using WindowsFormsApp3.Models;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 配置管理服务接口，负责应用程序配置的加载、保存、导入和导出
    /// 提供统一的配置访问和管理机制
    /// </summary>
    public interface IConfigService
    {
        /// <summary>
        /// 加载所有已保存的配置项
        /// </summary>
        /// <returns>包含所有配置的字典，键为配置名称，值为配置对象</returns>
        /// <exception cref="IOException">当配置文件读取失败时抛出</exception>
        Dictionary<string, AppConfig> LoadAllConfigs();

        /// <summary>
        /// 保存所有配置项到配置文件
        /// </summary>
        /// <param name="configs">要保存的配置字典，键为配置名称，值为配置对象</param>
        /// <exception cref="ArgumentNullException">当configs为null时抛出</exception>
        /// <exception cref="IOException">当配置文件写入失败时抛出</exception>
        void SaveAllConfigs(Dictionary<string, AppConfig> configs);

        /// <summary>
        /// 保存单个配置项
        /// </summary>
        /// <param name="configName">配置的唯一名称</param>
        /// <param name="config">要保存的配置对象</param>
        /// <exception cref="ArgumentNullException">当configName为null或空，或config为null时抛出</exception>
        /// <exception cref="IOException">当配置文件写入失败时抛出</exception>
        void SaveConfig(string configName, AppConfig config);

        /// <summary>
        /// 删除指定名称的配置项
        /// </summary>
        /// <param name="configName">要删除的配置名称</param>
        /// <returns>删除操作是否成功</returns>
        /// <exception cref="ArgumentNullException">当configName为null或空时抛出</exception>
        bool DeleteConfig(string configName);

        /// <summary>
        /// 获取指定名称的配置项
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns>配置对象，如果配置不存在则返回null</returns>
        /// <exception cref="ArgumentNullException">当configName为null或空时抛出</exception>
        AppConfig GetConfig(string configName);

        /// <summary>
        /// 获取所有配置项的名称列表
        /// </summary>
        /// <returns>包含所有配置名称的列表</returns>
        List<string> GetConfigNames();

        /// <summary>
        /// 获取默认配置项
        /// </summary>
        /// <returns>默认配置对象</returns>
        AppConfig GetDefaultConfig();

        /// <summary>
        /// 从外部文件导入配置
        /// </summary>
        /// <param name="filePath">导入文件的完整路径</param>
        /// <returns>导入的配置字典，键为配置名称，值为配置对象</returns>
        /// <exception cref="ArgumentNullException">当filePath为null或空时抛出</exception>
        /// <exception cref="FileNotFoundException">当指定的文件不存在时抛出</exception>
        /// <exception cref="IOException">当文件读取失败时抛出</exception>
        Dictionary<string, AppConfig> ImportConfigsFromFile(string filePath);

        /// <summary>
        /// 导出所有配置到外部文件
        /// </summary>
        /// <param name="filePath">导出文件的完整路径</param>
        /// <returns>导出操作是否成功</returns>
        /// <exception cref="ArgumentNullException">当filePath为null或空时抛出</exception>
        /// <exception cref="IOException">当文件写入失败时抛出</exception>
        bool ExportConfigsToFile(string filePath);

        /// <summary>
        /// 导出单个配置到外部文件
        /// </summary>
        /// <param name="configName">要导出的配置名称</param>
        /// <param name="filePath">导出文件的完整路径</param>
        /// <returns>导出操作是否成功</returns>
        /// <exception cref="ArgumentNullException">当configName或filePath为null或空时抛出</exception>
        /// <exception cref="IOException">当文件写入失败时抛出</exception>
        bool ExportSingleConfig(string configName, string filePath);

        /// <summary>
        /// 获取配置项的值
        /// </summary>
        /// <param name="key">配置项的键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置项的值</returns>
        object GetValue(string key, object defaultValue = null);

        /// <summary>
        /// 设置配置项的值
        /// </summary>
        /// <param name="key">配置项的键</param>
        /// <param name="value">配置项的值</param>
        void SetValue(string key, object value);

        /// <summary>
        /// 保存当前配置
        /// </summary>
        void SaveConfig();
    }
}