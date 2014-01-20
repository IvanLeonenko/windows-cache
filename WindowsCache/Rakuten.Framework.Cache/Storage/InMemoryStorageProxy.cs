using System.IO;
using System.Threading.Tasks;

namespace Rakuten.Framework.Cache.Storage
{
    public class InMemoryStorageProxy : IStorage
    {
        private readonly IStorage _storage;
        private readonly bool _inMemory;

        public InMemoryStorageProxy(IStorage storage, bool inMemory)
        {
            _storage = storage;
            _inMemory = inMemory;
        }

        public async Task<Stream> GetStream(string key)
        {
            return _inMemory ? null : await _storage.GetStream(key);
        }

        public async Task<string> GetString(string key)
        {
            return _inMemory ? null : await _storage.GetString(key);
        }

        public async Task<byte[]> GetBytes(string key)
        {
            return _inMemory ? null : await _storage.GetBytes(key);
        }

        public async Task Write(string key, Stream value)
        {
            if(!_inMemory)
                await _storage.Write(key, value);
        }

        public async Task Write(string key, string value)
        {
            if (!_inMemory)
                await _storage.Write(key, value);
        }

        public async Task Write(string key, byte[] value)
        {
            if (!_inMemory)
                await _storage.Write(key, value);
        }

        public async Task Remove(string key)
        {
            if (!_inMemory)
                await _storage.Remove(key);
        }
    }
}
