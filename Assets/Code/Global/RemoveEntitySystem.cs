using EcsCore;
using Leopotam.Ecs;

namespace Global
{
    [EcsSystem(typeof(MainModule))]
    public class RemoveEntitySystem : IEcsRunSystem
    {
        private EcsFilter<RemoveTag> _removeFilter;


        public void Run()
        {
            foreach (var i in _removeFilter)
            {
                _removeFilter.GetEntity(i).Destroy();
            }
        }
    }
}