using System;
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
    public class When_nullable_value_types_serialized
    {
        static readonly ProtoBufSerializer ProtoBufSerializer = new ProtoBufSerializer(new DummyStorage());
        
        [TestMethod]
        public void Nullable_Int16_should_be_serializable_deserialiazble()
        {
            Int16? value = 5;
            var stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Int16?>(stream).Should().Be(value);

            value = Int16.MaxValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Int16?>(stream).Should().Be(value);

            value = Int16.MinValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Int16?>(stream).Should().Be(value);

            value = null;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Int16?>(stream).Should().Be(value);
        }

        [TestMethod]
        public void Nullable_Int32_should_be_serializable_deserialiazble()
        {
            Int32? value = 5;
            var stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Int32?>(stream).Should().Be(value);

            value = Int16.MaxValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Int32?>(stream).Should().Be(value);

            value = Int16.MinValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Int32?>(stream).Should().Be(value);

            value = null;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Int32?>(stream).Should().Be(value);
        }

        [TestMethod]
        public void Nullable_Int64_should_be_serializable_deserialiazble()
        {
            Int32? value = 5;
            var stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Int32?>(stream).Should().Be(value);

            value = Int16.MaxValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Int32?>(stream).Should().Be(value);

            value = Int16.MinValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Int32?>(stream).Should().Be(value);

            value = null;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Int32?>(stream).Should().Be(value);
        }

        [TestMethod]
        public void Nullable_UInt16_should_be_serializable_deserialiazble()
        {
            UInt16? value = null;
            var stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt16?>(stream).Should().Be(value);

            value = 42;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt16?>(stream).Should().Be(value);

            value = default(UInt16?);
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt16?>(stream).Should().Be(value);

            value = UInt16.MinValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt16?>(stream).Should().Be(value);

            value = UInt16.MaxValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt16?>(stream).Should().Be(value);
        }

        [TestMethod]
        public void Nullable_UInt32_should_be_serializable_deserialiazble()
        {
            UInt32? value = null;
            var stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt32?>(stream).Should().Be(value);

            value = 42;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt32?>(stream).Should().Be(value);

            value = default(UInt32?);
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt32?>(stream).Should().Be(value);

            value = UInt32.MinValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt32?>(stream).Should().Be(value);

            value = UInt32.MaxValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt32?>(stream).Should().Be(value);
        }

        [TestMethod]
        public void Nullable_UInt64_should_be_serializable_deserialiazble()
        {
            UInt64? value = null;
            var stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt64?>(stream).Should().Be(value);

            value = 42;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt64?>(stream).Should().Be(value);

            value = default(UInt32?);
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt64?>(stream).Should().Be(value);

            value = UInt64.MinValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt64?>(stream).Should().Be(value);

            value = UInt64.MaxValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<UInt64?>(stream).Should().Be(value);
        }

        [TestMethod]
        public void Nullable_DateTime_should_be_serializable_deserialiazble()
        {
            DateTime? dateTime = DateTime.Now;
            var stream = ProtoBufSerializer.Serialize(dateTime);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<DateTime?>(stream).Should().Be(dateTime);

            dateTime = default(DateTime?);
            stream = ProtoBufSerializer.Serialize(dateTime);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<DateTime?>(stream).Should().Be(dateTime);
        }

        /*
         Nullable DateTimeOffset is not fully supported, only when it has value
         */
        [TestMethod]
        public void Nullable_DateTimeOffset_having_a_value_should_be_serializable_deserialiazble()
        {
            DateTimeOffset? dateTimeOffset = DateTimeOffset.Now;
            var stream = ProtoBufSerializer.Serialize(dateTimeOffset);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<DateTimeOffset?>(stream).Should().Be(dateTimeOffset);

        }

        [TestMethod]
        public void Nullable_float_should_be_serializable_deserialiazble()
        {
            float? value = 0.1f;
            var stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<float?>(stream).Should().Be(value);

            value = default(float?);
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<float?>(stream).Should().Be(value);
        }

        [TestMethod]
        public void Nullable_Double_should_be_serializable_deserialiazble()
        {
            double? value = Double.MinValue;
            var stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<double?>(stream).Should().Be(value);

            value = Double.MaxValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<double?>(stream).Should().Be(value);

            value = default(double?);
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<double?>(stream).Should().Be(value);
        }

        [TestMethod]
        public void Nullable_Char_should_be_serializable_deserialiazble()
        {
            char? value = Char.MinValue;
            var stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Char?>(stream).Should().Be(value);

            value = 'a';
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Char?>(stream).Should().Be(value);

            value = Char.MaxValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Char?>(stream).Should().Be(value);

            value = default(Char?);
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Char?>(stream).Should().Be(value);
        }

        [TestMethod]
        public void Nullable_Boolean_should_be_serializable_deserialiazble()
        {
            bool? value = false;
            var stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Boolean?>(stream).Should().Be(value);

            value = true;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Boolean?>(stream).Should().Be(value);

            value = default(bool?);
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Boolean?>(stream).Should().Be(value);
        }

        [TestMethod]
        public void Nullable_Byte_should_be_serializable_deserialiazble()
        {
            Byte? value = Byte.MinValue;
            var stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Byte?>(stream).Should().Be(value);

            value = 42;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Byte?>(stream).Should().Be(value);

            value = Byte.MaxValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Byte?>(stream).Should().Be(value);

            value = default(Byte?);
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Byte?>(stream).Should().Be(value);
        }
        
        [TestMethod]
        public void Nullable_SByte_should_be_serializable_deserialiazble()
        {
            SByte? value = SByte.MinValue;
            var stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<SByte?>(stream).Should().Be(value);

            value = 42;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<SByte?>(stream).Should().Be(value);

            value = SByte.MaxValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<SByte?>(stream).Should().Be(value);

            value = default(SByte?);
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<SByte?>(stream).Should().Be(value);
        }
        
        [TestMethod]
        public void Nullable_Decimal_should_be_serializable_deserialiazble()
        {
            decimal? value = Decimal.MinValue;
            var stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Decimal?>(stream).Should().Be(value);

            value = 42m;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Decimal?>(stream).Should().Be(value);

            value = Decimal.MaxValue;
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Decimal?>(stream).Should().Be(value);

            value = default(Decimal?);
            stream = ProtoBufSerializer.Serialize(value);
            stream.Should().NotBeNull();
            ProtoBufSerializer.Deserialize<Decimal?>(stream).Should().Be(value);
        }
    }
}
