using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using iText.Kernel.Font;
using iText.IO.Font;
using iText.IO.Font.Constants;
using WindowsFormsApp3.Services;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// 字体管理器 - 处理嵌入字体和字体缓存
    /// </summary>
    public static class FontManager
    {
        private static readonly Dictionary<string, PdfFont> _fontCache = new Dictionary<string, PdfFont>();
        private static readonly object _lockObject = new object();
        private static bool _initialized = false;

        // 嵌入字体文件列表
        private static readonly Dictionary<string, string> _embeddedFonts = new Dictionary<string, string>
        {
            { "simhei", "WindowsFormsApp3.Resources.Fonts.simhei.ttf" },
            { "simsun", "WindowsFormsApp3.Resources.Fonts.simsun.ttc" },
            { "msyh", "WindowsFormsApp3.Resources.Fonts.msyh.ttc" },

            { "noto_sans", "WindowsFormsApp3.Resources.Fonts.NotoSansSC-Regular.ttf" }
        };

        /// <summary>
        /// 初始化字体管理器
        /// </summary>
        public static void Initialize()
        {
            if (_initialized) return;

            lock (_lockObject)
            {
                if (_initialized) return;

                try
                {
                    LogHelper.Info("开始初始化字体管理器");

                    // 预加载常用字体
                    PreloadFont("simhei");
                    PreloadFont("simsun");
                    PreloadFont("msyh");

                    PreloadFont("noto_sans");

                    _initialized = true;
                    LogHelper.Info("字体管理器初始化完成");
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"字体管理器初始化失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 获取中文字体
        /// </summary>
        /// <param name="fontName">字体名称</param>
        /// <returns>PdfFont对象</returns>
        public static PdfFont GetChineseFont(string fontName = "simhei")
        {
            lock (_lockObject)
            {
                if (_fontCache.ContainsKey(fontName) && _fontCache[fontName] != null)
                {
                    return _fontCache[fontName];
                }

                return PreloadFont(fontName);
            }
        }

        /// <summary>
        /// 预加载字体
        /// </summary>
        /// <param name="fontName">字体名称</param>
        /// <returns>PdfFont对象</returns>
        private static PdfFont PreloadFont(string fontName)
        {
            try
            {
                if (_fontCache.ContainsKey(fontName))
                {
                    return _fontCache[fontName];
                }

                // 尝试从嵌入资源加载
                PdfFont font = LoadEmbeddedFont(fontName);
                if (font != null)
                {
                    _fontCache[fontName] = font;
                    LogHelper.Debug($"成功加载嵌入字体: {fontName}");
                    return font;
                }

                // 尝试从系统字体目录加载
                font = LoadSystemFont(fontName);
                if (font != null)
                {
                    _fontCache[fontName] = font;
                    LogHelper.Debug($"成功加载系统字体: {fontName}");
                    return font;
                }

                // 使用默认字体作为回退
                LogHelper.Warn($"字体 {fontName} 加载失败，使用默认字体");
                font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                _fontCache[fontName] = font;
                return font;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"加载字体 {fontName} 失败: {ex.Message}");
                var fallbackFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                _fontCache[fontName] = fallbackFont;
                return fallbackFont;
            }
        }

        /// <summary>
        /// 从嵌入资源加载字体
        /// </summary>
        /// <param name="fontName">字体名称</param>
        /// <returns>PdfFont对象</returns>
        private static PdfFont LoadEmbeddedFont(string fontName)
        {
            try
            {
                if (!_embeddedFonts.ContainsKey(fontName))
                {
                    return null;
                }

                string resourceName = _embeddedFonts[fontName];
                Assembly assembly = Assembly.GetExecutingAssembly();

                using (Stream fontStream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (fontStream != null)
                    {
                        byte[] fontBytes = new byte[fontStream.Length];
                        fontStream.Read(fontBytes, 0, fontBytes.Length);

                        // 对于TTC字体集合，需要特殊处理
                        if (resourceName.EndsWith(".ttc", StringComparison.OrdinalIgnoreCase))
                        {
                            // 尝试使用TTC字体的第一个字体，避免使用IDENTITY_H防止触发CJK
                            return PdfFontFactory.CreateFont(fontBytes, PdfEncodings.WINANSI);
                        }
                        else
                        {
                            // 使用 WINANSI 编码，避免触发 CJK 字体系统
                            return PdfFontFactory.CreateFont(fontBytes, PdfEncodings.WINANSI);
                        }
                    }
                }
            }
            catch (System.NullReferenceException nullEx) when (nullEx.Message.Contains("CjkResourceLoader") || nullEx.Source == "itext.io")
            {
                LogHelper.Debug($"嵌入字体 {fontName} 触发 CJK 异常，跳过加载");
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.Debug($"从嵌入资源加载字体 {fontName} 失败: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// 从系统字体目录加载字体
        /// </summary>
        /// <param name="fontName">字体名称</param>
        /// <returns>PdfFont对象</returns>
        private static PdfFont LoadSystemFont(string fontName)
        {
            try
            {
                string[] fontPaths = GetSystemFontPaths(fontName);

                foreach (string fontPath in fontPaths)
                {
                    if (File.Exists(fontPath))
                    {
                        try
                        {
                            // 避免使用 IDENTITY_H 编码，防止触发 CjkResourceLoader
                            // 使用 WinAnsiEncoding 作为替代，对于中文字符可能不完全支持但能避免异常
                            if (fontPath.EndsWith(".ttc", StringComparison.OrdinalIgnoreCase))
                            {
                                // 尝试直接加载TTC文件，使用WinAnsi编码避免CJK系统
                                return PdfFontFactory.CreateFont(fontPath, PdfEncodings.WINANSI);
                            }
                            else
                            {
                                return PdfFontFactory.CreateFont(fontPath, PdfEncodings.WINANSI);
                            }
                        }
                        catch (System.NullReferenceException nullEx) when (nullEx.Message.Contains("CjkResourceLoader") || nullEx.Source == "itext.io")
                        {
                            LogHelper.Debug($"字体 {fontPath} 触发 CJK 异常，跳过加载");
                            continue; // 跳过这个字体文件，尝试下一个
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Debug($"加载字体文件 {fontPath} 失败: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Debug($"从系统目录加载字体 {fontName} 失败: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// 获取系统字体路径
        /// </summary>
        /// <param name="fontName">字体名称</param>
        /// <returns>字体路径数组</returns>
        private static string[] GetSystemFontPaths(string fontName)
        {
            string windowsFontsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts");

            return fontName switch
            {
                "simhei" => new[]
                {
                    Path.Combine(windowsFontsDir, "simhei.ttf"),
                    Path.Combine(windowsFontsDir, "simhei2.ttf")
                },
                "simsun" => new[]
                {
                    Path.Combine(windowsFontsDir, "simsun.ttc"),
                    Path.Combine(windowsFontsDir, "simsunb.ttf")
                },
                "msyh" => new[]
                {
                    Path.Combine(windowsFontsDir, "msyh.ttc"),
                    Path.Combine(windowsFontsDir, "msyhbd.ttc"),
                    Path.Combine(windowsFontsDir, "msyhl.ttc")
                },

                "noto_sans" => new[]
                {
                    Path.Combine(windowsFontsDir, "NotoSansCJKsc-Regular.otf"),
                    Path.Combine(windowsFontsDir, "NotoSansSC-Regular.ttf")
                },
                _ => new string[0]
            };
        }

        /// <summary>
        /// 获取可用的字体列表
        /// </summary>
        /// <returns>可用字体名称列表</returns>
        public static List<string> GetAvailableFonts()
        {
            var availableFonts = new List<string>();

            foreach (var fontName in _embeddedFonts.Keys)
            {
                try
                {
                    var font = GetChineseFont(fontName);
                    if (font != null && !availableFonts.Contains(fontName))
                    {
                        availableFonts.Add(fontName);
                    }
                }
                catch
                {
                    // 忽略加载失败的字体
                }
            }

            return availableFonts;
        }

        /// <summary>
        /// 清理字体缓存
        /// </summary>
        public static void ClearCache()
        {
            lock (_lockObject)
            {
                _fontCache.Clear();
                LogHelper.Debug("字体缓存已清理");
            }
        }

        /// <summary>
        /// 重新加载指定字体
        /// </summary>
        /// <param name="fontName">字体名称</param>
        /// <returns>是否成功加载</returns>
        public static bool ReloadFont(string fontName)
        {
            lock (_lockObject)
            {
                if (_fontCache.ContainsKey(fontName))
                {
                    _fontCache.Remove(fontName);
                }

                var font = PreloadFont(fontName);
                return font != null;
            }
        }
    }
}