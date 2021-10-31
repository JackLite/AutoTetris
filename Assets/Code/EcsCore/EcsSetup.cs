using System;
using System.Threading.Tasks;
using Leopotam.Ecs;

namespace EcsCore
{
    /// <summary>
    /// Базовый класс для всех точек создания ECS-систем
    /// </summary>
    public abstract class EcsSetup
    {
        protected abstract Type Type { get; }

        /// <summary>
        /// Создаёт системы и добавляет их в набор
        /// </summary>
        /// <param name="systems">Набор ECS-систем</param>
        public async Task Setup(EcsSystems systems)
        {
            await Setup();
            foreach (var system in EcsUtilities.CreateSystems(Type))
            {
                systems.Add(system);
                InsertDependencies(system);
            }
        }

        protected virtual async Task Setup()
        {
            await Task.CompletedTask;
        }

        protected virtual async void InsertDependencies(IEcsSystem system)
        {
            
        }
    }
}