using System;
using GooglePlayGames.Android;
using GooglePlayGames.BasicApi;
using UnityEngine;
using Random = System.Random;

namespace Utilities
{
    public static class Extensions
    {
        public static void Shuffle<T> (this Random rnd, T[] array)
        {
            var n = array.Length;
            while (n > 1) 
            {
                var k = rnd.Next(n--);
                (array[n], array[k]) = (array[k], array[n]);
            }
        }
    }
}