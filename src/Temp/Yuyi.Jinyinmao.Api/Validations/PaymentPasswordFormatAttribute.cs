// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-25  2:46 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-25  2:46 AM
// ***********************************************************************
// <copyright file="PaymentPasswordFormatAttribute.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;

namespace Yuyi.Jinyinmao.Api.Validations
{
    /// <summary>
    ///     PaymentPasswordFormatAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class PaymentPasswordFormatAttribute : RegularExpressionAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.ComponentModel.DataAnnotations.RegularExpressionAttribute" /> class.
        /// </summary>
        public PaymentPasswordFormatAttribute()
            : base(@"^(?![^a-zA-Z~!@#$%^&*_]+$)(?!\D+$).{8,18}$")
        {
        }
    }
}
