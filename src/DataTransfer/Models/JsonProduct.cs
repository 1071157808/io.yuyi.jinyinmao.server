// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JsonProduct.cs
// Created          : 2015-07-31  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-31  7:40 PM
// ***********************************************************************
// <copyright file="JsonProduct.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    [Table("JsonProduct")]
    public class JsonProduct
    {
        [Required]
        public string Data { get; set; }

        public int Id { get; set; }

        public Guid ProductId { get; set; }
    }
}