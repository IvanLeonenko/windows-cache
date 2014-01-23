using System.Diagnostics;
using System.Reflection;

namespace Rakuten.Framework.Cache.Desktop
{
    public class EntryAssemblyVersionProvider : IVersionProvider
    {
        public System.Version GetVersion()
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new System.Version(fvi.FileVersion);
        }
    }
}
