using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3
{
    /// <summary>
    /// 自动更新管理器
    /// </summary>
    public class UpdateManager
    {
        private readonly string updateUrl;
        private readonly Version currentVersion;
        private readonly HttpClient httpClient;
        private readonly string appDataPath;

        public UpdateManager()
        {
            // 更新检查URL（您可以修改为您的服务器地址）
            updateUrl = "https://your-server.com/updates/update.json";
            currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            httpClient = new HttpClient();
            // 增加超时时间：检查更新30秒，下载文件10分钟
            httpClient.Timeout = TimeSpan.FromMinutes(10);

            // 应用数据目录
            appDataPath = AppDataPathManager.AppRootDirectory;
        }

        /// <summary>
        /// 更新信息模型
        /// </summary>
        public class UpdateInfo
        {
            [JsonProperty("version")]
            public Version Version { get; set; }

            [JsonProperty("downloadUrl")]
            public string DownloadUrl { get; set; }

            [JsonProperty("releaseNotes")]
            public string ReleaseNotes { get; set; }

            [JsonProperty("isMandatory")]
            public bool IsMandatory { get; set; }

            [JsonProperty("releaseDate")]
            public DateTime ReleaseDate { get; set; }

            [JsonProperty("fileSize")]
            public long FileSize { get; set; }

            [JsonProperty("sha256Hash")]
            public string Sha256Hash { get; set; }
        }

        /// <summary>
        /// 检查更新
        /// </summary>
        public async Task<UpdateInfo> CheckForUpdatesAsync()
        {
            try
            {
                // 添加User-Agent头
                httpClient.DefaultRequestHeaders.UserAgent.Clear();
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd($"大诚重命名工具/{currentVersion}");

                var response = await httpClient.GetAsync(updateUrl);
                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                var updateInfo = JsonConvert.DeserializeObject<UpdateInfo>(json);

                if (updateInfo != null && updateInfo.Version > currentVersion)
                {
                    return updateInfo;
                }
            }
            catch (Exception ex)
            {
                // 静默处理错误，不影响主程序运行
                LogUpdateError($"检查更新失败: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// 显示更新对话框
        /// </summary>
        public DialogResult ShowUpdateDialog(UpdateInfo updateInfo)
        {
            var title = updateInfo.IsMandatory ? "强制更新" : "发现新版本";
            var message = $"发现新版本 {updateInfo.Version}\n\n" +
                         $"当前版本: {currentVersion}\n" +
                         $"发布日期: {updateInfo.ReleaseDate:yyyy-MM-dd}\n" +
                         $"文件大小: {FormatFileSize(updateInfo.FileSize)}\n\n" +
                         $"更新内容:\n{updateInfo.ReleaseNotes}\n\n" +
                         (updateInfo.IsMandatory ? "此为强制更新，必须立即更新才能继续使用程序。" : "是否立即下载并安装？");

            var buttons = updateInfo.IsMandatory ? MessageBoxButtons.OK : MessageBoxButtons.YesNo;
            var icon = updateInfo.IsMandatory ? MessageBoxIcon.Warning : MessageBoxIcon.Information;

            return MessageBox.Show(message, title, buttons, icon);
        }

        /// <summary>
        /// 下载并安装更新
        /// </summary>
        public async Task<bool> DownloadAndInstallUpdateAsync(UpdateInfo updateInfo)
        {
            try
            {
                var updateFilePath = Path.Combine(appDataPath, $"update_v{updateInfo.Version}.exe");

                // 显示下载进度
                using (var progressForm = new UpdateProgressForm())
                {
                    progressForm.Show();
                    Application.DoEvents();

                    // 下载更新文件
                    var success = await DownloadFileWithProgress(updateInfo.DownloadUrl, updateFilePath,
                        (progress) => progressForm.UpdateProgress(progress));

                    if (!success)
                    {
                        progressForm.Close();
                        MessageBox.Show("下载更新文件失败，请检查网络连接后重试。", "下载失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    progressForm.SetStatus("准备安装...");
                    await Task.Delay(1000);
                    progressForm.Close();
                }

                // 验证文件完整性
                if (!VerifyFileHash(updateFilePath, updateInfo.Sha256Hash))
                {
                    MessageBox.Show("更新文件校验失败，可能文件已损坏，请重新下载。", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    File.Delete(updateFilePath);
                    return false;
                }

                // 启动安装程序
                var startInfo = new ProcessStartInfo
                {
                    FileName = updateFilePath,
                    Arguments = "/SILENT /NORESTART",
                    UseShellExecute = false
                };

                Process.Start(startInfo);

                // 退出当前程序
                Application.Exit();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"安装更新时发生错误: {ex.Message}", "安装失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUpdateError($"安装更新失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 带进度的文件下载
        /// </summary>
        private async Task<bool> DownloadFileWithProgress(string url, string filePath, Action<int> progressCallback)
        {
            try
            {
                using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                if (!response.IsSuccessStatusCode)
                    return false;

                var totalBytes = response.Content.Headers.ContentLength ?? 0;
                var downloadedBytes = 0L;

                using var contentStream = await response.Content.ReadAsStreamAsync();
                using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

                var buffer = new byte[8192];
                int bytesRead;

                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                    downloadedBytes += bytesRead;

                    if (totalBytes > 0)
                    {
                        var progress = (int)((downloadedBytes * 100) / totalBytes);
                        progressCallback?.Invoke(progress);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogUpdateError($"下载文件失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 验证文件SHA256哈希
        /// </summary>
        private bool VerifyFileHash(string filePath, string expectedHash)
        {
            try
            {
                using var sha256 = System.Security.Cryptography.SHA256.Create();
                using var stream = File.OpenRead(filePath);
                var hashBytes = sha256.ComputeHash(stream);
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

                return hashString.Equals(expectedHash, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                LogUpdateError($"验证文件哈希失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 格式化文件大小
        /// </summary>
        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        /// <summary>
        /// 记录更新错误日志
        /// </summary>
        private void LogUpdateError(string message)
        {
            try
            {
                var logPath = Path.Combine(appDataPath, "update.log");
                var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";
                File.AppendAllText(logPath, logMessage);
            }
            catch
            {
                // 忽略日志记录错误
            }
        }

        /// <summary>
        /// 启动时检查更新
        /// </summary>
        public async void CheckForUpdatesOnStartup()
        {
            try
            {
                // 检查距离上次更新的时间（避免频繁检查）
                var lastCheckFile = Path.Combine(appDataPath, "last_update_check.txt");
                var lastCheckTime = DateTime.MinValue;

                if (File.Exists(lastCheckFile))
                {
                    if (DateTime.TryParse(File.ReadAllText(lastCheckFile), out var parsedTime))
                    {
                        lastCheckTime = parsedTime;
                    }
                }

                // 如果距离上次检查不足24小时，跳过检查
                if (DateTime.Now - lastCheckTime < TimeSpan.FromHours(24))
                    return;

                // 记录本次检查时间
                File.WriteAllText(lastCheckFile, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                // 检查更新
                var updateInfo = await CheckForUpdatesAsync();
                if (updateInfo != null)
                {
                    var result = ShowUpdateDialog(updateInfo);
                    if (result == DialogResult.Yes || updateInfo.IsMandatory)
                    {
                        await DownloadAndInstallUpdateAsync(updateInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUpdateError($"启动时更新检查失败: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 更新进度窗体
    /// </summary>
    public partial class UpdateProgressForm : Form
    {
        private ProgressBar progressBar;
        private Label statusLabel;

        public UpdateProgressForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 窗体设置
            this.Text = "正在下载更新...";
            this.Size = new System.Drawing.Size(400, 120);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;

            // 进度条
            this.progressBar = new ProgressBar();
            this.progressBar.Location = new System.Drawing.Point(20, 20);
            this.progressBar.Size = new System.Drawing.Size(360, 23);
            this.progressBar.Style = ProgressBarStyle.Continuous;

            // 状态标签
            this.statusLabel = new Label();
            this.statusLabel.Location = new System.Drawing.Point(20, 50);
            this.statusLabel.Size = new System.Drawing.Size(360, 23);
            this.statusLabel.Text = "正在下载更新文件...";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 添加控件
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.statusLabel);

            this.ResumeLayout(false);
        }

        public void UpdateProgress(int percentage)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action(() => UpdateProgress(percentage)));
                return;
            }

            progressBar.Value = percentage;
            statusLabel.Text = $"正在下载更新文件... {percentage}%";
        }

        public void SetStatus(string status)
        {
            if (statusLabel.InvokeRequired)
            {
                statusLabel.Invoke(new Action(() => SetStatus(status)));
                return;
            }

            statusLabel.Text = status;
        }
    }
}