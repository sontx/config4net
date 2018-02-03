using System;

namespace Config4Net.Core.Manager
{
    /// <summary>
    /// Manages <see cref="IConfigDataFactory"/>.
    /// </summary>
    public interface IConfigDataFactoryManager
    { 
        /// <summary>
        /// Registers an <see cref="IConfigDataFactory"/> to the managed list for a specific config type.
        /// </summary>
        void Register(Type configType, IConfigDataFactory factory);

        /// <summary>
        /// Unregisters an <see cref="IConfigDataFactory"/> from the managed list by a specific config type.
        /// </summary>
        void Unregister(Type configType);

        /// <summary>
        /// Gets an <see cref="IConfigDataFactory"/> by a giving config type.
        /// </summary>
        IConfigDataFactory Get(Type configType, bool preventNullReference);
    }
}