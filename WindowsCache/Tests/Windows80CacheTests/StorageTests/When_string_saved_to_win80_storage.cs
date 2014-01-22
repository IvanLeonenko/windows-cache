using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Rakuten.Framework.Cache.WindowsStore;

namespace Windows80CacheTests.StorageTests
{
    [TestClass]
    public class When_string_saved_to_win80_storage
    {
        private StoreStorage _storeStorage;

        [TestInitialize]
        public async Task Initialize()
        {
            _storeStorage = new StoreStorage("default");
            await _storeStorage.Write("key1", "stringValue");
        }

        [TestMethod]
        public async Task it_should_be_available()
        {
            var value = await _storeStorage.GetString("key1");
            value.Should().Be("stringValue");
        }

        [TestMethod]
        public async Task it_should_not_be_available_after_remove()
        {
            await _storeStorage.Remove("key1");
            _storeStorage.GetString("key1").Result.Should().BeNull();
        }
    }
}
