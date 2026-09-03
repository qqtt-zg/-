using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsFormsApp3.Models;
using Xunit;

namespace WindowsFormsApp3.Tests.Forms
{
    public class MaterialSelectionSchemeATests
    {
        [Fact]
        public void ExtractOrderNumberByRegex_ShouldMatchFullFileName_WithExtension_ConsistentWithMainShell()
        {
            string fileName = "203X203.pdf";
            string pattern = @"(.+).pdf";

            string result = MaterialSelectFormModern.ExtractOrderNumberByRegex(fileName, pattern);

            Assert.Equal("203X203", result);
        }

        [Fact]
        public void ExtractOrderNumberByRegex_ShouldExtractCorrectly_WithNamedGroup()
        {
            string fileName = "PO123456_Label_54x84-1000pcs.pdf";
            string pattern = @"(?<order>PO\d+)";

            string result = MaterialSelectFormModern.ExtractOrderNumberByRegex(fileName, pattern);

            Assert.Equal("PO123456", result);
        }

        [Fact]
        public void ExtractOrderNumberByRegex_ShouldExtractCorrectly_WithStandardGroup()
        {
            string fileName = "CustomJob_ORDER-2026-08_500pcs.pdf";
            string pattern = @"(ORDER-\d{4}-\d{2})";

            string result = MaterialSelectFormModern.ExtractOrderNumberByRegex(fileName, pattern);

            Assert.Equal("ORDER-2026-08", result);
        }

        [Fact]
        public void ExtractOrderNumberByRegex_ShouldExtractCorrectly_WithFullMatch()
        {
            string fileName = "Batch_987654321_Glossy.pdf";
            string pattern = @"\d{6,}";

            string result = MaterialSelectFormModern.ExtractOrderNumberByRegex(fileName, pattern);

            Assert.Equal("987654321", result);
        }

