using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;
using Random = System.Random;

namespace Global
{
    [EcsSystem(typeof(MainModule))]
    public class GeneticTestSystem : IEcsInitSystem
    {
        private bool _isFinish;
        private int _startPopulationCount = 25;
        private Random _random = new Random();
        private int _iterationsCount = 50;
        private float _mutation = .1f;
        /// <summary>
        /// 2x + 4y = 96
        /// x + 8y = 150
        /// </summary>
        public void Init()
        {
            // генерируем начальную популяцию
            var population = GenerateStartPopulation();
            var iterations = 0;
            var isFoundAnswer = false;
            while (iterations < _iterationsCount)
            {
                // производим скрещивание каждого с каждым, увеличивая популяцию
                CrossPopulation(population);
                // для каждого члена популяции проверяем пригодность
                foreach (var individual in population)
                {
                    CalculateFitness(individual);
                    if (individual.fitness == 0)
                    {
                        Debug.Log("Answer is " + individual);
                        isFoundAnswer = true;
                        break;
                    }
                }
                if (isFoundAnswer)
                    break;
                // оставляем 100 индивидуумов
                population.Sort(new IndividualComparer());
                var temp = population.Take(100).ToList();
                population = temp;
                Debug.Log("Best Individual " + temp[0].ToString());
                iterations++;
            }
             
        }

        private void CalculateFitness(Individual individual)
        {
            var a1 = individual.x * 2 + individual.y * 4;
            var a2 = individual.x + individual.y * 8;
            var fitness1 = Math.Abs(96 - a1);
            var fitness2 = Math.Abs(150 - a2);
            individual.fitness = fitness1 + fitness2;
        }
        
        private void CrossPopulation(List<Individual> population)
        {
            var startIndex = 0;
            var count = population.Count;
            for (var i = 0; i < count; ++i)
            {
                var parent1 = population[i];
                for (var j = i + 1; j < count; ++j)
                {
                    var parent2 = population[j];
                    var child = Cross(parent1, parent2);
                    var r = _random.Next(0, 100) / 100f;
                    if (r < _mutation)
                        Mutate(child);
                    population.Add(child);
                }
            }
        }
        private void Mutate(Individual child)
        {
            child.x = (child.x * child.y + _random.Next(0, 100)) % 101;
            child.y = (child.x * child.y + _random.Next(0, 100)) % 101;
        }

        private Individual Cross(Individual p1, Individual p2)
        {
            var r = _random.Next();
            if(r % 2 == 0)
                return new Individual{ x = p1.x, y = p2.y};
            return new Individual{ x = p2.x, y = p1.y};
        }
        
        private List<Individual> GenerateStartPopulation()
        {
            var population = new List<Individual>();
            while (population.Count < _startPopulationCount)
            {
                var individual = new Individual();
                individual.x = _random.Next(0, 101);
                individual.y = _random.Next(0, 101);
                population.Add(individual);
            }
            return population;
        }
    }

    public class Individual
    {
        public int x;
        public int y;
        public int fitness;
        public float likelihood;

        public override string ToString()
        {
            return $"x = {x}; y = {y}; fitness = {fitness}";
        }
    }
    
    public class IndividualComparer : IComparer<Individual>
    {
        public int Compare(Individual x, Individual y)
        {
            if (ReferenceEquals(x, y))
                return 0;
            if (ReferenceEquals(null, y))
                return 1;
            if (ReferenceEquals(null, x))
                return -1;
            return x.fitness.CompareTo(y.fitness);
        }
    }
}