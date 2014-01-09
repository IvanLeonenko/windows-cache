using System.Linq;
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
        private ProtoBufMappings _protoBufMappings = new ProtoBufMappings();
        private List<Type> _userTypes;

        public ProtoBufSerializer(IStorage storage, List<Type> userTypes = null)
        {
            _storage = storage;
            _userTypes = userTypes;

            RestoreMappings();
            
            if (_userTypes == null) 
                return;
            foreach (var userType in _userTypes)
            {
                RegisterType(userType);
            }
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
            //if (!RuntimeTypeModel.Default.CanSerializeBasicType(type))
            //{
            //    var metaType = RuntimeTypeModel.Default.Add(type, true);
            //    AddProperties(type, metaType);
            //}

            //RegisterGenericCacheEntry(type);

            UpdateMapping(type);
            //RegisterGenericCacheEntry(type);
            //UpdateMapping(CacheEntryTemplate.MakeGenericType(type));
        }
        
        private void UpdateMapping(Type type)
        {
            if (RuntimeTypeModel.Default.CanSerializeBasicType(type))
                return;

            var typeName = type.FullName;
            var mapping = _protoBufMappings.TypePropertiesIndices.ContainsKey(typeName) ? _protoBufMappings.TypePropertiesIndices[typeName] : new Dictionary<string, int>();
            
            var typeProperties = type.GetTypeInfo().DeclaredProperties.ToLookup(x => x.Name);

            foreach (var item in mapping.Select(x => x.Key).Where(x => !typeProperties.Contains(x)).ToList())
            {
                mapping.Remove(item);
            }
            
            var metaType = RuntimeTypeModel.Default.Add(type, true);
            
            var maxInd = 0;
            var addProperties = new List<string>();
            foreach (var property in typeProperties)
            {
                if (mapping.ContainsKey(property.Key))
                {
                    var currentIndex = mapping[property.Key];
                    if (currentIndex > maxInd)
                        maxInd = currentIndex;
                    metaType.Add(currentIndex, property.Key);
                    metaType[currentIndex].IsRequired = false;
                }
                else
                {
                    addProperties.Add(property.Key);
                }
            }

            foreach (var property in addProperties)
            {
                mapping.Add(property, ++maxInd);
                metaType.Add(maxInd, property);
                metaType[maxInd].IsRequired = false;
            }

            _protoBufMappings.TypePropertiesIndices[typeName] = mapping;

            
            //todo: TypeSubTypesIndices should be cleaned up on initialization (e g cctor)
            maxInd = 0;
            var cacheEntryTypeName = typeof (ICacheEntry).FullName;
            if (_protoBufMappings.TypeSubTypesIndices.ContainsKey(cacheEntryTypeName))
            {
                maxInd = _protoBufMappings.TypeSubTypesIndices[cacheEntryTypeName].Select(subtype => subtype.Value).Concat(new[] {maxInd}).Max();
                maxInd++;
                if (!_protoBufMappings.TypeSubTypesIndices[cacheEntryTypeName].ContainsKey(typeName))
                    _protoBufMappings.TypeSubTypesIndices[cacheEntryTypeName].Add(typeName, maxInd);
            }
            else
            {
                maxInd++;
                _protoBufMappings.TypeSubTypesIndices[cacheEntryTypeName] = new Dictionary<string, int> { { typeName, maxInd } };
            }
            RuntimeTypeModel.Default[typeof(ICacheEntry)].AddSubType(_protoBufMappings.TypeSubTypesIndices[cacheEntryTypeName][typeName], CacheEntryTemplate.MakeGenericType(type));
            
            SaveMappings();
        }

        private void RestoreMappings()
        {
            using (var stream = _storage.ReadStream("type.mappings.protobuf"))
            {
                if (stream != null && stream.Length > 0)
                    _protoBufMappings = Deserialize<ProtoBufMappings>(stream);
            }
        }

        private void SaveMappings()
        {
            if (_protoBufMappings != null && _protoBufMappings.TypePropertiesIndices.Count <= 0 && _protoBufMappings.TypeSubTypesIndices.Count <= 0) 
                return;
            using (var stream = Serialize(_protoBufMappings))
            {
                _storage.WriteStream("type.mappings.protobuf", stream);
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

        public static int NextSubtypeIndex(Type type)
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
