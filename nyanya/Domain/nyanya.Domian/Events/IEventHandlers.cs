using System;
using System.Collections.Generic;

namespace Domian.Events
{
    public interface IEventHandlers
    {
        IEnumerable<Type> EventTypes { get; }

        void Clear();

        IEnumerable<IEventHandler<T>> GetHandlers<T>() where T : IEvent;

        void Register<T>(IEventHandler<T> handler) where T : IEvent;

        void UnRegister<T>(IEventHandler<T> handler) where T : IEvent;
    }
}