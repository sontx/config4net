using System;
using Config4Net.Utils;

namespace Config4Net.Core.Adapter
{
    internal class DefaultConfigFileFactory : IConfigFileFactory
    {
        public IConfigFile Create(string key, object configData)
        {
            Precondition.ArgumentNotNull(key, nameof(key));
            Precondition.ArgumentNotNull(configData, nameof(configData));

            return new ConfigFile
            {
                ConfigData = configData,
                Metadata = new Metadata
                {
                    Key = key,
                    TypeId = configData.GetType().AssemblyQualifiedName,
                    Modified = DateTime.Now
                }
            };
        }
    }
}