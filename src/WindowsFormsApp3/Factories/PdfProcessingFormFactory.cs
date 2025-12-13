using System;
using WindowsFormsApp3.Presenters;

namespace WindowsFormsApp3.Factories
{
    /// <summary>
    /// PDF处理窗体工厂 - 负责创建和配置MVP模式的组件
    /// 提供统一的创建入口，便于依赖注入和测试
    /// </summary>
    public static class PdfProcessingFormFactory
    {
        /// <summary>
        /// 创建PDF处理窗体（使用默认配置）
        /// </summary>
        /// <returns>配置好的PDF处理窗体</returns>
        public static PdfProcessingForm CreateForm()
        {
            // 创建Presenter
            var presenter = new PdfProcessingPresenter();

            // 创建View并注入Presenter
            var form = new PdfProcessingForm(presenter);

            return form;
        }

        /// <summary>
        /// 创建PDF处理窗体（传入初始PDF文件路径）
        /// </summary>
        /// <param name="initialFilePath">初始PDF文件路径</param>
        /// <returns>配置好的PDF处理窗体</returns>
        public static PdfProcessingForm CreateForm(string initialFilePath)
        {
            var form = CreateForm();

            // 如果提供了初始文件路径，设置到窗体
            if (!string.IsNullOrEmpty(initialFilePath))
            {
                // 这里需要通过反射或公共方法来设置文件路径
                // 为了简单起见，我们假设窗体有一个SetInitialFilePath方法
                // form.SetInitialFilePath(initialFilePath);
            }

            return form;
        }

        /// <summary>
        /// 创建PDF处理窗体（用于依赖注入）
        /// </summary>
        /// <param name="presenter">PDF处理Presenter</param>
        /// <returns>配置好的PDF处理窗体</returns>
        public static PdfProcessingForm CreateForm(PdfProcessingPresenter presenter)
        {
            if (presenter == null)
                throw new ArgumentNullException(nameof(presenter));

            return new PdfProcessingForm(presenter);
        }

        /// <summary>
        /// 创建PDF处理窗体并显示为对话框
        /// </summary>
        /// <param name="owner">父窗体</param>
        /// <returns>对话框结果</returns>
        public static System.Windows.Forms.DialogResult ShowDialog(System.Windows.Forms.Form owner = null)
        {
            using (var form = CreateForm())
            {
                return form.ShowDialog(owner);
            }
        }

        /// <summary>
        /// 创建PDF处理窗体并显示为对话框（传入初始文件）
        /// </summary>
        /// <param name="initialFilePath">初始PDF文件路径</param>
        /// <param name="owner">父窗体</param>
        /// <returns>对话框结果</returns>
        public static System.Windows.Forms.DialogResult ShowDialog(string initialFilePath, System.Windows.Forms.Form owner = null)
        {
            using (var form = CreateForm(initialFilePath))
            {
                return form.ShowDialog(owner);
            }
        }

        /// <summary>
        /// 创建PDF处理窗体并显示为非模态窗口
        /// </summary>
        /// <param name="owner">父窗体</param>
        /// <returns>创建的窗体实例</returns>
        public static PdfProcessingForm Show(System.Windows.Forms.Form owner = null)
        {
            var form = CreateForm();
            form.Show(owner);
            return form;
        }

        /// <summary>
        /// 创建PDF处理窗体并显示为非模态窗口（传入初始文件）
        /// </summary>
        /// <param name="initialFilePath">初始PDF文件路径</param>
        /// <param name="owner">父窗体</param>
        /// <returns>创建的窗体实例</returns>
        public static PdfProcessingForm Show(string initialFilePath, System.Windows.Forms.Form owner = null)
        {
            var form = CreateForm(initialFilePath);
            form.Show(owner);
            return form;
        }
    }

    /// <summary>
    /// PDF处理窗体配置选项
    /// </summary>
    public class PdfProcessingFormOptions
    {
        /// <summary>
        /// 初始PDF文件路径
        /// </summary>
        public string InitialFilePath { get; set; }

        /// <summary>
        /// 是否启用详细日志
        /// </summary>
        public bool EnableDetailedLogging { get; set; } = true;

        /// <summary>
        /// 首选PDF处理库
        /// </summary>
        public string PreferredLibrary { get; set; } = "IText7";

        /// <summary>
        /// 是否启用自动错误恢复
        /// </summary>
        public bool EnableErrorRecovery { get; set; } = true;

        /// <summary>
        /// 最大重试次数
        /// </summary>
        public int MaxRetryCount { get; set; } = 3;

