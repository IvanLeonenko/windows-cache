using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Rakuten.Framework.Cache
{
    [ProtoContract]
    public class CacheData
    {
        [ProtoMember(1)]
        public Dictionary<string, ICacheEntry> Entries { get; set; }

        [ProtoMember(2)]
        public Dictionary<string, Type> KeyToType { get; set; }

        public CacheData()
        {
            Entries = new Dictionary<string, ICacheEntry>();
            KeyToType = new Dictionary<string, Type>();
        }
    }
}
