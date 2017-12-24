using System.IO;
using System.Threading.Tasks;

namespace Config4Net.Core
{
    public abstract class FileStoreService : IStoreService
    {
        public Task<string[]> GetEntriesAsync(string configDir, string fileExtension)
        {
            return Task.Run(() => Directory.GetFiles(configDir, $@"*.{fileExtension}"));
        }

        public abstract Task SaveAsync(string filePath, string content);

        public abstract Task<string> LoadAsync(string filePath);
    }
}