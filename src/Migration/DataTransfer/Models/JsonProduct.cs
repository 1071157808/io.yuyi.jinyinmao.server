// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JsonProduct.cs
// Created          : 2015-08-02  7:06 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-02  7:15 AM
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
    /// <summary>
    ///     JsonProduct.
    /// </summary>
    [Table("JsonProduct")]
    public class JsonProduct
    {
        /// <summary>
        ///     Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        [Required]
        public string Data { get; set; }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public Guid ProductId { get; set; }
    }
}