using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using WindowsFormsApp3.Interfaces;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Controls
{
    /// <summary>
    /// 基于CefSharp的PDF预览控件
    /// 提供高质量的PDF渲染和现代化的用户体验
    /// </summary>
    public partial class CefSharpPdfPreviewControl : UserControl, IPdfPreviewControl
    {
        private ChromiumWebBrowser _browser;
        private string _currentFilePath;
        private bool _isInitialized = false;
        private int _currentPage = 1;
        private int _totalPages = 0;

        /// <summary>
        /// 当前页码（从1开始）
        /// </summary>
        public int CurrentPage
        {
            get => _currentPage;
            private set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    PageChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages => _totalPages;

        /// <summary>
        /// 当前PDF文件路径
        /// </summary>
        public string CurrentFilePath => _currentFilePath;

        /// <summary>
        /// 页面变化事件
        /// </summary>
        public event EventHandler PageChanged;

        /// <summary>
        /// PDF加载完成事件
        /// </summary>
        public event EventHandler PageLoaded;

        /// <summary>
        /// PDF加载错误事件
        /// </summary>
        public event EventHandler<string> LoadError;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CefSharpPdfPreviewControl()
        {
            InitializeComponent();
            InitializeBrowser();
        }

        /// <summary>
        /// 初始化浏览器控件
        /// </summary>
        private void InitializeBrowser()
        {
            try
            {
                // 确保CefSharp已初始化
                if (!CefSharpInitializer.IsInitialized)
                {
                    CefSharpInitializer.Initialize();
                }

                // 创建ChromiumWebBrowser实例
                _browser = new ChromiumWebBrowser("about:blank")
                {
                    Dock = DockStyle.Fill,
                    BrowserSettings = new BrowserSettings()
                    {
                        DefaultEncoding = "UTF-8",
                        Javascript = CefState.Enabled
                    }
                };

                // 绑定事件
                _browser.LoadingStateChanged += Browser_LoadingStateChanged;
                _browser.FrameLoadEnd += Browser_FrameLoadEnd;
                _browser.LoadError += Browser_LoadError;

                // 禁用右键菜单
                _browser.MenuHandler = new CustomMenuHandler();

                this.Controls.Add(_browser);
                _isInitialized = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化PDF预览控件失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 浏览器加载状态变化事件
        /// </summary>
        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (e.IsLoading == false)
            {
                // 页面加载完成，设置PDF查看器参数
                _ = Task.Run(async () =>
                {
                    await Task.Delay(500); // 等待PDF viewer初始化
                    await ConfigurePdfViewer();
                });
            }
        }

        /// <summary>
        /// 浏览器框架加载完成事件
        /// </summary>
        private async void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                // 等待PDF viewer完全加载
                await Task.Delay(1000);

                // 获取PDF信息
                await UpdatePdfInfo();

                // 触发加载完成事件
                PageLoaded?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 浏览器加载错误事件
        /// </summary>
        private void Browser_LoadError(object sender, LoadErrorEventArgs e)
        {
            string errorMsg = $"加载PDF失败: {e.ErrorText}";
            LoadError?.Invoke(this, errorMsg);
        }

        /// <summary>
        /// 配置PDF查看器参数
        /// </summary>
        private async Task ConfigurePdfViewer()
        {
            try
            {
                // JavaScript代码设置PDF查看器为单页模式和适应页面
                var script = @"
                    (function() {
                        try {
                            // 查找PDF viewer元素
                            var viewer = document.querySelector('pdf-viewer');
                            if (viewer) {
                                // 设置为单页模式
                                viewer.setScrollMode('page');
                                // 设置不展开页面
                                viewer.setSpreadMode('none');
                                // 设置为适应页面
                                viewer.currentScaleValue = 'page-fit';

                                // 监听页面变化
                                viewer.addEventListener('pagechange', function() {
                                    window.currentPdfPage = viewer.currentPageNumber + 1;
                                    window.totalPdfPages = viewer.pagesCount;
                                });

                                // 初始化页面信息
                                window.currentPdfPage = viewer.currentPageNumber + 1;
                                window.totalPdfPages = viewer.pagesCount;

                                return {success: true, page: viewer.currentPageNumber + 1, total: viewer.pagesCount};
                            }
                            return {success: false, error: 'PDF viewer not found'};
                        } catch (e) {
                            return {success: false, error: e.message};
                        }
                    })();
                ";

                var result = await _browser.EvaluateScriptAsync(script);
                if (result.Success && result.Result != null)
                {
                    var data = result.Result as System.Collections.Generic.Dictionary<string, object>;
                    if (data != null && data.TryGetValue("success", out var success) && success.Equals(true))
                    {
                        if (data.TryGetValue("page", out var page) && int.TryParse(page.ToString(), out int currentPage))
                        {
                            CurrentPage = currentPage;
                        }
                        if (data.TryGetValue("total", out var total) && int.TryParse(total.ToString(), out int totalPages))
                        {
                            _totalPages = totalPages;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"配置PDF查看器失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新PDF信息
        /// </summary>
        private async Task UpdatePdfInfo()
        {
            try
            {
                var script = @"
                    (function() {
                        try {
                            var viewer = document.querySelector('pdf-viewer');
                            if (viewer) {
                                return {
                                    page: viewer.currentPageNumber + 1,
                                    total: viewer.pagesCount
                                };
                            }
                            return null;
                        } catch (e) {
                            return null;
                        }
                    })();
                ";

                var result = await _browser.EvaluateScriptAsync(script);
                if (result.Success && result.Result != null)
                {
                    var data = result.Result as System.Collections.Generic.Dictionary<string, object>;
                    if (data != null)
                    {
                        if (data.TryGetValue("page", out var page) && int.TryParse(page.ToString(), out int currentPage))
                        {
                            CurrentPage = currentPage;
                        }
                        if (data.TryGetValue("total", out var total) && int.TryParse(total.ToString(), out int totalPages))
                        {
                            _totalPages = totalPages;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"更新PDF信息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 加载PDF文件
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>是否加载成功</returns>
        public bool LoadPdf(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return false;
            }

            if (!_isInitialized)
            {
                return false;
            }

            try
            {
                _currentFilePath = filePath;
                _currentPage = 1;
                _totalPages = 0;

                // 加载本地PDF文件
                // 使用鼠标滚轮可缩放，Ctrl+滚轮可快速缩放
                _browser.Load(filePath);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载PDF文件失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 重新加载当前PDF
        /// </summary>
        public new void Refresh()
        {
            if (!string.IsNullOrEmpty(_currentFilePath))
            {
                _browser.Reload();
            }
        }

        /// <summary>
        /// 导航到第一页
        /// </summary>
        public void FirstPage()
        {
            _ = GoToPage(1);
        }

        /// <summary>
        /// 导航到上一页
        /// </summary>
        public void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                _ = GoToPage(CurrentPage - 1);
            }
        }

        /// <summary>
        /// 导航到下一页
        /// </summary>
        public void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                _ = GoToPage(CurrentPage + 1);
            }
        }

        /// <summary>
        /// 导航到最后一页
        /// </summary>
        public void LastPage()
        {
            if (TotalPages > 0)
            {
                _ = GoToPage(TotalPages);
            }
        }

        /// <summary>
        /// 导航到指定页
        /// </summary>
        /// <param name="pageNumber">页码（从1开始）</param>
        /// <returns>是否导航成功</returns>
        public async Task<bool> GoToPage(int pageNumber)
        {
            if (pageNumber < 1 || pageNumber > TotalPages)
            {
                return false;
            }

            try
            {
                var script = $@"
                    (function() {{
                        try {{
                            var viewer = document.querySelector('pdf-viewer');
                            if (viewer) {{
                                viewer.currentPageNumber = {pageNumber - 1};
                                return true;
                            }}
                            return false;
                        }} catch (e) {{
                            return false;
                        }}
                    }})();
                ";

                var result = await _browser.EvaluateScriptAsync(script);
                if (result.Success && result.Result is bool success && success)
                {
                    CurrentPage = pageNumber;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"跳转到页面失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 切换边栏显示状态
        /// </summary>
        public async Task ToggleSidebar()
        {
            try
            {
                // Chrome PDF Viewer使用特定的工具栏按钮
                var script = @"
                    (function() {
                        try {
                            // 方法1: 尝试点击侧边栏切换按钮 (Chrome PDF Viewer)
                            var sidenavBtn = document.querySelector('viewer-sidenav-toggle, #sidenavToggle, [id*=""sidenav""], button[aria-label*=""sidenav""], button[aria-label*=""Sidenav""]');
                            if (sidenavBtn) {
                                sidenavBtn.click();
                                return 'sidenav button clicked';
                            }
                            
                            // 方法2: 尝试查找pdf-viewer shadow DOM
                            var pdfViewer = document.querySelector('pdf-viewer');
                            if (pdfViewer && pdfViewer.shadowRoot) {
                                var shadowBtn = pdfViewer.shadowRoot.querySelector('viewer-sidenav-toggle, #sidenavToggle');
                                if (shadowBtn) {
                                    shadowBtn.click();
                                    return 'shadow sidenav clicked';
                                }
                            }
                            
                            // 方法3: 模拟键盘快捷键 Ctrl+Shift+P (某些版本)
                            return 'no sidenav found';
                        } catch (e) {
                            return 'error: ' + e.message;
                        }
                    })();
                ";

                var result = await _browser.EvaluateScriptAsync(script);
                System.Diagnostics.Debug.WriteLine($"ToggleSidebar result: {result.Result}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"切换边栏失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 设置为适应页面缩放
        /// </summary>
        public async Task FitToPage()
        {
            try
            {
                var script = @"
                    (function() {
                        try {
                            var viewer = document.querySelector('pdf-viewer');
                            if (viewer) {
                                viewer.currentScaleValue = 'page-fit';
                                return true;
                            }
                            return false;
                        } catch (e) {
                            console.error('Fit to page failed:', e);
                            return false;
                        }
                    })();
                ";

                await _browser.EvaluateScriptAsync(script);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"适应页面失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _browser?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// 自定义菜单处理器，添加PDF控制菜单项
    /// </summary>
    public class CustomMenuHandler : IContextMenuHandler
    {
        // 自定义菜单命令ID
        private const CefMenuCommand MenuToggleSidebar = (CefMenuCommand)26501;
        private const CefMenuCommand MenuFitToPage = (CefMenuCommand)26502;
        
        private readonly CefSharpPdfPreviewControl _pdfControl;
        
        public CustomMenuHandler()
        {
            _pdfControl = null;
        }
        
        public CustomMenuHandler(CefSharpPdfPreviewControl pdfControl)
        {
            _pdfControl = pdfControl;
        }

        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            // 清除默认菜单项
            model.Clear();
            
            // 添加操作提示（仅显示，无实际功能）
            model.AddItem(MenuToggleSidebar, "提示: 滚轮缩放 | Ctrl+滚轮快速缩放");
            model.AddItem(MenuFitToPage, "提示: 拖拽可移动视图");
        }

        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            // 菜单项仅作为提示，无实际操作
            return true;
        }
        
        private async Task ExecuteToggleSidebar(IFrame frame)
        {
            try
            {
                // Chrome PDF Viewer 使用键盘快捷键 Ctrl+Shift+S 或直接调用viewer API
                var script = @"
                    (function() {
                        try {
                            // 方法1: 尝试查找embed元素并发送postMessage
                            var embed = document.querySelector('embed[type=""application/pdf""]');
                            if (embed) {
                                embed.postMessage({type: 'toggleBookmarks'});
                                return 'postMessage sent';
                            }
                            
                            // 方法2: 模拟键盘快捷键
                            var event = new KeyboardEvent('keydown', {
                                key: 's',
                                code: 'KeyS',
                                ctrlKey: true,
                                shiftKey: true,
                                bubbles: true
                            });
                            document.dispatchEvent(event);
                            return 'keyboard event sent';
                        } catch (e) {
                            return 'error: ' + e.message;
                        }
                    })();
                ";
                var result = await frame.EvaluateScriptAsync(script);
                System.Diagnostics.Debug.WriteLine($"ToggleSidebar result: {result.Result}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"切换边栏失败: {ex.Message}");
            }
        }
        
        private async Task ExecuteFitToPage(IFrame frame)
        {
            try
            {
                // Chrome PDF Viewer 使用键盘快捷键或postMessage
                var script = @"
                    (function() {
                        try {
                            // 方法1: 尝试查找embed元素并发送postMessage
                            var embed = document.querySelector('embed[type=""application/pdf""]');
                            if (embed) {
                                embed.postMessage({type: 'setZoom', zoom: 'fit-to-page'});
                                return 'postMessage sent';
                            }
                            
                            // 方法2: 模拟键盘快捷键 Ctrl+0 (适应页面)
                            var event = new KeyboardEvent('keydown', {
                                key: '0',
                                code: 'Digit0',
                                ctrlKey: true,
                                bubbles: true
                            });
                            document.dispatchEvent(event);
                            return 'keyboard event sent';
                        } catch (e) {
                            return 'error: ' + e.message;
                        }
                    })();
                ";
                var result = await frame.EvaluateScriptAsync(script);
                System.Diagnostics.Debug.WriteLine($"FitToPage result: {result.Result}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"适应页面失败: {ex.Message}");
            }
        }

        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
        }

        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }
}