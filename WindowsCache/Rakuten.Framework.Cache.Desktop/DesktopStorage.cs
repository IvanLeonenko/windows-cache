using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Rakuten.Framework.Cache.Storage;

namespace Rakuten.Framework.Cache.Desktop
{
    public class DesktopStorage : IStorage
    {
        private static string _storageLocation;
        private readonly string _cacheName = "default";
        private readonly object _locker = new object();
        private readonly Dictionary<string, object> _keyToLockers = new Dictionary<string, object>();

        public DesktopStorage(string cacheName)
        {
            if (!String.IsNullOrEmpty(cacheName))
                _cacheName = cacheName;
            Init();
        }

        public DesktopStorage()
        {
            Init();
        }

        private void Init()
        {
            _storageLocation = String.Format(CultureInfo.InvariantCulture, @"{0}\Rakuten\cache\{1}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _cacheName);
            if (!Directory.Exists(_storageLocation))
            {
                Directory.CreateDirectory(_storageLocation);
            }
        }

        public Stream GetStreamSync(string key)
        {
            var filePath = GetFilePath(key);
            lock (GetLocker(filePath))
            {
                if(!File.Exists(filePath))
                    return null;
                
                var stream = new MemoryStream();
                using (var fileStream = File.OpenRead(filePath))
                {
                    fileStream.CopyTo(stream);
                }
                stream.Position = 0;
                return stream;
            }
        }

        public async Task<Stream> GetStream(string key)
        {
            return await Task.Run(() => GetStreamSync(key));
        }

        public async Task<byte[]> GetBytes(string key)
        {
            return await Task.Run(() => GetBytesSync(key));
        }
        public byte[] GetBytesSync(string key)
        {
            var filePath = GetFilePath(key);
            lock (GetLocker(filePath))
            {
                if (!File.Exists(filePath))
                    return null;

                using (var memoryStream = new MemoryStream())
                {
                    using (var fileStream = File.OpenRead(filePath))
                    {
                        fileStream.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        public async Task Write(string key, Stream value)
        {
            await Task.Run(() => WriteSync(key, value));
        }
        public void WriteSync(string key, Stream value)
        {
            if (value == null)
                return;
            var filePath = GetFilePath(key);
            lock (GetLocker(filePath))
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                //using (value)
                //{
                    value.Position = 0;
                    using (var file = File.Create(filePath))
                    {
                        value.CopyTo(file);
                    }
                //}
            }
        }

        public async Task<String> GetString(string key)
        {
            return await Task.Run(() => GetStringSync(key));
        }
        public string GetStringSync(string key)
        {
            var filePath = GetFilePath(key);
            lock (GetLocker(filePath))
            {
                return File.Exists(filePath) ? File.ReadAllText(filePath) : null;
            }
        }

        public async Task Write(string key, string value)
        {
            await Task.Run(() => WriteSync(key, value));
        }
        public void WriteSync(string key, string value)
        {
            var filePath = GetFilePath(key);
            lock (GetLocker(filePath))
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                File.WriteAllText(filePath, value);
            }
        }

        public async Task Write(string key, byte[] value)
        {
            await Task.Run(() => WriteSync(key, value));
        }
        public void WriteSync(string key, byte[] value)
        {
            if (value == null)
                return;
            var filePath = GetFilePath(key);
            lock (GetLocker(filePath))
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                using (var file = File.Create(filePath))
                {
                    file.Write(value, 0, value.Length);
                }
            }
        }

        public async Task Remove(string key)
        {
            await Task.Run(() => RemoveSync(key));
        }
        public void RemoveSync(string key)
        {
            var filePath = GetFilePath(key);
            lock (GetLocker(filePath))
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            RemoveLocker(key);
        }

        private static string GetFilePath(string key)
        {
            return Path.Combine(_storageLocation, key);
        }

        private object GetLocker(string key)
        {
            if (_keyToLockers.ContainsKey(key)) 
                return _keyToLockers[key];

            lock (_locker)
            {
                if (!_keyToLockers.ContainsKey(key))
                {
                    _keyToLockers[key] = new object();
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
