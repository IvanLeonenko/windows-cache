using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache.Storage;

namespace CacheTests.StorageTests
{
    [TestClass]
    public class When_storage_has_in_memory_only_flag_set
    {
        private TestStorage _storage;
        private InMemoryStorageProxy _storageProxy;

        [TestInitialize]
        public async void Initializtion()
        {
            _storage = new TestStorage();

            await _storage.Write("string", "string");
            await _storage.Write("byte[]", new byte[]{12,23,34});
            await _storage.Write("stream", new MemoryStream(new byte[] { 56,67,78 }));

            _storageProxy = new InMemoryStorageProxy(_storage, inMemory:true);

        }

        [TestMethod]
        public void it_should_return_nulls_for_all_get_methhods()
        {
            _storageProxy.GetString("string").Result.Should().BeNull();
            _storageProxy.GetBytes("byte[]").Result.Should().BeNull();
            _storageProxy.GetStream("stream").Result.Should().BeNull();
        }

        [TestMethod]
        public void its_set_methods_should_not_alter_storage()
        {
            _storageProxy.Write("string", "string");
            _storageProxy.Write("byte[]", new byte[] { 12, 23, 34 });
            _storageProxy.Write("stream", new MemoryStream(new byte[] { 56, 67, 78 }));

            _storageProxy.GetString("string").Result.Should().BeNull();
            _storageProxy.GetBytes("byte[]").Result.Should().BeNull();
            _storageProxy.GetStream("stream").Result.Should().BeNull();
        }
    }
}
