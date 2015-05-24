// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-24  10:11 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-24  10:14 PM
// ***********************************************************************
// <copyright file="SequenceNoHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

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
            Generator = SequenceGeneratorFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey());
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