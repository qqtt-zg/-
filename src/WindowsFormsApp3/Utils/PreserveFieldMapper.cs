using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// 保留功能的字段映射器
    /// 负责在项目名称（treeViewEvents中的字段）和 FileRenameInfo 属性之间建立映射
    /// </summary>
    public static class PreserveFieldMapper
    {
        /// <summary>
        /// 项目名称 → FileRenameInfo 属性名 的完整映射表
        /// 覆盖 treeViewEvents 中的所有字段
        /// </summary>
        public static readonly Dictionary<string, string> FieldMapping = 
            new Dictionary<string, string>
            {
                // 标准字段（已有属性）
                { "订单号", "OrderNumber" },       // ✅ FileRenameInfo 中存在
                { "材料", "Material" },            // ✅ FileRenameInfo 中存在
                { "数量", "Quantity" },            // ✅ FileRenameInfo 中存在
                { "工艺", "Process" },             // ✅ Notes → Process (已修改)
                { "正则结果", "RegexResult" },     // ✅ FileRenameInfo 中存在
                { "尺寸", "Dimensions" },          // ✅ FileRenameInfo 中存在
                { "序号", "SerialNumber" },        // ✅ FileRenameInfo 中存在
                { "列组合", "CompositeColumn" },   // ✅ FileRenameInfo 中存在
                { "行数", "LayoutRows" },          // ✅ 新增属性 (FileRenameInfo 中存在)
                { "列数", "LayoutColumns" },       // ✅ 新增属性 (FileRenameInfo 中存在)

                
                // 注：已删除 treeViewEvents 中不存在的下列映射：
                // { "时间", "Time" },           // treeViewEvents 中不存在
                // { "规格", "Dimensions" },     // treeViewEvents 中不存在
                // { "客户", "Customer" },      // treeViewEvents 中不存在

                // 兼容性映射（防止旧数据导入时失败）
                // 注：删除了不符合的兼容性映射：
                // { "工艺(Notes)", "Process" },  // 旧映射：Notes 已重命名
            };

        /// <summary>
        /// 获取所有支持的备份字段名称
        /// </summary>
        public static List<string> GetAllSupportedFields()
        {
            return FieldMapping.Keys
                .Where(k => !k.Contains("("))  // 排除兼容性映射
                .ToList();
        }

        /// <summary>
        /// 项目名称 → 属性名
        /// 获取指定项目对应的属性名
        /// </summary>
        public static string GetPropertyName(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return null;

            return FieldMapping.TryGetValue(fieldName, out var propertyName) 
                ? propertyName 
                : null;
        }

        /// <summary>
        /// 属性名 → 项目名称
        /// 反向映射：根据属性名获取项目名称
        /// </summary>
        public static string GetFieldName(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return null;

            var mapping = FieldMapping.FirstOrDefault(x => x.Value == propertyName);
            return mapping.Key;
        }

        /// <summary>
        /// 验证映射是否有效
        /// 检查字段是否在映射表中，且对应的属性名非空
        /// </summary>
        public static bool IsValidMapping(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return false;

            return FieldMapping.TryGetValue(fieldName, out var propertyName) &&
                   !string.IsNullOrEmpty(propertyName);
        }

        /// <summary>
        /// 检查属性是否在 FileRenameInfo 中存在
        /// </summary>
        public static bool IsPropertyExists(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return false;

            var property = typeof(FileRenameInfo).GetProperty(propertyName, 
                BindingFlags.Public | BindingFlags.IgnoreCase);
            return property != null && property.CanWrite;
        }

        /// <summary>
        /// 获取缺失的属性映射
        /// 返回映射表中指定但 FileRenameInfo 中不存在的属性
        /// </summary>
        public static List<string> GetMissingProperties()
        {
            var missing = new List<string>();
            foreach (var mapping in FieldMapping)
            {
                if (!IsPropertyExists(mapping.Value))
                {
                    missing.Add($"{mapping.Key} → {mapping.Value}");
                }
            }
            return missing;
        }

        /// <summary>
        /// 获取映射统计信息
        /// </summary>
        public static MappingStatistics GetMappingStatistics()
        {
            var stats = new MappingStatistics();

            foreach (var mapping in FieldMapping)
            {
                if (!mapping.Key.Contains("("))  // 排除兼容性映射
                {
                    if (IsPropertyExists(mapping.Value))
                    {
                        stats.ValidMappings.Add(mapping);
                    }
                    else
                    {
                        stats.MissingProperties.Add(mapping.Key, mapping.Value);
                    }
                }
            }

            return stats;
        }
    }

    /// <summary>
    /// 映射统计信息
    /// </summary>
    public class MappingStatistics
    {
        public List<KeyValuePair<string, string>> ValidMappings { get; set; } = new List<KeyValuePair<string, string>>();
        public Dictionary<string, string> MissingProperties { get; set; } = new Dictionary<string, string>();

        public int TotalMappings => ValidMappings.Count + MissingProperties.Count;
        public int ValidCount => ValidMappings.Count;
        public int MissingCount => MissingProperties.Count;

        public override string ToString()
        {
            return $"映射总数: {TotalMappings}, 有效: {ValidCount}, 缺失属性: {MissingCount}";
        }
    }
}
