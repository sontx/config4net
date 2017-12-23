using System.Threading.Tasks;

namespace Config4Net.Core
{
    public interface IStoreService
    {
        Task SaveAsync(string filePath, string content);

        Task<string> LoadAsync(string filePath);
    }
}