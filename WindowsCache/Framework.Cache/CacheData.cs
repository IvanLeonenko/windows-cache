﻿using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Framework.Cache.ProtoBuf;
using Framework.Cache.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Cache
{
    class CacheData
    {
        public const int CacheEntrySize = 128;
        private const string CacheName = "cache.protobuf";
        private readonly ISerializer _serializer;
        private readonly IStorage _storage;
        private readonly ILogger _logger;
        private Dictionary<string, ICacheEntry> _entries;
        private readonly CacheConfiguration _cacheConfiguration;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        const int NotChanged = 0;
        const int Changed = 1;
        private int _cacheDataChanged = NotChanged;
        //private int _shouldCheckLimits = NotChanged;
        //private int _shouldCheckInMemoryLimits = NotChanged;

        public CacheData(IStorage storage, ISerializer serializer, CacheConfiguration cacheConfiguration, ILogger logger)
        {
            _logger = logger;
            _cacheConfiguration = cacheConfiguration;
            _serializer = serializer;
            _storage = storage;
        }

        public async Task RestoreState()
        {
            await _serializer.Initialize();
            var stream = await _storage.GetStream(CacheName);

            _entries = (stream == null) ? new Dictionary<string, ICacheEntry>() : _serializer.Deserialize<Dictionary<string, ICacheEntry>>(stream);
            
            #region load values to memory: warning task.Wait() is used
            //var count = 0;
            //var size = 0;
            
            //foreach (var cacheEntry in _entries)
            //{
            //    while (count <= _cacheConfiguration.MaxInMemoryCacheDataEntries &&
            //            size <= _cacheConfiguration.MaxInMemoryCacheDataSize)
            //    {
            //        count++;
            //        size += cacheEntry.Value.Size;
            //        var methodInfo =
            //            GetType()
            //                .GetRuntimeMethod("Get", new[] {typeof (string), typeof (bool)})
            //                .MakeGenericMethod(new[] {cacheEntry.Value.Type});
            //        var @out = methodInfo.Invoke(this, new object[] {cacheEntry.Key, false});
            //        if (@out != null)
            //        {
            //            var task = ((Task) @out);
            //            if (task != null)
            //            {
            //                task.Wait();
            //            }
            //        }

            //    }
            //}
            #endregion

            Size = _entries.Sum(x => x.Value.Size);
            InMemorySize = _entries.Where(x => x.Value.IsInMemory).Sum(x => x.Value.Size);
            Count = _entries.Count;
            InMemorySize = _entries.Count(x => x.Value.IsInMemory);
            _logger.Info("Cache data initialized.");
        }

        public async Task Set<T>(string key, T value, TimeSpan? timeToLive = null)
        {
            var currentTime = DateTime.UtcNow;
            var createdTime = currentTime;

            _lock.EnterReadLock();
            try
            {
                if (_entries.ContainsKey(key))
                {
                    var existingCacheEntry = _entries[key] as CacheEntry<T>;
                    if (existingCacheEntry != null)
                    {
                        createdTime = existingCacheEntry.CreatedTime;
                    }
                }
            } finally { _lock.ExitReadLock(); }

            var expirationTime = currentTime + (timeToLive.HasValue ? timeToLive.Value : _cacheConfiguration.DefaultTimeToLive);

            var cacheEntry = new CacheEntry<T> { Value = value, Type = typeof(T), IsInMemory = true, CreatedTime = createdTime, ModifiedTime = currentTime, LastAccessTime = currentTime, ExpirationTime = expirationTime};

            await SetSizeAndType(cacheEntry, value);

            await AddEntry(key, cacheEntry);

            Interlocked.Exchange(ref _cacheDataChanged, Changed);
            //Interlocked.Exchange(ref _shouldCheckInMemoryLimits, Changed);
            //Interlocked.Exchange(ref _shouldCheckLimits, Changed);
        }

        public async Task<CacheEntry<T>> Get<T>(string key, bool doLimitCheck = true)
        {
            CacheEntry<T> resultEntry = null;

            _lock.EnterReadLock();
            try
            {
                resultEntry = _entries.ContainsKey(key) ? _entries[key] as CacheEntry<T> : null;
            }
            finally { _lock.ExitReadLock(); }

            if (resultEntry == null)
            {
                await RemoveEntry(key);
            }
            else if (resultEntry.IsExpired)
            {
                await RemoveEntry(key);
                await _storage.Remove(resultEntry.FileName);
                resultEntry = null;
            }
            else
            {
                if (!_cacheConfiguration.InMemoryOnly && !resultEntry.IsInMemory)
                {
                    if (resultEntry.EntryType == EntryType.TemplateType)
                    {
                        resultEntry.Value = _serializer.Deserialize<T>(await _storage.GetStream(resultEntry.FileName));
                    }
                    else if (resultEntry.EntryType == EntryType.Binary)
                    {
                        object obj = await _storage.GetBytes(resultEntry.FileName);
                        resultEntry.Value = (T) obj;
                    }
                    else
                    {
                        object obj = await _storage.GetString(resultEntry.FileName);
                        resultEntry.Value = (T) obj;
                    }
                    
                    //InMemoryCount++;
                    //InMemorySize += resultEntry.Size;
                    
                    if (doLimitCheck)
                    {
                        //Interlocked.Exchange(ref _shouldCheckInMemoryLimits, Changed);
                        await RemoveOnReachingLimit(InMemorySize + resultEntry.Size, _cacheConfiguration.MaxInMemoryCacheDataSize, true, x => x.Size);
                        await RemoveOnReachingLimit(InMemoryCount + 1, _cacheConfiguration.MaxInMemoryCacheDataEntries, true, x => 1);
                    }

                    resultEntry.IsInMemory = true;
                }

                resultEntry.LastAccessTime = DateTime.UtcNow;
            }
            Interlocked.Exchange(ref _cacheDataChanged, Changed);
            return resultEntry;
        }

        public async Task SaveCacheMappings()
        {
            if (Interlocked.Exchange(ref _cacheDataChanged, NotChanged) == Changed)
            {
                if (!_cacheConfiguration.InMemoryOnly)
                {
                    Stream entriesStream = null;
                    
                    var toRemove = (from entry in _entries where entry.Value.IsExpired select entry.Key).ToList();

                    foreach (var item in toRemove)
                    {
                        await RemoveEntry(item);
                    }

                    _lock.EnterReadLock();
                    try
                    {
                        entriesStream = _serializer.Serialize(_entries);
                    }
                    finally
                    {
                        _lock.ExitReadLock();
                    }

                    await _storage.Write(CacheName, entriesStream);
                }
            }
        }

        public async Task CheckLimits()
        {
            //if (Interlocked.Exchange(ref _shouldCheckLimits, NotChanged) == Changed)
            //{
            //    await RemoveOnReachingLimit(Size, _cacheConfiguration.MaxCacheDataSize, false, x => x.Size);
            //    await RemoveOnReachingLimit(Count, _cacheConfiguration.MaxCacheDataEntries, false, x => 1);
            //}

            //if (Interlocked.Exchange(ref _shouldCheckInMemoryLimits, NotChanged) == Changed)
            //{
            //    await RemoveOnReachingLimit(InMemorySize, _cacheConfiguration.MaxInMemoryCacheDataSize, true, x => x.Size);
            //    await RemoveOnReachingLimit(InMemoryCount, _cacheConfiguration.MaxInMemoryCacheDataEntries, true, x => 1);
            //}
        }

        public async Task Clear()
        {
            if (!_cacheConfiguration.InMemoryOnly)
            {
                foreach (var cacheEntry in _entries)
                    await _storage.Remove(cacheEntry.Value.FileName);
                await _storage.Remove(CacheName);
            }

            Count = 0;
            Size = 0;
            InMemorySize = 0;
            InMemoryCount = 0;

            _lock.EnterWriteLock();
            try
            {
                _entries.Clear();
            }
            finally { _lock.ExitWriteLock(); }
            
            _logger.Info("Cache data cleared.");
        }

        public async Task Remove(string key, bool inMemory = false)
        {
            await RemoveEntry(key, inMemory);
        }

        private async Task SetSizeAndType<T>(CacheEntry<T> cacheEntry, T value)
        {
            var stringValue = value as string;
            var bytesValue = value as byte[];
            if (stringValue != null)
            {
                cacheEntry.EntryType = EntryType.String;
                cacheEntry.Size = Convert.ToInt32(stringValue.Length) * 2 + CacheEntrySize;
                await _storage.Write(cacheEntry.FileName, stringValue);
            }
            else if (bytesValue != null)
            {
                cacheEntry.EntryType = EntryType.Binary;
                cacheEntry.Size = Convert.ToInt32(bytesValue.Length) + CacheEntrySize;
                await _storage.Write(cacheEntry.FileName, bytesValue);
            }
            else
            {
                using (var stream = _serializer.Serialize(value))
                {
                    cacheEntry.Size = Convert.ToInt32(stream.Length) + CacheEntrySize;
                    await _storage.Write(cacheEntry.FileName, stream);
                }
            }

            await RemoveOnReachingLimit(InMemorySize + cacheEntry.Size, _cacheConfiguration.MaxInMemoryCacheDataSize, true, x => x.Size);
            await RemoveOnReachingLimit(InMemoryCount + 1, _cacheConfiguration.MaxInMemoryCacheDataEntries, true, x => 1);

            await RemoveOnReachingLimit(Size + cacheEntry.Size, _cacheConfiguration.MaxCacheDataSize, false, x => x.Size);
            await RemoveOnReachingLimit(Count + 1, _cacheConfiguration.MaxCacheDataEntries, false, x => 1);
        }

        private async Task RemoveOnReachingLimit(int target, int max, bool inMemory, Func<ICacheEntry, int> nextValueFunc)
        {
            if (target > max)
            {
                var toRemove = new List<string>();
                IOrderedEnumerable<KeyValuePair<string, ICacheEntry>> orderedEntries;
                _lock.EnterReadLock();
                try
                {
                    orderedEntries = inMemory ? _entries.Where(x => x.Value.IsInMemory).OrderBy(x => x.Value.LastAccessTime) : _entries.OrderBy(x => x.Value.LastAccessTime);

                }
                finally { _lock.ExitReadLock(); }

                foreach (var orderedEntry in orderedEntries)
                {
                    toRemove.Add(orderedEntry.Key);
                    target -= nextValueFunc(orderedEntry.Value);
                    if (max >= target)
                        break;
                }

                foreach (var item in toRemove)
                {
                    await RemoveEntry(item, inMemory);
                }
            }
        }


        private async Task AddEntry<T>(string key, CacheEntry<T> cacheEntry)
        {
            await RemoveEntry(key);
            
            InMemoryCount++;
            InMemorySize += cacheEntry.Size;

            Count++;
            Size += cacheEntry.Size;

            _lock.EnterWriteLock();
            try
            {
                _entries[key] = cacheEntry;
            }
            finally { _lock.ExitWriteLock(); }
        }

        private async Task RemoveEntry(string key, bool inMemory = false)
        {
            ICacheEntry entry = null;

            _lock.EnterReadLock();
            try
            {
                if (!_entries.ContainsKey(key))
                    return;
                entry = _entries[key];
            }
            finally { _lock.ExitReadLock(); }

            if (entry == null)
                return;

            if (inMemory)
            {
                _lock.EnterWriteLock();
                try
                {
                    entry.RemoveValue();
                }
                finally { _lock.ExitWriteLock(); }
                    
                InMemoryCount--;
                InMemorySize -= entry.Size;
            }
            else
            {
                if (entry.IsInMemory)
                {
                    InMemoryCount--;
                    InMemorySize -= entry.Size;
                }
                Count--;
                Size -= entry.Size;

                await _storage.Remove(entry.FileName);

                _lock.EnterWriteLock();
                try
                {
                    _entries.Remove(key);
                }
                finally { _lock.ExitWriteLock(); }
            }
        }

        public Int32 Count { get; set; }
        public Int32 InMemoryCount { get; set; }
        public Int32 Size { get; private set; }
        public Int32 InMemorySize { get; private set; }
    }
}
