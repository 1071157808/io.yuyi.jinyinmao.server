// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  11:27 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-24  10:26 PM
// ***********************************************************************
// <copyright file="ISequenceGenerator.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface ISequenceGenerator
    /// </summary>
    public interface ISequenceGenerator : IGrain
    {
        /// <summary>
        ///     Generates the no asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.String&gt;.</returns>
        Task<string> GenerateNoAsync();
    }
}