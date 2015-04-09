// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-07  10:57 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-08  1:06 PM
// ***********************************************************************
// <copyright file="Cellphone.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Lib;
using Orleans;
using Yuyi.Jinyinmao.Domain.Dto;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Class Cellphone.
    /// </summary>
    public class Cellphone : Grain<ICellphoneState>, ICellphone
    {
        #region ICellphone Members

        /// <summary>
        ///     Gets the cellphone information.
        /// </summary>
        /// <returns>Task&lt;CellphoneInfo&gt;.</returns>
        public async Task<CellphoneInfo> GetCellphoneInfo()
        {
            if (!this.State.UserId.HasValue)
            {
                this.State.UserId = Guid.NewGuid();
            }

            await this.State.WriteStateAsync();

            return new CellphoneInfo
            {
                Cellphone = this.State.Cellphone,
                Registered = this.State.Registered,
                UserId = this.State.UserId.GetValueOrDefault()
            };
        }

        #endregion ICellphone Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.State.Cellphone = this.GetPrimaryKey().ToGuidString().Substring(7);
            if (!this.State.UserId.HasValue)
            {
                this.State.UserId = Guid.NewGuid();
            }
            return base.OnActivateAsync();
        }
    }
}
