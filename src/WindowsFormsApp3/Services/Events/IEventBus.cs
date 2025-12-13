using System;
using System.Threading.Tasks;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 事件总线接口，用于模块间的事件通信
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="event">事件实例</param>
        void Publish<TEvent>(TEvent @event) where TEvent : class;

        /// <summary>
        /// 异步发布事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="event">事件实例</param>
        /// <returns>异步任务</returns>
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : class;

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="handler">事件处理程序</param>
        void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : class;

        /// <summary>
        /// 订阅事件（异步处理程序）
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="handler">异步事件处理程序</param>
        void Subscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : class;

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="handler">事件处理程序</param>
        void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : class;

        /// <summary>
        /// 取消订阅事件（异步处理程序）
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="handler">异步事件处理程序</param>
        void Unsubscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : class;
    }
}