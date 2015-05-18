// FileInformation: nyanya/Services.WebAPI.V1.nyanya/OrderShowingStatus.cs
// CreatedTime: 2014/08/08   10:13 AM
// LastUpdatedTime: 2014/08/09   6:20 PM

namespace Services.WebAPI.V1.nyanya.Models
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