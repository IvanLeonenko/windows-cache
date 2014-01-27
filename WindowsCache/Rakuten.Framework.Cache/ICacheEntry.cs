using ProtoBuf;
using System;

namespace Rakuten.Framework.Cache
{
    [ProtoContract]
    public interface ICacheEntry
    {
        [ProtoMember(1)]
        Int32 Size { get; set; }

        [ProtoMember(2)]
        DateTime LastAccessTime { get; set; }

        bool IsInMemory { get; set; }

        [ProtoMember(4)]
        string FileName { get; set; }

        [ProtoMember(5)]
        Type Type { get; set; }

        void RemoveValue();
    }
}
