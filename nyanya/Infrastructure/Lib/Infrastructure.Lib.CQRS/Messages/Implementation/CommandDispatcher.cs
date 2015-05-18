// FileInformation: nyanya/Infrastructure.Lib.CQRS/CommandDispatcher.cs
// CreatedTime: 2014/07/01   1:28 PM
// LastUpdatedTime: 2014/07/08   5:45 PM

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Infrastructure.Lib.CQRS.Bus;
using Infrastructure.Lib.CQRS.Config;
using Infrastructure.Lib.CQRS.Log;
using Infrastructure.Lib.CQRS.MessageLogs;

namespace Infrastructure.Lib.CQRS.Messages.Implementation
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly CommandLogStore commandLogStore;
        private readonly Dictionary<Type, object> handlers = new Dictionary<Type, object>();
        private readonly ILogger logger;

        public CommandDispatcher()
        {
            this.commandLogStore = new CommandLogStore();
            //this.logger = CqrsConfigration.Loggers.CommandStoreLogger;
        }

        #region ICommandDispatcher Members

        /// <summary>
        ///     Dispatches the command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command to be dispatched.</param>
        /// <returns>
        ///     Dispatch Successed
        /// </returns>
        public virtual bool Dispatch<T>(T command) where T : ICommand
        {
            Type commandType = typeof(T);
            object value;
            if (this.handlers.TryGetValue(commandType, out value) && value is ICommandHandler<T>)
            {
                ICommandHandler<T> commandHandler = (ICommandHandler<T>)value;
                try
                {
                    commandHandler.Handle(command);
                    this.OnDispatched(command, commandHandler);
                    return true;
                }
                catch (Exception e)
                {
                    this.OnDispatchFailed(command, commandHandler, e);
                }
            }
            else
            {
                this.OnDispatchFailed(command);
            }
            return false;
        }

        /// <summary>
        ///     Registers a command handler into command dispatcher.
        /// </summary>
        /// <typeparam name="T">The type of the command.</typeparam>
        /// <param name="handler">The handler to be registered.</param>
        public virtual void Register<T>(ICommandHandler<T> handler) where T : ICommand
        {
            Type keyType = typeof(T);
            this.handlers[keyType] = handler;
        }

        /// <summary>
        ///     Unregisters a command handler from the command dispatcher.
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

        #endregion ICommandDispatcher Members

        /// <summary>
        ///     Creates a command dispatcher and registers all the command handlers
        ///     specified in the <see cref="Infrastructure.Lib.CQRS.Config.IConfigSource" /> instance.
        /// </summary>
        /// <param name="commandDispatcherType">The type of the command dispatcher.</param>
        /// <param name="args">The arguments that is used for initializing the command dispatcher.</param>
        /// <returns>A <see cref="ICommandDispatcher" /> instance.</returns>
        public static ICommandDispatcher CreateAndRegister(Type commandDispatcherType, params object[] args)
        {
            ICommandDispatcher commandDispatcher = (ICommandDispatcher)Activator.CreateInstance(commandDispatcherType, args);
            Collection<CommandHandlerElement> handlerElementCollection = null;//CqrsConfigration.Config.CommandHandlers;

            foreach (CommandHandlerElement handlerElement in handlerElementCollection)
            {
                switch (handlerElement.SourceType)
                {
                    case HandlerSourceType.Type:
                        string typeName = handlerElement.Source;
                        Type handlerType = Type.GetType(typeName);
                        RegisterType(commandDispatcher, handlerType);
                        break;

                    case HandlerSourceType.Assembly:
                        string assemblyString = handlerElement.Source;
                        Assembly assembly = Assembly.Load(assemblyString);
                        RegisterAssembly(commandDispatcher, assembly);
                        break;
                }
            }
            return commandDispatcher;
        }

        public virtual void Clear()
        {
            this.handlers.Clear();
        }

        /// <summary>
        ///     Registers all the handler types within a given assembly to the command dispatcher.
        /// </summary>
        /// <param name="commandDispatcher">Command dispatcher instance.</param>
        /// <param name="assembly">The assembly.</param>
        private static void RegisterAssembly(ICommandDispatcher commandDispatcher, Assembly assembly)
        {
            foreach (Type type in assembly.GetExportedTypes())
            {
                Type[] intfs = type.GetInterfaces();
                if (intfs.Any(p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(ICommandHandler<>)) && intfs.Any(p => p.IsDefined(typeof(RegisterCommandDispatchAttribute), true)))
                {
                    RegisterType(commandDispatcher, type);
                }
            }
        }

        /// <summary>
        ///     Registers the specified handler type to the command dispatcher.
        /// </summary>
        /// <param name="commandDispatcher">Command dispatcher instance.</param>
        /// <param name="handlerType">The type to be registered.</param>
        private static void RegisterType(ICommandDispatcher commandDispatcher, Type handlerType)
        {
            //MethodInfo methodInfo = commandDispatcher.GetType().GetMethod("Register", BindingFlags.Public | BindingFlags.Instance);

            //IEnumerable<Type> handlerIntfTypeQuery = handlerType.GetInterfaces().Where(p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(ICommandHandler<>));

            //foreach (Type handlerIntfType in handlerIntfTypeQuery)
            //{
            //    object handlerInstance = CqrsConfigration.Config.DependencyResolver.GetService(handlerType) ??
            //                             Activator.CreateInstance(handlerType);
            //    Type commandType = handlerIntfType.GetGenericArguments().First();
            //    MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(commandType);
            //    genericMethodInfo.Invoke(commandDispatcher, new[] { handlerInstance });
            //}
        }

        private void OnDispatched(ICommand command, ICommandHandler commandHandler)
        {
            // ReSharper disable once CSharpWarnings::CS4014
            // this.commandLogStore.Delivered(command);
            this.logger.Info("Dispatched Command {0} To Handler {1}.", command.CommandId, commandHandler.Name);
        }

        private void OnDispatchFailed(ICommand command, ICommandHandler commandHandler, Exception e)
        {
            // ReSharper disable once CSharpWarnings::CS4014
            // this.commandLogStore.Delivered(command, false);
            this.logger.Error(e, "Exception Duraing Dispatching Command {0} To Handler {1}.\n{2}", command.CommandId, commandHandler.Name, e.Message);
        }

        private void OnDispatchFailed(ICommand command)
        {
            // ReSharper disable once CSharpWarnings::CS4014
            // this.commandLogStore.Delivered(command, false);
            this.logger.Error("Can not find Command Handler During Sending Command {0}.", command.CommandId);
        }
    }
}