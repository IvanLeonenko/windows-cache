using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;
using System;
using System.IO;

namespace CacheTests.CacheContainerTests
{
    public class TestSerializer : ISerializer
    {
        private readonly IStorage _storage;
        public TestSerializer(IStorage storage)
        {
            _storage = storage;
        }

        public void RegisterType(Type type)
        {
            throw new NotImplementedException();
        }

        public void RegisterSubType(Type type, Type subType)
        {
            throw new NotImplementedException();
        }

        public bool CanSerialize(Type type)
        {
            return _storage != null;
        }

        public T Deserialize<T>(Stream stream)
        {
            throw new NotImplementedException();
        }

        public Stream Serialize<T>(T value)
        {
            throw new NotImplementedException();
        }
    }
}
