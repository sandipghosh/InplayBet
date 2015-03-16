
namespace InplayBet.Web.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public static class Extensions
    {
        #region Conversion Section
        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyTo(this object source, object destination)
        {
            var sourceType = source.GetType();
            var destinationType = destination.GetType();

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            var properties = sourceType.GetProperties(flags);
            foreach (var sourceProperty in properties)
            {
                var destinationProperty = destinationType.GetProperty(sourceProperty.Name, flags);
                if (destinationProperty.PropertyType.Equals(sourceProperty.PropertyType))
                {
                    destinationProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                }
            }
        }

        /// <summary>
        /// Parses the nullable int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int? ParseNullableInt(this string value)
        {
            int intValue;
            if (int.TryParse(value, out intValue))
                return intValue;
            return null;
        }

        /// <summary>
        /// Tries the parse nullable date time.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static DateTime? TryParseNullableDateTime(string text)
        {
            DateTime date;
            return DateTime.TryParse(text, out date) ? date : (DateTime?)null;
        }
        #endregion

        #region Extension Functions Section
        /// <summary>
        /// Determines whether the specified STR is GUID.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static bool IsGUID(this string str)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            Regex format = new Regex(
                "^[A-Fa-f0-9]{32}$|" +
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");

            Match match = format.Match(str);
            return match.Success;
        }

        /// <summary>
        /// Check whether the string is empty or null (Extension Method)
        /// </summary>
        /// <param name="str">The string to be checked</param>
        /// <returns>Returns true if string is NULL or String.Empty, otherwise false</returns>
        public static bool IsEmptyOrNull(this string str)
        {
            if (str == null || str == string.Empty)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Transform object into Identity data type (integer).
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="defaultId">Optional default value is -1.</param>
        /// <returns>Identity value.</returns>
        public static int AsId(this object item, int defaultId = -1)
        {
            if (item == null)
                return defaultId;

            int result;
            if (!int.TryParse(item.ToString(), out result))
                return defaultId;

            return result;
        }

        /// <summary>
        /// Transform object into integer data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="defaultId">Optional default value is default(int).</param>
        /// <returns>The integer value.</returns>
        public static int AsInt(this object item, int defaultInt = 0)
        {
            if (item == null)
                return defaultInt;

            int result;
            if (!int.TryParse(item.ToString(), out result))
                return defaultInt;

            return result;
        }

        /// <summary>
        /// Transform object into byte data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="defaultByte">Optional default value is default(byte).</param>
        /// <returns>The byte value.</returns>
        public static byte AsByte(this object item, byte defaultByte = default(byte))
        {
            if (item == null)
                return defaultByte;

            byte result;
            if (!byte.TryParse(item.ToString(), out result))
                return defaultByte;

            return result;
        }

        /// <summary>
        /// Transform object into double data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="defaultId">Optional default value is default(double).</param>
        /// <returns>The double value.</returns>
        public static double AsDouble(this object item, double defaultDouble = 0)
        {
            if (item == null)
                return defaultDouble;

            double result;
            if (!double.TryParse(item.ToString(), out result))
                return defaultDouble;

            return result;
        }

        /// <summary>
        /// Ases the decimal.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultDouble">The default double.</param>
        /// <returns></returns>
        public static decimal AsDecimal(this object item, decimal defaultDecimal = 0)
        {
            if (item == null)
                return defaultDecimal;

            decimal result;
            if (!decimal.TryParse(item.ToString(), out result))
                return defaultDecimal;

            return result;
        }

        /// <summary>
        /// Ases the long.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultLong">The default long.</param>
        /// <returns></returns>
        public static long AsLong(this object item, long defaultLong = 0)
        {
            if (item == null)
                return defaultLong;

            long result;
            if (!long.TryParse(item.ToString(), out result))
                return defaultLong;

            return result;
        }

        /// <summary>
        /// Transform object into string data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="defaultId">Optional default value is default(string).</param>
        /// <returns>The string value.</returns>
        public static string AsString(this object item, string defaultString = "")
        {
            if (item == null || item.Equals(System.DBNull.Value))
                return defaultString;

            return item.ToString().Trim();
        }

        /// <summary>
        /// Transform object into DateTime data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="defaultId">Optional default value is default(DateTime).</param>
        /// <returns>The DateTime value.</returns>
        public static DateTime AsDateTime(this object item, DateTime defaultDateTime = default(DateTime))
        {
            if (item == null || string.IsNullOrEmpty(item.ToString()))
                return defaultDateTime;

            DateTime result;
            if (!DateTime.TryParse(item.ToString(), out result))
                return defaultDateTime;

            return result;
        }

        /// <summary>
        /// Transform object into bool data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="defaultId">Optional default value is default(bool).</param>
        /// <returns>The bool value.</returns>
        public static bool AsBool(this object item, bool defaultBool = false)
        {
            if (item == null)
                return defaultBool;

            return new List<string>() { "yes", "y", "true" }.Contains(item.ToString().ToLower());
        }

        /// <summary>
        /// Transform string into byte array.
        /// </summary>
        /// <param name="s">The object to be transformed.</param>
        /// <returns>The transformed byte array.</returns>
        public static byte[] AsByteArray(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            return Convert.FromBase64String(s);
        }

        /// <summary>
        /// Transform object into Guid data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <returns>The Guid value.</returns>
        public static Guid AsGuid(this object item)
        {
            try { return new Guid(item.ToString()); }
            catch { return Guid.Empty; }
        }

        /// <summary>
        /// Concatenates SQL and ORDER BY clauses into a single string. 
        /// </summary>
        /// <param name="sql">The SQL string</param>
        /// <param name="sortExpression">The Sort Expression.</param>
        /// <returns>Concatenated SQL Statement.</returns>
        public static string OrderBy(this string sql, string sortExpression)
        {
            if (string.IsNullOrEmpty(sortExpression))
                return sql;

            return sql + " ORDER BY " + sortExpression;
        }

        /// <summary>
        /// Takes an enumerable source and returns a comma separate string.
        /// Handy to build SQL Statements (for example with IN () statements) from object collections.
        /// </summary>
        /// <typeparam name="T">The Enumerable type</typeparam>
        /// <typeparam name="U">The original data type (typically identities - int).</typeparam>
        /// <param name="source">The enumerable input collection.</param>
        /// <param name="func">The function that extracts property value in object.</param>
        /// <returns>The comma separated string.</returns>
        public static string CommaSeparate<T, U>(this IEnumerable<T> source, Func<T, U> func)
        {
            return string.Join(",", source.Select(s => func(s).ToString()).ToArray());
        }

        /// <summary>
        /// Toes the null able.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static Nullable<T> ToNullable<T>(this string s) where T : struct
        {
            Nullable<T> result = new Nullable<T>();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(s);
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="isRequired">The is required.</param>
        /// <returns></returns>
        public static T GetAttribute<T>
            (this MemberInfo member, bool isRequired)
            where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }

        /// <summary>
        /// Gets the display name of the property.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns></returns>
        public static string GetPropertyDisplayName<T>
            (Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = GetPropertyInformation(propertyExpression.Body);
            if (memberInfo == null)
            {
                throw new ArgumentException(
                    "No property reference expression was found.",
                    "propertyExpression");
            }

            var attr = memberInfo.GetAttribute<DisplayNameAttribute>(false);
            if (attr == null)
            {
                return memberInfo.Name;
            }

            return attr.DisplayName;
        }

        /// <summary>
        /// Gets the property information.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns></returns>
        public static MemberInfo GetPropertyInformation(System.Linq.Expressions.Expression propertyExpression)
        {
            MemberExpression memberExpr = propertyExpression as MemberExpression;
            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = propertyExpression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
            {
                return memberExpr.Member;
            }

            return null;
        }
        #endregion

        #region Reflection Extension
        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static object GetPropertyValue<TArg>(this TArg obj, string propertyName)
            where TArg : class
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">The property value.</param>
        public static void SetPropertyValue<TArg>(this TArg obj, string propertyName, object propertyValue)
            where TArg : class
        {
            obj.GetType().GetProperty(propertyName).SetValue(obj, propertyValue, null);
        }

        /// <summary>
        /// Determines whether this instance [can directly compare] the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool CanDirectlyCompare<T>(this T type)
            where T : Type
        {
            return typeof(IComparable).IsAssignableFrom(type) || type.IsPrimitive || type.IsValueType;
        }

        /// <summary>
        /// Determines whether [is empty collection] [the specified collection].
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static bool IsEmptyCollection<T>(this List<T> collection)
        {
            //return (collection == null && collection.Count > 0);
            return (collection == null || collection.Count == 0);
        }

        /// <summary>
        /// Determines whether [is empty collection] [the specified collection].
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static bool IsEmptyCollection<T>(this IEnumerable<T> collection)
        {
            //return (collection == null && collection.Count() > 0);
            return (collection == null || collection.Count() == 0);
        }

        /// <summary>
        /// Firsts the or default custom.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static T FirstOrDefaultCustom<T>(this List<T> collection) where T : class
        {
            if (!collection.IsEmptyCollection())
            {
                return collection.FirstOrDefault();
            }
            return default(T);
        }

        /// <summary>
        /// Firsts the or default custom.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="pradicate">The pradicate.</param>
        /// <returns></returns>
        public static T FirstOrDefaultCustom<T>(this List<T> collection, Func<T, bool> pradicate) where T : class
        {
            if (!collection.IsEmptyCollection())
            {
                return collection.FirstOrDefault(pradicate);
            }
            return default(T);
        }

        /// <summary>
        /// Firsts the or default custom.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static T FirstOrDefaultCustom<T>(this IEnumerable<T> collection) where T : class
        {
            if (!collection.ToList().IsEmptyCollection())
            {
                return collection.FirstOrDefault();
            }
            return default(T);
        }

        /// <summary>
        /// Firsts the or default custom.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="pradicate">The pradicate.</param>
        /// <returns></returns>
        public static T FirstOrDefaultCustom<T>(this IEnumerable<T> collection, Func<T, bool> pradicate) where T : class
        {
            if (!collection.IsEmptyCollection())
            {
                return collection.FirstOrDefault(pradicate);
            }
            return default(T);
        }

        /// <summary>
        /// Toes the data table.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this object items)
        {
            //checks for list type and Ienumerable type only
            if (items.GetType().GetGenericTypeDefinition() == typeof(List<>) ||
                items.GetType().GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                //Get the type
                Type collectionContentType = items.GetType().GetInterfaces()
                    .Where(t => t.IsGenericType == true
                    && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    .Select(t => t.GetGenericArguments()[0]).SingleOrDefault();

                if (collectionContentType != null)
                {
                    DataTable table = new DataTable(collectionContentType.Name);
                    PropertyInfo[] props = collectionContentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    // Add the properties as columns to the datatable
                    foreach (var prop in props)
                    {
                        Type propType = prop.PropertyType;

                        // Is it a nullable type? Get the underlying type 
                        if (propType.IsGenericType && propType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                            propType = new NullableConverter(propType).UnderlyingType;

                        table.Columns.Add(prop.Name, propType);
                    }

                    // Add the property values per T as rows to the datatable
                    foreach (var item in (IEnumerable)items)
                    {
                        var values = new object[props.Length];
                        for (var i = 0; i < props.Length; i++)
                            values[i] = props[i].GetValue(item, null);

                        table.Rows.Add(values);
                    }

                    return table;
                }
            }
            return null;
        }

        #endregion
    }
}