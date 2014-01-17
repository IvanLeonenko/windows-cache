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

        public string GetString(string key)
        {
            return _inMemory ? null : _storage.GetString(key);
        }

        public byte[] GetBytes(string key)
        {
            return _inMemory ? null : _storage.GetBytes(key);
        }

        public void Write(string key, Stream value)
        {
            if(!_inMemory)
                _storage.Write(key, value);
        }

        public void Write(string key, string value)
        {
            if (!_inMemory)
                _storage.Write(key, value);
        }

        public void Write(string key, byte[] value)
        {
            if (!_inMemory)
                _storage.Write(key, value);
        }

        public void Remove(string key)
        {
            if (!_inMemory)
                _storage.Remove(key);
        }
    }
}
