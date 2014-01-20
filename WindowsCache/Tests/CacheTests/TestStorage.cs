using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Rakuten.Framework.Cache.Storage;

namespace CacheTests
{
    class TestStorage : IStorage
    {
        public Dictionary<string, Stream> KeyToStreams = new Dictionary<string, Stream>();
        public Dictionary<string, String> KeyToStrings = new Dictionary<string, string>();
        public Dictionary<string, byte[]> KeyToBytes = new Dictionary<string, byte[]>();

        public async Task<Stream> GetStream(string key)
        {
            return KeyToStreams.ContainsKey(key) ? KeyToStreams[key] : null;
        }

        public async Task<byte[]> GetBytes(string key)
        {
            return KeyToBytes[key];
        }

        public async Task Write(string key, Stream value)
        {
            value.Position = 0;
            var newStream = new MemoryStream();
            value.CopyTo(newStream);
            KeyToStreams[key] = newStream;
        }

        public async Task<string> GetString(string key)
        {
            return KeyToStrings.ContainsKey(key) ? KeyToStrings[key] : null;
        }

        public async Task Write(string key, string value)
        {
            KeyToStrings[key] = value;
        }

        public async Task Write(string key, byte[] value)
        {
            KeyToBytes[key] = value;
        }

        public async Task Remove(string key)
        {
            if (KeyToStreams.ContainsKey(key))
                KeyToStreams.Remove(key);
            if (KeyToStrings.ContainsKey(key))
                KeyToStrings.Remove(key);
            if (KeyToBytes.ContainsKey(key))
                KeyToBytes.Remove(key);
        }
    }
}
