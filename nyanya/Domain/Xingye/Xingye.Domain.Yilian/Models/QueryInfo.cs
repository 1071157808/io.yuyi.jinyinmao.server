// FileInformation: nyanya/Xingye.Domain.Yilian/QueryInfo.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:24 PM

using System;
using Domian.Bus;
using Domian.Config;
using Domian.Models;

namespace Xingye.Domain.Yilian.Models
{
    public class QueryInfo : IValueObject
    {
        public QueryInfo(string requestIdentifier)
        {
            this.RequestIdentifier = requestIdentifier;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianAuthRequest" /> class.
        ///     Only for Entity framework
        /// </summary>
        private QueryInfo()
        {
        }

        public string Message { get; set; }

        public DateTime QueryTime { get; set; }

        public string RequestIdentifier { get; set; }

        public string ResponseString { get; set; }

        public bool Result { get; set; }

        private IEventBus eventbus
        {
            get { return CqrsDomainConfig.Configuration.EventBus; }
        }
    }
}