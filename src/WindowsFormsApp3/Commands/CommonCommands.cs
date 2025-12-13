using System;
using System.Collections.Generic;

namespace WindowsFormsApp3.Commands
{
    /// <summary>
    /// 复合命令（可以包含多个子命令）
    /// </summary>
    public class CompositeCommand : CommandBase
    {
        private readonly List<ICommand> _commands;
        private readonly List<ICommand> _executedCommands;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="description">命令描述</param>
        /// <param name="commands">子命令列表</param>
        public CompositeCommand(string description, params ICommand[] commands)
            : base(description)
        {
            _commands = new List<ICommand>(commands ?? throw new ArgumentNullException(nameof(commands)));
            _executedCommands = new List<ICommand>();
        }

        /// <summary>
        /// 添加子命令
        /// </summary>
        /// <param name="command">要添加的命令</param>
        public void AddCommand(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            _commands.Add(command);
        }

        /// <summary>
        /// 执行所有子命令
        /// </summary>
        protected override void OnExecute()
        {
            _executedCommands.Clear();

            foreach (var command in _commands)
            {
                try
                {
                    command.Execute();
                    _executedCommands.Add(command);
                }
                catch (Exception ex)
                {
                    // 执行失败，撤销已执行的命令
                    try
                    {
                        for (int i = _executedCommands.Count - 1; i >= 0; i--)
                        {
                            if (_executedCommands[i].CanUndo())
                            {
                                _executedCommands[i].Undo();
                            }
                        }
                    }
                    catch (Exception undoEx)
                    {
                        // 撤销操作也失败了，记录错误
                        System.Diagnostics.Debug.WriteLine($"撤销复合命令失败: {undoEx.Message}");
                    }

                    _executedCommands.Clear();
                    throw new InvalidOperationException($"复合命令执行失败: {command.Description}", ex);
                }
            }
        }

        /// <summary>
        /// 撤销所有子命令（逆序）
        /// </summary>
        protected override void OnUndo()
        {
            for (int i = _executedCommands.Count - 1; i >= 0; i--)
            {
                var command = _executedCommands[i];
                if (command.CanUndo())
                {
                    try
                    {
                        command.Undo();
                    }
                    catch (Exception ex)
                    {
                        // 撤销失败，记录错误但继续撤销其他命令
                        System.Diagnostics.Debug.WriteLine($"撤销子命令失败: {command.Description}, 错误: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 检查是否可以撤销
        /// </summary>
        /// <returns>是否可以撤销</returns>
        public override bool CanUndo()
        {
            if (!base.CanUndo())
                return false;

            // 检查是否有可撤销的已执行命令
            foreach (var command in _executedCommands)
            {
                if (command.CanUndo())
                    return true;
            }

            return false;
        }
    }

    /// <summary>
    /// 简单操作命令（通过委托定义操作）
    /// </summary>
    public class ActionCommand : CommandBase
    {
        private readonly Action _executeAction;
        private readonly Action _undoAction;
        private readonly Func<bool> _canUndoFunc;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="description">命令描述</param>
        /// <param name="executeAction">执行操作</param>
        /// <param name="undoAction">撤销操作</param>
        /// <param name="canUndoFunc">检查是否可以撤销的函数</param>
        public ActionCommand(string description, Action executeAction, Action undoAction = null, Func<bool> canUndoFunc = null)
            : base(description)
        {
            _executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            _undoAction = undoAction;
            _canUndoFunc = canUndoFunc;
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        protected override void OnExecute()
        {
            _executeAction?.Invoke();
        }

        /// <summary>
        /// 撤销操作
        /// </summary>
        protected override void OnUndo()
        {
            _undoAction?.Invoke();
        }

        /// <summary>
        /// 检查是否可以撤销
        /// </summary>
        /// <returns>是否可以撤销</returns>
        public override bool CanUndo()
        {
            if (!base.CanUndo())
                return false;

            return _canUndoFunc?.Invoke() ?? _undoAction != null;
        }
    }

    /// <summary>
    /// 参数化操作命令
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    public class ParameterizedActionCommand<T> : CommandBase
    {
        private readonly T _parameter;
        private readonly Action<T> _executeAction;
        private readonly Action<T> _undoAction;
        private readonly Func<T, bool> _canUndoFunc;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="description">命令描述</param>
        /// <param name="parameter">参数</param>
        /// <param name="executeAction">执行操作</param>
        /// <param name="undoAction">撤销操作</param>
        /// <param name="canUndoFunc">检查是否可以撤销的函数</param>
        public ParameterizedActionCommand(string description, T parameter, Action<T> executeAction, Action<T> undoAction = null, Func<T, bool> canUndoFunc = null)
            : base(description)
        {
            _parameter = parameter;
            _executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            _undoAction = undoAction;
            _canUndoFunc = canUndoFunc;
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        protected override void OnExecute()
        {
            _executeAction?.Invoke(_parameter);
        }

        /// <summary>
        /// 撤销操作
        /// </summary>
        protected override void OnUndo()
        {
            _undoAction?.Invoke(_parameter);
        }

        /// <summary>
        /// 检查是否可以撤销
        /// </summary>
        /// <returns>是否可以撤销</returns>
        public override bool CanUndo()
        {
            if (!base.CanUndo())
                return false;

            return _canUndoFunc?.Invoke(_parameter) ?? _undoAction != null;
        }
    }

    /// <summary>
    /// 状态变化命令（用于记录状态变化）
    /// </summary>
    /// <typeparam name="T">状态类型</typeparam>
    public class StateChangeCommand<T> : CommandBase
    {
        private readonly object _target;
        private readonly string _propertyName;
        private readonly T _oldValue;
        private readonly T _newValue;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="description">命令描述</param>
        /// <param name="target">目标对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="oldValue">旧值</param>
        /// <param name="newValue">新值</param>
        public StateChangeCommand(string description, object target, string propertyName, T oldValue, T newValue)
            : base(description)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            _oldValue = oldValue;
            _newValue = newValue;
        }

        /// <summary>
        /// 执行状态变化
        /// </summary>
        protected override void OnExecute()
        {
            SetPropertyValue(_target, _propertyName, _newValue);
        }

        /// <summary>
        /// 撤销状态变化
        /// </summary>
        protected override void OnUndo()
        {
            SetPropertyValue(_target, _propertyName, _oldValue);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        private void SetPropertyValue(object target, string propertyName, T value)
        {
            var property = target.GetType().GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                if (property.PropertyType == typeof(T))
                {
                    property.SetValue(target, value);
                }
                else
                {
                    // 尝试类型转换
                    var convertedValue = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(target, convertedValue);
                }
            }
        }
    }

    /// <summary>
    /// 事务命令（确保要么全部成功，要么全部失败）
    /// </summary>
    public class TransactionCommand : CompositeCommand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="description">命令描述</param>
        /// <param name="commands">子命令列表</param>
        public TransactionCommand(string description, params ICommand[] commands)
            : base($"事务: {description}", commands)
        {
        }

        /// <summary>
        /// 执行事务命令
        /// </summary>
        protected override void OnExecute()
        {
            var originalDescription = Description;
            try
            {
                base.OnExecute();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"事务执行失败: {originalDescription}", ex);
            }
        }
    }
}