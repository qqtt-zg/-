using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spire.Pdf;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Controls
{
    /// <summary>
    /// 现代化 PDF 预览控件
    /// 使用轻量级方案实现预览，支持实时加载和缓存清除
    /// 后续可集成更完整的 PDF 渲染引擎
    /// </summary>
    public class PdfPreviewControl : Panel
    {
        #region 私有字段

        private int _currentPageIndex = 0;
        private float _currentZoom = 100f;
        private string _currentFilePath;
        private bool _isLoading = false;
        private PictureBox _pictureBox;
        private int _totalPages = 0;
        
        // ✅ 缓存原始 PDF 页面的真实尺寸（未经任何缩放），用于精确的缩放计算
        private int _originalPageWidth = 0;
        private int _originalPageHeight = 0;

        #endregion

        #region 公共属性

        /// <summary>
        /// 当前显示的页面索引（从0开始）
        /// </summary>
        public int CurrentPageIndex
        {
            get => _currentPageIndex;
            set
            {
                if (value >= 0 && value < _totalPages)
                {
                    _currentPageIndex = value;
                    RefreshCurrentPage();
                }
            }
        }

        /// <summary>
        /// PDF 文档总页数
        /// </summary>
        public int PageCount => _totalPages;

        /// <summary>
        /// 当前缩放百分比
        /// </summary>
        public float CurrentZoom
        {
            get => _currentZoom;
            set
            {
                // ✅ 修复：确保值总是有效并且在有效范围内
                float validZoom = Math.Max(10, Math.Min(400, value));
                
                if (Math.Abs(_currentZoom - validZoom) > 0.01f) // 允许一个小的容差
                {
                    _currentZoom = validZoom;
                    LogHelper.Debug($"[PdfPreviewControl] 缩放字段已更新: {_currentZoom:F0}%");
                    RefreshCurrentPage();
                }
                else
                {
                    LogHelper.Debug($"[PdfPreviewControl] 缩放月所改变（当前: {_currentZoom:F0}%, 新值: {validZoom:F0}%）");
                }
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

        public PdfPreviewControl()
        {
            InitializeUI();
        }

        #region UI 初始化

        private void InitializeUI()
        {
            this.BackColor = Color.FromArgb(248, 248, 248); // 更浅的灰色背景，便于看出PDF内容边界
            this.Dock = DockStyle.Fill;
            this.BorderStyle = BorderStyle.None;
            this.Padding = new Padding(0);
            this.DoubleBuffered = true;

            // 创建 PictureBox 用于昺示预览
            _pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.CenterImage,
                BackColor = Color.FromArgb(248, 248, 248), // 更浅的灰色背景
                BorderStyle = BorderStyle.None
            };
            this.Controls.Add(_pictureBox);

            LogHelper.Debug("[PdfPreviewControl] UI 初始化完成");
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 异步加载 PDF 文件
        /// 实时加载，禁止显示缓存的旧内容
        /// </summary>
        public async Task<bool> LoadPdfAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                LogHelper.Error($"[PdfPreviewControl] PDF 文件不存在: {filePath}");
                return false;
            }

            try
            {
                _isLoading = true;

                // 在后台线程加载 PDF
                var loadTask = Task.Run(() =>
                {
                    try
                    {
                        // 通过 iText7 获取页数
                        using (var reader = new iText.Kernel.Pdf.PdfReader(filePath))
                        using (var doc = new iText.Kernel.Pdf.PdfDocument(reader))
                        {
                            _totalPages = doc.GetNumberOfPages();
                        }

                        LogHelper.Debug($"[PdfPreviewControl] PDF 加载成功: {filePath}, 共 {_totalPages} 页");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error($"[PdfPreviewControl] PDF 加载异常: {ex.Message}");
                        return false;
                    }
                });

                bool success = await loadTask;

                if (success)
                {
                    _currentFilePath = filePath;
                    _currentPageIndex = 0;
                    
                    // ✅ 优化：并行处理页面加载和缩放计算
                    // 立即计算缩放参数，不等待渲染完成
                    float initialZoom = CalculateFitToHeightZoom();
                    LogHelper.Debug($"[PdfPreviewControl] 预计算适应高度缩放: {initialZoom:F0}%");
                    
                    // ✅ 修复：正确等待 PDF 渲染完成后再应用缩放
                    // 在后台异步执行：加载页面 + 应用缩放
                    _ = Task.Run(async () =>
                    {
                        // 1. 在后台线程中渲染 PDF 页面（阻塞操作）
                        await RefreshCurrentPageAsync();
                        
                        // 2. 回到 UI 线程应用缩放和触发事件
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                CurrentZoom = initialZoom;
                                LogHelper.Debug($"[PdfPreviewControl] ✅ 应用适应高度缩放: {initialZoom:F0}%");
                                OnPageLoaded(_currentPageIndex, PageCount);
                            }));
                        }
                        else
                        {
                            CurrentZoom = initialZoom;
                            LogHelper.Debug($"[PdfPreviewControl] ✅ 应用适应高度缩放: {initialZoom:F0}%");
                            OnPageLoaded(_currentPageIndex, PageCount);
                        }
                    });

                    return true;
                }
                else
                {
                    OnLoadError(new Exception("PDF 加载失败"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[PdfPreviewControl] 加载 PDF 异常: {ex.Message}");
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
            if (pageIndex >= 0 && pageIndex < _totalPages)
            {
                CurrentPageIndex = pageIndex;
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        public void NextPage()
        {
            if (_currentPageIndex < _totalPages - 1)
            {
                CurrentPageIndex++;
            }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        public void PreviousPage()
        {
            if (_currentPageIndex > 0)
            {
                CurrentPageIndex--;
            }
        }

        /// <summary>
        /// 放大
        /// </summary>
        public void ZoomIn()
        {
            CurrentZoom = Math.Min(400, _currentZoom * 1.2f);
        }

        /// <summary>
        /// 缩小
        /// </summary>
        public void ZoomOut()
        {
            CurrentZoom = Math.Max(10, _currentZoom / 1.2f);
        }

        /// <summary>
        /// 清除缓存（用于禁止显示旧内容）
        /// </summary>
        public void ClearCache()
        {
            try
            {
                _currentFilePath = null;
                _currentPageIndex = 0;
                _totalPages = 0;
                _pictureBox.Image = null;
                // ✅ 清除缓存的原始页面尺寸
                _originalPageWidth = 0;
                _originalPageHeight = 0;
                LogHelper.Debug("[PdfPreviewControl] 缓存已清除");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[PdfPreviewControl] 清除缓存异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 计算适应页面宽度的缩放百分比
        /// 根据已渲染的图片宽度和控件宽度计算
        /// </summary>
        public float CalculateFitToWidthZoom()
        {
            try
            {
                // ✅ 修复：使用原始 PDF 页面的真实尺寸（而不是当前显示的缩放图片尺寸）
                if (_originalPageWidth <= 0 || this.Width <= 0)
                {
                    LogHelper.Debug("[PdfPreviewControl] 无法计算适应宽度缩放（原始页面未加载或控件宽度为0）");
                    return 100f;
                }

                // 原始页面宽度和控件可用宽度（减去 Padding，并保留3像素边缘底色）
                float availableWidth = this.Width - this.Padding.Left - this.Padding.Right - 6; // 左右各保留3像素

                if (availableWidth <= 0)
                {
                    return 100f;
                }

                // 计算缩放百分比
                float zoomPercent = (availableWidth / _originalPageWidth) * 100f;
                float finalZoom = Math.Max(10, Math.Min(400, zoomPercent)); // 限制在 10-400% 范围内

                LogHelper.Debug($"[PdfPreviewControl] ✅ 计算适应宽度缩放: 原始宽度={_originalPageWidth}, 可用宽度={availableWidth:F0}, 缩放={finalZoom:F0}%");
                return finalZoom;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[PdfPreviewControl] 计算适应宽度缩放异常: {ex.Message}");
                return 100f;
            }
        }

        /// <summary>
        /// 计算适应页面高度的缩放百分比
        /// 根据已渲染的图片高度和控件高度计算
        /// </summary>
        public float CalculateFitToHeightZoom()
        {
            try
            {
                // ✅ 修复：使用原始 PDF 页面的真实尺寸（而不是当前显示的缩放图片尺寸）
                if (_originalPageHeight <= 0 || this.Height <= 0)
                {
                    LogHelper.Debug("[PdfPreviewControl] 无法计算适应高度缩放（原始页面未加载或控件高度为0）");
                    return 100f;
                }

                // 原始页面高度和控件可用高度（减去 Padding，并保留3像素边缘底色）
                float availableHeight = this.Height - this.Padding.Top - this.Padding.Bottom - 6; // 上下各保留3像素

                if (availableHeight <= 0)
                {
                    return 100f;
                }

                // 计算缩放百分比
                float zoomPercent = (availableHeight / _originalPageHeight) * 100f;
                float finalZoom = Math.Max(10, Math.Min(400, zoomPercent)); // 限制在 10-400% 范围内

                LogHelper.Debug($"[PdfPreviewControl] ✅ 计算适应高度缩放: 原始高度={_originalPageHeight}, 可用高度={availableHeight:F0}, 缩放={finalZoom:F0}%");
                return finalZoom;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[PdfPreviewControl] 计算适应高度缩放异常: {ex.Message}");
                return 100f;
            }
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 刷新当前页面显示
        /// 使用 Spire.Pdf 渲染真实 PDF 页面
        /// </summary>
        private void RefreshCurrentPage()
        {
            try
            {
                if (string.IsNullOrEmpty(_currentFilePath) || _totalPages == 0)
                {
                    LogHelper.Warn("[PdfPreviewControl] 没有加载的 PDF 文件");
                    return;
                }

                // 在后台线程中进行 PDF 渲染，避免阻塞 UI
                Task.Run(() =>
                {
                    try
                    {
                        using (PdfDocument document = new PdfDocument(_currentFilePath))
                        {
                            if (_currentPageIndex >= 0 && _currentPageIndex < document.Pages.Count)
                            {
                                // ✅ 将指定页面渲染为高质量图片
                                // 使用 DPI 150 平衡质量与性能
                                Image pageImage = document.SaveAsImage(_currentPageIndex, 150, 150);

                                if (pageImage != null)
                                {
                                    Bitmap pageBitmap = pageImage as Bitmap;
                                    if (pageBitmap == null)
                                    {
                                        // 根据需要转换为 Bitmap
                                        pageBitmap = new Bitmap(pageImage);
                                        pageImage.Dispose();
                                    }
                                    
                                    // ✅ 缓存原始页面尺寸（仅存一次）
                                    if (_originalPageWidth == 0 || _originalPageHeight == 0)
                                    {
                                        _originalPageWidth = pageBitmap.Width;
                                        _originalPageHeight = pageBitmap.Height;
                                        LogHelper.Debug($"[PdfPreviewControl] 缓存原始页面尺寸: 宽={_originalPageWidth}, 高={_originalPageHeight}");
                                    }
                                    
                                    // 应用缩放（在渲染后缩放可能影响质量，这里只做缩放计算）
                                    int scaledWidth = (int)(pageBitmap.Width * _currentZoom / 100f);
                                    int scaledHeight = (int)(pageBitmap.Height * _currentZoom / 100f);

                                    // 缩放图片（如果需要）
                                    if (Math.Abs(_currentZoom - 100f) > 0.1f)
                                    {
                                        Bitmap scaledBitmap = new Bitmap(pageBitmap, scaledWidth, scaledHeight);
                                        pageBitmap.Dispose();
                                        pageBitmap = scaledBitmap;
                                    }

                                    // 在 UI 线程更新显示
                                    if (InvokeRequired)
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            _pictureBox.Image?.Dispose();
                                            _pictureBox.Image = pageBitmap;
                                            LogHelper.Debug($"[PdfPreviewControl] ✅ 第 {_currentPageIndex + 1}/{_totalPages} 页已渲染，缩放: {_currentZoom:F0}%");
                                        }));
                                    }
                                    else
                                    {
                                        _pictureBox.Image?.Dispose();
                                        _pictureBox.Image = pageBitmap;
                                        LogHelper.Debug($"[PdfPreviewControl] ✅ 第 {_currentPageIndex + 1}/{_totalPages} 页已渲染，缩放: {_currentZoom:F0}%");
                                    }
                                }
                                else
                                {
                                    LogHelper.Error($"[PdfPreviewControl] 渲染第 {_currentPageIndex + 1} 页失败");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error($"[PdfPreviewControl] PDF 渲染异常: {ex.Message}");
                        OnLoadError(ex);
                    }
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[PdfPreviewControl] 刷新页面异常: {ex.Message}");
                OnLoadError(ex);
            }
        }

        /// <summary>
        /// ✅ 异步刷新当前页面 - 正确等待渲染完成
        /// </summary>
        private async Task RefreshCurrentPageAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_currentFilePath) || _totalPages == 0)
                {
                    LogHelper.Warn("[PdfPreviewControl] 没有加载的 PDF 文件");
                    return;
                }

                // ✅ 改为真正的异步渲染，返回 Task 以便上层正确等待
                await Task.Run(() =>
                {
                    try
                    {
                        using (PdfDocument document = new PdfDocument(_currentFilePath))
                        {
                            if (_currentPageIndex >= 0 && _currentPageIndex < document.Pages.Count)
                            {
                                // ✅ 将指定页面渲染为高质量图片
                                // 使用 DPI 150 平衡质量与性能
                                Image pageImage = document.SaveAsImage(_currentPageIndex, 150, 150);

                                if (pageImage != null)
                                {
                                    Bitmap pageBitmap = pageImage as Bitmap;
                                    if (pageBitmap == null)
                                    {
                                        // 根据需要转换为 Bitmap
                                        pageBitmap = new Bitmap(pageImage);
                                        pageImage.Dispose();
                                    }
                                    
                                    // ✅ 缓存原始页面尺寸（仅存一次）
                                    if (_originalPageWidth == 0 || _originalPageHeight == 0)
                                    {
                                        _originalPageWidth = pageBitmap.Width;
                                        _originalPageHeight = pageBitmap.Height;
                                        LogHelper.Debug($"[PdfPreviewControl] 缓存原始页面尺寸: 宽={_originalPageWidth}, 高={_originalPageHeight}");
                                    }
                                    
                                    // 应用缩放（在渲染后缩放可能影响质量，这里只做缩放计算）
                                    int scaledWidth = (int)(pageBitmap.Width * _currentZoom / 100f);
                                    int scaledHeight = (int)(pageBitmap.Height * _currentZoom / 100f);

                                    // 缩放图片（如果需要）
                                    if (Math.Abs(_currentZoom - 100f) > 0.1f)
                                    {
                                        Bitmap scaledBitmap = new Bitmap(pageBitmap, scaledWidth, scaledHeight);
                                        pageBitmap.Dispose();
                                        pageBitmap = scaledBitmap;
                                    }

                                    // 在 UI 线程更新显示
                                    if (InvokeRequired)
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            _pictureBox.Image?.Dispose();
                                            _pictureBox.Image = pageBitmap;
                                            LogHelper.Debug($"[PdfPreviewControl] ✅ 第 {_currentPageIndex + 1}/{_totalPages} 页已渲染，缩放: {_currentZoom:F0}%");
                                        }));
                                    }
                                    else
                                    {
                                        _pictureBox.Image?.Dispose();
                                        _pictureBox.Image = pageBitmap;
                                        LogHelper.Debug($"[PdfPreviewControl] ✅ 第 {_currentPageIndex + 1}/{_totalPages} 页已渲染，缩放: {_currentZoom:F0}%");
                                    }
                                }
                                else
                                {
                                    LogHelper.Error($"[PdfPreviewControl] 渲染第 {_currentPageIndex + 1} 页失败");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error($"[PdfPreviewControl] PDF 渲染异常: {ex.Message}");
                        OnLoadError(ex);
                    }
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[PdfPreviewControl] 异步刷新页面异常: {ex.Message}");
                OnLoadError(ex);
            }
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
                    _pictureBox?.Image?.Dispose();
                    _pictureBox?.Dispose();
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
