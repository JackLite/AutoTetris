using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Core.AI.Genetic
{
    public class AiGeneticService
    {
        public GeneticIndividual currentIndividual;
        public float mutationFactor;
        private List<GeneticIndividual> _population;
        private int _currentIndex;
        private const string SAVE_POPULATION_KEY = "AI_GENETIC_POPULATION";

        public AiGeneticService()
        {
            _population = new List<GeneticIndividual>();
        }

        public void GenerateStartPopulation(int count)
        {
            _currentIndex = 0;
            var i = 0;
            while (i < count)
            {
                var ah = Random.Range(-5f, 0);
                var lines = Random.Range(0f, 5f);
                var height = Random.Range(-5f, 0);
                var bumpiness = Random.Range(-5f, 0);
                var individual = new GeneticIndividual(ah, lines, height, bumpiness);
                _population.Add(individual);
                i++;
            }
            currentIndividual = _population.First();
        }

        public bool IsHasMore()
        {
            return _currentIndex < _population.Count - 1;
        }

        public void CrossPopulation()
        {
            var count = _population.Count;
            for (var i = 0; i < count; ++i)
            {
                var parent1 = _population[i];
                for (var j = i + 1; j < count; ++j)
                {
                    var parent2 = _population[j];
                    var children = Cross(parent1, parent2);
                    foreach (var child in children)
                        _population.Add(child);
                }
            }

            for (var i = 0; i < (int) (_population.Count * mutationFactor); ++i)
            {
                _population.Add(Mutate(_population[Random.Range(0, _population.Count)]));
            }
        }
        private GeneticIndividual Mutate(GeneticIndividual child)
        {
            var r = Random.Range(0, .8f);
            var ah = child.ah;
            var lines = child.lines;
            var holes = child.holes;
            var bumpiness = child.bumpiness;
            if (r < .2f)
                ah = Random.Range(-5f, 0);
            else if (r < .4f)
                lines = Random.Range(0, 5);
            else if (r < .6f)
                holes = Random.Range(-5f, 0);
            else if (r < .8f)
                bumpiness = Random.Range(-5f, 0);

            return new GeneticIndividual(ah, lines, holes, bumpiness);
        }

        private GeneticIndividual[] Cross(GeneticIndividual p1, GeneticIndividual p2)
        {
            if (p1.Equals(p2))
                p2 = Mutate(p2);

            var ahDiff = math.abs(p1.ah - p2.ah) / 2;
            var linesDiff = math.abs(p1.lines - p2.lines) / 2;
            var holesDiff = math.abs(p1.holes - p2.holes) / 2;
            var bumpinessDiff = math.abs(p1.bumpiness - p2.bumpiness) / 2;
            return new[]
            {
                new GeneticIndividual(p1.ah + ahDiff, p1.lines, p1.holes, p1.bumpiness),
                new GeneticIndividual(p2.ah + ahDiff, p2.lines, p2.holes, p2.bumpiness),
                new GeneticIndividual(p1.ah - ahDiff, p1.lines, p1.holes, p1.bumpiness),
                new GeneticIndividual(p2.ah - ahDiff, p2.lines, p2.holes, p2.bumpiness),
                
                new GeneticIndividual(p1.ah, p1.lines + linesDiff, p1.holes, p1.bumpiness),
                new GeneticIndividual(p2.ah, p2.lines + linesDiff, p2.holes, p2.bumpiness),
                new GeneticIndividual(p1.ah, p1.lines - linesDiff, p1.holes, p1.bumpiness),
                new GeneticIndividual(p2.ah, p2.lines - linesDiff, p2.holes, p2.bumpiness),
                
                new GeneticIndividual(p1.ah, p1.lines, p1.holes + holesDiff, p1.bumpiness),
                new GeneticIndividual(p2.ah, p2.lines, p2.holes + holesDiff, p2.bumpiness),
                new GeneticIndividual(p1.ah, p1.lines, p1.holes - holesDiff, p1.bumpiness),
                new GeneticIndividual(p2.ah, p2.lines, p2.holes - holesDiff, p2.bumpiness),
                
                new GeneticIndividual(p1.ah, p1.lines, p1.holes, p1.bumpiness + bumpinessDiff),
                new GeneticIndividual(p2.ah, p2.lines, p2.holes, p2.bumpiness + bumpinessDiff),
                new GeneticIndividual(p1.ah, p1.lines, p1.holes, p1.bumpiness - bumpinessDiff),
                new GeneticIndividual(p2.ah, p2.lines, p2.holes, p2.bumpiness - bumpinessDiff)
            };
        }

        public GeneticIndividual GetBest()
        {
            _population.Sort(new GeneticIndividualComparer());
            return _population.First();
        }

        public void MakeSelection(int count)
        {
            _population.Sort(new GeneticIndividualComparer());
            var temp = _population.Take(count);
            _population = temp.ToList();
        }

        private bool IsEveryOneTheSame()
        {
            var first = _population[0];
            for (var i = 1; i < _population.Count; ++i)
            {
                if (Math.Abs(first.avgHeight - _population[i].avgHeight) > 0.0001f)
                    return false;
            }

            return true;
        }

        public void Next()
        {
            _currentIndex++;
            currentIndividual = _population[_currentIndex];
        }
        public void Reset()
        {
            _currentIndex = 0;
        }

        public int GetRemain()
        {
            return _population.Count - _currentIndex;
        }
        public void LoadFromJson()
        {
            var lastGenerationJson = SaveUtility.LoadString(SAVE_POPULATION_KEY);
            _population = JsonHelper.FromJson<GeneticIndividual>(lastGenerationJson).ToList();
            _currentIndex = _population.FindIndex(0, g => g.turns == -1);
            currentIndividual = _population[_currentIndex];
        }
        public void Save()
        {
            var value = JsonHelper.ToJson(_population.ToArray(), false);
            SaveUtility.SaveString(SAVE_POPULATION_KEY, value);
        }
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}