// FileInformation: nyanya/Xingye.Domain.Yilian/YilianAuthRequest.cs
// CreatedTime: 2014/09/02   1:13 PM
// LastUpdatedTime: 2014/09/03   10:31 AM

using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class YilianAuthRequest
    {
        private IEventBus eventbus
        {
            get { return CqrsDomainConfig.Configuration.EventBus; }
        }

        private IEnumerable<string> SuccessfulCodes
        {
            get { return new[] { "0000", "0051", "T425", "T212", "U011" }; }
        }

        internal async Task ProcessTheReplyAsync(JObject reply)
        {
            Guard.IdentifierMustBeAssigned(this.RequestIdentifier, this.GetType().ToString());
            Guard.ArgumentNotNull(this.RequestInfo, "RequestInfo");
            string respCode = reply.GetValue("RESP_CODE").Value<string>().ToUpperInvariant();
            string respRemark = YinlianErrorCodeCache.TryGetErrorMessage(respCode);
            if (string.IsNullOrEmpty(respRemark))
                respRemark = reply.GetValue("RESP_REMARK").Value<string>();
            CallbackInfo info = new CallbackInfo
            {
                CallbackString = reply.ToString(),
                CallbackTime = DateTime.Now,
                Message = respRemark,
                RequestIdentifier = this.RequestIdentifier,
                Result = this.SuccessfulCodes.Contains(respCode)
            };

            using (YilianContext context = new YilianContext())
            {
                await context.SaveAsync(info);
            }

            this.eventbus.Publish(new YilianAuthRequestCallbackProcessed(this.RequestIdentifier, this.GetType())
            {
                Message = info.Message,
                Result = info.Result,
                SequenceNo = this.SequenceNo,
                UserIdentifier = this.UserIdentifier
            });
        }

        internal async Task SendRequestAsync()
        {
            UserAuthRequestParameter parameter = this.BuildRequestParameter();
            IYilianPaymentGatewayService service = new YilianPaymentGatewayService();
            UserAuthRequestResult result = await service.UserAuthRequestAsync(parameter);
            await this.AddGatewayResponseAsync(result.Message, result.ResponseString, result.Result);
            this.eventbus.Publish(new YilianAuthRequestSended(this.RequestIdentifier, this.GetType())
            {
                Message = "未知错误，请稍后再试",
                Result = result.Result,
                SequenceNo = this.SequenceNo,
                UserIdentifier = this.UserIdentifier
            });
        }

        /// <summary>
        ///     Builds the request parameter.
        /// </summary>
        /// <returns></returns>
        private UserAuthRequestParameter BuildRequestParameter()
        {
            Guard.IdentifierMustBeAssigned(this.RequestIdentifier, this.GetType().ToString());
            Guard.ArgumentNotNull(this.RequestInfo, "RequestInfo");
            return new UserAuthRequestParameter(this.RequestIdentifier, this.SequenceNo,
                this.RequestInfo.AccountNo, this.RequestInfo.AccountName, this.RequestInfo.Province, this.RequestInfo.City, this.RequestInfo.BankName,
                this.RequestInfo.IdType, this.RequestInfo.IdNo, this.RequestInfo.MobileNo, this.RequestInfo.UserIdentifier);
        }
    }
}
