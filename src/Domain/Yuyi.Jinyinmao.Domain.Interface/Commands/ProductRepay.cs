// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : ProductRepay.cs
// Created          : 2015-08-13  23:14
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-13  23:17
// ***********************************************************************
// <copyright file="ProductRepay.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    /// </summary>
    [Immutable]
    public class ProductRepay : Command
    {
        /// <summary>
        ///     产品Id
        /// </summary>
        public Guid ProductId { get; set; }
    }
}