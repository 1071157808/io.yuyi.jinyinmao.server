// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-14  6:12 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-15  6:17 PM
// ***********************************************************************
// <copyright file="IDepositSagaState.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Sagas
{
    /// <summary>
    ///     Enum DepositSagaStatus
    /// </summary>
    public enum DepositSagaStatus
    {
        /// <summary>
        ///     The initialize
        /// </summary>
        Init = 0,

        /// <summary>
        ///     The add bank card
        /// </summary>
        AddBankCard = 10,

        /// <summary>
        ///     The authenticate
        /// </summary>
        Authenticate = 20,

        /// <summary>
        ///     The query authenticate result
        /// </summary>
        QueryAuthenticateResult = 30,

        /// <summary>
        ///     The verifiy bank card
        /// </summary>
        VerifyBankCard = 40,

        /// <summary>
        ///     The query bank card verified result
        /// </summary>
        QueryBankCardVerifiedResult = 50,

        /// <summary>
        ///     The pay by yilian
        /// </summary>
        PayByYilian = 60,

        /// <summary>
        ///     The query yilian payment result
        /// </summary>
        QueryYilianPaymentResult = 70,

        //        /// <summary>
        //        ///     The pay by lianlian
        //        /// </summary>
        //        PayByLianlian = 80,
        //
        //        /// <summary>
        //        ///     The query lianlian payment
        //        /// </summary>
        //        QueryLianlianPayment = 90,

        /// <summary>
        ///     The finished
        /// </summary>
        Finished = 1000,

        /// <summary>
        ///     The fault
        /// </summary>
        Fault = 2000
    }

    /// <summary>
    ///     Interface IDepositSagaState
    /// </summary>
    public interface IDepositSagaState : ISagaState
    {
        /// <summary>
        ///     Gets or sets the initialize data.
        /// </summary>
        /// <value>The initialize data.</value>
        DepositSagaInitData InitData { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        DepositSagaStatus Status { get; set; }
    }

    /// <summary>
    ///     DepositSagaInitData.
    /// </summary>
    [Immutable]
    public class DepositSagaInitData
    {
        /// <summary>
        ///     Gets or sets the add bank card command.
        /// </summary>
        /// <value>The add bank card command.</value>
        public AddBankCard AddBankCardCommand { get; set; }

        /// <summary>
        ///     Gets or sets the authenticate command.
        /// </summary>
        /// <value>The authenticate command.</value>
        public Authenticate AuthenticateCommand { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo InitUserInfo { get; set; }

        /// <summary>
        ///     Gets or sets the pay by lianlian command.
        /// </summary>
        /// <value>The pay by lianlian command.</value>
        public PayByLianlian PayByLianlianCommand { get; set; }

        /// <summary>
        ///     Gets or sets the pay by yilian command.
        /// </summary>
        /// <value>The pay by yilian command.</value>
        public PayByYilian PayByYilianCommand { get; set; }

        /// <summary>
        ///     Gets or sets the verifiy bank card command.
        /// </summary>
        /// <value>The verifiy bank card command.</value>
        public VerifyBankCard VerifyBankCardCommand { get; set; }
    }
}