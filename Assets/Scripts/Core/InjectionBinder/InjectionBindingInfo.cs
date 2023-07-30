using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Attributes.Inject;

namespace Core.Module
{
    public sealed class InjectionBindingInfo
    {
        public readonly object Instance;
        public readonly Type Type;
        public List<PropertyInfo> Properties;
        public List<MethodInfo> Methods;

        public InjectionBindingInfo(Type type, object instance)
        {
            Type = type;
            Instance = instance;
            InitInjections();
            InitMethods();
        }
        
        private void InitInjections()
        {
            var type = Instance.GetType();
            Properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.IsDefined(typeof(InjectAttribute))).ToList();
        }

        private void InitMethods()
        {
            var type = Instance.GetType();
            Methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.IsDefined(typeof(PostConstructAttribute))).ToList();
        }
    }
}
