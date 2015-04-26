// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:54 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  11:55 PM
// ***********************************************************************
// <copyright file="PaymentPasswordResetProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Yuyi.Jinyinmao.Domain.Events;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     PaymentPasswordResetProcessor.
    /// </summary>
    public class PaymentPasswordResetProcessor : EventProcessor<PaymentPasswordReset>, IPaymentPasswordReset
    {
    }
}
