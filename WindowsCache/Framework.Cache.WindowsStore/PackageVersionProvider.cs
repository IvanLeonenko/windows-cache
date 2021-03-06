﻿using System;
using Windows.ApplicationModel;

namespace Framework.Cache.WindowsStore
{
    public class PackageVersionProvider : IVersionProvider
    {
        public Version GetVersion()
        {
            var packageVersion = Package.Current.Id.Version;
            return new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
    }
}
