using System;

namespace Config4Net.Core
{
    /// <summary>
    /// Provides a helper class that can produce a config object from a giving type.
    /// It can be used when you want to setup some initial data for the config before
    /// give it to <see cref="ConfigPool"/> for example set some default values.
    /// </summary>
    public interface IConfigObjectFactory
    {
        /// <summary>
        /// Create a config object, the implementation should check the request type
        /// before creates new instance or returns null.
        /// </summary>
        /// <param name="fromType">The request type.</param>
        /// <returns>New config object or null if the request type is not handled by current implementation.</returns>
        object CreateDefault(Type fromType);
    }

    /// <summary>
    /// Default implementation for <see cref="IConfigObjectFactory"/> that creates a default instance
    /// from a giving type uses the default constructor.
    /// </summary>
    public class DefaultConfigObjectFactory : IConfigObjectFactory
    {
        /// <inheritdoc />
        public object CreateDefault(Type fromType)
        {
            return Activator.CreateInstance(fromType);
        }
    }
}