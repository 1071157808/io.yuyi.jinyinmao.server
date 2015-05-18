// FileInformation: nyanya/Services.WebAPI.V1.nyanya/BankCardInfoResponse.cs
// CreatedTime: 2014/08/09   2:24 PM
// LastUpdatedTime: 2014/08/09   2:32 PM

using Cqrs.Domain.Users.ReadModels;
using Infrastructure.Lib.Utility;

namespace Services.WebAPI.V1.nyanya.Models
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