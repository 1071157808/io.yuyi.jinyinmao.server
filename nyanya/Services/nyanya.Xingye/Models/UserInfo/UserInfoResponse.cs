// FileInformation: nyanya/nyanya.Xingye/UserInfoResponse.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/02   3:29 PM

using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using nyanya.AspDotNet.Common.ResponseModels;
using Xingye.Domain.Users.Helper;
using Xingye.Domain.Users.ReadModels;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     用户信息
    /// </summary>
    public class UserInfoResponse : IResponseModel
    {
        /// <summary>
        ///     默认银行卡密码
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     银行卡数量
        /// </summary>
        public int BankCardsCount { get; set; }

        /// <summary>
        ///     默认银行卡名称
        /// </summary>
        public string CardBankName { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        ///     证件
        /// </summary>
        public int Credential { get; set; }

        /// <summary>
        ///     证件编号
        /// </summary>
        public string CredentialNo { get; set; }

        /// <summary>
        ///     用户是否绑定了默认银行卡，也即是是否开通了手机支付
        /// </summary>
        public bool HasDefaultBankCard
        {
            get { return this.BankCardNo.IsNotNullOrEmpty(); }
        }

        /// <summary>
        ///     是否设定了支付密码
        /// </summary>
        public bool HasSetPaymentPassword { get; set; }

        /// <summary>
        ///     是否有银生宝的信息
        /// </summary>
        public bool HasYSBInfo
        {
            get { return !StringEx.IsNullOrWhiteSpace(this.YSBRealName); }
        }

        /// <summary>
        ///     登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        ///     用户真实姓名.
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        ///     用户注册时间
        /// </summary>
        public string SignUpTime { get; set; }

        /// <summary>
        ///     用户是否通过实名认证.
        /// </summary>
        public bool Verified { get; set; }

        /// <summary>
        ///     用户是否正在进行实名认证.
        /// </summary>
        public bool Verifing { get; set; }

        /// <summary>
        ///     银生宝认证的真实身份证号
        /// </summary>
        public string YSBIdCard { get; set; }

        /// <summary>
        ///     银生宝的认证的真实姓名
        /// </summary>
        public string YSBRealName { get; set; }

        /// <summary>
        ///     用户是否有登陆密码
        /// </summary>
        public bool HasLoginPwd { get; set; }
    }

    internal static class UserInfoExtensions
    {
        internal static UserInfoResponse ToUserInfoResponse(this UserInfo userInfo, bool hasLoginPwd)
        {
            return new UserInfoResponse
            {
                Cellphone = userInfo.Cellphone.ToStringIncludeNull(),
                Credential = CredentialHelper.GetCredentialTypeCode(userInfo.Credential),
                CredentialNo = userInfo.CredentialNo.HideStringBalance(),
                BankCardNo = userInfo.BankCardNo.ToStringIncludeNull(),
                CardBankName = userInfo.CardBankName.ToStringIncludeNull(),
                HasSetPaymentPassword = userInfo.HasSetPaymentPassword,
                LoginName = userInfo.LoginName.ToStringIncludeNull(),
                RealName = userInfo.RealName.ToStringIncludeNull(),
                SignUpTime = userInfo.SignUpTime.ToMeowFormat(),
                YSBIdCard = userInfo.YSBIdCard.ToStringIncludeNull(),
                YSBRealName = userInfo.YSBRealName.ToStringIncludeNull(),
                Verified = userInfo.Verified.GetValueOrDefault(false),
                Verifing = userInfo.Verified == false,
                BankCardsCount = userInfo.BankCardsCount,
                HasLoginPwd = hasLoginPwd
            };
        }
    }
}