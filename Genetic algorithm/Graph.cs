using System;
using System.IO;

namespace Genetic_algorithm
{
    internal class Graph
    {
        private int[,] Matrix { get; set; }
        public int this[int i, int j]
        {
            get
            {
                return Matrix[i, j];
            }
            set
            {
                Matrix[i, j] = value;
            }
        }
        public int Size { get; set; }
        public Graph(int size)
        {
            Size = size;
            Matrix = new int[Size, Size];
        }
        public Graph(string path)
        {
            Read(path);
        }
        public void Read(string path)
        {
            using StreamReader sr = new StreamReader(path);
            Size = Convert.ToInt32(sr.ReadLine());
            Matrix = new int[Size, Size];

            for (int i = 0; i < Size; i++)
            {
                string line = sr.ReadLine().Trim();
                string[] arr = line.Split(' ');
                for (int j = 0; j < Size; j++)
                    Matrix[i, j] = Convert.ToInt32(arr[j]);
            }
        }
        public void Write(string path)
        {
            using StreamWriter sw = new StreamWriter(path);
            sw.WriteLine(Size);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                    sw.Write(Matrix[i, j] + " ");
                sw.WriteLine();
            }
        }
        /// <summary>
        /// Mixed filling. This means directed graph with some undirected lines.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void Fill(int min, int max)
        {
            const int PROBABILITY_OF_UNDIRECTED_LINE = 35;
            Random random = new();
            for (int i = 0; i < Size; i++)
            {
                for (int j = i + 1; j < Size; j++)
                {
                    if (random.Next(100) < PROBABILITY_OF_UNDIRECTED_LINE)
                    {
                        Matrix[i, j] = Matrix[j, i] = random.Next(min, max);
                    }
                    else
                    {
                        do
                        {
                            Matrix[i, j] = random.Next(min, max);
                            Matrix[j, i] = random.Next(min, max);
                        } while (Matrix[i, j] == Matrix[j, i]);
                    }
                }
            }

        }
        /// <summary>
        /// Count of undirected lines in this graph.
        /// </summary>
        /// <returns>Undirected lines of total lines count.</returns>
        public string UndirectedLinesPercentage()
        {
            int cnt = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = i + 1; j < Size; j++)
                {
                    if (Matrix[i, j] == Matrix[j, i]) cnt += 2;
                }
            }
            int totalLines = Size * (Size - 1);
            double undirectedOfTotal = (double)cnt / totalLines;
            return string.Format($"Percent of undirected lines: {undirectedOfTotal:p}");
        }
    }
}
