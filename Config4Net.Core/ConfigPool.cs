using Config4Net.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Config4Net.Core
{
    /// <summary>
    /// Provides a pool of configs that can be used to contain settings, share them among modules
    /// or save them to files and load them later.
    /// <para>
    /// There are two kinds of config are app config and the other. Each instance of application just has
    /// only an app config but there are many module configs.
    /// </para>
    /// <para>
    /// A config is a normal class with public properties, it can be annotated by a <see cref="ConfigAttribute"/>. Example:
    /// <code>
    /// [Config]
    /// class MyConfig
    /// {}
    /// </code>
    /// The config is identified by key, see <see cref="ConfigAttribute"/> for more detail.
    /// </para>
    /// </summary>
    public sealed class ConfigPool
    {
        /// <summary>
        /// Default <see cref="ConfigPool"/> instance that uses default <see cref="Settings"/> from <see cref="DefaultSettingsFactory"/>.
        /// </summary>
        public static ConfigPool Default { get; } = Create();

        /// <summary>
        /// Create an <see cref="ConfigPool"/> instance that uses default <see cref="Settings"/> from <see cref="DefaultSettingsFactory"/>.
        /// </summary>
        /// <returns>New <see cref="ConfigPool"/> instance.</returns>
        public static ConfigPool Create()
        {
            return new ConfigPool(new DefaultSettingsFactory().Create());
        }

        /// <summary>
        /// Create an <see cref="ConfigPool"/> that uses a specific <see cref="Core.Settings"/>.
        /// </summary>
        /// <param name="settings"><see cref="Settings"/> that will be assigned to new <see cref="ConfigPool"/> instance.</param>
        /// <returns>New <see cref="ConfigPool"/> instance.</returns>
        public static ConfigPool Create(Settings settings)
        {
            return new ConfigPool(settings);
        }

        private readonly Dictionary<string, ConfigWrapper> _configMap;
        private readonly List<IConfigObjectFactory> _configObjectFactoryList;
        private volatile bool _loaded;
        private JsonSerializerSettings _jsonSerializerSettings;
        private Settings _settings;

        /// <summary>
        /// <see cref="ConfigPool"/> settings, it can not be null.
        /// <seealso cref="Core.Settings"/>
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
            }
        }

        private ConfigPool(Settings settings)
        {
            Settings = settings;

            _configMap = new Dictionary<string, ConfigWrapper>();
            _configObjectFactoryList = new List<IConfigObjectFactory> {new DefaultConfigObjectFactory()};
            _loaded = false;

            HandleJsonError();
        }

        /// <summary>
        /// Load configuration files from <see cref="Core.Settings.ConfigDir"/>"/>.
        /// </summary>
        public Task LoadAsync()
        {
            return LoadAsync(null);
        }

        /// <summary>
        /// Load configuration files from specify config directory.
        /// </summary>
        public Task LoadAsync(string configDir)
        {
            return Task.Run(() =>
            {
                _loaded = false;
                Load(configDir);
            });
        }

        /// <summary>
        /// Save configuration data to files, the directory that will contain these files
        /// is specified by <see cref="Core.Settings.ConfigDir"/>.
        /// </summary>
        public Task SaveAsync()
        {
            return SaveAsync(null);
        }

        /// <summary>
        /// Save configuration data to files with a specify config directory.
        /// </summary>
        /// <param name="configDir">
        /// If it's null or empty, the library will
        /// use <see cref="Core.Settings.ConfigDir"/> instead otherwise use this param.
        /// </param>
        public Task SaveAsync(string configDir)
        {
            return Task.Run(() => { SaveToDir(configDir); });
        }

        /// <summary>
        /// Get app config. Each instance of this library just hold only one app config, it means
        /// each application just has an app config data.
        /// </summary>
        /// <typeparam name="T">
        /// App config type.
        /// </typeparam>
        /// <returns></returns>
        public T App<T>() where T : class
        {
            return Get<T>(Settings.AppConfigKey);
        }

        /// <summary>
        /// Get config by specify type.
        /// </summary>
        /// <typeparam name="T">
        /// Config type that could be annotated by a <see cref="ConfigAttribute"/>.
        /// </typeparam>
        public T Get<T>() where T : class
        {
            var key = GetConfigKey(typeof(T));
            return Get<T>(key);
        }

        /// <summary>
        /// Get config by specify key.
        /// </summary>
        /// <typeparam name="T">
        /// Config type that could be annotated by a <see cref="ConfigAttribute"/>.
        /// </typeparam>
        /// <param name="key">
        /// Config key.
        /// </param>
        public T Get<T>(string key) where T : class
        {
            return GetConfig<T>(key);
        }

        /// <summary>
        /// Get config for the calling assembly. The calling assembly can be a library
        /// or an executing.
        /// </summary>
        /// <typeparam name="T">
        /// Config type that could be annotated by a <see cref="ConfigAttribute"/>.
        /// </typeparam>
        public T Calling<T>() where T : class
        {
            return QualifiedByAssembly<T>(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Get config for the entry assembly. The entry assembly is the executable file
        /// that is running the main method insides.
        /// </summary>
        /// <typeparam name="T">
        /// Config type that could be annotated by a <see cref="ConfigAttribute"/>.
        /// </typeparam>
        public T Entry<T>() where T : class
        {
            return QualifiedByAssembly<T>(Assembly.GetEntryAssembly());
        }

        /// <summary>
        /// Register a factory that will be used when create new instance for a config type.
        /// </summary>
        public void RegisterFactory(IConfigObjectFactory factory)
        {
            EnsureConfigLoaded();

            lock (_configObjectFactoryList)
            {
                _configObjectFactoryList.Add(factory);
            }
        }

        /// <summary>
        /// Unregister a factory that was registered by <see cref="RegisterFactory"/> method.
        /// </summary>
        public void UnregisterFactory(IConfigObjectFactory factory)
        {
            lock (_configObjectFactoryList)
            {
                _configObjectFactoryList.Remove(factory);
            }
        }

        /// <summary>
        /// Register a config type to the library. It will create a new instance of this type
        /// to hold configuration data if necessary. If the config type already exists then
        /// it just ignores and return the existing object.
        /// </summary>
        /// <typeparam name="T">
        /// Config type that could be annotated by a <see cref="ConfigAttribute"/>.
        /// </typeparam>
        public T RegisterConfigType<T>() where T : class
        {
            return RegisterConfigType<T>(null);
        }

        /// <summary>
        /// Register a config type to the library. It will create a new instance of this type
        /// to hold configuration data if necessary. If the config type already exists then
        /// it just ignores and return the existing object.
        /// <para>
        /// The library will detect register information automatically by assembly name or the <see cref="ConfigAttribute"/>
        /// that is annotated to register type. If the key in <see cref="ConfigAttribute.Key"/>
        /// is null or empty then the library will use the assembly name that contains this register type instead.
        /// There is an option is <see cref="ConfigAttribute.IsAppConfig"/>, if it's true then the library will
        /// ignore the <see cref="ConfigAttribute.Key"/> and use the default app key for this config type
        /// that defined by <see cref="Core.Settings.AppConfigKey"/>.
        /// </para>
        /// </summary>
        /// <param name="instance">
        /// The default instance insteads of creating new one.
        /// </param>
        /// <typeparam name="T">
        /// Config type that could be annotated by a <see cref="ConfigAttribute"/>.
        /// </typeparam>
        public T RegisterConfigType<T>(T instance) where T : class
        {
            EnsureConfigLoaded();

            var type = typeof(T);
            var key = GetConfigKey(type);
            var configObject = instance;

            lock (_configMap)
            {
                if (_configMap.ContainsKey(key))
                {
                    var wrapper = _configMap[key];
                    if (wrapper.TypeIdentify != type.AssemblyQualifiedName)
                        throw new ArgumentException("Same key but difference type.");
                    return wrapper.ConfigObject is T variable ? variable : null;
                }
            }

            if (configObject == null)
            {
                lock (_configObjectFactoryList)
                {
                    for (var i = _configObjectFactoryList.Count - 1; i >= 0; i--)
                    {
                        var configObjectFactory = _configObjectFactoryList[i];
                        configObject = (T) configObjectFactory.CreateDefault(type);
                        if (configObject != null) break;
                    }
                }

                if (configObject == null) return null;
            }

            lock (_configMap)
            {
                _configMap.Add(key, CreateConfigWrapper(type.AssemblyQualifiedName, configObject));
            }

            return configObject;
        }

        /// <summary>
        /// Unregister config type that was registered by <see cref="RegisterConfigType{T}(T)"/> method.
        /// It's useless except for creating test.
        /// </summary>
        /// <typeparam name="T">
        /// Config type that could be annotated by a <see cref="ConfigAttribute"/>.
        /// </typeparam>
        public void UnregisterConfigType<T>() where T : class
        {
            EnsureConfigLoaded();

            var type = typeof(T);
            var key = GetConfigKey(type);

            lock (_configMap)
            {
                if (_configMap.ContainsKey(key))
                {
                    _configMap.Remove(key);
                }
            }
        }

        private T QualifiedByAssembly<T>(Assembly assembly) where T : class
        {
            var fileLocation = assembly.Location;
            var key = GetKeyFromFile(fileLocation);

            return GetConfig<T>(key);
        }

        private T GetConfig<T>(string key) where T : class
        {
            EnsureConfigLoaded();

            lock (_configMap)
            {
                if (_configMap.TryGetValue(key, out var configWrapper))
                    return (T) configWrapper.ConfigObject;
            }

            return Settings.AutoRegisterConfigType ? RegisterConfigType<T>() : null;
        }

        private string EnsureConfigDir(string configDir = null)
        {
            configDir = string.IsNullOrEmpty(configDir)
                ? (string.IsNullOrEmpty(Settings.ConfigDir) ? Environment.CurrentDirectory : Settings.ConfigDir)
                : configDir;

            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
            }

            return configDir;
        }

        private void Load(string configDir)
        {
            Precondition.PropertyNotNull(Settings.StoreService, nameof(Settings.StoreService));
            var storeService = Settings.StoreService;
            configDir = EnsureConfigDir(configDir);
            var configFiles = storeService.GetEntriesAsync(configDir, Settings.ConfigFileExtension).Result;

            lock (_configMap)
            {
                _configMap.Clear();

                foreach (var configFile in configFiles)
                {
                    try
                    {
                        var configWrapper = LoadFromFile(configFile);
                        if (configWrapper == null) continue;
                        var configKey = GetKeyFromFile(configFile);
                        _configMap.Add(configKey, configWrapper);
                    }
                    catch (Exception ex) when (ex is IOException || ex is TypeLoadException)
                    {
                        if (!Settings.IgnoreLoadFailure)
                            throw;
                    }
                }
            }

            _loaded = true;
        }

        private ConfigWrapper LoadFromFile(string configFile)
        {
            Precondition.PropertyNotNull(Settings.StoreService, nameof(Settings.StoreService));

            var json = Settings.StoreService.LoadAsync(configFile).Result;

            if (string.IsNullOrWhiteSpace(json)) return null;

            var jsonObject = JObject.Parse(json);
            var typeIdentify = (string) jsonObject.GetValue(nameof(ConfigWrapper.TypeIdentify));
            var type = Type.GetType(typeIdentify, true);
            var serializer = _jsonSerializerSettings != null
                ? JsonSerializer.CreateDefault(_jsonSerializerSettings)
                : JsonSerializer.CreateDefault();
            var configObject = jsonObject
                .GetValue(nameof(ConfigWrapper.ConfigObject))
                .ToObject(type, serializer);

            return CreateConfigWrapper(typeIdentify, configObject);
        }

        private ConfigWrapper CreateConfigWrapper(string typeIdentify, object configObject)
        {
            ObjectUtils.FillNullProperties(configObject, propertyInfo =>
            {
                var defaultAttribute = propertyInfo.GetCustomAttribute<DefaultAttribute>();
                if (defaultAttribute != null) return defaultAttribute.DefaultValue;

                if (Settings.PreventNullReference)
                {
                    return propertyInfo.PropertyType == typeof(string)
                        ? string.Empty
                        : ObjectUtils.CreateDefaultInstance(propertyInfo.PropertyType);
                }

                return null;
            });

            return new ConfigWrapper
            {
                TypeIdentify = typeIdentify,
                ConfigObject = configObject
            };
        }

        private void EnsureConfigLoaded()
        {
            if (!_loaded)
                Load(null);
        }

        private string GetKeyFromFile(string file)
        {
            if (string.IsNullOrEmpty(file)) return null;

            file = Path.GetFileName(file);

            var extentionIndex = file.LastIndexOf(Settings.ConfigFileExtension, StringComparison.Ordinal);

            return extentionIndex > 0 ? file.Substring(0, extentionIndex - 1) : file;
        }

        private string GetConfigKey(Type type)
        {
            var configAttribute = type.GetCustomAttribute<ConfigAttribute>();

            if (configAttribute != null)
            {
                if (configAttribute.IsAppConfig)
                    return Settings.AppConfigKey;

                if (!string.IsNullOrEmpty(configAttribute.Key))
                {
                    if (configAttribute.Key == Settings.AppConfigKey)
                        throw new ArgumentException("A module config key must be difference to app key." +
                                                    "If you intent to use it as the app config, you can set IsAppConfig property to true.");
                    return configAttribute.Key;
                }
            }

            var moduleKey = Path.GetFileNameWithoutExtension(type.Assembly.Location);
            if (moduleKey == Settings.AppConfigKey)
                throw new ArgumentException("A module config key must be difference to app key." +
                                            "You should change your app config key in the Settings.");
            return moduleKey;
        }

        private void SaveToDir(string configDir)
        {
            Precondition.PropertyNotNull(Settings.StoreService, nameof(Settings.StoreService));

            configDir = EnsureConfigDir(configDir);
            lock (_configMap)
            {
                var storeService = Settings.StoreService;
                foreach (var configWrapper in _configMap)
                {
                    var configKey = configWrapper.Key;
                    var configFileName = $@"{configKey}.{Settings.ConfigFileExtension}";
                    var configFile = Path.Combine(configDir, configFileName);
                    var jsonObject = JObject.FromObject(configWrapper.Value);
                    storeService.SaveAsync(configFile, jsonObject.ToString()).Wait();
                }
            }
        }

        private void OnApplicationClosing()
        {
            if (!Settings.AutoSaveWhenApplicationClosing) return;

            var configDir = EnsureConfigDir();
            SaveAsync(configDir).Wait(5000);
        }

        private void HandleJsonError()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                Error = (sender, args) => { args.ErrorContext.Handled = Settings.IgnoreMismatchType; }
            };
        }
    }
}