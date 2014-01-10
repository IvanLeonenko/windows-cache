using Rakuten.Framework.Cache.Storage;
using System.IO;

namespace SerializationTests
{
    class DummyStorage : IStorage
    {
        public Stream ReadStream(string key)
        {
            return null;
        }

        public void WriteStream(string key, Stream value)
        {
            
        }

        public string ReadString(string key)
        {
            return null;
        }

        public void WriteString(string key, string value)
        {
            
        }

        public void Remove(string key)
        {
            
        }
    }
}
