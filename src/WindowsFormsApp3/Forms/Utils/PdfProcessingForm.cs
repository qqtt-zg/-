using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp3.Models;
using WindowsFormsApp3.Interfaces;
using LogLevel = WindowsFormsApp3.Interfaces.LogLevel;
using WindowsFormsApp3.Presenters;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3
{
    /// <summary>
    /// PDF处理窗体 - MVP模式的View实现
    /// 负责UI展示和用户交互，业务逻辑由Presenter处理
    /// </summary>
    public partial class PdfProcessingForm : Form, IPdfProcessingView
    {
        #region 私有字段

        private PdfProcessingPresenter _presenter;
        private PdfProcessingConfig _currentConfig;
        private bool _isProcessing = false;
        private readonly List<LogMessage> _logMessages = new List<LogMessage>();

        #endregion

        #region 事件实现

        public event EventHandler<string> ProcessPdfRequested;
        public event EventHandler<string> GetPageInfoRequested;
        public event EventHandler<string> CompareLibrariesRequested;
        public event EventHandler<string> GenerateReportRequested;
        public event EventHandler<string> CancelProcessingRequested;
        public event EventHandler<PdfProcessingConfig> ConfigUpdated;

        #endregion

        #region 构造函数

        public PdfProcessingForm()
        {
            InitializeComponent();
            InitializeForm();
            InitializePresenter();
            LoadDefaultConfig();
        }

        /// <summary>
        /// 传入Presenter的构造函数（用于依赖注入）
        /// </summary>
        /// <param name="presenter">Presenter实例</param>
        public PdfProcessingForm(PdfProcessingPresenter presenter) : this()
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            _presenter.SetView(this);
        }

        #endregion

        #region 初始化方法

        private void InitializeForm()
        {
            // 设置窗体属性
            this.Text = "PDF处理工具 - iText7";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(600, 400);

            // 初始化控件
            InitializeEventHandlers();
        }


        private void InitializeEventHandlers()
        {
            // 按钮事件
            btnBrowseFile.Click += (s, e) =>
            {
                var filePath = ShowFileDialog();
                if (!string.IsNullOrEmpty(filePath))
                {
                    txtFilePath.Text = filePath;
                }
            };

            btnGetPageCount.Click += (s, e) =>
            {
                var filePath = GetSelectedPdfFilePath();
                if (!string.IsNullOrEmpty(filePath))
                    ProcessPdfRequested?.Invoke(this, filePath);
            };

            btnGetFirstPageSize.Click += (s, e) =>
            {
                var filePath = GetSelectedPdfFilePath();
                if (!string.IsNullOrEmpty(filePath))
                    GetPageInfoRequested?.Invoke(this, filePath);
            };

            btnGetAllPageSizes.Click += (s, e) =>
            {
                var filePath = GetSelectedPdfFilePath();
                if (!string.IsNullOrEmpty(filePath))
                    ProcessPdfRequested?.Invoke(this, filePath);
            };

            btnCompareLibraries.Click += (s, e) =>
            {
                var filePath = GetSelectedPdfFilePath();
                if (!string.IsNullOrEmpty(filePath))
                    CompareLibrariesRequested?.Invoke(this, filePath);
            };

            btnGenerateReport.Click += (s, e) =>
            {
                var filePath = GetSelectedPdfFilePath();
                if (!string.IsNullOrEmpty(filePath))
                    GenerateReportRequested?.Invoke(this, filePath);
            };

            btnCancel.Click += (s, e) =>
            {
                var filePath = GetSelectedPdfFilePath();
                if (!string.IsNullOrEmpty(filePath))
                    CancelProcessingRequested?.Invoke(this, filePath);
            };

            // 配置变更事件
            chkUseCropBox.CheckedChanged += (s, e) => NotifyConfigUpdated();
            chkEnableDetailedLogging.CheckedChanged += (s, e) => NotifyConfigUpdated();
            cmbPreferredLibrary.SelectedIndexChanged += (s, e) => NotifyConfigUpdated();

            // 窗体事件
            this.FormClosing += (s, e) =>
            {
                if (_isProcessing)
                {
                    if (!RequestConfirmation("处理正在进行中，确定要退出吗？"))
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            };
        }

        private void InitializePresenter()
        {
            _presenter = new PdfProcessingPresenter();
            _presenter.SetView(this);
        }

        #endregion

        #region 配置管理

        private void LoadDefaultConfig()
        {
            _currentConfig = new PdfProcessingConfig
            {
                UseCropBoxByDefault = true,
                EnableDetailedLogging = true,
                MaxRetryCount = 3,
                RetryDelayMs = 100,
                EnableErrorRecovery = true,
                PreferredLibrary = "IText7"
            };

            LoadConfig(_currentConfig);
        }

        private void NotifyConfigUpdated()
        {
            var config = SaveConfig();
            ConfigUpdated?.Invoke(this, config);
        }

        #endregion

        #region IPdfProcessingView 实现

        public void ShowProcessingResult(PdfProcessingResponse response)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<PdfProcessingResponse>(ShowProcessingResult), response);
                return;
            }

            var result = new System.Text.StringBuilder();
            result.AppendLine($"处理结果: {(response.Success ? "成功" : "失败")}");
            result.AppendLine($"处理时间: {response.ProcessingTimeMs} ms");
            result.AppendLine($"请求ID: {response.RequestId}");

            if (!string.IsNullOrEmpty(response.Message))
                result.AppendLine($"消息: {response.Message}");

            if (response.Exception != null)
                result.AppendLine($"异常: {response.Exception.Message}");

            if (response.Data != null)
                result.AppendLine($"数据: {response.Data}");

            txtResults.AppendText(result.ToString());
            txtResults.AppendText(Environment.NewLine);
            txtResults.ScrollToCaret();
        }

        public void ShowPdfFileInfo(PdfFileInfo fileInfo)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<PdfFileInfo>(ShowPdfFileInfo), fileInfo);
                return;
            }

            var info = new System.Text.StringBuilder();
            info.AppendLine($"文件: {fileInfo.FileName}");
            info.AppendLine($"路径: {fileInfo.FilePath}");
            info.AppendLine($"大小: {fileInfo.FileSize / 1024.0:F1} KB");
            info.AppendLine($"页数: {fileInfo.PageCount}");
            info.AppendLine($"首页尺寸: {fileInfo.FirstPageSize}");
            info.AppendLine($"修改时间: {fileInfo.LastModified}");

            if (fileInfo.Errors.Count > 0)
            {
                info.AppendLine();
                info.AppendLine("发现的错误:");
                foreach (var error in fileInfo.Errors)
                {
                    info.AppendLine($"  - {error}");
                }
            }

            txtResults.AppendText(info.ToString());
            txtResults.AppendText(Environment.NewLine);

            // 更新页面信息表格
            UpdatePageInfoGrid(fileInfo.AllPageSizes);
        }

        public void ShowProcessingProgress(PdfProcessingProgress progress)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<PdfProcessingProgress>(ShowProcessingProgress), progress);
                return;
            }

            lblStatus.Text = progress.Message;
            UpdateProgressBar(progress.ProgressPercentage);

            if (progress.Status == PdfProcessingStatus.Completed)
            {
                SetProcessingState(false);
            }
            else if (progress.Status == PdfProcessingStatus.Failed)
            {
                SetProcessingState(false);
                ShowError($"处理失败: {progress.Message}");
            }
        }

        public void ShowError(string message, string title = "错误")
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, string>(ShowError), message, title);
                return;
            }

            MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            AddLogMessage(message, LogLevel.Error);
        }

        public void ShowWarning(string message, string title = "警告")
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, string>(ShowWarning), message, title);
                return;
            }

            MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            AddLogMessage(message, LogLevel.Warning);
        }

        public void ShowSuccess(string message, string title = "成功")
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, string>(ShowSuccess), message, title);
                return;
            }

            MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            AddLogMessage(message, LogLevel.Information);
        }

        public void ShowLibraryComparison(PdfLibraryComparisonResult comparison)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<PdfLibraryComparisonResult>(ShowLibraryComparison), comparison);
                return;
            }

            var result = new System.Text.StringBuilder();
            result.AppendLine($"文件: {comparison.FileName}");
            result.AppendLine($"PDFsharp: {(comparison.PdfToolsResult.Success ? $"{comparison.PdfToolsResult.Width}x{comparison.PdfToolsResult.Height}mm" : "失败")}");
            result.AppendLine($"iText 7: {(comparison.IText7Result.Success ? $"{comparison.IText7Result.Width}x{comparison.IText7Result.Height}mm" : "失败")}");
            result.AppendLine($"结果一致: {(comparison.ResultsMatch ? "是" : "否")}");

            if (!comparison.ResultsMatch)
            {
                result.AppendLine($"尺寸差异 - 宽度: {comparison.WidthDifference:F2}mm, 高度: {comparison.HeightDifference:F2}mm");
            }

            result.AppendLine($"建议: {comparison.Recommendation}");

            txtResults.AppendText(result.ToString());
            txtResults.AppendText(Environment.NewLine);
        }

        public void ShowProcessingReport(string report)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowProcessingReport), report);
                return;
            }

            txtResults.AppendText(report);
            txtResults.AppendText(Environment.NewLine);
        }

        public void ShowStatistics(PdfProcessingStatistics statistics)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<PdfProcessingStatistics>(ShowStatistics), statistics);
                return;
            }

            var stats = new System.Text.StringBuilder();
            stats.AppendLine("=== 处理统计 ===");
            stats.AppendLine($"总处理数: {statistics.TotalProcessed}");
            stats.AppendLine($"成功: {statistics.Successful}");
            stats.AppendLine($"失败: {statistics.Failed}");
            stats.AppendLine($"平均处理时间: {statistics.AverageProcessingTimeMs:F2} ms");
            stats.AppendLine($"最后更新: {statistics.LastUpdated}");

            txtResults.AppendText(stats.ToString());
            txtResults.AppendText(Environment.NewLine);
        }

        public void SetProcessingState(bool isProcessing)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(SetProcessingState), isProcessing);
                return;
            }

            _isProcessing = isProcessing;
            SetProcessingButtonsEnabled(!isProcessing);
            SetCancelButtonEnabled(isProcessing);
            SetProgressBarVisible(isProcessing);

            if (isProcessing)
            {
                lblStatus.Text = "正在处理...";
            }
            else
            {
                lblStatus.Text = "就绪";
                UpdateProgressBar(0);
            }
        }

        public void SetProcessingButtonsEnabled(bool enabled)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(SetProcessingButtonsEnabled), enabled);
                return;
            }

            btnGetPageCount.Enabled = enabled;
            btnGetFirstPageSize.Enabled = enabled;
            btnGetAllPageSizes.Enabled = enabled;
            btnCompareLibraries.Enabled = enabled;
            btnGenerateReport.Enabled = enabled;
        }

        public void SetCancelButtonEnabled(bool enabled)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(SetCancelButtonEnabled), enabled);
                return;
            }

            btnCancel.Enabled = enabled;
        }

        public void SetProgressBarVisible(bool visible)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(SetProgressBarVisible), visible);
                return;
            }

            progressBar.Visible = visible;
        }

        public void UpdateProgressBar(int percentage)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(UpdateProgressBar), percentage);
                return;
            }

            progressBar.Value = Math.Max(0, Math.Min(100, percentage));
        }

        public void ClearResults()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ClearResults));
                return;
            }

            txtResults.Clear();
            dgvPageInfo.DataSource = null;
        }

        public void SetLoadingState(bool isLoading)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(SetLoadingState), isLoading);
                return;
            }

            this.UseWaitCursor = isLoading;
        }

        public string GetSelectedPdfFilePath()
        {
            return txtFilePath.Text.Trim();
        }

        public PdfProcessingConfig GetCurrentConfig()
        {
            return _currentConfig;
        }

        public Dictionary<string, object> GetProcessingParameters()
        {
            return new Dictionary<string, object>
            {
                ["UseCropBox"] = chkUseCropBox.Checked,
                ["EnableDetailedLogging"] = chkEnableDetailedLogging.Checked,
                ["PreferredLibrary"] = cmbPreferredLibrary.SelectedItem?.ToString()
            };
        }

        public bool HasActiveProcessing()
        {
            return _isProcessing;
        }

        public void LoadConfig(PdfProcessingConfig config)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<PdfProcessingConfig>(LoadConfig), config);
                return;
            }

            if (config == null) return;

            _currentConfig = config;
            chkUseCropBox.Checked = config.UseCropBoxByDefault;
            chkEnableDetailedLogging.Checked = config.EnableDetailedLogging;
            cmbPreferredLibrary.SelectedItem = config.PreferredLibrary;
        }

        public PdfProcessingConfig SaveConfig()
        {
            _currentConfig = new PdfProcessingConfig
            {
                UseCropBoxByDefault = chkUseCropBox.Checked,
                EnableDetailedLogging = chkEnableDetailedLogging.Checked,
                PreferredLibrary = cmbPreferredLibrary.SelectedItem?.ToString() ?? "IText7"
            };

            return _currentConfig;
        }

        public void ResetConfigToDefault()
        {
            LoadDefaultConfig();
        }

        public PdfProcessingValidationResult ValidateConfig(PdfProcessingConfig config)
        {
            var result = new PdfProcessingValidationResult { IsValid = true };

            if (config == null)
            {
                result.IsValid = false;
                result.ErrorMessage = "配置不能为空";
                result.ErrorType = ValidationErrorType.InvalidParameters;
                return result;
            }

            if (string.IsNullOrEmpty(config.PreferredLibrary))
            {
                result.IsValid = false;
                result.ErrorMessage = "首选库不能为空";
                result.ErrorType = ValidationErrorType.InvalidParameters;
            }

            return result;
        }

        public string RequestPdfFileSelection()
        {
            return ShowFileDialog();
        }

        public bool RequestConfirmation(string message, string title = "确认")
        {
            return MessageBox.Show(this, message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public string RequestUserInput(string prompt, string title = "输入", string defaultValue = "")
        {
            // 简单实现，可以使用更复杂的输入对话框
            using (var form = new Form())
            {
                form.Text = title;
                form.Size = new Size(300, 150);
                form.StartPosition = FormStartPosition.CenterParent;

                var label = new Label { Text = prompt, Location = new Point(10, 10), Width = 260 };
                var textBox = new TextBox { Text = defaultValue, Location = new Point(10, 40), Width = 260 };
                var okButton = new Button { Text = "确定", DialogResult = DialogResult.OK, Location = new Point(10, 80), Width = 80 };
                var cancelButton = new Button { Text = "取消", DialogResult = DialogResult.Cancel, Location = new Point(100, 80), Width = 80 };

                form.Controls.AddRange(new Control[] { label, textBox, okButton, cancelButton });
                form.AcceptButton = okButton;
                form.CancelButton = cancelButton;

                return form.ShowDialog(this) == DialogResult.OK ? textBox.Text : string.Empty;
            }
        }

        public string ShowFileDialog(string filter = "PDF文件|*.pdf", string title = "选择PDF文件")
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = filter;
                dialog.Title = title;
                dialog.Multiselect = false;

                return dialog.ShowDialog(this) == DialogResult.OK ? dialog.FileName : string.Empty;
            }
        }

        public void AddLogMessage(string message, Interfaces.LogLevel level = LogLevel.Information)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, LogLevel>(AddLogMessage), message, level);
                return;
            }

            var logMessage = new LogMessage
            {
                Timestamp = DateTime.Now,
                Level = level,
                Message = message
            };

            _logMessages.Add(logMessage);
            UpdateLogDisplay();
        }

        public void ClearLog()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ClearLog));
                return;
            }

            _logMessages.Clear();
            txtLog.Clear();
        }

        public void RefreshLog()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RefreshLog));
                return;
            }

            UpdateLogDisplay();
        }

        #endregion

        #region 私有辅助方法

        private void UpdatePageInfoGrid(List<PageSizeInfo> pageSizes)
        {
            if (pageSizes == null || pageSizes.Count == 0) return;

            // 设置数据源
            dgvPageInfo.DataSource = pageSizes;

            // 设置列
            dgvPageInfo.Columns["PageNumber"].HeaderText = "页码";
            dgvPageInfo.Columns["CropBox"].HeaderText = "裁剪框尺寸";
            dgvPageInfo.Columns["MediaBox"].HeaderText = "媒体框尺寸";
            dgvPageInfo.Columns["Rotation"].HeaderText = "旋转角度";

            // 调整列宽
            dgvPageInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void UpdateLogDisplay()
        {
            txtLog.Clear();

            var recentLogs = _logMessages.Count > 1000 ? _logMessages.Skip(_logMessages.Count - 1000) : _logMessages;
      foreach (var log in recentLogs) // 限制显示最近1000条日志
            {
                var color = GetLogLevelColor(log.Level);
                var timestamp = log.Timestamp.ToString("HH:mm:ss.fff");
                var level = log.Level.ToString().PadRight(7);

                txtLog.SelectionColor = color;
                txtLog.AppendText($"[{timestamp}] [{level}] {log.Message}{Environment.NewLine}");
            }

            txtLog.ScrollToCaret();
        }

        private Color GetLogLevelColor(Interfaces.LogLevel level)
        {
            return level switch
            {
                Interfaces.LogLevel.Debug => Color.Gray,
                Interfaces.LogLevel.Information => Color.White,
                Interfaces.LogLevel.Warning => Color.Yellow,
                Interfaces.LogLevel.Error => Color.Red,
                Interfaces.LogLevel.Critical => Color.Magenta,
                _ => Color.White
            };
        }

        #endregion

}    }

    /// <summary>
    /// 日志消息
    /// </summary>
    internal class LogMessage
    {
        public DateTime Timestamp { get; set; }
        public LogLevel Level { get; set; }
        public string Message { get; set; }
    }
