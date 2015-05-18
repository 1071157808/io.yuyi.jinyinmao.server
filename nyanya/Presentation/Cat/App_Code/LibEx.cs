using System;

namespace Cat
{
    public static class LibEx
    {
        #region Public Methods

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

        public static decimal RoundScale(this decimal d, int min, int max)
        {
            if (min >= max)
            {
                throw new ArgumentException("max must greater than min");
            }

            return decimal.Round(d, d - decimal.Round(d, min) == 0 ? min : max);
        }

        public static string SubString(this string source, int start, int count)
        {
            if (source.Length - count - start < 0)
            {
                return source.Substring(start);
            }
            return source.Substring(start, count);
        }

        public static decimal ToIntFormat(this decimal d)
        {
            return decimal.Round(d);
        }

        public static int ToIntSafely(this object value, int defaultValue = 0)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static decimal ToDecimalSafely(this object value, decimal defaultValue = 0)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion Public Methods
    }
}