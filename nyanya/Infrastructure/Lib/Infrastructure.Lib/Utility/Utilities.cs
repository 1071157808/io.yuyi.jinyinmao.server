// FileInformation: nyanya/Infrastructure.Lib/Utilities.cs
// CreatedTime: 2015/03/04   6:31 PM
// LastUpdatedTime: 2015/03/10   10:49 PM

using Infrastructure.Lib.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Lib.Utility
{
    public static class Utilities
    {
        public static void CopyItemsTo<TSource>(this IEnumerable<TSource> fromSet, ICollection<TSource> toSet)
        {
            if (toSet == null)
                throw new ArgumentNullException("toSet");
            if (fromSet == null)
                return;
            foreach (TSource source in fromSet)
                toSet.Add(source);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> hash = new HashSet<TKey>();
            return source.Where(p => hash.Add(keySelector(p)));
        }

        public static void ForEach<TObject>(this IEnumerable<TObject> collection, Action<TObject> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (collection == null)
                return;
            foreach (TObject @object in collection)
                IfNotNull(@object, action);
        }

        public static Type GetDictionaryType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                return type;
            return type.GetInterfaces().Where(t =>
            {
                if (t.IsGenericType)
                    return t.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                return false;
            }).FirstOrDefault();
        }

        public static Type GetTypeOfNullable(this Type type)
        {
            return type.GetGenericArguments()[0];
        }

        public static TResult IfNotNull<TObject, TResult>(this TObject obj, Func<TObject, TResult> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            return obj.IsNotNull() ? action(obj) : default(TResult);
        }

        public static TResult IfNotNull<TObject, TResult>(this TObject obj, Func<TObject, TResult> action, TResult defaultValue)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            return obj.IsNotNull() ? action(obj) : defaultValue;
        }

        public static void IfNotNull<TObject>(this TObject obj, Action<TObject> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (obj.IsNull())
                return;
            action(obj);
        }

        public static string IfNullOrWhiteSpace(this string text, string defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(text))
                return text;
            return defaultValue;
        }

        public static bool IsCollectionType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>))
                return true;
            return type.GetInterfaces().Where(t => t.IsGenericType).Select(t => t.GetGenericTypeDefinition()).Any(t => t == typeof(ICollection<>));
        }

        public static bool IsDictionaryType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                return true;
            return type.GetInterfaces().Where(t => t.IsGenericType).Select(t => t.GetGenericTypeDefinition()).Any(t => t == typeof(IDictionary<,>));
        }

        public static bool IsEnumerableType(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(IEnumerable));
        }

        public static bool IsListOrDictionaryType(this Type type)
        {
            if (!IsListType(type))
                return IsDictionaryType(type);
            return true;
        }

        public static bool IsListType(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(IList));
        }

        public static bool IsNullableType(this Type type)
        {
            if (type.IsGenericType)
                return type.GetGenericTypeDefinition() == typeof(Nullable<>);
            return false;
        }

        public static bool IsNullOrWhiteSpace(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        public static string SubstringUpToFirst(this string text, char delimiter)
        {
            if (text == null)
                return null;
            int length = text.IndexOf(delimiter);
            if (length >= 0)
                return text.Substring(0, length);
            return text;
        }

        public static string ToStringExceptNull(this object value)
        {
            if (value == null)
            {
                return null;
            }
            return value.ToString();
        }

        public static string ToStringIncludeNull(this object value)
        {
            if (value == null)
            {
                return "";
            }
            return value.ToString();
        }
    }
}