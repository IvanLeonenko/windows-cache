using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf;
using Rakuten.Framework.Cache.ProtoBuf;

namespace SerializationTests
{
    [TestClass]
    public class When_user_reference_with_protobuf_attributes_types_serialized
    {
        [ProtoContract]
        public class SomeData
        {
            [ProtoMember(1)]
            public string stringValue { get; set; }
            [ProtoMember(2)]
            public int intValue { get; set; }
            [ProtoMember(3)]
            public bool boolValue { get; set; }
            [ProtoMember(4)]
            public string[] stringArray { get; set; }
        }

        [TestMethod]
        public void it_should_be_serializable_deserialiazble()
        {
            var someData = new SomeData
            {
                stringValue = "qwerty",
                intValue = 12345,
                boolValue = false,
                stringArray = new[] { "q", "w", "e" }
            };
            var stream = ProtoBufSerializer.Serialize(someData);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<SomeData>(stream).ShouldBeEquivalentTo(someData);
        }
    }
}
