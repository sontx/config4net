using System;
using System.Threading.Tasks;
using Config4Net.Core.Manager;

namespace Config4Net.Core
{
    public sealed partial class Config
    {
        /// <summary>
        ///     Loads configuration files to memory. Normally there is no need to call this method
        ///     as the library will load files automatically when needed.
        /// </summary>
        /// <remarks>
        ///     Configuration files must be placed in a specific directory that already set at
        ///     <see cref="ConfigDataManagerSettings.ConfigDir" />. All old configurations data will
        ///     be cleared and fill up with new configurations data. All config objects that retrieved from
        ///     <see cref="Get{T}()" /> will not reference to <see cref="Config" /> instance anymore
        ///     and any changes will be ignored. So you should call <see cref="Get{T}()" /> again
        ///     to retrieve the config object that up to date and available to save.
        /// </remarks>
        public Task LoadAsync()
        {
            return Task.Run(() => Load());
        }

        /// <summary>
        ///     Saves configuration data to files. Normally there is no need to call this method
        ///     as the library will save to files automatically when needed (ex: app is closing).
        /// </summary>
        /// <remarks>
        ///     Configuration files will be saved in a specific directory that already set at
        ///     <see cref="ConfigDataManagerSettings.ConfigDir" />.
        /// </remarks>
        public Task SaveAsync()
        {
            return Task.Run(() => Save());
        }

        /// <summary>
        ///     Gets a configuration by a specific type.
        /// </summary>
        /// <typeparam name="T">
        ///     Configuration type that will be returned.
        /// </typeparam>
        /// <remarks>
        ///     If the specific configuration does not exist, then the library will register a new one if
        ///     the <see cref="Core.Settings.AutoRegisterConfigType" /> is true, otherwise an exception will
        ///     be thrown.
        /// </remarks>
        public T Get<T>()
        {
            return Get<T>(null);
        }

        /// <summary>
        ///     Gets a configuration by a specific config key.
        /// </summary>
        /// <typeparam name="T">
        ///     The library will cast the configuration to this type.
        /// </typeparam>
        /// <exception cref="ConfigException">
        ///     Occurs when the specific key does not exist.
        /// </exception>
        /// <remarks>
        ///     If the specific configuration does not exist, then the library will register a new one if
        ///     the <see cref="Core.Settings.AutoRegisterConfigType" /> is true, otherwise an exception will
        ///     be thrown.
        ///     If the specific key is null or empty, the libary will use a default key depends on
        ///     the giving config type.
        /// </remarks>
        public T Get<T>(string key)
        {
            EnsureLoaded();
            var configKey = string.IsNullOrEmpty(key) ? GetConfigKeyByType(typeof(T)) : key;
            lock (this)
            {
                if (_configDataManager.Has(configKey))
                    return (T) _configDataManager.Get(configKey);
                if (!Settings.AutoRegisterConfigType)
                    throw new ConfigException($"Config key {configKey} does not exist");
                return RegisterWithResult<T>(configKey);
            }
        }

        /// <summary>
        ///     Registers a configuration. Normally there is no need to call this method as the libary
        ///     will register the configuration automatically when <see cref="Core.Settings.AutoRegisterConfigType" />
        ///     is true.
        /// </summary>
        /// <remarks>
        ///     Configuration data will be created automatically, you can change how the library creates configuration
        ///     data by register an <see cref="IConfigDataFactory" /> for a specific configuration type.
        /// </remarks>
        public void Register<T>()
        {
            Register(typeof(T));
        }

        /// <summary>
        ///     Registers a configuration. Normally there is no need to call this method as the libary
        ///     will register the configuration automatically when <see cref="Core.Settings.AutoRegisterConfigType" />
        ///     is true.
        /// </summary>
        /// <param name="configData">
        ///     Initial configuration data for the specific configuration type.
        /// </param>
        public void Register<T>(T configData)
        {
            Register(typeof(T), configData);
        }

        /// <summary>
        ///     Registers a configuration. Normally there is no need to call this method as the libary
        ///     will register the configuration automatically when <see cref="Core.Settings.AutoRegisterConfigType" />
        ///     is true.
        /// </summary>
        /// <remarks>
        ///     Configuration data will be created automatically, you can change how the library creates configuration
        ///     data by register an <see cref="IConfigDataFactory" /> for a specific configuration type.
        /// </remarks>
        public void Register(Type configType)
        {
            var configData = GetConfigDataByType(configType);
            Register(configType, configData);
        }

        /// <summary>
        ///     Registers a configuration. Normally there is no need to call this method as the libary
        ///     will register the configuration automatically when <see cref="Core.Settings.AutoRegisterConfigType" />
        ///     is true.
        /// </summary>
        /// <param name="configType">
        ///     Specific configuration type to register.
        /// </param>
        /// <param name="configData">
        ///     Initial configuration data for the specific configuration type.
        /// </param>
        public void Register(Type configType, object configData)
        {
            EnsureLoaded();
            Unregister(configType);
            var configKey = GetConfigKeyByType(configType);
            _configDataManager.Add(configKey, configData);
        }

        /// <summary>
        ///     Unregisters a configuration.
        /// </summary>
        public void Unregister<T>()
        {
            Unregister(typeof(T));
        }

        /// <summary>
        ///     Unregisters a configuration.
        /// </summary>
        public void Unregister(Type configType)
        {
            // TODO: delete configuration file if necessary.
            var configKey = GetConfigKeyByType(configType);
            _configDataManager.Remove(configKey);
        }

        /// <summary>
        ///     Registers an <see cref="IConfigDataFactory" /> that will be used to create
        ///     configuration data for a specific configuration type if necessary.
        /// </summary>
        public void RegisterFactory<T>(IConfigDataFactory configDataFactory)
        {
            RegisterFactory(typeof(T), configDataFactory);
        }

        /// <summary>
        ///     Registers an <see cref="IConfigDataFactory" /> that will be used to create
        ///     configuration data for a specific configuration type if necessary.
        /// </summary>
        public void RegisterFactory(Type configType, IConfigDataFactory configDataFactory)
        {
            _configDataFactoryManager.Register(configType, configDataFactory);
        }

        /// <summary>
        ///     Unregisters an <see cref="IConfigDataFactory" /> for specific configuration type.
        /// </summary>
        public void UnregisterFactory<T>()
        {
            UnregisterFactory(typeof(T));
        }

        /// <summary>
        ///     Unregisters an <see cref="IConfigDataFactory" /> for specific configuration type.
        /// </summary>
        public void UnregisterFactory(Type configType)
        {
            _configDataFactoryManager.Unregister(configType);
        }
    }
}