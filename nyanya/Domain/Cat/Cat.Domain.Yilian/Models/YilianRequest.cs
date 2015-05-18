// FileInformation: nyanya/Cat.Domain.Yilian/YilianRequest.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:24 PM

using System;
using System.Data.Entity;
using System.Linq;
using Cat.Domain.Yilian.Database;
using Domian.Events;
using Domian.Models;
using ServiceStack;

namespace Cat.Domain.Yilian.Models
{
    public partial class YilianRequest : IAggregateRoot, IHasMemento
    {
        public decimal Amount { get; set; }

        public CallbackInfo CallbackInfo { get; set; }

        public DateTime CreateTime { get; set; }

        public GatewayResponse GatewayResponse { get; set; }

        public bool IsPayment { get; set; }

        public QueryInfo QueryInfo { get; set; }

        public string RequestIdentifier { get; set; }

        public RequestInfo RequestInfo { get; set; }

        public string SequenceNo { get; set; }

        public string TypeCode { get; set; }

        public string UserIdentifier { get; set; }

        #region IHasMemento Members

        public AggregateMemento GetMemento()
        {
            YilianRequest request;
            using (YilianContext context = new YilianContext())
            {
                request = context.ReadonlyQuery<YilianRequest>()
                    .Include(r => r.RequestInfo)
                    .Include(r => r.GatewayResponse)
                    .Include(r => r.QueryInfo)
                    .Include(r => r.CallbackInfo)
                    .FirstOrDefault(r => r.RequestIdentifier == this.RequestIdentifier);
            }

            return new AggregateMemento
            {
                Value = request == null ? "" : request.ToJson()
            };
        }

        #endregion IHasMemento Members
    }
}