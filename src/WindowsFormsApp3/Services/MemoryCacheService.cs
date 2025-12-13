using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WindowsFormsApp3.Interfaces;

namespace WindowsFormsApp3.Services
{
    /// <summary>
    /// 内存缓存服务实现
    /// </summary>
    public class MemoryCacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, CacheItem> _cache;
        private long _hitCount;
        private long _missCount;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MemoryCacheService()
        {
            _cache = new ConcurrentDictionary<string, CacheItem>();
            _hitCount = 0;
            _missCount = 0;
        }

        /// <summary>
        /// 获取缓存项
        /// </summary>
        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("缓存键不能为空", nameof(key));

            if (_cache.TryGetValue(key, out var cacheItem))
            {
                if (cacheItem.IsExpired)
                {
                    _cache.TryRemove(key, out _);
                    _missCount++;
                    return default;
                }

                _hitCount++;
                return (T)cacheItem.Value;
            }

            _missCount++;
            return default;
        }

        /// <summary>
        /// 设置缓存项
        /// </summary>
        public void Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("缓存键不能为空", nameof(key));

            var cacheItem = new CacheItem
            {
                Value = value,
                CreatedAt = DateTime.UtcNow,
                Expiration = expiration
            };

            _cache[key] = cacheItem;
        }

        /// <summary>
        /// 移除缓存项
        /// </summary>
        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("缓存键不能为空", nameof(key));

            return _cache.TryRemove(key, out _);
        }

        /// <summary>
        /// 检查缓存项是否存在
        /// </summary>
        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("缓存键不能为空", nameof(key));

            return _cache.TryGetValue(key, out var cacheItem) && !cacheItem.IsExpired;
        }

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
            _hitCount = 0;
            _missCount = 0;
        }

        /// <summary>
        /// 获取或设置缓存项
        /// </summary>
        public T GetOrCreate<T>(string key, Func<T> factory, TimeSpan? expiration = null)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("缓存键不能为空", nameof(key));

            if (Contains(key))
            {
                var cachedValue = Get<T>(key);
                if (cachedValue != null)
                    return cachedValue;
                // 如果缓存值为null，继续执行factory方法重新创建值
            }

            var value = factory();
            if (value == null)
            {
                throw new InvalidOperationException("缓存工厂方法不能返回null值");
            }
            Set(key, value, expiration);
            return value;
        }

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        public IEnumerable<string> GetAllKeys()
        {
            return _cache.Keys.ToList();
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        public CacheStatistics GetStatistics()
        {
            return new CacheStatistics
            {
                TotalItems = _cache.Count,
                HitCount = _hitCount,
                MissCount = _missCount,
                TotalSize = CalculateTotalSize()
            };
        }

        /// <summary>
        /// 计算缓存总大小（估算）
        /// </summary>
        private long CalculateTotalSize()
        {
            long totalSize = 0;
            foreach (var item in _cache.Values)
            {
                if (item.Value != null)
                {
                    // 简单估算对象大小
                    totalSize += System.Runtime.InteropServices.Marshal.SizeOf(item.Value.GetType());
                }
            }
            return totalSize;
        }

        /// <summary>
        /// 缓存项内部类
        /// </summary>
        private class CacheItem
        {
            public object Value { get; set; }
            public DateTime CreatedAt { get; set; }
            public TimeSpan? Expiration { get; set; }

            public bool IsExpired
            {
                get
                {
                    if (!Expiration.HasValue)
                        return false;

                    return DateTime.UtcNow - CreatedAt > Expiration.Value;
                }
            }
        }
    }
}