// FileInformation: nyanya/Xingye.Events.Yilian/IForYilianRequest.cs
// CreatedTime: 2014/09/02   10:16 AM
// LastUpdatedTime: 2014/09/02   10:42 AM

using ServiceStack;

namespace Xingye.Events.Yilian
{
    public interface IForYilianRequest
    {
        decimal Amount { get; }

        string BankCardNo { get; }

        string BankName { get; }

        string Cellphone { get; }

        string CityName { get; }

        int CredentialCode { get; }

        string CredentialNo { get; }

        string OrderIdentifier { get; }

        string ProductIdentifier { get; }

        string ProductNo { get; }

        string RealName { get; }

        string SequenceNo { get; }

        string UserIdentifier { get; }

        RequestType YilianType { get; }

    }
    /// <summary>
    /// 易联请求类型
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// 认证
        /// </summary>
        Auth = 0,
        /// <summary>
        /// 订单
        /// </summary>
        Order = 1
    }
}