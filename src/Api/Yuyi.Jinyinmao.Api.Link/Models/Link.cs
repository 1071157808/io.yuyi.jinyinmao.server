using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Yuyi.Jinyinmao.Api.Link.Models
{
    /// <summary>
    /// Link.
    /// </summary>
    public class Link : TableEntity
    {
        /// <summary>
        /// Gets or sets the generate time.
        /// </summary>
        /// <value>The generate time.</value>
        public DateTime GenerateTime { get; set; }

        /// <summary>
        /// Gets or sets the original link.
        /// </summary>
        /// <value>The original link.</value>
        public string OriginalLink { get; set; }

        /// <summary>
        /// Gets or sets the shorted link.
        /// </summary>
        /// <value>The shorted link.</value>
        public string ShortedLink { get; set; }
    }
}