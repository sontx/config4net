using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;

namespace Config4Net.Utils
{
    public static class ObjectUtils
    {
        public static void SetProperty(object source, string name, object value)
        {
            source.GetType().GetProperty(name)?.SetValue(source, value);
        }

        public static bool DeepEquals(object obj1, object obj2, bool ignoreCase = false)
        {
            var st1 = JsonConvert.SerializeObject(obj1);
            var st2 = JsonConvert.SerializeObject(obj2);
            return string.Compare(st1, st2, ignoreCase) == 0;
        }

        public static string ToString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T ToObject<T>(string st)
        {
            return JsonConvert.DeserializeObject<T>(st);
        }

        public static object CreateDefaultInstance(Type fromType)
        {
            Precondition.ArgumentNotNull(fromType, nameof(fromType));
            return fromType.GetConstructors().Any(constructor => constructor.GetParameters().Length == 0) 
                ? Activator.CreateInstance(fromType)
                : null;
        }

        public static void FillNullProperties(object source, Func<PropertyInfo, object> propertyValueFactory)
        {
            Precondition.ArgumentNotNull(source, nameof(source));
            Precondition.ArgumentNotNull(propertyValueFactory, nameof(propertyValueFactory));

            var sourceType = source.GetType();

            var propertyInfos = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos)
            {
                var propertyType = propertyInfo.PropertyType;
                var setMethodInfo = propertyInfo.GetSetMethod();
                if (!propertyType.IsClass || setMethodInfo == null || !setMethodInfo.IsPublic || !propertyInfo.CanWrite) continue;

                var propertyValue = propertyInfo.GetValue(source);
                if (propertyValue != null)
                {
                    FillNullProperties(propertyValue, propertyValueFactory);
                }
                else if (propertyInfo.CanWrite)
                {
                    propertyValue = propertyValueFactory(propertyInfo);
                    if (propertyValue == null) continue;
                    FillNullProperties(propertyValue, propertyValueFactory);
                    propertyInfo.SetValue(source, propertyValue);
                }
            }
        }
    }
}