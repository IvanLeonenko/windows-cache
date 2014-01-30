/*
 * Cache module
 *
 * CacheManager - main interface for cache module
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

namespace Rakuten.Framework.Cache.Web
{
    /// <summary>
    /// Main interface for cache module.
    /// </summary>
    public class CacheManager : ICacheManager
    {
        private ICacheDbHandler _db;
        private HttpGetter _getter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dbHandler">The database handler to uses for data persistence.</param>
        public CacheManager(ICacheDbHandler dbHandler)
        {
            _db = dbHandler;
            _getter = new HttpGetter(dbHandler);
        }

        /// <summary>
        /// Requests a url
        /// </summary>
        /// <param name="url">The url to request</param>
        /// <returns>A task containing the resulting data stream.</returns>
        public Task<Stream> RequestItemAsync(string url)
        {
            return _getter.DoGetRequestAsync(url);
        }

        /// <summary>
        /// Flush the entire cache
        /// </summary>
        /// <returns>Returns a task.</returns>
        public Task FlushCacheAsync()
        {
            return _db.FlushCacheAsync();            
        }

        /// <summary>
        /// Cleans bytesToClean bytes of cached files
        /// </summary>
        /// <param name="bytesToClean">number of bytes to clean</param>
        /// <returns>Returns a task.</returns>
        public Task CleanCacheAsync(long bytesToClean)
        {
            return _db.CleanCacheAsync(bytesToClean);
        }
        
    }
}
