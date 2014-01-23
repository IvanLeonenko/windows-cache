using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Windows.Storage;
using Rakuten.Framework.Cache.Storage;

namespace Rakuten.Framework.Cache.WindowsPhone
{
    public class PhoneStorage : IStorage
    {
        public Task<Stream> GetStream(string key)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetString(string key)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetBytes(string key)
        {
            throw new NotImplementedException();
        }

        public Task Write(string key, Stream value)
        {
            throw new NotImplementedException();
        }

        public Task Write(string key, string value)
        {
            throw new NotImplementedException();
        }

        public Task Write(string key, byte[] value)
        {
            throw new NotImplementedException();
        }

        public Task Remove(string key)
        {
            throw new NotImplementedException();
        }
    }
}
