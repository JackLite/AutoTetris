using System;

namespace Core.AI.Genetic
{
    [Serializable]
    public class GeneticIndividual
    {
        public int scores;
        public int height;
        public float ah;
        public float lines;
        public float holes;
        public float bumpiness;
        public GeneticIndividual(float ah, float lines, float holes, float bumpiness)
        {
            this.ah = ah;
            this.lines = lines;
            this.holes = holes;
            this.bumpiness = bumpiness;
        }

        public override string ToString()
        {
            return
                $"AH: {ah}; Lines: {lines}; Holes: {holes}; Bumpiness: {bumpiness}; Scores: {scores}; Height: {height}";
        }
    }
}