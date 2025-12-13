using System;

namespace WindowsFormsApp3.Commands
{
    /// <summary>
    /// 命令接口，实现撤销/重做功能
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 命令描述
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 命令创建时间
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// 执行命令
        /// </summary>
        void Execute();

        /// <summary>
        /// 撤销命令
        /// </summary>
        void Undo();

        /// <summary>
        /// 检查命令是否可以撤销
        /// </summary>
        /// <returns>是否可以撤销</returns>
        bool CanUndo();

        /// <summary>
        /// 检查命令是否可以重做
        /// </summary>
        /// <returns>是否可以重做</returns>
        bool CanRedo();
    }
}