// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  8:15 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  8:45 PM
// ***********************************************************************
// <copyright file="EntityGrain.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
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
        public ICommandStore CommandStore
        {
            get { return this.State.CommandStore; }
        }

        /// <summary>
        ///     Gets the event store.
        /// </summary>
        public IEventStore EventStore
        {
            get { return this.State.EventStore; }
        }

        /// <summary>
        /// Gets the error logger.
        /// </summary>
        /// <value>The error logger.</value>
        public IEventProcessingLogger ErrorLogger
        {
            get { return new EventProcessingLogger(); }
        }

        #region IEntity Members

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        /// <returns>Task&lt;Guid&gt;.</returns>
        public Task<Guid> GetIdAsync()
        {
            return Task.FromResult(this.State.Id);
        }

        #endregion IEntity Members

        /// <summary>
        ///     Stores the command asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task StoreCommandAsync(ICommand command)
        {
            CommandRecord record = new CommandRecord
            {
                Command = command,
                CommandId = command.CommandId,
                TimeStamp = DateTime.UtcNow.Ticks
            };
            return this.CommandStore.StoreCommandRecordAsync(record);
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
                Event = @event,
                EventId = @event.EventId,
                TimeStamp = DateTime.UtcNow.Ticks
            };
            return this.EventStore.StoreEventRecordAsync(record);
        }

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            if (this.State.CommandStore == null)
            {
                this.State.CommandStore = new CommandStore { EntityId = this.State.Id };
            }

            if (this.State.EventStore == null)
            {
                this.State.EventStore = new EventStore { EntityId = this.State.Id };
            }

            return base.OnActivateAsync();
        }
    }
}
