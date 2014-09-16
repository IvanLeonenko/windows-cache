using CacheTests.VersionTests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Cache;
using Framework.Cache.ProtoBuf;
using Framework.Cache.Storage;
using System;
using System.Threading;
namespace CacheTests.CacheTests
{
    [TestClass]
    public class When_cache_set_with_async_method
    {
        private Cache _cache;
        [TestInitialize]
        public async void Initialize()
        {
            var cacheContainer = new CacheContainer();
            cacheContainer.Register<ILogger, TestLogger>();
            cacheContainer.Register<IVersionProvider, TestVersionProvider>().WithValue("version", new Version("1.0"));
            cacheContainer.Register<IStorage, TestStorage>();
            cacheContainer.Register<ISerializer, ProtoBufSerializer>().WithDependency("storage", typeof(IStorage).FullName).WithValue("userTypes", null);

            var cacheConfiguration = new CacheConfiguration(2048, 6, 2048, 5);

            _cache = new Cache(cacheContainer, cacheConfiguration);
            await _cache.Initialize();
            await _cache.SetAsync("key1", "string1");
            await _cache.SetAsync("key2", 42);
            await _cache.SetAsync("key3", new byte[] { 12, 32, 54 });
        }

        [TestMethod]
        public void cache_should_return_set_entries()
        {
            Thread.Sleep(400);
            _cache.Get<string>("key1").Result.Value.Should().Be("string1");
            _cache.Get<Int32>("key2").Result.Value.Should().Be(42);
            _cache.Get<byte[]>("key3").Result.Value.Should().BeEquivalentTo(new byte[] { 12, 32, 54 });
        }

        [TestMethod]
        public void cache_should_return_set_entries_with_async_get()
        {
            Thread.Sleep(300);
            _cache.GetAsync<string>("key1").Result.Value.Should().Be("string1");
            _cache.GetAsync<Int32>("key2").Result.Value.Should().Be(42);
            _cache.GetAsync<byte[]>("key3").Result.Value.Should().BeEquivalentTo(new byte[] { 12, 32, 54 });
        }
    }
}
