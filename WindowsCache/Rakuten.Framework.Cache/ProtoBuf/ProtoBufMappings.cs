using ProtoBuf;
using System.Collections.Generic;

namespace Rakuten.Framework.Cache.ProtoBuf
{
    [ProtoContract]
    public class ProtoBufMappings
    {
        [ProtoMember(1, IsRequired = false)]
        public Dictionary<string, Dictionary<string, int>> TypePropertiesIndices { get; set; }
        
        [ProtoMember(2, IsRequired = false)]
        public Dictionary<string, Dictionary<string, int>> TypeSubTypesIndices { get; set; }

        public ProtoBufMappings()
        {
            TypePropertiesIndices = new Dictionary<string, Dictionary<string, int>>();
            TypeSubTypesIndices = new Dictionary<string, Dictionary<string, int>>();
        }
    }
}
