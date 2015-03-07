
namespace InplayBet.Web.Utilities.Expression
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// This class provides several useful functions for runtime type conversions and extraction of class members information
    /// </summary>
    static class TypeUtils
    {
        /// <summary>
        /// Determines whether [is nullable type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool IsNullableType(Type type)
        {
            return (type.IsGenericType &&
                type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        /// <summary>
        /// Determines whether the specified STR is convertable.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool IsConvertable(string str, Type type)
        {
            if (str == null)
                return type.IsClass || IsNullableType(type);

            object value;
            return ConvertFromString(str, type, out value);
        }

        /// <summary>
        /// Converts from string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool ConvertFromString(string str, Type type, out object value)
        {
            value = null;
            try
            {
                MethodInfo mi = type.GetMethod("FromString", BindingFlags.Public | BindingFlags.Static);
                if (mi != null)
                    value = mi.Invoke(null, new object[] { str });
                else if (type == typeof(DateTime))
                    value = DateTime.Parse(str);
                else if (type.BaseType == typeof(Enum))
                    value = Enum.Parse(type, str);
                else if (IsNullableType(type) /*|| type.IsClass*/)
                {
                    if (str == string.Empty || string.Compare(str, "null", true) == 0)
                        value = null;
                    else
                    {
                        ConstructorInfo[] ci = type.GetConstructors();
                        ParameterInfo[] pi = ci[0].GetParameters();
                        object val = Convert.ChangeType(str, pi[0].ParameterType);
                        value = ci[0].Invoke(new object[] { val });
                    }
                }
                else
                    value = Convert.ChangeType(str, type);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the property info.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            BindingFlags bf = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase;
            PropertyInfo info = type.GetProperty(propertyName, bf);
            if (info != null) return info;

            foreach (Type interfaceType in type.GetInterfaces())
            {
                info = interfaceType.GetProperty(propertyName, bf);
                if (info != null) return info;
            }

            return null;
        }

        /// <summary>
        /// Methods the exists.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        public static bool MethodExists(Type type, string methodName)
        {
            if (GetExtensionMethod(type, methodName) != null) return true;

            BindingFlags bf = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase;
            MethodInfo[] infos = type.GetMethods(bf);

            foreach (MethodInfo mi in infos)
                if (mi.Name == methodName) return true;
            return false;
        }

        /// <summary>
        /// Gets the method info.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="argsCount">The args count.</param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(Type type, string methodName, int argsCount)
        {
            var miex = GetExtensionMethod(type, methodName, argsCount);
            if (miex != null) return miex;

            BindingFlags bf = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Static;
            MethodInfo[] infos = type.GetMethods(bf);

            foreach (MethodInfo mi in infos)
                if (mi.Name == methodName && mi.GetParameters().Length == argsCount) return mi;
            return null;
        }

        /// <summary>
        /// Gets the type by string.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public static Type GetTypeByString(string typeName)
        {
            List<Type> AssTypes = new List<Type>();

            foreach (Assembly item in AppDomain.CurrentDomain.GetAssemblies())
            {
                AssTypes.AddRange(item.GetTypes());
            }

            var currenttype = AssTypes.FirstOrDefault(type => type.IsSealed && !type.IsGenericType
                && !type.IsNested && type.Name.Equals(typeName));

            return currenttype;
        }

        /// <summary>
        /// Gets the extension methods.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public static MethodInfo[] GetExtensionMethods(this Type t)
        {
            List<Type> AssTypes = new List<Type>();

            foreach (Assembly item in AppDomain.CurrentDomain.GetAssemblies())
            {
                AssTypes.AddRange(item.GetTypes());
            }

            var query = from type in AssTypes
                        where type.IsSealed && !type.IsGenericType && !type.IsNested
                        from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                        where method.IsDefined(typeof(ExtensionAttribute), false)
                        where method.GetParameters()[0].ParameterType == t
                        select method;
            return query.ToArray<MethodInfo>();
        }

        /// <summary>
        /// Extends the System.Type-type to search for a given extended MethodeName.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="methodName">Name of the Methode</param>
        /// <param name="argsCount">The arguments count.</param>
        /// <returns>
        /// the found Methode or null
        /// </returns>
        public static MethodInfo GetExtensionMethod(this Type t, string methodName, int? argsCount = 0)
        {
            if (argsCount.Value > 0)
            {
                return t.GetExtensionMethods().FirstOrDefault(mi => mi.Name.Equals(methodName)
                  && (mi.GetParameters().Length - 1) == argsCount);
            }
            else
            {
                return t.GetExtensionMethods().FirstOrDefault(mi => mi.Name.Equals(methodName));
            }
        }

        /// <summary>
        /// Determines whether [is extension method] [the specified t].
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="mi">The mi.</param>
        /// <returns></returns>
        public static bool IsExtensionMethod(this Type t, MethodInfo mi)
        {
            return t.GetExtensionMethods().Any(m => m == mi);
        }
    }
}