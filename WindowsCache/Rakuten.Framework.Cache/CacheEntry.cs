using ProtoBuf;
using System;
using System.ComponentModel;

namespace Rakuten.Framework.Cache
{
    [ProtoContract]
    public class CacheEntry<T> : ICacheEntry
    {
        #region Data section
        [ProtoMember(6), DefaultValue(EntryType.TemplateType)]
        public EntryType EntryType { get; set; }

        public T Value { get; set; }

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
        
        public bool IsInMemory { get; set; }

        [ProtoMember(15)]
        public Type Type { get; set; }

        public void RemoveValue()
        {
            Value = default(T);
            IsInMemory = false;
        }
    }
}
