using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Cache.ProtoBuf;

namespace SerializationTests
{
    [TestClass]
    public class When_value_types_serialized
    {
        static readonly ProtoBufSerializer ProtoBufSerializer = new ProtoBufSerializer(new DummyStorage());
        
        [TestMethod]
        public void Int16_should_be_serializable_deserialiazble()
        {
            var stream = ProtoBufSerializer.Serialize(42);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<int>(stream).Should().Be(42);

            stream = ProtoBufSerializer.Serialize(Int16.MaxValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<int>(stream).Should().Be(Int16.MaxValue);

            stream = ProtoBufSerializer.Serialize(Int16.MinValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<int>(stream).Should().Be(Int16.MinValue);

            stream = ProtoBufSerializer.Serialize(default(Int32));
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<int>(stream).Should().Be(default(Int32));

            stream = ProtoBufSerializer.Serialize(-42);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<int>(stream).Should().Be(-42);
        }

        [TestMethod]
        public void Int32_should_be_serializable_deserialiazble()
        {
            var stream = ProtoBufSerializer.Serialize(42);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<int>(stream).Should().Be(42);

            stream = ProtoBufSerializer.Serialize(default(Int32));
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<int>(stream).Should().Be(default(Int32));

            stream = ProtoBufSerializer.Serialize(-42);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<int>(stream).Should().Be(-42);
        }

        [TestMethod]
        public void Int64_should_be_serializable_deserialiazble()
        {
            var stream = ProtoBufSerializer.Serialize(1L << 33);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Int64>(stream).Should().Be(1L << 33);

            stream = ProtoBufSerializer.Serialize(default(Int64));
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Int64>(stream).Should().Be(default(Int64));

            stream = ProtoBufSerializer.Serialize(-1L << 33);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Int64>(stream).Should().Be(-1L << 33);
        }

        [TestMethod]
        public void UInt16_should_be_serializable_deserialiazble()
        {
            var stream = ProtoBufSerializer.Serialize(42);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt16>(stream).Should().Be(42);

            stream = ProtoBufSerializer.Serialize(default(UInt16));
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt16>(stream).Should().Be(default(UInt16));

            stream = ProtoBufSerializer.Serialize(UInt16.MinValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt16>(stream).Should().Be(UInt16.MinValue);

            stream = ProtoBufSerializer.Serialize(UInt16.MaxValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt16>(stream).Should().Be(UInt16.MaxValue);
        }

        [TestMethod]
        public void UInt32_should_be_serializable_deserialiazble()
        {
            var stream = ProtoBufSerializer.Serialize(42);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt32>(stream).Should().Be(42);

            stream = ProtoBufSerializer.Serialize(default(UInt32));
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt32>(stream).Should().Be(default(UInt32));

            stream = ProtoBufSerializer.Serialize(UInt32.MinValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt32>(stream).Should().Be(UInt32.MinValue);

            stream = ProtoBufSerializer.Serialize(UInt32.MaxValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt32>(stream).Should().Be(UInt32.MaxValue);
        }

        [TestMethod]
        public void UInt64_should_be_serializable_deserialiazble()
        {
            var stream = ProtoBufSerializer.Serialize(42);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt64>(stream).Should().Be(42);

            stream = ProtoBufSerializer.Serialize(default(UInt64));
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt64>(stream).Should().Be(default(UInt64));

            stream = ProtoBufSerializer.Serialize(UInt64.MinValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt64>(stream).Should().Be(UInt64.MinValue);

            stream = ProtoBufSerializer.Serialize(UInt64.MaxValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt64>(stream).Should().Be(UInt64.MaxValue);
        }

        [TestMethod]
        public void DateTime_should_be_serializable_deserialiazble()
        {
            var dateTime = DateTime.Now;
            var stream = ProtoBufSerializer.Serialize(dateTime);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<DateTime>(stream).Should().Be(dateTime);

            dateTime = default(DateTime);
            stream = ProtoBufSerializer.Serialize(dateTime);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<DateTime>(stream).Should().Be(dateTime);
        }

        [TestMethod]
        public void DateTimeOffset_should_be_serializable_deserialiazble()
        {
            var dateTimeOffset = DateTimeOffset.Now;
            var stream = ProtoBufSerializer.Serialize(dateTimeOffset);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<DateTimeOffset>(stream).Should().Be(dateTimeOffset);

            stream = ProtoBufSerializer.Serialize(default(DateTimeOffset));
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<DateTimeOffset>(stream).Should().Be(default(DateTimeOffset));
        }

        [TestMethod]
        public void Float_should_be_serializable_deserialiazble()
        {
            var stream = ProtoBufSerializer.Serialize(0.1f);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<float>(stream).Should().Be(0.1f);

            stream = ProtoBufSerializer.Serialize(default(float));
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<float>(stream).Should().Be(default(float));
        }

        [TestMethod]
        public void Double_should_be_serializable_deserialiazble()
        {
            var stream = ProtoBufSerializer.Serialize(Double.MinValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<double>(stream).Should().Be(Double.MinValue);

            stream = ProtoBufSerializer.Serialize(Double.MaxValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<double>(stream).Should().Be(Double.MaxValue);

            stream = ProtoBufSerializer.Serialize(default(double));
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<double>(stream).Should().Be(default(double));
        }

        [TestMethod]
        public void Char_should_be_serializable_deserialiazble()
        {
            var stream = ProtoBufSerializer.Serialize(Char.MinValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Char>(stream).Should().Be(Char.MinValue);

            stream = ProtoBufSerializer.Serialize('a');
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Char>(stream).Should().Be('a');

            stream = ProtoBufSerializer.Serialize(Char.MaxValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Char>(stream).Should().Be(Char.MaxValue);

            stream = ProtoBufSerializer.Serialize(default(Char));
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Char>(stream).Should().Be(default(Char));
        }

        [TestMethod]
        public void Boolean_should_be_serializable_deserialiazble()
        {
            var stream = ProtoBufSerializer.Serialize(false);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Boolean>(stream).Should().Be(false);

            stream = ProtoBufSerializer.Serialize(true);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Boolean>(stream).Should().Be(true);
        }

        [TestMethod]
        public void Byte_should_be_serializable_deserialiazble()
        {
            var stream = ProtoBufSerializer.Serialize(Byte.MinValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Byte>(stream).Should().Be(Byte.MinValue);

            stream = ProtoBufSerializer.Serialize(42);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Byte>(stream).Should().Be(42);

            stream = ProtoBufSerializer.Serialize(Byte.MaxValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Byte>(stream).Should().Be(Byte.MaxValue);

            stream = ProtoBufSerializer.Serialize(default(Byte));
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Byte>(stream).Should().Be(default(Byte));
        }

        [TestMethod]
        public void SByte_should_be_serializable_deserialiazble()
        {
            var stream = ProtoBufSerializer.Serialize(SByte.MinValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<SByte>(stream).Should().Be(SByte.MinValue);

            stream = ProtoBufSerializer.Serialize(42);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<SByte>(stream).Should().Be(42);

            stream = ProtoBufSerializer.Serialize(SByte.MaxValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<SByte>(stream).Should().Be(SByte.MaxValue);

            stream = ProtoBufSerializer.Serialize(default(SByte));
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<SByte>(stream).Should().Be(default(SByte));
        }

        [TestMethod]
        public void Decimal_should_be_serializable_deserialiazble()
        {
            var stream = ProtoBufSerializer.Serialize(Decimal.MinValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Decimal>(stream).Should().Be(Decimal.MinValue);

            stream = ProtoBufSerializer.Serialize(42m);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Decimal>(stream).Should().Be(42m);

            stream = ProtoBufSerializer.Serialize(Decimal.MaxValue);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Decimal>(stream).Should().Be(Decimal.MaxValue);

            stream = ProtoBufSerializer.Serialize(default(Decimal));
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Decimal>(stream).Should().Be(default(Decimal));
        }

        //todo: test enum using register type
    }
}
