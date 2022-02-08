using System.Collections.Generic;
using Core.GameOver;
using EcsCore;
using Global;
using Global.Saving;
using Leopotam.Ecs;
using UnityEngine;
using Utilities;

namespace Core.AI.Genetic
{
    [EcsSystem(typeof(MainModule))]
    public class AiGeneticSystem : IEcsInitSystem, IEcsRunSystem
    {
        private AiGeneticService _aiGeneticService;
        private EcsEventTable _eventTable;
        private EcsWorld _world;
        private PlayerData _playerData;
        private int _iterationsRemain = 10;
        private const string SAVE_FLAG_KEY = "AI_GENETIC_SAVE";
        private const string SAVE_GENERATION_KEY = "AI_GENETIC_GENERATION";
        
        public void Init()
        {
            _aiGeneticService.mutationFactor = .2f;
            if (SaveUtility.LoadInt(SAVE_FLAG_KEY) > 0)
            {
                // load 
                _aiGeneticService.LoadFromJson();
                _iterationsRemain = SaveUtility.LoadInt(SAVE_GENERATION_KEY);
            }
            else
            {
                _aiGeneticService.GenerateStartPopulation(25);
                _aiGeneticService.CrossPopulation();
                _aiGeneticService.Save();
                SaveUtility.SaveInt(SAVE_FLAG_KEY, 1);
                SaveUtility.SaveInt(SAVE_GENERATION_KEY, _iterationsRemain);
            }
        }

        public void Run()
        {
            // ждём конец игры
            if(!_eventTable.Has<GameOverSignal>())
                return;
            // записываем в текущую особь кол-во очков
            _aiGeneticService.currentIndividual.scores = _playerData.CurrentScores;
            Debug.Log(_aiGeneticService.currentIndividual);
            Debug.Log("Remain: " + _aiGeneticService.GetRemain());
            // если особи кончились
            if (!_aiGeneticService.IsHasMore())
            {
                _iterationsRemain--;
                SaveUtility.SaveInt(SAVE_GENERATION_KEY, _iterationsRemain);
                _playerData.GeneticScoreBreak *= 2;
                if (_iterationsRemain <= 0)
                {
                    Debug.Log(_aiGeneticService.GetBest());
                    SaveUtility.SaveInt(SAVE_FLAG_KEY, 0);
                    return;
                }
                _aiGeneticService.Reset();
                _aiGeneticService.MakeSelection(25);
                _aiGeneticService.CrossPopulation();
                _aiGeneticService.Save();
            }
                
            // берём следующую особь и запускаем игру заново
            _aiGeneticService.Next();
            _aiGeneticService.Save();
            _eventTable.AddEvent<StartCoreSignal>();
            _eventTable.AddEvent<RestartCoreSignal>();
            _world.DeactivateModule<CoreModule>();
        }
    }
}