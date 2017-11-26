using System;
using System.Reflection;

namespace Config4Net.Utils
{
    public static class Precondition
    {
        public static void ArgumentNotNull(object argument, string message = null)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(message);
            }
        }

        public static void PropertyNotNull(object property, string message = null)
        {
            if (property == null)
            {
                throw new NullReferenceException(message);
            }
        }

        public static void ArgumentCompatibleType(object argument, Type checkType, string message = null)
        {
            if (argument is Type argumentType && checkType.IsAssignableFrom(argumentType)) return;
            if (checkType.IsInstanceOfType(argument)) return;

            throw new ArgumentException(message);
        }

        public static void ArgumentHasAttribute(object argument, Type checkAttribute, string message = null)
        {
            if (argument.GetType().GetCustomAttribute(checkAttribute) == null)
            {
                throw new ArgumentException(message);
            }
        }
    }
}