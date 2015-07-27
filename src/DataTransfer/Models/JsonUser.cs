// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JsonUser.cs
// Created          : 2015-07-27  6:28 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  6:40 PM
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