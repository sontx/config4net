using System;
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
}