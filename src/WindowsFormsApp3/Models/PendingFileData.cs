using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// 待处理文件数据传输对象
    /// 用于封装添加到待处理列表时的文件信息和相关数据
    /// </summary>
    public class PendingFileData
    {
        /// <summary>
        /// 文件信息
        /// </summary>
        public FileInfo FileInfo { get; set; }

        /// <summary>
        /// 文件宽度
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// 文件高度
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// 出血值
        /// </summary>
        public double TetBleed { get; set; }

        /// <summary>
        /// 调整后的尺寸
        /// </summary>
        public string AdjustedDimensions { get; set; }

        /// <summary>
        /// 正则表达式匹配结果
        /// </summary>
        public string RegexResult { get; set; }

        /// <summary>
        /// 正则表达式模式
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string Quantity { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 匹配的Excel行数据
        /// </summary>
        public List<DataRow> MatchedRows { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PendingFileData()
        {
            FileInfo = null;
            Width = "0";
            Height = "0";
            TetBleed = 0.0;
            AdjustedDimensions = string.Empty;
            RegexResult = string.Empty;
            Pattern = string.Empty;
            Quantity = string.Empty;
            SerialNumber = string.Empty;
            MatchedRows = new List<DataRow>();
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public PendingFileData(FileInfo fileInfo, string width, string height, double tetBleed)
        {
            FileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
            Width = width ?? "0";
            Height = height ?? "0";
            TetBleed = tetBleed;
            AdjustedDimensions = string.Empty;
            RegexResult = string.Empty;
            Pattern = string.Empty;
            Quantity = string.Empty;
            SerialNumber = string.Empty;
            MatchedRows = new List<DataRow>();
        }
    }
}