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

        public async Task<byte[]> GetBytes(string key)
        {
            return null;
        }

        public async Task Write(string key, Stream value)
        {
        }

        public async Task<string> GetString(string key)
        {
            return null;
        }

        public async Task Write(string key, string value)
        {
            
        }

        public async Task Write(string key, byte[] value)
        {

        }

        public async Task Remove(string key)
        {
            
        }
    }
}
