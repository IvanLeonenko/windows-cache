using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Rakuten.Framework.Cache.Desktop.Test
{
    [ProtoContract]
    public class SomeData
    {
        [ProtoMember(1)]
        public Dictionary<string, int> Entries { get; set; }

        [ProtoMember(2)]
        public String StringProperty { get; set; }

    }

    [ProtoContract]
    public interface IData
    {
        
    }
    //[ProtoContract]
    public class SomeData2 //:IData
    {
        public Dictionary<string, int> Entries { get; set; }
        public string o { get; set; }
       // public Int32 f { get; set; }
        public String StringProperty { get; set; }
    }

    public class SomeData3 //:IData
    {
        public Dictionary<string, int> Entries { get; set; }
        public string o { get; set; }
        // public Int32 f { get; set; }
        public String StringProperty { get; set; }
    }
    public class SomeData4 //:IData
    {
        public Dictionary<string, int> Entries { get; set; }
        public string o { get; set; }
        // public Int32 f { get; set; }
        public String StringProperty { get; set; }
    }

    //[ProtoContract]
    //[ProtoInclude(10, typeof(CacheEntry2<string>))]
    //[ProtoInclude(11, typeof(CacheEntry<int>))]
    public interface ICacheEntry2{}

    //[ProtoContract]
    public class CacheEntry2<T> : ICacheEntry2
    {
        //[ProtoMember(1)]
        public T Val { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var types = new List<Type> { typeof(SomeData4), typeof(SomeData2), typeof(SomeData3) };
            CacheFactory factory = new DesktopCacheFactory();
            var cache = factory.Cache(types);

            var someData = new SomeData2 { Entries = new Dictionary<string, int> { { "q", 45 }, { "w", 34 } }, StringProperty = "Some string of proto"};
            cache.Set("SomeProto", someData);

            var someData3 = new SomeData3 { Entries = new Dictionary<string, int> { { "q", 33 }, { "w", 33 } }, StringProperty = "Some string of proto3" };
            cache.Set("SomeProto3", someData3);
            var someData4 = new SomeData4 { Entries = new Dictionary<string, int> { { "q", 44 }, { "w", 44 } }, StringProperty = "Some string of proto4" };
            cache.Set("SomeProto4", someData4);

            var sd2 = cache.Get<SomeData2>("SomeProto");
            
            Console.WriteLine("Done. Press any key");
            
            Console.ReadKey();
        }
    }
}
