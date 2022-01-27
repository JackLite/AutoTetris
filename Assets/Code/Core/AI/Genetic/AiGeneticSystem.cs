using Core.GameOver;
using EcsCore;
using Global;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.AI.Genetic
{
    [EcsSystem(typeof(MainModule))]
    public class AiGeneticSystem : IEcsInitSystem, IEcsRunSystem
    {
        private AiGeneticService aiGeneticService;
        private EcsEventTable _eventTable;
        private EcsWorld _world;
        private PlayerData _playerData;
        private int _iterationsRemain = 5;
        
        public void Init()
        {
            aiGeneticService.mutationFactor = .2f;
            aiGeneticService.GenerateStartPopulation(25);
            aiGeneticService.CrossPopulation();
        }

        public void Run()
        {
            // ждём конец игры
            if(!_eventTable.Has<GameOverSignal>())
                return;
            // записываем в текущую особь кол-во очков
            aiGeneticService.currentIndividual.scores = _playerData.CurrentScores;
            Debug.Log(aiGeneticService.currentIndividual);
            Debug.Log("Remain: " + aiGeneticService.GetRemain());
            // если особи кончились
            if (!aiGeneticService.IsHasMore())
            {
                _iterationsRemain--;
                if (_iterationsRemain <= 0)
                {
                    Debug.Log(aiGeneticService.GetBest());
                    return;
                }
                aiGeneticService.Reset();
                aiGeneticService.MakeSelection(25);
                aiGeneticService.CrossPopulation();
            }
                
            // берём следующую особь и запускаем игру заново
            aiGeneticService.Next();
            _eventTable.AddEvent<StartCoreSignal>();
            _eventTable.AddEvent<RestartCoreSignal>();
            _world.DeactivateModule<CoreModule>();
        }
    }
}