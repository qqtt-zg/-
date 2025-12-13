using System;
using System.Text.RegularExpressions;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// 原文件名数据提取器
    /// 用于从原文件名精确提取各种字段信息，支持返单场景的数据保留
    /// </summary>
    public static class OriginalNameDataExtractor
    {
        /// <summary>
        /// 从原文件名精确提取订单号
        /// 示例：PO-2024-001-不锈钢-100pcs.pdf → PO-2024-001
        /// </summary>
        public static string ExtractOrderNumber(string originalName)
        {
            if (string.IsNullOrEmpty(originalName))
                return "";

            // 支持多种订单号格式
            var patterns = new[]
            {
                @"([A-Z]{2,}-\d{4}-\d{3})",     // PO-2024-001
                @"([A-Z]{2,}\d{6,})",          // PO2024001
                @"(\d{4}-\d{4}-\d{4})",        // 2024-1201-001
                @"([A-Z]{1,3}\d{8,})",         // P20241201001
            };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(originalName, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return match.Value;
                }
            }

            return "";
        }

        /// <summary>
        /// 从原文件名精确提取材料
        /// 示例：PO-2024-001-不锈钢-100pcs.pdf → 不锈钢
        /// </summary>
        public static string ExtractMaterial(string originalName)
        {
            if (string.IsNullOrEmpty(originalName))
                return "";

            // 常见材料类型
            var materialPattern = @"(不锈钢|铝合金|PVC|PE|ABS|碳钢|铜|铝板|铁板|镀锌板|马口铁|锌合金|镁合金|钛合金|尼龙|亚克力|有机玻璃|PC|PP|PS|PET|POM|PTFE)";
            var match = Regex.Match(originalName, materialPattern);
            return match.Success ? match.Value : "";
        }

        /// <summary>
        /// 从原文件名精确提取工艺
        /// 示例：PO-2024-001-不锈钢-激光切割-100pcs.pdf → 激光切割
        /// </summary>
        public static string ExtractProcess(string originalName)
        {
            if (string.IsNullOrEmpty(originalName))
                return "";

            // 常见工艺类型
            var processPattern = @"(激光切割|冲压|折弯|焊接|喷涂|电镀|抛光|磨光|拉丝|阳极氧化|丝印|热转印|UV打印| CNC|数控|铣削|车削|磨削|钻孔|攻丝|钣金|锻造|铸造|注塑|挤出|吹塑|吸塑|压铸|热处理|退火|淬火|回火|调质|渗碳|氮化|镀铬|镀镍|镀锌|喷砂|阳极|钝化|清洗|除油|防锈|包装|装配)";
            var match = Regex.Match(originalName, processPattern);
            return match.Success ? match.Value : "";
        }

        /// <summary>
        /// 从原文件名精确提取数量
        /// 示例：PO-2024-001-不锈钢-100pcs.pdf → 100
        /// </summary>
        public static string ExtractQuantity(string originalName)
        {
            if (string.IsNullOrEmpty(originalName))
                return "";

            // 数量模式，支持多种单位
            var quantityPattern = @"(\d+)(?:pcs|个|件|张|片|块|条|根|支|套|组|对|双|箱|包|袋|瓶|桶|罐|盒|盘|卷|米|厘米|毫米|公斤|克|吨|升|毫升|平方米|平方厘米|平方毫米)";
            var match = Regex.Match(originalName, quantityPattern, RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : "";
        }

        /// <summary>
        /// 从原文件名精确提取行数
        /// </summary>
        public static string ExtractRowCount(string originalName)
        {
            if (string.IsNullOrEmpty(originalName))
                return "";

            var patterns = new[]
            {
                @"(?:Row|行)[_-]?(\d+)",
                @"(\d+)行",
                @"R(\d+)",
                @"行数[=_-]?(\d+)"
            };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(originalName, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }

            return "";
        }

        /// <summary>
        /// 从原文件名精确提取列数
        /// </summary>
        public static string ExtractColumnCount(string originalName)
        {
            if (string.IsNullOrEmpty(originalName))
                return "";

            var patterns = new[]
            {
                @"(?:Col|列)[_-]?(\d+)",
                @"(\d+)列",
                @"C(\d+)",
                @"列数[=_-]?(\d+)"
            };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(originalName, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }

            return "";
        }

        /// <summary>
        /// 从原文件名精确提取客户信息
        /// </summary>
        public static string ExtractCustomer(string originalName)
        {
            if (string.IsNullOrEmpty(originalName))
                return "";

            // 常见客户标识模式
            var customerPatterns = new[]
            {
                @"(华为|Apple|三星|小米|OPPO|vivo|联想|戴尔|惠普|佳能|尼康|索尼|松下|LG|微软|谷歌|亚马逊|阿里|腾讯|百度|京东|美团|滴滴|字节跳动)",
                @"([A-Z]{2,}Customer)",
                @"(客户)[_-]?([A-Za-z0-9]+)",
                @"(Client)[_-]?([A-Za-z0-9]+)"
            };

            foreach (var pattern in customerPatterns)
            {
                var match = Regex.Match(originalName, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return match.Value;
                }
            }

            return "";
        }

        /// <summary>
        /// 从原文件名精确提取备注信息
        /// </summary>
        public static string ExtractRemark(string originalName)
        {
            if (string.IsNullOrEmpty(originalName))
                return "";

            // 备注模式 - 通常在文件名末尾
            var remarkPatterns = new[]
            {
                @"(?:备注|Remark|Note|Note)[_-]?([A-Za-z0-9\u4e00-\u9fa5]+)",
                @"(?:紧急|Urgent|加急)",
                @"(?:版本|Version|Ver)[_-]?([vV]?\d+(?:\.\d+)?)",
                @"(?:修订|Rev|Revision)[_-]?(\d+)"
            };

            foreach (var pattern in remarkPatterns)
            {
                var match = Regex.Match(originalName, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return match.Value;
                }
            }

            return "";
        }

        /// <summary>
        /// 通用字段提取方法
        /// </summary>
        /// <param name="originalName">原文件名</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns>提取的值</returns>
        public static string ExtractField(string originalName, string fieldName)
        {
            if (string.IsNullOrEmpty(originalName) || string.IsNullOrEmpty(fieldName))
                return "";

            return fieldName switch
            {
                "订单号" => ExtractOrderNumber(originalName),
                "材料" => ExtractMaterial(originalName),
                "工艺" => ExtractProcess(originalName),
                "数量" => ExtractQuantity(originalName),
                "行数" => ExtractRowCount(originalName),
                "列数" => ExtractColumnCount(originalName),
                "客户" => ExtractCustomer(originalName),
                "备注" => ExtractRemark(originalName),
                _ => ""
            };
        }

        /// <summary>
        /// 批量提取所有支持的字段
        /// </summary>
        /// <param name="originalName">原文件名</param>
        /// <returns>字段名-值的字典</returns>
        public static System.Collections.Generic.Dictionary<string, string> ExtractAllFields(string originalName)
        {
            var result = new System.Collections.Generic.Dictionary<string, string>();

            var fieldNames = new[] { "订单号", "材料", "工艺", "数量", "行数", "列数", "客户", "备注" };

            foreach (var fieldName in fieldNames)
            {
                var value = ExtractField(originalName, fieldName);
                if (!string.IsNullOrEmpty(value))
                {
                    result[fieldName] = value;
                }
            }

            return result;
        }
    }
}