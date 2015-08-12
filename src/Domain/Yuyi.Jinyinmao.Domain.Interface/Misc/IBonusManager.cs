// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IBonusManager.cs
// Created          : 2015-08-12  12:55 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-12  12:56 PM
// ***********************************************************************
// <copyright file="IBonusManager.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain.Misc
{
    /// <summary>
    ///     Interface IBonusManager
    /// </summary>
    public interface IBonusManager : IGrainWithGuidKey
    {
        /// <summary>
        /// Gets the bonus amount.
        /// </summary>
        /// <param name="baseAmount">The base amount.</param>
        /// <returns>Task&lt;System.Int64&gt;.</returns>
        Task<long> GetBonusAmount(long baseAmount);
    }
}