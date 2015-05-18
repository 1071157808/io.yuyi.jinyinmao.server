// FileInformation: nyanya/nyanya.Cat/OrderShowingStatus.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/01   10:53 AM

namespace nyanya.Cat.Models
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
        ///     已起息/收益中
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