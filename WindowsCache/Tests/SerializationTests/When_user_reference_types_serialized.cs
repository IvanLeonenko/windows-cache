using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache.ProtoBuf;

namespace SerializationTests
{
    [TestClass]
    public class When_user_reference_without_protobuf_attributes_types_serialized
    {
        public class SomeData
        {
            public string stringValue { get; set; }
            public int intValue { get; set; }
            public bool boolValue { get; set; }
            public string[] stringArray { get; set; }
        }

        [TestMethod]
        public void it_should_be_serializable_deserialiazble()
        {
            ProtoBufSerializer.RegisterType(typeof(SomeData));
            
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
    }
}
