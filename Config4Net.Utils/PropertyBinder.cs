using System;
using System.Reflection;

namespace Config4Net.Utils
{
    public sealed class PropertyBinder<T>
    {
        private readonly object _source;
        private readonly PropertyInfo _propertyInfo;
        private readonly object _originValue;

        public T Value
        {
            get => (T) Convert.ChangeType(_propertyInfo.GetValue(_source), typeof(T));
            set => SetValue(value);
        }

        public void Reset()
        {
            SetValue(_originValue);
        }

        private void SetValue(object value)
        {
            _propertyInfo.SetValue(_source, Convert.ChangeType(value, _propertyInfo.PropertyType));
        }

        public PropertyBinder(object source, PropertyInfo propertyInfo)
        {
            Precondition.ArgumentNotNull(source, nameof(source));
            Precondition.ArgumentNotNull(propertyInfo, nameof(propertyInfo));

            _source = source;
            _propertyInfo = propertyInfo;

            _originValue = _propertyInfo.GetValue(_source);
        }
    }
}