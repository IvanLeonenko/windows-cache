using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;
using System;
using System.Collections.Generic;

namespace Rakuten.Framework.Cache.WindowsStore
{
    public class WindowsStoreCacheFactory : CacheFactory
    {
        public override CacheConfiguration GetDefaultCacheConfiguration()
        {
            return new CacheConfiguration(1024 * 1024 * 30, 1000, 1024 * 1024 * 30, 1000, TimeSpan.FromDays(14));
        }

        public override CacheConfiguration GetDefaultInMemoryCacheConfiguration()
        {
            return new CacheConfiguration(1024 * 1024 * 30, 1000, 1024 * 1024 * 30, 1000, TimeSpan.FromDays(14));
        }

        public override CacheContainer GetDefaultCacheContainer(IEnumerable<Type> userTypes, string cacheName)
        {
            var cacheContainer = new CacheContainer();
            cacheContainer.Register<ILogger, StoreDebugCacheLogger>();
            cacheContainer.Register<IVersionProvider, WindowsStoreApplicationVersionProvider>();
            cacheContainer.Register<IStorage, StoreStorage>().WithValue("cacheName", cacheName);
            cacheContainer.Register<ISerializer, ProtoBufSerializer>().WithDependency("storage", typeof(IStorage).FullName).WithValue("userTypes", userTypes);
            return cacheContainer;
        }
    }
}
