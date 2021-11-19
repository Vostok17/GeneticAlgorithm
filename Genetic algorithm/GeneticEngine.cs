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
        private const int generationSize = 100;
        private readonly Graph distance;
        public GeneticEngine(Graph distance)
        {
            this.distance = distance;
            generation = CreateGeneration();

            Selection();
        }
        private Path[] CreateGeneration()
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
        private void Selection()
        {
            
        }
        private Path PartiallyMappedCrossover(Path father, Path mother)
        {
            int lenght = father.Chromosome.Length;

            Random random = new();
            int crossPoint1 = random.Next(0, lenght);
            int crossPoint2 = random.Next(0, lenght);

            if (crossPoint1 > crossPoint2)
            {
                int temp = crossPoint1;
                crossPoint1 = crossPoint2;
                crossPoint2 = temp;
            }

            int[] child = new int[lenght];
            father.Chromosome.CopyTo(child, 0);

            // map stores indexes of cities in child
            // child[index] = city
            // map[city] = index
            int[] map = new int[lenght];
            for (int i = 0; i < lenght; i++)
            {
                map[child[i]] = i;
            }

            for (int i = crossPoint1; i <= crossPoint2; i++)
            {
                int city = mother[i];
                int indexOfCity = map[city];
                // swap genes in the child
                int temp = child[indexOfCity];
                child[indexOfCity] = child[i];
                child[i] = temp;
                // swap indexes in the map 
                temp = map[child[indexOfCity]]; 
                map[child[indexOfCity]] = map[child[i]];
                map[child[i]] = temp;
            }

            return new Path(child, distance);
        }
        private Path RouletteWheelSelection()
        {
            int summary = 0;
            foreach (Path path in generation)
            {
                summary += path.Fitness;
            }
            Random random = new();
            
            int randomNumber = random.Next(summary + 1);
            int currFitness = 0;
            foreach (Path path in generation)
            {
                currFitness += path.Fitness;
                if (currFitness >= randomNumber)
                {
                    return path;
                }
            }
            throw new Exception("No path was chosen in roulette wheel selection.");
        }
    }
}
