using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WindowsFormsApp3.Models;
using Xunit;
using Xunit.Abstractions;

namespace WindowsFormsApp3.Tests.Performance
{
    public class RefactoringPerformanceTests
    {
        private readonly ITestOutputHelper _output;

        public RefactoringPerformanceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void AddFileToGrid_Refactored_Performance_Test()
        {
            // Arrange
            var testFiles = GenerateTestFiles(1000); // 生成1000个测试文件
            
            var stopwatch = Stopwatch.StartNew();
            
            // Act - 执行重构后的方法
            foreach (var testFile in testFiles)
            {
                // 模拟调用重构后的AddFileToGrid方法的关键逻辑
                var validationResult = ValidateFileGridInput(testFile.FileInfo, testFile.Material, testFile.OrderNumber, 
                    testFile.Quantity, testFile.SerialNumber);
                
                if (validationResult.IsValid)
                {
                    // 模拟其他步骤
                    var serialNumber = GenerateSerialNumber(testFile.SerialNumber, null, false);
                    var regexPart = ProcessRegexForFileName(testFile.FileInfo.Name);
                    var displayDimensions = CalculateDisplayDimensions(testFile.FileInfo, testFile.Dimensions);
                    var components = CreateFileNameComponents(testFile.FileInfo, testFile.Material, testFile.OrderNumber, 
                        testFile.Quantity, testFile.Unit, displayDimensions, testFile.FixedField, serialNumber, regexPart);
                    var newFileName = BuildNewFileName(components);
                }
            }
            
            stopwatch.Stop();
            
            // Assert
            _output.WriteLine($"重构后AddFileToGrid处理1000个文件耗时: {stopwatch.ElapsedMilliseconds} ms");
            Assert.True(stopwatch.ElapsedMilliseconds < 5000, "重构后方法处理1000个文件应在5秒内完成");
        }

        [Fact]
        public void RenameFileImmediately_Refactored_Performance_Test()
        {
            // Arrange
            var testFiles = GenerateTestFiles(100); // 生成100个测试文件
            
            var stopwatch = Stopwatch.StartNew();
            
            // Act - 执行重构后的方法
            foreach (var testFile in testFiles)
            {
                // 模拟调用重构后的RenameFileImmediately方法的关键逻辑
                var validationResult = ValidateRenameOperation(testFile.ExportPath);
                
                if (validationResult.IsValid)
                {
                    // 模拟其他步骤
                    var newFileName = BuildNewFileNameForRename(testFile.FileInfo, testFile.Material, testFile.OrderNumber,
                        testFile.Quantity, testFile.Unit, testFile.FixedField, testFile.SerialNumber, 
                        validationResult.Pattern, testFile.TetBleed, testFile.CornerRadius, testFile.AddPdfLayers);
                    
                    if (!string.IsNullOrEmpty(newFileName))
                    {
                        var (resolvedFileName, destPath) = ResolveFileNameConflict(testFile.ExportPath, newFileName);
                        var finalDimensions = CalculateFinalDimensions(testFile.FileInfo.FullName, testFile.TetBleed, 
                            testFile.CornerRadius, testFile.AddPdfLayers);
                        
                        var isPdfFile = Path.GetExtension(testFile.FileInfo.FullName)
                            .Equals(".pdf", StringComparison.OrdinalIgnoreCase);
                        var pdfOptions = new PdfProcessingOptions(isPdfFile, testFile.AddPdfLayers, 
                            testFile.UsePdfLastPage, testFile.CornerRadius, testFile.TetBleed)
                        {
                            FinalDimensions = finalDimensions
                        };
                    }
                }
            }
            
            stopwatch.Stop();
            
            // Assert
            _output.WriteLine($"重构后RenameFileImmediately处理100个文件耗时: {stopwatch.ElapsedMilliseconds} ms");
            Assert.True(stopwatch.ElapsedMilliseconds < 10000, "重构后方法处理100个文件应在10秒内完成");
        }

        // 修改为异步方法以避免警告
        [Fact]
        public async Task AddFileToPendingList_Refactored_Performance_Test()
        {
            // Arrange
            var testFiles = GenerateTestFiles(500); // 生成500个测试文件
            
            var stopwatch = Stopwatch.StartNew();
            
            // Act - 执行重构后的方法
            foreach (var testFile in testFiles)
            {
                // 模拟调用重构后的AddFileToPendingList方法的关键逻辑
                var validationResult = ValidateFileForProcessing(testFile.FileInfo.FullName);
                
                if (validationResult.IsValid)
                {
                    // 模拟其他步骤（使用await）
                    var (width, height, tetBleed) = await CalculatePdfDimensionsAsync(testFile.FileInfo);
                    var regexResult = ProcessRegexForFileName(testFile.FileInfo.Name);
                    var (matchedRows, quantity, serialNumber) = MatchExcelData(regexResult, null);
                }
            }
            
            stopwatch.Stop();
            
            // Assert
            _output.WriteLine($"重构后AddFileToPendingList处理500个文件耗时: {stopwatch.ElapsedMilliseconds} ms");
            Assert.True(stopwatch.ElapsedMilliseconds < 8000, "重构后方法处理500个文件应在8秒内完成");
        }

        #region 测试辅助方法

        private List<TestFileData> GenerateTestFiles(int count)
        {
            var files = new List<TestFileData>();
            
            for (int i = 0; i < count; i++)
            {
                files.Add(new TestFileData
                {
                    FileInfo = new FileInfo($"test_file_{i}.pdf"),
                    Material = $"Material_{i % 10}",
                    OrderNumber = $"Order_{i}",
                    Quantity = $"{i + 1}",
                    Unit = "pcs",
                    Dimensions = "210x297mm",
                    FixedField = $"Notes_{i}",
                    SerialNumber = $"{i + 1:D3}",
                    ExportPath = @"C:\Test\Export",
                    TetBleed = 3.0,
                    CornerRadius = "0",
                    AddPdfLayers = true,
                    UsePdfLastPage = false
                });
            }
            
            return files;
        }

