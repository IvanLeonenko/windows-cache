using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

namespace Rakuten.Framework.Cache.WindowsStore.Test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public async Task DoStuff()
        {
            var storeStorage = new StoreStorage("default");
            await storeStorage.WriteAsync("test3", "3");
            var test3 = await storeStorage.ReadStringAsync("test3");

            await storeStorage.WriteAsync("test2", new byte[] { 12, 34, 45 });
            var test2 = await storeStorage.ReadBytesAsync("test2");

            Stream str = new MemoryStream(new byte[] { 67,78,89 });
            await storeStorage.WriteAsync("test1", str);
            var test1 = await storeStorage.ReadStreamAsync("test1");

            await storeStorage.RemoveAsync("test3");
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await DoStuff();
        }
    }
}
