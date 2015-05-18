// FileInformation: nyanya/Cat.Events.Yilian/IForYilianRequest.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:28 PM

namespace Cat.Events.Yilian
{
    /// <summary>
    /// 易联请求接口
    /// </summary>
    public interface IForYilianRequest
    {
        /// <summary>
        ///     金额
        /// </summary>
        decimal Amount { get; }
        /// <summary>
        ///     银行卡号
        /// </summary>
        string BankCardNo { get; }
        /// <summary>
        ///     银行名称
        /// </summary>
        string BankName { get; }
        /// <summary>
        ///     手机号
        /// </summary>
        string Cellphone { get; }
        /// <summary>
        ///     开户城市名称
        /// </summary>
        string CityName { get; }
        /// <summary>
        ///     证件类型
        /// </summary>
        int CredentialCode { get; }
        /// <summary>
        ///     证件号
        /// </summary>
        string CredentialNo { get; }
        /// <summary>
        ///     订单标识
        /// </summary>
        string OrderIdentifier { get; }
        /// <summary>
        ///     产品标识
        /// </summary>
        string ProductIdentifier { get; }
        /// <summary>
        ///     产品标识
        /// </summary>
        string ProductNo { get; }
        /// <summary>
        ///     真实姓名
        /// </summary>
        string RealName { get; }
        /// <summary>
        ///     订单编号
        /// </summary>
        string SequenceNo { get; }
        /// <summary>
        ///     用户标识
        /// </summary>
        string UserIdentifier { get; }
    }
}