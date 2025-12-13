using System;

namespace WindowsFormsApp3.EventArguments
{
    /// <summary>
    /// 项目移动事件参数
    /// </summary>
    public class ItemMovedEventArgs : EventArgs
    {
        /// <summary>
        /// 移动的项目名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 源分组
        /// </summary>
        public string FromGroup { get; set; }

        /// <summary>
        /// 目标分组
        /// </summary>
        public string ToGroup { get; set; }

        /// <summary>
        /// 移动时间
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// 操作来源
        /// </summary>
        public string Source { get; set; } = "拖拽操作";

        /// <summary>
        /// 附加信息
        /// </summary>
        public string Message { get; set; } = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemMovedEventArgs()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="itemName">项目名称</param>
        /// <param name="fromGroup">源分组</param>
        /// <param name="toGroup">目标分组</param>
        public ItemMovedEventArgs(string itemName, string fromGroup, string toGroup)
        {
            ItemName = itemName;
            FromGroup = fromGroup;
            ToGroup = toGroup;
        }

        /// <summary>
        /// 转换为字符串描述
        /// </summary>
        /// <returns>描述信息</returns>
        public override string ToString()
        {
            return $"项目 '{ItemName}' 从 '{FromGroup}' 移动到 '{ToGroup}' (来源: {Source}) {Message}";
        }
    }
}