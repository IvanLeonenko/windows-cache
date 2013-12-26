using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Rakuten.Framework.Cache
{
    public class ProtobufHelper
    {
        private static readonly Type CacheEntryTemplate = typeof(CacheEntry<>);
        private static readonly Dictionary<Type, int> TypeToPropertyIndex = new Dictionary<Type, int>();
        private static readonly Dictionary<Type, int> TypeToSubtypeIndex = new Dictionary<Type, int>(); 
        public static void RegisterType(Type type)
        {
            var metaType = RuntimeTypeModel.Default.Add(type, true);
            
            AddProperties(type, metaType);
            
            RuntimeTypeModel.Default[typeof(ICacheEntry)].AddSubType(NextSubtypeIndex(type), CacheEntryTemplate.MakeGenericType(type));
        }

        public static void RegisterSubType(Type type, Type subType)
        {
            var metaType = RuntimeTypeModel.Default[type].AddSubType(NextSubtypeIndex(subType), subType);

            AddProperties(subType, metaType);

            RuntimeTypeModel.Default[typeof(ICacheEntry)].AddSubType(NextSubtypeIndex(type), CacheEntryTemplate.MakeGenericType(type));
        }


        /// <summary>
        /// Checks if type can be serialized. Unfortunately not all type configurations supported by protobuf (e.g. generics subtypes),
        ///  so this check doesn't guarantee that type is serialized.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool CanSerialize(Type type)
        {
            return RuntimeTypeModel.Default.CanSerialize(type) && RuntimeTypeModel.Default.CanSerialize(CacheEntryTemplate.MakeGenericType(type));
        }

        public static T Deserialize<T>(Stream stream)
        {
            using (stream)
            {
                return Serializer.Deserialize<T>(stream);
            }
        }

        public static Stream Serialize<T>(T value)
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, value);
            return stream;
        }

        private static void AddProperties(Type type, MetaType metaType)
        {
            foreach (var property in type.GetTypeInfo().DeclaredProperties)
            {
                metaType.Add(NextTypePropertyIndex(type), property.Name);
            }
        }

        private static int NextSubtypeIndex(Type type)
        {
            if (!TypeToSubtypeIndex.ContainsKey(type))
                TypeToSubtypeIndex[type] = 100;
            return ++TypeToSubtypeIndex[type];
        }

        private static int NextTypePropertyIndex(Type type)
        {
            if (!TypeToPropertyIndex.ContainsKey(type))
                TypeToPropertyIndex[type] = 0;
            return ++TypeToPropertyIndex[type];
        }
    }
}
