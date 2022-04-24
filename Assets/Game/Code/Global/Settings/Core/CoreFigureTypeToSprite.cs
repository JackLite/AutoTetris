using System;
using Core.Figures;
using UnityEngine.AddressableAssets;

namespace Global.Settings.Core
{
    [Serializable]
    public struct CoreFigureTypeToSprite
    {
        public FigureType figureType;
        public AssetReference sprite;
    }
}