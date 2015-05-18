// FileInformation: nyanya/nyanya.Xingye/OrderShowingStatus.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/01   10:48 AM

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     OrderShowingStatus
    /// </summary>
    public enum OrderShowingStatus
    {
        /// <summary>
        ///     付款中
        /// </summary>
        Paying = 10,

        /// <summary>
        ///     待起息
        /// </summary>
        WaitForInterest = 20,

        /// <summary>
        ///     已起息
        /// </summary>
        Interesting = 30,

        /// <summary>
        ///     已结息
        /// </summary>
        Interested = 40,

        /// <summary>
        ///     支付失败
        /// </summary>
        Failed = 50
    }
}