        /// <summary>
        /// 重试延迟（毫秒）
        /// </summary>
        public int RetryDelayMs { get; set; } = 100;

        /// <summary>
        /// 窗体标题
        /// </summary>
        public string FormTitle { get; set; } = "PDF处理工具 - iText7";

        /// <summary>
        /// 窗体起始位置
        /// </summary>
        public System.Windows.Forms.FormStartPosition StartPosition { get; set; } = System.Windows.Forms.FormStartPosition.CenterParent;

        /// <summary>
        /// 窗体大小
        /// </summary>
        public System.Drawing.Size FormSize { get; set; } = new System.Drawing.Size(800, 600);

        /// <summary>
        /// 窗体最小尺寸
        /// </summary>
        public System.Drawing.Size MinimumFormSize { get; set; } = new System.Drawing.Size(600, 400);

        /// <summary>
        /// 创建默认配置
        /// </summary>
        /// <returns>默认配置选项</returns>
        public static PdfProcessingFormOptions CreateDefault()
        {
            return new PdfProcessingFormOptions();
        }

        /// <summary>
        /// 创建用于快速处理的配置
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>快速处理配置选项</returns>
        public static PdfProcessingFormOptions CreateForQuickProcessing(string filePath)
        {
            return new PdfProcessingFormOptions
            {
                InitialFilePath = filePath,
                EnableDetailedLogging = false,
                MaxRetryCount = 1,
                RetryDelayMs = 50,
                FormTitle = "快速PDF处理",
                FormSize = new System.Drawing.Size(700, 500)
            };
        }

        /// <summary>
        /// 创建用于详细分析的配置
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <returns>详细分析配置选项</returns>
        public static PdfProcessingFormOptions CreateForDetailedAnalysis(string filePath)
        {
            return new PdfProcessingFormOptions
            {
                InitialFilePath = filePath,
                EnableDetailedLogging = true,
                MaxRetryCount = 5,
                RetryDelayMs = 200,
                EnableErrorRecovery = true,
                FormTitle = "PDF详细分析工具",
                FormSize = new System.Drawing.Size(900, 700)
            };
        }
    }

    /// <summary>
    /// 扩展的PDF处理窗体工厂 - 支持配置选项
    /// </summary>
    public static class PdfProcessingFormFactoryExtended
    {
        /// <summary>
        /// 使用配置选项创建PDF处理窗体
        /// </summary>
        /// <param name="options">配置选项</param>
        /// <returns>配置好的PDF处理窗体</returns>
        public static PdfProcessingForm CreateForm(PdfProcessingFormOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            // 创建Presenter
            var presenter = new PdfProcessingPresenter();

            // 创建View并注入Presenter
            var form = new PdfProcessingForm(presenter);

            // 应用配置选项
            ConfigureForm(form, options);

            return form;
        }

        /// <summary>
        /// 使用配置选项显示PDF处理窗体对话框
        /// </summary>
        /// <param name="options">配置选项</param>
        /// <param name="owner">父窗体</param>
        /// <returns>对话框结果</returns>
        public static System.Windows.Forms.DialogResult ShowDialog(PdfProcessingFormOptions options, System.Windows.Forms.Form owner = null)
        {
            using (var form = CreateForm(options))
            {
                return form.ShowDialog(owner);
            }
        }

        /// <summary>
        /// 使用配置选项显示PDF处理窗体
        /// </summary>
        /// <param name="options">配置选项</param>
        /// <param name="owner">父窗体</param>
        /// <returns>创建的窗体实例</returns>
        public static PdfProcessingForm Show(PdfProcessingFormOptions options, System.Windows.Forms.Form owner = null)
        {
            var form = CreateForm(options);
            form.Show(owner);
            return form;
        }

        /// <summary>
        /// 配置窗体属性
        /// </summary>
        /// <param name="form">PDF处理窗体</param>
        /// <param name="options">配置选项</param>
        private static void ConfigureForm(PdfProcessingForm form, PdfProcessingFormOptions options)
        {
            // 设置窗体基本属性
            form.Text = options.FormTitle;
            form.StartPosition = options.StartPosition;
            form.Size = options.FormSize;
            form.MinimumSize = options.MinimumFormSize;

            // 如果需要设置初始文件路径（假设窗体有相应方法）
            if (!string.IsNullOrEmpty(options.InitialFilePath))
            {
                // form.SetInitialFilePath(options.InitialFilePath);
            }

            // 应用其他配置选项（假设窗体有相应方法）
            // form.ApplyOptions(options);
        }
    }
}