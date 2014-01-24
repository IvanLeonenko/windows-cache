using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache.Desktop;

namespace DesktopTests
{
    /// <summary>
    /// Summary description for PerformanceInitNonEmpty
    /// </summary>
    [TestClass]
    public class PerformanceInitNonEmpty
    {
        [TestMethod]
        public async Task ConstructCacheOf16() { await PrepareCache(16); }
        [TestMethod]
        public async Task ConstructCacheOf32() { await PrepareCache(32); }
        [TestMethod]
        public async Task ConstructCacheOf64() { await PrepareCache(64); }
        [TestMethod]
        public async Task ConstructCacheOf128() { await PrepareCache(128); }
        [TestMethod]
        public async Task ConstructCacheOf256() { await PrepareCache(256); }

        public static async Task PrepareCache(int N)
        {
            await PerformanceWrite.Write100KbByteArrays(N);
            
            var sw = Stopwatch.StartNew();
            var cache = await DesktopCacheFactory.GetCache(null, PerformanceWrite.PerfCacheName);
            cache.Size.Should().BeGreaterThan(0);
            sw.Stop();
            Console.WriteLine("Elapsed on PrepareCache:" + sw.ElapsedMilliseconds);
        }

    }
}
