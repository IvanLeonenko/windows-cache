using CacheTests.VersionTests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakuten.Framework.Cache;
using Rakuten.Framework.Cache.ProtoBuf;
using Rakuten.Framework.Cache.Storage;
using System;

namespace CacheTests.CacheContainerTests
{
    [TestClass]
    public class Cache_container_should
    {
        [TestMethod]
        public void support_named_registration()
        {
            var c = new CacheContainer();
            c.Register<IStorage, TestStorage>("filesystem");
            var m = c.Resolve<IStorage>("filesystem");
            
            m.Should().NotBeNull();

            m.Write("key1", "value1");
            m.GetString("key1").Should().Be("value1");
        }

        [TestMethod]
        public void support_anonymous_registration()
        {
            var c = new CacheContainer();
            c.Register<IStorage, TestStorage>();
            var m = c.Resolve<IStorage>();

            m.Should().NotBeNull();

            m.Write("key1", "value1");
            m.GetString("key1").Should().Be("value1");
        }

        [TestMethod]
        public void support_anonymous_sub_dependency()
        {
            var c = new CacheContainer();
            c.Register<IStorage, TestStorage>();
            c.Register<ISerializer, TestSerializer>();
            var m = c.Resolve<ISerializer>();

            m.Should().NotBeNull();

            m.CanSerialize(typeof (String)).Should().BeTrue();
        }

        [TestMethod]
        public void support_initialization_types_with_value()
        {
            var c = new CacheContainer();
            c.Register<IStorage, TestStorageWithParameter>("testStorage").WithValue("value", "defaultCacheValue");
            var value = c.Resolve<IStorage>("testStorage").GetString("default");
            value.Should().Be("defaultCacheValue");
        }

        [TestMethod]
        public void support_named_sub_dependencies()
        {
            var c = new CacheContainer();
            c.Register<IMathNode, Number>("five").WithValue("number", 5);
            c.Register<IMathNode, Number>("six").WithValue("number", 6);
            c.Register<IMathNode, Add>("add").WithDependency("m1", "five").WithDependency("m2", "six");
            c.Resolve<IMathNode>("add").Calculate().Should().Be(11);
        }

        [TestMethod]
        public void support_named_sub_dependency_out_of_order()
        {
            var c = new CacheContainer();
            c.Register<IMathNode, Add>("add").WithDependency("m1", "five").WithDependency("m2", "six");
            c.Register<IMathNode, Number>("five").WithValue("number", 5);
            c.Register<IMathNode, Number>("six").WithValue("number", 6);
            c.Resolve<IMathNode>("add").Calculate().Should().Be(11);
        }

        [TestMethod]
        public void support_singleton_initialization()
        {
            var c = new CacheContainer();
            c.Register<IMathNode, Zero>().AsSingleton();
            c.Resolve<IMathNode>().Should().Be(c.Resolve<IMathNode>());
        }

        [TestMethod]
        public void support_multi_instance_initialization()
        {
            var c = new CacheContainer();
            c.Register<IMathNode, Zero>();
            c.Resolve<IMathNode>().Should().NotBe(c.Resolve<IMathNode>());
        }

        public interface IMathNode
        {
            int Calculate();
        }

        public class Zero : IMathNode
        {
            public int Calculate()
            {
                return 0;
            }
        }

        public class Number : IMathNode
        {
            private readonly int number;

            public Number(int number)
            {
                this.number = number;
            }

            public int Calculate()
            {
                return number;
            }
        }

        public class Add : IMathNode
        {
            private readonly IMathNode m1;
            private readonly IMathNode m2;

            public Add(IMathNode m1, IMathNode m2)
            {
                this.m1 = m1;
                this.m2 = m2;
            }

            public int Calculate()
            {
                return m1.Calculate() + m2.Calculate();
            }
        }
    }
}
