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
        private const int iterations = 10000;
        private readonly Graph distance;
        
        private readonly CrossingType crossingType = CrossingType.PartiallyMapped;
        private readonly MutationType mutationType = MutationType.Exchange;
        private readonly LocalImprovements localImprovement = LocalImprovements.LocalImprovement1;
        public GeneticEngine(Graph distance)
        {
            this.distance = distance;
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
            generation = CreateGeneration();
            Array.Sort(generation);

            for (int i = 0; i < iterations; i++)
            {
                Path father = generation[0];
                Path mother = RouletteWheelSelection();

                Path child = Crossover(father, mother);
                Mutation(child);
                LocalImprovement(child);

                Path worst = generation[generationSize - 1];
                if (child.Fitness < worst.Fitness)
                {
                    generation[generationSize - 1] = child;
                }
                Array.Sort(generation);

                Console.WriteLine($"{i}. {generation[0].Fitness}");
            }
        }
        private Path Crossover(Path father, Path mother)
        {
            switch(crossingType)
            {
                case CrossingType.PartiallyMapped:
                    return PartiallyMappedCrossover(father, mother);
                case CrossingType.Ordered:
                    return OrderedCrossover(father, mother);
                case CrossingType.Cycle:
                    return CycleCrossover(father, mother);
                default: throw new ArgumentException("Crossing type was not chosen.");
            }
        }
        private void Mutation(Path path)
        {
            switch (mutationType)
            {
                case MutationType.Exchange:
                    ExchangeMutation(path);
                    break;
                case MutationType.CentreInverse:
                    CentreInverseMutation(path);
                    break;
                case MutationType.Reverse:
                    ReverseMutation(path);
                    break;
                default:
                    throw new ArgumentException("Mutation type was not chosen.");
            }
        }
        private void LocalImprovement(Path path)
        {
            switch (localImprovement)
            {
                case LocalImprovements.LocalImprovement1:
                    LocalImprovement1(path);
                    break;
                case LocalImprovements.LocalImprovement2:
                    LocalImprovement2(path);
                    break;
                default:
                    throw new ArgumentException("Local improvement was not chosen.");
            }
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

            int first, second;
            do
            {
                first = random.Next(chromosomeLenght);
                second = random.Next(chromosomeLenght);
            } while (first == second);

            int temp = path[first];
            path[first] = path[second];
            path[second] = temp;

            path.UpdateFitness(distance);
        }
        private void CentreInverseMutation(Path path)
        {
            int lenght = path.Chromosome.Length;
            Random random = new();
            int crossPoint = random.Next(1, lenght - 1); // do not use first and last

            crossPoint = 2;
            int[] temp = new int[lenght];
            // [0; crossPoint)
            for (int i = 0; i < crossPoint; i++)
            {
                temp[i] = path[crossPoint - 1 - i];
            }
            // [crossPoint; lenght - 1]
            for (int i = crossPoint; i < lenght; i++)
            {
                temp[i] = path[crossPoint + lenght - 1 - i];
            }
            temp.CopyTo(path.Chromosome, 0);
            path.UpdateFitness(distance);
        }
        private void ReverseMutation(Path path)
        {
            int lenght = path.Chromosome.Length;

            Random random = new();
            int point1 = random.Next(0, lenght);
            int point2 = random.Next(0, lenght);

            if (point1 > point2)
            {
                int temp = point1;
                point1 = point2;
                point2 = temp;
            }

            int[] tmp = new int[lenght];
            path.Chromosome.CopyTo(tmp, 0);

            int centre = point1 + (point2 - point1) / 2;
            for (int i = point1; i < centre; i++)
            {
                int temp = path[i];
                path[i] = path[point1 + point2 - i];
                path[point1 + point2 - i] = temp;
            }

            path.UpdateFitness(distance);
        }
        private void LocalImprovement1(Path path)
        {
            // find longest distance between vertexes
            int index = 0;
            int max = int.MinValue;
            for (int i = 0; i < path.Chromosome.Length - 1; i++)
            {
                int curr = distance[path[i], path[i + 1]];
                if (curr > max)
                {
                    max = curr;
                    index = i;
                }
            }
            // find nearest vertex
            int min = int.MaxValue;
            int vertex = -1;
            for (int i = 0; i < path.Chromosome.Length; i++)
            {
                if (i != index && distance[path[index], path[i]] < min)
                {
                    min = distance[path[index], path[i]];
                    vertex = i;
                }
            }
            // swap
            int indexOfVertex = Array.IndexOf(path.Chromosome, vertex);
            int temp = path[index + 1];
            path[index + 1] = path[indexOfVertex];
            path[indexOfVertex] = temp;
            // check
            int previousFitness = path.Fitness;
            path.UpdateFitness(distance);
            if (path.Fitness > previousFitness)
            {
                // swap back
                temp = path[index + 1];
                path[index + 1] = path[indexOfVertex];
                path[indexOfVertex] = temp;
                path.Fitness = previousFitness;
            }
        }
        private void LocalImprovement2(Path path)
        {
            for (int i = 2; i < path.Chromosome.Length - 2; i++) // do not swap first and last
            {
                // 1 2 3 4
                int primary = distance[path[i - 1], path[i]] + distance[path[i], path[i + 1]] + distance[path[i + 1], path[i + 2]];
                // 1 3 2 4
                int swapped = distance[path[i - 1], path[i + 1]] + distance[path[i + 1], path[i]] + distance[path[i], path[i + 2]];
                if (swapped < primary)
                {
                    path.Fitness = path.Fitness - primary + swapped;
                    int temp = path[i];
                    path[i] = path[i + 1];
                    path[i + 1] = temp;
                    break;
                }
            }
        }
        /// <summary>
        /// Map stores indexes of cities in chromosome. 
        /// map[city] = index.
        /// </summary>
        /// <param name="chromosome"></param>
        /// <returns></returns>
        private static int[] Map(int[] chromosome)
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
