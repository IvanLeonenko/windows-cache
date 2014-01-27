using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache.Desktop;

namespace DesktopTests
{
    [TestClass]
    public class PerformanceGet
    {
        public const string PerfCacheName = "pcache";

        [TestMethod]
        public async Task PerfCreateEmptyCache()
        {
            var sw = Stopwatch.StartNew();
            var cache = await DesktopCacheFactory.GetCache(null, PerfCacheName);

            sw.Stop();
            Console.WriteLine("Elapsed:" + sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public async Task PerfGet16() { await Get100KbByteArrays(16); }
        [TestMethod]
        public async Task PerfGet32() { await Get100KbByteArrays(32); }
        [TestMethod]
        public async Task PerfGet64() { await Get100KbByteArrays(64); }
        [TestMethod]
        public async Task PerfGet128() { await Get100KbByteArrays(128); }
        [TestMethod]
        public async Task PerfGet256() { await Get100KbByteArrays(256); }

        public static async Task Get100KbByteArrays(int numberOfChunks)
        {
            var byteArrays = PerfHelpers.GetByteArrays(numberOfChunks);
            var cache = await DesktopCacheFactory.GetCache(null, PerfCacheName);
            await cache.Clear();
            var length = byteArrays.Length;
            for (var i = 0; i < length; i++)
            {
                cache.Set(i.ToString(), byteArrays[i]).Wait();
            }
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < length; i++)
            {
                cache.Get<byte[]>(i.ToString()).Wait();
                //bytes.Value.Should().HaveCount(102400);
            }
            sw.Stop();
            Console.WriteLine("Elapsed on get:" + sw.ElapsedMilliseconds);
        }

    }
}
