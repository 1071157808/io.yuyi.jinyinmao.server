// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : ISequenceGenerator.cs
// Created          : 2015-05-27  7:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-12  3:26 AM
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
    public interface ISequenceGenerator : IGrainWithIntegerKey
    {
        /// <summary>
        ///     Generates the no asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.String&gt;.</returns>
        Task<string> GenerateNoAsync();
    }
}