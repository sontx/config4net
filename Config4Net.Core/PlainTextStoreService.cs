using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Config4Net.Core
{
    public sealed class PlainTextStoreService : IStoreService
    {
        public Task SaveAsync(string filePath, string content)
        {
            return Task.Run(() =>
            {
                File.WriteAllText(filePath, content, Encoding.UTF8);
            });
        }

        public Task<string> LoadAsync(string filePath)
        {
            return Task.Run(() => File.ReadAllText(filePath, Encoding.UTF8));
        }
    }
}