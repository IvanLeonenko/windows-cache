/*
 * Cache module
 *
 * ICacheDbHandler - database handler interface
 *
 * @author Per Johan Groland (perjohan.groland@mail.com)
 *
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Cache.Web
{
    /// <summary>
    /// Database handler interface.
    /// </summary>
    public interface ICacheDbHandler
    {
        /// <summary>
        /// Initialize the cache module
        /// </summary>
        /// <param name="dbFile">Filename for the sqlite database</param>
        /// <param name="documentPath">Path for where to store the sqlite database and cached files</param>
        void InitWithPath(string dbFile, string documentPath);

        /// <summary>
        /// Flushes all entries in the database.
        /// </summary>
        /// <returns>Returns a task.</returns>
        Task FlushCacheAsync();

        /// <summary>
        /// Cleans the cache by deleting items until bytesToClean bytes have been deleted.
        /// </summary>
        /// <param name="bytesToClean">amount of cached data to delete, in bytes.</param>
        /// <returns>Returns a task with information about how much data was deleted.</returns>
        Task<long> CleanCacheAsync(long bytesToClean);

        /// <summary>
        /// Adds a cache line to the cache database and stores the cached file
        /// </summary>
        /// <param name="cl">The cache line to add </param>
        /// <returns>Returns a task.</returns>
        Task AddCacheLineAsync(ICacheLine cl);

        /// <summary>
        /// Removes a cache line from the cache database and removes the cached file
        /// </summary>
        /// <param name="cl"></param>
        /// <returns>Returns a task.</returns>
        Task RemoveCacheLineAsync(ICacheLine cl);

        /// <summary>
        /// Get a cache line from the database.
        /// </summary>
        /// <param name="url">The url to get the cache line for</param>
        /// <returns>A task containing the cache line if it exists, or null.</returns>
        Task<ICacheLine> GetCacheLineAsync(string url);

        /// <summary>
        /// Gets the current total size of the cached files, in bytes
        /// </summary>
        /// <returns>current size</returns>
        Task<ulong> GetTotalSizeAsync();

        /// <summary>
        /// Gets a random filename used for cache file storage
        /// </summary>
        /// <returns>The random filename</returns>
        string GetRandomFilename();

        /// <summary>
        /// Saves a stream to file
        /// </summary>
        /// <param name="s">The stream to save</param>
        /// <param name="filename">The file to save the stream to</param>
        /// <returns>Returns a task.</returns>
        Task SaveFileAsync(Stream s, string filename);

        /// <summary>
        /// Gets a file from the cache
        /// </summary>
        /// <param name="cl">The cacheline to retrieve the cached file for</param>
        /// <returns>A task containing the data stream.</returns>
        Task<Stream> GetFileAsync(ICacheLine cl);

        /// <summary>
        /// Create a new cache line
        /// </summary>
        /// <returns>A new cache line.</returns>
        ICacheLine CreateCacheLine();
    }
}
