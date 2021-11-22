using System;

namespace Genetic_algorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int SIZE = 6;
            const int MIN = 5, MAX = 50;

            Graph graph = new Graph(SIZE);
            graph.Fill(MIN, MAX);
            //Console.WriteLine(graph.UndirectedLinesPercentage());

            GeneticEngine geneticEngine = new GeneticEngine(graph); 
            geneticEngine.Start();

            Console.ReadKey();
        }
    }
}
