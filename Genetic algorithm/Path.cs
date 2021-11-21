using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_algorithm
{
    internal class Path: IComparable<Path>
    {
        public int[] Chromosome;
        public int Fitness;

        public int this[int index]
        {
            get { return Chromosome[index]; }
            set { Chromosome[index] = value; }
        }
        public Path(int[] chromosome, Graph distance)
        {
            Chromosome = chromosome;
            Fitness = FitnessFunction(distance);
        }
        public int FitnessFunction(Graph distance)
        {
            int valueOfPath = 0;
            for (int i = 1; i < distance.Size; i++)
            {
                int curr = Chromosome[i - 1];
                int next = Chromosome[i];
                valueOfPath += distance[curr, next];
            }
            // Return to start
            int last = Chromosome[distance.Size - 1];
            int start = Chromosome[0];
            valueOfPath += distance[last, start];

            return valueOfPath;
        }

        public int CompareTo(Path other)
        {
            return Fitness.CompareTo(other.Fitness);
        }
    }
}
