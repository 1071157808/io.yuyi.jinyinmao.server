using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Yuyi.Jinyinmao.Api.Link.Models
{
    public class LinkLog:TableEntity
    {
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public DateTime HitTime { get; set; }
        public string SourceUrl { get; set; }
        public string TargetUrl { get; set; }

    }
}