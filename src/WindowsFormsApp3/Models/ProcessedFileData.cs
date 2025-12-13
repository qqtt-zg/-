using System;
using System.IO;

namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// 处理后的文件数据DTO，用于在服务间传递文件处理结果
    /// </summary>
    public class ProcessedFileData
    {
        /// <summary>
        /// 新文件名
        /// </summary>
        public string NewFileName { get; set; }

        /// <summary>
        /// 目标文件路径
        /// </summary>
        public string DestinationPath { get; set; }

        /// <summary>
        /// 正则表达式匹配结果
        /// </summary>
        public string RegexResult { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 材料
        /// </summary>
        public string Material { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string Quantity { get; set; }

        /// <summary>
        /// 尺寸
        /// </summary>
        public string Dimensions { get; set; }

        /// <summary>
        /// 工艺
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 列组合值
        /// </summary>
        public string CompositeColumn { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        public string LayoutRows { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        public string LayoutColumns { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        public int? PageCount { get; set; }
    }
}