// FileInformation: nyanya/nyanya.Xingye/BankCardInfoResponse.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/02   3:29 PM

using Infrastructure.Lib.Utility;
using Xingye.Domain.Users.ReadModels;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     用户信息
    /// </summary>
    public class BankCardInfoResponse
    {
        /// <summary>
        ///     银行卡密码
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     银行名称
        /// </summary>
        public string CardBankName { get; set; }

        /// <summary>
        ///     是否是默认银行卡
        /// </summary>
        public bool IsDefault { get; set; }
    }

    internal static class BankCardSummaryInfoExtensions
    {
        internal static BankCardInfoResponse ToBankCardInfoResponse(this BankCardSummaryInfo info)
        {
            return new BankCardInfoResponse
            {
                BankCardNo = info.BankCardNo.ToStringIncludeNull(),
                CardBankName = info.BankName.ToStringIncludeNull(),
                IsDefault = info.IsDefault
            };
        }
    }
}