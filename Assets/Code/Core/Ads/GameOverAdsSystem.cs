using System;
using Core.Cells;
using Core.Figures;
using Core.Grid;
using EcsCore;
using Global;
using Global.Ads;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Ads
{
    [EcsSystem(typeof(CoreModule))]
    public class GameOverAdsSystem : IEcsRunSystem
    {
        private EcsEventTable _eventTable;
        private AdsService _adsService;
        private readonly Action _onSuccess;
        private EcsFilter<Cell> _cells;
        private GridData _gridData;
        private PlayerData _playerData;

        public GameOverAdsSystem()
        {
            _onSuccess = OnRewardedVideoSuccess;
        }

        public void Run()
        {
            if (!_eventTable.Has<GameOverAdsSignal>())
                return;
            _adsService.ShowRewardedVideo(_onSuccess);
        }

        private void OnRewardedVideoSuccess()
        {
            var rowIndex = (_gridData.Rows - 3) / 2;
            for (var row = _gridData.Rows - 1; row >= rowIndex; --row)
            {
                for (var column = 0; column < _gridData.Columns; ++column)
                    _gridData.FillMatrix[row, column] = false;
                Debug.Log("Clear row " + row);
            }
            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                if (cell.Row >= rowIndex)
                {
                    Debug.Log("Clear cell at row " + cell.Row);
                    cell.View.SetEmpty();
                }
            }
            _eventTable.AddEvent<ContinueForAdsSignal>();
            _playerData.AdsWasUsedInCore = true;
        }
    }
}