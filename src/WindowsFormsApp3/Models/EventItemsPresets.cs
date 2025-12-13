using System;
using System.Collections.Generic;

namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// EventItems预设配置
    /// 用于管理重命名规则项的不同预设方案
    /// </summary>
    public class EventItemsPresets
    {
        /// <summary>
        /// 上次使用的预设名称
        /// </summary>
        public string LastUsedPreset { get; set; } = "默认方案";

        /// <summary>
        /// 所有预设方案
        /// Key: 方案名称
        /// Value: EventItems字符串（格式："项目名|勾选状态|项目名|勾选状态..."）
        /// </summary>
        public Dictionary<string, string> Presets { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 初始化默认预设方案
        /// </summary>
        public void InitializeDefaultPresets()
        {
            if (Presets.Count == 0)
            {
                // 默认方案：全部启用
                Presets["默认方案"] = "正则结果|True|订单号|True|材料|True|数量|True|工艺|True|尺寸|True|序号|False|列组合|True|行数|True|列数|True";

                // 简化方案：只保留核心项
                Presets["简化方案"] = "订单号|True|材料|True|尺寸|True|数量|True|正则结果|False|工艺|False|序号|False|列组合|False|行数|False|列数|False";

                // 详细方案：全部启用包括序号
                Presets["详细方案"] = "正则结果|True|订单号|True|材料|True|数量|True|工艺|True|尺寸|True|序号|True|列组合|True|行数|True|列数|True";
            }
        }

        /// <summary>
        /// 获取预设的EventItems字符串
        /// </summary>
        public string GetPresetEventItems(string presetName)
        {
            if (Presets.TryGetValue(presetName, out var eventItems))
            {
                return eventItems;
            }
            return null;
        }

        /// <summary>
        /// 检查预设是否为内置预设（不可删除）
        /// </summary>
        public bool IsBuiltInPreset(string presetName)
        {
            return presetName == "默认方案" || presetName == "简化方案" || presetName == "详细方案";
        }
    }
}
