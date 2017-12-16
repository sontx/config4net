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
            get
            {
                var value = _propertyInfo.GetValue(_source);
                return (T)(value is IConvertible ? Convert.ChangeType(value, typeof(T)) : value);
            }
            set => SetValue(value);
        }

        public void Reset()
        {
            SetValue(_originValue);
        }

        private void SetValue(object value)
        {
            object convertedValue;
            if (value is IConvertible)
            {
                convertedValue = Convert.ChangeType(value, _propertyInfo.PropertyType);
            }
            else
            {
                convertedValue = value == null || value.GetType() == _propertyInfo.PropertyType ? value : ObjectUtils.ToString(value);
            }

            _propertyInfo.SetValue(_source, convertedValue);
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