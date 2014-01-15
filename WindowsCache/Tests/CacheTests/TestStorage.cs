using System;
using System.Collections.Generic;
using System.IO;
using Rakuten.Framework.Cache.Storage;

namespace CacheTests
{
    class TestStorage : IStorage
    {
        public Dictionary<string, Stream> KeyToStreams = new Dictionary<string, Stream>();
        public Dictionary<string, String> KeyToStrings = new Dictionary<string, string>();
        public Dictionary<string, byte[]> KeyToBytes = new Dictionary<string, byte[]>();

        public Stream GetStream(string key)
        {
            return KeyToStreams.ContainsKey(key) ? KeyToStreams[key] : null;
        }

        public byte[] GetBytes(string key)
        {
            return KeyToBytes[key];
        }

        public void Write(string key, Stream value)
        {
            KeyToStreams[key] = value;
        }

        public string GetString(string key)
        {
            return KeyToStrings.ContainsKey(key) ? KeyToStrings[key] : null;
        }

        public void Write(string key, string value)
        {
            KeyToStrings[key] = value;
        }

        public void Write(string key, byte[] value)
        {
            KeyToBytes[key] = value;
        }

        public void Remove(string key)
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
