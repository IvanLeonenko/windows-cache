using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using Windows.Storage;
using Framework.Cache.Web;
using Framework.Cache.Web.Windows8;

namespace testapp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string _dbFile;
        private string _dbTempDir;
        private ICacheDbHandler _cacheDbHandler;
        private CacheManager _cacheManager;

        public MainPage()
        {
            this.InitializeComponent();

            _dbFile = "cache.db";
            _dbTempDir = "cache_data";

            _cacheDbHandler = new SQLiteDbHandler();
            _cacheDbHandler.InitWithPath(_dbFile, _dbTempDir);

            _cacheManager = new CacheManager(_cacheDbHandler);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("http://www.ulv.no/ulv.jpg");
            await _cacheManager.RequestItemAsync("http://localhost:3000/ulv.jpg");
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("http://localhost:3000/cachecontrol-5min.jpg");
            await _cacheManager.RequestItemAsync("http://localhost:3000/cachecontrol-5min.jpg");
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("http://localhost:3000/expires-5min.jpg");
            await _cacheManager.RequestItemAsync("http://localhost:3000/expires-5min.jpg");
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Flush cache");
            await _cacheManager.FlushCacheAsync();
        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Clear cache");
            await _cacheManager.CleanCacheAsync(100);
        }
    }
}
