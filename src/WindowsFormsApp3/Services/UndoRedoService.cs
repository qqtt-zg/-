using System;
using System.Collections.Generic;
using WindowsFormsApp3.Commands;
using WindowsFormsApp3.Interfaces;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 撤销/重做服务接口
    /// </summary>
    public interface IUndoRedoService
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">要执行的命令</param>
        void ExecuteCommand(CommandBase command);

        /// <summary>
        /// 撤销命令
        /// </summary>
        /// <returns>撤销的命令描述</returns>
        string Undo();

        /// <summary>
        /// 重做命令
        /// </summary>
        /// <returns>重做的命令描述</returns>
        string Redo();

        /// <summary>
        /// 清空历史记录
        /// </summary>
        void ClearHistory();

        /// <summary>
        /// 检查是否可以撤销
        /// </summary>
        bool CanUndo();

        /// <summary>
        /// 检查是否可以重做
        /// </summary>
        bool CanRedo();

        /// <summary>
        /// 获取可以撤销的命令数量
        /// </summary>
        int CanUndoCount { get; }

        /// <summary>
        /// 获取可以重做的命令数量
        /// </summary>
        int CanRedoCount { get; }

        /// <summary>
        /// 获取撤销历史
        /// </summary>
        List<string> GetUndoHistory();

        /// <summary>
        /// 获取重做历史
        /// </summary>
        List<string> GetRedoHistory();

        /// <summary>
        /// 状态变化事件
        /// </summary>
        event EventHandler<UndoRedoStateChangedEventArgs> StateChanged;
    }

    /// <summary>
    /// 撤销/重做服务实现
    /// </summary>
    public class UndoRedoService : IUndoRedoService
    {
        private readonly UndoRedoManager _undoRedoManager;
        protected readonly Interfaces.ILogger _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志服务</param>
        /// <param name="maxHistorySize">最大历史记录大小</param>
        public UndoRedoService(Interfaces.ILogger logger, int maxHistorySize = 100)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _undoRedoManager = new UndoRedoManager(maxHistorySize);
            _undoRedoManager.StateChanged += OnUndoRedoStateChanged;
        }

        /// <summary>
        /// 可以撤销的命令数量
        /// </summary>
        public int CanUndoCount => _undoRedoManager.CanUndoCount;

        /// <summary>
        /// 可以重做的命令数量
        /// </summary>
        public int CanRedoCount => _undoRedoManager.CanRedoCount;

        /// <summary>
        /// 状态变化事件
        /// </summary>
        public event EventHandler<UndoRedoStateChangedEventArgs> StateChanged;

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">要执行的命令</param>
        public virtual void ExecuteCommand(CommandBase command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            try
            {
                _logger.LogDebug($"执行命令: {command.Description}");
                _undoRedoManager.ExecuteCommand(command);
                _logger.LogDebug($"命令执行成功: {command.Description}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"命令执行失败: {command.Description} - {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 撤销命令
        /// </summary>
        /// <returns>撤销的命令描述</returns>
        public virtual string Undo()
        {
            if (!CanUndo())
                return null;

            try
            {
                string result = _undoRedoManager.Undo();
                _logger.LogDebug($"撤销命令: {result}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"撤销命令失败 - {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 重做命令
        /// </summary>
        /// <returns>重做的命令描述</returns>
        public virtual string Redo()
        {
            if (!CanRedo())
                return null;

            try
            {
                string result = _undoRedoManager.Redo();
                _logger.LogDebug($"重做命令: {result}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"重做命令失败 - {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 清空历史记录
        /// </summary>
        public void ClearHistory()
        {
            try
            {
                _undoRedoManager.ClearHistory();
                _logger.LogDebug("清空撤销/重做历史记录");
            }
            catch (Exception ex)
            {
                _logger.LogError($"清空历史记录失败 - {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 检查是否可以撤销
        /// </summary>
        public bool CanUndo()
        {
            return _undoRedoManager.CanUndo();
        }

        /// <summary>
        /// 检查是否可以重做
        /// </summary>
        public bool CanRedo()
        {
            return _undoRedoManager.CanRedo();
        }

        /// <summary>
        /// 获取撤销历史
        /// </summary>
        public List<string> GetUndoHistory()
        {
            return new List<string>(_undoRedoManager.GetUndoHistory());
        }

        /// <summary>
        /// 获取重做历史
        /// </summary>
        public List<string> GetRedoHistory()
        {
            return new List<string>(_undoRedoManager.GetRedoHistory());
        }

        /// <summary>
        /// 创建文件重命名命令并执行
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <param name="overwrite">是否覆盖现有文件</param>
        public void ExecuteFileRename(string sourcePath, string targetPath, bool overwrite = false)
        {
            var command = (CommandBase)(object)new FileRenameCommand(sourcePath, targetPath, overwrite);
            ExecuteCommand(command);
        }

        /// <summary>
        /// 创建批量文件重命名命令并执行
        /// </summary>
        /// <param name="operations">重命名操作列表</param>
        public void ExecuteBatchFileRename(BatchFileRenameCommand.FileRenameOperation[] operations)
        {
            var command = (CommandBase)(object)new BatchFileRenameCommand(operations);
            ExecuteCommand(command);
        }

        /// <summary>
        /// 创建文件复制命令并执行
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <param name="overwrite">是否覆盖现有文件</param>
        public void ExecuteFileCopy(string sourcePath, string targetPath, bool overwrite = false)
        {
            var command = (CommandBase)(object)new FileCopyCommand(sourcePath, targetPath, overwrite);
            ExecuteCommand(command);
        }

        /// <summary>
        /// 创建文件删除命令并执行
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="moveToRecycleBin">是否移动到回收站</param>
        public void ExecuteFileDelete(string filePath, bool moveToRecycleBin = true)
        {
            var command = (CommandBase)(object)new FileDeleteCommand(filePath, moveToRecycleBin);
            ExecuteCommand(command);
        }

        /// <summary>
        /// 创建自定义动作命令并执行
        /// </summary>
        /// <param name="description">命令描述</param>
        /// <param name="executeAction">执行操作</param>
        /// <param name="undoAction">撤销操作</param>
        public void ExecuteActionCommand(string description, Action executeAction, Action undoAction = null)
        {
            var command = (CommandBase)(object)new ActionCommand(description, executeAction, undoAction);
            ExecuteCommand(command);
        }

        /// <summary>
        /// 撤销/重做状态变化事件处理
        /// </summary>
        private void OnUndoRedoStateChanged(object sender, UndoRedoStateChangedEventArgs e)
        {
            StateChanged?.Invoke(this, e);
        }
    }
}