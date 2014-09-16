using System.Threading.Tasks;
using Framework.Cache.Storage;
using System.IO;

namespace SerializationTests
{
    class DummyStorage : IStorage
    {
        public async Task<Stream> GetStream(string key)
        {
            return await Task.FromResult((Stream)null); ;
        }

        public async Task<byte[]> GetBytes(string key)
        {
            return await Task.FromResult((byte[])null); 
        }

        public async Task Write(string key, Stream value)
        {
            await Task.FromResult(true);
        }

        public async Task<string> GetString(string key)
        {
            return await Task.FromResult((string)null); 
        }

        public async Task Write(string key, string value)
        {
            await Task.FromResult(true);
        }

        public async Task Write(string key, byte[] value)
        {
            await Task.FromResult(true);
        }

        public async Task Remove(string key)
        {
            await Task.FromResult(true);
        }
    }
}
