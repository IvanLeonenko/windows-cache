using System;
using CacheTests.VersionTests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache;
using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;

namespace CacheTests.CacheTests
{
    [TestClass]
    public class When_limits_not_exceeded
    {
        private Cache _cache;
        readonly DateTime _dateTime = DateTime.Now;

        [TestInitialize]
        public void Initialize()
        {
            var cacheContainer = new CacheContainer();
            cacheContainer.Register<IVersionProvider, TestVersionProvider>().WithValue("version", new Version("1.0"));
            cacheContainer.Register<IStorage, TestStorage>();
            cacheContainer.Register<ISerializer, ProtoBufSerializer>().WithDependency("storage", typeof(IStorage).FullName).WithValue("userTypes", null);

            var cacheConfiguration = new CacheConfiguration(2048, 6, 1024, 6);

            _cache = new Cache(cacheContainer, cacheConfiguration);
            
            _cache.Set("strinKey1", "stringValue1");
            _cache.Set("strinKey2", "stringValue2");
            _cache.Set("someBytes", new byte[]{12,32,43});
            _cache.Set("Int32Key1", 1);
            _cache.Set("dateTimeKey1", _dateTime);

        }
        
        [TestMethod]
        public void cache_should_return_all_stored_values()
        {
            _cache.Get<string>("strinKey1").Value.Should().Be("stringValue1");
            _cache.Get<string>("strinKey2").Value.Should().Be("stringValue2");
            _cache.Get<byte[]>("someBytes").Value.Should().BeEquivalentTo(new byte[] { 12, 32, 43 });
            _cache.Get<int>("Int32Key1").Value.Should().Be(1);
            _cache.Get<DateTime>("dateTimeKey1").Value.Should().Be(_dateTime);
        }

        [TestMethod]
        public void cache_should_have_in_memory_size_706_bytes()
        {
            _cache.InMemorySize.Should().Be(706);
        }

        [TestMethod]
        public void cache_should_have_size_706_bytes()
        {
            _cache.Size.Should().Be(706);
        }


        [TestMethod]
        public void in_memory_cache_entries_quantity_should_be_5()
        {
            _cache.InMemoryCount.Should().Be(5);
        }

        [TestMethod]
        public void cache_entries_quantity_should_be_5()
        {
            _cache.Count.Should().Be(5);
        }
    }
}
