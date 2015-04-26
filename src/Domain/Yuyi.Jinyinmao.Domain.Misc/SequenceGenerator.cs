// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  1:52 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  2:01 AM
// ***********************************************************************
// <copyright file="SequenceGenerator.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Yuyi.Jinyinmao.Packages;

namespace Yuyi.Jinyinmao.Domain.Misc
{
    public class SequenceGenerator : ISequenceGenerator
    {
        #region ISequenceGenerator Members

        /// <summary>
        ///     Generates the no asynchronous.
        /// </summary>
        /// <param name="sequencePrefix">The sequence prefix.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public Task<string> GenerateNoAsync(char sequencePrefix)
        {
            return Task.FromResult(SequenceNoUtils.GenerateNo(sequencePrefix));
        }

        #endregion ISequenceGenerator Members
    }
}
