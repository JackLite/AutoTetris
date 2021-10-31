using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Leopotam.Ecs;
using UnityEngine;

namespace EcsCore
{
    /// <summary>
    ///     Отвечает за запуск ECS-мира
    ///     Создаёт, обрабатывает и удаляет системы
    /// </summary>
    public class EcsWorldContainer : MonoBehaviour
    {
        private static readonly Lazy<EcsWorld> LazyWorld = new Lazy<EcsWorld>();
        public static readonly EcsWorld world = LazyWorld.Value;
        private bool _isInitialize;
        private EcsSystems _systems;

        private async void Awake()
        {
            _systems = new EcsSystems(world);
            var setups = GetAllEcsSetups();

            foreach (var type in setups)
                await type.Setup(_systems);

            _systems.Init();
            _isInitialize = true;
        }

        private void Update()
        {
            if (!_isInitialize)
                return;
            _systems.Run();
            EcsWorldEventsBlackboard.Update();
        }

        private void FixedUpdate()
        {
            if (!_isInitialize)
                return;
            _systems.RunPhysics();
        }

        private void OnDestroy()
        {
            if (!_isInitialize)
                return;
            _systems.Destroy();
            world.Destroy();
        }

        private static IEnumerable<EcsSetup> GetAllEcsSetups()
        {
            return Assembly.GetExecutingAssembly()
                           .GetTypes()
                           .Where(t => t.IsSubclassOf(typeof(EcsSetup)))
                           .Select(t => (EcsSetup)Activator.CreateInstance(t));
        }
    }
}