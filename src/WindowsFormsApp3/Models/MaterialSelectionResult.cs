using System.Collections.Generic;

namespace WindowsFormsApp3.Models
{
    public enum OrderNumberMode
    {
        None = 0,
        AutoIncrement = 1,
        RegexExtraction = 2
    }

    public class BatchFileItem
    {
        public int Index { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string OrderNumber { get; set; }
        public string Quantity { get; set; }
        public string SerialNumber { get; set; }
        public string RegexResult { get; set; }
        public string Dimensions { get; set; }
        public string Shape { get; set; }
    }

    public class MaterialSelectionResult
    {
        public string SelectedMaterial { get; set; }
        public string SelectedQuantity { get; set; }
        public string SelectedSerialNumber { get; set; }
        public Dictionary<string, string> ColumnValues { get; set; }
        public bool IsColumnCombineMode { get; set; }
        public string ExportPath { get; set; }
        public string OrderNumber { get; set; }
        public string Dimensions { get; set; }
        public double SelectedTetBleed { get; set; } = 0;
        public string Process { get; set; }
        public string LayoutRows { get; set; }
        public string LayoutColumns { get; set; }
        public int SelectedShape { get; set; }
        public double RoundRadius { get; set; }
        public bool IsShapeSelected { get; set; }
        public string CornerRadius { get; set; }
        public bool NeedsRotation { get; set; }
        public int RotationAngle { get; set; }
        public bool IsForceRotationEnabled { get; set; } = false;
        public bool EnableImposition { get; set; } = false;
        public LayoutMode LayoutMode { get; set; } = LayoutMode.Continuous;
        public int LayoutQuantity { get; set; } = 0;
        public string CompositeColumn { get; set; }
        public bool AddIdentifierPage { get; set; } = false;
        public string IdentifierPageContent { get; set; } = "";
        public int CopyCount { get; set; }
        public CopyMode CopyMode { get; set; }
        public CopyType CopyType { get; set; }
        public int DuplicateCount { get; set; }
        public string ImpositionMaterialType { get; set; }
        public string UpdatedRegexResult { get; set; }
        public TemporaryImpositionParameters TemporaryImpositionParameters { get; set; }

        public bool IsApplyToAll { get; set; } = false;
        public OrderNumberMode OrderNumberMode { get; set; } = OrderNumberMode.None;
        public string SelectedOrderRegexName { get; set; } = "";
        public string SelectedOrderRegexPattern { get; set; } = "";
        public List<BatchFileItem> BatchItems { get; set; } = new List<BatchFileItem>();

        public MaterialSelectionResult()
        {
            ColumnValues = new Dictionary<string, string>();
            BatchItems = new List<BatchFileItem>();
        }
    }
}
