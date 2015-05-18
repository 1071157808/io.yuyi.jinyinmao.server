// FileInformation: nyanya/Xingye.Domain.Yilian/YilianPaymentRequest.cs
// CreatedTime: 2014/09/02   1:13 PM
// LastUpdatedTime: 2014/09/03   10:31 AM

using System;
using System.Threading.Tasks;
using Domian.Bus;
using Domian.Config;
using Infrastructure.Xingye.Gateway.Payment.Yilian;
using Infrastructure.Lib.Utility;
using Newtonsoft.Json.Linq;
using Xingye.Domain.Yilian.Database;
using Xingye.Events.Yilian;
using Infrastructure.Cache.Couchbase;

namespace Xingye.Domain.Yilian.Models
{
    public partial class YilianPaymentRequest
    {
        private IEventBus eventbus
        {
            get { return CqrsDomainConfig.Configuration.EventBus; }
        }

        internal async Task ProcessTheReplyAsync(JObject reply)
        {
            Guard.IdentifierMustBeAssigned(this.RequestIdentifier, this.GetType().ToString());
            Guard.ArgumentNotNull(this.RequestInfo, "RequestInfo");
            string respCode = reply.GetValue("RESP_CODE").Value<string>().ToUpperInvariant();
            string respRemark = YinlianErrorCodeCache.TryGetErrorMessage(respCode);
            if (string.IsNullOrEmpty(respRemark))
                respRemark = reply.GetValue("RESP_REMARK").Value<string>();
            decimal amount = reply.GetValue("AMOUNT").Value<decimal>();

            CallbackInfo info = new CallbackInfo
            {
                CallbackString = reply.ToString(),
                CallbackTime = DateTime.Now,
                Message = respRemark,
                RequestIdentifier = this.RequestIdentifier,
                Result = respCode == "0000" && amount == this.Amount
            };

            using (YilianContext context = new YilianContext())
            {
                await context.SaveAsync(info);
            }

            this.eventbus.Publish(new YilianPaymentRequestCallbackProcessed(this.RequestIdentifier, this.GetType())
            {
                Message = info.Message,
                Result = info.Result,
                SequenceNo = this.SequenceNo,
                UserIdentifier = this.UserIdentifier,
                OrderIdentifier = this.RequestInfo.OrderIdentifier
            });
        }

        internal async Task SendRequestAsync()
        {
            PaymentRequestParameter parameter = this.BuildRequestParameter();
            IYilianPaymentGatewayService service = new YilianPaymentGatewayService();
            PaymentRequestResult result = await service.PaymentRequestAsync(parameter);
            await this.AddGatewayResponseAsync(result.Message, result.ResponseString, result.Result);
            this.eventbus.Publish(new YilianPaymentRequestSended(this.RequestIdentifier, this.GetType())
            {
                Message = result.Message,
                Result = result.Result,
                SequenceNo = this.SequenceNo,
                UserIdentifier = this.UserIdentifier,
                OrderIdentifier = this.RequestInfo.OrderIdentifier
            });
        }

        /// <summary>
        ///     Builds the request parameter.
        /// </summary>
        /// <returns></returns>
        private PaymentRequestParameter BuildRequestParameter()
        {
            Guard.IdentifierMustBeAssigned(this.RequestIdentifier, this.GetType().ToString());
            Guard.ArgumentNotNull(this.RequestInfo, "RequestInfo");
            return new PaymentRequestParameter(this.RequestIdentifier, this.SequenceNo,
                this.RequestInfo.AccountNo, this.RequestInfo.AccountName, this.RequestInfo.Province, this.RequestInfo.City, this.RequestInfo.BankName,
                this.RequestInfo.IdType, this.RequestInfo.IdNo, this.RequestInfo.MobileNo, this.RequestInfo.UserIdentifier, this.RequestInfo.ProductNo, this.Amount);
        }
    }
}
