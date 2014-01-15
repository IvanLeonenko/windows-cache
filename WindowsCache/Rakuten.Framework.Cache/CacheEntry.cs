using ProtoBuf;
using System;
using System.ComponentModel;

namespace Rakuten.Framework.Cache
{
    [ProtoContract]
    public interface ICacheEntry
    {

        [ProtoMember(1)]
        Int32 Size { get; set; }
        
        [ProtoMember(2)]
        DateTime LastAccessTime { get; set; }

        [ProtoMember(3)]
        bool IsInMemory { get; set; }

        [ProtoMember(4)]
        string FileName { get; set; }

        void RemoveValue();
    }

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
        [ProtoMember(5), DefaultValue(EntryType.TemplateType)]
        public EntryType EntryType { get; set; }

        /*
         Get accessot to be used only by serializer, to access Value please use GetValue method
         */
        private T _value;
        [ProtoMember(6)]
        public T Value
        {
            get
            {
                /*
                 * this is to omit two serializations
                 */
                //return EntryType == EntryType.TemplateType ? default(T) : _value;
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public T GetValue()
        {
           return _value;
        }

        [ProtoMember(7)]
        public byte[] SerializedValue { get; set; }

        [ProtoMember(8)]
        public Int32 Size { get; set; }

        #endregion

        #region Timestamps section
        [ProtoMember(9)]
        public DateTime CreatedTime { get; set; }

        [ProtoMember(10)]
        public DateTime ModifiedTime { get; set; }

        [ProtoMember(11)]
        public DateTime LastAccessTime { get; set; }

        [ProtoMember(12)]
        public DateTime ExpirationTime { get; set; }

        public bool IsExpired { get { return DateTime.UtcNow >= ExpirationTime; } }

        #endregion

        private string _fileName;
        [ProtoMember(13)]
        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(_fileName))
                    _fileName = Guid.NewGuid().ToString();
                return _fileName;
            }
            set { _fileName = value; }
        }

        [ProtoMember(14)]
        public bool IsInMemory { get; set; }
        
        public void RemoveValue()
        {
            Value = default(T);
            IsInMemory = false;
        }
    }
}
