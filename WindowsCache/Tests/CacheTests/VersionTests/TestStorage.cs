using Rakuten.Framework.Cache.Storage;
using System;
using System.Collections.Generic;
using System.IO;

namespace CacheTests.VersionTests
{
    class TestStorage : IStorage
    {
        public static Dictionary<string, Stream> KeyToStreams = new Dictionary<string, Stream>();
        public static Dictionary<string, String> KeyToStrings = new Dictionary<string, string>();

        public Stream ReadStream(string key)
        {
            return KeyToStreams.ContainsKey(key) ? KeyToStreams[key] : null;
        }

        public void WriteStream(string key, Stream value)
        {
            KeyToStreams[key] = value;
        }

        public string Read(string key)
        {
            return KeyToStrings.ContainsKey(key) ? KeyToStrings[key] : null;
        }

        public void Write(string key, string value)
        {
            KeyToStrings[key] = value;
        }

        public void Remove(string key)
        {
            if (KeyToStreams.ContainsKey(key))
                KeyToStreams.Remove(key);
            if (KeyToStrings.ContainsKey(key))
                KeyToStrings.Remove(key);
        }
    }
}
