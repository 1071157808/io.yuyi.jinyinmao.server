// FileInformation: nyanya/Services.WebAPI.V1.nyanya/ItemDto.cs
// CreatedTime: 2014/08/08   9:18 AM
// LastUpdatedTime: 2014/08/08   9:27 AM

using System;
using Services.WebAPI.Common.Dtos;

namespace Services.WebAPI.V1.nyanya.Models
{
    /// <summary>
    ///     ItemDto
    /// </summary>
    public class ItemDto : IDto
    {
        #region Public Properties

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

        #endregion Public Properties
    }

    //internal static class ItemExtensions
    //{
    //    internal static ItemDto ToItemDto(this Item item)
    //    {
    //        return new ItemDto
    //        {
    //            Id = item.Id,
    //            Description = item.Category.CategoryDescription,
    //            Expires = item.Expires,
    //            ImageSrc = item.Category.ImageSrc,
    //            Title = item.Category.CategoryTitle
    //        };
    //    }
    //}
}