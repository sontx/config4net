using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Config4Net.Utils
{
    public static class ObjectUtils
    {
        public static void SetProperty(object source, string name, object value)
        {
            var propertyInfo = source.GetType().GetProperty(name);
            if (propertyInfo == null) return;

            var convertedValue = ChangeType(value, propertyInfo.PropertyType);
            propertyInfo.SetValue(source, convertedValue);
        }

        public static object GetProperty(object source, string name)
        {
            return source.GetType().GetProperty(name)?.GetValue(source);
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

        public static void IterateProperties(object source, Func<PropertyInfo, object, object> callback)
        {
            Precondition.ArgumentNotNull(source, nameof(source));
            Precondition.ArgumentNotNull(callback, nameof(callback));

            var sourceType = source.GetType();

            var propertyInfos = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos)
            {
                var setMethodInfo = propertyInfo.GetSetMethod();
                if (setMethodInfo == null || !setMethodInfo.IsPublic || !propertyInfo.CanWrite) continue;
                var propertyValue = propertyInfo.GetValue(source);
                propertyValue = callback(propertyInfo, propertyValue);
                propertyInfo.SetValue(source, propertyValue);
                if (propertyValue != null)
                    IterateProperties(propertyValue, callback);
            }
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

        public static object ExecuteMethod(object source, string methodName, params object[] paramsObjects)
        {
            Precondition.ArgumentNotNull(source, nameof(source));
            Precondition.ArgumentNotNull(methodName, nameof(methodName));

            return source.GetType().GetMethod(methodName)?.Invoke(
                source,
                BindingFlags.Instance,
                null,
                paramsObjects,
                CultureInfo.CurrentCulture);
        }

        public static object CreateGenericList(Type underlyingType, Type listType)
        {
            var constructedListType = listType.MakeGenericType(underlyingType);
            return Activator.CreateInstance(constructedListType);
        }

        public static object CreateGenericList(Type underlyingType)
        {
            return CreateGenericList(underlyingType, typeof(List<>));
        }

        public static void CopyToGenericList(object genericListDest, Type itemType, IEnumerable enumerableSrc)
        {
            ExecuteMethod(genericListDest, "Clear");

            foreach (var item in enumerableSrc)
            {
                ExecuteMethod(genericListDest, "Add", Convert.ChangeType(item, itemType));
            }
        }

        public static object ChangeType(object value, Type destType)
        {
            if (value == null) return null;

            if (!IsGenericList(destType) || !IsGenericList(value.GetType()))
                return value is IConvertible ? Convert.ChangeType(value, destType) : value;

            return ChangeGenericListType(value, destType);
        }

        public static object ChangeGenericListType(object value, Type destType)
        {
            var destItemType = destType.GenericTypeArguments[0];
            var srcItemType = value.GetType().GenericTypeArguments[0];
            if (destItemType == srcItemType)
                return value;

            var destList = CreateGenericList(destItemType);
            CopyToGenericList(destList, destItemType, (IEnumerable) value);
            return destList;
        }

        public static bool IsGenericList(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IList<>) || type.GetInterface("IList") == typeof(IList));
        }
    }
}