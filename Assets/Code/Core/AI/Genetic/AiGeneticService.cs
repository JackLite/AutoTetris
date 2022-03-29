using System;
using System.Collections.Generic;
using System.Linq;
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
                var ah = Random.Range(-5f, 5f);
                var lines = Random.Range(-5f, 5f);
                var height = Random.Range(-5f, 5f);
                var bumpiness = Random.Range(-5f, 5f);
                var individual = new GeneticIndividual(ah, lines, height, bumpiness);
                _population.Add(individual);
                i++;
            }
            currentIndividual = _population.First();
        }

        public bool IsHasMore()
        {
            return _currentIndex < _population.Count;
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
                    {
                        var r = Random.Range(0f, 1);
                        if (r < mutationFactor)
                            Mutate(child);
                        _population.Add(child);
                    }
                }
            }
        }
        private void Mutate(GeneticIndividual child)
        {
            var r = Random.Range(-5f, 5);
            if (r < .2f)
                child.ah = Random.Range(-5f, 5);
            else if (r < .4f)
                child.lines = Random.Range(-5f, 5);
            else if (r < .6f)
                child.holes = Random.Range(-5f, 5);
            else if (r < .8f)
                child.bumpiness = Random.Range(-5f, 5);
        }

        private GeneticIndividual[] Cross(GeneticIndividual p1, GeneticIndividual p2)
        {
            var r = Random.Range(0f, 1);
            if (r < .2f)
                return new[]
                {
                    new GeneticIndividual(p1.ah, p2.lines, p2.holes, p2.bumpiness),
                    new GeneticIndividual(p2.ah, p1.lines, p1.holes, p1.bumpiness)
                };
            if (r < .4f)
                return new[]
                {
                    new GeneticIndividual(p2.ah, p1.lines, p2.holes, p2.bumpiness),
                    new GeneticIndividual(p1.ah, p2.lines, p1.holes, p1.bumpiness)
                };
            if (r < .6f)
                return new[]
                {
                    new GeneticIndividual(p2.ah, p2.lines, p1.holes, p2.bumpiness),
                    new GeneticIndividual(p1.ah, p1.lines, p2.holes, p1.bumpiness)
                };
            return new[]
            {
                new GeneticIndividual(p2.ah, p2.lines, p2.holes, p1.bumpiness),
                new GeneticIndividual(p1.ah, p1.lines, p1.holes, p2.bumpiness)
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