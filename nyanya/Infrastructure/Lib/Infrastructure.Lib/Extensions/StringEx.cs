// FileInformation: nyanya/Infrastructure.Lib/StringEx.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/15   11:58 AM

using Infrastructure.Lib.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Lib.Extensions
{
    /// <summary>
    ///     Extensions for <see cref="System.String" />
    /// </summary>
    public static class StringEx
    {
        #region Private Fields

        private const int LowerCaseOffset = 'a' - 'A';

        private static readonly char[] DirSeps = { '\\', '/' };

        private static char[] SystemTypeChars = { '<', '>', '+' };

        #endregion Private Fields

        #region Public Methods

        public static string AppendPath(this string uri, params string[] uriComponents)
        {
            return AppendUrlPaths(uri, uriComponents);
        }

        public static string AppendUrlPaths(this string uri, params string[] uriComponents)
        {
            StringBuilder sb = new StringBuilder(uri.WithTrailingSlash());
            int i = 0;
            foreach (string uriComponent in uriComponents)
            {
                if (i++ > 0) sb.Append('/');
                sb.Append(uriComponent.UrlEncode());
            }
            return sb.ToString();
        }

        public static string AppendUrlPathsRaw(this string uri, params string[] uriComponents)
        {
            StringBuilder sb = new StringBuilder(uri.WithTrailingSlash());
            int i = 0;
            foreach (string uriComponent in uriComponents)
            {
                if (i++ > 0) sb.Append('/');
                sb.Append(uriComponent);
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Converts from base: 0 - 62
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public static string BaseConvert(this string source, int from, int to)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = "";
            int length = source.Length;
            int[] number = new int[length];

            for (int i = 0; i < length; i++)
            {
                number[i] = chars.IndexOf(source[i]);
            }

            int newlen;

            do
            {
                int divide = 0;
                newlen = 0;

                for (int i = 0; i < length; i++)
                {
                    divide = divide * @from + number[i];

                    if (divide >= to)
                    {
                        number[newlen++] = divide / to;
                        divide = divide % to;
                    }
                    else if (newlen > 0)
                    {
                        number[newlen++] = 0;
                    }
                }

                length = newlen;
                result = chars[divide] + result;
            } while (newlen != 0);

            return result;
        }

        /// <summary>
        ///     Checks if the <paramref name="source" /> contains the <paramref name="input" /> based on the provided <paramref name="comparison" /> rules.
        /// </summary>
        public static bool Contains(this string source, string input, StringComparison comparison)
        {
            return source.IndexOf(input, comparison) >= 0;
        }

        public static bool ContainsAny(this string text, params string[] testMatches)
        {
            foreach (string testMatch in testMatches)
            {
                if (text.Contains(testMatch)) return true;
            }
            return false;
        }

        public static string EncodeJson(this string value)
        {
            return String.Concat
                ("\"",
                    value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "\\n"),
                    "\""
                );
        }

        public static string EncodeXml(this string value)
        {
            return value.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;");
        }

        public static bool EqualsIgnoreCase(this string value, string other)
        {
            return String.Equals(value, other, StringComparison.CurrentCultureIgnoreCase);
        }

        public static string ExtractContents(this string fromText, string startAfter, string endAt)
        {
            return ExtractContents(fromText, startAfter, startAfter, endAt);
        }

        public static string ExtractContents(this string fromText, string uniqueMarker, string startAfter, string endAt)
        {
            if (String.IsNullOrEmpty(uniqueMarker))
                throw new ArgumentNullException("uniqueMarker");
            if (String.IsNullOrEmpty(startAfter))
                throw new ArgumentNullException("startAfter");
            if (String.IsNullOrEmpty(endAt))
                throw new ArgumentNullException("endAt");

            if (String.IsNullOrEmpty(fromText)) return null;

            int markerPos = fromText.IndexOf(uniqueMarker);
            if (markerPos == -1) return null;

            int startPos = fromText.IndexOf(startAfter, markerPos);
            if (startPos == -1) return null;
            startPos += startAfter.Length;

            int endPos = fromText.IndexOf(endAt, startPos);
            if (endPos == -1) endPos = fromText.Length;

            return fromText.Substring(startPos, endPos - startPos);
        }

        /// <summary>
        ///     A nicer way of calling <see cref="System.String.Format(string, object[])" />
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding objects in args.</returns>
        public static string Fmt(this string format, params object[] args)
        {
            return format.FormatWith(args);
        }

        /// <summary>
        ///     A nicer way of calling <see cref="System.String.Format(string, object[])" />
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding objects in args.</returns>
        public static string FmtWith(this string format, params object[] args)
        {
            return format.FormatWith(args);
        }

        /// <summary>
        ///     A nicer way of calling <see cref="System.String.Format(string, object[])" />
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding objects in args.</returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format ?? string.Empty, args);
        }

        public static string FromUtf8Bytes(this byte[] bytes)
        {
            return bytes == null ? null
                : Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        public static string GetFirst(this string source, int count = 1)
        {
            return source.SubString(0, count);
        }

        public static string GetLast(this string source, int count = 1)
        {
            int start = source.Length - count;
            if (start < 0)
            {
                start = 0;
            }
            return source.SubString(start, count);
        }

        public static bool Glob(this string value, string pattern)
        {
            int pos;
            for (pos = 0; pattern.Length != pos; pos++)
            {
                switch (pattern[pos])
                {
                    case '?':
                        break;

                    case '*':
                        for (int i = value.Length; i >= pos; i--)
                        {
                            if (Glob(value.Substring(i), pattern.Substring(pos + 1)))
                                return true;
                        }
                        return false;

                    default:
                        if (value.Length == pos || Char.ToUpper(pattern[pos]) != Char.ToUpper(value[pos]))
                        {
                            return false;
                        }
                        break;
                }
            }

            return value.Length == pos;
        }

        public static string HexEscape(this string text, params char[] anyCharOf)
        {
            if (String.IsNullOrEmpty(text)) return text;
            if (anyCharOf == null || anyCharOf.Length == 0) return text;

            HashSet<char> encodeCharMap = new HashSet<char>(anyCharOf);

            StringBuilder sb = new StringBuilder();
            int textLength = text.Length;
            for (int i = 0; i < textLength; i++)
            {
                char c = text[i];
                if (encodeCharMap.Contains(c))
                {
                    sb.Append('%' + ((int)c).ToString("x"));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string HexUnescape(this string text, params char[] anyCharOf)
        {
            if (String.IsNullOrEmpty(text)) return null;
            if (anyCharOf == null || anyCharOf.Length == 0) return text;

            StringBuilder sb = new StringBuilder();

            int textLength = text.Length;
            for (int i = 0; i < textLength; i++)
            {
                string c = text.Substring(i, 1);
                if (c == "%")
                {
                    int hexNo = Convert.ToInt32(text.Substring(i + 1, 2), 16);
                    sb.Append((char)hexNo);
                    i += 2;
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static int IndexOfAny(this string text, params string[] needles)
        {
            return IndexOfAny(text, 0, needles);
        }

        public static int IndexOfAny(this string text, int startIndex, params string[] needles)
        {
            int firstPos = -1;
            if (text != null)
            {
                foreach (string needle in needles)
                {
                    int pos = text.IndexOf(needle, startIndex);
                    if ((pos >= 0) && (firstPos == -1 || pos < firstPos))
                        firstPos = pos;
                }
            }

            return firstPos;
        }

        public static bool IsEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

        public static bool IsInt(this string text)
        {
            if (String.IsNullOrEmpty(text)) return false;
            int ret;
            return Int32.TryParse(text, out ret);
        }

        /// <summary>
        ///     A nicer way of calling the inverse of <see cref="System.String.IsNullOrEmpty(string)" />
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the value parameter is not null or an empty string (""); otherwise, false.</returns>
        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        ///     A nicer way of calling <see cref="System.String.IsNullOrEmpty(string)" />
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the sring value parameter is not null or only include white space("  "); otherwise, false.</returns>
        public static bool IsNotNullOrWhiteSpace(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        ///     A nicer way of calling <see cref="System.String.IsNullOrEmpty(string)" />
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        ///     A nicer way of calling <see cref="System.String.IsNullOrEmpty(string)" />
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the sring value parameter is null or only include white space("  "); otherwise, false.</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string Join(this List<string> items, string delimeter)
        {
            return String.Join(delimeter, items.ToArray());
        }

        /// <summary>
        ///     Limits the length of the <paramref name="source" /> to the specified <paramref name="maxLength" />.
        /// </summary>
        public static string Limit(this string source, int maxLength, string suffix = null)
        {
            if (suffix.IsNotNullOrEmpty())
            {
                // ReSharper disable once PossibleNullReferenceException
                maxLength = maxLength - suffix.Length;
            }

            if (source.Length <= maxLength)
            {
                return source;
            }

            return string.Concat(source.Substring(0, maxLength).Trim(), suffix ?? string.Empty);
        }

        /// <summary>
        ///     Allows for using strings in null coalescing operations
        /// </summary>
        /// <param name="value">The string value to check</param>
        /// <returns>Null if <paramref name="value" /> is empty or the original value of <paramref name="value" />.</returns>
        public static string NullIfEmpty(this string value)
        {
            if (value == string.Empty)
                return null;

            return value;
        }

        public static Dictionary<string, string> ParseKeyValueText(this string text, string delimiter = ":")
        {
            Dictionary<string, string> to = new Dictionary<string, string>();
            if (text == null) return to;

            foreach (string[] parts in text.ReadLines().Select(line => line.SplitOnFirst(delimiter)))
            {
                string key = parts[0].Trim();
                if (key.Length == 0) continue;
                to[key] = parts.Length == 2 ? parts[1].Trim() : null;
            }

            return to;
        }

        public static IEnumerable<string> ReadLines(this string text)
        {
            string line;
            StringReader reader = new StringReader(text ?? "");
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        public static string RemoveCharFlags(this string text, bool[] charFlags)
        {
            if (text == null) return null;

            char[] copy = text.ToCharArray();
            int nonWsPos = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char @char = text[i];
                if (@char < charFlags.Length && charFlags[@char]) continue;
                copy[nonWsPos++] = @char;
            }

            return new String(copy, 0, nonWsPos);
        }

        public static string ReplaceAll(this string haystack, string needle, string replacement)
        {
            int pos;
            // Avoid a possible infinite loop
            if (needle == replacement) return haystack;
            while ((pos = haystack.IndexOf(needle)) > 0)
            {
                haystack = haystack.Substring(0, pos)
                           + replacement
                           + haystack.Substring(pos + needle.Length);
            }
            return haystack;
        }

        public static string ReplaceFirst(this string haystack, string needle, string replacement)
        {
            int pos = haystack.IndexOf(needle);
            if (pos < 0) return haystack;

            return haystack.Substring(0, pos) + replacement + haystack.Substring(pos + needle.Length);
        }

        public static string SafeSubstring(this string value, int startIndex)
        {
            return SafeSubstring(value, startIndex, value.Length);
        }

        public static string SafeSubstring(this string value, int startIndex, int length)
        {
            if (String.IsNullOrEmpty(value)) return String.Empty;
            if (value.Length >= (startIndex + length))
                return value.Substring(startIndex, length);

            return value.Length > startIndex ? value.Substring(startIndex) : String.Empty;
        }

        /// <summary>
        ///     Separates a PascalCase string
        /// </summary>
        /// <example>
        ///     "ThisIsPascalCase".SeparatePascalCase(); // returns "This Is Pascal Case"
        /// </example>
        /// <param name="value">The value to split</param>
        /// <returns>The original string separated on each uppercase character.</returns>
        public static string SeparatePascalCase(this string value)
        {
            Argument.NotNullOrEmpty(value, "value");
            return Regex.Replace(value, "([A-Z])", " $1").Trim();
        }

        /// <summary>
        ///     Returns a string array containing the trimmed substrings in this <paramref name="value" />
        ///     that are delimited by the provided <paramref name="separators" />.
        /// </summary>
        public static IEnumerable<string> SplitAndTrim(this string value, params char[] separators)
        {
            Argument.NotNull(value, "source");
            return value.Trim().Split(separators, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
        }

        public static string[] SplitOnFirst(this string strVal, char needle)
        {
            if (strVal == null) return new string[0];
            int pos = strVal.IndexOf(needle);
            return pos == -1
                ? new[] { strVal }
                : new[] { strVal.Substring(0, pos), strVal.Substring(pos + 1) };
        }

        public static string[] SplitOnFirst(this string strVal, string needle)
        {
            if (strVal == null) return new string[0];
            int pos = strVal.IndexOf(needle);
            return pos == -1
                ? new[] { strVal }
                : new[] { strVal.Substring(0, pos), strVal.Substring(pos + 1) };
        }

        public static string[] SplitOnLast(this string strVal, char needle)
        {
            if (strVal == null) return new string[0];
            int pos = strVal.LastIndexOf(needle);
            return pos == -1
                ? new[] { strVal }
                : new[] { strVal.Substring(0, pos), strVal.Substring(pos + 1) };
        }

        public static string[] SplitOnLast(this string strVal, string needle)
        {
            if (strVal == null) return new string[0];
            int pos = strVal.LastIndexOf(needle);
            return pos == -1
                ? new[] { strVal }
                : new[] { strVal.Substring(0, pos), strVal.Substring(pos + 1) };
        }

        public static string SubString(this string source, int start, int count)
        {
            if (source.Length - count - start < 0)
            {
                return source.Substring(start);
            }
            return source.Substring(start, count);
        }

        public static string ToCamelCase(this string value)
        {
            if (String.IsNullOrEmpty(value)) return value;

            int len = value.Length;
            char[] newValue = new char[len];
            bool firstPart = true;

            for (int i = 0; i < len; ++i)
            {
                char c0 = value[i];
                char c1 = i < len - 1 ? value[i + 1] : 'A';
                bool c0isUpper = c0 >= 'A' && c0 <= 'Z';
                bool c1isUpper = c1 >= 'A' && c1 <= 'Z';

                if (firstPart && c0isUpper && (c1isUpper || i == 0))
                    c0 = (char)(c0 + LowerCaseOffset);
                else
                    firstPart = false;

                newValue[i] = c0;
            }

            return new string(newValue);
        }

        public static decimal ToDecimal(this string text)
        {
            return text == null ? default(decimal) : decimal.Parse(text);
        }

        public static decimal ToDecimal(this string text, decimal defaultValue)
        {
            decimal ret;
            return decimal.TryParse(text, out ret) ? ret : defaultValue;
        }

        public static double ToDouble(this string text)
        {
            return text == null ? default(double) : double.Parse(text);
        }

        public static double ToDouble(this string text, double defaultValue)
        {
            double ret;
            return double.TryParse(text, out ret) ? ret : defaultValue;
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T ToEnumOrDefault<T>(this string value, T defaultValue)
        {
            if (String.IsNullOrEmpty(value)) return defaultValue;
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static float ToFloat(this string text)
        {
            return text == null ? default(float) : float.Parse(text);
        }

        public static float ToFloat(this string text, float defaultValue)
        {
            float ret;
            return float.TryParse(text, out ret) ? ret : defaultValue;
        }

        public static int ToInt(this string text)
        {
            return text == null ? default(int) : Int32.Parse(text);
        }

        public static int ToInt(this string text, int defaultValue)
        {
            int ret;
            return Int32.TryParse(text, out ret) ? ret : defaultValue;
        }

        public static long ToInt64(this string text)
        {
            return Int64.Parse(text);
        }

        public static long ToInt64(this string text, long defaultValue)
        {
            long ret;
            return Int64.TryParse(text, out ret) ? ret : defaultValue;
        }

        public static string ToLowercaseUnderscore(this string value)
        {
            if (String.IsNullOrEmpty(value)) return value;
            value = value.ToCamelCase();

            StringBuilder sb = new StringBuilder(value.Length);
            foreach (char t in value)
            {
                if (Char.IsDigit(t) || (Char.IsLetter(t) && Char.IsLower(t)) || t == '_')
                {
                    sb.Append(t);
                }
                else
                {
                    sb.Append("_");
                    sb.Append(Char.ToLowerInvariant(t));
                }
            }
            return sb.ToString();
        }

        public static string ToMD5Hash(this string source)
        {
            MD5 md5 = MD5.Create();
            byte[] sourceBytes = Encoding.ASCII.GetBytes(source);
            byte[] hash = md5.ComputeHash(sourceBytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        public static string ToNullIfEmpty(this string text)
        {
            return String.IsNullOrEmpty(text) ? null : text;
        }

        public static string ToParentPath(this string path)
        {
            int pos = path.LastIndexOf('/');
            if (pos == -1) return "/";

            string parentPath = path.Substring(0, pos);
            return parentPath;
        }

        public static string ToRot13(this string value)
        {
            char[] array = value.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                int number = array[i];

                if (number >= 'a' && number <= 'z')
                    number += (number > 'm') ? -13 : 13;
                else if (number >= 'A' && number <= 'Z')
                    number += (number > 'M') ? -13 : 13;

                array[i] = (char)number;
            }
            return new string(array);
        }

        /// <summary>
        ///     Slugifies a string
        /// </summary>
        /// <param name="value">The string value to slugify</param>
        /// <param name="maxLength">An optional maximum length of the generated slug</param>
        /// <returns>A URL safe slug representation of the input <paramref name="value" />.</returns>
        public static string ToSlug(this string value, int? maxLength = null)
        {
            Argument.NotNull(value, "value");

            // if it's already a valid slug, return it
            if (RegexUtils.SlugRegex.IsMatch(value))
                return value;

            return GenerateSlug(value, maxLength);
        }

        /// <summary>
        ///     Converts a string into a slug that allows segments e.g.
        ///     <example>.blog/2012/07/01/title</example>
        ///     .
        ///     Normally used to validate user entered slugs.
        /// </summary>
        /// <param name="value">The string value to slugify</param>
        /// <returns>A URL safe slug with segments.</returns>
        public static string ToSlugWithSegments(this string value)
        {
            Argument.NotNull(value, "value");

            string[] segments = value.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string result = segments.Aggregate(string.Empty, (slug, segment) => slug + ("/" + segment.ToSlug()));
            return result.Trim('/');
        }

        public static byte[] ToUtf8Bytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static byte[] ToUtf8Bytes(this int intVal)
        {
            return FastToUtf8Bytes(intVal.ToString());
        }

        public static byte[] ToUtf8Bytes(this long longVal)
        {
            return FastToUtf8Bytes(longVal.ToString());
        }

        public static byte[] ToUtf8Bytes(this ulong ulongVal)
        {
            return FastToUtf8Bytes(ulongVal.ToString());
        }

        public static string TrimPrefixes(this string fromString, params string[] prefixes)
        {
            if (string.IsNullOrEmpty(fromString))
                return fromString;

            foreach (string prefix in prefixes)
            {
                if (fromString.StartsWith(prefix))
                    return fromString.Substring(prefix.Length);
            }

            return fromString;
        }

        public static string UrlDecode(this string text)
        {
            if (String.IsNullOrEmpty(text)) return null;

            List<byte> bytes = new List<byte>();

            int textLength = text.Length;
            for (int i = 0; i < textLength; i++)
            {
                char c = text[i];
                if (c == '+')
                {
                    bytes.Add(32);
                }
                else if (c == '%')
                {
                    byte hexNo = Convert.ToByte(text.Substring(i + 1, 2), 16);
                    bytes.Add(hexNo);
                    i += 2;
                }
                else
                {
                    bytes.Add((byte)c);
                }
            }

            byte[] byteArray = bytes.ToArray();
            return Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
        }

        public static string UrlEncode(this string text)
        {
            if (String.IsNullOrEmpty(text)) return text;

            StringBuilder sb = new StringBuilder();

            foreach (byte charCode in Encoding.UTF8.GetBytes(text))
            {
                if (
                    charCode >= 65 && charCode <= 90 // A-Z
                    || charCode >= 97 && charCode <= 122 // a-z
                    || charCode >= 48 && charCode <= 57 // 0-9
                    || charCode >= 44 && charCode <= 46 // ,-.
                    )
                {
                    sb.Append((char)charCode);
                }
                else if (charCode == 32)
                {
                    sb.Append('+');
                }
                else
                {
                    sb.Append('%' + charCode.ToString("x2"));
                }
            }

            return sb.ToString();
        }

        public static string UrlFormat(this string url, params string[] urlComponents)
        {
            string[] encodedUrlComponents = new string[urlComponents.Length];
            for (int i = 0; i < urlComponents.Length; i++)
            {
                string x = urlComponents[i];
                encodedUrlComponents[i] = x.UrlEncode();
            }

            return String.Format(url, encodedUrlComponents);
        }

        public static string WithHtmlUnderline(this string value)
        {
            return "<u> {0} </u>".FormatWith(value);
        }

        public static string WithoutExtension(this string filePath)
        {
            if (String.IsNullOrEmpty(filePath)) return null;

            int extPos = filePath.LastIndexOf('.');
            if (extPos == -1) return filePath;

            int dirPos = filePath.LastIndexOfAny(DirSeps);
            return extPos > dirPos ? filePath.Substring(0, extPos) : filePath;
        }

        public static string WithTrailingSlash(this string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            if (path[path.Length - 1] != '/')
            {
                return path + "/";
            }
            return path;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Skip the encoding process for 'safe strings'
        /// </summary>
        /// <param name="strVal"></param>
        /// <returns></returns>
        private static byte[] FastToUtf8Bytes(string strVal)
        {
            byte[] bytes = new byte[strVal.Length];
            for (int i = 0; i < strVal.Length; i++)
                bytes[i] = (byte)strVal[i];

            return bytes;
        }

        /// <summary>
        ///     Credit for this method goes to http://stackoverflow.com/questions/2920744/url-slugify-alrogithm-in-cs
        /// </summary>
        private static string GenerateSlug(string value, int? maxLength = null)
        {
            // prepare string, remove accents, lower case and convert hyphens to whitespace
            string result = RemoveAccent(value).Replace("-", " ").ToLowerInvariant();

            result = Regex.Replace(result, @"[^a-z0-9\s-]", string.Empty); // remove invalid characters
            result = Regex.Replace(result, @"\s+", " ").Trim(); // convert multiple spaces into one space

            if (maxLength.HasValue) // cut and trim
                result = result.Substring(0, result.Length <= maxLength ? result.Length : maxLength.Value).Trim();

            return Regex.Replace(result, @"\s", "-"); // replace all spaces with hyphens
        }

        private static string RemoveAccent(string value)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            return Encoding.ASCII.GetString(bytes);
        }

        #endregion Private Methods

        public static string Remove(this string value, string drop)
        {
            return value.Replace(drop, "");
        }

        public static string RemoveToFirst(this string value, char destination)
        {
            return value.RemoveToFirst(destination.ToString());
        }

        public static string RemoveToFirst(this string value, string destination)
        {
            int begin = value.IndexOf(destination, StringComparison.Ordinal);
            return begin == -1 ? value : value.Substring(begin + destination.Length);
        }

        public static string HideStringBalance(this string value, int tail = 4)
        {
            if (value.IsNullOrEmpty() || tail < 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            if ((value.Length / 3) >= tail)
            {
                sb.Append(value.GetFirst(tail));
                (value.Length - tail - tail).Times().Do(() => sb.Append("*"));
            }
            else
            {
                (value.Length - tail).Times().Do(() => sb.Append("*"));
            }
            sb.Append(value.GetLast(tail));
            return sb.ToString();
        }
    }
}