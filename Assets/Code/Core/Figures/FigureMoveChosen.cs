﻿using System.Collections.Generic;
using Core.Path;

namespace Core.Figures
{
    /// <summary>
    /// Обозначает что фигура движется в выбранное место
    /// </summary>
    public struct FigureMoveChosen
    {
        public Stack<PathAction> Actions;
    }
}