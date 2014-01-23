using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache.Desktop;
using System.Threading.Tasks;

namespace DesktopTests
{
    [TestClass]
    public class When_cache_created_with_desktop_factory
    {
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
