using System;

namespace WindowsFormsApp3.Services.Events
{
    /// <summary>
    /// 配置变更事件基类
    /// </summary>
    public abstract class ConfigChangedEvent
    {
        /// <summary>
        /// 配置键
        /// </summary>
        public string ConfigKey { get; set; }
        
        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// 操作用户（如果有）
        /// </summary>
        public string User { get; set; }
        
        protected ConfigChangedEvent()
        {
            Timestamp = DateTime.Now;
        }
    }
    
    /// <summary>
    /// 配置项变更事件
    /// </summary>
    public class ConfigItemChangedEvent : ConfigChangedEvent
    {
        /// <summary>
        /// 原值
        /// </summary>
        public object OldValue { get; set; }
        
        /// <summary>
        /// 新值
        /// </summary>
        public object NewValue { get; set; }
        
        /// <summary>
        /// 配置项类型
        /// </summary>
        public string ConfigType { get; set; }
    }
    
    /// <summary>
    /// 配置保存事件
    /// </summary>
    public class ConfigSavedEvent : ConfigChangedEvent
    {
        /// <summary>
        /// 保存的配置项数量
        /// </summary>
        public int SavedItemsCount { get; set; }
        
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string ConfigFilePath { get; set; }
    }
    
    /// <summary>
    /// 配置加载事件
    /// </summary>
    public class ConfigLoadedEvent : ConfigChangedEvent
    {
        /// <summary>
        /// 加载的配置项数量
        /// </summary>
        public int LoadedItemsCount { get; set; }
        
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string ConfigFilePath { get; set; }
    }
}