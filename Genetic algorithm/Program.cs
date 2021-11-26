using System;

namespace Genetic_algorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int SIZE = 300;

            Graph graph = new Graph(SIZE);
            graph.Read(@"C:\Users\Artem\Desktop\ПА\Lab4\Genetic algorithm\graph.dat");

            Console.WriteLine(graph.UndirectedLinesPercentage());

            GeneticEngine geneticEngine = new GeneticEngine(graph); 
            geneticEngine.Start();
            geneticEngine.ShowResult();

            Console.ReadKey();
        }
    }
}
