using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_algorithm
{
    internal class GeneticEngine
    {
        private Path[] generation;
        private readonly int generationSize;
        public GeneticEngine(Graph distance)
        {
            generationSize = 1000;
            generation = CreateGeneration(distance);
        }
        private Path[] CreateGeneration(Graph distance)
        {
            Path[] generation = new Path[generationSize];
            int chromosomeLenght = distance.Size; // chromosome lenght is equal to num of cities

            for (int i = 0; i < generationSize; i++)
            {
                int[] chromosome = RandomChromosome(chromosomeLenght);
                generation[i] = new Path(chromosome, distance);
            }
            return generation;
        }
        private int[] RandomChromosome(int lenght)
        {
            int[] chromosome = new int[lenght];
            
            List<int> possibleWays = new();
            for (int i = 0; i < lenght; i++)
            {
                possibleWays.Add(i);
            }

            Random random = new();
            for (int i = 0; i < lenght; i++)
            {
                int index = random.Next(possibleWays.Count);
                int way = possibleWays[index];
                chromosome[i] = way;
                possibleWays.Remove(way);
            }

            return chromosome;
        }

    }
}
