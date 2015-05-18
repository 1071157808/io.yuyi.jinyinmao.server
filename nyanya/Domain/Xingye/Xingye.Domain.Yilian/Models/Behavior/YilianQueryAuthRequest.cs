// FileInformation: nyanya/Xingye.Domain.Yilian/YilianQueryAuthRequest.cs
// CreatedTime: 2014/09/02   1:13 PM
// LastUpdatedTime: 2014/09/24   6:21 PM

using System;
using System.Threading.Tasks;
using Domian.Bus;
using Domian.Config;
using Infrastructure.Xingye.Gateway.Payment.Yilian;
using Infrastructure.Lib.Utility;
using Xingye.Domain.Yilian.Database;
using Xingye.Events.Yilian;
using Newtonsoft.Json.Linq;

namespace Xingye.Domain.Yilian.Models
{
    public partial class YilianQueryAuthRequest
    {
        public YilianQueryAuthRequest(string requestIdentifier)
        {
            this.RequestIdentifier = requestIdentifier;
        }

        private YilianQueryAuthRequest()
        {
        }

        private IEventBus eventbus
        {
            get { return CqrsDomainConfig.Configuration.EventBus; }
        }

        public void BuildQueryInfo(string message, string responseString, bool result)
        {
            Guard.IdentifierMustBeAssigned(this.RequestIdentifier, this.GetType().ToString());
            //Guard.ArgumentNotNull(this.QueryInfo,"QueryInfo");
            this.QueryInfo = new QueryInfo(this.RequestIdentifier)
            {
                Message = message,
                ResponseString = responseString,
                Result = result,
                QueryTime = DateTime.Now,
            };
        }

        public async Task SendQueryPaymentRequestAsync(string requestIdentifier, string orderIdentifier, string userIdentifier, string sequenceNo)
        {
            IYilianPaymentGatewayService service = new YilianPaymentGatewayService();
            RequestQueryResult result = await service.QueryRequestAsync(requestIdentifier, true);

            // 易联很坑爹，现在这样写，00A4 需要忽略
            if (result == null)
            {
                return;
            }

            await this.SaveResponseAsync(result.Message, result.ResponseString, result.Result);

            this.eventbus.Publish(new YilianQueryPaymentRequestProcessed(this.RequestIdentifier, this.GetType())
            {
                Message = result.Message,
                Result = result.Result,
                SequenceNo = sequenceNo,
                UserIdentifier = userIdentifier,
                OrderIdentifier = orderIdentifier
            });
        }

        internal async Task SendQueryAuthRequestAsync(string requestIdentifier, string userIdentifier, string sequenceNo)
        {
            IYilianPaymentGatewayService service = new YilianPaymentGatewayService();
            RequestQueryResult result = await service.QueryRequestAsync(requestIdentifier, false);

            // 易联很坑爹，现在这样写，00A4 需要忽略
            if (result == null)
            {
                return;
            }

            await this.SaveResponseAsync(result.Message, result.ResponseString, result.Result);

            this.eventbus.Publish(new YilianQueryAuthRequestProcessed(this.RequestIdentifier, this.GetType())
            {
                Message = result.Message,
                Result = result.Result,
                SequenceNo = sequenceNo,
                UserIdentifier = userIdentifier
            });
        }

        protected async Task SaveResponseAsync(string message, string responseString, bool result)
        {
            Guard.IdentifierMustBeAssigned(this.RequestIdentifier, this.GetType().ToString());
            this.BuildQueryInfo(message, responseString, result);

            using (YilianContext context = new YilianContext())
            {
                await context.SaveAsync(this.QueryInfo);
            }
        }
    }
}