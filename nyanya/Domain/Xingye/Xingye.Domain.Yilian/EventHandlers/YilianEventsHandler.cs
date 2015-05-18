// FileInformation: nyanya/Xingye.Domain.Yilian/YilianEventsHandler.cs
// CreatedTime: 2014/09/02   1:13 PM
// LastUpdatedTime: 2014/09/03   10:31 AM

using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domian.Config;
using Domian.Events;
using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Extensions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Xingye.Domain.Yilian.Database;
using Xingye.Domain.Yilian.Models;
using Xingye.Events.Yilian;

namespace Xingye.Domain.Yilian.EventHandlers
{
    public class YilianEventsHandler : EventHandlerBase,
        IEventHandler<YilianAuthRequestCallbackReceived>,
        IEventHandler<YilianPaymentRequestCallbackReceived>
    {
        public YilianEventsHandler(CqrsConfiguration config)
            : base(config)
        {
        }

        #region IEventHandler<YilianAuthRequestCallbackReceived> Members

        public async Task Handler(YilianAuthRequestCallbackReceived @event)
        {
            await this.DoAsync(this.ProcessYilianRequestRepliedEvent, @event);
        }

        #endregion IEventHandler<YilianAuthRequestCallbackReceived> Members

        #region IEventHandler<YilianPaymentRequestCallbackReceived> Members

        public async Task Handler(YilianPaymentRequestCallbackReceived @event)
        {
            await this.DoAsync(this.ProcessYilianRequestRepliedEvent, @event);
        }

        #endregion IEventHandler<YilianPaymentRequestCallbackReceived> Members

        private async Task ProcessYilianRequestRepliedEvent(YilianAuthRequestCallbackReceived @event)
        {
            JObject reply = this.ValidateJsonSchema(@event.Reply);

            string requestIdentifier = reply.GetValue("ORDER_NO").Value<string>();

            YilianAuthRequest request;
            using (YilianContext context = new YilianContext())
            {
                request = context.ReadonlyQuery<YilianAuthRequest>().Include(r => r.RequestInfo).FirstOrDefault(b => b.RequestIdentifier == requestIdentifier);
            }

            if (request == null)
            {
                throw new ApplicationBusinessException("YilianAuthRequestCallbackReceived can not be found with requestIdentifier {0}".FormatWith(requestIdentifier));
            }

            await request.ProcessTheReplyAsync(reply);
        }

        private async Task ProcessYilianRequestRepliedEvent(YilianPaymentRequestCallbackReceived @event)
        {
            JObject reply = this.ValidateJsonSchema(@event.Reply);

            string requestIdentifier = reply.GetValue("ORDER_NO").Value<string>();

            YilianPaymentRequest request;
            using (YilianContext context = new YilianContext())
            {
                request = context.ReadonlyQuery<YilianPaymentRequest>().Include(r => r.RequestInfo).FirstOrDefault(b => b.RequestIdentifier == requestIdentifier);
            }

            if (request == null)
            {
                throw new ApplicationBusinessException("YilianAuthRequestCallbackReceived can not be found with requestIdentifier {0}".FormatWith(requestIdentifier));
            }

            await request.ProcessTheReplyAsync(reply);
        }

        private JObject ValidateJsonSchema(string reply)
        {
            //{"Reply":"{\"ACCOUNT_NO\":\"6222021001128790018\",\"AMOUNT\":\"1.01\",\"CURRENCY\":\"\",\"ENCODING\":\"\",\"MAC\":\"\","
            //         "\"MERCHANT_NO\":\"\",\"MER_ORDER_NO\":\"\",\"MOBILE_NO\":\"15800780728\",\"ORDER_NO\":\"D62F451F13F34473BDAC0147887C9333\","
            //         "\"RESP_CODE\":\"0000\",\"RESP_REMARK\":\"交易成功\",\"SETT_AMOUNT\":\"1.01\",\"SN\":\"ACBH4S15500234\",\"TRANS_DESC\":\"\","
            //         "\"YL_BATCH_NO\":\"\",\"createTm\":null,\"id\":0,\"version\":0}",
            //    "EventId":"4d18e7dcdd8b4517b79e0147887ee825","SourceId":"YL9bed2b1dfa2f4e34a5420147887ee81b"}}
            if (reply.IsNullOrEmpty())
            {
                throw new BusinessValidationFailedException("YilianRequestCallbackReceived is empty.");
            }

            string schemaString = @"{
            'description': 'Yilian Auth Callback',
            'type': 'object',
            'properties':
            {
                'ACCOUNT_NO':{'type':'string'},
                'AMOUNT':{'type':'string'},
                'CURRENCY':{'type':'string'},
                'ENCODING':{'type':'string'},
                'MAC':{'typr':'string'},
                'MERCHANT_NO':{'typr':'string'},
                'MER_ORDER_NO':{'typr':'string'},
                'MOBILE_NO':{'typr':'string'},
                'ORDER_NO':{'typr':'string'},
                'RESP_CODE':{'type':'string'},
                'RESP_REMARK':{'type':'string'},
                'SETT_AMOUNT':{'type':'string'},
                'SN':{'type':'string'},
                'TRANS_DESC':{'type':'string'},
                'YL_BATCH_NO':{'type':'string'},
                'createTm':{'type':['string','null']},
                'id':{'type':'integer'},
                'version':{'type':'integer'}
            }}";

            JsonSchema schema = JsonSchema.Parse(schemaString);

            JObject replyJson = JObject.Parse(reply);

            if (!replyJson.IsValid(schema))
            {
                throw new BusinessValidationFailedException("YilianRequestCallbackReceived json bad format");
            }
            return replyJson;
        }
    }
}