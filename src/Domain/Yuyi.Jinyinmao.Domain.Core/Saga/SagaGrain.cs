// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  10:00 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  10:14 PM
// ***********************************************************************
// <copyright file="SagaGrain.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Orleans;
using Orleans.Runtime;
using Yuyi.Jinyinmao.Domain.Sagas;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     SagaGrain.
    /// </summary>
    /// <typeparam name="TState">The type of the t state.</typeparam>
    public abstract class SagaGrain<TState> : Grain<TState>, ISaga, IRemindable where TState : class, ISagaState
    {
        /// <summary>
        ///     Gets or sets the saga entity.
        /// </summary>
        /// <value>The saga entity.</value>
        protected SagaEntity SagaEntity { get; set; }

        #region IRemindable Members

        /// <summary>
        ///     Receieve a new Reminder.
        /// </summary>
        /// <param name="reminderName">Name of this Reminder</param>
        /// <param name="status">Status of this Reminder tick</param>
        /// <returns>
        ///     Completion promise which the grain will resolve when it has finished processing this Reminder tick.
        /// </returns>
        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            await this.ProcessAsync();
        }

        #endregion IRemindable Members

        #region ISaga Members

        /// <summary>
        ///     Processes the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public abstract Task ProcessAsync();

        #endregion ISaga Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            if (this.State.SagaId == Guid.Empty || this.State.SagaType.IsNullOrEmpty())
            {
                this.State.SagaId = this.GetPrimaryKey();
                this.State.SagaType = this.GetType().Name;
            }
            return base.OnActivateAsync();
        }

        /// <summary>
        ///     Initializes the saga entity.
        /// </summary>
        protected virtual void InitSagaEntity()
        {
            this.SagaEntity = new SagaEntity
            {
                BeginTime = DateTime.UtcNow.AddHours(8),
                UpdateTime = DateTime.UtcNow.AddHours(8),
                Info = new Dictionary<string, object>(),
                InitData = new Object().ToJson(),
                Message = string.Empty,
                PartitionKey = this.State.SagaType,
                RowKey = this.State.SagaId.ToGuidString(),
                SagaId = this.State.SagaId,
                SagaType = this.State.SagaType,
                State = 0
            };
        }

        /// <summary>
        ///     store saga entity as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        protected async Task StoreSagaEntityAsync()
        {
            await SiloClusterConfig.SagasTable.ExecuteAsync(TableOperation.Insert(this.SagaEntity));
        }
    }
}
