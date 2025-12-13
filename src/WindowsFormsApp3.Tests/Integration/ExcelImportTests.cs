using System;
using System.Data;
using System.IO;
using Xunit;
using OfficeOpenXml;
using WindowsFormsApp3.Services;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Tests.Integration
{
    public class ExcelImportTests : IDisposable
    {
        private readonly string _testDir;
        private readonly ExcelImportHelper _excelImportHelper;

        public ExcelImportTests()
        {
            // 创建测试目录
            _testDir = Path.Combine(Path.GetTempPath(), "ExcelImportTests_" + DateTime.Now.Ticks);
            Directory.CreateDirectory(_testDir);

            // 初始化Excel导入助手
            _excelImportHelper = new ExcelImportHelper();
        }

        public void Dispose()
        {
            // 清理测试目录
            if (Directory.Exists(_testDir))
            {
                try
                {
                    Directory.Delete(_testDir, true);
                }
                catch { }
            }
        }

        [Fact]
        public void ExcelImportHelper_Should_Have_Valid_ImportedData_Property()
        {
            // 验证ImportedData属性的初始状态
            Assert.Null(_excelImportHelper.ImportedData);
        }

        [Fact]
        public void ExcelImportHelper_Should_Handle_Column_Indexes()
        {
            // 验证列索引属性
            _excelImportHelper.SearchColumnIndex = 0;
            _excelImportHelper.ReturnColumnIndex = 1;
            _excelImportHelper.SerialColumnIndex = -1;
            
            Assert.Equal(0, _excelImportHelper.SearchColumnIndex);
            Assert.Equal(1, _excelImportHelper.ReturnColumnIndex);
            Assert.Equal(-1, _excelImportHelper.SerialColumnIndex);
        }

        [Fact]
        public void ExcelImportHelper_Should_Have_ImportExcelDataWrapper_Method()
        {
            // 准备测试Excel文件
            var excelFilePath = Path.Combine(_testDir, "test_data.xlsx");
            CreateTestExcelFile(excelFilePath);

            Assert.True(File.Exists(excelFilePath));

            // 测试导入Excel数据的异步方法
            var result = _excelImportHelper.ImportExcelDataWrapper(excelFilePath);

            // 验证导入结果
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        private void CreateTestExcelFile(string filePath)
        {
            using (var package = new ExcelPackage())
            {
                // 添加工作表
                var worksheet = package.Workbook.Worksheets.Add("TestSheet");
                
                // 添加标题行
                worksheet.Cells["A1"].Value = "Column1";
                worksheet.Cells["B1"].Value = "Column2";
                
                // 添加数据行
                worksheet.Cells["A2"].Value = "Item1";
                worksheet.Cells["B2"].Value = "Value1";
                worksheet.Cells["A3"].Value = "Item2";
                worksheet.Cells["B3"].Value = "Value2";
                worksheet.Cells["A4"].Value = "Item3";
                worksheet.Cells["B4"].Value = "Value3";
                
                // 保存文件
                package.SaveAs(new FileInfo(filePath));
            }
        }
    }
}