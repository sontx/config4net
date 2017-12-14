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
    /// only one app config but there are many module configs.
    /// </para>
    /// <para>
    /// A config is a class that is annotated by a <see cref="ConfigAttribute"/>. Example:
    /// <code>
    /// [Config]
    /// class MyConfig
    /// {}
    /// </code>
    /// The config is identified by key, see <seealso cref="ConfigAttribute"/> for more detail.
    /// </para>
    /// </summary>
    public sealed class ConfigPool
    {
        public static ConfigPool Default { get; } = new ConfigPool();

        private readonly Dictionary<string, ConfigWrapper> _configMap;
        private readonly List<IConfigObjectFactory> _configObjectFactoryList;
        private IApplicationClosingEvent _applicationClosingEvent;
        private volatile bool _loaded;
        private JsonSerializerSettings _jsonSerializerSettings;

        /// <summary>
        /// Auto register type whenever there have a request configuration data from an unkown type.
        /// </summary>
        public bool AutoRegisterConfigType { get; set; } = true;

        /// <summary>
        /// Auto save configuration data to files when application is closing.
        /// Configuration will be saved into <see cref="ConfigDir"/> automatically.
        /// </summary>
        public bool AutoSaveWhenApplicationClosing { get; set; } = true;

        /// <summary>
        /// Ignore mismatch type, the property that is mismatch type will be assigned
        /// default value.
        /// </summary>
        public bool IgnoreMismatchType { get; set; } = true;

        /// <summary>
        /// The directory that will be held configuration files. If it's null or empty,
        /// library will use current directory instead.
        /// </summary>
        public string ConfigDir { get; set; }

        public ConfigPool()
        {
            _configMap = new Dictionary<string, ConfigWrapper>();
            _configObjectFactoryList = new List<IConfigObjectFactory> {new DefaultConfigObjectFactory()};
            _loaded = false;

            SetApplicationClosingEvent(new DefaultApplicationClosingEvent());
            HandleJsonError();
        }

        public void SetApplicationClosingEvent(IApplicationClosingEvent applicationClosingEvent)
        {
            if (_applicationClosingEvent != null)
            {
                _applicationClosingEvent.AppClosing -= OnApplicationClosing;
                _applicationClosingEvent.Unregister();
            }

            _applicationClosingEvent = applicationClosingEvent;

            if (_applicationClosingEvent != null)
            {
                _applicationClosingEvent.AppClosing += OnApplicationClosing;
                _applicationClosingEvent.Register();
            }
        }

        /// <summary>
        /// Load configuration files from <see cref="ConfigDir"/>.
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
        /// is specified by <see cref="ConfigDir"/>.
        /// </summary>
        public Task SaveAsync()
        {
            return SaveAsync(null, true);
        }

        /// <summary>
        /// Save configuration data to files with a specify config directory.
        /// </summary>
        /// <param name="configDir">
        /// If it's null or empty, the library will
        /// use <see cref="ConfigDir"/> instead otherwise use this param.
        /// </param>
        public Task SaveAsync(string configDir)
        {
            return SaveAsync(configDir, true);
        }

        /// <summary>
        /// Save configuration data to files, the directory that will contain these files
        /// is specified by <see cref="ConfigDir"/>.
        /// </summary>
        /// <param name="isPersistent">
        /// If it's true, the library will make sure that all
        /// configuration data will be saved (ex: the other instance of this library and current
        /// instance are reading/writing a same configuration file, so it's make sure that configuration
        /// data will be saved after some tries) otherwise the library just save and doesn't care
        /// about the saving is successful or not.
        /// </param>
        public Task SaveAsync(bool isPersistent)
        {
            return SaveAsync(null, true);
        }

        /// <summary>
        /// Save configuration data to files with a specify config directory.
        /// </summary>
        /// <param name="configDir">
        /// If it's null or empty, the library will
        /// use <see cref="ConfigDir"/> instead otherwise use this param.
        /// </param>
        /// <param name="isPersistent">
        /// If it's true, the library will make sure that all
        /// configuration data will be saved (ex: the other instance of this library and current
        /// instance are reading/writing a same configuration file, so it's make sure that configuration
        /// data will be saved after some tries) otherwise the library just save and doesn't care
        /// about the saving is successful or not.
        /// </param>
        public Task SaveAsync(string configDir, bool isPersistent)
        {
            return Task.Run(() => { Save(configDir, isPersistent); });
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
            return Get<T>(Constants.ApplicationConfigKey);
        }

        /// <summary>
        /// Get config by specify type.
        /// </summary>
        /// <typeparam name="T">
        /// Config type that is annotated by a <see cref="ConfigAttribute"/>.
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
        /// Config type that is annotated by a <see cref="ConfigAttribute"/>.
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
        /// Config type that is annotated by a <see cref="ConfigAttribute"/>.
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
        /// Config type that is annotated by a <see cref="ConfigAttribute"/>.
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
        /// Config type that is annotated by a <see cref="ConfigAttribute"/>.
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
        /// The library will detect register information automatically by the <see cref="ConfigAttribute"/>
        /// that is annotated to register type. If the key in <see cref="ConfigAttribute.Key"/>
        /// is null or empty then the library will use the assembly name that contains this register type instead.
        /// There is an option is <see cref="ConfigAttribute.IsAppConfig"/>, if it's true then the library will
        /// ignore the <see cref="ConfigAttribute.Key"/> and use the default app key for this config type
        /// that defined by <see cref="Constants.ApplicationConfigKey"/>.
        /// </para>
        /// </summary>
        /// <param name="instance">
        /// The default instance insteads of creating new one.
        /// </param>
        /// <typeparam name="T">
        /// Config type that is annotated by a <see cref="ConfigAttribute"/>.
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
                    var obj = _configMap[key].ConfigObject;
                    return obj is T variable ? variable : null;
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
                _configMap.Add(key, new ConfigWrapper
                {
                    ConfigObject = configObject,
                    TypeIdentify = type.AssemblyQualifiedName
                });
            }

            return configObject;
        }

        /// <summary>
        /// Unregister config type that was registered by <see cref="RegisterConfigType{T}(T)"/> method.
        /// It's useless except for creating test.
        /// </summary>
        /// <typeparam name="T">
        /// Config type that is annotated by a <see cref="ConfigAttribute"/>.
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

            return AutoRegisterConfigType ? RegisterConfigType<T>() : null;
        }

        private string EnsureConfigDir(string configDir = null)
        {
            configDir = string.IsNullOrEmpty(configDir)
                ? (string.IsNullOrEmpty(ConfigDir) ? Environment.CurrentDirectory : ConfigDir)
                : configDir;

            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
            }

            return configDir;
        }

        private void Load(string configDir)
        {
            configDir = EnsureConfigDir(configDir);
            var configFiles = Directory.GetFiles(configDir, $@"*.{Constants.ConfigFileExtention}");

            lock (_configMap)
            {
                _configMap.Clear();

                foreach (var configFile in configFiles)
                {
                    var configWrapper = LoadFromFile(configFile);
                    if (configWrapper == null) continue;
                    var configKey = GetKeyFromFile(configFile);
                    _configMap.Add(configKey, configWrapper);
                }
            }

            _loaded = true;
        }

        private ConfigWrapper LoadFromFile(string configFile)
        {
            var json = File.ReadAllText(configFile);

            if (string.IsNullOrEmpty(json)) return null;

            var jsonObject = JObject.Parse(json);
            var typeIdentify = (string) jsonObject.GetValue(nameof(ConfigWrapper.TypeIdentify));
            var type = Type.GetType(typeIdentify, true);
            var serializer = _jsonSerializerSettings != null
                ? JsonSerializer.CreateDefault(_jsonSerializerSettings)
                : JsonSerializer.CreateDefault();
            var configObject = jsonObject
                .GetValue(nameof(ConfigWrapper.ConfigObject))
                .ToObject(type, serializer);

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

            var extentionIndex = file.LastIndexOf(Constants.ConfigFileExtention, StringComparison.Ordinal);

            return extentionIndex > 0 ? file.Substring(0, extentionIndex - 1) : file;
        }

        private string GetConfigKey(Type type)
        {
            if (!type.IsDefined(typeof(ConfigAttribute), false))
                throw new InvalidConfigTypeException($@"Config class must be defined with {nameof(ConfigAttribute)}.");

            var configAttribute = type.GetCustomAttribute<ConfigAttribute>();
            if (configAttribute.IsAppConfig)
                return Constants.ApplicationConfigKey;

            return string.IsNullOrEmpty(configAttribute.Key)
                ? Path.GetFileNameWithoutExtension(type.Assembly.Location)
                : configAttribute.Key;
        }

        private void Save(string configDir, bool isPersistent)
        {
            configDir = EnsureConfigDir(configDir);
            lock (_configMap)
            {
                foreach (var configWrapper in _configMap)
                {
                    var configKey = configWrapper.Key;
                    var configFileName = $@"{configKey}.{Constants.ConfigFileExtention}";
                    var configFile = Path.Combine(configDir, configFileName);
                    var jsonObject = JObject.FromObject(configWrapper.Value);

                    SaveToFile(isPersistent, configFile, jsonObject);
                }
            }
        }

        private void SaveToFile(bool isPersistent, string configFile, JObject jsonObject)
        {
            FileWriter fileWriter;

            if (isPersistent)
                fileWriter = new FileWriter
                {
                    Timeout = Constants.WriteFileTimeoutInMilliseconds,
                    FilePath = configFile,
                    ThrowIfFail = true,
                    Content = jsonObject.ToString()
                };
            else
                fileWriter = new FileWriter
                {
                    Timeout = 0,
                    FilePath = configFile,
                    ThrowIfFail = false,
                    Content = jsonObject.ToString()
                };

            fileWriter.SaveAsync().Wait();
        }

        private void OnApplicationClosing(object sender, EventArgs e)
        {
            if (!AutoSaveWhenApplicationClosing) return;

            var configDir = EnsureConfigDir();
            SaveAsync(configDir).Wait(5000);
        }

        private void HandleJsonError()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                Error = (sender, args) =>
                {
                    args.ErrorContext.Handled = IgnoreMismatchType;
                }
            };
        }
    }
}