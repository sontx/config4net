using System.Threading.Tasks;

namespace Config4Net.Core.StoreService
{
    /// <summary>
    ///     Manages the way to save/load config data.
    /// </summary>
    public interface IStoreService
    {
        /// <summary>
        ///     Gets all available entries from config directory. Each entry can be a file from disk
        ///     or a value name from win32 registry.
        /// </summary>
        /// <param name="configDir">Config directory path that will be scanned to get all available entries.</param>
        /// <param name="fileExtension">Config file extenstion that uses to check a file is a config file.</param>
        /// <returns>A list of entries.</returns>
        Task<string[]> GetEntriesAsync(string configDir, string fileExtension);

        /// <summary>
        ///     Saves config data to file.
        /// </summary>
        /// <param name="filePath">File path that will be saved, if the file is already existing then it will be overwrited</param>
        /// <param name="content">The config content that will be saved.</param>
        /// <returns></returns>
        Task SaveAsync(string filePath, string content);

        /// <summary>
        ///     Loads config data from file.
        /// </summary>
        /// <param name="filePath">File path that will be loaded.</param>
        /// <returns>Config content.</returns>
        Task<string> LoadAsync(string filePath);
    }
}