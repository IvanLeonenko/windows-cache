using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Rakuten.Framework.Cache
{
    [ProtoContract]
    public interface ICacheEntry{}

    [ProtoContract]
    public class CacheEntry<T> : ICacheEntry
    {
        [ProtoMember(1)]
        public T Value { get; set; }


        public string link { get; set; }

        //public byte[] SerializedValue;

        //public void Set(string key, T value)
        //{
        //    Value = value;
        //}

        //public static byte[] ReadFully(Stream input)
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        input.CopyTo(ms);
        //        return ms.ToArray();
        //    }
        //}
    }
}
