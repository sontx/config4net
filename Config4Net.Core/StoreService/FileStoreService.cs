using System.IO;
using System.Threading.Tasks;

namespace Config4Net.Core.StoreService
{
    /// <summary>
    /// Save and load config data from file system.
    /// </summary>
    public abstract class FileStoreService : IStoreService
    {
        /// <inheritdoc />
        public Task<string[]> GetEntriesAsync(string configDir, string fileExtension)
        {
            return Task.Run(() => Directory.GetFiles(configDir, $@"*.{fileExtension}"));
        }

        /// <inheritdoc />
        public abstract Task SaveAsync(string filePath, string content);

        /// <inheritdoc />
        public abstract Task<string> LoadAsync(string filePath);
    }
}