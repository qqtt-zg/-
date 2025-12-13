using System.Collections.Generic;
using System.Data;

namespace WindowsFormsApp3.Interfaces
{
    /// <summary>
    /// 列组合服务接口，定义列组合相关的操作
    /// </summary>
    public interface ICompositeColumnService
    {
        /// <summary>
        /// 获取列组合值
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="selectedColumns">选中的列名列表</param>
        /// <param name="separator">分隔符</param>
        /// <returns>组合后的列值</returns>
        string GetCompositeColumnValue(DataRow row, List<string> selectedColumns, string separator);
        
        /// <summary>
        /// 为数据表格添加列组合列
        /// </summary>
        /// <param name="dataTable">原始数据表格</param>
        /// <param name="selectedColumns">选中的列名列表</param>
        /// <param name="separator">分隔符</param>
        /// <returns>包含列组合列的新数据表格</returns>
        DataTable AddCompositeColumnToDataTable(DataTable dataTable, List<string> selectedColumns, string separator);
        
        /// <summary>
        /// 保存列组合设置
        /// </summary>
        /// <param name="selectedColumns">选中的列名列表</param>
        /// <param name="separator">分隔符</param>
        void SaveCompositeColumnSettings(List<string> selectedColumns, string separator);
        
        /// <summary>
        /// 加载列组合设置
        /// </summary>
        /// <returns>包含选中列和分隔符的元组</returns>
        (List<string> selectedColumns, string separator) LoadCompositeColumnSettings();
        
        /// <summary>
        /// 获取组合列设置
        /// </summary>
        /// <returns>包含选中列和分隔符的元组</returns>
        (List<string> selectedColumns, string separator) GetCompositeColumnSettings();
        
        /// <summary>
        /// 为指定行获取组合列值（使用服务中保存的设置）
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>组合后的列值</returns>
        string GetCompositeColumnValueForRow(DataRow row);
        
        /// <summary>
        /// 检查列组合功能是否启用
        /// </summary>
        /// <returns>是否启用</returns>
        bool IsCompositeColumnFeatureEnabled();
        
        /// <summary>
        /// 启用或禁用列组合功能
        /// </summary>
        /// <param name="enabled">是否启用</param>
        void SetCompositeColumnFeatureEnabled(bool enabled);
    }
}