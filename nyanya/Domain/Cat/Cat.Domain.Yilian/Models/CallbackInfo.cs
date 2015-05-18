// FileInformation: nyanya/Cat.Domain.Yilian/CallbackInfo.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:24 PM

using System;
using Domian.Models;

namespace Cat.Domain.Yilian.Models
{
    public class CallbackInfo : IValueObject
    {
        public string CallbackString { get; set; }

        public DateTime CallbackTime { get; set; }

        public string Message { get; set; }

        public string RequestIdentifier { get; set; }

        public bool Result { get; set; }
    }
}