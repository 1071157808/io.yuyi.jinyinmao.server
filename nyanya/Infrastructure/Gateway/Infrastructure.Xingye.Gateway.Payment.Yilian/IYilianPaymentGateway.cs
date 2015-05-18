// FileInformation: nyanya/Infrastructure.Xingye.Gateway.Payment.Yilian/IYilianPaymentGateway.cs
// CreatedTime: 2014/09/30   4:07 PM
// LastUpdatedTime: 2014/09/30   4:21 PM

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Xingye.Gateway.Payment.Yilian
{
    public interface IYilianPaymentGatewayService : IDisposable
    {
        #region Public Methods

        Task<PaymentRequestResult> PaymentRequestAsync(PaymentRequestParameter parameter);

        Task<RequestQueryResult> QueryRequestAsync(string batchNo, bool isPayment);

        Task<UserAuthRequestResult> UserAuthRequestAsync(UserAuthRequestParameter parameter);

        #endregion Public Methods
    }

    public static class YilianRequestParameterHelper
    {
        #region Public Methods

        public static string TransformCredentialType(int idType)
        {
            switch (idType)
            {
                case 0:
                    return "0";

                case 1:
                    return "2";

                case 2:
                    return "6";

                case 3:
                    return "3";

                default:
                    return "X";
            }
        }

        #endregion Public Methods
    }

    // ReSharper disable InconsistentNaming
    public class PaymentRequestParameter
    {
        #region Public Constructors

        public PaymentRequestParameter(string bn, string sn, string accNo, string accName, string accProvince, string accCity, string bankName,
            int idType, string idNo, string mobileNo, string userId, string pn, decimal amount)
        {
            this.BATCH_NO = bn.ToUpper();

            TransDetail tran = new TransDetail
            {
                USER_UUID = userId.ToUpper(),
                SN = sn,
                ACC_NO = accNo,
                // 账号名
                ACC_NAME = accName,
                // 省份
                ACC_PROVINCE = accProvince,
                // 城市名
                ACC_CITY = accCity,
                AMOUNT = decimal.Round(amount, 2, MidpointRounding.AwayFromZero).ToString(),
                // 银行名
                BANK_NAME = bankName,
                // 证件类型
                ID_TYPE = YilianRequestParameterHelper.TransformCredentialType(idType),
                // 证件号码
                ID_NO = idNo,
                // 手机号
                MOBILE_NO = mobileNo,
                // 回调URL
                MERCHANT_URL = "",
                // 产品编号
                MER_ORDER_NO = pn
            };

            this.TRANS_DETAILS = new List<TransDetail>();
            this.TRANS_DETAILS.Add(tran);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        ///     “BATCH_NO 批次号”须保证唯一性
        /// </summary>
        public string BATCH_NO { get; set; }

        public List<TransDetail> TRANS_DETAILS { get; set; }

        #endregion Public Properties
    }

    public class PaymentRequestResult : YilianRequestResultBase
    {
    }

    public class QueryRequestParmeter
    {
        #region Public Properties

        public string BATCH_NO { get; set; }

        #endregion Public Properties
    }

    public class RequestQueryResult : YilianRequestResultBase
    {
    }

    public class TransDetail
    {
        #region Public Properties

        /// <summary>
        ///     开户城市
        /// </summary>
        public string ACC_CITY { get; set; }

        /// <summary>
        ///     账户名
        /// </summary>
        public string ACC_NAME { get; set; }

        /// <summary>
        ///     账号 19位借记卡号
        /// </summary>
        public string ACC_NO { get; set; }

        /// <summary>
        ///     开户省份
        /// </summary>
        public string ACC_PROVINCE { get; set; }

        /// <summary>
        ///     金额（即认证费，由金银猫自己生成，可以是动态的每次都不一样，也可以是写死的每次都一样）
        /// </summary>
        public string AMOUNT { get; set; }

        /// <summary>
        ///     支行名称（即银行名称）
        /// </summary>
        public string BANK_NAME { get; set; }

        /// <summary>
        ///     币值
        /// </summary>
        public string CNY
        {
            get { return "CNY"; }
        }

        /// <summary>
        ///     开户证件号
        /// </summary>
        public string ID_NO { get; set; }

        /// <summary>
        ///     开户证件类型
        /// </summary>
        public string ID_TYPE { get; set; }

        /// <summary>
        ///     产品编号
        /// </summary>
        public string MER_ORDER_NO { get; set; }

        /// <summary>
        ///     回调URL
        /// </summary>
        public string MERCHANT_URL { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        public string MOBILE_NO { get; set; }

        /// <summary>
        ///     “SN 流水号”须保证唯一性,总长6——14位, 有字母要用大写
        /// </summary>
        public string SN { get; set; }

        public string TRANS_DESC
        {
            get { return "兴业票快捷支付认证开通扣款 此费用稍后将返还到您的认证卡里 兴业票客服 4008556333"; }
        }

        /// <summary>
        ///     用户uuid
        /// </summary>
        public string USER_UUID { get; set; }

        #endregion Public Properties
    }

    public class UserAuthRequestParameter
    {
        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserAuthRequestParameter" /> class.
        /// </summary>
        /// <param name="bn">The bn.</param>
        /// <param name="sn">The sn.</param>
        /// <param name="accNo">The acc no.</param>
        /// <param name="accName">Name of the acc.</param>
        /// <param name="accProvince">The acc province.</param>
        /// <param name="accCity">The acc city.</param>
        /// <param name="bankName">Name of the bank.</param>
        /// <param name="idType">Type of the identifier.</param>
        /// <param name="idNo">The identifier no.</param>
        /// <param name="mobileNo">The mobile no.</param>
        /// <param name="userId">The user identifier.</param>
        public UserAuthRequestParameter(string bn, string sn, string accNo, string accName, string accProvince, string accCity, string bankName,
            int idType, string idNo, string mobileNo, string userId)
        {
            this.BATCH_NO = bn.ToUpper();

            TransDetail tran = new TransDetail
            {
                USER_UUID = userId.ToUpper(),
                SN = sn,
                ACC_NO = accNo,
                // 账号名
                ACC_NAME = accName,
                // 省份
                ACC_PROVINCE = accProvince,
                // 城市名
                ACC_CITY = accCity,
                AMOUNT = "1.08",
                // 银行名
                BANK_NAME = bankName,
                // 证件类型
                ID_TYPE = YilianRequestParameterHelper.TransformCredentialType(idType),
                // 证件号码
                ID_NO = idNo,
                // 手机号
                MOBILE_NO = mobileNo,
                // 回调URL
                MERCHANT_URL = "",
                // 模拟产品编号
                MER_ORDER_NO = "A" + userId.ToUpper()
            };

            this.TRANS_DETAILS = new List<TransDetail>();
            this.TRANS_DETAILS.Add(tran);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        ///     “BATCH_NO 批次号”须保证唯一性
        /// </summary>
        public string BATCH_NO { get; set; }

        public List<TransDetail> TRANS_DETAILS { get; set; }

        #endregion Public Properties
    }

    public class UserAuthRequestResult : YilianRequestResultBase
    {
    }

    public class YilianRequestResultBase
    {
        #region Public Properties

        public string Message { get; set; }

        public string ResponseString { get; set; }

        public bool Result { get; set; }

        #endregion Public Properties
    }

    // ReSharper restore InconsistentNaming
}