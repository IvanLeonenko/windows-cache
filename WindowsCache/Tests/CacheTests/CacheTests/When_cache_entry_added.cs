using System;
using System.Threading.Tasks;
using CacheTests.VersionTests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache;
using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;

namespace CacheTests.CacheTests
{
    [TestClass]
    public class When_cache_entry_added
    {
        private Cache _cache;
        CacheContainer _cacheContainer;

        [TestInitialize]
        public async void Initialize()
        {
            _cacheContainer = new CacheContainer();
            _cacheContainer.Register<ILogger, TestLogger>();
            _cacheContainer.Register<IVersionProvider, TestVersionProvider>().WithValue("version", new Version("1.0"));
            _cacheContainer.Register<IStorage, TestStorage>().AsSingleton();
            _cacheContainer.Register<ISerializer, ProtoBufSerializer>().WithDependency("storage", typeof(IStorage).FullName).WithValue("userTypes", null);

            var cacheConfiguration = new CacheConfiguration(2048, 6, 2048, 5);
            _cache = new Cache(_cacheContainer, cacheConfiguration);
            await _cache.Initialize();
            await _cache.Set("key1", "stringValue");
            await _cache.Set("key2", 42);
            await _cache.Set("key3", new byte[] { 12, 23, 34 });
        }

        [TestMethod]
        public async Task its_old_value_should_be_rewritten()
        {
            await _cache.Set("key1", "otherStringValue");
            await _cache.Set("key2", 142);
            await _cache.Set("key3", new byte[] { 56, 67, 78 });

            _cache.Get<string>("key1").Result.Value.Should().Be("otherStringValue");
            _cache.Get<Int32>("key2").Result.Value.Should().Be(142);
            _cache.Get<byte[]>("key3").Result.Value.Should().BeEquivalentTo(new byte[] { 56, 67, 78 });
        }

        [TestMethod]
        public void it_should_be_available()
        {
            _cache.Get<string>("key1").Result.Value.Should().Be("stringValue");
            _cache.Get<Int32>("key2").Result.Value.Should().Be(42);
            _cache.Get<byte[]>("key3").Result.Value.Should().BeEquivalentTo(new byte[] { 12, 23, 34 });
        }

        
    }
}
