// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-25  3:20 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  12:23 AM
// ***********************************************************************
// <copyright file="PaymentPasswordSet.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     PaymentPasswordSet.
    /// </summary>
    [Immutable]
    public class PaymentPasswordSet : Event
    {
    }
}
