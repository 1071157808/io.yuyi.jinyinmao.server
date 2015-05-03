// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-03  6:42 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  12:17 AM
// ***********************************************************************
// <copyright file="IDepositByYilianSaga.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Sagas
{
    /// <summary>
    ///     Interface IDepositSaga
    /// </summary>
    public interface IDepositByYilianSaga : IGrain
    {
        /// <summary>
        ///     Begins the process.
        /// </summary>
        /// <param name="initData">The initialize data.</param>
        /// <returns>Task.</returns>
        Task BeginProcessAsync(DepositFromYilianSagaInitDto initData);
    }
}