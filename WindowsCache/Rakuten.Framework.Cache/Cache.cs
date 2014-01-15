using System;
using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;

namespace Rakuten.Framework.Cache
{
    public class Cache
    {
        private readonly IVersionProvider _versionProvider;
        private readonly IStorage _storage;
        private readonly CacheData _cacheData;

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

        public void Clear()
        {
            _cacheData.Clean();
        }

        public Int32 GetSize(bool inMemory = false)
        {
            return _cacheData.GetSize(inMemory);
        }
        public Int32 Count(bool inMemory = false)
        {
            return _cacheData.Count(inMemory);
        }

        private bool DifferentVersion()
        {
            var version = _versionProvider.GetVersion();
            const string versionFileName = "cache.version";
            Version cacheVersion;
            if (!Version.TryParse(_storage.GetString(versionFileName), out cacheVersion))
                cacheVersion = new Version(0, 0);
            _storage.Write(versionFileName, version.ToString());
            return version != cacheVersion;
        }
    }
}
