using System;
using System.Collections.Generic;
using System.Linq;
using Event = Core.Events.Event;

namespace Core.EventMap.Impl
{
    public sealed class EventMap : IEventMap
    {
        private readonly Dictionary<Event, object> _listeners;

        public EventMap()
        {
            _listeners = new Dictionary<Event, object>();
        }
        
        public void Map(Event @event, Action action)
        {
            if (!_listeners.TryGetValue(@event, out var listenersObj))
            {
                _listeners.Add(@event, action);
                return;
            }

            var listeners = (Action)listenersObj;
            if (!listeners.GetInvocationList().Contains(action))
                _listeners[@event] = listeners + action;
        }

        public void Map<T1>(Event @event, Action<T1> action)
        {
            if (!_listeners.TryGetValue(@event, out var listenersObj))
            {
                _listeners.Add(@event, action);
                return;
            }

            var listeners = (Action<T1>)listenersObj;
            if (!listeners.GetInvocationList().Contains(action))
                _listeners[@event] = listeners + action;
        }

        public void Map<T1, T2>(Event @event, Action<T1, T2> action)
        {
            if (!_listeners.TryGetValue(@event, out var listenersObj))
            {
                _listeners.Add(@event, action);
                return;
            }

            var listeners = (Action<T1, T2>)listenersObj;
            if (!listeners.GetInvocationList().Contains(action))
                _listeners[@event] = listeners + action;
        }

        public void UnMap(Event @event, Action action)
        {
            if (!_listeners.TryGetValue(@event, out var listenersObj))
                return;

            var listeners = (Action)listenersObj;
            if (listeners.GetInvocationList().Contains(action))
                _listeners[@event] = listeners - action;
        }

        public void UnMap<T1>(Event @event, Action<T1> action)
        {
            if (!_listeners.TryGetValue(@event, out var listenersObj))
                return;

            var listeners = (Action<T1>)listenersObj;
            if (listeners.GetInvocationList().Contains(action))
                _listeners[@event] = listeners - action;
        }

        public void UnMap<T1, T2>(Event @event, Action<T1, T2> action)
        {
            if (!_listeners.TryGetValue(@event, out var listenersObj))
                return;

            var listeners = (Action<T1, T2>)listenersObj;
            if (listeners.GetInvocationList().Contains(action))
                _listeners[@event] = listeners - action;
        }

        public void Dispatch(Event @event)
        {
            if (!_listeners.TryGetValue(@event, out var listenersObj))
                return;
            
            var listeners = (Action)listenersObj;
            listeners?.Invoke();
        }

        public void Dispatch<T1>(Event @event, T1 param01)
        {
            if (!_listeners.TryGetValue(@event, out var listenersObj))
                return;
            
            var listeners = (Action<T1>)listenersObj;
            listeners?.Invoke(param01);
        }

        public void Dispatch<T1, T2>(Event @event, T1 param01, T2 param02)
        {
            if (!_listeners.TryGetValue(@event, out var listenersObj))
                return;
            
            var listeners = (Action<T1, T2>)listenersObj;
            listeners?.Invoke(param01, param02);
        }
    }
}
