namespace Config4Net.Core.Adapter
{
    /// <summary>
    ///     Converts from <see cref="IConfigFile" /> to string and vice versa.
    /// </summary>
    public interface IConfigFileAdapter
    {
        /// <summary>
        ///     Converts from <see cref="IConfigFile" /> to string that can save.
        /// </summary>
        string ToString(IConfigFile configFile);

        /// <summary>
        ///     Converts from saved config to <see cref="IConfigFile" />.
        /// </summary>
        IConfigFile ToConfigFile(string configFileAsString);
    }
}