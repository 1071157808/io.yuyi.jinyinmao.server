// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IDepositSaga.cs
// Created          : 2015-05-27  7:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-12  3:25 AM
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
    public interface IDepositSaga : IGrainWithGuidKey
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

        /// <summary>
        ///     Reprocess asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task ReprocessAsync();
    }
}