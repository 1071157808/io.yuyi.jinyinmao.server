// FileInformation: nyanya/Domian/CommandHandlers.cs
// CreatedTime: 2014/07/10   12:08 AM
// LastUpdatedTime: 2014/07/10   12:19 AM

using System;
using System.Collections.Generic;

namespace Domian.Commands
{
    public class CommandHandlers : ICommandHandlers
    {
        private readonly Dictionary<Type, object> handlers = new Dictionary<Type, object>();

        #region ICommandHandlers Members

        public virtual void Clear()
        {
            this.handlers.Clear();
        }

        public virtual ICommandHandler<T> GetHandler<T>() where T : ICommand
        {
            Type keyType = typeof(T);
            object value;
            if (this.handlers.TryGetValue(keyType, out value))
            {
                return value as ICommandHandler<T>;
            }

            return null;
        }

        /// <summary>
        ///     Registers a command handler into command handlers.
        /// </summary>
        /// <typeparam name="T">The type of the command.</typeparam>
        /// <param name="handler">The handler to be registered.</param>
        public virtual void Register<T>(ICommandHandler<T> handler) where T : ICommand
        {
            Type keyType = typeof(T);
            this.handlers[keyType] = handler;
        }

        /// <summary>
        ///     Unregisters a command handler from the command handler.
        /// </summary>
        /// <typeparam name="T">The type of the command.</typeparam>
        public virtual void UnRegister<T>() where T : ICommand
        {
            Type keyType = typeof(T);
            if (this.handlers.ContainsKey(keyType))
            {
                this.handlers.Remove(keyType);
            }
        }

        #endregion ICommandHandlers Members
    }
}