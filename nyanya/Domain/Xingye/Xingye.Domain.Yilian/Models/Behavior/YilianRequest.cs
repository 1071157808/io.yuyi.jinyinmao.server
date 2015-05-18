// FileInformation: nyanya/Xingye.Domain.Yilian/YilianRequest.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:34 PM

using System;
using System.Linq;
using System.Threading.Tasks;
using Xingye.Domain.Yilian.Database;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using Xingye.Events.Yilian;

namespace Xingye.Domain.Yilian.Models
{
    public partial class YilianRequest
    {
        internal virtual async Task CreateFromEvent(IForYilianRequest @event)
        {
            this.FillWithEventData(@event);

            using (YilianContext context = new YilianContext())
            {
                await context.SaveAsync(this);
            }
        }

        protected async Task AddGatewayResponseAsync(string message, string responseString, bool result)
        {
            Guard.IdentifierMustBeAssigned(this.RequestIdentifier, this.GetType().ToString());
            GatewayResponse gatewayResponse = new GatewayResponse
            {
                Message = message,
                ResponseString = responseString,
                Result = result,
                SendingTime = DateTime.Now,
                RequestIdentifier = this.RequestIdentifier
            };

            using (YilianContext context = new YilianContext())
            {
                await context.SaveAsync(gatewayResponse);
            }
        }

        protected virtual RequestInfo BuildRequestInfo(IForYilianRequest @event)
        {
            Guard.IdentifierMustBeAssigned(this.RequestIdentifier, this.GetType().ToString());

            return new RequestInfo
            {
                AccountNo = @event.BankCardNo,
                AccountName = @event.RealName,
                BankName = @event.BankName,
                City = @event.CityName.Split(new[] { '|' }).Last(),
                IdNo = @event.CredentialNo,
                IdType = @event.CredentialCode,
                MobileNo = @event.Cellphone,
                Province = @event.CityName.Split(new[] { '|' }).First(),
                RequestIdentifier = this.RequestIdentifier,
                UserIdentifier = @event.UserIdentifier,
                ProductNo = @event.ProductNo,
                ProductIdentifier = @event.ProductIdentifier,
                OrderIdentifier = @event.OrderIdentifier
            };
        }

        protected virtual YilianRequest FillWithEventData(IForYilianRequest @event)
        {
            this.CallbackInfo = null;
            this.CreateTime = DateTime.Now;
            this.GatewayResponse = null;
            this.QueryInfo = null;
            this.RequestInfo = null;
            this.SequenceNo = @event.SequenceNo;
            this.TypeCode = @event.SequenceNo.GetFirst();
            this.UserIdentifier = @event.UserIdentifier;
            this.RequestInfo = this.BuildRequestInfo(@event);
            this.Amount = @event.Amount;
            this.IsPayment = @event.YilianType != RequestType.Auth;

            return this;
        }
    }
}