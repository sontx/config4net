using System.IO;
using System.Threading.Tasks;

namespace Config4Net.Core
{
    public abstract class FileStoreService : IStoreService
    {
        public Task<string[]> GetEntriesAsync(string configDir, string searchPattern)
        {
            return Task.Run(() => Directory.GetFiles(configDir, searchPattern));
        }

        public abstract Task SaveAsync(string filePath, string content);

        public abstract Task<string> LoadAsync(string filePath);
    }
}