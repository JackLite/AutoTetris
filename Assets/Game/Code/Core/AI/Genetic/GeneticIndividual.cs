using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.AI.Genetic
{
    [Serializable]
    public class GeneticIndividual : IEquatable<GeneticIndividual>
    {
        public int turns = -1;
        public float ah;
        public float lines;
        public float holes;
        public float bumpiness;
        public int scores;
        public float avgHeight;

        public List<int> heights = new List<int>(64 * 64);

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
                $"AH: {ah}; Lines: {lines}; Holes: {holes}; Bumpiness: {bumpiness}; Turns: {turns}; AvgHeight: {avgHeight}";
        }
        public void CalculateAvgHeight()
        {
            avgHeight = heights.Sum() / (float) turns;
        }
        public bool Equals(GeneticIndividual other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return ah.Equals(other.ah)
                   && lines.Equals(other.lines)
                   && holes.Equals(other.holes)
                   && bumpiness.Equals(other.bumpiness);
        }
    }
}