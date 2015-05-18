// FileInformation: nyanya/Domain.Meow/Feedback.cs
// CreatedTime: 2014/03/30   10:12 PM
// LastUpdatedTime: 2014/04/21   4:24 PM

using System;

namespace Domain.Meow.Models
{
    public class Feedback
    {
        public virtual string Cellphone { get; set; }

        public virtual string Content { get; set; }

        public virtual long Id { get; set; }

        public virtual DateTime Time { get; set; }
    }
}