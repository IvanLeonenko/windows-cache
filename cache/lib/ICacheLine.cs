/*
 * Cache module
 *
 * ICacheLine - wrapper class for cache lines stored in database
 *
 * @author Per Johan Groland (perjohan.groland@mail.rakuten.com)
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakuten.Framework.Cache
{
    /// <summary>
    /// Interface for cache lines stored in the database.
    /// </summary>
    public interface ICacheLine
    {
        /// <summary>
        /// Sets the data to be stored in the cache database
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="contentType">contentType</param>
        /// <param name="contentLength">contentLength</param>
        /// <param name="etag">etag</param>
        /// <param name="cacheControl">cacheControl</param>
        /// <param name="expires">expires</param>
        /// <param name="lastModified">lastModified</param>
        /// <param name="fileName">fileName</param>
        void SetData(string url, string contentType, int contentLength, string etag,
                     string cacheControl, string expires, string lastModified, string fileName);

        /// <summary>
        /// Is the item stale.
        /// </summary>
        /// <returns>Is the item stale.</returns>
        bool IsStale();

        /// <summary>
        /// Is the item valid.
        /// </summary>
        /// <returns>Is the item valid.</returns>
        bool IsValid();

        /// <summary>
        /// Does the item have an etag.
        /// </summary>
        /// <returns> Does the item have an etag.</returns>
        bool HasETag();

        /// <summary>
        /// Does the item have a last modified header.
        /// </summary>
        /// <returns>Does the item have a last modified header.</returns>
        bool HasLastModified();

        /// <summary>
        /// Parse all http headers
        /// </summary>
        /// <returns>Parse all http headers.</returns>
        void ParseHeaders();

        // properties
        /// <summary>
        /// Get the url.
        /// </summary>
        /// <returns>Get the url.</returns>
        string GetUrl();

        /// <summary>
        /// Get the ETag
        /// </summary>
        /// <returns>Get the ETag</returns>
        string GetETag();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetLastModified();

        /// <summary>
        /// Get the file name.
        /// </summary>
        /// <returns>Get the file name.</returns>
        string GetFileName();
    }
}
