using System.Collections.Generic;
using Rakuten.Framework.Cache.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Rakuten.Framework.Cache.Threading;

namespace Rakuten.Framework.Cache.WindowsStore
{
    public class IsolatedStorage : IStorage
    {
        private readonly string _cacheName;
        private StorageFolder _storageFolder;
        public IsolatedStorage(string cacheName)
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
            using (var @lock = await GetLocker(key).LockAsync())
            {
                var cacheFolder = await GetWorkingFolder();
                var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
                await file.Write(value);
            }
        }

        public async Task<string> GetString(string key)
        {
            using (var @lock = await GetLocker(key).LockAsync())
            {
                var storageFile = await GetStorageFile(key);
                return storageFile != null ? await storageFile.ReadString() : null;
            }
        }

        public async Task Write(string key, byte[] value)
        {
            using (var @lock = await GetLocker(key).LockAsync())
            {
                var cacheFolder = await GetWorkingFolder();
                var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
                await file.Write(value);
            }
        }

        public async Task<byte[]> GetBytes(string key)
        {
            using (var @lock = await GetLocker(key).LockAsync())
            {
                var storageFile = await GetStorageFile(key);
                return storageFile != null ? await storageFile.ReadBytes() : null;
            }
        }

        public async Task Write(string key, Stream value)
        {
            using (var @lock = await GetLocker(key).LockAsync())
            {
                var cacheFolder = await GetWorkingFolder();
                var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
                await file.Write(value);
            }
        }

        public async Task<Stream> GetStream(string key)
        {
            using (var @lock = await GetLocker(key).LockAsync())
            {
                var storageFile = await GetStorageFile(key);
                return storageFile != null ? await storageFile.ReadStream() : null;
            }
        }

        public async Task Remove(string key)
        {
            using (var @lock = await GetLocker(key).LockAsync())
            {
                var storageFile = await GetStorageFile(key);
                if (storageFile != null)
                    await storageFile.DeleteAsync();
            }
            RemoveLocker(key);
        }


        private readonly object _locker = new object();
        private readonly Dictionary<string, AsyncLock> _keyToLockers = new Dictionary<string, AsyncLock>();

        private AsyncLock GetLocker(string key)
        {
            if (_keyToLockers.ContainsKey(key))
                return _keyToLockers[key];

            lock (_locker)
            {
                if (!_keyToLockers.ContainsKey(key))
                {
                    _keyToLockers[key] = new AsyncLock();
                }
                return _keyToLockers[key];
            }
        }

        private void RemoveLocker(string key)
        {
            lock (_locker)
            {
                if (_keyToLockers.ContainsKey(key))
                    _keyToLockers.Remove(key);
            }
        }
    }
}
