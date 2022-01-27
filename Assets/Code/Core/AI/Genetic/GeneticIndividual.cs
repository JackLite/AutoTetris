using UnityEngine;

namespace Core.AI.Genetic
{
    public class GeneticIndividual
    {
        public int scores;
        public float Ah { get; set; }
        public float Lines { get; set; }
        public float Holes { get; set; }
        public float Bumpiness { get; set; }
        public GeneticIndividual(float ah, float lines, float holes, float bumpiness)
        {
            Ah = ah;
            Lines = lines;
            Holes = holes;
            Bumpiness = bumpiness;
        }

        public override string ToString()
        {
            return $"AH: {Ah}; Lines: {Lines}; Holes: {Holes}; Bumpiness: {Bumpiness}; Scores: {scores}";
        }
    }
}