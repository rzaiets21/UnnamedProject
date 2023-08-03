using System;
using Camera;
using Core.EventMap;
using Mouse;
using Units;
using UnityEngine;

namespace Core.Engine
{
    [DefaultExecutionOrder(-1000)]
    public partial class Engine : UnityEngine.MonoBehaviour
    {
        public static event Action OnUpdate;

        private void Awake()
        {
            Init()
                .AddModule<EventMapModule>()
                .AddModule<MouseModule>()
                .AddModule<CameraModule>()
                .AddModule<UnitsModule>()
                .Build();
        }

        private void OnDestroy()
        {
            Debug.Log("---OnDestroy---");
            Dispose();
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }
    }
}
