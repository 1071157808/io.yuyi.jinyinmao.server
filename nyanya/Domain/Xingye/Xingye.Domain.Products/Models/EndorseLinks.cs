// FileInformation: nyanya/Xingye.Domain.Products/EndorseLinks.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:25 PM

using Domian.Models;

namespace Xingye.Domain.Products.Models
{
    public class EndorseLinks : IValueObject
    {
        /// <summary>
        ///     担保物链接
        /// </summary>
        public string EndorseImageLink { get; set; }

        /// <summary>
        ///     担保物缩略图链接
        /// </summary>
        public string EndorseImageThumbnailLink { get; set; }

        public string ProductIdentifier { get; set; }
    }
}