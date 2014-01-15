using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rakuten.Framework.Cache
{
    class CacheData
    {
        private const string CacheName = "cache.protobuf";
        private readonly ISerializer _serializer;
        private readonly IStorage _storage;
        private Dictionary<string, ICacheEntry> Entries { get; set; }
        public const int CacheEntrySize = 128;

        private readonly CacheConfiguration _cacheConfiguration;

        public CacheData(IStorage storage, ISerializer serializer, CacheConfiguration cacheConfiguration)
        {
            _cacheConfiguration = cacheConfiguration;
            _serializer = serializer;
            _storage = storage;
            var stream = _storage.GetStream(CacheName);
            Entries = (stream == null) ? new Dictionary<string, ICacheEntry>() : _serializer.Deserialize<Dictionary<string, ICacheEntry>>(stream);
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
            
            var cacheEntry = new CacheEntry<T> { Value = value, IsInMemory = true, CreatedTime = createdTime, ModifiedTime = currentTime, LastAccessTime = currentTime, ExpirationTime = expirationTime};

            SetSizeAndType(cacheEntry, value);
            
            Entries[key] = cacheEntry;

            var entriesStream = _serializer.Serialize(Entries);
            _storage.Write(CacheName, entriesStream);
        }
        
        public CacheEntry<T> Get<T>(string key)
        {
            CheckType(typeof(T));

            var resultEntry = Entries.ContainsKey(key) ? Entries[key] as CacheEntry<T> : null;

            if (resultEntry == null)
            {
                Entries.Remove(key);
            }
            else if (resultEntry.IsExpired)
            {
                //if (!resultEntry.IsInMemory)
                _storage.Remove(resultEntry.FileName);
                Entries.Remove(key);
                resultEntry = null;
            }
            else
            {
                if (!resultEntry.IsInMemory)
                {
                    if (resultEntry.EntryType == EntryType.TemplateType)
                    {
                        resultEntry.Value = _serializer.Deserialize<T>(_storage.GetStream(resultEntry.FileName));
                    }
                    else if (resultEntry.EntryType == EntryType.Binary)
                    {
                        object obj = _storage.GetBytes(resultEntry.FileName);
                        resultEntry.Value = (T)obj;
                    }
                    else
                    {
                        object obj = _storage.GetString(resultEntry.FileName);
                        resultEntry.Value = (T)obj;
                    }

                    RemoveOnReachingSizeLimit(resultEntry.Size, _cacheConfiguration.MaxInMemoryCacheDataSize, true);
                    RemoveEntryOnMaxCapacityReached(_cacheConfiguration.MaxInMemoryCacheDataEntries, true);

                    resultEntry.IsInMemory = true;
                }

                resultEntry.LastAccessTime = DateTime.UtcNow;
            }
            
            /*
             * this is to omit two serializations
             */
            //if (resultEntry!= null &&  !resultEntry.IsInMemory)
            //{
            //    using (var stream = new MemoryStream(resultEntry.SerializedValue))
            //    {
            //        resultEntry.Value = _serializer.Deserialize<T>(stream);
            //    }
            //    resultEntry.IsInMemory = true;
            //}
            
            return resultEntry;
        }

        public void Clean()
        {
            _storage.Remove(CacheName);
        }

        private void CheckType(Type type)
        {
            //todo: do not throw, write to log, return null
            if (!_serializer.CanSerialize(type))
                throw new Exception("Cannot process provided Type. Please register it or provide proto attributes.");
        }

        private void SetSizeAndType<T>(CacheEntry<T> cacheEntry, T value)
        {
            var stringValue = value as string;
            var bytesValue = value as byte[];
            if (stringValue != null)
            {
                cacheEntry.EntryType = EntryType.String;
                cacheEntry.Size = Convert.ToInt32(stringValue.Length) * 2 + CacheEntrySize;
                _storage.Write(cacheEntry.FileName, stringValue);
            }
            else if (bytesValue != null)
            {
                cacheEntry.EntryType = EntryType.Binary;
                cacheEntry.Size = Convert.ToInt32(bytesValue.Length) + CacheEntrySize;
                _storage.Write(cacheEntry.FileName, bytesValue);
            }
            else
            {
                using (var stream = _serializer.Serialize(value))
                {
                    cacheEntry.Size = Convert.ToInt32(stream.Length) + CacheEntrySize;
                    _storage.Write(cacheEntry.FileName, stream);
                }

                /*
                 * this is to omit two serializations
                 */
                //using (var memoryStream = new MemoryStream())
                //{
                //    stream.CopyTo(memoryStream);
                //    cacheEntry.SerializedValue = memoryStream.ToArray();
                //}
            }

            RemoveOnReachingSizeLimit(cacheEntry.Size, _cacheConfiguration.MaxInMemoryCacheDataSize, true);
            RemoveEntryOnMaxCapacityReached(_cacheConfiguration.MaxInMemoryCacheDataEntries, true);

            RemoveOnReachingSizeLimit(cacheEntry.Size, _cacheConfiguration.MaxCacheDataSize);
            RemoveEntryOnMaxCapacityReached(_cacheConfiguration.MaxCacheDataEntries);

        }

        private void RemoveOnReachingSizeLimit(int cacheEntrySize, int maxCacheSize, bool inMemory = false)
        {
            var neededCapacity = GetSize(inMemory) + cacheEntrySize;

            if (neededCapacity > maxCacheSize)
            {
                var toRemove = new List<string>();

                var orderedEntries = inMemory? Entries.Where(x => x.Value.IsInMemory).OrderBy(x => x.Value.LastAccessTime): Entries.OrderBy(x => x.Value.LastAccessTime);
                
                foreach (var orderedEntry in orderedEntries)
                {
                    toRemove.Add(orderedEntry.Key);
                    neededCapacity -= orderedEntry.Value.Size;
                    if (maxCacheSize >= neededCapacity)
                        break;
                }

                foreach (var item in toRemove)
                {
                    if (inMemory)
                    {
                        Entries[item].RemoveValue();
                    }
                    else
                    {
                        _storage.Remove(Entries[item].FileName);
                        Entries.Remove(item);
                    }
                }
            }
        }

        private void RemoveEntryOnMaxCapacityReached(int maxEntries, bool inMemory = false)
        {
            var entries = inMemory ? Entries.Where(x => x.Value.IsInMemory) : Entries;
            var keyValuePairs = entries as KeyValuePair<string, ICacheEntry>[] ?? entries.ToArray();
            if (maxEntries != keyValuePairs.Count()) 
                return;
            
            var entryToRemove = keyValuePairs.OrderBy(x => x.Value.LastAccessTime).Select(x => x.Key).FirstOrDefault();
            if (string.IsNullOrEmpty(entryToRemove)) 
                return;
            if (inMemory)
            {
                Entries[entryToRemove].RemoveValue();
            }
            else
            {
                _storage.Remove(Entries[entryToRemove].FileName);
                Entries.Remove(entryToRemove);
            }
        }

        public Int32 GetSize(bool inMemory)
        {
            return inMemory ? Entries.Where(x => x.Value.IsInMemory).Sum(x => x.Value.Size) : Entries.Sum(x => x.Value.Size);
        }
        public Int32 Count(bool inMemory)
        {
            return inMemory ? Entries.Count(x => x.Value.IsInMemory) : Entries.Count;
        }
    }
}
