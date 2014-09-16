using System;
using System.Threading;
using CacheTests.VersionTests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Cache;
using Framework.Cache.ProtoBuf;
using Framework.Cache.Storage;
namespace CacheTests.CacheTests
{
    [TestClass]
    public class When_cache_entry_expired
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
            await _cache.Set("key1", "string1", TimeSpan.FromMilliseconds(1));
            
            Thread.Sleep(10);
        }

        [TestMethod]
        public void cache_should_return_null_for_this_entry()
        {
            _cache.Get<string>("key1").Result.Should().BeNull();
        }
    }
}
