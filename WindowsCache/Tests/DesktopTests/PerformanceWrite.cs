using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache.Desktop;

namespace DesktopTests
{
    [TestClass]
    public class PerformanceWrite
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
        public async Task PerfWrite16() { await Write100KbByteArrays(16); }
        [TestMethod]
        public async Task PerfWrite32() { await Write100KbByteArrays(32); }
        [TestMethod]
        public async Task PerfWrite64() { await Write100KbByteArrays(64); }
        [TestMethod]
        public async Task PerfWrite128() { await Write100KbByteArrays(128); }
        [TestMethod]
        public async Task PerfWrite256() { await Write100KbByteArrays(256); }

        public static async Task Write100KbByteArrays(int numberOfChunks, string cachename = null)
        {
            var byteArrays = PerfHelpers.GetByteArrays(numberOfChunks);
            var cache = await DesktopCacheFactory.GetCache(null, cachename ?? PerfCacheName);
            await cache.Clear();

            var sw = Stopwatch.StartNew();
            for (var i = 0; i < byteArrays.Length; i++)
            {
                cache.Set(i.ToString(), byteArrays[i]).Wait();
            }
            sw.Stop();
            Console.WriteLine("Elapsed:" + sw.ElapsedMilliseconds);
        }

    }
}
