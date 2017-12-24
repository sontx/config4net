namespace Config4Net.Core
{
    /// <summary>
    /// The <see cref="Settings"/> factory.
    /// </summary>
    public interface ISettingsFactory
    {
        /// <summary>
        /// Create new <see cref="Settings"/> instance.
        /// </summary>
        /// <returns></returns>
        Settings Create();
    }
}