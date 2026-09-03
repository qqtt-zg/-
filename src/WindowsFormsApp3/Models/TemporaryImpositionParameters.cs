namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// 材料选择对话框本次使用的临时排版参数，不写入全局自动排版设置。
    /// </summary>
    public class TemporaryImpositionParameters
    {
        public bool IsPaperSizeUnlocked { get; set; }

        public float PaperWidth { get; set; }
        public float PaperHeight { get; set; }
        public float FixedWidth { get; set; }
        public float MinLength { get; set; }

        public float MarginTop { get; set; }
        public float MarginBottom { get; set; }
        public float MarginLeft { get; set; }
        public float MarginRight { get; set; }

        public float RollMarginTop { get; set; }
        public float RollMarginBottom { get; set; }
        public float RollMarginLeft { get; set; }
        public float RollMarginRight { get; set; }

        /// <summary>0 表示自动计算。</summary>
        public int RequestedRows { get; set; }

        /// <summary>0 表示自动计算。</summary>
        public int RequestedColumns { get; set; }

        /// <summary>卷装旋转标签是否处于强制切换状态。</summary>
        public bool IsForceRotationEnabled { get; set; }

        public TemporaryImpositionParameters Clone()
        {
            return (TemporaryImpositionParameters)MemberwiseClone();
        }
    }
}
