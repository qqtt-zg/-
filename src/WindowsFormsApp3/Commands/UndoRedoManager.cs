using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp3.Commands
{
    /// <summary>
    /// 撤销/重做管理器
    /// </summary>
    public class UndoRedoManager
    {
        private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();
        private readonly int _maxHistorySize;
        private readonly object _lock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxHistorySize">最大历史记录大小</param>
        public UndoRedoManager(int maxHistorySize = 100)
        {
            _maxHistorySize = maxHistorySize;
        }

        /// <summary>
        /// 可以撤销的命令数量
        /// </summary>
        public int CanUndoCount
        {
            get
            {
                lock (_lock)
                {
                    return _undoStack.Count;
                }
            }
        }

        /// <summary>
        /// 可以重做的命令数量
        /// </summary>
        public int CanRedoCount
        {
            get
            {
                lock (_lock)
                {
                    return _redoStack.Count;
                }
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">要执行的命令</param>
        public void ExecuteCommand(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            lock (_lock)
            {
                try
                {
                    // 执行命令
                    command.Execute();

                    // 将命令添加到撤销栈
                    _undoStack.Push(command);

                    // 清空重做栈（新命令执行后，之前的重做历史失效）
                    _redoStack.Clear();

                    // 检查历史记录大小限制
                    while (_undoStack.Count > _maxHistorySize)
                    {
                        _undoStack.Pop();
                    }

                    // 触发状态变化事件
                    OnStateChanged();
                }
                catch (Exception ex)
                {
                    throw new CommandExecutionException($"执行命令失败: {command.Description}", ex);
                }
            }
        }

        /// <summary>
        /// 撤销最后一个命令
        /// </summary>
        /// <returns>撤销的命令描述，如果没有可撤销的命令则返回null</returns>
        public string Undo()
        {
            lock (_lock)
            {
                if (_undoStack.Count == 0)
                    return null;

                var command = _undoStack.Peek();

                if (!command.CanUndo())
                    return null;

                try
                {
                    // 从撤销栈移除
                    _undoStack.Pop();

                    // 执行撤销
                    command.Undo();

                    // 添加到重做栈
                    _redoStack.Push(command);

                    // 触发状态变化事件
                    OnStateChanged();

                    return command.Description;
                }
                catch (Exception ex)
                {
                    // 撤销失败，将命令重新放回撤销栈
                    _undoStack.Push(command);
                    throw new CommandExecutionException($"撤销命令失败: {command.Description}", ex);
                }
            }
        }

        /// <summary>
        /// 重做最后一个撤销的命令
        /// </summary>
        /// <returns>重做的命令描述，如果没有可重做的命令则返回null</returns>
        public string Redo()
        {
            lock (_lock)
            {
                if (_redoStack.Count == 0)
                    return null;

                var command = _redoStack.Peek();

                if (!command.CanRedo())
                    return null;

                try
                {
                    // 从重做栈移除
                    _redoStack.Pop();

                    // 执行重做
                    if (command is CommandBase commandBase)
                    {
                        commandBase.Redo();
                    }
                    else
                    {
                        // 对于没有实现Redo方法的命令，重新执行
                        command.Execute();
                    }

                    // 添加到撤销栈
                    _undoStack.Push(command);

                    // 触发状态变化事件
                    OnStateChanged();

                    return command.Description;
                }
                catch (Exception ex)
                {
                    // 重做失败，将命令重新放回重做栈
                    _redoStack.Push(command);
                    throw new CommandExecutionException($"重做命令失败: {command.Description}", ex);
                }
            }
        }

        /// <summary>
        /// 清空历史记录
        /// </summary>
        public void ClearHistory()
        {
            lock (_lock)
            {
                _undoStack.Clear();
                _redoStack.Clear();
                OnStateChanged();
            }
        }

        /// <summary>
        /// 获取可以撤销的命令列表
        /// </summary>
        /// <returns>可撤销的命令描述列表</returns>
        public List<string> GetUndoHistory()
        {
            lock (_lock)
            {
                return _undoStack.Select(c => c.Description).Reverse().ToList();
            }
        }

        /// <summary>
        /// 获取可以重做的命令列表
        /// </summary>
        /// <returns>可重做的命令描述列表</returns>
        public List<string> GetRedoHistory()
        {
            lock (_lock)
            {
                return _redoStack.Select(c => c.Description).Reverse().ToList();
            }
        }

        /// <summary>
        /// 获取最近的命令历史
        /// </summary>
        /// <param name="count">获取的命令数量</param>
        /// <returns>命令描述列表</returns>
        public List<string> GetRecentHistory(int count = 10)
        {
            lock (_lock)
            {
                var recentCommands = _undoStack.Take(count).Select(c => c.Description).Reverse().ToList();
                var redoCommands = _redoStack.Take(count).Select(c => $"[重做] {c.Description}").ToList();

                return recentCommands.Concat(redoCommands).ToList();
            }
        }

        /// <summary>
        /// 检查是否可以撤销
        /// </summary>
        /// <returns>是否可以撤销</returns>
        public bool CanUndo()
        {
            return CanUndoCount > 0;
        }

        /// <summary>
        /// 检查是否可以重做
        /// </summary>
        /// <returns>是否可以重做</returns>
        public bool CanRedo()
        {
            return CanRedoCount > 0;
        }

        /// <summary>
        /// 状态变化事件
        /// </summary>
        public event EventHandler<UndoRedoStateChangedEventArgs> StateChanged;

        /// <summary>
        /// 触发状态变化事件
        /// </summary>
        protected virtual void OnStateChanged()
        {
            StateChanged?.Invoke(this, new UndoRedoStateChangedEventArgs
            {
                CanUndoCount = CanUndoCount,
                CanRedoCount = CanRedoCount,
                CanUndo = CanUndo(),
                CanRedo = CanRedo()
            });
        }
    }

    /// <summary>
    /// 撤销/重做状态变化事件参数
    /// </summary>
    public class UndoRedoStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 可以撤销的命令数量
        /// </summary>
        public int CanUndoCount { get; set; }

        /// <summary>
        /// 可以重做的命令数量
        /// </summary>
        public int CanRedoCount { get; set; }

        /// <summary>
        /// 是否可以撤销
        /// </summary>
        public bool CanUndo { get; set; }

        /// <summary>
        /// 是否可以重做
        /// </summary>
        public bool CanRedo { get; set; }
    }
}