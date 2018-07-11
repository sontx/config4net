using System;
using System.IO;
using System.Reflection;
using Config4Net.Core.Adapter;
using Config4Net.Core.Manager;
using Config4Net.Core.StoreService;
using Config4Net.Utils;
using Newtonsoft.Json;

namespace Config4Net.Core
{
    /// <summary>
    ///     Manages configurations.
    /// </summary>
    public sealed partial class Config
    {
        private readonly IConfigDataFactoryManager _configDataFactoryManager;
        private readonly IConfigDataManager _configDataManager;
        private volatile bool _loaded;
        private Settings _settings;

        private Config()
        {
            _configDataManager = new ConfigDataManagerImpl();
            _configDataFactoryManager = new ConfigDataFactoryManagerImpl();
            InitializeSettings();
        }

        /// <summary>
        ///     Default instance of <see cref="Config" />.
        /// </summary>
        public static Config Default { get; } = Create();

        /// <summary>
        ///     Settings for <see cref="Config" />. Can not be null.
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
        ///     Gets a confiuration by a specific config key.
        /// </summary>
        /// <exception cref="ConfigException">
        ///     Occurs when the config key does not exist.
        /// </exception>
        /// <remarks>
        ///     If the specific configuration does not exist, an exception will be thrown.
        /// </remarks>
        public dynamic this[string key]
        {
            get
            {
                EnsureLoaded();
                lock (this)
                {
                    if (_configDataManager.Has(key))
                        return _configDataManager.Get(key);
                    throw new ConfigException($"Config key {key} does not exist");
                }
            }
        }

        /// <summary>
        ///     Creates new <see cref="Config" /> instance.
        /// </summary>
        /// <returns></returns>
        public static Config Create()
        {
            return new Config();
        }

        private void Load()
        {
            lock (this)
            {
                _configDataManager.Load();
                _loaded = true;
            }
        }

        private void Save()
        {
            _configDataManager.Save();
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
            return (T) configData;
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
                PreferAppNameAsKey = false
            };
        }

        private void OnApplicationClosing()
        {
            if (!Settings.SaveWhenApplicationClosing) return;
            SaveAsync().Wait(Settings.WriteFileTimeout);
        }
    }
}