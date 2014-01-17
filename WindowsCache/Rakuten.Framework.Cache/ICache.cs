using System;

namespace Rakuten.Framework.Cache
{
    public interface ICache
    {
        CacheEntry<T> Get<T>(string key);

        void Set<T>(string key, T value, TimeSpan? timeToLive = null);

        void Clear();

        Int32 Size { get; }

        Int32 Count { get; }

        Int32 InMemorySize { get; }

        Int32 InMemoryCount { get; }
    }
}
