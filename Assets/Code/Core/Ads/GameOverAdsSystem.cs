using System;
using Core.Cells;
using Core.Figures;
using Core.Grid;
using EcsCore;
using Global;
using Global.Ads;
using Global.Settings.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Ads
{
    [EcsSystem(typeof(CoreModule))]
    public class GameOverAdsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsEventTable _eventTable;
        private AdsService _adsService;
        private readonly Action _onSuccess;
        private readonly Action _onFailed;
        private EcsFilter<Cell> _cells;
        private GridData _gridData;
        private MainScreenMono _mainScreen;
        private PlayerData _playerData;
        private CoreSettings _coreSettings;

        public GameOverAdsSystem()
        {
            _onSuccess = OnRewardedVideoSuccess;
            _onFailed = OnRewardVideoFailed;
        }

        public void Init()
        {
            _adsService.Init();
        }

        public void Run()
        {
            if (!_eventTable.Has<GameOverAdsSignal>())
                return;
            _adsService.ShowRewardedVideo(_onSuccess, _onFailed);
        }

        private void OnRewardedVideoSuccess()
        {
            ContinueGame();
        }
        private void ContinueGame()
        {
            var rowIndex = Math.Max(_gridData.Rows - 3 - _coreSettings.adsClearRows, 0);

            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                if (cell.row < rowIndex)
                    continue;
                cell.view.SetEmpty();
                if (_gridData.FillMatrix[cell.row, cell.column])
                    cell.view.PlayVfx();
            }

            for (var row = _gridData.Rows - 1; row >= rowIndex; --row)
            {
                for (var column = 0; column < _gridData.Columns; ++column)
                    _gridData.FillMatrix[row, column] = false;
            }

            _eventTable.AddEvent<ContinueForAdsSignal>();
            _playerData.AdsWasUsedInCore = true;
        }

        private void OnRewardVideoFailed()
        {
            // TODO: save that video failed
            ContinueGame();
        }
    }
}