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
        private void Read(string path)
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
            // generate directed graph
            Random random = new Random();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Matrix[i, j] = random.Next(min, max + 1);
                }
            }

            const double PERCENT_OF_UNDIRECTED_LINES = 0.3;
            int totalLines = Size * Size;
            int undirectedLines = (int)(PERCENT_OF_UNDIRECTED_LINES * totalLines);

            // change some directed lines to undirected
            for (int k = 0; k < undirectedLines; k++)
            {
                int i = random.Next(Size);
                int j = random.Next(Size);
                Matrix[i, j] = Matrix[j, i] = random.Next(min, max + 1);
            }
        }
        /// <summary>
        /// Count of undirected lines in this graph.
        /// </summary>
        /// <returns>Undirected lines of total lines count.</returns>
        public string UndirectedLinesCount()
        {
            int cnt = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = i + 1; j < Size; j++)
                {
                    if (Matrix[i, j] == Matrix[j, i]) cnt++;
                }
            }
            int totalLines = Size * Size;
            return string.Format($"{cnt} undirected lines of {totalLines}");
        }
    }
}
