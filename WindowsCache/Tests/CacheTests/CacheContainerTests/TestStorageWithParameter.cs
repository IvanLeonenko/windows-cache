using CacheTests.VersionTests;

namespace CacheTests.CacheContainerTests
{
    class TestStorageWithParameter : TestStorage
    {
        public TestStorageWithParameter(string value)
        {
            KeyToStrings.Add("default", value);
        }
    }
}
