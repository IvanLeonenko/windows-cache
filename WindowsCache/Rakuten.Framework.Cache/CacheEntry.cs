using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public enum EntryType
    {
        [ProtoEnum(Name = "TemplateType", Value = 0)]
        TemplateType = 0,
        [ProtoEnum(Name = "Binary", Value = 1)]
        Binary = 1,
        [ProtoEnum(Name = "String", Value = 2)]
        String = 2
    }
    
    [ProtoContract]
    public class CacheEntry<T> : ICacheEntry
    {
        #region Data section
        [ProtoMember(1), DefaultValue(EntryType.TemplateType)]
        public EntryType EntryType { get; set; }

        [ProtoMember(2)]
        public T Value { get; set; }

        [ProtoMember(3)]
        public byte[] SerializedValue { get; set; }

        [ProtoMember(4)]
        public UInt32 Size { get; set; }

        #endregion

        #region Timestamps section
        [ProtoMember(5)]
        public DateTime CreatedTime { get; set; }

        [ProtoMember(6)]
        public DateTime ModifiedTime { get; set; }

        [ProtoMember(7)]
        public DateTime LastAccessTime { get; set; }

        [ProtoMember(8)]
        public DateTime ExpirationTime { get; set; }

        public bool IsExpired { get { return DateTime.UtcNow >= ExpirationTime; } }

        #endregion


        // type enum binary, string, type   ?
        // affects how value saved
        // as binary, big string (> 1024 bytes) or serialized by protobuf and saved
        //
        //in case of binary or big string or big serialized object (> 1024 bytes) 
        // serialized in separate files
        //
        //

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
