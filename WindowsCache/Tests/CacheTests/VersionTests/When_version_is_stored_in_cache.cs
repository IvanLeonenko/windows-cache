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
            var cacheConfiguration = new CacheConfiguration(1024, 5, 1024, 5, 1024);

            InitializeCacheContainer();
            _cacheContainer.Register<IVersionProvider, TestVersionProvider>().WithValue("version", new Version(1, 1));

            var cache = new Cache(_cacheContainer, cacheConfiguration);

            //when at least one value set cache is written
            cache.Set("some_entry", 42);

            //cache will be cleanued up if versions in storage and executing assembly differ
            _cacheContainer.Register<IVersionProvider, TestVersionProvider>().WithValue("version", new Version(6, 1));
            new Cache(_cacheContainer, cacheConfiguration);
            
            _storage.KeyToStreams.Should().BeEmpty();
            cache.Get<Int32>("some_entry").Should().BeNull();
        }

        [TestMethod]
        public void should_keep_cache_if_versions_same()
        {
            var cacheConfiguration = new CacheConfiguration(1024, 5, 1024, 5, 1024);

            InitializeCacheContainer();
            _cacheContainer.Register<IVersionProvider, TestVersionProvider>().WithValue("version", new Version(1, 1));

            var cache = new Cache(_cacheContainer, cacheConfiguration);
            
            //when at least one value set cache is written
            cache.Set("some_entry", 42);

            //cache should not be cleanued up if versions in storage and executing assembly differ
            _cacheContainer.Register<IVersionProvider, TestVersionProvider>().WithValue("version", new Version(1, 1));
            new Cache(_cacheContainer, cacheConfiguration);

            _storage.KeyToStreams.Should().NotBeEmpty();
            cache.Get<Int32>("some_entry").Value.Should().Be(42);
        }

        private CacheContainer _cacheContainer;
        private TestStorage _storage;

        private CacheContainer InitializeCacheContainer()
        {
            _cacheContainer = new CacheContainer();
            _cacheContainer.Register<IStorage, TestStorage>().AsSingleton();
            _storage = (TestStorage)_cacheContainer.Resolve<IStorage>();
            _cacheContainer.Register<ISerializer, ProtoBufSerializer>().WithDependency("storage", typeof(IStorage).FullName).WithValue("userTypes", null);
            return _cacheContainer;
        }
    }
}
