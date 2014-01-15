using System;
using System.Threading.Tasks;
using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;

namespace Rakuten.Framework.Cache
{
    public class Cache : ICache
    {
        private readonly IVersionProvider _versionProvider;
        private readonly IStorage _storage;
        private readonly CacheData _cacheData;
        public static string VersionEntryName = "cache.version";

        public Cache(CacheContainer container, CacheConfiguration cacheConfiguration)
        {
            _versionProvider = container.Resolve<IVersionProvider>();
            _storage = container.Resolve<IStorage>();
            _cacheData = new CacheData(_storage, container.Resolve<ISerializer>(), cacheConfiguration);
            if (DifferentVersion())
                Clear();
        }
        public void Set<T>(string key, T value, TimeSpan? timeToLive = null)
        {
            _cacheData.Set(key, value, timeToLive);
        }

        public CacheEntry<T> Get<T>(string key)
        {
            return _cacheData.Get<T>(key);
        }

        public async Task<CacheEntry<T>> GetAsync<T>(string key)
        {
            return await Task.Run(() => Get<T>(key));
        }

        public async Task SetAsync<T>(string key, T value)
        {
            await Task.Run(() => Set(key, value));
        }

        public void Clear()
        {
            _cacheData.Clear();
        }

        public Int32 Size(bool inMemory = false)
        {
            return _cacheData.Size(inMemory);
        }
        public Int32 Count(bool inMemory = false)
        {
            return _cacheData.Count(inMemory);
        }

        private bool DifferentVersion()
        {
            var version = _versionProvider.GetVersion();
            Version cacheVersion;
            if (!Version.TryParse(_storage.GetString(VersionEntryName), out cacheVersion))
                cacheVersion = new Version(0, 0);
            _storage.Write(VersionEntryName, version.ToString());
            return version != cacheVersion;
        }
    }
}
