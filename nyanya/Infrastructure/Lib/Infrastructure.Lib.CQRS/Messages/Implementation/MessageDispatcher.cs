// FileInformation: nyanya/Infrastructure.Lib.CQRS/MessageDispatcher.cs
// CreatedTime: 2014/06/11   3:23 PM
// LastUpdatedTime: 2014/06/17   3:32 PM

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Infrastructure.Lib.CQRS.Config;

namespace Infrastructure.Lib.CQRS.Messages.Implementation
{
    /// <summary>
    ///     Represents the message dispatcher.
    /// </summary>
    public abstract class MessageDispatcher : MessageDispatcherEventHandler, IMessageDispatcher
    {
        private readonly Dictionary<Type, List<object>> handlers = new Dictionary<Type, List<object>>();

        #region IMessageDispatcher Members

        /// <summary>
        ///     Clears the registration of the message handlers.
        /// </summary>
        public virtual void Clear()
        {
            this.handlers.Clear();
        }

        /// <summary>
        ///     Dispatches the message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message to be dispatched.</param>
        /// <returns>
        ///     Dispatch Successed
        /// </returns>
        public virtual bool Dispatch<T>(T message)
        {
            Type messageType = typeof(T);
            List<object> value;
            if (this.handlers.TryGetValue(messageType, out value))
            {
                List<object> messageHandlers = value;
                foreach (object messageHandler in messageHandlers)
                {
                    IHandler<T> dynMessageHandler = (IHandler<T>)messageHandler;
                    MessageDispatchEventArgs evtArgs = new MessageDispatchEventArgs(message, messageHandler.GetType(), messageHandler);
                    this.OnDispatching(evtArgs);
                    try
                    {
                        dynMessageHandler.Handle(message);
                        this.OnDispatched(evtArgs);
                        return true;
                    }
                    catch
                    {
                        this.OnDispatchFailed(evtArgs);
                    }
                }
            }
            return false;
        }

        /// <summary>
        ///     Registers a message handler into message dispatcher.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="handler">The handler to be registered.</param>
        public virtual void Register<T>(IHandler<T> handler)
        {
            Type keyType = typeof(T);

            List<object> value;
            if (this.handlers.TryGetValue(keyType, out value))
            {
                List<object> registeredHandlers = value;
                if (registeredHandlers != null)
                {
                    if (!registeredHandlers.Contains(handler))
                        registeredHandlers.Add(handler);
                }
                else
                {
                    registeredHandlers = new List<object>();
                    registeredHandlers.Add(handler);
                    this.handlers[keyType] = registeredHandlers;
                }
            }
            else
            {
                List<object> registeredHandlers = new List<object>();
                registeredHandlers.Add(handler);
                this.handlers.Add(keyType, registeredHandlers);
            }
        }

        /// <summary>
        ///     Unregisters a message handler from the message dispatcher.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="handler">The handler to be registered.</param>
        public virtual void UnRegister<T>(IHandler<T> handler)
        {
            Type keyType = typeof(T);
            List<object> value;
            if (this.handlers.TryGetValue(keyType, out value) && value != null && value.Count > 0 && value.Contains(handler))
            {
                value.Remove(handler);
            }
        }

        #endregion IMessageDispatcher Members

        /// <summary>
        ///     Creates a message dispatcher and registers all the message handlers
        ///     specified in the <see cref="Infrastructure.Lib.CQRS.Config.IConfigSource" /> instance.
        /// </summary>
        /// <param name="configSource">
        ///     The <see cref="Infrastructure.Lib.CQRS.Config.IConfigSource" /> instance
        ///     that contains the definitions for message handlers.
        /// </param>
        /// <param name="messageDispatcherType">The type of the message dispatcher.</param>
        /// <param name="args">The arguments that is used for initializing the message dispatcher.</param>
        /// <returns>A <see cref="IMessageDispatcher" /> instance.</returns>
        public static IMessageDispatcher CreateAndRegister(IConfigSource configSource,
            Type messageDispatcherType,
            params object[] args)
        {
            IMessageDispatcher messageDispatcher = (IMessageDispatcher)Activator.CreateInstance(messageDispatcherType,
                args);

            Collection<HandlerElement> handlerElementCollection = null;//configSource.Config.Handlers;
            foreach (HandlerElement handlerElement in handlerElementCollection)
            {
                switch (handlerElement.SourceType)
                {
                    case HandlerSourceType.Type:
                        string typeName = handlerElement.Source;
                        Type handlerType = Type.GetType(typeName);
                        RegisterType(messageDispatcher, handlerType);
                        break;

                    case HandlerSourceType.Assembly:
                        string assemblyString = handlerElement.Source;
                        Assembly assembly = Assembly.Load(assemblyString);
                        RegisterAssembly(messageDispatcher, assembly);
                        break;
                }
            }
            return messageDispatcher;
        }

        /// <summary>
        ///     Registers all the handler types within a given assembly to the message dispatcher.
        /// </summary>
        /// <param name="messageDispatcher">Message dispatcher instance.</param>
        /// <param name="assembly">The assembly.</param>
        private static void RegisterAssembly(IMessageDispatcher messageDispatcher, Assembly assembly)
        {
            //foreach (Type type in from type in assembly.GetExportedTypes() let intfs = type.GetInterfaces() where intfs.Any(p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(IHandler<>)) && intfs.Any(p => p.IsDefined(typeof(RegisterDispatchAttribute), true)) select type)
            foreach (Type type in assembly.GetExportedTypes())
            {
                Type[] intfs = type.GetInterfaces();
                if (intfs.Any(p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(IHandler<>)) && intfs.Any(p => p.IsDefined(typeof(RegisterDispatchAttribute), true)))
                {
                    RegisterType(messageDispatcher, type);
                }
            }
        }

        /// <summary>
        ///     Registers the specified handler type to the message dispatcher.
        /// </summary>
        /// <param name="messageDispatcher">Message dispatcher instance.</param>
        /// <param name="handlerType">The type to be registered.</param>
        private static void RegisterType(IMessageDispatcher messageDispatcher, Type handlerType)
        {
            MethodInfo methodInfo = messageDispatcher.GetType().GetMethod("Register", BindingFlags.Public | BindingFlags.Instance);

            IEnumerable<Type> handlerIntfTypeQuery = handlerType.GetInterfaces().Where(p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(IHandler<>));

            foreach (Type handlerIntfType in handlerIntfTypeQuery)
            {
                object handlerInstance = Activator.CreateInstance(handlerType);
                Type messageType = handlerIntfType.GetGenericArguments().First();
                MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(messageType);
                genericMethodInfo.Invoke(messageDispatcher, new[] { handlerInstance });
            }
        }
    }
}