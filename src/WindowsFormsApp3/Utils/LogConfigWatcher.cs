using System;
using System.IO;
using System.Threading;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// 日志配置监视器，负责监控日志配置文件的变化并自动更新配置
    /// </summary>
    public class LogConfigWatcher : IDisposable
    {
        private FileSystemWatcher _watcher;
        private bool _isWatching = false;
        private readonly object _syncLock = new object();
        private const string _configFileName = "LogConfig.json";

        /// <summary>
        /// 配置更新事件，当配置文件发生变化时触发
        /// </summary>
        public event EventHandler ConfigUpdated;

        /// <summary>
        /// 开始监控日志配置文件
        /// </summary>
        public void StartWatching()
        {
            lock (_syncLock)
            {
                if (_isWatching)
                {
                    return;
                }

                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configFileName);
                string configDirectory = Path.GetDirectoryName(configPath);

                if (Directory.Exists(configDirectory))
                {
                    try
                    {
                        _watcher = new FileSystemWatcher
                        {
                            Path = configDirectory,
                            Filter = _configFileName,
                            NotifyFilter = NotifyFilters.LastWrite,
                            EnableRaisingEvents = true
                        };

                        _watcher.Changed += OnConfigChanged;
                        _isWatching = true;
                        LogHelper.Info("已开始监控日志配置文件变化");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("启动日志配置文件监控失败", ex);
                    }
                }
                else
                {
                    LogHelper.Warn("日志配置文件所在目录不存在，无法启动监控");
                }
            }
        }

        /// <summary>
        /// 停止监控日志配置文件
        /// </summary>
        public void StopWatching()
        {
            lock (_syncLock)
            {
                if (!_isWatching)
                {
                    return;
                }

                if (_watcher != null)
                {
                    _watcher.EnableRaisingEvents = false;
                    _watcher.Changed -= OnConfigChanged;
                    _watcher.Dispose();
                    _watcher = null;
                }

                _isWatching = false;
                LogHelper.Info("已停止监控日志配置文件变化");
            }
        }

        /// <summary>
        /// 配置文件变化时的处理方法
        /// </summary>
        private void OnConfigChanged(object sender, FileSystemEventArgs e)
        {
            // 防止频繁触发
            Thread.Sleep(100);

            try
            {
                // 重新加载配置
                LogConfigManager.GetConfig();

                // 通知订阅者配置已更新
                ConfigUpdated?.Invoke(this, EventArgs.Empty);

                LogHelper.Info("日志配置文件已更新，新配置已生效");
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新日志配置失败", ex);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            StopWatching();
        }
    }
}