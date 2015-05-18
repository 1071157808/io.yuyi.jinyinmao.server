// FileInformation: nyanya/Infrastructure.Lib/SequenceNoUtils.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/26   1:57 PM

using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Infrastructure.Lib.Utility
{
    public static class SequenceNoUtils
    {
        #region Private Fields

        private static readonly char[] characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        private static readonly object lockObject = new object();
        private static readonly char machineKey = ConfigurationManager.AppSettings.Get("AppKey").First();

        #endregion Private Fields

        #region Public Methods

        public static string GenerateNo(char orderType)
        {
            // A => auth for yilian
            // B => add bank card for yilian
            // O => common order
            // S => common sequence number
            // R => common ZCBBillRedeem
            DateTime currentTime;
            lock (lockObject)
            {
                currentTime = DateTime.Now;
                Thread.Sleep(new TimeSpan(0, 0, 0, 0, 3));
            }

            int year = currentTime.Year - 2013;
            if (year > 35 || year < 0)
            {
                year = 0;
            }
            int month = currentTime.Month;
            int day = currentTime.Day;
            int hour = currentTime.Hour;

            string yearChar = characters[year].ToString(CultureInfo.InvariantCulture);
            string monthChar = characters[month].ToString(CultureInfo.InvariantCulture);
            string dayChar = characters[day].ToString(CultureInfo.InvariantCulture);
            string hourChar = characters[hour].ToString(CultureInfo.InvariantCulture);

            string time = DateTime.Now.ToString("mmssffff");
            StringBuilder sb = new StringBuilder();
            sb.Append(orderType).Append(machineKey).Append(yearChar).Append(monthChar).Append(dayChar).Append(hourChar).Append(time);
            return sb.ToString();
        }

        public static string GenerateNo(string orderType)
        {
            throw new Exception("请确定字符串长度");
            // A => auth for yilian
            // B => add bank card for yilian
            // O => common order
            // S => common sequence number
            // Z => common ZCBBill
            DateTime currentTime;
            lock (lockObject)
            {
                currentTime = DateTime.Now;
                Thread.Sleep(new TimeSpan(0, 0, 0, 0, 3));
            }

            int year = currentTime.Year - 2013;
            if (year > 35 || year < 0)
            {
                year = 0;
            }
            int month = currentTime.Month;
            int day = currentTime.Day;
            int hour = currentTime.Hour;

            string yearChar = characters[year].ToString(CultureInfo.InvariantCulture);
            string monthChar = characters[month].ToString(CultureInfo.InvariantCulture);
            string dayChar = characters[day].ToString(CultureInfo.InvariantCulture);
            string hourChar = characters[hour].ToString(CultureInfo.InvariantCulture);

            string time = DateTime.Now.ToString("mmssffff");
            StringBuilder sb = new StringBuilder();
            sb.Append(orderType).Append(machineKey).Append(yearChar).Append(monthChar).Append(dayChar).Append(hourChar).Append(time);
            return sb.ToString();
        }

        #endregion Public Methods
    }
}