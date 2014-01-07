using System;
using System.IO;
using Rakuten.Framework.Cache.Storage;

namespace Rakuten.Framework.Cache.ProtoBuf
{
    public interface ISerializer
    {
        //void RegisterType(Type type);

        //void RegisterSubType(Type type, Type subType);

        bool CanSerialize(Type type);

        T Deserialize<T>(Stream stream);

        Stream Serialize<T>(T value);
    }
}
