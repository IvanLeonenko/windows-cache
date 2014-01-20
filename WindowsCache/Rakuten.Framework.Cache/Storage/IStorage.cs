using System;
using System.IO;
using System.Threading.Tasks;

namespace Rakuten.Framework.Cache.Storage
{
    public interface IStorage
    {
        Task<Stream> GetStream(string key);
        Task<String> GetString(string key);
        Task<byte[]> GetBytes(string key);
        
        Task Write(string key, Stream value);
        Task Write(string key, string value);
        Task Write(string key, byte[] value);

        Task Remove(string key);
    }
}
