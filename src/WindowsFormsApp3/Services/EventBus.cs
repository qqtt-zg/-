using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsFormsApp3.Interfaces; // 修改为使用Interfaces命名空间的ILogger
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 事件总线实现类
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<object>> _syncHandlers = new Dictionary<Type, List<object>>();
        private readonly Dictionary<Type, List<object>> _asyncHandlers = new Dictionary<Type, List<object>>();
        private readonly object _lock = new object();
        private readonly Interfaces.ILogger _logger; // 明确指定使用Interfaces命名空间的ILogger

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志服务（可选）</param>
        public EventBus(Interfaces.ILogger logger = null) // 明确指定使用Interfaces命名空间的ILogger
        {
            _logger = logger;
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : class
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var eventType = typeof(TEvent);
            _logger?.LogInformation($"Publishing event: {eventType.Name}");

            try
            {
                // 处理同步订阅者
                if (_syncHandlers.TryGetValue(eventType, out var syncHandlerList))
                {
                    foreach (var handler in syncHandlerList.Cast<Action<TEvent>>())
                    {
                        try
                        {
                            handler(@event);
                        }
                        catch (Exception ex)
                        {
                            // 记录错误但不中断其他处理程序
                            var errorMessage = $"Error in sync event handler for {eventType.Name}: {ex.Message}";
                            _logger?.LogError(ex, errorMessage);
                            LogHelper.Debug(errorMessage);
                        }
                    }
                }

                // 处理异步订阅者（同步执行）
                if (_asyncHandlers.TryGetValue(eventType, out var asyncHandlerList))
                {
                    foreach (var handler in asyncHandlerList.Cast<Func<TEvent, Task>>())
                    {
                        try
                        {
                            handler(@event).GetAwaiter().GetResult();
                        }
                        catch (Exception ex)
                        {
                            var errorMessage = $"Error in async event handler for {eventType.Name}: {ex.Message}";
                            _logger?.LogError(ex, errorMessage);
                            LogHelper.Debug(errorMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error publishing event {eventType.Name}: {ex.Message}";
                _logger?.LogError(ex, errorMessage);
                throw;
            }
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var eventType = typeof(TEvent);
            _logger?.LogInformation($"Publishing async event: {eventType.Name}");

            try
            {
                // 处理同步订阅者
                if (_syncHandlers.TryGetValue(eventType, out var syncHandlerList))
                {
                    foreach (var handler in syncHandlerList.Cast<Action<TEvent>>())
                    {
                        try
                        {
                            handler(@event);
                        }
                        catch (Exception ex)
                        {
                            var errorMessage = $"Error in sync event handler for {eventType.Name}: {ex.Message}";
                            _logger?.LogError(ex, errorMessage);
                            LogHelper.Debug(errorMessage);
                        }
                    }
                }

                // 处理异步订阅者
                if (_asyncHandlers.TryGetValue(eventType, out var asyncHandlerList))
                {
                    var tasks = asyncHandlerList.Cast<Func<TEvent, Task>>()
                        .Select(handler =>
                        {
                            try
                            {
                                return handler(@event);
                            }
                            catch (Exception ex)
                            {
                                var errorMessage = $"Error in async event handler for {eventType.Name}: {ex.Message}";
                                _logger?.LogError(ex, errorMessage);
                                LogHelper.Debug(errorMessage);
                                return Task.CompletedTask;
                            }
                        })
                        .ToList(); // 立即执行ToList以触发异常

                    if (tasks.Count > 0)
                    {
                        await Task.WhenAll(tasks);
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error publishing async event {eventType.Name}: {ex.Message}";
                _logger?.LogError(ex, errorMessage);
                throw;
            }
        }

        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : class
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var eventType = typeof(TEvent);
            _logger?.LogInformation($"Subscribing sync handler for event: {eventType.Name}");

            lock (_lock)
            {
                if (!_syncHandlers.ContainsKey(eventType))
                {
                    _syncHandlers[eventType] = new List<object>();
                }

                _syncHandlers[eventType].Add(handler);
            }
        }

        public void Subscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : class
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var eventType = typeof(TEvent);
            _logger?.LogInformation($"Subscribing async handler for event: {eventType.Name}");

            lock (_lock)
            {
                if (!_asyncHandlers.ContainsKey(eventType))
                {
                    _asyncHandlers[eventType] = new List<object>();
                }

                _asyncHandlers[eventType].Add(handler);
            }
        }

        public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : class
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var eventType = typeof(TEvent);
            _logger?.LogInformation($"Unsubscribing sync handler for event: {eventType.Name}");

            lock (_lock)
            {
                if (_syncHandlers.TryGetValue(eventType, out var handlers))
                {
                    handlers.Remove(handler);
                    if (handlers.Count == 0)
                    {
                        _syncHandlers.Remove(eventType);
                    }
                }
            }
        }

        public void Unsubscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : class
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var eventType = typeof(TEvent);
            _logger?.LogInformation($"Unsubscribing async handler for event: {eventType.Name}");

            lock (_lock)
            {
                if (_asyncHandlers.TryGetValue(eventType, out var handlers))
                {
                    handlers.Remove(handler);
                    if (handlers.Count == 0)
                    {
                        _asyncHandlers.Remove(eventType);
                    }
                }
            }
        }

        /// <summary>
        /// 获取当前订阅者的数量
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <returns>订阅者数量</returns>
        public int GetSubscriberCount<TEvent>() where TEvent : class
        {
            var eventType = typeof(TEvent);
            var syncCount = _syncHandlers.ContainsKey(eventType) ? _syncHandlers[eventType].Count : 0;
            var asyncCount = _asyncHandlers.ContainsKey(eventType) ? _asyncHandlers[eventType].Count : 0;
            return syncCount + asyncCount;
        }

        /// <summary>
        /// 清除所有订阅者
        /// </summary>
        public void ClearAllSubscribers()
        {
            lock (_lock)
            {
                _syncHandlers.Clear();
                _asyncHandlers.Clear();
            }
            _logger?.LogInformation("Cleared all event subscribers");
        }
    }
}