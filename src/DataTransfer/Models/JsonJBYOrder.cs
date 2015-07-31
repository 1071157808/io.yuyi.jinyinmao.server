// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JsonJBYOrder.cs
// Created          : 2015-07-31  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-31  7:40 PM
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

        [StringLength(50)]
        public string OrderId { get; set; }
    }
}