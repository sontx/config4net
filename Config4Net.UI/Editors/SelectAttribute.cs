using Config4Net.Types;
using System;

namespace Config4Net.UI.Editors
{
    /// <summary>
    /// Extra configs for <see cref="ISelectEditor"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class SelectAttribute : Attribute
    {
        /// <summary>
        /// Type of <see cref="ISelectFactory"/>.
        /// </summary>
        public Type SelectFactoryType { get; set; }

        /// <summary>
        /// Create <see cref="SelectAttribute"/> instance.
        /// </summary>
        public SelectAttribute(Type selectFactoryType)
        {
            SelectFactoryType = selectFactoryType;
        }
    }
}