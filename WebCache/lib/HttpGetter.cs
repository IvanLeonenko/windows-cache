/*
 * Cache module
 *
 * HttpGetter - helper class for http get requests
 *
 * @author Per Johan Groland (perjohan.groland@mail.rakuten.com)
 *
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Rakuten.Framework.Cache
{
    internal class HttpGetter
    {
        private readonly ICacheDbHandler _db;

        public HttpGetter(ICacheDbHandler dbHandler)
        {
            _db = dbHandler;
        }

        // Wrapper for getting raw header values.
        private string GetHeaderValue(HttpResponseMessage response, string value)
        {
            try {
                if (response.Headers.Contains(value)) {
                    IEnumerable<string> temp = response.Headers.GetValues(value);
                    return temp.First();
                }
            }
            catch (InvalidOperationException) {}

            return null;
        }

        // Wrapper for getting raw header values.
        private string GetContentHeaderValue(HttpResponseMessage response, string value) {
            try {
                if (response.Content.Headers.Contains(value)) {
                    IEnumerable<string> temp = response.Content.Headers.GetValues(value);
                    return temp.First();
                }
            }
            catch (InvalidOperationException) { }

            return null;
        }

        public async Task<Stream> DoGetRequestAsync(string url)
        {
            string fileName;

            // Cache lookup
            var cacheLine = await _db.GetCacheLineAsync(url);

            if (cacheLine != null && cacheLine.IsStale()) {
                Debug.WriteLine("Requested item is stale and will be discarded");
                await _db.RemoveCacheLineAsync(cacheLine);
                cacheLine = null;
            }

            // Check if item is still valid so we can return it directly without asking the server
            if (cacheLine != null && cacheLine.IsValid()) {
                Debug.WriteLine("CACHE HIT with validity");
                return await _db.GetFileAsync(cacheLine);
            }

            var client = new HttpClient();

            // Check for cache hits that must be verified with the server
            if (cacheLine != null)
            {
                if (cacheLine.HasETag())
                    client.DefaultRequestHeaders.Add("If-None-Match", cacheLine.GetETag());

                if (cacheLine.HasLastModified())
                    client.DefaultRequestHeaders.Add("If-Modified-Since", cacheLine.GetLastModified());

                fileName = cacheLine.GetFileName();
            }
            else {
                fileName = _db.GetRandomFilename();
            }

            try {
                var response = await client.GetAsync(url);
                var headers = response.Headers;
                var contentHeaders = response.Content.Headers;

                if (response.StatusCode == HttpStatusCode.OK) {
                    int contentLength;
                    int.TryParse(GetContentHeaderValue(response, "Content-Length"), out contentLength);

                    var newCacheLine = _db.CreateCacheLine();
                    newCacheLine.SetData(url,
                                GetContentHeaderValue(response, "Content-Type"),
                                contentLength,
                                GetHeaderValue(response, "ETag"),
                                GetHeaderValue(response, "Cache-Control"),
                                GetContentHeaderValue(response, "Expires"),
                                GetHeaderValue(response, "Last-Modified"),
                                fileName);
                    newCacheLine.ParseHeaders();

                    // Save in cache for future use
                    await _db.AddCacheLineAsync(newCacheLine);
                    var stream = await response.Content.ReadAsStreamAsync();
                    await _db.SaveFileAsync(stream, newCacheLine.GetFileName());

                    // return newly downloaded file as stream
                    return await _db.GetFileAsync(newCacheLine);
                }
                else if (response.StatusCode == HttpStatusCode.NotModified) {
                    Debug.WriteLine("CACHE HIT from server");
                    // return cached file as stream
                    return await _db.GetFileAsync(cacheLine);
                }
            }
            catch (UnauthorizedAccessException e) {
                Debug.WriteLine(e);
            }

            return null;   
        }
    }
}
