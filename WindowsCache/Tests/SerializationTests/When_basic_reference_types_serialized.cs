using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Cache.ProtoBuf;

namespace SerializationTests
{
    [TestClass]
    public class When_basic_reference_types_serialized
    {
        static readonly ProtoBufSerializer ProtoBufSerializer = new ProtoBufSerializer(new DummyStorage());
        
        [TestMethod]
        public void String_should_be_serializable_deserialiazble()
        {
            var str = "test";
            var stream = ProtoBufSerializer.Serialize(str);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<string>(stream).Should().Be(str);

            str = "";
            stream = ProtoBufSerializer.Serialize(str);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<string>(stream).Should().Be(str);

            str = null;
            stream = ProtoBufSerializer.Serialize(str);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<string>(stream).Should().Be(str);
        }

        [TestMethod]
        public void One_dimension_array_should_be_serializable_deserialiazble()
        {
            var valueInt = new[] { 1, 2, 3 };
            var stream = ProtoBufSerializer.Serialize(valueInt);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<int[]>(stream).Should().BeEquivalentTo(valueInt);

            var value = new []{"1","2","3"};
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<string[]>(stream).Should().BeEquivalentTo(value);

            value = new string[0];
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<string[]>(stream).Should().BeEquivalentTo(value);

            value = null;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<string[]>(stream).Should().BeEmpty();
        }
        
        [TestMethod]
        public void Generic_list_should_be_serializable_deserialiazble()
        {
            var value = new List<string>{"1","2","3"};
            var stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<List<string>>(stream).Should().BeEquivalentTo(value);

            value = new List<string>();
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<List<string>>(stream).Should().BeEquivalentTo(value);

            value = null;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<List<string>>(stream).Should().BeEmpty();

            var intList = new List<int> { 1, 2, 3 };
            stream = ProtoBufSerializer.Serialize(intList);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<List<int>>(stream).Should().BeEquivalentTo(intList);
        }

        [TestMethod]
        public void Generic_dictionary_should_be_serializable_deserialiazble()
        {
            var intBoolValue = new Dictionary<int, bool> { { 1, false }, { 42, true } };
            var stream = ProtoBufSerializer.Serialize(intBoolValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Dictionary<int, bool>>(stream).ShouldBeEquivalentTo(intBoolValue);

            var value = new Dictionary<string, string> { { "k1", "v1" }, { "k2", "v2" } };
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Dictionary<string, string>>(stream).ShouldBeEquivalentTo(value);

            value = new Dictionary<string, string>();
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Dictionary<string, string>>(stream).ShouldBeEquivalentTo(value);

            value = null;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Dictionary<string, string>>(stream).Should().BeEmpty();
        }

        [TestMethod]
        public void Generic_IEnumerable_should_be_serializable_deserialiazble()
        {;

            var value = new HashSet<string> { "q", "w", "e" };
            var stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<HashSet<string>>(stream).ShouldBeEquivalentTo(value);

            var sortedSet = new SortedSet<string> { "a", "b", "c" };
            stream = ProtoBufSerializer.Serialize(sortedSet);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<HashSet<string>>(stream).ShouldBeEquivalentTo(sortedSet);
        }
    }
}
