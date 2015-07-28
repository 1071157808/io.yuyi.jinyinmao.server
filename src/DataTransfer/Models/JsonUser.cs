// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JsonUser.cs
// Created          : 2015-07-28  11:38 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-28  11:41 AM
// ***********************************************************************
// <copyright file="JsonUser.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    [Table("JsonUser")]
    public class JsonUser
    {
        [Required]
        public string Data { get; set; }

        public int Id { get; set; }
    }
}