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
            Start();
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
        public void Start()
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

            int[] map = Map(child);

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
        private Path CycleCrossover(Path father, Path mother)
        {
            int lenght = father.Chromosome.Length;
            int[] child = new int[lenght];
            for (int k = 0; k < lenght; k++)
            {
                child[k] = -1; // unused vertex
            }

            int[] map = Map(father.Chromosome);

            int i = 0;
            child[i] = father[i];
            do
            {
                int j = map[mother[i]];
                child[j] = father[j];
                i = j;
            } while (mother[i] != child[0]);

            // for all unused vertexes
            for (int k = 0; k < lenght; k++)
            {
                if (child[k] == -1) child[k] = mother[k];
            }

            return new Path(child, distance);
        }
        private Path OrderedCrossover(Path father, Path mother)
        {
            int lenght = father.Chromosome.Length;
            int[] child = new int[lenght];

            Random random = new();
            int crossPoint1 = random.Next(0, lenght);
            int crossPoint2 = random.Next(0, lenght);

            if (crossPoint1 > crossPoint2)
            {
                int temp = crossPoint1;
                crossPoint1 = crossPoint2;
                crossPoint2 = temp;
            }

            int[] map = Map(father.Chromosome);
            for (int i = crossPoint1; i <= crossPoint2; i++)
            {
                child[i] = father[i];
            }

            int pos = crossPoint2 + 1;
            foreach (int item in mother.Chromosome)
            {
                int indexInFather = map[item];
                if (indexInFather < crossPoint1 || indexInFather > crossPoint2)
                {
                    child[pos] = item;
                    pos = ++pos % lenght;
                }
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
        private void ExchangeMutation(Path path)
        {
            Random random = new();
            int chromosomeLenght = path.Chromosome.Length;
            int first = random.Next(chromosomeLenght);
            int second = random.Next(chromosomeLenght);

            int temp = path[first];
            path[first] = path[second];
            path[second] = temp;
        }
        /// <summary>
        /// Map stores indexes of cities in chromosome. 
        /// map[city] = index.
        /// </summary>
        /// <param name="chromosome"></param>
        /// <returns></returns>
        private int[] Map(int[] chromosome)
        {
            int lenght = chromosome.Length;
            int[] map = new int[lenght];
            for (int i = 0; i < lenght; i++)
            {
                map[chromosome[i]] = i;
            }
            return map;
        }
    }
}
