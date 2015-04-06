// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-05  7:56 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-05  8:02 PM
// ***********************************************************************
// <copyright file="IClusterConfig.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IClusterConfig
    /// </summary>
    public interface IClusterConfig : IGrain
    {
        /// <summary>
        ///     Gets the user cellphone manager asynchronous.
        /// </summary>
        /// <returns>Task&lt;IUserCellphoneManager&gt;.</returns>
        Task<IUserCellphoneManager> GetUserCellphoneManagerAsync();
    }
}
