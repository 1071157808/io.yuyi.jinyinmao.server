// FileInformation: nyanya/Services.WebAPI.V1.nyanya/BankCardHelper.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/28   10:38 AM

namespace Services.WebAPI.V1.nyanya.Helper
{
    /// <summary>
    ///     BankCardHelper
    /// </summary>
    public static class BankCardHelper
    {
        #region Public Methods

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
                case "招商银行":
                    return amount <= 50000;

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
                    return amount <= 1000000;

                case "兴业银行":
                case "浦发银行":
                    return amount <= 20000;

                default:
                    return false;
            }
        }

        #endregion Public Methods
    }
}