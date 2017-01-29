using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edge_betweenness
{
    class Program
    {
        static int MAX = 9999999;
        static void Main(string[] args)
        {
            Graph g = new Graph();
            g.Read("D:/solution/edge_betweenness/edge_betweenness/graph.txt");
            g.Print();

            Edge_Betweenness.EdgeBetweenness(g);
            
            Console.ReadLine();
        }
    }

    public class Edge_Betweenness
    {
        public static double[,] EdgeBetweenness(Graph g)
        {
            int n = g.NumVertex();
            int MAX = 99999999;
            double[] Cb = new double[n];
            double[,] Cbe = new double[n, n];

            for (int c = 0; c < n; c++)
            {
                Cb[c] = 0;
                for (int z = 0; z < n; z++)
                {
                    Cbe[c, z] = 0;
                }
            }

            Queue<int> Q = new Queue<int>();
            Stack<int> S = new Stack<int>();
            double[] dist = new double[n];
            double[] sigma = new double[n];
            double[] delta = new double[n];
            List<int>[] pred = new List<int>[n];

            for (int s = 0; s < n; s++)
            {
                // initialization
                for (int w = 0; w < n; w++)
                {
                    pred[w] = new List<int>();
                    dist[w] = MAX;
                    sigma[w] = 0;
                }

                dist[s] = 0;
                sigma[s] = 1;
                Q.Enqueue(s);

                // while
                while (Q.Count != 0)
                {
                    int v = Q.Dequeue();
                    S.Push(v);
                    for (int w = 0; w < n; w++)
                    {
                        if (g.IsConnect(v, w) == true)
                        {
                            if (dist[w] == MAX)
                            {
                                dist[w] = dist[v] + 1;
                                Q.Enqueue(w);
                            }
                            if (dist[w] == dist[v] + 1)
                            {
                                sigma[w] = sigma[w] + sigma[v];
                                pred[w].Add(v);
                            }
                        }
                    }
                }

                // accumuation
                for (int v = 0; v < n; v++)
                {
                    delta[v] = 0;
                }
                while (S.Count != 0)
                {
                    int w = S.Pop();
                    foreach (int v in pred[w])
                    {
                        double c = (sigma[v] / sigma[w]) * (1.0 + delta[w]);
                        Cbe[v, w] = Cbe[v, w] + c;
                        delta[v] = delta[v] + c;
                    }
                    if (w != s)
                    {
                        Cb[w] += delta[w];
                    }
                }

                // end
            }
            return Cbe;
        }
    }

    class Graph
    {
        public int[,] matrix;
        public bool IsConnect(int i, int j)
        {
            return matrix[i, j] != 0;
        }

        public int NumVertex() { return this.matrix.GetLength(0); }

        public void Read(string path)
        {
            StreamReader reader = new StreamReader(path);
            int n = int.Parse(reader.ReadLine());

            this.matrix = new int[n, n];
            int i = 0;
            while (reader.EndOfStream == false)
            {
                string[] _str = reader.ReadLine().Split(' ');
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = int.Parse(_str[j]);
                }
                i++;
            }
        }

        public void Print()
        {
            for (int i = 0; i < this.matrix.GetLength(0); i++)
            {
                for (int j = 0; j < this.matrix.GetLength(1); j++)
                {
                    Console.Write("{0} ", this.matrix[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
