using System;
using System.Collections.Generic;
using Config4Net.Utils;

namespace Config4Net.UI.Editors.Definations
{
    public abstract class EnumDefination : IDefinationType
    {
        private readonly IEnumerable<Enum> _enumerable;

        protected EnumDefination(Type enumType)
        {
            Precondition.ArgumentNotNull(enumType, nameof(enumType));
            var values = Enum.GetValues(enumType);
            _enumerable = WrapperUtils.GetEnumerable<Enum>(values.GetEnumerator());
        }

        public object GetDefination()
        {
            return _enumerable;
        }
    }
}