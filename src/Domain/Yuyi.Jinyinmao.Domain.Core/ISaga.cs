// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  10:07 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  10:08 PM
// ***********************************************************************
// <copyright file="ISaga.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface ISaga
    /// </summary>
    public interface ISaga : IGrain
    {
        /// <summary>
        ///     Processes the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task ProcessAsync();
    }
}
