using System;
using System.Reflection;
using Config4Net.Utils;

namespace Config4Net.UI.Editors.Definations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class DefinationAttribute : Attribute
    {
        public Type Value { get; set; }

        public DefinationAttribute(Type definationType)
        {
            Precondition.ArgumentCompatibleType(definationType, typeof(IDefinationType), nameof(definationType));
            Value = definationType;
        }
    }

    public class DefinationInfo
    {
        public Type Value { get; set; }

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