using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache.Storage;

namespace CacheTests.StorageTests
{
    [TestClass]
    public class When_storage_has_not_in_memory_only_flag_set
    {
        private TestStorage _storage;
        private InMemoryStorageProxy _storageProxy;
        private readonly Stream _stream = new MemoryStream(new byte[] { 56, 67, 78 });

        [TestInitialize]
        public void Initializtion()
        {
            _storage = new TestStorage();

            _storage.Write("string", "string");
            _storage.Write("byte[]", new byte[] { 12, 23, 34 });
            _storage.Write("stream", _stream);

            _storageProxy = new InMemoryStorageProxy(_storage, inMemory: false);
        }

        [TestMethod]
        public void it_should_return_values_for_all_get_methods()
        {
            _storageProxy.GetString("string").Result.Should().Be("string");
            _storageProxy.GetBytes("byte[]").Result.Should().BeEquivalentTo(new byte[] { 12, 23, 34 });
            _storageProxy.GetStream("stream").Result.Should().NotBeNull();
        }

        [TestMethod]
        public void its_set_methods_should_alter_storage()
        {
            _storageProxy.Write("string", "string");
            _storageProxy.Write("byte[]", new byte[] { 12, 23, 34 });
            _storageProxy.Write("stream", new MemoryStream(new byte[] { 56, 67, 78 }));

            _storageProxy.GetString("string").Result.Should().Be("string");
            _storageProxy.GetBytes("byte[]").Result.Should().BeEquivalentTo(new byte[] { 12, 23, 34 });
            _storageProxy.GetStream("stream").Result.Should().NotBeNull();
        }
    }
}
