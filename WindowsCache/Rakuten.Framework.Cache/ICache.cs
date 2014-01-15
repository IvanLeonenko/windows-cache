
using System;

namespace Rakuten.Framework.Cache
{
    public interface ICache
    {
        CacheEntry<T> Get<T>(string key);

        void Set<T>(string key, T value, TimeSpan? timeToLive = null);
    }
}
