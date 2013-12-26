using System;
using System.IO;

namespace Rakuten.Framework.Cache.Storage
{
    public interface IStorage
    {
        Stream ReadStream(string key);

        void WriteStream(string key, Stream value);

        String Read(string key);

        void Write(string key, string value);
    }
}
