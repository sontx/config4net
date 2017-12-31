using System;
using System.Collections.Generic;
using Config4Net.Utils;

namespace Config4Net.UI.Editors.Definations
{
    /// <summary>
    /// Addition info for <see cref="IEnumEditor"/>.
    /// </summary>
    public abstract class EnumDefination : IDefinationType
    {
        private readonly IEnumerable<Enum> _enumerable;

        /// <summary>
        /// Create an <see cref="EnumDefination"/> object.
        /// </summary>
        /// <param name="enumType"></param>
        protected EnumDefination(Type enumType)
        {
            Precondition.ArgumentNotNull(enumType, nameof(enumType));
            var values = Enum.GetValues(enumType);
            _enumerable = WrapperUtils.GetEnumerable<Enum>(values.GetEnumerator());
        }

        /// <inheritdoc />
        public object GetDefination()
        {
            return _enumerable;
        }
    }
}