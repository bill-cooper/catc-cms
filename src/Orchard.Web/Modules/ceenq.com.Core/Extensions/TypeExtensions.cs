using System;
using System.Reflection;

namespace ceenq.com.Core.Extensions {
    public static class TypeExtensions
    {
        public static bool IsIEnumerable(this PropertyInfo property)
        {
            Type inter;
            Type type = property.PropertyType;
            inter = type.GetInterface("System.Collections.Generic.IReadOnlyCollection`1", false);
            if (inter != null)
                return true;

            inter = type.GetInterface("System.Collections.IEnumerable", false);
            if (inter != null && type.GetMethod("Add", new Type[] { typeof(object) }) != null)
                return true;

            inter = type.GetInterface("System.Collections.Generic.IEnumerable`1", false);
            if (inter != null && type.GetMethod("Add", new Type[] { inter.GetGenericArguments()[0] }) != null)
                return true;
            return false;
        }

        public static PropertyInfo[] GetPublicProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty);
        }

    }
}