        [Theory]
        [InlineData("PO001", 0, "PO001")]
        [InlineData("PO001", 1, "PO002")]
        [InlineData("PO001", 9, "PO010")]
        [InlineData("PO099", 1, "PO100")]
        [InlineData("ORDER_0005", 3, "ORDER_0008")]
        [InlineData("20260901-001", 5, "20260901-006")]
        public void CalculateIncrementalOrderNumber_ShouldPreserveLeadingZeros(string baseOrder, int offset, string expected)
        {
            string result = MaterialSelectFormModern.CalculateIncrementalOrderNumber(baseOrder, offset);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BatchFileSorting_AscendingAndDescending_ShouldWorkProperly()
        {
            var files = new List<string>
            {
                @"C:\test\Z_File_500pcs.pdf",
                @"C:\test\A_File_100pcs.pdf",
                @"C:\test\M_File_200pcs.pdf"
            };

            var items = files.Select((f, idx) => new BatchFileItem
            {
                Index = idx + 1,
                FilePath = f,
                FileName = System.IO.Path.GetFileName(f)
            }).ToList();

            // A-Z 升序
            var ascList = items.OrderBy(x => x.FileName, StringComparer.OrdinalIgnoreCase).ToList();
            Assert.Equal("A_File_100pcs.pdf", ascList[0].FileName);
            Assert.Equal("M_File_200pcs.pdf", ascList[1].FileName);
            Assert.Equal("Z_File_500pcs.pdf", ascList[2].FileName);

            // Z-A 降序
            var descList = items.OrderByDescending(x => x.FileName, StringComparer.OrdinalIgnoreCase).ToList();
            Assert.Equal("Z_File_500pcs.pdf", descList[0].FileName);
            Assert.Equal("M_File_200pcs.pdf", descList[1].FileName);
            Assert.Equal("A_File_100pcs.pdf", descList[2].FileName);
        }

        [Fact]
        public void BatchQuantityParsing_FromClipboard_ShouldExtractNumbers()
        {
            string clipboardText = "1000\r\n2000 pcs\r\n数量: 500\r\n800";
            var lines = clipboardText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var quantities = lines.Select(l =>
            {
                var match = Regex.Match(l.Trim(), @"\d+");
                return match.Success ? match.Value : l.Trim();
            }).ToList();

            Assert.Equal(4, quantities.Count);
            Assert.Equal("1000", quantities[0]);
            Assert.Equal("2000", quantities[1]);
            Assert.Equal("500", quantities[2]);
            Assert.Equal("800", quantities[3]);
        }

        [Fact]
        public void MaterialSelectionResult_ShouldContainSchemeAProperties()
        {
            var result = new MaterialSelectionResult
            {
                IsApplyToAll = true,
                OrderNumberMode = OrderNumberMode.RegexExtraction,
                SelectedOrderRegexName = "订单号_PO数字",
                SelectedOrderRegexPattern = @"PO\d+",
                BatchItems = new List<BatchFileItem>
                {
                    new BatchFileItem { Index = 1, FileName = "PO1001_54x84-100pcs.pdf", OrderNumber = "PO1001", Quantity = "100" },
                    new BatchFileItem { Index = 2, FileName = "PO1002_54x84-200pcs.pdf", OrderNumber = "PO1002", Quantity = "200" }
                }
            };

            Assert.True(result.IsApplyToAll);
            Assert.Equal(OrderNumberMode.RegexExtraction, result.OrderNumberMode);
            Assert.Equal("订单号_PO数字", result.SelectedOrderRegexName);
            Assert.Equal(2, result.BatchItems.Count);
            Assert.Equal("PO1001", result.BatchItems[0].OrderNumber);
            Assert.Equal("100", result.BatchItems[0].Quantity);
        }

        [Fact]
        public void AppendPendingFiles_ShouldDynamicallyAddFilesAndAvoidDuplicates()
        {
            using (var form = new MaterialSelectFormModern(
                materials: new List<string> { "PET", "PP" },
                fileName: @"C:\test\FirstFile_54x84-100pcs.pdf",
                regexResult: "FirstFile",
                opacity: 1.0,
                width: "54",
                height: "84",
                excelData: null,
                searchColumnIndex: -1,
                returnColumnIndex: -1,
                serialColumnIndex: -1,
                newColumnIndex: -1,
                serialNumber: "1"))
            {
                var handle = form.Handle;

                // 初始设置待处理文件列表
                form.SetPendingFiles(new[] { @"C:\test\FirstFile_54x84-100pcs.pdf" });
                Assert.Single(form.BatchFileItems);

                // 动态追加新进入监控目录的文件
                form.AppendPendingFile(@"C:\test\SecondFile_54x84-200pcs.pdf");
                form.AppendPendingFile(@"C:\test\ThirdFile_54x84-500pcs.pdf");

                Assert.Equal(3, form.BatchFileItems.Count);
                Assert.Equal("SecondFile_54x84-200pcs.pdf", form.BatchFileItems[1].FileName);
                Assert.Equal("200", form.BatchFileItems[1].Quantity);
                Assert.Equal("2", form.BatchFileItems[1].SerialNumber);

                Assert.Equal("ThirdFile_54x84-500pcs.pdf", form.BatchFileItems[2].FileName);
                Assert.Equal("500", form.BatchFileItems[2].Quantity);
                Assert.Equal("3", form.BatchFileItems[2].SerialNumber);

                // 尝试重复追加已存在的文件，应自动去重
                form.AppendPendingFile(@"C:\test\SecondFile_54x84-200pcs.pdf");
                Assert.Equal(3, form.BatchFileItems.Count);
            }
        }

        [Fact]
        public void MoveBatchItem_ShouldReorderItems_AndUpdateIndexAndOrderNumbers()
        {
            using (var form = new MaterialSelectFormModern(
                materials: new List<string> { "PET" },
                fileName: @"C:\test\File1.pdf",
                regexResult: "File1",
                opacity: 1.0,
                width: "54",
                height: "84",
                excelData: null,
                searchColumnIndex: -1,
                returnColumnIndex: -1,
                serialColumnIndex: -1,
                newColumnIndex: -1,
                serialNumber: "1"))
            {
                var handle = form.Handle;

                form.SetPendingFiles(new[] {
                    @"C:\test\FileA_100pcs.pdf",
                    @"C:\test\FileB_200pcs.pdf",
                    @"C:\test\FileC_300pcs.pdf"
                });

                Assert.Equal(3, form.BatchFileItems.Count);
                Assert.Equal("FileA_100pcs.pdf", form.BatchFileItems[0].FileName);
                Assert.Equal("FileB_200pcs.pdf", form.BatchFileItems[1].FileName);
                Assert.Equal("FileC_300pcs.pdf", form.BatchFileItems[2].FileName);

                // 模拟拖拽：将第 0 项移动到第 2 项（移到末尾）
                form.MoveBatchItem(0, 2);

                Assert.Equal("FileB_200pcs.pdf", form.BatchFileItems[0].FileName);
                Assert.Equal(1, form.BatchFileItems[0].Index);
                Assert.Equal("FileC_300pcs.pdf", form.BatchFileItems[1].FileName);
                Assert.Equal(2, form.BatchFileItems[1].Index);
                Assert.Equal("FileA_100pcs.pdf", form.BatchFileItems[2].FileName);
                Assert.Equal(3, form.BatchFileItems[2].Index);
            }
        }
    }
}
