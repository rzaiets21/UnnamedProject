using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Attributes.Inject;
using Core.Module;
using UnityEngine;

namespace Core.InjectionBinder.Impl
{
    public sealed class InjectionBinder : IInjectionBinder
    {
        private readonly List<InjectionBindingInfo> _injections;

        public InjectionBinder()
        {
            _injections = new List<InjectionBindingInfo>
            {
                new(typeof(IInjectionBinder), this)
            };
        }

        public void Bind<T1, T2>()
        {
            var interfaceType = typeof(T1);
            var objectType = typeof(T2);
            
            if(!objectType.GetInterfaces().Contains(interfaceType))
                return;

            var injection = Activator.CreateInstance(objectType);
            _injections.Add(new InjectionBindingInfo(interfaceType, injection));
        }

        public void Unbind<T1, T2>()
        {
            throw new System.NotImplementedException();
        }

        public T1 GetInjection<T1>()
        {
            var interfaceType = typeof(T1);
            if (_injections.All(x => x.Type != interfaceType))
            {
                throw new NullReferenceException($"Injection {interfaceType} not found!");
            }

            var injectionInfo = _injections.First(x => x.Type == interfaceType);
            
            return (T1)injectionInfo.Instance;
        }
        
        public object GetInjection(Type type)
        {
            var interfaceType = type;
            if (_injections.All(x => x.Type != interfaceType))
            {
                throw new NullReferenceException($"Injection {interfaceType} not found!");
            }
            
            var injectionInfo = _injections.First(x => x.Type == interfaceType);
            
            return injectionInfo.Instance;
        }

        public void ForeachBinding(Action<InjectionBindingInfo> action)
        {
            foreach (var injection in _injections)
            {
                action?.Invoke(injection);
            }
        }
    }
}
