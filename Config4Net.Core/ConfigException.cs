using System;

namespace Config4Net.Core
{
    /// <summary>
    /// Exception while manipulating <see cref="Config"/>.
    /// </summary>
    [Serializable]
    public class ConfigException : Exception
    {
        /// <summary>
        /// Creates <see cref="ConfigException"/>.
        /// </summary>
        public ConfigException()
        {
        }

        /// <summary>
        /// Creates <see cref="ConfigException"/> with message.
        /// </summary>
        public ConfigException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates <see cref="ConfigException"/> with message and inner exception.
        /// </summary>
        public ConfigException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}