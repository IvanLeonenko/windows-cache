using System;
using System.Threading.Tasks;

namespace Framework.Cache
{
    public interface ICache : IDisposable
    {
        Task<CacheEntry<T>> Get<T>(string key);

        Task Set<T>(string key, T value, TimeSpan? timeToLive = null);

        Task Remove(string key, bool inMemory = false);

        Task Clear();

        Int32 Size { get; }

        Int32 Count { get; }

        Int32 InMemorySize { get; }

        Int32 InMemoryCount { get; }
    }
}
