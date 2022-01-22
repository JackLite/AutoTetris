using System;
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
        private const string UNFINISHED_GAME = "core.has_figure";
        private const string HAS_FIGURE_KEY = "core.has_saved_game";

        public void SetHasGame(bool isHasGame)
        {
            SaveUtility.SaveInt(UNFINISHED_GAME, isHasGame ? 1 : 0, true);
        }

        public bool HasGame()
        {
            return SaveUtility.LoadInt(UNFINISHED_GAME) > 0;
        }

        public void SetHasFigure(bool isHasGame)
        {
            SaveUtility.SaveInt(HAS_FIGURE_KEY, isHasGame ? 1 : 0, true);
        }

        public bool HasFigure()
        {
            return SaveUtility.LoadInt(HAS_FIGURE_KEY) > 0;
        }


        public void SaveScores(int scores)
        {
            SaveUtility.SaveInt(SCORES_KEY, scores);
        }

        public int LoadScores()
        {
            return SaveUtility.LoadInt(SCORES_KEY);
        }

        public void SaveMaxScores(int maxScores)
        {
            SaveUtility.SaveInt(MAX_SCORES_KEY, maxScores);
        }

        public int LoadMaxScores()
        {
            return SaveUtility.LoadInt(MAX_SCORES_KEY);
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
            var bytes = SaveUtility.LoadBytes(FILL_MATRIX_KEY);
            var fillMatrix = new bool[l1, l2];
            var i = 0;
            for (var k = 0; k < l1; ++k)
            for (var j = 0; j < l2; ++j)
                fillMatrix[k, j] = Convert.ToBoolean(bytes[i++]);
            return fillMatrix;
        }

        public void SaveCurrentFigure(Figure figure)
        {
            figure.mono = null;
            var saved = JsonUtility.ToJson(figure);
            SaveUtility.SaveString(FIGURE_KEY, saved);
        }

        public Figure LoadFigure()
        {
            var serialized = SaveUtility.LoadString(FIGURE_KEY);
            return JsonUtility.FromJson<Figure>(serialized);
        }

        public void SaveFigureBag(Stack<FigureType> bag)
        {
            var bytes = new byte[bag.Count];
            var i = 0;
            foreach (var type in bag)
            {
                bytes[i++] = (byte) type;
            }

            SaveUtility.SaveBytes(FIGURE_BAG_KEY, bytes);
        }

        public Stack<FigureType> LoadFigureBag()
        {
            var bytes = SaveUtility.LoadBytes(FIGURE_BAG_KEY);
            var bag = new Stack<FigureType>();
            for (var i = bytes.Length - 1; i >= 0; --i)
                bag.Push((FigureType) bytes[i]);
            return bag;
        }

        public void Flush()
        {
            PlayerPrefs.Save();
        }
    }
}