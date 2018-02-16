using Config4Net.Core;
using System.IO;
using System.Reflection;

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

        public static void WriteFile(string dir, string fileName, string content)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(Path.Combine(dir, fileName), content);
        }

        public static T GetFieldValueByPath<T>(object source, string path, T defaultValue = default(T))
        {
            var parts = path.Split('.');
            var tempSource = source;
            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                var tempSourceType = tempSource.GetType();
                var tempFieldInfo = tempSourceType.GetField(part, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (tempFieldInfo == null) return defaultValue;
                tempSource = tempFieldInfo.GetValue(tempSource);
                if (tempSource == null)
                    return defaultValue;
                if (i == parts.Length - 1)
                    return (T)tempSource;
            }

            return defaultValue;
        }
    }
}