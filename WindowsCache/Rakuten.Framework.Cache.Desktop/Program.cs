using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Rakuten.Framework.Cache;
using Rakuten.Framework.Cache.Storage;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Rakuten.Framework.Cache.Desktop
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
    public class SomeData2 :IData
    {
        public Dictionary<string, int> Entries { get; set; }

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
            ProtobufHelper.RegisterType(typeof(CacheEntry2<string>));
            
            var cacheContainer = new CacheContainer();
            cacheContainer.Register<IStorage, DesktopStorage>().WithValue("cacheName", "def1");
            var cache = new Cache(cacheContainer);

            var sdf = cache.Get<CacheEntry2<string>>("SomeProto");
            Console.WriteLine(sdf.Val);
            Console.ReadKey();
            return;

            //cache.Set("SomeInt", 55);
            //cache.Set("SomeString", "_string_");
            //cache.Set("SomeDateTime", DateTime.Now);
            //cache.Set("SomeDouble", 234.345D);
            //var someData = new SomeData {Entries = new Dictionary<string, int>{{"q",45}, {"w",34}}, StringProperty = "Some strin of proto"};
            
            //var someData = new SomeData2 { Entries = new Dictionary<string, int> { { "q", 45 }, { "w", 34 } }, StringProperty = "Some strin of proto" };
            //cache.Set("SomeProto", someData);

            var ce = new CacheEntry2<string> {Val = "qwerty"};
            cache.Set("SomeProto", ce);

            Console.WriteLine("Done. Press any key");
            Console.ReadKey();
            return;

            var ce1 = new CacheEntry<string>();
            ce1.Value = "q";

            
            var ce2 = new CacheEntry<int>();
            ce2.Value = 5;
            RuntimeTypeModel.Default[typeof(ICacheEntry)].AddSubType(10, typeof(CacheEntry<string>));
            RuntimeTypeModel.Default[typeof(ICacheEntry)].AddSubType(11, typeof(CacheEntry<int>));
            
            
            //Cache cache = null;
            //using (var file = File.OpenRead(@"c:\Users\excadmin\Documents\Visual Studio 2013\Projects\CacheTest\CacheTest\bin\Debug\out.txt"))
            //    cache = Serializer.Deserialize<Cache>(file);

            //return;


            //var c = new Cache();
            //c.Entries = new Dictionary<string, ICacheEntry>();
            //c.Entries.Add("1", ce1);


            //c.Entries.Add("2", ce2);

            //var v = c.Entries["1"];

            //using (var file = File.Create(@"c:\Users\excadmin\Documents\Visual Studio 2013\Projects\CacheTest\CacheTest\bin\Debug\out.txt"))
            //{
            //    Serializer.Serialize(file, c);
            //    file.Flush(true);
            //}
            
            Console.ReadKey();
        }
    }
}
