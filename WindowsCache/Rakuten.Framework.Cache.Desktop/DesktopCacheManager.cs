using System;
using System.Collections.Generic;
using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;

namespace Rakuten.Framework.Cache.Desktop
{
    public class DesktopCacheFactory : CacheFactory
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
            cacheContainer.Register<IVersionProvider, EntryAssemblyVersionProvider>();
            cacheContainer.Register<IStorage, DesktopStorage>().WithValue("cacheName", cacheName);
            cacheContainer.Register<ISerializer, ProtoBufSerializer>().WithDependency("storage", typeof(IStorage).FullName).WithValue("userTypes", userTypes);
            return cacheContainer;
        }
    }
}
