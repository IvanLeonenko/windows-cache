using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Cache;
using Framework.Cache.ProtoBuf;
using Framework.Cache.Storage;

namespace CacheTests.VersionTests
{
    [TestClass]
    public class When_no_version_is_stored_after_cache_create
    {
        [TestMethod]
        public async Task should_save_version_on_cache_create()
        {
            var version = new Version(5, 32);
            var cacheContainer = new CacheContainer();
            cacheContainer.Register<ILogger, TestLogger>();
            cacheContainer.Register<IVersionProvider, TestVersionProvider>().WithValue("version", version);
            cacheContainer.Register<IStorage, TestStorage>().AsSingleton();
            cacheContainer.Register<ISerializer, ProtoBufSerializer>().WithDependency("storage", typeof(IStorage).FullName).WithValue("userTypes", null);
            var cacheConfiguration = new CacheConfiguration(1024, 5, 1024, 5);
            var cache = new Cache(cacheContainer, cacheConfiguration);
            await cache.Initialize();
            var storage = (TestStorage)cacheContainer.Resolve<IStorage>();
            storage.KeyToStrings[Cache.VersionEntryName].Should().BeEquivalentTo(version.ToString());
        }
    }
}
