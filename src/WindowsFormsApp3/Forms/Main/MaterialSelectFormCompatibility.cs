using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using WindowsFormsApp3.Forms.Main;
using WindowsFormsApp3.Forms.Dialogs;

namespace WindowsFormsApp3
{
    /// <summary>
    /// MaterialSelectForm兼容性适配器
    /// 将MaterialSelectFormModern包装为MaterialSelectForm的兼容接口
    /// </summary>
    public class MaterialSelectFormCompatibility
    {
        private readonly MaterialSelectFormModern _modernForm;

        // 兼容原版MaterialSelectForm的属性
        public string SelectedMaterial => _modernForm.SelectedMaterial;
        public string OrderNumber => _modernForm.OrderNumber;
        public string Quantity => _modernForm.Quantity;
        public string FixedField => _modernForm.FixedField;
        public string SelectedExportPath => _modernForm.SelectedExportPath;
        public string AdjustedDimensions => _modernForm.AdjustedDimensions;
        public double SelectedTetBleed => _modernForm.SelectedTetBleed;
        public string SerialNumber { get => _modernForm.SerialNumber; set => _modernForm.SerialNumber = value; }
        public List<string> Quantities { get => _modernForm.Quantities; set => _modernForm.Quantities = value; }
        public List<DataRow> MatchedRows => _modernForm.MatchedRows;

        // 新增属性访问
        public string ColorMode => _modernForm.ColorMode;
        public string FilmType => _modernForm.FilmType;
        public int Increment => _modernForm.Increment;
        public bool IsShapeSelected => _modernForm.GetIsShapeSelected();
        public string CornerRadius => _modernForm.SelectedShape == ShapeType.RoundRect ? _modernForm.RoundRadius.ToString() : "0";

        public MaterialSelectFormCompatibility(
            List<string> materials,
            string fileName,
            string regexResult,
            double opacity,
            string width,
            string height,
            DataTable excelData,
            int searchColumnIndex,
            int returnColumnIndex,
            int serialColumnIndex,
            int newColumnIndex,
            string serialNumber,
            List<DataRow> matchedRows = null)
        {
            _modernForm = new MaterialSelectFormModern(
                materials, fileName, regexResult, opacity, width, height,
                excelData, searchColumnIndex, returnColumnIndex, serialColumnIndex, newColumnIndex, serialNumber, matchedRows);
        }

        public DialogResult ShowDialog()
        {
            return _modernForm.ShowDialog();
        }

        public void Show()
        {
            _modernForm.Show();
        }

        public void UpdatePageHeaderSubText(string fileName)
        {
            _modernForm.UpdatePageHeaderSubText(fileName);
        }

        public void ResetPageHeaderSubText()
        {
            _modernForm.ResetPageHeaderSubText();
        }

        public Dictionary<string, object> GetFormData()
        {
            return _modernForm.GetFormData();
        }

        public bool ValidateForm()
        {
            return _modernForm.ValidateForm();
        }

        public void RefreshExportPaths()
        {
            _modernForm.RefreshExportPaths();
        }

        public bool SetExportPath(string path)
        {
            return _modernForm.SetExportPath(path);
        }

        public string GetCurrentExportPath()
        {
            return _modernForm.GetCurrentExportPath();
        }

        public List<MaterialSelectFormModern.PathItem> GetExportPathItems()
        {
            return _modernForm.GetExportPathItems();
        }

        public void Dispose()
        {
            _modernForm?.Dispose();
        }
    }
}