using System.IO;
using Config4Net.Core;

namespace Config4Net.Tests
{
    public static class Utils
    {
        public static Config CreateConfig()
        {
            var config = Config.Create();
            config.Settings.ConfigDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(config.Settings.ConfigDir);
            return config;
        }

        public static void ReleaseConfig(Config config)
        {
            if (!string.IsNullOrEmpty(config.Settings.ConfigDir) && Directory.Exists(config.Settings.ConfigDir))
                Directory.Delete(config.Settings.ConfigDir, true);
        }
    }
}