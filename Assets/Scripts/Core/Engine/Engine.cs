using Core.EventMap;
using Settings;
using UnityEngine;

namespace Core.Engine
{
    [DefaultExecutionOrder(-1000)]
    public partial class Engine : UnityEngine.MonoBehaviour
    {
        private void Awake()
        {
            Init()
                .AddModule<EventMapModule>()
                .Build();
        }
    }
}
