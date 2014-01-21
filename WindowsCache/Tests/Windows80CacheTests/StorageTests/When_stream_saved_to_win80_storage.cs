using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Rakuten.Framework.Cache.WindowsStore;
using System.IO;
using System.Threading.Tasks;

namespace Windows80CacheTests.StorageTests
{
    [TestClass]
    public class When_stream_saved_to_win80_storage
    {
        private StoreStorage _storeStorage;

        [TestInitialize]
        public async Task Initialize()
        {
            _storeStorage = new StoreStorage("default");
            await _storeStorage.Write("key1", new MemoryStream(new byte[] { 12, 23, 34 }));
        }

        [TestMethod]
        public async Task it_should_be_available()
        {
            _storeStorage.GetStream("key1").Result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task it_should_not_be_available_after_remove()
        {
            await _storeStorage.Remove("key1");
            _storeStorage.GetBytes("key1").Result.Should().BeNull();
        }
    }
}
