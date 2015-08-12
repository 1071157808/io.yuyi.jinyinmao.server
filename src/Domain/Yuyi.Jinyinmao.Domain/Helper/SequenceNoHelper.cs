// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SequenceNoHelper.cs
// Created          : 2015-05-27  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-12  3:05 PM
// ***********************************************************************
// <copyright file="SequenceNoHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain.Helper
{
    /// <summary>
    ///     SequenceNoHelper.
    /// </summary>
    public static class SequenceNoHelper
    {
        private static readonly ISequenceGenerator Generator;

        static SequenceNoHelper()
        {
            Generator = GrainClient.GrainFactory.GetGrain<ISequenceGenerator>(GrainTypeHelper.GetSequenceGeneratorGrainTypeKey());
        }

        /// <summary>
        ///     Gets the sequence no asynchronous.
        /// </summary>
        /// <returns>System.String.</returns>
        public static async Task<string> GetSequenceNoAsync()
        {
            return await Generator.GenerateNoAsync();
        }
    }
}