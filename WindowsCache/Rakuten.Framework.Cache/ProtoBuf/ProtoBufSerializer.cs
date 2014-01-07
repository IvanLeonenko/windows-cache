using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Rakuten.Framework.Cache.Storage;

namespace Rakuten.Framework.Cache.ProtoBuf
{
    public class ProtoBufSerializer : ISerializer
    {
        private static readonly Type CacheEntryTemplate = typeof(CacheEntry<>);
        private static readonly Dictionary<Type, int> TypeToPropertyIndex = new Dictionary<Type, int>();
        private static readonly Dictionary<Type, int> TypeToSubtypeIndex = new Dictionary<Type, int>();
        private readonly IStorage _storage;

        public ProtoBufSerializer(IStorage storage)
        {
            _storage = storage;
        }

        static ProtoBufSerializer() 
        {
            RuntimeTypeModel.Default.Add(typeof(DateTimeOffset), false).SetSurrogate(typeof(DateTimeOffsetSurrogate));
            RegisterGenericCacheEntry(typeof(DateTimeOffset));
            RegisterGenericCacheEntry(typeof(String));
            RegisterGenericCacheEntry(typeof(Double));
            RegisterGenericCacheEntry(typeof(float));
            RegisterGenericCacheEntry(typeof(Int16));
            RegisterGenericCacheEntry(typeof(Int32));
            RegisterGenericCacheEntry(typeof(Int64));
            RegisterGenericCacheEntry(typeof(UInt16));
            RegisterGenericCacheEntry(typeof(UInt32));
            RegisterGenericCacheEntry(typeof(UInt64));
            RegisterGenericCacheEntry(typeof(Decimal));
            RegisterGenericCacheEntry(typeof(DateTime));
            RegisterGenericCacheEntry(typeof(Byte));
            RegisterGenericCacheEntry(typeof(SByte));
            RegisterGenericCacheEntry(typeof(Boolean));
            RegisterGenericCacheEntry(typeof(Char));
        }

        private static void RegisterGenericCacheEntry(Type type)
        {
            RuntimeTypeModel.Default[typeof(ICacheEntry)].AddSubType(NextSubtypeIndex(typeof(ICacheEntry)), CacheEntryTemplate.MakeGenericType(type));
        }

        public void RegisterType(Type type)
        {
            if (!RuntimeTypeModel.Default.CanSerializeBasicType(type))
            {
                var metaType = RuntimeTypeModel.Default.Add(type, true);
                AddProperties(type, metaType);
            }

            RegisterGenericCacheEntry(type);

            if (!RuntimeTypeModel.Default.CanSerializeBasicType(type))
            {
                //save to 
            }
        }

        public void RegisterSubType(Type type, Type subType)
        {
            RuntimeTypeModel.Default[type].AddSubType(NextSubtypeIndex(subType), subType);
            
            var subTypeMetaType = RuntimeTypeModel.Default[subType];
            AddProperties(subType, subTypeMetaType);

            RegisterGenericCacheEntry(type);
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
        public bool CanSerialize(Type type)
        {
            return RuntimeTypeModel.Default.CanSerialize(type) && RuntimeTypeModel.Default.CanSerialize(CacheEntryTemplate.MakeGenericType(type));
        }

        public T Deserialize<T>(Stream stream)
        {
            stream.Position = 0;

            using (stream)
            {
                return Serializer.Deserialize<T>(stream);
            }
        }

        public Stream Serialize<T>(T value)
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
