using System.Threading.Tasks;
using Rakuten.Framework.Cache.Storage;
using System.IO;

namespace SerializationTests
{
    class DummyStorage : IStorage
    {
        public async Task<Stream> GetStream(string key)
        {
            return null;
        }

        public byte[] GetBytes(string key)
        {
            return null;
        }

        public void Write(string key, Stream value)
        {
            
        }

        public string GetString(string key)
        {
            return null;
        }

        public void Write(string key, string value)
        {
            
        }

        public void Write(string key, byte[] value)
        {

        }

        public void Remove(string key)
        {
            
        }
    }
}
