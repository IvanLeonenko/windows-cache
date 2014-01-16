using System;
using System.Collections.Generic;

namespace Rakuten.Framework.Cache
{
    public abstract class CacheFactory
    {
        public ICache Cache(IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return Cache(GetDefaultCacheContainer(userTypes, cacheName), GetDefaultCacheConfiguration());
        }

        public ICache Cache(CacheConfiguration cacheConfiguration, IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return Cache(GetDefaultCacheContainer(userTypes, cacheName), cacheConfiguration);
        }

        public ICache Cache(CacheContainer cacheContainer, CacheConfiguration cacheConfiguration)
        {
            return new Cache(cacheContainer, cacheConfiguration);
        }

        public ICache InMemoryCache(IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return InMemoryCache(GetDefaultCacheContainer(userTypes, cacheName), GetDefaultInMemoryCacheConfiguration());
        }

        public ICache InMemoryCache(CacheConfiguration cacheConfiguration, IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return InMemoryCache(GetDefaultCacheContainer(userTypes, cacheName), cacheConfiguration);
        }

        public ICache InMemoryCache(CacheContainer cacheContainer, CacheConfiguration cacheConfiguration)
        {
            cacheConfiguration.InMemoryOnly = true;
            return Cache(cacheContainer, cacheConfiguration);
        }

        public abstract CacheConfiguration GetDefaultCacheConfiguration();

        public abstract CacheConfiguration GetDefaultInMemoryCacheConfiguration();
        
        public abstract CacheContainer GetDefaultCacheContainer(IEnumerable<Type> userTypes, string cacheName);
    }
}
