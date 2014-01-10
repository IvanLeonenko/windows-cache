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

        public Cache(CacheContainer container)
        {
            _versionProvider = container.Resolve<IVersionProvider>();
            _storage = container.Resolve<IStorage>();
            _cacheData = new CacheData(_storage, container.Resolve<ISerializer>());

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

        private bool DifferentVersion()
        {
            var version = _versionProvider.GetVersion();
            const string versionFileName = "cache.version";
            Version cacheVersion;
            if (!Version.TryParse(_storage.ReadString(versionFileName), out cacheVersion))
                cacheVersion = new Version(0, 0);
            _storage.WriteString(versionFileName, version.ToString());
            return version != cacheVersion;
        }
    }
}
