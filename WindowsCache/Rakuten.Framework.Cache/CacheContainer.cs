// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Container.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Container type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Framework.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The container.
    /// </summary>
    public class CacheContainer
    {
        protected readonly Dictionary<string, Func<object>> Services = new Dictionary<string, Func<object>>();
        protected readonly Dictionary<Type, string> ServiceNames = new Dictionary<Type, string>();

        public DependencyManager Register<S, C>() where C : S
        {
            return Register<S, C>(Guid.NewGuid().ToString());//todo: guid or type?
        }

        public DependencyManager Register<S, C>(string name) where C : S
        {
            if (!ServiceNames.ContainsKey(typeof(S)))
            {
                ServiceNames[typeof(S)] = name;
            }
            return new DependencyManager(this, name, typeof(C));
        }

        public T Resolve<T>(string name) where T : class
        {
            return (T)Services[name]();
        }

        public T Resolve<T>() where T : class
        {
            return Resolve<T>(ServiceNames[typeof(T)]);
        }

        public class DependencyManager
        {
            private readonly CacheContainer _container;
            private readonly Dictionary<string, Func<object>> _args;
            private readonly string _name;

            internal DependencyManager(CacheContainer container, string name, Type type)
            {
                _container = container;
                _name = name;
                
                var c = type.GetTypeInfo().DeclaredConstructors.First();
                _args = c.GetParameters()
                    .ToDictionary<ParameterInfo, string, Func<object>>(
                    x => x.Name,
                    x => (() => container.Services[container.ServiceNames[x.ParameterType]]())
                    );

                container.Services[name] = () => c.Invoke(_args.Values.Select(x => x()).ToArray());
            }

            public DependencyManager AsSingleton()
            {
                object value = null;
                Func<object> service = _container.Services[_name];
                _container.Services[_name] = () => value ?? (value = service());
                return this;
            }

            public DependencyManager WithDependency(string parameter, string component)
            {
                _args[parameter] = () => _container.Services[component]();
                return this;
            }

            public DependencyManager WithValue(string parameter, object value)
            {
                _args[parameter] = () => value;
                return this;
            }
        }
    }
}
