using Config4Net.Utils;

namespace Config4Net.Core.Adapter
{
    /// <summary>
    ///     Creates config file name.
    /// </summary>
    public interface IConfigFileNameFactory
    {
        /// <summary>
        ///     Creates config file name.
        /// </summary>
        string Create(IConfigFile configFile, string extension);
    }

    internal class DefaultConfigFileNameFactory : IConfigFileNameFactory
    {
        public string Create(IConfigFile configFile, string extension)
        {
            Precondition.ArgumentNotNull(configFile, nameof(configFile));
            return string.IsNullOrWhiteSpace(extension)
                ? configFile.Metadata.Key
                : $"{configFile.Metadata.Key}.{extension}";
        }
    }
}