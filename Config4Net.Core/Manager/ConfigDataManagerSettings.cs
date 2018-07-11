using Config4Net.Core.Adapter;
using Config4Net.Core.StoreService;

namespace Config4Net.Core.Manager
{
    /// <summary>
    ///     Settings for <see cref="IConfigDataManager" />.
    /// </summary>
    public class ConfigDataManagerSettings
    {
        /// <summary>
        ///     Gets or sets <see cref="IConfigFileFactory" />.
        /// </summary>
        public IConfigFileFactory ConfigFileFactory { get; set; }

        /// <summary>
        ///     Gets or sets <see cref="IConfigFileAdapter" />.
        /// </summary>
        public IConfigFileAdapter ConfigFileAdapter { get; set; }

        /// <summary>
        ///     Gets or sets <see cref="IStoreService" />
        /// </summary>
        public IStoreService StoreService { get; set; }

        /// <summary>
        ///     Gets or sets <see cref="IConfigFileNameFactory" />.
        /// </summary>
        public IConfigFileNameFactory ConfigFileNameFactory { get; set; }

        /// <summary>
        ///     Gets or sets config directory that saved config files.
        /// </summary>
        public string ConfigDir { set; get; }

        /// <summary>
        ///     Gets or sets config file extension.
        /// </summary>
        public string ConfigFileExtension { get; set; }

        /// <summary>
        ///     Ignore loading config file that is corrupted, otherwise an exception will be thrown.
        /// </summary>
        public bool IgnoreLoadingFailure { get; set; }

        /// <summary>
        ///     The library will create new instance for null properties to prevent null reference.
        /// </summary>
        public bool PreventNullReference { get; set; }
    }
}