using System;

namespace Config4Net.Core
{
    /// <summary>
    /// Provides a helper class that can produce an config object from a giving type.
    /// It can be used when you want to setup some initial data for the config before
    /// give it to <see cref="ConfigPool"/> for example set some default values.
    /// </summary>
    public interface IConfigObjectFactory
    {
        object CreateDefault(Type fromType);
    }

    public class DefaultConfigObjectFactory : IConfigObjectFactory
    {
        public object CreateDefault(Type fromType)
        {
            return Activator.CreateInstance(fromType);
        }
    }
}