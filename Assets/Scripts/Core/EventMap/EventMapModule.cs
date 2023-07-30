using Core.Attributes.Inject;
using Core.InjectionBinder;
using UnityEngine;

namespace Core.EventMap
{
    public sealed class EventMapModule : Module.Module
    {
        [Inject] private IInjectionBinder InjectionBinder { get; set; }

        [PostConstruct]
        private void PostConstruct()
        {
            InjectionBinder.Bind<IEventMap, Impl.EventMap>();
        }
    }
}
