using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Rakuten.Framework.Cache.Storage;

namespace Rakuten.Framework.Cache.WindowsStore
{
    public class StoreStorage : IStorage
    {
        private readonly string _cacheName;
        private StorageFolder _storageFolder;
        public StoreStorage(string cacheName)
        {
            _cacheName = cacheName;
        }

        private async Task<StorageFolder> GetWorkingFolder()
        {
            return _storageFolder ?? (_storageFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(string.Format(@"Rakuten\cache\{0}", _cacheName), CreationCollisionOption.OpenIfExists));
        }

        public async Task WriteAsync(string key, string value)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
            await file.Write(value);
        }

        public async Task<string> ReadStringAsync(string key)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.OpenIfExists);
            return await file.ReadString();
        }

        public async Task WriteAsync(string key, byte[] value)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
            await file.Write(value);
        }

        public async Task<byte[]> ReadBytesAsync(string key)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.OpenIfExists);
            return await file.ReadBytes();
        }

        public async Task WriteAsync(string key, Stream value)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
            await file.Write(value);
        }

        public async Task<Stream> GetStream(string key)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.OpenIfExists);
            return await file.ReadStream();
        }

        public async Task RemoveAsync(string key)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.GetFileAsync(key);
            await file.DeleteAsync();
        }


        public string GetString(string key)
        {
            throw new NotImplementedException();
        }

        public byte[] GetBytes(string key)
        {
            throw new NotImplementedException();
        }

        public void Write(string key, Stream value)
        {
            throw new NotImplementedException();
        }

        public void Write(string key, string value)
        {
            throw new NotImplementedException();
        }

        public void Write(string key, byte[] value)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }
    }
}
