using Config4Net.Core.Adapter;
using Config4Net.Core.Manager;
using Config4Net.Core.StoreService;
using Config4Net.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Config4Net.Core
{
    /// <summary>
    /// Manages configurations.
    /// </summary>
    public sealed class Config
    {
        private readonly IConfigDataManager _configDataManager;
        private readonly IConfigDataFactoryManager _configDataFactoryManager;
        private volatile bool _loaded;
        private Settings _settings;

        private Config()
        {
            _configDataManager = new ConfigDataManagerImpl();
            _configDataFactoryManager = new ConfigDataFactoryManagerImpl();
            InitializeSettings();
        }

        /// <summary>
        /// Default instance of <see cref="Config"/>.
        /// </summary>
        public static Config Default { get; } = Create();

        /// <summary>
        /// Settings for <see cref="Config"/>. Can not be null.
        /// </summary>
        public Settings Settings
        {
            get => _settings;
            set
            {
                Precondition.ArgumentNotNull(value, nameof(Settings));
                _settings?.Release();
                _settings = value;
                _settings.SetOnApplicationClosing(OnApplicationClosing);
                _configDataManager.Settings = value;
            }
        }

        /// <summary>
        /// Creates new <see cref="Config"/> instance.
        /// </summary>
        /// <returns></returns>
        public static Config Create() { return new Config(); }

        /// <summary>
        /// Loads configuration files to memory. Normally there is no need to call this method
        /// as the library will load files automatically when needed.
        /// </summary>
        /// <remarks>
        /// Configuration files must be placed in a specific directory that already set at
        /// <see cref="ConfigDataManagerSettings.ConfigDir"/>.
        /// </remarks>
        public Task LoadAsync()
        {
            return Task.Run(() => Load());
        }

        private void Load()
        {
            lock (this)
            {
                _configDataManager.Load();
                _loaded = true;
            }
        }

        /// <summary>
        /// Saves configuration data to files. Normally there is no need to call this method
        /// as the library will save to files automatically when needed (ex: app is closing).
        /// </summary>
        /// <remarks>
        /// Configuration files will be saved in a specific directory that already set at
        /// <see cref="ConfigDataManagerSettings.ConfigDir"/>.
        /// </remarks>
        public Task SaveAsync()
        {
            return Task.Run(() => Save());
        }

        private void Save()
        {
            _configDataManager.Save();
        }

        /// <summary>
        /// Gets a configuration by a specific type.
        /// </summary>
        /// <typeparam name="T">
        /// Configuration type that will be returned.
        /// </typeparam>
        /// <remarks>
        /// If the specific configuration does not exist, then the library will register a new one if
        /// the <see cref="Core.Settings.AutoRegisterConfigType"/> is true, otherwise an exception will
        /// be thrown.
        /// </remarks>
        public T Get<T>()
        {
            return Get<T>(null);
        }

        /// <summary>
        /// Gets a configuration by a specific config key.
        /// </summary>
        /// <typeparam name="T">
        /// The library will cast the configuration to this type.
        /// </typeparam>
        /// <exception cref="ConfigException">
        /// Occurs when the specific key does not exist.
        /// </exception>
        /// <remarks>
        /// If the specific configuration does not exist, then the library will register a new one if
        /// the <see cref="Core.Settings.AutoRegisterConfigType"/> is true, otherwise an exception will
        /// be thrown.
        /// If the specific key is null or empty, the libary will use a default key depends on
        /// the giving config type.
        /// </remarks>
        public T Get<T>(string key)
        {
            EnsureLoaded();
            var configKey = string.IsNullOrEmpty(key) ? GetConfigKeyByType(typeof(T)) : key;
            lock (this)
            {
                if (_configDataManager.Has(configKey))
                    return (T)_configDataManager.Get(configKey);
                if (!Settings.AutoRegisterConfigType)
                    throw new ConfigException($"Config key {configKey} does not exist");
                return RegisterWithResult<T>(configKey);
            }
        }

        private void EnsureLoaded()
        {
            if (!_loaded)
                Load();
        }

        private string GetConfigKeyByType(Type type)
        {
            var configAttribute = type.GetCustomAttribute<ConfigAttribute>();
            if (!string.IsNullOrEmpty(configAttribute?.Key))
                return configAttribute.Key;
            if (Settings.PreferAppNameAsKey)
            {
                if (string.IsNullOrEmpty(Settings.AppName))
                {
                    var assembly = Assembly.GetEntryAssembly() ?? type.Assembly;
                    return Path.GetFileNameWithoutExtension(assembly.Location);
                }
                return Settings.AppName;
            }
                
            return type.Name;
        }

        private object GetConfigDataByType(Type type)
        {
            var configDataFactory = _configDataFactoryManager.Get(type, Settings.PreventNullReference);
            return configDataFactory.Create();
        }

        private T RegisterWithResult<T>(string configKey)
        {
            var configData = GetConfigDataByType(typeof(T));
            _configDataManager.Add(configKey, configData);
            return (T)configData;
        }

        /// <summary>
        /// Registers a configuration. Normally there is no need to call this method as the libary
        /// will register the configuration automatically when <see cref="Core.Settings.AutoRegisterConfigType"/>
        /// is true.
        /// </summary>
        /// <remarks>
        /// Configuration data will be created automatically, you can change how the library creates configuration
        /// data by register an <see cref="IConfigDataFactory"/> for a specific configuration type.
        /// </remarks>
        public void Register<T>()
        {
            Register(typeof(T));
        }

        /// <summary>
        /// Registers a configuration. Normally there is no need to call this method as the libary
        /// will register the configuration automatically when <see cref="Core.Settings.AutoRegisterConfigType"/>
        /// is true.
        /// </summary>
        /// <param name="configData">
        /// Initial configuration data for the specific configuration type.
        /// </param>
        public void Register<T>(T configData)
        {
            Register(typeof(T), configData);
        }

        /// <summary>
        /// Registers a configuration. Normally there is no need to call this method as the libary
        /// will register the configuration automatically when <see cref="Core.Settings.AutoRegisterConfigType"/>
        /// is true.
        /// </summary>
        /// <remarks>
        /// Configuration data will be created automatically, you can change how the library creates configuration
        /// data by register an <see cref="IConfigDataFactory"/> for a specific configuration type.
        /// </remarks>
        public void Register(Type configType)
        {
            var configData = GetConfigDataByType(configType);
            Register(configType, configData);
        }

        /// <summary>
        /// Registers a configuration. Normally there is no need to call this method as the libary
        /// will register the configuration automatically when <see cref="Core.Settings.AutoRegisterConfigType"/>
        /// is true.
        /// </summary>
        /// <param name="configType">
        /// Specific configuration type to register.
        /// </param>
        /// <param name="configData">
        /// Initial configuration data for the specific configuration type.
        /// </param>
        public void Register(Type configType, object configData)
        {
            EnsureLoaded();
            var configKey = GetConfigKeyByType(configType);
            if (_configDataManager.Has(configKey))
                _configDataManager.Remove(configKey);
            _configDataManager.Add(configKey, configData);
        }

        /// <summary>
        /// Unregisters a configuration.
        /// </summary>
        public void Unregister<T>()
        {
            Unregister(typeof(T));
        }

        /// <summary>
        /// Unregisters a configuration.
        /// </summary>
        public void Unregister(Type configType)
        {
            // TODO: delete configuration file if necessary.
            var configKey = GetConfigKeyByType(configType);
            _configDataManager.Remove(configKey);
        }

        /// <summary>
        /// Registers an <see cref="IConfigDataFactory"/> that will be used to create
        /// configuration data for a specific configuration type if necessary.
        /// </summary>
        public void RegisterFactory<T>(IConfigDataFactory configDataFactory)
        {
            RegisterFactory(typeof(T), configDataFactory);
        }

        /// <summary>
        /// Registers an <see cref="IConfigDataFactory"/> that will be used to create
        /// configuration data for a specific configuration type if necessary.
        /// </summary>
        public void RegisterFactory(Type configType, IConfigDataFactory configDataFactory)
        {
            _configDataFactoryManager.Register(configType, configDataFactory);
        }

        /// <summary>
        /// Unregisters an <see cref="IConfigDataFactory"/> for specific configuration type.
        /// </summary>
        public void UnregisterFactory<T>()
        {
            UnregisterFactory(typeof(T));
        }

        /// <summary>
        /// Unregisters an <see cref="IConfigDataFactory"/> for specific configuration type.
        /// </summary>
        public void UnregisterFactory(Type configType)
        {
            _configDataFactoryManager.Unregister(configType);
        }

        private void InitializeSettings()
        {
            Settings = new Settings
            {
                StoreService = new PlainTextStoreService(),
                ApplicationClosingEvent = new DefaultApplicationClosingEvent(),
                ConfigFileAdapter = new JsonConfigFileAdapter(JsonSerializer.CreateDefault()),
                ConfigFileFactory = new DefaultConfigFileFactory(),
                ConfigFileNameFactory = new DefaultConfigFileNameFactory(),
                ConfigFileExtension = "json",
                ConfigDir = Environment.CurrentDirectory,
                PreventNullReference = false,
                WriteFileTimeout = 5000,
                AutoRegisterConfigType = true,
                SaveWhenApplicationClosing = true,
                IgnoreLoadingFailure = true,
                PreferAppNameAsKey = true
            };
        }

        private void OnApplicationClosing()
        {
            if (!Settings.SaveWhenApplicationClosing) return;
            SaveAsync().Wait(Settings.WriteFileTimeout);
        }
    }
}