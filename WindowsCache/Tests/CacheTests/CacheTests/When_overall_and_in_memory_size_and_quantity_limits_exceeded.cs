using System;
using System.IO;
using System.Threading;
using CacheTests.VersionTests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache;
using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;

namespace CacheTests.CacheTests
{
    [TestClass]
    public class When_overall_and_in_memory_size_and_quantity_limits_exceeded
    {
        Cache _cache;
        private readonly DateTime _dateTime = DateTime.Now;

        [TestInitialize]
        public async void Initialize()
        {
            var cacheContainer = new CacheContainer();
            cacheContainer.Register<ILogger, TestLogger>();
            cacheContainer.Register<IVersionProvider, TestVersionProvider>().WithValue("version", new Version("1.0"));
            cacheContainer.Register<IStorage, TestStorage>();
            cacheContainer.Register<ISerializer, ProtoBufSerializer>().WithDependency("storage", typeof(IStorage).FullName).WithValue("userTypes", null);

            var cacheConfiguration = new CacheConfiguration(900, 6, 800, 5);

            _cache = new Cache(cacheContainer, cacheConfiguration);
            await _cache.Initialize();
            await _cache.Set("stringKey1", "stringValue1");
            Thread.Sleep(20);
            await _cache.Set("stringKey2", "stringValue2");
            Thread.Sleep(20);
            await _cache.Set("stringKey3", "stringValue3");
            Thread.Sleep(20);
            await _cache.Set("stringKey4", "stringValue4");
            Thread.Sleep(20);
            await _cache.Set("Int32Key", 42);
            Thread.Sleep(20);
            await _cache.Set("DateTimeKey", _dateTime);
            Thread.Sleep(20);
            await _cache.Set("floatKey", 13.37f);
            Thread.Sleep(20);
            await _cache.Set("decimalKey", 13.37m);
        }

        [TestMethod]
        public void cache_should_return_only_most_recent_values_above_the_limit()
        {
            _cache.Get<string>("stringKey1").Result.Should().BeNull();
            _cache.Get<string>("stringKey2").Result.Should().BeNull();
            _cache.Get<string>("stringKey3").Result.Value.Should().Be("stringValue3");
            _cache.Get<string>("stringKey4").Result.Value.Should().Be("stringValue4");
            _cache.Get<Int32>("Int32Key").Result.Value.Should().Be(42);
            _cache.Get<DateTime>("DateTimeKey").Result.Value.Should().Be(_dateTime);
            _cache.Get<float>("floatKey").Result.Value.Should().Be(13.37f);
            _cache.Get<decimal>("decimalKey").Result.Value.Should().Be(13.37m);
        }

        [TestMethod]
        public void cache_should_have_in_memory_size_691_bytes()
        {
            _cache.InMemorySize.Should().Be(691);
        }

        [TestMethod]
        public void cache_should_have_size_843_bytes()
        {
            _cache.Size.Should().Be(843);
        }

        [TestMethod]
        public void in_memory_cache_entries_quantity_should_be_5()
        {
            _cache.InMemoryCount.Should().Be(5);
        }

        [TestMethod]
        public void cache_entries_quantity_should_be_6()
        {
            _cache.Count.Should().Be(6);
        }
    }
}
