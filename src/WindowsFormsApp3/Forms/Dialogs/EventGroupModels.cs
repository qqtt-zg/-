using System;
using System.Collections.Generic;
using WindowsFormsApp3.Models;

namespace WindowsFormsApp3.Forms.Dialogs
{
    /// <summary>
    /// 事件项目分组枚举
    /// </summary>
    public enum EventGroup
    {
        Order,      // 订单组
        Material,   // 材料组
        Quantity,   // 数量组
        Process,    // 工艺组
        Customer,   // 客户组
        Remark,     // 备注组
        Row,        // 行数组
        Column,     // 列数组
        Ungrouped   // 未分组
    }

    /// <summary>
    /// 分组配置类（用于SettingsForm）
    /// </summary>
    public class EventGroupConfig
    {
        public EventGroup Group { get; set; }
        public string DisplayName { get; set; }
        public string Prefix { get; set; }
        public bool IsEnabled { get; set; } = true;
        public int SortOrder { get; set; }
        public List<string> Items { get; set; } = new List<string>();

        /// <summary>
        /// 是否为保留分组（返单时保留原数据）
        /// </summary>
        public bool IsPreserved { get; set; } = false;
    }

    /// <summary>
    /// 分组排序配置（用于持久化分组顺序）
    /// </summary>
    public class EventGroupOrderConfig
    {
        /// <summary>
        /// 分组名称列表，按显示顺序排列
        /// </summary>
        public List<string> GroupOrder { get; set; } = new List<string>();

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        /// <summary>
        /// 配置版本号（用于兼容性检查）
        /// </summary>
        public string Version { get; set; } = "1.0";
    }

    /// <summary>
    /// 事件项目类（用于SettingsForm）
    /// </summary>
    public class EventItem
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public EventGroup Group { get; set; }
        public int SortOrder { get; set; }

        /// <summary>
        /// 是否为保留项目（同一分组内只能有一个保留项目）
        /// </summary>
        public bool IsPreserved { get; set; } = false;
    }

    /// <summary>
    /// TreeView节点类型
    /// </summary>
    public enum TreeNodeType
    {
        Group,
        Item
    }

    /// <summary>
    /// TreeView节点数据
    /// </summary>
    public class TreeNodeData
    {
        public TreeNodeType NodeType { get; set; }
        public EventGroup? Group { get; set; }
        public string ItemName { get; set; }
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 是否为保留状态（用于返单时保留原数据）
        /// 对于分组：表示该分组是否启用保留功能
        /// 对于项目：表示该项目是否为分组内的保留项目（同一分组内只能有一个保留项目）
        /// </summary>
        public bool IsPreserved { get; set; } = false;
    }
}