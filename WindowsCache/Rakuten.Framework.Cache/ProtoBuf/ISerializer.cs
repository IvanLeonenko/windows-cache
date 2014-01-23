using System;
using System.IO;
using System.Threading.Tasks;

namespace Rakuten.Framework.Cache.ProtoBuf
{
    public interface ISerializer
    {
        Task Initialize ();
        Task RegisterType(Type type);

        void RegisterSubType(Type type, Type subType);

        bool CanSerialize(Type type);

        T Deserialize<T>(Stream stream);

        Stream Serialize<T>(T value);
    }
}
