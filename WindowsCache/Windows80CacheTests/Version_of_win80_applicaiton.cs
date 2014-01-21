﻿using System.Threading.Tasks;
using Windows.ApplicationModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Rakuten.Framework.Cache.WindowsStore;

namespace Windows80CacheTests
{
    [TestClass]
    public class Version_of_win80_applicaiton
    {
        [TestMethod]
        public async Task should_be_the_same_as_current_package_has()
        {
            var packageVersion = Package.Current.Id.Version;
            var versoin = new WindowsStoreApplicationVersionProvider();
            versoin.GetVersion().Major.Should().Be(packageVersion.Major);
            versoin.GetVersion().Minor.Should().Be(packageVersion.Minor);
            versoin.GetVersion().Revision.Should().Be(packageVersion.Revision);
            versoin.GetVersion().Build.Should().Be(packageVersion.Build);
        }
    }
}