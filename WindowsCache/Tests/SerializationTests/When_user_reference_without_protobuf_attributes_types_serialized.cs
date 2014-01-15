using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache;
using Rakuten.Framework.Cache.ProtoBuf;
using System.Collections.Generic;

namespace SerializationTests
{
    [TestClass]
    public class When_user_reference_without_protobuf_attributes_types_serialized
    {
        static readonly ProtoBufSerializer ProtoBufSerializer = new ProtoBufSerializer(new DummyStorage());
        
        public class SomeData
        {
            public string stringValue { get; set; }
            public int intValue { get; set; }
            public bool boolValue { get; set; }
            public string[] stringArray { get; set; }
        }

        public class NewData : SomeData
        {
            public Dictionary<string, int> Dictionary { get; set; }
        }

        static When_user_reference_without_protobuf_attributes_types_serialized()
        {
            ProtoBufSerializer.RegisterType(typeof(SomeData));
            ProtoBufSerializer.RegisterSubType(typeof(SomeData), typeof(NewData));
        }

        [TestMethod]
        public void it_should_be_serializable_deserialiazble()
        {
            var someData = new SomeData
            {
                stringValue = "qwerty",
                intValue = 12345,
                boolValue = false,
                stringArray = new[] {"q", "w", "e"}
            };
            var stream = ProtoBufSerializer.Serialize(someData);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<SomeData>(stream).ShouldBeEquivalentTo(someData);
        }

        [TestMethod]
        public void its_subtype_should_be_serializable_deserialiazble()
        {
            var newData = new NewData
            {
                stringValue = "qwerty",
                intValue = 12345,
                boolValue = true,
                stringArray = new[] { "q", "w", "e" },
                Dictionary = new Dictionary<string, int> { { "kq", 1 }, { "k2", 2 } }
            };
            var stream = ProtoBufSerializer.Serialize(newData);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<NewData>(stream).ShouldBeEquivalentTo(newData);
        }

        [TestMethod]
        public void cache_entry_should_support_this_type()
        {
            var someData = new SomeData
            {
                stringValue = "qwerty",
                intValue = 12345,
                boolValue = false,
                stringArray = new[] { "q", "w", "e" }
            };
            var cacheEntry = new CacheEntry<SomeData> {Value = someData};
            var stream = ProtoBufSerializer.Serialize(cacheEntry);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<CacheEntry<SomeData>>(stream).ShouldBeEquivalentTo(cacheEntry);
        }

        [TestMethod]
        public void cache_entry_should_support_this_type_subtype()
        {
            var newData = new NewData
            {
                stringValue = "qwerty",
                intValue = 12345,
                boolValue = true,
                stringArray = new[] { "q", "w", "e" },
                Dictionary = new Dictionary<string, int> { { "kq", 1 }, { "k2", 2 } }
            };
            var cacheEntry = new CacheEntry<SomeData> { Value = newData };
            var stream = ProtoBufSerializer.Serialize(cacheEntry);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<CacheEntry<SomeData>>(stream).ShouldBeEquivalentTo(cacheEntry);
        }
    }
}
