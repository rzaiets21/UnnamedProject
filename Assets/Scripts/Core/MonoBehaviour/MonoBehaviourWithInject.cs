using System.Linq;
using System.Reflection;
using Core.Attributes.Inject;
using Core.InjectionBinder;
using EngineCore = Core.Engine.Engine;

namespace Core.MonoBehaviour
{
    public abstract class MonoBehaviourWithInject : UnityEngine.MonoBehaviour
    {
        private void Awake()
        {
            InjectionProcess();
            
            OnAwake();
        }

        private void Start() => OnStart();
        private void OnEnable() => OnEnabled();
        private void OnDisable() => OnDisabled();
        private void OnDestroy() => OnDestroyed();
        
        protected virtual void OnAwake(){ }
        protected virtual void OnStart(){ }
        protected virtual void OnEnabled(){ }
        protected virtual void OnDisabled(){ }
        protected virtual void OnDestroyed(){ }

        private void InjectionProcess()
        {
            var injectionBinder = EngineCore.Get<IInjectionBinder>();
            
            var type = GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.IsDefined(typeof(InjectAttribute))).ToList();

            foreach (var propertyInfo in properties)
            {
                if(!propertyInfo.IsDefined(typeof(InjectAttribute)))
                    continue;

                var propertyType = propertyInfo.PropertyType;
                var injection = injectionBinder.GetInjection(propertyType);
                
                propertyInfo.SetValue(this, injection);
            }
        }
    }
}
