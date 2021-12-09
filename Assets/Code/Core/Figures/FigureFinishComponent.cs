using System.Collections.Generic;
using System.Collections.Generic;
using Core.Figures.FigureAlgorithms.Path;

namespace Core.Figures
{
    /// <summary>
    /// Обозначает что фигура движется в выбранное место
    /// </summary>
    public struct FigureFinishComponent
    {
        public Stack<PathAction> Actions;
    }
}