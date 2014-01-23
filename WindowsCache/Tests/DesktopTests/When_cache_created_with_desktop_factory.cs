using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DesktopTests.UserTypes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache.Desktop;

namespace DesktopTests
{
    [TestClass]
    public class When_cache_created_with_desktop_factory
    {
        // must be first as long as it registers user types, protobuf model is static, so you cannot add types after you defined the model
        [TestMethod]
        public async Task usesr_types_storing_cache_construction_should_work()
        {
            var types = new List<Type> { typeof(SomeData), typeof(SomeData2) };

            var cache = await DesktopCacheFactory.GetCache(types, "cache3");

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
        public async Task parameterless_construction_should_work()
        {
            var cache = await DesktopCacheFactory.GetCache();

            await cache.Set("key1", "stringValue");
            cache.Size.Should().BeGreaterThan(0);
            cache.Count.Should().Be(1);
            cache.Get<string>("key1").Result.Value.Should().Be("stringValue");
            await cache.Remove("key1");
            cache.Count.Should().Be(0);
            cache.Size.Should().Be(0);
            cache.Get<string>("key1").Result.Should().BeNull();
            await cache.Clear();
        }

        [TestMethod]
        public async Task named_cache_construction_should_work()
        {
            var cache = await DesktopCacheFactory.GetCache(null, "cache2");

            await cache.Set("key1", "stringValue");
            cache.Size.Should().BeGreaterThan(0);
            cache.Count.Should().Be(1);
            cache.Get<string>("key1").Result.Value.Should().Be("stringValue");
            await cache.Remove("key1");
            cache.Count.Should().Be(0);
            cache.Size.Should().Be(0);
            cache.Get<string>("key1").Result.Should().BeNull();
            await cache.Clear();
        }
    }
}
