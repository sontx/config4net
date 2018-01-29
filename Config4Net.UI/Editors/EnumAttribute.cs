using System;

namespace Config4Net.UI.Editors
{
    /// <summary>
    /// Extra configs for <see cref="IEnumEditor"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EnumAttribute : Attribute
    {
        /// <summary>
        /// Enum type that will be listed in combobox.
        /// </summary>
        public Type EnumType { get; set; }

        /// <summary>
        /// Create new <see cref="EnumAttribute"/>.
        /// </summary>
        public EnumAttribute(Type enumType)
        {
            EnumType = enumType;
        }
    }
}