using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DesktopTests;
using DesktopTests.UserTypes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache.Desktop;
using UserTypeDesktopTests.UserTypes;

namespace UserTypeDesktopTests
{
    [TestClass]
    public class When_cache_created_with_desktop_factory
    {
        public const string PerfCacheName = "cache02";
        static When_cache_created_with_desktop_factory()
        {
            var types = new List<Type> { typeof(TestObject), typeof(SomeData), typeof(SomeData2) };
            var cache = DesktopCacheFactory.GetCache(types, PerfCacheName).Result;
        }

        [TestInitialize]
        public void Init()
        {
            
        }

        [TestMethod]
        public async Task usesr_types_storing_cache_construction_should_work()
        {
            var types = new List<Type> { typeof(SomeData), typeof(SomeData2) };
            var cache = await DesktopCacheFactory.GetCache(null, PerfCacheName);
            await cache.Clear();

            await cache.Set("key1", "stringValue");
            cache.Size.Should().BeGreaterThan(0);
            cache.Count.Should().Be(1);
            cache.Get<string>("key1").Result.Value.Should().Be("stringValue");
            await cache.Remove("key1");
            cache.Count.Should().Be(0);
            cache.Size.Should().Be(0);
            cache.Get<string>("key1").Result.Should().BeNull();
            await cache.Clear();

            var someData = new SomeData2 { Entries = new Dictionary<string, int> { { "q", 45 }, { "w", 34 } }, StringProperty = "Some string of proto" };
            await cache.Set("SomeProto", someData);
            await cache.Set("SomeProto2", someData);

            var sd2 = cache.Get<SomeData2>("SomeProto").Result;

            sd2.Value.StringProperty.Should().Be("Some string of proto");
            sd2.Value.Entries.ShouldBeEquivalentTo(new Dictionary<string, int> { { "q", 45 }, { "w", 34 } });
        }

        

        [TestMethod]
        public async Task PerfCreateEmptyCache()
        {
            var sw = Stopwatch.StartNew();
            var cache = await DesktopCacheFactory.GetCache(null, PerfCacheName);

            sw.Stop();
            Console.WriteLine("Elapsed:" + sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public async Task PerfUserTypeWrite16() { await Write100UserTypesArrays(16); }
        [TestMethod]
        public async Task PerfUserTypeWrite32() { await Write100UserTypesArrays(32); }
        [TestMethod]
        public async Task PerfUserTypeWrite64() { await Write100UserTypesArrays(64); }
        [TestMethod]
        public async Task PerfUserTypeWrite128() { await Write100UserTypesArrays(128); }
        [TestMethod]
        public async Task PerfUserTypeWrite256() { await Write100UserTypesArrays(256); }

        public static async Task Write100UserTypesArrays(int numberOfChunks)
        {
            var userTypeArrays = PerfHelpers.GetUserTypeArrays(numberOfChunks);
            var cache = await DesktopCacheFactory.GetCache(null, PerfCacheName);
            await cache.Clear();

            var sw = Stopwatch.StartNew();
            for (var i = 0; i < userTypeArrays.Length; i++)
            {
                cache.Set(i.ToString(), userTypeArrays[i]).Wait();
            }
            sw.Stop();
            Console.WriteLine("Elapsed:" + sw.ElapsedMilliseconds);
        }

        //get 


        [TestMethod]
        public async Task PerfUserTypeGet16() { await Get100KbByteArrays(16); }
        [TestMethod]
        public async Task PerfUserTypeGet32() { await Get100KbByteArrays(32); }
        [TestMethod]
        public async Task PerfUserTypeGet64() { await Get100KbByteArrays(64); }
        [TestMethod]
        public async Task PerfUserTypeGet128() { await Get100KbByteArrays(128); }
        [TestMethod]
        public async Task PerfUserTypeGet256() { await Get100KbByteArrays(256); }

        public static async Task Get100KbByteArrays(int numberOfChunks)
        {
            var userTypeArrays = PerfHelpers.GetUserTypeArrays(numberOfChunks);
            var cache = await DesktopCacheFactory.GetCache(null, PerfCacheName);
            await cache.Clear();
            var length = userTypeArrays.Length;
            for (var i = 0; i < length; i++)
            {
                cache.Set(i.ToString(), userTypeArrays[i]).Wait();
            }
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < length; i++)
            {
                var value = await cache.Get<TestObject>(i.ToString());
            }
            sw.Stop();
            Console.WriteLine("Elapsed on get:" + sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public async Task ConstructUserTypeCacheOf16() { await PrepareCache(16); }
        [TestMethod]
        public async Task ConstructUserTypeCacheOf32() { await PrepareCache(32); }
        [TestMethod]
        public async Task ConstructUserTypeCacheOf64() { await PrepareCache(64); }
        [TestMethod]
        public async Task ConstructUserTypeCacheOf128() { await PrepareCache(128); }
        [TestMethod]
        public async Task ConstructUserTypeCacheOf256() { await PrepareCache(256); }

        public static async Task PrepareCache(int N)
        {
            await Write100UserTypesArrays(N);

            var sw = Stopwatch.StartNew();
            var cache = await DesktopCacheFactory.GetCache(null, PerfCacheName);
            cache.Size.Should().BeGreaterThan(0);
            sw.Stop();
            Console.WriteLine("Elapsed on PrepareCache:" + sw.ElapsedMilliseconds);
        }
    }
}
