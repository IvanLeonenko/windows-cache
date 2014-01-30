using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
//using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using NUnit.Framework;

using Windows.Storage;
using Rakuten.Framework.Cache.Web;
using Rakuten.Framework.Cache.Web.Windows8;

namespace tests2
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public async void TestMethod1()
        {
            Debug.WriteLine("TestMethod1");

            var db = new SQLiteDbHandler();

            Debug.WriteLine("here");

            StorageFolder localFolder = ApplicationData.Current.LocalFolder;

            Debug.WriteLine("path = " + ApplicationData.Current.LocalFolder.Path);

            var dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path,
                "cache.db");

            Debug.WriteLine("Database path: " + dbPath);

            db.InitWithPath(dbPath, ApplicationData.Current.LocalFolder.Path);

            var cacheManager = new CacheManager(null);
            await cacheManager.RequestItemAsync("http://www.ulv.no/ulv.jpg");

        }
    }
}
