using System;
using System.Collections.Generic;
using System.Linq;
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

        public static Dictionary<TKey, TValue> CreateMap<TData, TKey, TValue>(
            this IEnumerable<TData> collection,
            Func<TData, TKey> keyGetter,
            Func<TData, TValue> valueGetter)
        {
            return collection.ToDictionary(keyGetter, valueGetter);
        }
    }
}