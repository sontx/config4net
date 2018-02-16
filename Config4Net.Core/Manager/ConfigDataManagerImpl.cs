using Config4Net.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace Config4Net.Core.Manager
{
    internal class ConfigDataManagerImpl : IConfigDataManager
    {
        private readonly Dictionary<string, object> _configMap = new Dictionary<string, object>();

        public ConfigDataManagerSettings Settings { get; set; }

        public void Add(string configKey, object configData)
        {
            lock (_configMap)
            {
                _configMap.Add(configKey, configData);
            }
        }

        public bool Has(string configKey)
        {
            lock (_configMap)
            {
                return _configMap.ContainsKey(configKey);
            }
        }

        public object Get(string configKey)
        {
            lock (_configMap)
            {
                return _configMap[configKey];
            }
        }

        public void Remove(string configKey)
        {
            lock (_configMap)
            {
                _configMap.Remove(configKey);
            }
        }

        public void Save()
        {
            Precondition.PropertyNotNull(Settings, nameof(Settings));

            if (string.IsNullOrEmpty(Settings.ConfigDir))
                throw new ConfigException("Config directory is null.");

            if (!Directory.Exists(Settings.ConfigDir))
                Directory.CreateDirectory(Settings.ConfigDir);

            lock (_configMap)
            {
                foreach (var configPair in _configMap)
                {
                    SaveConfig(configPair.Key, configPair.Value);
                }
            }
        }

        private void SaveConfig(string key, object configData)
        {
            var configFile = Settings.ConfigFileFactory.Create(key, configData);
            var configFileAsString = Settings.ConfigFileAdapter.ToString(configFile);
            var configFilePath = Path.Combine(
                Settings.ConfigDir,
                Settings.ConfigFileNameFactory.Create(configFile, Settings.ConfigFileExtension));
            Settings.StoreService.SaveAsync(configFilePath, configFileAsString).Wait();
        }

        public void Load()
        {
            Precondition.PropertyNotNull(Settings, nameof(Settings));
            if (!Directory.Exists(Settings.ConfigDir))
                return;

            var entryFiles = Settings
                .StoreService
                .GetEntriesAsync(Settings.ConfigDir, Settings.ConfigFileExtension)
                .Result;

            lock (_configMap)
            {
                _configMap.Clear();
                foreach (var file in entryFiles)
                {
                    try
                    {
                        LoadConfigDataFrom(file);
                    }
                    catch (Exception ex)
                    {
                        OnLoadConfigDataFail(ex);
                    }
                }
            }
        }

        private void OnLoadConfigDataFail(Exception exception)
        {
            if (!IsHandlableException(exception)) throw exception;
            if (!Settings.IgnoreLoadingFailure)
                throw new ConfigException("Can not load configuration file.", exception);
        }

        private bool IsHandlableException(Exception exception)
        {
            return exception is IOException ||
                   exception is TypeLoadException ||
                   exception is JsonException;
        }

        private void LoadConfigDataFrom(string file)
        {
            var configFileAsString = Settings.StoreService.LoadAsync(file).Result;
            var configFile = Settings.ConfigFileAdapter.ToConfigFile(configFileAsString);
            _configMap.Add(
                configFile.Metadata.Key, 
                new ConfigDataCheckerImpl(Settings.PreventNullReference).Check(configFile.ConfigData));
        }
    }
}