using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Attributes.Inject;
using UnityEngine;

namespace Core.Engine
{
    public partial class Engine
    {
        private InjectionBinder.Impl.InjectionBinder InjectionBinder;
        
        private List<Module.Module> _modules;

        private static Engine _instance;

        private Engine Init()
        {
            CreateInjectionBinder();
            DontDestroyOnLoad(this);
            _instance = this;
            return this;
        }

        private void Build()
        {
            foreach (var module in _modules)
            {
                var type = module.GetType();
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(f => f.IsDefined(typeof(InjectAttribute))).ToList();

                foreach (var propertyInfo in properties)
                {
                    if(!propertyInfo.IsDefined(typeof(InjectAttribute), true))
                        continue;

                    var propertyType = propertyInfo.PropertyType;
                    var injection = InjectionBinder.GetInjection(propertyType);
                
                    propertyInfo.SetValue(module, injection);
                }
                
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(f => f.IsDefined(typeof(PostConstructAttribute))).ToList();
                
                if(methods.Count == 0)
                    continue;
                
                foreach (var methodInfo in methods)
                {
                    if(!methodInfo.IsDefined(typeof(PostConstructAttribute), true))
                        continue;

                    methodInfo.Invoke(module, null);
                }
            }
            
            InjectionBinder.ForeachBinding(x =>
            {
                var properties = x.Properties;
                foreach (var propertyInfo in properties)
                {
                    if(!propertyInfo.IsDefined(typeof(InjectAttribute), true))
                        continue;

                    var propertyType = propertyInfo.PropertyType;
                    var injection = InjectionBinder.GetInjection(propertyType);
                
                    propertyInfo.SetValue(x.Instance, injection);
                }

                var methods = x.Methods;
                
                foreach (var methodInfo in methods)
                {
                    if(!methodInfo.IsDefined(typeof(PostConstructAttribute), true))
                        continue;

                    methodInfo.Invoke(x.Instance, null);
                }
            });
        }
        
        private void Dispose()
        {
            foreach (var module in _modules)
            {
                var type = module.GetType();
                
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(f => f.IsDefined(typeof(OnDestroyAttribute))).ToList();
                
                if(methods.Count == 0)
                    continue;
                
                foreach (var methodInfo in methods)
                {
                    if(!methodInfo.IsDefined(typeof(OnDestroyAttribute), true))
                        continue;

                    methodInfo.Invoke(module, null);
                }
            }
            _modules = null;
            
            InjectionBinder.ForeachBinding(x =>
            {
                var type = x.Instance.GetType();
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(f => f.IsDefined(typeof(OnDestroyAttribute))).ToList();
                foreach (var methodInfo in methods)
                {
                    if(!methodInfo.IsDefined(typeof(OnDestroyAttribute), true))
                        continue;

                    methodInfo.Invoke(x.Instance, null);
                }
            });
        }

        private void CreateInjectionBinder()
        {
            InjectionBinder = (InjectionBinder.Impl.InjectionBinder)Activator.CreateInstance(typeof(InjectionBinder.Impl.InjectionBinder));
        }
        
        private Engine AddModule<T>() where T : Module.Module
        {
            _modules ??= new List<Module.Module>();
            
            var module = (Module.Module)Activator.CreateInstance(typeof(T));
            _modules.Add(module);
            return this;
        }

        public static T Get<T>()
        {
            var injection = _instance.InjectionBinder.GetInjection<T>();
            if (injection == null)
                throw new NullReferenceException();

            return injection;
        }
    }
}
