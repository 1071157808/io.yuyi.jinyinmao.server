// FileInformation: nyanya/nyanya.Meow/ItemDto.cs
// CreatedTime: 2014/09/01   10:26 AM
// LastUpdatedTime: 2014/09/01   11:34 AM

using System;
using nyanya.AspDotNet.Common.Dtos;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     ItemDto
    /// </summary>
    public class ItemDto : IDto
    {
        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the expires.
        /// </summary>
        /// <value>
        ///     The expires.
        /// </value>
        public DateTime Expires { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this instance has expired.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has expired; otherwise, <c>false</c>.
        /// </value>
        public bool HasExpired
        {
            get { return DateTime.Now > this.Expires; }
        }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the image source.
        /// </summary>
        /// <value>
        ///     The image source.
        /// </value>
        public string ImageSrc { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>
        ///     The title.
        /// </value>
        public string Title { get; set; }
    }
}