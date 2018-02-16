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
        private readonly bool _preventNullReference;

        public ConfigDataFactoryImpl(Type configType, bool preventNullReference)
        {
            _configType = configType;
            _preventNullReference = preventNullReference;
        }

        public object Create()
        {
            var configData = Activator.CreateInstance(_configType);
            ObjectUtils.IterateProperties(configData, (propertyInfo, propertyValue) =>
            {
                var defaultValueAttribute = propertyInfo.GetCustomAttribute<DefaultValueAttribute>();
                if (defaultValueAttribute != null) return defaultValueAttribute.Value;
                if (_preventNullReference && propertyValue == null)
                    return ObjectUtils.CreateDefaultInstance(propertyInfo.PropertyType);
                return propertyValue;
            });
            return configData;
        }
    }
}