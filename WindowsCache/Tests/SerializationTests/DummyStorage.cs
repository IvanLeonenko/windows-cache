using Rakuten.Framework.Cache.Storage;
using System;
using System.IO;

namespace SerializationTests
{
    class DummyStorage : IStorage
    {
        public Stream ReadStream(string key)
        {
            throw new NotImplementedException();
        }

        public void WriteStream(string key, Stream value)
        {
            throw new NotImplementedException();
        }

        public string Read(string key)
        {
            return null;
        }

        public void Write(string key, string value)
        {
            
        }
    }
}
