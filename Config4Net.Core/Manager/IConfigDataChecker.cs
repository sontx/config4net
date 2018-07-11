using System.ComponentModel;
using System.Reflection;
using Config4Net.Utils;

namespace Config4Net.Core.Manager
{
    internal interface IConfigDataChecker
    {
        object Check(object configData);
    }

    internal class ConfigDataCheckerImpl : IConfigDataChecker
    {
        private readonly bool _preventNullReference;

        public ConfigDataCheckerImpl(bool preventNullReference)
        {
            _preventNullReference = preventNullReference;
        }

        public object Check(object configData)
        {
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