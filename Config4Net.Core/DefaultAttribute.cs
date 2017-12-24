using System;

namespace Config4Net.Core
{
    /// <summary>
    /// Annotate a default value for a config's property if it is not present.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultAttribute : Attribute
    {
        /// <summary>
        /// Default value if the property is not present.
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultValue">Set value for <see cref="DefaultValue"/></param>
        public DefaultAttribute(object defaultValue)
        {
            DefaultValue = defaultValue;
        }
    }
}