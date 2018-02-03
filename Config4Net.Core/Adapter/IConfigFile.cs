namespace Config4Net.Core.Adapter
{
    /// <summary>
    /// Represents a savable object that uses to save a config data.
    /// </summary>
    public interface IConfigFile
    {
        /// <summary>
        /// Represents extra info for saved config.
        /// </summary>
        IMetadata Metadata { get; set; }

        /// <summary>
        /// Represents for the real config data.
        /// </summary>
        object ConfigData { get; set; }
    }
}