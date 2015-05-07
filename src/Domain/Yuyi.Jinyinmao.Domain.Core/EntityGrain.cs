// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  8:15 AM
// 
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-07  3:03 PM
// ***********************************************************************
// <copyright file="EntityGrain.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
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
        public ICommandStore CommandStore { get; set; }

        /// <summary>
        ///     Gets the event store.
        /// </summary>
        public IEventStore EventStore { get; set; }

        #region IEntity Members

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        /// <returns>Task&lt;Guid&gt;.</returns>
        public Task<Guid> GetIdAsync()
        {
            return Task.FromResult(this.State.Id);
        }

        /// <summary>
        ///     Reload state data as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public abstract Task ReloadAsync();

        #endregion

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.CommandStore = new CommandStore();
            this.EventStore = new EventStore();
            return base.OnActivateAsync();
        }

        /// <summary>
        ///     Stores the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public Task StoreEventAsync(IEvent @event)
        {
            EventRecord record = new EventRecord
            {
                Event = @event.ToJson(),
                EventId = @event.EventId,
                TimeStamp = DateTime.UtcNow.Ticks,
                EventName = @event.GetType().Name,
                PartitionKey = this.GetPrimaryKey().ToGuidString(),
                RowKey = @event.EventId.ToGuidString()
            };
            return this.EventStore.StoreEventRecordAsync(record);
        }

        /// <summary>
        ///     Begins the process command asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        protected async Task BeginProcessCommandAsync(ICommand command)
        {
            await this.StoreCommandAsync(command);
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
                TimeStamp = DateTime.UtcNow.Ticks,
                CommandName = command.GetType().Name,
                PartitionKey = this.GetPrimaryKey().ToGuidString(),
                RowKey = command.CommandId.ToGuidString()
            };
            return this.CommandStore.StoreCommandRecordAsync(record);
        }
    }
}