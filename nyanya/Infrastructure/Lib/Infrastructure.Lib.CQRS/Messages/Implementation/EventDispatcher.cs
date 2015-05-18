// FileInformation: nyanya/Infrastructure.Lib.CQRS/EventDispatcher.cs
// CreatedTime: 2014/06/13   5:14 PM
// LastUpdatedTime: 2014/06/19   9:29 AM

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
    public class EventDispatcher : IEventDispatcher
    {
        private readonly EventLogStore eventLogStore;
        private readonly Dictionary<Type, List<object>> handlers = new Dictionary<Type, List<object>>();
        private readonly ILogger logger;

        public EventDispatcher()
        {
            this.eventLogStore = new EventLogStore();
            //this.logger = CqrsConfigration.Loggers.EventDispatcherLogger;
        }

        #region IEventDispatcher Members

        /// <summary>
        ///     Dispatches the event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event">The event to be dispatched.</param>
        /// <returns>
        ///     Dispatch Successed
        /// </returns>
        public virtual bool Dispatch<T>(T @event) where T : IEvent
        {
            try
            {
                // 解析出事件实例所实现的所有时间接口
                Type eventType = typeof(T);
                List<Type> eventTypes = eventType.GetInterfaces().Where(i => i.IsSubclassOf(typeof(IEvent))).ToList();

                eventTypes.Add(eventType);

                foreach (Type t in eventTypes)
                {
                    List<object> value;

                    // 对于时间而言，非常有可能很多事件类型是没有注册handler的，这一点与command不同
                    if (this.handlers.TryGetValue(t, out value))
                    {
                        List<object> eventHandlers = value;
                        foreach (object handler in eventHandlers)
                        {
                            IEventHandler<T> eventHandler = (IEventHandler<T>)handler;
                            try
                            {
                                eventHandler.Handle(@event);
                                // 这里只记录日志
                                this.OnDispatched(@event, eventHandler);
                            }
                            catch (Exception e)
                            {
                                // 这里只记录日志
                                this.OnDispatchFailed(@event, eventHandler, e);
                            }
                        }
                    }
                }

                this.OnDispatched(@event);
                return true;
            }
            catch (Exception)
            {
                this.OnDispatchFailed(@event);
                return false;
            }
        }

        public void Register<T>(IEventHandler<T> handler) where T : IEvent
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

        public void UnRegister<T>(IEventHandler<T> handler) where T : IEvent
        {
            Type keyType = typeof(T);
            List<object> value;
            if (this.handlers.TryGetValue(keyType, out value) && value != null && value.Count > 0 && value.Contains(handler))
            {
                value.Remove(handler);
            }
        }

        #endregion IEventDispatcher Members

        /// <summary>
        ///     Creates a event dispatcher and registers all the event handlers
        ///     specified in the <see cref="Infrastructure.Lib.CQRS.Config.IConfigSource" /> instance.
        /// </summary>
        /// <param name="eventDispatcherType">The type of the event dispatcher.</param>
        /// <param name="args">The arguments that is used for initializing the event dispatcher.</param>
        /// <returns>A <see cref="IEventDispatcher" /> instance.</returns>
        public static IEventDispatcher CreateAndRegister(Type eventDispatcherType, params object[] args)
        {
            IEventDispatcher eventDispatcher = (IEventDispatcher)Activator.CreateInstance(eventDispatcherType, args);

            Collection<EventHandlerElement> handlerElementCollection = null;//CqrsConfigration.Config.EventHandlers;
            foreach (EventHandlerElement handlerElement in handlerElementCollection)
            {
                switch (handlerElement.SourceType)
                {
                    case HandlerSourceType.Type:
                        string typeName = handlerElement.Source;
                        Type handlerType = Type.GetType(typeName);
                        RegisterType(eventDispatcher, handlerType);
                        break;

                    case HandlerSourceType.Assembly:
                        string assemblyString = handlerElement.Source;
                        Assembly assembly = Assembly.Load(assemblyString);
                        RegisterAssembly(eventDispatcher, assembly);
                        break;
                }
            }

            return eventDispatcher;
        }

        public virtual void Clear()
        {
            this.handlers.Clear();
        }

        /// <summary>
        ///     Registers all the handler types within a given assembly to the event dispatcher.
        /// </summary>
        /// <param name="eventDispatcher">Event dispatcher instance.</param>
        /// <param name="assembly">The assembly.</param>
        private static void RegisterAssembly(IEventDispatcher eventDispatcher, Assembly assembly)
        {
            foreach (Type type in assembly.GetExportedTypes())
            {
                Type[] intfs = type.GetInterfaces();
                if (intfs.Any(p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(IEventHandler<>)) && intfs.Any(p => p.IsDefined(typeof(RegisterEventDispatchAttribute), true)))
                {
                    RegisterType(eventDispatcher, type);
                }
            }
        }

        /// <summary>
        ///     Registers the specified handler type to the event dispatcher.
        /// </summary>
        /// <param name="eventDispatcher">Event dispatcher instance.</param>
        /// <param name="handlerType">The type to be registered.</param>
        private static void RegisterType(IEventDispatcher eventDispatcher, Type handlerType)
        {
            //MethodInfo methodInfo = eventDispatcher.GetType().GetMethod("Register", BindingFlags.Public | BindingFlags.Instance);

            //IEnumerable<Type> handlerIntfTypeQuery = handlerType.GetInterfaces().Where(p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(IEventHandler<>));

            //foreach (Type handlerIntfType in handlerIntfTypeQuery)
            //{
            //    object handlerInstance = CqrsConfigration.Config.DependencyResolver.GetService(handlerType) ??
            //                             Activator.CreateInstance(handlerType);
            //    Type commandType = handlerIntfType.GetGenericArguments().First();
            //    MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(commandType);
            //    genericMethodInfo.Invoke(eventDispatcher, new[] { handlerInstance });
            //}
        }

        private void OnDispatched(IEvent @event, IEventHandler eventHandler)
        {
            // ReSharper disable once CSharpWarnings::CS4014
            this.logger.Info("Dispatched Event {0} To Handler {1}.", @event.Guid, eventHandler.Name);
        }

        private void OnDispatched(IEvent @event)
        {
            // ReSharper disable once CSharpWarnings::CS4014
            this.eventLogStore.Delivered(@event);
        }

        private void OnDispatchFailed(IEvent @event, IEventHandler eventHandler, Exception e)
        {
            // ReSharper disable once CSharpWarnings::CS4014
            this.eventLogStore.Delivered(@event, false);
            this.logger.Error(e, "Exception Duraing Dispatching Event {0} To Handler {1}.\n{2}", @event.Guid, eventHandler.Name, e.Message);
        }

        private void OnDispatchFailed(IEvent @event)
        {
            // ReSharper disable once CSharpWarnings::CS4014
            this.eventLogStore.Delivered(@event, false);
            this.logger.Error("Can not find Event Handler During Sending Event {0}.", @event.Guid);
        }
    }
}