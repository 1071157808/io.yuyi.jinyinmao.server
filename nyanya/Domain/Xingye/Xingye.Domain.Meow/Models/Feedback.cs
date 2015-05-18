// FileInformation: nyanya/Xingye.Domain.Meow/Feedback.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using System;
using Domian.Models;

namespace Xingye.Domain.Meow.Models
{
    public class Feedback : IEntity
    {
        public string Cellphone { get; set; }

        public string Content { get; set; }

        public int Id { get; set; }

        public DateTime Time { get; set; }
    }
}