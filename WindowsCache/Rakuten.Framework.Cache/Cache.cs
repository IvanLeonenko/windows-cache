using System;
using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;

namespace Rakuten.Framework.Cache
{
    public class Cache
    {
        private const string CacheName = "cache.protobuf";
        private readonly IStorage _storage;
        private readonly ISerializer _serializer;
        private readonly CacheData _cacheData;

        public Cache(CacheContainer container)
        {
            _storage = container.Resolve<IStorage>();
            _serializer = container.Resolve<ISerializer>();

            var stream = _storage.ReadStream(CacheName);
            _cacheData = stream == null ? new CacheData() : _serializer.Deserialize<CacheData>(stream);
        }
        public void Set<T>(string key, T value)
        {
            CheckType(typeof(T));
            var cacheEntry = new CacheEntry<T> {Value = value};
            _cacheData.Entries[key] = cacheEntry;
            var stream = _serializer.Serialize(_cacheData);
            _storage.WriteStream(CacheName, stream);
        }

        public T Get<T>(string key)
        {
            CheckType(typeof(T));
            return ((CacheEntry<T>)_cacheData.Entries[key]).Value;
        }

        private void CheckType(Type type)
        {
            if (!_serializer.CanSerialize(type))
                throw new Exception("Cannot process provided Type. Please register it or provide proto attributes.");
        }
    }
}
