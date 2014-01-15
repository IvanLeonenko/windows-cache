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
    public class When_overall_and_in_memory_size_limits_exceeded
    {
        Cache _cache;

        [TestInitialize]
        public void Initialize()
        {
            var cacheContainer = new CacheContainer();
            cacheContainer.Register<IVersionProvider, TestVersionProvider>().WithValue("version", new Version("1.0"));
            cacheContainer.Register<IStorage, TestStorage>();
            cacheContainer.Register<ISerializer, ProtoBufSerializer>().WithDependency("storage", typeof(IStorage).FullName).WithValue("userTypes", null);

            var cacheConfiguration = new CacheConfiguration(500, 10, 500, 5, 2048);

            _cache = new Cache(cacheContainer, cacheConfiguration);
            _cache.Set("stringKey1", "stringValue1");
            Thread.Sleep(30);
            _cache.Set("stringKey2", "stringValue2");
            Thread.Sleep(30);
            _cache.Set("stringKey3", "stringValue3");
            Thread.Sleep(30);
            _cache.Set("stringKey4", "stringValue4");
            Thread.Sleep(30);
            _cache.Set("Int32Key", 42);
        }

        [TestMethod]
        public void cache_should_return_only_most_recent_values_above_the_limit()
        {
            _cache.Get<string>("stringKey1").Should().BeNull();
            _cache.Get<string>("stringKey2").Should().BeNull();
            _cache.Get<string>("stringKey3").Value.Should().Be("stringValue3");
            _cache.Get<string>("stringKey4").Value.Should().Be("stringValue4");
            _cache.Get<Int32>("Int32Key").Value.Should().Be(42);
        }

        [TestMethod]
        public void cache_should_have_in_memory_size_434_bytes()
        {
            _cache.Size(true).Should().Be(434);
        }

        [TestMethod]
        public void cache_should_have_size_434_bytes()
        {
            _cache.Size().Should().Be(434);
        }

        [TestMethod]
        public void in_memory_cache_entries_quantity_should_be_3()
        {
            _cache.Count(true).Should().Be(3);
        }

        [TestMethod]
        public void cache_entries_quantity_should_be_3()
        {
            _cache.Count().Should().Be(3);
        }
    }
}
