using System;
using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Collections.Generic;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// CefSharp 初始化工具类
    /// 负责配置和初始化CefSharp运行时环境
    /// </summary>
    public static class CefSharpInitializer
    {
        private static bool _isInitialized = false;

        /// <summary>
        /// 初始化CefSharp运行时
        /// 必须在创建任何ChromiumWebBrowser实例之前调用
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized)
            {
                LogHelper.Debug("[CefSharpInitializer] CefSharp 已经初始化，跳过重复初始化");
                return;
            }

            try
            {
                // 确保缓存目录存在
                string cachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PrepressToolbox", "CefSharpCache");
                if (!Directory.Exists(cachePath))
                {
                    Directory.CreateDirectory(cachePath);
                }

                // 使用CefSharp 87.x兼容的初始化设置
                var settings = new CefSettings()
                {
                    // 设置缓存路径
                    CachePath = cachePath,

                    // 禁用不必要的功能以提高性能
                    PersistSessionCookies = false,
                    UserAgent = "PrepressToolbox/1.0 (PDF Preview)",

                    // 设置日志级别
                    LogSeverity = LogSeverity.Warning,

                    // 设置语言为中文
                    Locale = "zh-CN"
                };

                // 添加命令行参数优化PDF显示
                // 注意：不要禁用pdf-embedded-viewer，否则工具栏不会显示
                settings.CefCommandLineArgs.Add("force-prefers-reduced-motion", "0");

                // 初始化Cef
                if (Cef.IsInitialized != true)
                {
                    Cef.Initialize(settings);
                    _isInitialized = true;
                    LogHelper.Debug("[CefSharpInitializer] CefSharp 初始化成功");
                }
                else
                {
                    _isInitialized = true;
                    LogHelper.Debug("[CefSharpInitializer] CefSharp 已经被其他进程初始化");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[CefSharpInitializer] CefSharp 初始化失败: {ex.Message}");
                MessageBox.Show($"PDF预览组件初始化失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 清理CefSharp资源
        /// 在应用程序退出时调用
        /// </summary>
        public static void Shutdown()
        {
            try
            {
                if (_isInitialized && Cef.IsInitialized == true)
                {
                    Cef.Shutdown();
                    _isInitialized = false;
                    LogHelper.Debug("[CefSharpInitializer] CefSharp 已关闭");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[CefSharpInitializer] CefSharp 关闭失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取初始化状态
        /// </summary>
        public static bool IsInitialized => _isInitialized;

        /// <summary>
        /// 清理缓存
        /// </summary>
        public static void ClearCache()
        {
            try
            {
                // 简化的缓存清理 - 只清理本地缓存目录
                string cachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PrepressToolbox", "CefSharpCache");
                if (Directory.Exists(cachePath))
                {
                    try
                    {
                        Directory.Delete(cachePath, true);
                        LogHelper.Debug("[CefSharpInitializer] 缓存目录已清理");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error($"[CefSharpInitializer] 删除缓存目录失败: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[CefSharpInitializer] 清理缓存失败: {ex.Message}");
            }
        }
    }
}