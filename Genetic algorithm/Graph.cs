using System;
using System.IO;

namespace traveling_salesman_problem
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
        public void Fill(int min, int max)
        {
            Random random = new Random();
            for (int i = 0; i < Size; i++)
            {
                for (int j = i + 1; j < Size; j++)
                {
                    Matrix[i, j] = Matrix[j, i] = random.Next(min, max + 1);
                }
            }
        }
    }
}
