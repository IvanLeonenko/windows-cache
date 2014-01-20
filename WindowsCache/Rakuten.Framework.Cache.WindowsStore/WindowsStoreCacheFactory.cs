using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;
using System;
using System.Collections.Generic;

namespace Rakuten.Framework.Cache.WindowsStore
{
    public sealed class WindowsStoreCacheFactory : CacheFactory
    {
        private static readonly Lazy<WindowsStoreCacheFactory> Lazy = new Lazy<WindowsStoreCacheFactory>(() => new WindowsStoreCacheFactory());
        private static WindowsStoreCacheFactory Instance { get { return Lazy.Value; } }
        private WindowsStoreCacheFactory() {}

        public static ICache GetCache(IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return Instance.Cache(userTypes, cacheName);
        }

        public static ICache GetCache(CacheConfiguration cacheConfiguration, IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return Instance.Cache(cacheConfiguration, userTypes, cacheName);
        }

        public static ICache GetCache(CacheContainer cacheContainer, CacheConfiguration cacheConfiguration)
        {
            return Instance.Cache(cacheContainer, cacheConfiguration);
        }

        public static ICache GetInMemotyCache(IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return Instance.InMemoryCache(userTypes, cacheName);
        }

        public static ICache GetInMemotyCache(CacheConfiguration cacheConfiguration, IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return Instance.InMemoryCache(cacheConfiguration, userTypes, cacheName);
        }

        public static ICache GetInMemotyCache(CacheContainer cacheContainer, CacheConfiguration cacheConfiguration)
        {
            return Instance.InMemoryCache(cacheContainer, cacheConfiguration);
        }

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
