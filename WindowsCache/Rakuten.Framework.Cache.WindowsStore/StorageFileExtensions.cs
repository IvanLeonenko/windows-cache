using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Rakuten.Framework.Cache.WindowsStore
{
    public static class StorageFileExtensions
    {
        public static async Task<string> ReadString(this IStorageFile file)
        {
            using (var stream = await file.OpenStreamForReadAsync())
            {
                TextReader reader = new StreamReader(stream);
                return await reader.ReadToEndAsync();
            }
        }

        public static async Task Write(this IStorageFile file, string content)
        {
            using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var outputStream = fileStream.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new DataWriter(outputStream))
                    {
                        dataWriter.WriteString(content);
                        await dataWriter.StoreAsync();
                        dataWriter.DetachStream();
                    }

                    await outputStream.FlushAsync();
                }
            }
        }

        public static async Task<byte[]> ReadBytes(this IStorageFile file)
        {
            using (var stream = await file.OpenStreamForReadAsync())
            {
                var bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, (int)stream.Length);
                return bytes;
            }
        }

        public static async Task Write(this IStorageFile file, byte[] content)
        {
            using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var outputStream = fileStream.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new DataWriter(outputStream))
                    {
                        dataWriter.WriteBytes(content);
                        await dataWriter.StoreAsync();
                        dataWriter.DetachStream();
                    }

                    await outputStream.FlushAsync();
                }
            }
        }

        public static async Task<Stream> ReadStream(this IStorageFile file)
        {
            using (var stream = await file.OpenStreamForReadAsync())
            {
                var bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, (int)stream.Length);
                var memoryStream = new MemoryStream();
                await memoryStream.WriteAsync(bytes, 0, bytes.Length);
                return memoryStream;
            }
        }

        public static async Task Write(this IStorageFile file, Stream content)
        {
            using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var outputStream = fileStream.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new DataWriter(outputStream))
                    {
                        var bytes = new byte[content.Length];
                        await content.ReadAsync(bytes, 0, (int)content.Length);

                        dataWriter.WriteBytes(bytes);
                        await dataWriter.StoreAsync();
                        dataWriter.DetachStream();
                    }

                    await outputStream.FlushAsync();
                }
            }
        }
    }
}
