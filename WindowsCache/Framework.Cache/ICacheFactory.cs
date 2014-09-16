using System;
using System.Collections.Generic;

namespace Framework.Cache
{
    public interface ICacheFactory
    {
        ICache GetCache(IEnumerable<Type> userTypes = null, string cacheName = "default");
        ICache GetCache(CacheConfiguration cacheConfiguration, IEnumerable<Type> userTypes = null, string cacheName = "default");
        ICache GetCache(CacheContainer cacheContainer, CacheConfiguration cacheConfiguration);

        ICache GetInMemoryCache(IEnumerable<Type> userTypes = null, string cacheName = "default");
        ICache GetInMemoryCache(CacheConfiguration cacheConfiguration, IEnumerable<Type> userTypes = null, string cacheName = "default");
        ICache GetInMemoryCache(CacheContainer cacheContainer, CacheConfiguration cacheConfiguration);

    }
}
