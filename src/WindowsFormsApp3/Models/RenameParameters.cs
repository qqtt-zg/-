using System.IO;

namespace WindowsFormsApp3.Models
{
    /// <summary>
    /// 文件重命名参数数据传输对象
    /// 用于封装RenameFileImmediately方法的所有输入参数
    /// </summary>
    public class RenameParameters
    {
        /// <summary>
        /// 待重命名的文件信息
        /// </summary>
        public FileInfo FileInfo { get; set; }

        /// <summary>
        /// 选中的材料
        /// </summary>
        public string SelectedMaterial { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string Quantity { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 导出路径
        /// </summary>
        public string ExportPath { get; set; }

        /// <summary>
        /// 出血值
        /// </summary>
        public double TetBleed { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// 固定字段（工艺）
        /// </summary>
        public string FixedField { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 圆角半径，默认为"0"
        /// </summary>
        public string CornerRadius { get; set; }

        /// <summary>
        /// 是否使用PDF最后一页，默认为false
        /// </summary>
        public bool UsePdfLastPage { get; set; }

        /// <summary>
        /// 是否添加PDF图层，默认为true
        /// </summary>
        public bool AddPdfLayers { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RenameParameters()
        {
            SelectedMaterial = string.Empty;
            OrderNumber = string.Empty;
            Quantity = string.Empty;
            Unit = string.Empty;
            ExportPath = string.Empty;
            TetBleed = 0.0;
            Width = string.Empty;
            Height = string.Empty;
            FixedField = string.Empty;
            SerialNumber = string.Empty;
            CornerRadius = "0";
            UsePdfLastPage = false;
            AddPdfLayers = true;
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public RenameParameters(FileInfo fileInfo, string selectedMaterial, string orderNumber, 
            string quantity, string unit, string exportPath, double tetBleed, string width, 
            string height, string fixedField, string serialNumber, string cornerRadius = "0", 
            bool usePdfLastPage = false, bool addPdfLayers = true)
        {
            FileInfo = fileInfo;
            SelectedMaterial = selectedMaterial ?? string.Empty;
            OrderNumber = orderNumber ?? string.Empty;
            Quantity = quantity ?? string.Empty;
            Unit = unit ?? string.Empty;
            ExportPath = exportPath ?? string.Empty;
            TetBleed = tetBleed;
            Width = width ?? string.Empty;
            Height = height ?? string.Empty;
            FixedField = fixedField ?? string.Empty;
            SerialNumber = serialNumber ?? string.Empty;
            CornerRadius = cornerRadius ?? "0";
            UsePdfLastPage = usePdfLastPage;
            AddPdfLayers = addPdfLayers;
        }

        /// <summary>
        /// 验证参数有效性
        /// </summary>
        /// <returns>验证结果</returns>
        public ValidationResult Validate()
        {
            if (FileInfo == null)
            {
                return ValidationResult.Failure("文件信息不能为空", ValidationErrorType.NullReference);
            }

            if (!FileInfo.Exists)
            {
                return ValidationResult.Failure("指定的文件不存在", ValidationErrorType.FileNotFound);
            }

            if (string.IsNullOrEmpty(ExportPath))
            {
                return ValidationResult.Failure("导出路径不能为空", ValidationErrorType.InvalidParameters);
            }

            if (!Directory.Exists(ExportPath))
            {
                return ValidationResult.Failure("导出目录不存在", ValidationErrorType.DirectoryNotFound);
            }

            if (TetBleed < 0)
            {
                return ValidationResult.Failure("出血值不能为负数", ValidationErrorType.InvalidParameters);
            }

            return ValidationResult.Success("参数验证通过");
        }

        /// <summary>
        /// 获取调试信息
        /// </summary>
        /// <returns>调试信息字符串</returns>
        public string GetDebugInfo()
        {
            return $"RenameParams: File='{FileInfo?.Name}', Material='{SelectedMaterial}', Order='{OrderNumber}', " +
                   $"Quantity='{Quantity}{Unit}', Export='{ExportPath}', Bleed={TetBleed}, " +
                   $"Size='{Width}x{Height}', Fixed='{FixedField}', Serial='{SerialNumber}', " +
                   $"CornerRadius='{CornerRadius}', LastPage={UsePdfLastPage}, AddLayers={AddPdfLayers}";
        }
    }
}