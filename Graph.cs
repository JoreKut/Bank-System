using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Graph
    {
        private readonly static int Y_Start = 0, X_Start = 0;
        private readonly static int graph_height = 20 + 1;
        private readonly static int graph_width = 80 + 2;
        private readonly static double  Y_Step = 0.25;
        private readonly static double dY = 2.5;
        private readonly static int min_Y_Next = 5;
        private readonly static int max_Y_Next = graph_height - 5;
        private string point_sign = "*";
        private string empty_sign = " ";
        private string[,] graph = new string[graph_width, graph_height];

        private int Y_Next = 10, X_Next = 2;


        public Graph(double value)
        {
            Base_Fill(value);
        }
        public void Show_Graph()
        {
            Console.Clear();

            if (X_Next > graph_width) XShift_Graph();
            if (Y_Next < min_Y_Next) YShift_Graph_Up();
            else if (Y_Next > max_Y_Next) YShift_Graph_Down();

            graph[X_Next, Y_Next] = point_sign;

            for (int j = 0; j < graph_height; j++)
            {
                for (int i = 0; i < graph_width; i++)
                    Console.Write(graph[i,j]);
                Console.WriteLine();
            }
            Console.WriteLine($"{X_Next} {Y_Next}");

            X_Next += 1;
            Y_Next = Get_Y_Next(Y_Next);

        }
        private void Base_Fill(double value)
        {

            // Ox и Oy
            for (int y = 0; y < graph_height; y++)
            {
                graph[0, y] = $"{Convert.ToString(value + dY - y * Y_Step),5}";
                graph[1, y] = "|";
            }
            for (int x = 2; x < graph_width; x++)
            {
                graph[x, graph_height - 1] = "_";
            }

            for (int x = 2; x < graph_width; x++)
            {
                for (int y = 0; y < graph_height-1; y++)
                {
                    graph[x, y] = empty_sign;
                }
            }
            
        }

        private void XShift_Graph() 
        {
            for (int x = X_Start; x < graph_width; x++)
            {
                for (int y = min_Y_Next; y < max_Y_Next; y++)
                {
                    if(graph[x,y] == point_sign)
                    {
                        if (x - 1 >= 2) graph[x - 1, y] = point_sign;
                        graph[x, y] = empty_sign;
                    }
                }
            }

            X_Next = graph_width;
        }
        private void YShift_Graph_Down()
        {
            Next_Value(Convert.ToDouble(graph[0, 0]));
            int shift = min_Y_Next - Y_Next;

            for (int x = X_Start; x < X_Next; x++)
                for (int y = min_Y_Next; y < max_Y_Next + 1; y++)
                {
                    if (graph[x, y] == point_sign)
                    {
                        if (y + shift <= max_Y_Next) graph[x, y + shift] = point_sign;
                        graph[x, y] = empty_sign;

                    }
                }

            Y_Next = min_Y_Next;
        }
        private void YShift_Graph_Up()
        {
            Next_Value(Convert.ToDouble(graph[0, 0]));
            int shift = Y_Next - max_Y_Next;

            for (int x = X_Start; x < X_Next; x++)
            {
                for (int y = min_Y_Next; y < max_Y_Next + 1; y++)
                {
                    if(graph[x,y] == point_sign)
                    {
                        if (y - shift >= min_Y_Next) graph[x, y - shift] = point_sign;
                        graph[x, y] = empty_sign;
                        
                    }
                }
            }

            Y_Next = max_Y_Next;
        }
        private int Get_Y_Next(int y)
        {
            int[] m = new int[5] { -2, -1, 0, 1, 2 };
            return y + m[new Random().Next() % 5];
        }
        private void Next_Value(double max_value)
        {
            for (int i = 0; i < graph_height; i++)
            {
                graph[0, i] = $"{Convert.ToString(max_value - i * Y_Step),5}";
            }
        }
    }
}
