﻿using System;
using System.Threading.Tasks;
using CacheTests.VersionTests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Cache;
using Framework.Cache.ProtoBuf;
using Framework.Cache.Storage;

namespace CacheTests.CacheTests
{
    [TestClass]
    public class When_cache_cleared
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
        }


        [TestMethod]
        public async Task cache_should_be_empty()
        {
            await _cache.Set("key1", "string1");
            await _cache.Set("key2", 42);
            await _cache.Set("key3", new byte[] { 12, 23, 34 });
            _cache.Size.Should().BeGreaterThan(0);
            _cache.Count.Should().BeGreaterThan(0);
            await _cache.Clear();
            _cache.Size.Should().Be(0);
            _cache.Count.Should().Be(0);
        }

        [TestMethod]
        public async Task storage_should_be_empty()
        {
            await _cache.Set("key1", "string1");
            await _cache.Set("key2", 42);
            await _cache.Set("key3", new byte[] { 12, 23, 34 });
            _cache.Size.Should().BeGreaterThan(0);
            _cache.Count.Should().BeGreaterThan(0);
            await _cache.Clear();
            _cache.Size.Should().Be(0);
            _cache.Count.Should().Be(0);

            var testStorage = (TestStorage)_cacheContainer.Resolve<IStorage>();

            testStorage.KeyToBytes.Should().BeEmpty();
            testStorage.KeyToStreams.Should().BeEmpty();
            testStorage.KeyToStrings.Should().HaveCount(1).And.ContainKey(Cache.VersionEntryName);
        }
    }
}
