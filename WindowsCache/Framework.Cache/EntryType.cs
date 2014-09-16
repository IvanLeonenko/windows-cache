using ProtoBuf;

namespace Framework.Cache
{
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
}
