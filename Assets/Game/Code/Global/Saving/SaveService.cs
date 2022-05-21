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
        private const string COLORS_KEY = "core.colors";
        private const string FALL_SPEED = "core.fall_speed";
        private const string AUDIO_MUSIC_KEY = "global.music.state";
        private const string AUDIO_SOUND_KEY = "global.sound.state";

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

        public void SaveCells(FigureType[,] types)
        {
            var list = new List<int>(types.Length);
            var i = 0;
            var l1 = types.GetLength(0);
            var l2 = types.GetLength(1);
            for (var k = 0; k < l1; ++k)
            for (var j = 0; j < l2; ++j)
                list.Add((int) types[k, j]);
            SaveUtility.SaveList(COLORS_KEY, list);
        }

        public FigureType[,] LoadCells(int l1, int l2)
        {
            var list = SaveUtility.LoadList<int>(COLORS_KEY);
            if (list == null)
                return null;
            var types = new FigureType[l1, l2];
            var i = 0;
            for (var k = 0; k < l1; ++k)
            for (var j = 0; j < l2; ++j)
                types[k, j] = (FigureType) list[i++];
            return types;
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

        public void SaveMusicState(bool isActive)
        {
            SaveUtility.SaveBool(AUDIO_MUSIC_KEY, isActive);
        }

        public void SaveSoundState(bool isActive)
        {
            SaveUtility.SaveBool(AUDIO_SOUND_KEY, isActive);
        }

        public bool GetMusicState()
        {
            return SaveUtility.LoadBool(AUDIO_MUSIC_KEY);
        }

        public bool GetSoundState()
        {
            return SaveUtility.LoadBool(AUDIO_SOUND_KEY);
        }

        public float GetFallSpeedFactor()
        {
            return SaveUtility.LoadFloat(FALL_SPEED);
        }

        public void SaveFallSpeedFactor(float speed)
        {
            SaveUtility.SaveFloat(FALL_SPEED, speed);
        }
    }
}