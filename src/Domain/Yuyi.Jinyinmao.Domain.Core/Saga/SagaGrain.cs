// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-07  1:17 PM
// ***********************************************************************
// <copyright file="SagaGrain.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Orleans;
using Orleans.Runtime;
using Yuyi.Jinyinmao.Domain.Sagas;
using Yuyi.Jinyinmao.Packages.Helper;

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
            TableResult tableResult = await SiloClusterConfig.SagasTable.ExecuteAsync(TableOperation.Retrieve<SagaEntity>(this.State.SagaType, this.State.SagaId.ToGuidString()));
            this.SagaEntity = (SagaEntity)tableResult.Result;

            try
            {
                await this.ProcessAsync();
            }
            catch (Exception e)
            {
                this.RunIntoError(e.Message, e);
            }

            await this.StoreSagaEntityAsync();
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
        /// <param name="initDataJson">The initialize data json.</param>
        protected virtual void InitSagaEntity(object initDataJson)
        {
            this.SagaEntity = new SagaEntity
            {
                BeginTime = DateTime.UtcNow.AddHours(8),
                UpdateTime = DateTime.UtcNow.AddHours(8),
                Info = JsonHelper.NewDictionary,
                InitData = initDataJson.ToJson(),
                Message = string.Empty,
                PartitionKey = this.State.SagaType,
                RowKey = this.State.SagaId.ToGuidString(),
                SagaId = this.State.SagaId,
                SagaType = this.State.SagaType,
                State = 0
            };
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
                Info = JsonHelper.NewDictionary,
                InitData = JsonHelper.NewObject,
                Message = string.Empty,
                PartitionKey = this.State.SagaType,
                RowKey = this.State.SagaId.ToGuidString(),
                SagaId = this.State.SagaId,
                SagaType = this.State.SagaType,
                State = 0
            };
        }

        /// <summary>
        ///     Registers the reminder.
        /// </summary>
        /// <returns>Task.</returns>
        protected virtual async Task RegisterReminder()
        {
            await this.RegisterOrUpdateReminder(this.GetType().Name, TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(1));
        }

        /// <summary>
        ///     Runs the into error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        protected void RunIntoError(string message, Exception exception)
        {
            this.SagaEntity.Message = message;
            this.SagaEntity.Add("Exception", exception.GetExceptionString());
        }

        /// <summary>
        ///     Runs the into error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        protected void RunIntoError(Exception exception)
        {
            this.SagaEntity.Message = exception.Message;
            this.SagaEntity.Add("Exception", exception.GetExceptionString());
        }

        /// <summary>
        ///     Store saga entity asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        protected async Task StoreSagaEntityAsync()
        {
            await SiloClusterConfig.SagasTable.ExecuteAsync(TableOperation.InsertOrReplace(this.SagaEntity));
        }

        /// <summary>
        ///     Unregisters the reminder.
        /// </summary>
        /// <returns>Task.</returns>
        protected virtual async Task UnregisterReminder()
        {
            await this.UnregisterReminder(await this.GetReminder(this.GetType().Name));
        }
    }
}