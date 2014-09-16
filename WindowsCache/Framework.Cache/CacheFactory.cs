using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Cache
{
    public abstract class CacheFactory
    {
        protected static Type GetDerivedType<T>() where T : CacheFactory
        {
            return typeof(T);
        }

        public async Task<ICache> Cache(IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return await Cache(GetDefaultCacheContainer(userTypes, cacheName), GetDefaultCacheConfiguration());
        }

        public async Task<ICache> Cache(CacheConfiguration cacheConfiguration, IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return await Cache(GetDefaultCacheContainer(userTypes, cacheName), cacheConfiguration);
        }

        public async Task<ICache> Cache(CacheContainer cacheContainer, CacheConfiguration cacheConfiguration)
        {
            var cache = new Cache(cacheContainer, cacheConfiguration);
            await cache.Initialize();
            return cache;
        }

        public async Task<ICache> InMemoryCache(IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return await InMemoryCache(GetDefaultCacheContainer(userTypes, cacheName), GetDefaultInMemoryCacheConfiguration());
        }

        public async Task<ICache> InMemoryCache(CacheConfiguration cacheConfiguration, IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return await InMemoryCache(GetDefaultCacheContainer(userTypes, cacheName), cacheConfiguration);
        }

        public async Task<ICache> InMemoryCache(CacheContainer cacheContainer, CacheConfiguration cacheConfiguration)
        {
            cacheConfiguration.InMemoryOnly = true;
            return await Cache(cacheContainer, cacheConfiguration);
        }

        public abstract CacheConfiguration GetDefaultCacheConfiguration();

        public abstract CacheConfiguration GetDefaultInMemoryCacheConfiguration();
        
        public abstract CacheContainer GetDefaultCacheContainer(IEnumerable<Type> userTypes, string cacheName);
    }
}
