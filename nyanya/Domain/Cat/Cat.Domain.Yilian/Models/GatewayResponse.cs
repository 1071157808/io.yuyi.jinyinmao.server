// FileInformation: nyanya/Cat.Domain.Yilian/GatewayResponse.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:24 PM

using System;
using Domian.Models;

namespace Cat.Domain.Yilian.Models
{
    public class GatewayResponse : IValueObject
    {
        public string Message { get; set; }

        public string RequestIdentifier { get; set; }

        public string ResponseString { get; set; }

        public bool Result { get; set; }

        public DateTime SendingTime { get; set; }
    }
}