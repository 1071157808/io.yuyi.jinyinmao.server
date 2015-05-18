// FileInformation: nyanya/Domian.Passport/OrderChange.cs
// CreatedTime: 2014/04/28   3:02 PM
// LastUpdatedTime: 2014/05/04   10:12 AM

using System;

namespace Domian.Passport.Models
{
    public class OrderChange
    {
        public virtual DateTime Time { get; set; }

        public virtual int TriggerId { get; set; }

        public virtual string TriggerType { get; set; }

        public virtual int TriggerTypeCode { get; set; }

        public virtual string UserGuid { get; set; }

        public virtual int UserId { get; set; }
    }
}