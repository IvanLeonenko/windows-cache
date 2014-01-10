using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;
using System;
using System.Collections.Generic;

namespace Rakuten.Framework.Cache
{
    class CacheData
    {
        private const string CacheName = "cache.protobuf";
        private readonly ISerializer _serializer;
        private readonly IStorage _storage;
        private Dictionary<string, ICacheEntry> Entries { get; set; }

        public CacheData(IStorage storage, ISerializer serializer)
        {
            _serializer = serializer;
            _storage = storage;
            var stream = _storage.ReadStream(CacheName);
            Entries = (stream == null) ? new Dictionary<string, ICacheEntry>() : serializer.Deserialize<Dictionary<string, ICacheEntry>>(stream);
        }

        public void Set<T>(string key, T value, TimeSpan? timeToLive = null)
        {
            CheckType(typeof(T));
            
            var currentTime = DateTime.UtcNow;

            var createdTime = currentTime;
            if (Entries.ContainsKey(key))
            {
                var existingCacheEntry = Entries[key] as CacheEntry<T>;
                if (existingCacheEntry != null)
                {
                    createdTime = existingCacheEntry.CreatedTime;
                }
            }

            var expirationTime = currentTime + (timeToLive.HasValue ? timeToLive.Value : TimeSpan.FromDays(3));
            
            var cacheEntry = new CacheEntry<T> { Value = value, CreatedTime = createdTime, ModifiedTime = currentTime, LastAccessTime = currentTime, ExpirationTime = expirationTime };
            
            var stringValue = value as string;
            var bytesValue = value as byte[];
            if (stringValue != null)
            {
                cacheEntry.EntryType = EntryType.String;
                cacheEntry.Size = Convert.ToUInt32(stringValue.Length) * 2;
            }
            else if (bytesValue != null)
            {
                cacheEntry.EntryType = EntryType.Binary;
                cacheEntry.Size = Convert.ToUInt32(bytesValue.Length);
            }
            else
            {
                var stream = _serializer.Serialize(Entries);
                //serialize
                cacheEntry.Size = Convert.ToUInt32(stream.Length);
            }

            Entries[key] = cacheEntry;


            var entriesStream = _serializer.Serialize(Entries);
            _storage.WriteStream(CacheName, entriesStream);
        }

        public CacheEntry<T> Get<T>(string key)
        {
            CheckType(typeof(T));
            var entry = Entries[key] as CacheEntry<T>;
            return entry != null && entry.IsExpired ? null : entry;
        }

        public void Clean()
        {
            _storage.Remove(CacheName);
        }

        private void CheckType(Type type)
        {
            if (!_serializer.CanSerialize(type))
                throw new Exception("Cannot process provided Type. Please register it or provide proto attributes.");
        }
    }
}
