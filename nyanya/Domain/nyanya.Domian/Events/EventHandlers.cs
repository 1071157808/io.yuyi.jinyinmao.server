// FileInformation: nyanya/Domian/EventHandlers.cs
// CreatedTime: 2014/07/10   12:20 AM
// LastUpdatedTime: 2014/07/12   2:37 PM

using System;
using System.Collections.Generic;
using System.Linq;

namespace Domian.Events
{
    public class EventHandlers : IEventHandlers
    {
        private readonly Dictionary<Type, List<object>> handlers = new Dictionary<Type, List<object>>();

        #region IEventHandlers Members

        public IEnumerable<Type> EventTypes
        {
            get { return this.handlers.Keys; }
        }

        public virtual void Clear()
        {
            this.handlers.Clear();
        }

        public IEnumerable<IEventHandler<T>> GetHandlers<T>() where T : IEvent
        {
            Type keyType = typeof(T);
            List<object> value;
            if (this.handlers.TryGetValue(keyType, out value))
            {
                return value.ConvertAll(input => (IEventHandler<T>)input);
            }

            return Enumerable.Empty<IEventHandler<T>>();
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

        #endregion IEventHandlers Members
    }
}