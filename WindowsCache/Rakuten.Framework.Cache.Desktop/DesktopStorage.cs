using System;
using System.Collections.Generic;
using System.IO;
using Rakuten.Framework.Cache.Storage;

namespace Rakuten.Framework.Cache.Desktop
{
    public class DesktopStorage : IStorage
    {
        private static string _storageLocation;
        private const string Product = "Rakuten";
        private const string CacheFolder = "cache";
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
            _storageLocation = String.Format(@"{0}\{1}\{2}\{3}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Product, CacheFolder, _cacheName);
            if (!Directory.Exists(_storageLocation))
            {
                Directory.CreateDirectory(_storageLocation);
            }
        }

        public Stream ReadStream(string key)
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

        public void WriteStream(string key, Stream value)
        {
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

        public string Read(string key)
        {
            var filePath = GetFilePath(key);
            lock (GetLocker(filePath))
            {
                return File.Exists(filePath) ? File.ReadAllText(filePath) : null;
            }
        }

        public void Write(string key, string value)
        {
            var filePath = GetFilePath(key);
            lock (GetLocker(filePath))
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                File.WriteAllText(filePath, value);
            }
        }

        public void Remove(string key)
        {
            var filePath = GetFilePath(key);
            lock (GetLocker(filePath))
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
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
            }
            return _keyToLockers[key];
        }
    }
}
