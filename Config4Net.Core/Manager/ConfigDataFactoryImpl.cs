using System;
using System.ComponentModel;
using System.Reflection;
using Config4Net.Utils;

namespace Config4Net.Core.Manager
{
    /// <summary>
    /// Default implementation for <see cref="IConfigDataFactory"/> that creates a default instance
    /// from a giving type uses the default constructor.
    /// </summary>
    internal class ConfigDataFactoryImpl : IConfigDataFactory
    {
        private readonly Type _configType;
        private readonly IConfigDataChecker _configDataChecker;

        public ConfigDataFactoryImpl(Type configType, IConfigDataChecker configDataChecker)
        {
            _configType = configType;
            _configDataChecker = configDataChecker;
        }

        public object Create()
        {
            var configData = Activator.CreateInstance(_configType);
            return _configDataChecker.Check(configData);
        }
    }
}