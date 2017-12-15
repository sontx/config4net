using System;

namespace Config4Net.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultAttribute : Attribute
    {
        public object DefaultValue { get; set; }

        public DefaultAttribute(object defaultValue)
        {
            DefaultValue = defaultValue;
        }
    }
}