using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache;
using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;
using System;

namespace CacheTests.VersionTests
{
    [TestClass]
    public class When_version_is_stored_in_cache
    {
        [TestMethod]
        public void should_erase_cache_on_if_versions_differ()
        {
            var cache = new Cache(GetCacheContainerWithSpecificVersion(new Version(1, 1)));

            //when at least one value set cache is written
            cache.Set("some_entry", "some cache");

            //cache will be cleanued up if versions in storage and executing assembly differ
            new Cache(GetCacheContainerWithSpecificVersion(new Version(6, 1)));

            TestStorage.KeyToStreams.Should().BeEmpty();
        }

        [TestMethod]
        public void should_keep_cache_if_versions_same()
        {

            var cache = new Cache(GetCacheContainerWithSpecificVersion(new Version(1, 1)));
            
            //when at least one value set cache is written
            cache.Set("some_entry", "some cache");

            //cache should not be cleanued up if versions in storage and executing assembly differ
            new Cache(GetCacheContainerWithSpecificVersion(new Version(1, 1)));

            TestStorage.KeyToStreams.Should().NotBeEmpty();
        }
        
        private CacheContainer GetCacheContainerWithSpecificVersion(Version version)
        {
            var cacheContainer = new CacheContainer();
            cacheContainer.Register<IVersionProvider, TestVersionProvider>().WithValue("version", version);
            cacheContainer.Register<IStorage, TestStorage>();
            cacheContainer.Register<ISerializer, ProtoBufSerializer>().WithDependency("storage", typeof(IStorage).FullName).WithValue("userTypes", null);
            return cacheContainer;
        }
    }
}
