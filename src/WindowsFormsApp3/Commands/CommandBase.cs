using System;

namespace WindowsFormsApp3.Commands
{
    /// <summary>
    /// 命令基类
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        private bool _executed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="description">命令描述</param>
        protected CommandBase(string description)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 命令描述
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 命令创建时间
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// 执行命令
        /// </summary>
        public void Execute()
        {
            if (_executed)
                throw new InvalidOperationException("命令已经执行过，不能重复执行");

            try
            {
                OnExecute();
                _executed = true;
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException($"执行命令 '{Description}' 失败", ex);
            }
        }

        /// <summary>
        /// 撤销命令
        /// </summary>
        public void Undo()
        {
            if (!_executed)
                throw new InvalidOperationException("命令尚未执行，无法撤销");

            if (!CanUndo())
                throw new InvalidOperationException($"命令 '{Description}' 不能被撤销");

            try
            {
                OnUndo();
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException($"撤销命令 '{Description}' 失败", ex);
            }
        }

        /// <summary>
        /// 检查命令是否可以撤销
        /// </summary>
        /// <returns>是否可以撤销</returns>
        public virtual bool CanUndo()
        {
            return _executed;
        }

        /// <summary>
        /// 检查命令是否可以重做
        /// </summary>
        /// <returns>是否可以重做</returns>
        public virtual bool CanRedo()
        {
            return _executed;
        }

        /// <summary>
        /// 执行命令的具体实现
        /// </summary>
        protected abstract void OnExecute();

        /// <summary>
        /// 撤销命令的具体实现
        /// </summary>
        protected abstract void OnUndo();

        /// <summary>
        /// 重做命令（默认重新执行）
        /// </summary>
        public virtual void Redo()
        {
            if (!CanRedo())
                throw new InvalidOperationException($"命令 '{Description}' 不能被重做");

            OnExecute();
        }
    }

    /// <summary>
    /// 命令执行异常
    /// </summary>
    public class CommandExecutionException : Exception
    {
        public CommandExecutionException(string message) : base(message)
        {
        }

        public CommandExecutionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}