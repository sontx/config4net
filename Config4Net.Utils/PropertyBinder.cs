using System.Reflection;

namespace Config4Net.Utils
{
    public sealed class PropertyBinder<T>
    {
        private readonly object _source;
        private readonly PropertyInfo _propertyInfo;
        private readonly T _originValue;

        public T Value
        {
            get => (T) _propertyInfo.GetValue(_source);
            set => _propertyInfo.SetValue(_source, value);
        }

        public void Reset()
        {
            Value = _originValue;
        }

        public PropertyBinder(object source, PropertyInfo propertyInfo)
        {
            Precondition.ArgumentNotNull(source, nameof(source));
            Precondition.ArgumentNotNull(propertyInfo, nameof(propertyInfo));

            _source = source;
            _propertyInfo = propertyInfo;

            _originValue = (T) _propertyInfo.GetValue(_source);
        }
    }
}