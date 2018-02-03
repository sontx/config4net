using Config4Net.Utils;
using System;

namespace Config4Net.Core.Adapter
{
    /// <summary>
    /// Creates an <see cref="IConfigFile"/>.
    /// </summary>
    public interface IConfigFileFactory
    {
        /// <summary>
        /// Creates an <see cref="IConfigFile"/>.
        /// </summary>
        IConfigFile Create(string key, object configData);
    }

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