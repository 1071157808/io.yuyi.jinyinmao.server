// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:55 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-03  10:23 PM
// ***********************************************************************
// <copyright file="PaymentPasswordSetProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Yuyi.Jinyinmao.Domain.Events;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     PaymentPasswordSetProcessor.
    /// </summary>
    public class PaymentPasswordSetProcessor : EventProcessor<PaymentPasswordSet>, IPaymentPasswordSetProcessor
    {
    }
}