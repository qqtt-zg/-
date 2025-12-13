using System;
using System.Collections.Generic;
using WindowsFormsApp3.Models;

namespace WindowsFormsApp3.Interfaces
{
    /// <summary>
    /// PDF处理视图接口 - MVP模式的View层
    /// 定义视图需要实现的方法和事件，供Presenter调用
    /// </summary>
    public interface IPdfProcessingView
    {
        #region 事件 - View向Presenter通知用户操作

        /// <summary>
        /// 当用户请求处理PDF文件时触发
        /// </summary>
        event EventHandler<string> ProcessPdfRequested;

        /// <summary>
        /// 当用户请求获取页面信息时触发
        /// </summary>
        event EventHandler<string> GetPageInfoRequested;

        /// <summary>
        /// 当用户请求比较PDF库时触发
        /// </summary>
        event EventHandler<string> CompareLibrariesRequested;

        /// <summary>
        /// 当用户请求生成报告时触发
        /// </summary>
        event EventHandler<string> GenerateReportRequested;

        /// <summary>
        /// 当用户取消处理时触发
        /// </summary>
        event EventHandler<string> CancelProcessingRequested;

        /// <summary>
        /// 当用户更新配置时触发
        /// </summary>
        event EventHandler<PdfProcessingConfig> ConfigUpdated;

        #endregion

        #region 显示方法 - Presenter调用View更新UI

        /// <summary>
        /// 显示PDF处理结果
        /// </summary>
        /// <param name="response">处理响应</param>
        void ShowProcessingResult(PdfProcessingResponse response);

        /// <summary>
        /// 显示PDF文件信息
        /// </summary>
        /// <param name="fileInfo">PDF文件信息</param>
        void ShowPdfFileInfo(PdfFileInfo fileInfo);

        /// <summary>
        /// 显示处理进度
        /// </summary>
        /// <param name="progress">进度信息</param>
        void ShowProcessingProgress(PdfProcessingProgress progress);

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="title">错误标题</param>
        void ShowError(string message, string title = "错误");

        /// <summary>
        /// 显示警告信息
        /// </summary>
        /// <param name="message">警告消息</param>
        /// <param name="title">警告标题</param>
        void ShowWarning(string message, string title = "警告");

        /// <summary>
        /// 显示成功信息
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <param name="title">成功标题</param>
        void ShowSuccess(string message, string title = "成功");

        /// <summary>
        /// 显示PDF库比较结果
        /// </summary>
        /// <param name="comparison">比较结果</param>
        void ShowLibraryComparison(PdfLibraryComparisonResult comparison);

        /// <summary>
        /// 显示处理报告
        /// </summary>
        /// <param name="report">报告内容</param>
        void ShowProcessingReport(string report);

        /// <summary>
        /// 显示统计信息
        /// </summary>
        /// <param name="statistics">统计信息</param>
        void ShowStatistics(PdfProcessingStatistics statistics);

        #endregion

        #region 状态控制方法 - Presenter控制View状态

        /// <summary>
        /// 设置处理状态
        /// </summary>
        /// <param name="isProcessing">是否正在处理</param>
        void SetProcessingState(bool isProcessing);

        /// <summary>
        /// 启用/禁用处理按钮
        /// </summary>
        /// <param name="enabled">是否启用</param>
        void SetProcessingButtonsEnabled(bool enabled);

        /// <summary>
        /// 启用/禁用取消按钮
        /// </summary>
        /// <param name="enabled">是否启用</param>
        void SetCancelButtonEnabled(bool enabled);

        /// <summary>
        /// 设置进度条可见性
        /// </summary>
        /// <param name="visible">是否可见</param>
        void SetProgressBarVisible(bool visible);

        /// <summary>
        /// 更新进度条值
        /// </summary>
        /// <param name="percentage">进度百分比</param>
        void UpdateProgressBar(int percentage);

        /// <summary>
        /// 清除结果显示
        /// </summary>
        void ClearResults();

        /// <summary>
        /// 设置加载状态
        /// </summary>
        /// <param name="isLoading">是否正在加载</param>
        void SetLoadingState(bool isLoading);

        #endregion

        #region 数据获取方法 - Presenter从View获取数据

        /// <summary>
        /// 获取当前选择的PDF文件路径
        /// </summary>
        /// <returns>PDF文件路径</returns>
        string GetSelectedPdfFilePath();

        /// <summary>
        /// 获取当前处理配置
        /// </summary>
        /// <returns>处理配置</returns>
        PdfProcessingConfig GetCurrentConfig();

        /// <summary>
        /// 获取用户输入的处理参数
        /// </summary>
        /// <returns>参数字典</returns>
        Dictionary<string, object> GetProcessingParameters();

        /// <summary>
        /// 检查是否有正在进行的处理
        /// </summary>
        /// <returns>是否有正在进行的处理</returns>
        bool HasActiveProcessing();

        #endregion

        #region 配置管理方法

        /// <summary>
        /// 加载配置到界面
        /// </summary>
        /// <param name="config">配置对象</param>
        void LoadConfig(PdfProcessingConfig config);

        /// <summary>
        /// 从界面保存配置
        /// </summary>
        /// <returns>配置对象</returns>
        PdfProcessingConfig SaveConfig();

        /// <summary>
        /// 重置配置为默认值
        /// </summary>
        void ResetConfigToDefault();

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        /// <param name="config">要验证的配置</param>
        /// <returns>验证结果</returns>
        PdfProcessingValidationResult ValidateConfig(PdfProcessingConfig config);

        #endregion

        #region UI交互方法

        /// <summary>
        /// 请求用户选择PDF文件
        /// </summary>
        /// <returns>选择的文件路径，如果取消则返回null</returns>
        string RequestPdfFileSelection();

        /// <summary>
        /// 请求用户确认操作
        /// </summary>
        /// <param name="message">确认消息</param>
        /// <param name="title">标题</param>
        /// <returns>用户确认结果</returns>
        bool RequestConfirmation(string message, string title = "确认");

        /// <summary>
        /// 请求用户输入
        /// </summary>
        /// <param name="prompt">提示信息</param>
        /// <param name="title">标题</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>用户输入值</returns>
        string RequestUserInput(string prompt, string title = "输入", string defaultValue = "");

        /// <summary>
        /// 显示文件选择对话框
        /// </summary>
        /// <param name="filter">文件过滤器</param>
        /// <param name="title">对话框标题</param>
        /// <returns>选择的文件路径</returns>
        string ShowFileDialog(string filter = "PDF文件|*.pdf", string title = "选择PDF文件");

        #endregion

        #region 日志和调试方法

        /// <summary>
        /// 添加日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="level">日志级别</param>
        void AddLogMessage(string message, LogLevel level = LogLevel.Information);

        /// <summary>
        /// 清除日志
        /// </summary>
        void ClearLog();

        /// <summary>
        /// 刷新日志显示
        /// </summary>
        void RefreshLog();

        #endregion
    }
}