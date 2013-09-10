/**
 * Cache module
 *
 * SQLiteDbHandler - sqlite interface
 *
 * @author Per Johan Groland (perjohan.groland@mail.rakuten.com)
 *
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.FileProperties;
using SQLite;
using Rakuten.Framework.Cache;

namespace Rakuten.Framework.Cache.Windows8
{
    public class SQLiteDbHandler : ICacheDbHandler
    {
        private SQLiteAsyncConnection _db;
        private string _dbFile;
        private string _dbTempDir;
        
        public SQLiteDbHandler() {}

        public async void InitWithPath(string dbFile, string dbTempDir)
        {
            _dbFile = dbFile;
            _dbTempDir = dbTempDir;

            try {
                await ApplicationData.Current.LocalFolder.CreateFolderAsync(_dbTempDir);
            }
            catch (Exception) {} // exception thrown when folder already exists

            _db = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, _dbFile));

            await _db.CreateTableAsync<CacheLine>();
        }

        public async Task FlushCacheAsync()
        {
            await _db.ExecuteAsync("delete from CacheLine");

            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(_dbTempDir);
            var files = await folder.GetFilesAsync();

            foreach (var f in files) {
                try {
                    await f.DeleteAsync();
                }
                catch (FileNotFoundException) {}
            }
        }

        public async Task<long> CleanCacheAsync(long bytesToClean)
        {
            long bytesCleaned = 0;
            IEnumerable<CacheLine> cacheLines = await _db.QueryAsync<CacheLine>("select * from CacheLine order by LastAccessed desc");
            
            foreach (var cl in cacheLines) {
                long contentLength = cl.ContentLength;

                await RemoveCacheLineAsync(cl);

                bytesCleaned += contentLength;
                bytesToClean -= contentLength;

                if (bytesToClean < 0) break;
            }

            return bytesCleaned;
        }

        public async Task AddCacheLineAsync(ICacheLine cl)
        {
            ICacheLine exists = await GetCacheLineAsync(cl.GetUrl());

            if (exists == null) {
                await _db.InsertAsync(cl);
            }
        }

        public async Task RemoveCacheLineAsync(ICacheLine cl)
        {
            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(_dbTempDir);

            try {
                var fn = cl.GetFileName();
                await _db.DeleteAsync(cl);
                var file = await folder.GetFileAsync(cl.GetFileName());
                await file.DeleteAsync();
            }
            catch (FileNotFoundException) {}
        }

        public async Task<ICacheLine> GetCacheLineAsync(string url)
        {
            IEnumerable<CacheLine> cacheLines = await _db.QueryAsync<CacheLine>("select * from CacheLine where Url = ?", url);

            if (cacheLines.Any()) {
                CacheLine cl = cacheLines.First();
                cl.LastAccessed = DateTime.Now;
                await _db.UpdateAsync(cl);
                cl.ParseHeaders();
                return cl;
            }
            else {
                return null;
            }
        }

        public async Task<ulong> GetTotalSizeAsync()
        {
            // StorageFolder should ideally be passed through ICacheDbHandler as a parameter,
            // but Windows.Storage is not available in a portable class library, so as a workaround
            // it is accessed directly here
            StorageFolder folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(_dbTempDir);
            IReadOnlyList<StorageFile> fileList = await folder.GetFilesAsync(); 

            ulong filesize = 0;

            foreach (var f in fileList) {
                BasicProperties bp = await f.GetBasicPropertiesAsync();
                filesize += bp.Size;
            }

            return filesize;
        }

        public string GetRandomFilename()
        {
            return System.Guid.NewGuid() + ".dat";
        }

        public async Task SaveFileAsync(Stream s, string filename)
        {
            try {
                s.Position = 0;
                StorageFolder folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(_dbTempDir);
                StorageFile file = await folder.CreateFileAsync(filename);

                using (Stream destStream = await file.OpenStreamForWriteAsync()) {
                    await s.CopyToAsync(destStream);
                }
            }
            catch (FileNotFoundException e) {
                Debug.WriteLine(e);
            }
            catch (System.ArgumentException e) {
                Debug.WriteLine(e);
            }

            Debug.WriteLine("SaveFileAsync OUT");
        }

        public async Task<Stream> GetFileAsync(ICacheLine cl)
        {
            try {
                StorageFolder folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(_dbTempDir);
                StorageFile file = await folder.GetFileAsync(cl.GetFileName());
                return await file.OpenStreamForReadAsync();
            }
            catch (FileNotFoundException e) {
                Debug.WriteLine(e);
            }
            catch (System.ArgumentException e) {
                Debug.WriteLine(e);
            }

            return null;
        }

        public ICacheLine CreateCacheLine()
        {
            return new CacheLine();
        }
    }
}
