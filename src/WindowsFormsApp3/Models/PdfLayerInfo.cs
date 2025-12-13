namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// PDF图层信息类
    /// </summary>
    public class PdfLayerInfo
    {
        /// <summary>
        /// 获取或设置图层名称
        /// </summary>
        public string LayerName { get; set; }

        /// <summary>
        /// 获取或设置图层内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 获取或设置图层位置（X坐标）
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// 获取或设置图层位置（Y坐标）
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// 获取或设置字体大小
        /// </summary>
        public float FontSize { get; set; }
    }
}