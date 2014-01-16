using System;

namespace Rakuten.Framework.Cache
{
    public interface ICache
    {
        CacheEntry<T> Get<T>(string key);

        void Set<T>(string key, T value, TimeSpan? timeToLive = null);

        void Clear();

        Int32 Size(bool inMemory = false);

        Int32 Count(bool inMemory = false);
    }
}
