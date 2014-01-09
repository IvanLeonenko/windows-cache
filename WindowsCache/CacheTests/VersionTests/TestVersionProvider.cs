using Rakuten.Framework.Cache;
using System;

namespace CacheTests.VersionTests
{
    class TestVersionProvider : IVersionProvider
    {
        private readonly Version _version;
        public TestVersionProvider(Version version)
        {
            _version = version;
        }

        public Version GetVersion()
        {
            return _version;
        }
    }
}
