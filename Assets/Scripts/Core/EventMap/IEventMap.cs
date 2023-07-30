using System;
using Core.Events;

namespace Core.EventMap
{
    public interface IEventMap
    {
        void Map(Event @event, Action @action);
        void Map<T1>(Event @event, Action<T1> @action);
        void Map<T1, T2>(Event @event, Action<T1, T2> @action);
        
        void UnMap(Event @event, Action @action);
        void UnMap<T1>(Event @event, Action<T1> @action);
        void UnMap<T1, T2>(Event @event, Action<T1, T2> @action);
        
        void Dispatch(Event @event);
        void Dispatch<T1>(Event @event, T1 param01);
        void Dispatch<T1, T2>(Event @event, T1 param01, T2 param02);
    }
}
