using System;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Config4Net.Core
{
    public sealed class Win32RegistryStoreService : IStoreService
    {
        private const string RootKeyName = "Software";

        public bool IsUseAppName { get; set; }

        public Task<string[]> GetEntriesAsync(string configDir, string fileExtension)
        {
            return Task.Run(() =>
            {
                configDir = configDir.TrimEnd('\\').TrimEnd('/');
                var appName = ResolveAppName(Path.GetFileName(configDir));
                using (var hkey = OpenAppRegistryKey(appName, false))
                {
                    return hkey?.GetValueNames() ?? new string[] {};
                }
            });
        }

        public Task SaveAsync(string filePath, string content)
        {
            return Task.Run(() =>
            {
                var appName = ResolveAppName(GetDirectoryName(filePath));
                using (var hkey = OpenAppRegistryKey(appName, true))
                {
                    var key = ResolveKey(filePath);
                    hkey?.SetValue(key, content, RegistryValueKind.String);
                }
            });
        }

        private RegistryKey OpenAppRegistryKey(string appName, bool writable)
        {
            var rootKey = Registry.CurrentUser.OpenSubKey(RootKeyName, true);
            if (rootKey != null && rootKey.GetSubKeyNames().Any(keyName =>
                string.Compare(keyName, appName, StringComparison.InvariantCultureIgnoreCase) == 0))
            {
                return rootKey.OpenSubKey(appName, writable);
            }

            return rootKey?.CreateSubKey(appName);
        }

        private string ResolveKey(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        private string ResolveAppName(string dirName)
        {
            if (IsUseAppName)
                return GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var appName = dirName;
            if (string.IsNullOrEmpty(appName))
                appName = GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return appName;
        }

        private string GetDirectoryName(string filePath)
        {
            return Path.GetFileName(Path.GetDirectoryName(filePath));
        }

        public Task<string> LoadAsync(string filePath)
        {
            return Task.Run(() =>
            {
                var appName = ResolveAppName(GetDirectoryName(filePath));
                using (var hkey = OpenAppRegistryKey(appName, false))
                {
                    var key = ResolveKey(filePath);
                    return hkey?.GetValue(key, "").ToString();
                }
            });
        }
    }
}