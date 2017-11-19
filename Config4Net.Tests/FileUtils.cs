using System.IO;

namespace Config4Net.Tests
{
    internal static class FileUtils
    {
        public static void EnsureDelete(string fileName)
        {
            if (!File.Exists(fileName)) return;

            do
            {
                try
                {
                    File.Delete(fileName);
                    break;
                }
                catch
                {
                    // ignored
                }
            } while (true);
        }
    }
}