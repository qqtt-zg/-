using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp3.Models;
using WindowsFormsApp3.Forms.Dialogs;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 批量处理服务接口，负责协调和管理多个文件的批量处理操作
    /// 提供文件批量重命名、复制、移动等功能，支持异步处理和进度报告
    /// </summary>
    public interface IBatchProcessingService
    {
        /// <summary>
        /// 批量处理进度事件，在处理过程中定期触发
        /// </summary>
        event EventHandler<BatchProgressEventArgs> ProgressChanged;

        /// <summary>
        /// 批量处理完成事件，在处理结束（正常完成、取消或失败）时触发
        /// </summary>
        event EventHandler<BatchCompleteEventArgs> ProcessingComplete;

        /// <summary>
        /// 开始批量处理文件
        /// </summary>
        /// <param name="fileInfos">包含文件信息和重命名规则的文件列表</param>
        /// <param name="exportPath">处理后文件的导出路径</param>
        /// <param name="isCopyMode">是否为复制模式（true为复制，false为重命名原文件）</param>
        /// <param name="batchSize">每批处理的文件数量</param>
        /// <param name="maxDegreeOfParallelism">最大并行处理数量</param>
        /// <returns>表示异步操作的Task</returns>
        /// <exception cref="ArgumentNullException">当fileInfos为null时抛出</exception>
        /// <exception cref="ArgumentException">当exportPath为空或无效时抛出</exception>
        /// <exception cref="InvalidOperationException">当服务内部状态异常时抛出</exception>
        Task StartBatchProcessingAsync(
            List<FileRenameInfo> fileInfos,
            string exportPath,
            bool isCopyMode = false,
            int batchSize = 10,
            int maxDegreeOfParallelism = 4);

        /// <summary>
        /// 取消正在进行的批量处理操作
        /// </summary>
        void CancelProcessing();

        /// <summary>
        /// 设置保留分组配置
        /// </summary>
        /// <param name="preserveGroupConfigs">保留的分组配置列表</param>
        void SetPreserveGroupConfigs(List<EventGroupConfig> preserveGroupConfigs);

        /// <summary>
        /// 应用保留模式到文件列表
        /// </summary>
        /// <param name="fileInfos">文件信息列表</param>
        void ApplyPreserveModeToFileList(List<FileRenameInfo> fileInfos);

        /// <summary>
        /// 恢复保留字段的原始数据
        /// </summary>
        /// <param name="fileInfos">文件信息列表</param>
        void RestorePreservedFields(List<FileRenameInfo> fileInfos);

        /// <summary>
        /// 处理文件列表的异步方法
        /// </summary>
        /// <param name="fileInfos">要处理的文件信息列表</param>
        /// <param name="exportPath">导出路径</param>
        /// <param name="isCopyMode">是否为复制模式</param>
        /// <param name="batchSize">批次大小</param>
        /// <param name="maxDegreeOfParallelism">最大并行度</param>
        /// <param name="progress">进度报告</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>批量处理结果</returns>
        Task<WindowsFormsApp3.Models.BatchProcessResult> ProcessFilesAsync(
            List<FileRenameInfo> fileInfos,
            string exportPath,
            bool isCopyMode = false,
            int batchSize = 10,
            int maxDegreeOfParallelism = -1,
            IProgress<WindowsFormsApp3.Models.BatchProgress> progress = null,
            CancellationToken cancellationToken = default);
    }
}