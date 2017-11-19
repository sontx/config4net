using System;

namespace Config4Net.Core
{
    /// <inheritdoc />
    /// <summary>
    /// Annotate a class is a config type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigAttribute : Attribute
    {
        /// <summary>
        /// Config key, it can be null. If it's null or empty
        /// the library will set a key automatically.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Determine the class is a app config or not.
        /// </summary>
        public bool IsAppConfig { get; set; }
    }
}