// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-23  1:14 PM
// ***********************************************************************
// <copyright file="SagaGrain.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
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
        ///     Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        protected Dictionary<string, object> Info { get; set; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        protected string Message { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="SagaGrain{TState}" /> is waiting.
        /// </summary>
        /// <value><c>true</c> if waiting; otherwise, <c>false</c>.</value>
        protected bool Waiting { get; set; }

        /// <summary>
        ///     Gets or sets the saga entity.
        /// </summary>
        /// <value>The saga entity.</value>
        private SagaStateRecord SagaStateRecord { get; set; }

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
            try
            {
                await this.ProcessAsync();
            }
            catch (Exception e)
            {
                this.RunIntoError(e).Forget();
            }
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

            this.Info = this.Info ?? new Dictionary<string, object>();
            this.Message = this.Message ?? string.Empty;

            return base.OnActivateAsync();
        }

        /// <summary>
        ///     Logs the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        protected void LogError(Exception exception, string message = null)
        {
            message = message ?? exception.Message;

            this.GetLogger().Error(0, message, exception);
        }

        /// <summary>
        ///     Registers the reminder.
        /// </summary>
        /// <returns>Task.</returns>
        protected async Task RegisterReminder()
        {
            await this.RegisterOrUpdateReminder(this.GetType().Name, TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(1));
        }

        /// <summary>
        ///     Runs the into error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="currentProcessingStatus">The current processing status.</param>
        /// <param name="info">The information.</param>
        /// <returns>Task.</returns>
        protected async Task RunIntoError(Exception exception, string message = null, int currentProcessingStatus = 0, Dictionary<string, object> info = null)
        {
            try
            {
                message = message ?? exception.Message;
                info = info ?? new Dictionary<string, object>();
                info.Add("Exception-{0}".FormatWith(DateTime.UtcNow), exception.GetExceptionString());

                await this.StoreSagaStateAsync(currentProcessingStatus, message, info, -1);

                await this.UnregisterReminder();
            }
            catch (Exception e)
            {
                SiloClusterErrorLogger.Log(e, "SagaErrorLogingError: {0}".FormatWith(e.Message));
            }
        }

        /// <summary>
        ///     Initializes the saga entity.
        /// </summary>
        /// <param name="currentProcessingStatus">The current processing status.</param>
        /// <param name="message">The message.</param>
        /// <param name="info">The information.</param>
        /// <param name="state">The state.</param>
        /// <returns>Task.</returns>
        protected async Task StoreSagaStateAsync(int currentProcessingStatus = 0, string message = null, Dictionary<string, object> info = null, int state = 0)
        {
            message = message ?? string.Empty;
            info = info ?? new Dictionary<string, object>();

            await this.State.WriteStateAsync();

            DateTime now = DateTime.UtcNow;

            this.SagaStateRecord = new SagaStateRecord
            {
                BeginTime = this.State.BeginTime,
                CurrentProcessingStatus = currentProcessingStatus,
                Info = info.ToJson(),
                Message = message,
                PartitionKey = this.State.SagaId.ToGuidString(),
                RowKey = now.ToString("O"),
                SagaId = this.State.SagaId,
                SagaState = this.State.ToJson(),
                SagaType = this.State.SagaType,
                State = state,
                UpdateTime = now
            };

            await SiloClusterConfig.SagasTable.ExecuteAsync(TableOperation.InsertOrReplace(this.SagaStateRecord));
        }

        /// <summary>
        ///     Unregisters the reminder.
        /// </summary>
        /// <returns>Task.</returns>
        protected async Task UnregisterReminder()
        {
            IGrainReminder reminder = (await this.GetReminders()).FirstOrDefault(r => r.ReminderName == this.GetType().Name);

            if (reminder != null)
            {
                await this.UnregisterReminder(reminder);
            }
        }
    }
}