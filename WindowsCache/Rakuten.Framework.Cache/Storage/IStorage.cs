using System;
using System.IO;

namespace Rakuten.Framework.Cache.Storage
{
    public interface IStorage
    {
        Stream ReadStream(string key);
        String ReadString(string key);
        //byte[] GetBytes(string key);
        
        void WriteStream(string key, Stream value);
        void WriteString(string key, string value);
        //void Write(string key, byte[] value);

        void Remove(string key);
    }
}
