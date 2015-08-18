using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Yuyi.Jinyinmao.Api.Link.Models
{
    public class Link:TableEntity
    {
        public string ShortedLink { get; set; }
        public string OriginalLink { get; set; }
        public DateTime GenerateTime { get; set; }

    }
}