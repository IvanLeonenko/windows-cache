using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Framework.Cache.Storage;

namespace Framework.Cache.WindowsStore81
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
            return _storageFolder ?? (_storageFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(string.Format(@"WinCache\cache\{0}", _cacheName), CreationCollisionOption.OpenIfExists));
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

        public async Task<Stream> GetStream(string key)
        {
            var file = await GetStorageFile(key);
            using (var stream = await file.OpenStreamForReadAsync())
            {
                var bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, (int)stream.Length);
                var memoryStream = new MemoryStream();
                await memoryStream.WriteAsync(bytes, 0, bytes.Length);
                return memoryStream;
            }
        }

        public async Task<string> GetString(string key)
        {
            var storageFile = await GetStorageFile(key);
            return storageFile != null ? await FileIO.ReadTextAsync(storageFile) : null;
        }

        public async Task<byte[]> GetBytes(string key)
        {
            var storageFile = await GetStorageFile(key);
            if (storageFile == null)
                return null;
            var buffer = await FileIO.ReadBufferAsync(storageFile);
            return buffer.ToArray();
        }

        public async Task Write(string key, Stream value)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
            using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var outputStream = fileStream.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new DataWriter(outputStream))
                    {
                        var bytes = new byte[value.Length];
                        await value.ReadAsync(bytes, 0, (int)value.Length);

                        dataWriter.WriteBytes(bytes);
                        await dataWriter.StoreAsync();
                        dataWriter.DetachStream();
                    }

                    await outputStream.FlushAsync();
                }
            }
        }

        public async Task Write(string key, string value)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, value);
        }

        public async Task Write(string key, byte[] value)
        {
            var cacheFolder = await GetWorkingFolder();
            var file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(file, value);
        }

        public async Task Remove(string key)
        {
            var storageFile = await GetStorageFile(key);
            if (storageFile != null)
                await storageFile.DeleteAsync();
        }
    }
}
