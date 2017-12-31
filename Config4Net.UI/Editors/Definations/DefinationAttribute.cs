using System;
using System.Reflection;
using Config4Net.Utils;

namespace Config4Net.UI.Editors.Definations
{
    /// <summary>
    /// Defines an addition info.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class DefinationAttribute : Attribute
    {
        /// <summary>
        /// Type of defination object.
        /// </summary>
        public Type Value { get; set; }

        /// <summary>
        /// Creates a <see cref="DefinationAttribute"/> object.
        /// </summary>
        /// <param name="definationType"></param>
        public DefinationAttribute(Type definationType)
        {
            Precondition.ArgumentCompatibleType(definationType, typeof(IDefinationType), nameof(definationType));
            Value = definationType;
        }
    }

    /// <summary>
    /// Extracts from <see cref="DefinationAttribute"/>.
    /// </summary>
    public class DefinationInfo
    {
        /// <summary>
        /// Type of defination object.
        /// </summary>
        public Type Value { get; set; }

        /// <summary>
        /// Creates <see cref="DefinationInfo"/> from a giving property.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static DefinationInfo From(MemberInfo memberInfo)
        {
            var definationAttribute = memberInfo.GetCustomAttribute<DefinationAttribute>();
            if (definationAttribute == null) return null;
            return new DefinationInfo
            {
                Value = definationAttribute.Value
            };
        }
    }
}