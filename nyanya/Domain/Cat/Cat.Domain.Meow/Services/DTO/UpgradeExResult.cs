// FileInformation: nyanya/Cat.Domain.Meow/Feedback.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using System;
using Domian.Models;

namespace Cat.Domain.Meow.Services.DTO
{
    public class UpgradeExResult
    {
        public int status { get; set; }

        public string url { get; set; }

        public string version { get; set; }

        public string message { get; set; }
    }
}