using System;

namespace Framework.Cache
{
    public class CacheConfiguration
    {
        public Int32 MaxCacheDataSize { get; set; }
        public Int32 MaxCacheDataEntries { get; set; }
        public Int32 MaxInMemoryCacheDataSize { get; set; }
        public Int32 MaxInMemoryCacheDataEntries { get; set; }
        public bool InMemoryOnly { get; set; }
        public TimeSpan DefaultTimeToLive { get; set; }
        public Int32 PeriodicOperationsDueTime { get; set; }
        public Int32 PeriodicOperationsPeriodTime { get; set; }

        public CacheConfiguration(int maxCacheDataSize, int maxCacheDataEntries, int maxInMemoryCacheDataSize, int maxInMemoryCacheDataEntries)
            : this(maxCacheDataSize, maxCacheDataEntries, maxInMemoryCacheDataSize, maxInMemoryCacheDataEntries, TimeSpan.FromDays(7)) { }

        public CacheConfiguration(int maxCacheDataSize, int maxCacheDataEntries, int maxInMemoryCacheDataSize, int maxInMemoryCacheDataEntries, TimeSpan timeToLive)
        {
            MaxCacheDataSize = maxCacheDataSize;
            MaxCacheDataEntries = maxCacheDataEntries;
            MaxInMemoryCacheDataSize = maxInMemoryCacheDataSize;
            MaxInMemoryCacheDataEntries = maxInMemoryCacheDataEntries;
            DefaultTimeToLive = timeToLive;
            PeriodicOperationsDueTime = 3000;
            PeriodicOperationsPeriodTime = 10000;
        }
    }
}
