using System;
using System.Threading.Tasks;
using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;

namespace Rakuten.Framework.Cache
{
    public class Cache : ICache
    {
        private readonly ILogger _logger;
        private readonly IVersionProvider _versionProvider;
        private readonly IStorage _storage;
        private readonly CacheData _cacheData;
        public static string VersionEntryName = "cache.version";

        public Cache(CacheContainer container, CacheConfiguration cacheConfiguration)
        {
            _versionProvider = container.Resolve<IVersionProvider>();
            _logger = container.Resolve<ILogger>();
            _storage = new InMemoryStorageProxy(container.Resolve<IStorage>(), cacheConfiguration.InMemoryOnly);
            _cacheData = new CacheData(_storage, container.Resolve<ISerializer>(), cacheConfiguration, _logger);
            _cacheData.RestoreState();
            Initialize();
        }

        private async void Initialize()
        {
            if (await DifferentVersion())
                await Clear();
        }

        public async Task Set<T>(string key, T value, TimeSpan? timeToLive = null)
        {
            await _cacheData.Set(key, value, timeToLive);
        }

        public async Task<CacheEntry<T>> Get<T>(string key)
        {
            return await _cacheData.Get<T>(key);
        }

        public async Task<CacheEntry<T>> GetAsync<T>(string key)
        {
            return await Task.Run(() => Get<T>(key));
        }

        public async Task SetAsync<T>(string key, T value)
        {
            await Task.Run(() => Set(key, value));
        }

        public async Task Clear()
        {
            await _cacheData.Clear();
        }

        public Int32 Size { get { return _cacheData.Size; } }
        public Int32 Count { get { return _cacheData.Count; } }
        public Int32 InMemorySize { get { return _cacheData.InMemorySize; } }
        public Int32 InMemoryCount { get { return _cacheData.InMemoryCount; } }

        private async Task<bool> DifferentVersion()
        {
            var version = _versionProvider.GetVersion();
            Version cacheVersion;
            if (!Version.TryParse(await _storage.GetString(VersionEntryName), out cacheVersion))
                cacheVersion = new Version(0, 0);
            var result = version != cacheVersion;
            await _storage.Write(VersionEntryName, version.ToString());
            return result;
        }
    }
}
