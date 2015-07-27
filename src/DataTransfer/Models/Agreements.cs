// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Agreements.cs
// Created          : 2015-07-27  9:16 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  3:59 PM
// ***********************************************************************
// <copyright file="Agreements.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace DataTransfer.Models
{
    public class Agreements
    {
        [Required]
        public string Content { get; set; }

        public int Id { get; set; }
    }
}