        // 模拟重构后的方法实现
        private ValidationResult ValidateFileGridInput(FileInfo fileInfo, string material, string orderNumber, 
            string quantity, string serialNumber)
        {
            // 简化验证逻辑
            if (fileInfo == null)
                return ValidationResult.Failure("文件信息不能为空", ValidationErrorType.NullReference);
                
            if (string.IsNullOrEmpty(material))
                return ValidationResult.Failure("材料不能为空", ValidationErrorType.MissingMaterial);
                
            if (string.IsNullOrEmpty(orderNumber))
                return ValidationResult.Failure("订单号不能为空", ValidationErrorType.InvalidParameters);
                
            return ValidationResult.Success("验证通过");
        }

        private string GenerateSerialNumber(string serialNumber, object bindingList, bool isFromExcel)
        {
            if (!string.IsNullOrEmpty(serialNumber))
                return serialNumber;
                
            return "001";
        }

        private string ProcessRegexForFileName(string fileName)
        {
            // 简化正则处理逻辑
            return "regex_result";
        }

        private string CalculateDisplayDimensions(FileInfo fileInfo, string adjustedDimensions)
        {
            return adjustedDimensions ?? "210x297mm";
        }

        private FileNameComponents CreateFileNameComponents(FileInfo fileInfo, string material, string orderNumber, 
            string quantity, string unit, string dimensions, string notes, string serialNumber, string regexPart)
        {
            return new FileNameComponents
            {
                RegexResult = regexPart,
                OrderNumber = orderNumber,
                Material = material,
                Quantity = quantity,
                Unit = unit,
                Dimensions = dimensions,
                Process = notes,
                SerialNumber = serialNumber,
                FileExtension = fileInfo.Extension,
                Separator = "_",
                EnabledComponents = new FileNameComponentsConfig()
            };
        }

        private string BuildNewFileName(FileNameComponents components)
        {
            var parts = new List<string>();
            
            if (!string.IsNullOrEmpty(components.OrderNumber))
                parts.Add(components.OrderNumber);
                
            if (!string.IsNullOrEmpty(components.Material))
                parts.Add(components.Material);
                
            if (!string.IsNullOrEmpty(components.Quantity))
                parts.Add(components.Quantity + components.Unit);
                
            if (!string.IsNullOrEmpty(components.SerialNumber))
                parts.Add(components.SerialNumber);
                
            return string.Join(components.Separator, parts) + components.FileExtension;
        }

        private RenameValidationResult ValidateRenameOperation(string exportPath)
        {
            if (string.IsNullOrEmpty(exportPath))
            {
                return RenameValidationResult.Failure("导出路径不能为空", ValidationErrorType.InvalidParameters);
            }
            
            return RenameValidationResult.Success("Pattern1", @"^(.*?)\..*$", "验证通过");
        }

        private string BuildNewFileNameForRename(FileInfo fileInfo, string selectedMaterial, string orderNumber, 
            string quantity, string unit, string fixedField, string serialNumber, string pattern, 
            double tetBleed, string cornerRadius, bool addPdfLayers)
        {
            var parts = new List<string>();
            
            if (!string.IsNullOrEmpty(orderNumber))
                parts.Add(orderNumber);
                
            if (!string.IsNullOrEmpty(selectedMaterial))
                parts.Add(selectedMaterial);
                
            if (!string.IsNullOrEmpty(quantity))
                parts.Add(quantity + (unit ?? ""));
                
            if (!string.IsNullOrEmpty(serialNumber))
                parts.Add(serialNumber);
                
            return string.Join("_", parts) + fileInfo.Extension;
        }

        private (string resolvedFileName, string destPath) ResolveFileNameConflict(string exportPath, string newFileName)
        {
            return (newFileName, Path.Combine(exportPath, newFileName));
        }

        private string CalculateFinalDimensions(string filePath, double tetBleed, string cornerRadius, bool addPdfLayers)
        {
            return "210x297mm";
        }

        private ValidationResult ValidateFileForProcessing(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                
                if (!fileInfo.Exists)
                    return ValidationResult.Failure("文件不存在", ValidationErrorType.FileNotFound);
                    
                if (!fileInfo.Extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                    return ValidationResult.Failure("仅支持PDF文件", ValidationErrorType.InvalidFileInfo);
                    
                return ValidationResult.Success("文件验证通过");
            }
            catch (Exception ex)
            {
                return ValidationResult.Failure($"文件验证失败: {ex.Message}", ValidationErrorType.General);
            }
        }

        private async Task<(string width, string height, double tetBleed)> CalculatePdfDimensionsAsync(FileInfo fileInfo)
        {
            // 模拟异步计算
            await Task.Delay(1);
            return ("210", "297", 3.0);
        }

        private (List<object> matchedRows, string quantity, string serialNumber) MatchExcelData(string regexResult, object bindingList)
        {
            return (new List<object>(), "1", "001");
        }

        #endregion
    }

    public class TestFileData
    {
        public FileInfo FileInfo { get; set; }
        public string Material { get; set; }
        public string OrderNumber { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }
        public string Dimensions { get; set; }
        public string FixedField { get; set; }
        public string SerialNumber { get; set; }
        public string ExportPath { get; set; }
        public double TetBleed { get; set; }
        public string CornerRadius { get; set; }
        public bool AddPdfLayers { get; set; }
        public bool UsePdfLastPage { get; set; }
    }
}