// FileInformation: nyanya/nyanya.Meow/BankCardHelper.cs
// CreatedTime: 2014/08/29   2:39 PM
// LastUpdatedTime: 2014/08/29   2:40 PM

namespace nyanya.Meow.Helper
{
    /// <summary>
    ///     BankCardHelper
    /// </summary>
    public static class BankCardHelper
    {
        /// <summary>
        ///     Checks the daily limit.
        /// </summary>
        /// <param name="bankName">Name of the bank.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static bool CheckDailyLimit(string bankName, decimal amount)
        {
            switch (bankName)
            {
                case "工商银行":
                    return amount <= 50000;

                case "招商银行":
                case "广发银行":
                case "广州银行":
                case "深发银行":
                case "建设银行":
                case "农业银行":
                case "邮储银行":
                case "光大银行":
                case "华夏银行":
                case "平安银行":
                case "中信银行":
                case "民生银行":
                case "海南农信社":
                case "广州农商行":
                case "中国银行":
                    return amount <= 1000000;

                case "兴业银行":
                    return amount <= 500000;

                case "浦发银行":
                    return amount <= 20000;

                case "富滇银行":
                    return amount <= 999;

                default:
                    return false;
            }
        }
    }
}
