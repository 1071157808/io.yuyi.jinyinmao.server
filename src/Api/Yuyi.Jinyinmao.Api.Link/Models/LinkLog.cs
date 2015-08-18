using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Yuyi.Jinyinmao.Api.Link.Models
{
    /// <summary>
    /// LinkLog.
    /// </summary>
    public class LinkLog : TableEntity
    {
        /// <summary>
        /// Gets or sets the hit time.
        /// </summary>
        /// <value>The hit time.</value>
        public DateTime HitTime { get; set; }

        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>The ip.</value>
        public string Ip { get; set; }

        /// <summary>
        /// Gets or sets the source URL.
        /// </summary>
        /// <value>The source URL.</value>
        public string SourceUrl { get; set; }

        /// <summary>
        /// Gets or sets the target URL.
        /// </summary>
        /// <value>The target URL.</value>
        public string TargetUrl { get; set; }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>The user agent.</value>
        public string UserAgent { get; set; }
    }
}