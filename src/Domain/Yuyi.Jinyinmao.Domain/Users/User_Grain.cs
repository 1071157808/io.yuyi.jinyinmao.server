// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-07  12:23 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  3:01 AM
// ***********************************************************************
// <copyright file="User_Grain.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     User.
    /// </summary>
    public partial class User
    {
        #region IUser Members

        /// <summary>
        ///     Reload state data as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public override async Task ReloadAsync()
        {
            await this.State.ReadStateAsync();
            this.ReloadSettleAccountData();
            this.ReloadJBYAccountData();
            this.ReloadOrderInfosData();
        }

        #endregion IUser Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        /// <returns>Task.</returns>
        public override Task OnActivateAsync()
        {
            this.ReloadSettleAccountData();
            this.ReloadJBYAccountData();
            this.ReloadOrderInfosData();
            return base.OnActivateAsync();
        }
    }
}