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

        private async Task<StorageFile> GetStorageFile(string key)
        {
            var cacheFolder = await GetWorkingFolder();
            try
            {
                return await cacheFolder.GetFileAsync(key);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public async Task Write(string key, string value)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
            await file.Write(value);
        }

        public async Task<string> GetString(string key)
        {
            var storageFile = await GetStorageFile(key);
            return storageFile != null ? await storageFile.ReadString() : null;
        }

        public async Task Write(string key, byte[] value)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
            await file.Write(value);
        }

        public async Task<byte[]> GetBytes(string key)
        {
            var storageFile = await GetStorageFile(key);
            return storageFile != null ? await storageFile.ReadBytes() : null;
        }

        public async Task Write(string key, Stream value)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
            await file.Write(value);
        }

        public async Task<Stream> GetStream(string key)
        {
            var storageFile = await GetStorageFile(key);
            return storageFile != null ? await storageFile.ReadStream() : null;
        }

        public async Task Remove(string key)
        {
            var storageFile = await GetStorageFile(key);
            if (storageFile != null)
                await storageFile.DeleteAsync();
        }
    }
}
