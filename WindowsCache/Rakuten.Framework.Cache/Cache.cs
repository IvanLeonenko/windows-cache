using System;
using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;

namespace Rakuten.Framework.Cache
{
    public class Cache
    {
        private const string CacheName = "cache.protobuf"; 
        private readonly IStorage _storage;
        private readonly CacheData _cacheData;

        public Cache(CacheContainer container)
        {
            _storage = container.Resolve<IStorage>();

            var stream = _storage.ReadStream(CacheName);
            _cacheData = stream == null ? new CacheData() : ProtoBufSerializer.Deserialize<CacheData>(stream);
        }

        public void Set<T>(string key, T value)
        {
            CheckType(typeof(T));
            _cacheData.Entries.Add(key, new CacheEntry<T> { Value = value });
            var stream = ProtoBufSerializer.Serialize(_cacheData);
            _storage.WriteStream(CacheName, stream);
        }

        public T Get<T>(string key)
        {
            CheckType(typeof(T));
            return ((CacheEntry<T>)_cacheData.Entries[key]).Value;
        }

        private static void CheckType(Type type)
        {
            if (!ProtoBufSerializer.CanSerialize(type))
                throw new Exception("Cannot process provided Type. Please register it or provide proto attributes.");
        }
    }
}
