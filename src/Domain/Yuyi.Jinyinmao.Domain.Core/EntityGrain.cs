// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  8:15 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-03  10:44 PM
// ***********************************************************************
// <copyright file="EntityGrain.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Lib;
using Orleans;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     EntityGrain.
    /// </summary>
    /// <typeparam name="TState">The type of the t state.</typeparam>
    public abstract class EntityGrain<TState> : Grain<TState>, IEntity where TState : class, IEntityState
    {
        /// <summary>
        ///     Gets the command store.
        /// </summary>
        private ICommandStore CommandStore { get; set; }

        /// <summary>
        ///     Gets the event store.
        /// </summary>
        private IEventStore EventStore { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [state changed].
        /// </summary>
        /// <value><c>true</c> if [state changed]; otherwise, <c>false</c>.</value>
        private bool StateChanged { get; set; }

        #region IEntity Members

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        /// <returns>Task&lt;Guid&gt;.</returns>
        public Task<Guid> GetIdAsync() => Task.FromResult(this.State.Id);

        /// <summary>
        ///     Reload state data as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public abstract Task ReloadAsync();

        #endregion IEntity Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.CommandStore = new CommandStore();
            this.EventStore = new EventStore();

            this.RegisterTimer(o => this.State.WriteStateAsync(), new object(), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10));
            this.RegisterTimer(o => this.SaveStateChangesAsync(), new object(), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));

            return base.OnActivateAsync();
        }

        /// <summary>
        ///     This method is called at the begining of the process of deactivating a grain.
        /// </summary>
        public override async Task OnDeactivateAsync()
        {
            await this.State.WriteStateAsync();

            await base.OnDeactivateAsync();
        }

        /// <summary>
        ///     Begins the process command asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        protected void BeginProcessCommandAsync(ICommand command) => this.StoreCommandAsync(command).Forget(e =>
            SiloClusterErrorLogger.Log(new ErrorLog
            {
                Exception = e.GetExceptionString(),
                Message = e.Message,
                PartitionKey = this.GetPrimaryKey().ToGuidString(),
                RowKey = "EntityCommandStoringError"
            })
            );

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
        ///     Saves the state asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        protected Task SaveStateAsync()
        {
            this.StateChanged = true;

            return TaskDone.Done;
        }

        /// <summary>
        ///     Stores the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        protected void StoreEventAsync(IEvent @event)
        {
            EventRecord record = @event.ToRecord();
            this.EventStore.StoreEventRecordAsync(record).Forget(e =>
                SiloClusterErrorLogger.Log(new ErrorLog
                {
                    Exception = e.GetExceptionString(),
                    Message = e.Message,
                    PartitionKey = this.GetPrimaryKey().ToGuidString(),
                    RowKey = "EntityEventStoringError"
                }));
        }

        /// <summary>
        ///     save state changes as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task SaveStateChangesAsync()
        {
            if (this.StateChanged)
            {
                await this.State.WriteStateAsync();
                this.StateChanged = false;
            }
        }

        /// <summary>
        ///     Stores the command asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        private Task StoreCommandAsync(ICommand command)
        {
            CommandRecord record = new CommandRecord
            {
                Command = command.ToJson(),
                CommandId = command.CommandId,
                CommandName = command.GetType().Name
            };
            return this.CommandStore.StoreCommandRecordAsync(record);
        }
    }
}