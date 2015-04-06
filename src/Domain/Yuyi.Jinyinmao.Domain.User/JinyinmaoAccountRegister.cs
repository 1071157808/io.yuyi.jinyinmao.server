// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-04  6:52 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-04  6:59 PM
// ***********************************************************************
// <copyright file="JinyinmaoAccountRegister.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Actor.Model;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Class JinyinmaoAccountRegister.
    /// </summary>
    public class JinyinmaoAccount : EntityGrain<IJinyinmaoAccountState>, IJinyinmaoAccount
    {
        #region IJinyinmaoAccount Members

        /// <summary>
        ///     Registers the specified jinyinmao account register.
        /// </summary>
        /// <param name="jinyinmaoAccountRegister">The jinyinmao account register.</param>
        /// <returns>Task.</returns>
        public Task Register(JinyinmaoAccountRegister jinyinmaoAccountRegister)
        {
            throw new NotImplementedException();
        }

        #endregion IJinyinmaoAccount Members
    }
}
