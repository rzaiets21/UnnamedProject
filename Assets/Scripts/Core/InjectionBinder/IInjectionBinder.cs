using System;

namespace Core.InjectionBinder
{
    public interface IInjectionBinder
    {
        void Bind<T1, T2>();

        void Unbind<T1, T2>();

        T1 GetInjection<T1>();
        object GetInjection(Type type);
    }
}
