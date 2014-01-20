using Rakuten.Framework.Cache.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

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

        public async Task Write(string key, string value)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
            await file.Write(value);
        }

        public async Task<string> GetString(string key)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.OpenIfExists);
            return await file.ReadString();
        }

        public async Task Write(string key, byte[] value)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
            await file.Write(value);
        }

        public async Task<byte[]> GetBytes(string key)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.OpenIfExists);
            return await file.ReadBytes();
        }

        public async Task Write(string key, Stream value)
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

        public async Task Remove(string key)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.GetFileAsync(key);
            await file.DeleteAsync();
        }
    }
}
