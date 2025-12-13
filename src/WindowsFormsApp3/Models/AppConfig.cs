using System;
using System.Collections.Generic;

namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// 应用程序配置类
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// 文件名分隔符
        /// </summary>
        /// <summary>
        /// 文件名分隔符
        /// </summary>
        public string Separator { get; set; } = "_";
        
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; } = "";
        
        /// <summary>
        /// 出血值设置
        /// </summary>
        public string TetBleed { get; set; } = "3,5,10";
        
        /// <summary>
        /// 材料
        /// </summary>
        public string Material { get; set; } = "";
        
        /// <summary>
        /// 透明度
        /// </summary>
        public int Opacity { get; set; } = 100;
        
        /// <summary>
        /// 最小化快捷键
        /// </summary>
        public string HotkeyToggle { get; set; } = "";
        
        /// <summary>
        /// 固定字段预设
        /// </summary>
        public string FixedFieldPresets { get; set; } = "";
        
        /// <summary>
        /// 正则表达式模式字典
        /// </summary>
        public Dictionary<string, string> RegexPatterns { get; set; } = new Dictionary<string, string>();
        
        /// <summary>
        /// 事件项列表
        /// </summary>
        /// <summary>
        /// 兼容旧版本事件项配置
        /// </summary>
        [Obsolete("此属性已弃用，请使用 EventGroupConfiguration 属性来管理事件项目配置")]
        public List<string> EventItems { get; set; } = new List<string>();
        
        /// <summary>
        /// 是否添加PDF图层
        /// </summary>
        public bool AddPdfLayers { get; set; } = true;
        
        /// <summary>
        /// 零形状代号
        /// </summary>
        public string ZeroShapeCode { get; set; } = "Z";
        
        /// <summary>
        /// 圆角矩形形状代号
        /// </summary>
        public string RoundShapeCode { get; set; } = "R";
        
        /// <summary>
        /// 椭圆形形状代号
        /// </summary>
        public string EllipseShapeCode { get; set; } = "Y";
        
        /// <summary>
        /// 圆形形状代号
        /// </summary>
        public string CircleShapeCode { get; set; } = "C";
        
        /// <summary>
        /// 是否隐藏半径值
        /// </summary>
        public bool HideRadiusValue { get; set; } = false;

        /// <summary>
        /// 左键膜类设置
        /// </summary>
        public string LeftClickFilm { get; set; } = "光膜,不过膜";

        /// <summary>
        /// 右键膜类设置
        /// </summary>
        public string RightClickFilm { get; set; } = "哑膜,红膜";

    }
}