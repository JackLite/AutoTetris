using System;
using System.Collections;
using System.Collections.Generic;
using Core.Figures;
using UnityEngine;
using Utilities;

namespace Global.Saving
{
    public class SaveService
    {
        private const string SCORES_KEY = "player.scores";
        private const string MAX_SCORES_KEY = "player.max_scores";
        private const string FILL_MATRIX_KEY = "core.fill_matrix";
        private const string FIGURE_KEY = "core.current_figure";
        private const string FIGURE_BAG_KEY = "core.figure_bag";

        public void SaveScores(int scores)
        {
            SaveUtility.SaveInt(SCORES_KEY, scores);
        }

        public int LoadScores()
        {
            return SaveUtility.GetInt(SCORES_KEY);
        }

        public void SaveMaxScores(int maxScores)
        {
            SaveUtility.SaveInt(MAX_SCORES_KEY, maxScores);
        }

        public int LoadMaxScores()
        {
            return SaveUtility.GetInt(MAX_SCORES_KEY);
        }


        public void SaveFillMatrix(bool[,] fillMatrix)
        {
            var bytes = new byte[fillMatrix.Length];
            var i = 0;
            var l1 = fillMatrix.GetLength(0);
            var l2 = fillMatrix.GetLength(1);
            for (var k = 0; k < l1; ++k)
            for (var j = 0; j < l2; ++j)
                bytes[i++] = Convert.ToByte(fillMatrix[k, j]);
            SaveUtility.SaveBytes(FILL_MATRIX_KEY, bytes);
        }

        public bool[,] LoadFillMatrix(int l1, int l2)
        {
            var bytes = SaveUtility.GetBytes(FILL_MATRIX_KEY);
            var fillMatrix = new bool[l1, l2];
            var i = 0;
            for (var k = 0; k < l1; ++k)
            for (var j = 0; j < l2; ++j)
                fillMatrix[k, j] = Convert.ToBoolean(bytes[i++]);
            return fillMatrix;
        }

        public void SaveCurrentFigure(in Figure figure)
        {
        }

        public Figure LoadFigure()
        {
            return new Figure();
        }

        public void SaveFigureBag(Stack<FigureType> bag)
        {
        }

        public Stack<FigureType> LoadFigureBag()
        {
            return new Stack<FigureType>();
        }

        public void Flush()
        {
            PlayerPrefs.Save();
        }
    }
}