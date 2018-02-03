using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Config4Net.Core.StoreService
{
    /// <summary>
    /// Save and load config data from win32 registry.
    /// </summary>
    public sealed class RegistryStoreService : IStoreService
    {
        private const string RootKeyName = "Software";

        /// <summary>
        /// Uses app name instead of resolving from config directory.
        /// </summary>
        public bool IsUseAppName { get; set; }

        /// <summary>
        /// Get saved entries from registry.
        /// </summary>
        /// <param name="configDir">Uses to resolve registry key that saved entries.</param>
        /// <param name="fileExtension">Ignored.</param>
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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