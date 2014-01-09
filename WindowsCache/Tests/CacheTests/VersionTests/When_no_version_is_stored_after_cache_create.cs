using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache;
using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;

namespace CacheTests.VersionTests
{
    [TestClass]
    public class When_no_version_is_stored_after_cache_create
    {
        [TestMethod]
        public void should_save_version_on_cache_create()
        {
            var version = new Version(5, 32);
            var cacheContainer = new CacheContainer();
            cacheContainer.Register<IVersionProvider, TestVersionProvider>().WithValue("version", version);
            cacheContainer.Register<IStorage, TestStorage>();
            cacheContainer.Register<ISerializer, ProtoBufSerializer>().WithDependency("storage", typeof(IStorage).FullName).WithValue("userTypes", null);
            var cache = new Cache(cacheContainer);
            TestStorage.KeyToStrings["cache.protobuf.version"].Should().BeEquivalentTo(version.ToString());
        }
    }
}
