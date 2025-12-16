using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Controls
{
    /// <summary>
    /// PDF预览控件包装器
    /// 内部使用CefSharpPdfPreviewControl实现PDF预览功能
    /// 保持向后兼容的公共API
    /// </summary>
    public class PdfPreviewControl : Panel
    {
        #region 私有字段

        private CefSharpPdfPreviewControl _cefControl;
        private bool _isLoading = false;
        private readonly bool _isDesignMode;

        #endregion

        #region 公共属性

        /// <summary>
        /// 当前显示的页面索引（从0开始）
        /// </summary>
        public int CurrentPageIndex
        {
            get => _cefControl?.CurrentPage - 1 ?? 0;
            set
            {
                if (_cefControl != null && value >= 0)
                {
                    _ = _cefControl.GoToPage(value + 1);
                }
            }
        }

        /// <summary>
        /// PDF 文档总页数
        /// </summary>
        public int PageCount => _cefControl?.TotalPages ?? 0;

        /// <summary>
        /// 当前缩放百分比（CefSharp自动管理，此处仅用于兼容）
        /// </summary>
        public float CurrentZoom
        {
            get => 100f; // CefSharp内部管理缩放
            set
            {
                // CefSharp通过JavaScript控制缩放，此处保留接口兼容性
                // 可以通过JavaScript注入实现，但通常使用原生缩放功能
            }
        }

        /// <summary>
        /// 是否正在加载
        /// </summary>
        public bool IsLoading => _isLoading;

        #endregion

        #region 事件

        /// <summary>
        /// 页面加载完成事件
        /// </summary>
        public event EventHandler<PageLoadedEventArgs> PageLoaded;

        /// <summary>
        /// 加载出错事件
        /// </summary>
        public event EventHandler<ErrorEventArgs> LoadError;

        #endregion

        #region 构造函数

        public PdfPreviewControl()
        {
            _isDesignMode = DesignMode;
            InitializeUI();
        }

        #endregion

        #region UI 初始化

        private void InitializeUI()
        {
            this.BackColor = Color.FromArgb(248, 248, 248);
            this.Dock = DockStyle.Fill;
            this.BorderStyle = BorderStyle.None;
            this.Padding = new Padding(0);
            this.DoubleBuffered = true;

            // 在设计器模式下，显示占位符而不创建CefSharp控件
            if (_isDesignMode)
            {
                CreateDesignTimePlaceholder();
                return;
            }

            try
            {
                // 确保CefSharp已初始化
                if (!CefSharpInitializer.IsInitialized)
                {
                    CefSharpInitializer.Initialize();
                }

                // 创建CefSharp PDF预览控件
                _cefControl = new CefSharpPdfPreviewControl
                {
                    Dock = DockStyle.Fill
                };

                // 绑定事件
                _cefControl.PageChanged += CefControl_PageChanged;
                _cefControl.PageLoaded += CefControl_PageLoaded;
                _cefControl.LoadError += CefControl_LoadError;

                this.Controls.Add(_cefControl);
                LogHelper.Debug("[PdfPreviewControl] CefSharp PDF预览控件初始化完成");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[PdfPreviewControl] 初始化失败: {ex.Message}");
                // 创建一个错误提示标签
                var errorLabel = new Label
                {
                    Text = $"PDF预览组件初始化失败\n{ex.Message}",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Red,
                    BackColor = Color.LightGray
                };
                this.Controls.Add(errorLabel);
            }
        }

        /// <summary>
        /// 创建设计时占位符
        /// </summary>
        private void CreateDesignTimePlaceholder()
        {
            var placeholder = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 245, 245),
                BorderStyle = BorderStyle.FixedSingle
            };

            var label = new Label
            {
                Text = "PDF预览控件\n(CefSharp - 设计时模式)",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(100, 100, 100),
                Font = new System.Drawing.Font("Microsoft YaHei", 9f, FontStyle.Regular),
                AutoSize = false
            };

            placeholder.Controls.Add(label);
            this.Controls.Add(placeholder);
        }

        #endregion

        #region 事件处理

        private void CefControl_PageChanged(object sender, EventArgs e)
        {
            // 页面变化时可以触发相关事件
            // 这里暂时不需要特殊处理
        }

        private void CefControl_PageLoaded(object sender, EventArgs e)
        {
            _isLoading = false;
            OnPageLoaded(CurrentPageIndex, PageCount);
        }

        private void CefControl_LoadError(object sender, string error)
        {
            _isLoading = false;
            OnLoadError(new Exception(error));
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 异步加载 PDF 文件
        /// </summary>
        public async Task<bool> LoadPdfAsync(string filePath)
        {
            // 设计时模式下不加载PDF
            if (_isDesignMode)
            {
                return true;
            }

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                LogHelper.Error($"[PdfPreviewControl] PDF 文件不存在: {filePath}");
                return false;
            }

            try
            {
                _isLoading = true;
                LogHelper.Debug($"[PdfPreviewControl] 开始加载PDF: {filePath}");

                if (_cefControl != null)
                {
                    bool success = _cefControl.LoadPdf(filePath);
                    if (success)
                    {
                        LogHelper.Debug("[PdfPreviewControl] PDF加载成功");
                        // PageLoaded事件将在CefSharp控件内部触发
                        return true;
                    }
                    else
                    {
                        LogHelper.Error("[PdfPreviewControl] PDF加载失败");
                        OnLoadError(new Exception("PDF加载失败"));
                        return false;
                    }
                }
                else
                {
                    LogHelper.Error("[PdfPreviewControl] CefSharp控件未初始化");
                    OnLoadError(new Exception("PDF预览组件未初始化"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[PdfPreviewControl] 加载PDF异常: {ex.Message}");
                OnLoadError(ex);
                return false;
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// 跳转到指定页面
        /// </summary>
        public void GoToPage(int pageIndex)
        {
            if (_cefControl != null && pageIndex >= 0)
            {
                _ = _cefControl.GoToPage(pageIndex + 1);
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        public void NextPage()
        {
            _cefControl?.NextPage();
        }

        /// <summary>
        /// 上一页
        /// </summary>
        public void PreviousPage()
        {
            _cefControl?.PreviousPage();
        }

        /// <summary>
        /// 第一页
        /// </summary>
        public void FirstPage()
        {
            _cefControl?.FirstPage();
        }

        /// <summary>
        /// 最后一页
        /// </summary>
        public void LastPage()
        {
            _cefControl?.LastPage();
        }

        /// <summary>
        /// 放大（由CefSharp原生处理，此处仅保持兼容）
        /// </summary>
        public void ZoomIn()
        {
            // CefSharp使用原生缩放功能
            // 可以通过JavaScript注入实现，但通常不需要
            LogHelper.Debug("[PdfPreviewControl] 缩放功能由CefSharp原生提供");
        }

        /// <summary>
        /// 缩小（由CefSharp原生处理，此处仅保持兼容）
        /// </summary>
        public void ZoomOut()
        {
            // CefSharp使用原生缩放功能
            LogHelper.Debug("[PdfPreviewControl] 缩放功能由CefSharp原生提供");
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearCache()
        {
            try
            {
                // 清理CefSharp缓存
                CefSharpInitializer.ClearCache();
                LogHelper.Debug("[PdfPreviewControl] 缓存已清除");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[PdfPreviewControl] 清除缓存异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 计算适应页面宽度的缩放百分比（保持兼容，但实际由CefSharp处理）
        /// </summary>
        public float CalculateFitToWidthZoom()
        {
            // CefSharp自动处理适应页面
            return 100f;
        }

        /// <summary>
        /// 计算适应页面高度的缩放百分比（保持兼容，但实际由CefSharp处理）
        /// </summary>
        public float CalculateFitToHeightZoom()
        {
            // CefSharp自动处理适应页面
            return 100f;
        }

        /// <summary>
        /// 计算自动最优适应的缩放百分比（保持兼容，但实际由CefSharp处理）
        /// </summary>
        public float CalculateBestFitZoom()
        {
            // CefSharp默认使用page-fit模式
            return 100f;
        }

        /// <summary>
        /// 应用自动最优适应缩放（保持兼容，但实际由CefSharp处理）
        /// </summary>
        public void ApplyBestFit()
        {
            // CefSharp已在初始化时设置为page-fit模式
            LogHelper.Debug("[PdfPreviewControl] CefSharp已设置为适应页面模式");
        }

        /// <summary>
        /// 刷新当前页面
        /// </summary>
        public new void Refresh()
        {
            _cefControl?.Refresh();
        }

        #endregion

        #region 事件重写

        /// <summary>
        /// 控件大小改变时的事件处理
        /// CefSharp会自动处理调整大小
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // CefSharp自动处理页面适应，无需额外操作
        }

        #endregion

        #region 事件触发

        protected virtual void OnPageLoaded(int pageIndex, int pageCount)
        {
            PageLoaded?.Invoke(this, new PageLoadedEventArgs { PageIndex = pageIndex, PageCount = pageCount });
        }

        protected virtual void OnLoadError(Exception exception)
        {
            LoadError?.Invoke(this, new ErrorEventArgs(exception));
        }

        #endregion

        #region 资源释放

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    _cefControl?.Dispose();
                }
                catch { }
            }
            base.Dispose(disposing);
        }

        #endregion
    }

    #region 事件参数类

    /// <summary>
    /// 页面加载完成事件参数
    /// </summary>
    public class PageLoadedEventArgs : EventArgs
    {
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
    }

    #endregion
}