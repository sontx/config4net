namespace Config4Net.Core.Manager
{
    /// <summary>
    /// Produces a config data from a giving type.
    /// It can be used when you want to setup some initial data for the config before
    /// give it to <see cref="Config"/> for example set some default values.
    /// </summary>
    public interface IConfigDataFactory
    {
        /// <summary>
        /// Create a config data object.
        /// </summary>
        object Create();
    }
}