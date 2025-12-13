using System;
using WindowsFormsApp3.Forms.Dialogs;

namespace WindowsFormsApp3.EventArguments
{
    /// <summary>
    /// 保留状态变化事件参数
    /// </summary>
    public class PreserveStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 变化的分组
        /// </summary>
        public EventGroup Group { get; set; }

        /// <summary>
        /// 变化的项目名称（可选，用于项目级别的保留）
        /// </summary>
        public string ItemName { get; set; } = "";

        /// <summary>
        /// 是否保留
        /// </summary>
        public bool IsPreserved { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// 操作来源（如"用户手动设置"、"程序自动设置"等）
        /// </summary>
        public string Source { get; set; } = "用户操作";

        /// <summary>
        /// 附加信息
        /// </summary>
        public string Message { get; set; } = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        public PreserveStateChangedEventArgs()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="group">分组</param>
        /// <param name="isPreserved">是否保留</param>
        public PreserveStateChangedEventArgs(EventGroup group, bool isPreserved)
        {
            Group = group;
            IsPreserved = isPreserved;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="group">分组</param>
        /// <param name="isPreserved">是否保留</param>
        /// <param name="source">操作来源</param>
        /// <param name="message">附加信息</param>
        public PreserveStateChangedEventArgs(EventGroup group, bool isPreserved, string source, string message = "")
        {
            Group = group;
            IsPreserved = isPreserved;
            Source = source;
            Message = message;
        }

        /// <summary>
        /// 转换为字符串描述
        /// </summary>
        /// <returns>描述信息</returns>
        public override string ToString()
        {
            var entity = string.IsNullOrEmpty(ItemName) ? $"分组 {Group}" : $"项目 '{ItemName}' (分组 {Group})";
            return $"{entity} 保留状态变更为: {(IsPreserved ? "保留" : "不保留")} (来源: {Source}) {Message}";
        }
    }
}