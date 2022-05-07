using Core.Figures;
using Core.GameOver;
using Core.GameOver.Components;
using Core.Grid;
using EcsCore;
using Global;
using Global.Settings.Core;
using Leopotam.Ecs;
using UnityEngine;
using Utilities;

namespace Core.AI.Genetic
{
    [EcsSystem(typeof(CoreModule))]
    public class AiGeneticSystem : IEcsInitSystem, IEcsRunSystem
    {
        private AiGeneticService _aiGeneticService;
        private EcsEventTable _eventTable;
        private EcsWorld _world;
        private PlayerData _playerData;
        private GridData _grid;
        private CoreSettings _settings;
        private MainScreenMono _mainScreen;
        private int _iterationsRemain;
        private const string SAVE_FLAG_KEY = "AI_GENETIC_SAVE";
        private const string SAVE_GENERATION_KEY = "AI_GENETIC_GENERATION";
        private const int GENERATIONS = 20;
        private const int START_POPULATION_SIZE = 800;
        private const int POPULATION_SIZE = 7;

        public void Init()
        {
            _mainScreen.GeneticText.gameObject.SetActive(_settings.aiEnable);
            if (!_settings.aiEnable)
                return;
            _aiGeneticService.mutationFactor = .4f;
            if (SaveUtility.LoadInt(SAVE_FLAG_KEY) > 0)
            {
                // load 
                _aiGeneticService.LoadFromJson();
                _iterationsRemain = SaveUtility.LoadInt(SAVE_GENERATION_KEY);
                ShowProgress();
            }
            else
            {
                _iterationsRemain = GENERATIONS;
                _aiGeneticService.GenerateStartPopulation(START_POPULATION_SIZE);
                _aiGeneticService.Save();
                SaveUtility.SaveInt(SAVE_FLAG_KEY, 1);
                SaveUtility.SaveInt(SAVE_GENERATION_KEY, _iterationsRemain);
                LogGenerations();
                ShowProgress();
            }
        }

        public void Run()
        {
            if (!_settings.aiEnable)
                return;

            if (_eventTable.Has<FigureSpawnSignal>())
            {
                _aiGeneticService.currentIndividual.turns++;
                if (_aiGeneticService.currentIndividual.turns > 0)
                {
                    var height = GridService.FindTopNotEmptyRow(_grid.FillMatrix);
                    _aiGeneticService.currentIndividual.heights.Add(height);
                    _aiGeneticService.currentIndividual.CalculateAvgHeight();
                }
            }
            if (!_eventTable.Has<GameOverCoreSignal>() && _aiGeneticService.currentIndividual.turns < 2000 && _aiGeneticService.IsHasMore())
                return;

            // если особи кончились
            if (!_aiGeneticService.IsHasMore())
            {
                _iterationsRemain--;
                SaveUtility.SaveInt(SAVE_GENERATION_KEY, _iterationsRemain);
                if (_iterationsRemain <= 0)
                {
                    Log("Best: " + _aiGeneticService.GetBest().ToString());
                    _mainScreen.GeneticText.text = _aiGeneticService.GetBest().ToString();
                    SaveUtility.SaveInt(SAVE_FLAG_KEY, 0);
                    _world.DeactivateModule<CoreModule>();
                    return;
                }
                _aiGeneticService.Reset();
                _aiGeneticService.MakeSelection(POPULATION_SIZE);
                _aiGeneticService.CrossPopulation();
                _aiGeneticService.Save();
                LogGenerations();
                return;
            }

            Log(_aiGeneticService.currentIndividual.ToString());
            Log("Remain: " + _aiGeneticService.GetRemain());
            ShowProgress();
            
            // берём следующую особь и запускаем игру заново
            _aiGeneticService.Next();
            _aiGeneticService.Save();
            _eventTable.AddEvent<StartCoreSignal>();
            _world.DeactivateModule<CoreModule>();
        }
        private void ShowProgress()
        {
            _mainScreen.GeneticText.text =
                $"Generation: {GENERATIONS + 1 - _iterationsRemain}; Remain: {_aiGeneticService.GetRemain()}";
        }

        private void LogGenerations()
        {
            Log($"Generation: {(GENERATIONS + 1 - _iterationsRemain).ToString()}");
        }

        private static void Log(string text)
        {
            Debug.Log("[Genetic] " + text);
        }
    }
}