// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-14  6:03 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-21  7:50 PM
// ***********************************************************************
// <copyright file="IDepositSaga.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain.Sagas
{
    /// <summary>
    ///     Interface IDepositSaga
    /// </summary>
    public interface IDepositSaga : IGrain
    {
        /// <summary>
        ///     Begins the process.
        /// </summary>
        /// <param name="initData">The initData.</param>
        /// <returns>Task.</returns>
        Task BeginProcessAsync(DepositSagaInitData initData);

        /// <summary>
        ///     Processes the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task ProcessAsync();
    }
}