using System.Threading.Tasks;
using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;
using System;
using System.Collections.Generic;

namespace Rakuten.Framework.Cache.WindowsStore
{
    public sealed class PortableCacheFactory : CacheFactory
    {
        private static readonly Lazy<CacheFactory> Lazy = new Lazy<CacheFactory>(() => new PortableCacheFactory());
        private static CacheFactory Instance { get { return Lazy.Value; } }
        private PortableCacheFactory() {}

        public static async Task<ICache> GetCache(IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return await Instance.Cache(userTypes, cacheName);
        }

        public static async Task<ICache> GetCache(CacheConfiguration cacheConfiguration, IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return await Instance.Cache(cacheConfiguration, userTypes, cacheName);
        }

        public static async Task<ICache> GetCache(CacheContainer cacheContainer, CacheConfiguration cacheConfiguration)
        {
            return await Instance.Cache(cacheContainer, cacheConfiguration);
        }

        public static async Task<ICache> GetInMemotyCache(IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return await Instance.InMemoryCache(userTypes, cacheName);
        }

        public static async Task<ICache> GetInMemotyCache(CacheConfiguration cacheConfiguration, IEnumerable<Type> userTypes = null, string cacheName = "default")
        {
            return await Instance.InMemoryCache(cacheConfiguration, userTypes, cacheName);
        }

        public static async Task<ICache> GetInMemotyCache(CacheContainer cacheContainer, CacheConfiguration cacheConfiguration)
        {
            return await Instance.InMemoryCache(cacheContainer, cacheConfiguration);
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
            cacheContainer.Register<ILogger, DebugCacheLogger>();
            cacheContainer.Register<IVersionProvider, PackageVersionProvider>();
            cacheContainer.Register<IStorage, IsolatedStorage>().WithValue("cacheName", cacheName);
            cacheContainer.Register<ISerializer, ProtoBufSerializer>().WithDependency("storage", typeof(IStorage).FullName).WithValue("userTypes", userTypes);
            return cacheContainer;
        }
    }
}
