// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  8:17 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-07  1:50 PM
// ***********************************************************************
// <copyright file="IEntity.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IEntity
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        /// <returns>Task&lt;Guid&gt;.</returns>
        Task<Guid> GetIdAsync();

        /// <summary>
        ///     Reloads the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task ReloadAsync();
    }
}