using System;

namespace Config4Net.Core
{
    /// <inheritdoc />
    /// <summary>
    ///     Annotates a class is a configuration type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigAttribute : Attribute
    {
        /// <summary>
        ///     Creates <see cref="ConfigAttribute" />.
        /// </summary>
        public ConfigAttribute(string key = null)
        {
            Key = key;
        }

        /// <summary>
        ///     configuration key, it can be null. If it's null or empty
        ///     the library will set a key automatically.
        /// </summary>
        public string Key { get; set; }
    }
}