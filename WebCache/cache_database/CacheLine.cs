/**
 * Cache module
 *
 * CacheLine - wrapper class for cache lines stored in database
 *
 * @author Per Johan Groland (perjohan.groland@mail.rakuten.com)
 *
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SQLite;

namespace Rakuten.Framework.Cache.Web.Windows8
{
    public class CacheLine : ICacheLine
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string Url { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long   ContentLength { get; set; }
        public string ETag { get; set; }
        public string Expires { get; set; }
        public string LastModified { get; set; }
        public string CacheControl { get; set; }
        public DateTime? LastAccessed { get; set; }
        public DateTime? Timestamp { get; set; }

        public DateTime? DownloadedTimestamp { get; set; }
        public DateTime? ValidUntil { get; protected set; }

        public bool CanCache { get; protected set; }

        //////////////////////////
        public string GetUrl() { return Url; }
        public string GetETag() { return ETag; }
        public string GetLastModified() { return LastModified; }
        public string GetFileName() { return FileName; }
        //////////////////////////


        public CacheLine()
        {
            CanCache = true;
        }

        public void SetData(string url, string contentType, int contentLength, string etag,
             string cacheControl, string expires, string lastModified, string fileName)
        {
            Url = url;
            ContentType = contentType;
            ContentLength = contentLength;
            ETag = etag;
            CacheControl = cacheControl;
            Expires = expires;
            LastModified = lastModified;
            FileName = fileName;

            DownloadedTimestamp = DateTime.UtcNow;
            LastAccessed = DateTime.UtcNow;
        }

        public bool IsValid()
        {
            if (ValidUntil == null) return false;

            if (DateTime.Compare((DateTime)ValidUntil, DateTime.UtcNow) > 0)
                return true;
            return false;
        }

        public bool IsStale()
        {
            if (ValidUntil == null) return false;

            if (DateTime.Compare((DateTime)ValidUntil, DateTime.UtcNow) < 0)
                return true;
            return false;
        }

        public bool HasETag()
        {
            return ETag != null;
        }

        public bool HasLastModified()
        {
            return LastModified != null;
        }

        // Parses the content of a single header line into key,value pairs
        // Format is:
        // Header: key1=value1, key2=value2, standalone-key, ...
        private Dictionary<string, string> ParseHeaderLine(string line)
        {
            var ret = new Dictionary<string, string>();

            string[] values = line.Split(',');

            foreach (string value in values) {
                if (value.Contains("=")) { // key=value
                    string[] temp = value.Split('=');

                    if (temp.Length == 2) // ignore non-compliant content
                        ret.Add(temp[0].Trim(), temp[1].Trim());
                }
                else { // value
                    ret.Add(value, value);
                }
            }

            return ret;
        }

        private void ParseExpires()
        {
            if (Expires != null) {
                // Parse RFC1123 date format
                ValidUntil = DateTime.Parse(Expires);
            }
        }

        private void ParseCacheControl()
        {
            if (CacheControl != null) {
                var m = ParseHeaderLine(CacheControl);

                if (m.ContainsKey("no-cache") || m.ContainsKey("must-revalidate"))
                    CanCache = false;

                if (m.ContainsKey("max-age")) {
                    int maxAge = int.Parse(m["max-age"]);

                    if (maxAge == 0) { // special case that means the same as no-cache
                        CanCache = false;
                    }
                    else if (maxAge > 0) {
                        if (DownloadedTimestamp != null)
                            ValidUntil = ((DateTime)DownloadedTimestamp).AddSeconds(maxAge);
                    }
                }
            }
        }

        public void ParseHeaders()
        {
            ParseExpires();
            ParseCacheControl();
        }
    }
}
