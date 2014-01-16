﻿using System;
using CacheTests.VersionTests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache;
using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;

namespace CacheTests.CacheTests
{
    [TestClass]
    public class When_overall_quantity_limit_exceeded
    {
        private Cache _cache;
        readonly DateTime _dateTime = DateTime.Now;

        [TestInitialize]
        public void Initialize()
        {
            //VersionTestStorage.KeyToBytes.Clear();
            //VersionTestStorage.KeyToStreams.Clear();
            //VersionTestStorage.KeyToStrings.Clear();

            var cacheContainer = new CacheContainer();
            cacheContainer.Register<IVersionProvider, TestVersionProvider>().WithValue("version", new Version("1.0"));
            cacheContainer.Register<IStorage, TestStorage>();
            cacheContainer.Register<ISerializer, ProtoBufSerializer>().WithDependency("storage", typeof(IStorage).FullName).WithValue("userTypes", null);

            var cacheConfiguration = new CacheConfiguration(2048, 6, 2048, 5);

            _cache = new Cache(cacheContainer, cacheConfiguration);

            _cache.Set("stringKey1", "stringValue1");
            _cache.Set("stringKey2", "stringValue2");
            _cache.Set("stringKey3", "stringValue3");
            _cache.Set("someBytes", new byte[] { 12, 32, 43 });
            _cache.Set("Int32Key1", 1);
            _cache.Set("dateTimeKey1", _dateTime);
            _cache.Set("boolKey", true);
        }

        [TestMethod]
        public void cache_should_return_only_most_recent_values_above_the_limit()
        {
            _cache.Get<string>("stringKey1").Should().BeNull();

            _cache.Get<string>("stringKey2").Value.Should().Be("stringValue2");
            _cache.Get<string>("stringKey3").Value.Should().Be("stringValue3");
            _cache.Get<byte[]>("someBytes").Value.Should().BeEquivalentTo(new byte[] { 12, 32, 43 });
            _cache.Get<int>("Int32Key1").Value.Should().Be(1);
            _cache.Get<DateTime>("dateTimeKey1").Value.Should().Be(_dateTime);
            _cache.Get<bool>("boolKey").Value.Should().Be(true);
        }

        [TestMethod]
        public void cache_should_have_in_memory_size_684_bytes()
        {
            _cache.Size(true).Should().Be(684);
        }

        [TestMethod]
        public void cache_should_have_size_836_bytes()
        {
            _cache.Size().Should().Be(836);
        }

        [TestMethod]
        public void in_memory_cache_entries_quantity_should_be_5()
        {
            _cache.Count(true).Should().Be(5);
        }

        [TestMethod]
        public void cache_entries_quantity_should_be_6()
        {
            _cache.Count().Should().Be(6);
        }
    }
}
