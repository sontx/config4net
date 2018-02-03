using Config4Net.Core.StoreService;
using System.IO;
using System.Threading.Tasks;

namespace Config4Net.Tests.Mock
{
    internal class MockStoreService : IStoreService
    {
        private readonly string _fileHeader;
        private readonly string _validContent;

        public MockStoreService(string fileHeader, string validContent)
        {
            _fileHeader = fileHeader;
            _validContent = validContent;
        }

        public Task<string[]> GetEntriesAsync(string configDir, string fileExtension)
        {
            return Task.Run(() => new[] { "file1" });
        }

        public Task SaveAsync(string filePath, string content)
        {
            return Task.Run(() =>
            {
                File.WriteAllText(filePath, $@"{_fileHeader}{content}");
            });
        }

        public Task<string> LoadAsync(string filePath)
        {
            return Task.Run(() => _validContent);
        }
    }
}