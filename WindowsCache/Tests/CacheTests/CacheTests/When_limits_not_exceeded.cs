﻿using System;
using CacheTests.VersionTests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Cache;
using Framework.Cache.ProtoBuf;
using Framework.Cache.Storage;

namespace CacheTests.CacheTests
{
    [TestClass]
    public class When_limits_not_exceeded
    {
        private Cache _cache;
        readonly DateTime _dateTime = DateTime.Now;

        [TestInitialize]
        public async void Initialize()
        {
            var cacheContainer = new CacheContainer();
            cacheContainer.Register<ILogger, TestLogger>();
            cacheContainer.Register<IVersionProvider, TestVersionProvider>().WithValue("version", new Version("1.0"));
            cacheContainer.Register<IStorage, TestStorage>();
            cacheContainer.Register<ISerializer, ProtoBufSerializer>().WithDependency("storage", typeof(IStorage).FullName).WithValue("userTypes", null);

            var cacheConfiguration = new CacheConfiguration(2048, 6, 1024, 6);

            _cache = new Cache(cacheContainer, cacheConfiguration);
            await _cache.Initialize();
            await _cache.Set("strinKey1", "stringValue1");
            await _cache.Set("strinKey2", "stringValue2");
            await _cache.Set("someBytes", new byte[] { 12, 32, 43 });
            await _cache.Set("Int32Key1", 1);
            await _cache.Set("dateTimeKey1", _dateTime);

        }
        
        [TestMethod]
        public void cache_should_return_all_stored_values()
        {
            _cache.Get<string>("strinKey1").Result.Value.Should().Be("stringValue1");
            _cache.Get<string>("strinKey2").Result.Value.Should().Be("stringValue2");
            _cache.Get<byte[]>("someBytes").Result.Value.Should().BeEquivalentTo(new byte[] { 12, 32, 43 });
            _cache.Get<int>("Int32Key1").Result.Value.Should().Be(1);
            _cache.Get<DateTime>("dateTimeKey1").Result.Value.Should().Be(_dateTime);
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
