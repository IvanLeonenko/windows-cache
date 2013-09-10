/*
 * Cache module
 *
 * ICacheManager - main interface for cache module
 *
 * @author Per Johan Groland (perjohan.groland@mail.rakuten.com)
 *
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakuten.Framework.Cache
{
    /// <summary>
    /// The main interface class for the cache
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Requests a url
        /// </summary>
        /// <param name="url">The url to request</param>
        /// <returns>A task containing the resulting data stream.</returns>
        Task<Stream> RequestItemAsync(string url);

        /// <summary>
        /// Flush the entire cache
        /// </summary>
        /// <returns>Returns a task.</returns>
        Task FlushCacheAsync();

        /// <summary>
        /// Cleans bytesToClean bytes of cached files
        /// </summary>
        /// <param name="bytesToClean">number of bytes to clean</param>
        /// <returns>Returns a task.</returns>
        Task CleanCacheAsync(long bytesToClean);
    }
}
