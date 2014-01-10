using System;
using System.IO;

namespace Rakuten.Framework.Cache.Storage
{
    public interface IStorage
    {
        Stream GetStream(string key);
        String GetString(string key);
        byte[] GetBytes(string key);
        
        void Write(string key, Stream value);
        void Write(string key, string value);
        void Write(string key, byte[] value);

        void Remove(string key);
    }
}
