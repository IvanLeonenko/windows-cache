using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Rakuten.Framework.Cache.WindowsStore;

namespace Windows80CacheTests.CacheTests
{
    [TestClass]
    public class When_windows_80_store_factory_used_to_create_cache_object
    {
        [TestMethod]
        public async Task default_cache_construction_without_parameters_should_work()
        {
            var cache = WindowsStoreCacheFactory.GetCache();

            await cache.Set("key1", "stringValue");
            cache.Size.Should().BeGreaterThan(0);
            cache.Count.Should().Be(1);
            cache.Get<string>("key1").Result.Value.Should().Be("stringValue");
            await cache.Remove("key1");
            cache.Count.Should().Be(0);
            cache.Size.Should().Be(0);
            cache.Get<string>("key1").Result.Should().BeNull();
        }
    }
}
