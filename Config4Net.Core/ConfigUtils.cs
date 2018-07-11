using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Config4Net.Core.Manager;
using Config4Net.Core.StoreService;

namespace Config4Net.Core
{
    /// <summary>
    ///     Utilities for <see cref="Config" /> that uses <see cref="Config.Default" />.
    /// </summary>
    public static class ConfigUtils
    {
        /// <summary>
        ///     Backups all current configuration files to a zip file.
        /// </summary>
        /// <param name="config">Backups from this <see cref="Config" /> instance.</param>
        /// <param name="zipFilePath">Zip file that will save backup data.</param>
        public static Task BackupToZipAsync(this Config config, string zipFilePath)
        {
            return Task.Run(() =>
            {
                var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                config.SaveAsync().Wait();
                BackupToDirectory(config, tempDir);
                if (File.Exists(zipFilePath))
                    File.Delete(zipFilePath);
                ZipFile.CreateFromDirectory(tempDir, zipFilePath, CompressionLevel.Optimal, false, Encoding.UTF8);
                Directory.Delete(tempDir, true);
            });
        }

        private static void BackupToDirectory(Config config, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            var entries = config.Settings.StoreService.GetEntriesAsync(
                config.Settings.ConfigDir,
                config.Settings.ConfigFileExtension).Result;
            var storeService = new PlainTextStoreService();
            foreach (var entry in entries)
            {
                var configFileAsString = config.Settings.StoreService.LoadAsync(entry).Result;
                var configFile = config.Settings.ConfigFileAdapter.ToConfigFile(configFileAsString);
                var configFileName =
                    config.Settings.ConfigFileNameFactory.Create(configFile, config.Settings.ConfigFileExtension);
                storeService.SaveAsync(Path.Combine(directoryPath, configFileName), configFileAsString).Wait();
            }
        }

        /// <summary>
        ///     Restores all configurations from zip file to <see cref="ConfigDataManagerSettings.ConfigDir" />.
        /// </summary>
        /// <param name="config">Restores to this <see cref="Config" /> instance.</param>
        /// <param name="zipFilePath">Zip file that contains backup data.</param>
        public static Task RestoreFromZipAsync(this Config config, string zipFilePath)
        {
            return Task.Run(() =>
            {
                if (!Directory.Exists(config.Settings.ConfigDir))
                    Directory.CreateDirectory(config.Settings.ConfigDir);
                config.SaveAsync().Wait();
                ZipFile.ExtractToDirectory(zipFilePath, config.Settings.ConfigDir, Encoding.UTF8);
                config.LoadAsync().Wait();
            });
        }
    }
}