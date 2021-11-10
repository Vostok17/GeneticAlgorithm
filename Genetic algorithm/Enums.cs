using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_algorithm
{
    public enum SelectionType
    {
        Tourney,
        RouletteWheel
    }
    public enum CrossingType
    {
        OnePointRecombination,
        TwoPointRecombination,
        ElementwiseRecombination,
        OneElementExchange
    }
}
