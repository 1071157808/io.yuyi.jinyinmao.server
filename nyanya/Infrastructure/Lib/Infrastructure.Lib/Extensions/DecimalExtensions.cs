// FileInformation: nyanya/Infrastructure.Lib/DecimalExtensions.cs
// CreatedTime: 2014/06/23   2:03 PM
// LastUpdatedTime: 2014/08/10   12:19 PM

using System;
using System.Globalization;

namespace Infrastructure.Lib.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal RoundScale(this decimal d, int min, int max)
        {
            if (min >= max)
            {
                throw new ArgumentException("max must greater than min");
            }

            return decimal.Round(d, d - decimal.Round(d, min) == 0 ? min : max);
        }

        public static decimal ToIntFormat(this decimal d)
        {
            return decimal.Round(d);
        }

        public static decimal ToFloor(this decimal d,int decimals)
        {
            decimal tDecimals = new decimal(Math.Pow(10, decimals));
            int temp = Convert.ToInt32(Math.Floor(decimal.Multiply(d,tDecimals)));
            return temp / tDecimals;
        }
    }
}