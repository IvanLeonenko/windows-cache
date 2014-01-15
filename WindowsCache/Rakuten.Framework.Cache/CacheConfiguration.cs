using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakuten.Framework.Cache
{
    public class CacheConfiguration
    {
        public Int32 MaxCacheDataSize { get; set; }
        public Int32 MaxCacheDataEntries { get; set; }
        public Int32 MaxInMemoryCacheDataSize { get; set; }
        public Int32 MaxInMemoryCacheDataEntries { get; set; }
        public Int32 MaxInMemoryEntrySize { get; set; }

        public CacheConfiguration(int maxCacheDataSize, int maxCacheDataEntries, int maxInMemoryCacheDataSize, int maxInMemoryCacheDataEntries, int maxInMemoryEntrySize)
        {
            MaxCacheDataSize = maxCacheDataSize;
            MaxCacheDataEntries = maxCacheDataEntries;
            MaxInMemoryCacheDataSize = maxInMemoryCacheDataSize;
            MaxInMemoryCacheDataEntries = maxInMemoryCacheDataEntries;
            MaxInMemoryEntrySize = maxInMemoryEntrySize;
        }
    }
}
