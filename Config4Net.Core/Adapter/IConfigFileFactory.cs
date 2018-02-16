namespace Config4Net.Core.Adapter
{
    /// <summary>
    /// Creates an <see cref="IConfigFile"/>.
    /// </summary>
    public interface IConfigFileFactory
    {
        /// <summary>
        /// Creates an <see cref="IConfigFile"/>.
        /// </summary>
        IConfigFile Create(string key, object configData);
    }
}