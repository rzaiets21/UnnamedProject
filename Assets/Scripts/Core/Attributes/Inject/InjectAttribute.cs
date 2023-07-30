using System;

namespace Core.Attributes.Inject
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class InjectAttribute : Attribute
    {
        public InjectAttribute()
        {
        
        }
    }
}
