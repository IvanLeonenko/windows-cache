using System;

namespace Rakuten.Framework.Cache
{
    public class CacheConfiguration
    {
        public Int32 MaxCacheDataSize { get; set; }
        public Int32 MaxCacheDataEntries { get; set; }
        public Int32 MaxInMemoryCacheDataSize { get; set; }
        public Int32 MaxInMemoryCacheDataEntries { get; set; }
        public bool InMemoryOnly { get; set; }

        public CacheConfiguration(int maxCacheDataSize, int maxCacheDataEntries, int maxInMemoryCacheDataSize, int maxInMemoryCacheDataEntries)
        {
            MaxCacheDataSize = maxCacheDataSize;
            MaxCacheDataEntries = maxCacheDataEntries;
            MaxInMemoryCacheDataSize = maxInMemoryCacheDataSize;
            MaxInMemoryCacheDataEntries = maxInMemoryCacheDataEntries;
        }
    }
}
