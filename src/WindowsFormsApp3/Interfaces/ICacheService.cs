using System;
using System.Collections.Generic;

namespace WindowsFormsApp3.Interfaces
{
    /// <summary>
    /// 缓存服务接口，提供内存缓存功能
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>缓存数据，如果不存在则返回默认值</returns>
        T Get<T>(string key);

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiration">过期时间（可选）</param>
        void Set<T>(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否成功移除</returns>
        bool Remove(string key);

        /// <summary>
        /// 检查缓存项是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否存在</returns>
        bool Contains(string key);

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        void Clear();

        /// <summary>
        /// 获取或设置缓存项，如果不存在则通过工厂方法创建
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="factory">值工厂方法</param>
        /// <param name="expiration">过期时间（可选）</param>
        /// <returns>缓存数据</returns>
        T GetOrCreate<T>(string key, Func<T> factory, TimeSpan? expiration = null);

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns>缓存键集合</returns>
        IEnumerable<string> GetAllKeys();

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        /// <returns>缓存统计信息</returns>
        CacheStatistics GetStatistics();
    }

    /// <summary>
    /// 缓存统计信息
    /// </summary>
    public class CacheStatistics
    {
        /// <summary>
        /// 缓存项总数
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// 缓存命中次数
        /// </summary>
        public long HitCount { get; set; }

        /// <summary>
        /// 缓存未命中次数
        /// </summary>
        public long MissCount { get; set; }

        /// <summary>
        /// 缓存命中率
        /// </summary>
        public double HitRate => TotalItems > 0 ? (double)HitCount / (HitCount + MissCount) : 0;

        /// <summary>
        /// 缓存总大小（字节）
        /// </summary>
        public long TotalSize { get; set; }
    }
}