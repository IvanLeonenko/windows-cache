using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Rakuten.Framework.Cache;
using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;
using Rakuten.Framework.Cache.WindowsStore;

namespace Windows80CacheTests.CacheTests
{
    [TestClass]
    public class When_cache_created_with_windows_80_store_factory
    {
        [TestMethod]
        public async Task default_cache_construction_without_parameters_should_work()
        {
            var cache = await WindowsStoreCacheFactory.GetCache();

            await cache.Set("key1", "stringValue");
            cache.Size.Should().BeGreaterThan(0);
            cache.Count.Should().Be(1);
            cache.Get<string>("key1").Result.Value.Should().Be("stringValue");
            await cache.Remove("key1");
            cache.Count.Should().Be(0);
            cache.Size.Should().Be(0);
            cache.Get<string>("key1").Result.Should().BeNull();
        }

        [TestMethod]
        public async Task named_cache_construction_without_parameters_should_work()
        {
            var cache = await WindowsStoreCacheFactory.GetCache(null, "someCacheName");

            await cache.Set("key1", "stringValue");
            cache.Size.Should().BeGreaterThan(0);
            cache.Count.Should().Be(1);
            cache.Get<string>("key1").Result.Value.Should().Be("stringValue");
            await cache.Remove("key1");
            cache.Count.Should().Be(0);
            cache.Size.Should().Be(0);
            cache.Get<string>("key1").Result.Should().BeNull();
        }

        [TestMethod]
        public async Task custom_config_cache_construction_should_work()
        {
            var cacheConfig = new CacheConfiguration(1024, 5, 1024, 5, TimeSpan.FromMinutes(1));

            var cache = await WindowsStoreCacheFactory.GetCache(cacheConfig, null, "someCacheName2");

            await cache.Set("key1", "stringValue");
            cache.Size.Should().BeGreaterThan(0);
            cache.Count.Should().Be(1);
            cache.Get<string>("key1").Result.Value.Should().Be("stringValue");
            await cache.Remove("key1");
            cache.Count.Should().Be(0);
            cache.Size.Should().Be(0);
            cache.Get<string>("key1").Result.Should().BeNull();
        }

        [TestMethod]
        public async Task custom_container_and_config_cache_construction_should_work()
        {
            var cacheContainer = new CacheContainer();
            cacheContainer.Register<ILogger, StoreDebugCacheLogger>();
            cacheContainer.Register<IVersionProvider, WindowsStoreApplicationVersionProvider>();
            cacheContainer.Register<IStorage, StoreStorage>().WithValue("cacheName", "cacheName");
            cacheContainer.Register<ISerializer, ProtoBufSerializer>().WithDependency("storage", typeof(IStorage).FullName).WithValue("userTypes", null);
            
            var cacheConfig = new CacheConfiguration(1024, 5, 1024, 5, TimeSpan.FromMinutes(1));

            var cache = await WindowsStoreCacheFactory.GetCache(cacheContainer, cacheConfig);

            await cache.Set("key1", "stringValue");
            cache.Size.Should().BeGreaterThan(0);
            cache.Count.Should().Be(1);
            cache.Get<string>("key1").Result.Value.Should().Be("stringValue");
            await cache.Remove("key1");
            cache.Count.Should().Be(0);
            cache.Size.Should().Be(0);
            cache.Get<string>("key1").Result.Should().BeNull();
            await cache.Clear();
        }
    }
}
