using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.AI.Genetic
{
    public class AiGeneticService
    {
        public GeneticIndividual currentIndividual;
        public float mutationFactor;
        private List<GeneticIndividual> _population;
        private int _currentIndex;
        
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
                var ah = Random.Range(-1f, 0);
                var lines = Random.Range(0f, 1);
                var height = Random.Range(-1f, 0);
                var bumpiness = Random.Range(-1f, 0);
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
            child.Ah = Random.Range(-1f, 0);
            child.Lines = Random.Range(0f, 1);
            child.Holes = Random.Range(-1f, 0);
            child.Bumpiness = Random.Range(-1f, 0);
        }

        private GeneticIndividual[] Cross(GeneticIndividual p1, GeneticIndividual p2)
        {
            return new[]
            {
                new GeneticIndividual(p1.Ah, p2.Lines, p2.Holes, p2.Bumpiness),
                new GeneticIndividual(p1.Ah, p1.Lines, p2.Holes, p2.Bumpiness),
                new GeneticIndividual(p1.Ah, p1.Lines, p1.Holes, p2.Bumpiness),
                new GeneticIndividual(p2.Ah, p1.Lines, p1.Holes, p1.Bumpiness),
                new GeneticIndividual(p2.Ah, p2.Lines, p1.Holes, p1.Bumpiness),
                new GeneticIndividual(p2.Ah, p2.Lines, p2.Holes, p1.Bumpiness),
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
    }
}