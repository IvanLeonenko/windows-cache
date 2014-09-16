using System.Diagnostics;
using System.Reflection;

namespace Framework.Cache.Desktop
{
    public class EntryAssemblyVersionProvider : IVersionProvider
    {
        public System.Version GetVersion()
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new System.Version(fvi.FileVersion);
        }
    }
}
