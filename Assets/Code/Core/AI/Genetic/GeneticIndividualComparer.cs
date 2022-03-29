﻿using System.Collections.Generic;

namespace Core.AI.Genetic
{
    public class GeneticIndividualComparer : IComparer<GeneticIndividual>
    {

        public int Compare(GeneticIndividual x, GeneticIndividual y)
        {
            if (ReferenceEquals(x, y))
                return 0;
            if (ReferenceEquals(null, y))
                return 1;
            if (ReferenceEquals(null, x))
                return -1;
            return y.turns.CompareTo(x.turns);
        }
    }
}