// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-27  4:14 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  4:40 PM
// ***********************************************************************
// <copyright file="IAuthenticateSaga.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Sagas
{
    /// <summary>
    ///     Interface IAuthenticateSaga
    /// </summary>
    public interface IAuthenticateSaga : ISaga
    {
        /// <summary>
        ///     Begins the process.
        /// </summary>
        /// <param name="initData">The initData.</param>
        /// <returns>Task.</returns>
        Task BeginProcessAsync(AuthenticateSagaInitDto initData);
    }
}
