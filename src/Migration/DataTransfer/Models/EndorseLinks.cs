// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : EndorseLinks.cs
// Created          : 2015-07-27  9:16 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  4:00 PM
// ***********************************************************************
// <copyright file="EndorseLinks.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace DataTransfer.Models
{
    public class EndorseLinks
    {
        [Required]
        [StringLength(300)]
        public string EndorseImageLink { get; set; }

        [Required]
        [StringLength(300)]
        public string EndorseImageThumbnailLink { get; set; }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }
    }
}