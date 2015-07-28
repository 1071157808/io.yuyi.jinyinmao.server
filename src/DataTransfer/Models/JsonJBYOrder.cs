// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JsonJBYOrder.cs
// Created          : 2015-07-28  11:38 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-28  11:40 AM
// ***********************************************************************
// <copyright file="JsonJBYOrder.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    [Table("JsonJBYOrder")]
    public class JsonJBYOrder
    {
        [Required]
        public string Data { get; set; }

        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string OrderId { get; set; }
    }
}