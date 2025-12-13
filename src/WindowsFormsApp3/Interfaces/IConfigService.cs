using System;
using System.Collections.Generic;
using WindowsFormsApp3.Models;

namespace WindowsFormsApp3.Interfaces
{
    public interface IConfigService
    {
        T LoadConfig<T>(string configKey, T defaultValue = default(T)) where T : class;
        void SaveConfig<T>(string configKey, T config) where T : class;
        void DeleteConfig(string configKey);
        bool ConfigExists(string configKey);
        List<string> GetConfigNames();
        AppConfig GetDefaultConfig();
        Dictionary<string, AppConfig> ImportConfigsFromFile(string filePath);
        bool ExportConfigsToFile(string filePath);
        bool ExportSingleConfig(string configName, string filePath);
    }
}