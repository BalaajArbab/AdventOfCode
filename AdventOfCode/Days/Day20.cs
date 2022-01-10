using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode_2021.Days
{
    class Day20
    {
        
        public static void Run()
        {
            string enhancementAlgorithm;

            Graph graph;

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day20_InputImage.txt"))
            {
                enhancementAlgorithm = sr.ReadLine();

                sr.ReadLine();

                List<List<char>> grid = new List<List<char>>();

                while (sr.Peek() > -1)
                {
                    string line = sr.ReadLine();

                    List<char> row = line.ToCharArray().ToList();

                    grid.Add(row);
                }

                Node2[,] arr = new Node2[grid.Count, grid[0].Count];

                

                for (int i = 0; i < grid.Count; i++)
                {
                    for (int j = 0; j < grid[i].Count; j++)
                    {
                        Node2 node = new Node2(grid[i][j]);

                        arr[i, j] = node;
                    }
                }

                graph = Graph.ConstructGraph(arr);

            }



            graph.extendSides();
            graph.extendSides();

            for (int i = 0; i < 50; i++)
            {
                graph.Expand(enhancementAlgorithm);
            }          

            Console.WriteLine($"Part 2 Count of Lit Pixels: {graph.CountLitPixels()}");           

            graph.Print();

        }

    }

    public class Graph
    {
        public Node2 TopLeft;

        public int n = 0;

        private bool extendedSides = false;

        public Graph(Node2 topLeft)
        {
            this.TopLeft = topLeft;
        }

        public void Expand(string enhancementAlgorithm)
        {
            this.extendSides();

            Node2 currY = this.TopLeft;

            while (currY != null)
            {
                Node2 currX = currY;

                while (currX != null)
                {
                    int index;
                    
                    if (!currX.isCorner) index = currX.binaryToIndex();

                    else
                    {
                        if (currX.CalcPixel == '#') index = 511;

                        else index = 0;
                    }

                    currX.Pixel = enhancementAlgorithm[index];

                    currX = currX.EastAdjacent;
                }

                currY = currY.SouthAdjacent;
            }

            this.replaceCalc();
        }

        private void replaceCalc()
        {
            Node2 currY = this.TopLeft;

            while (currY != null)
            {
                Node2 currX = currY;

                while (currX != null)
                {
                    currX.CalcPixel = currX.Pixel;

                    currX = currX.EastAdjacent;
                }

                currY = currY.SouthAdjacent;
            }
        }

        public void extendSides()
        {
            Node2 currX = this.TopLeft;

            Node2 last = null;

            resetIsCorner();

            char replaceChar;

            if (!extendedSides) replaceChar = '.';

            else replaceChar = TopLeft.Pixel;

            extendedSides = true;

            while (currX != null)
            {
                Node2 node = new Node2(replaceChar);
                node.isCorner = true;

                node.WestAdjacent = last;
                currX.NorthAdjacent = node;
                node.SouthAdjacent = currX;
                n++;

                if (last != null) last.EastAdjacent = node;

                last = node;

                if (currX.EastAdjacent == null) break;
                currX = currX.EastAdjacent;
            }

            
            Node2 currY = currX.NorthAdjacent;

            last = null;

            while (currY != null)
            {
                Node2 node = new Node2(replaceChar);
                node.isCorner = true;

                node.NorthAdjacent = last;
                currY.EastAdjacent = node;
                node.WestAdjacent = currY;
                n++;

                if (last != null) last.SouthAdjacent = node;

                last = node;

                currY = currY.SouthAdjacent;
            }

            currY = this.TopLeft.NorthAdjacent;

            last = null;

            while (currY  != null)
            {
                Node2 node = new Node2(replaceChar);
                node.isCorner = true;

                node.NorthAdjacent = last;
                currY.WestAdjacent = node;
                node.EastAdjacent = currY;
                n++;

                if (last != null) last.SouthAdjacent = node;

                last = node;

                if (currY.SouthAdjacent == null) break;
                currY = currY.SouthAdjacent;
            }

            this.TopLeft = this.TopLeft.NorthAdjacent.WestAdjacent;

            currX = currY.WestAdjacent;

            last = null;

            while (currX != null)
            {
                Node2 node = new Node2(replaceChar);
                node.isCorner = true;

                node.WestAdjacent = last;
                currX.SouthAdjacent = node;
                node.NorthAdjacent = currX;
                n++;

                if (last != null) last.EastAdjacent = node;

                last = node;

                currX = currX.EastAdjacent;
            }

            setDiagonals();

        }

        private void resetIsCorner()
        {
            Node2 row = this.TopLeft;
            Node2 column = null;

            while (row != null)
            {
                row.isCorner = false;

                if (row.EastAdjacent == null) column = row;
                row = row.EastAdjacent;
            }

            while (column != null)
            {
                column.isCorner = false;

                column = column.SouthAdjacent;
            }

            column = TopLeft;

            while (column != null)
            {
                column.isCorner = false;

                if (column.SouthAdjacent == null) row = column;
                column = column.SouthAdjacent;
            }

            while (row != null)
            {
                row.isCorner = false;

                row = row.EastAdjacent;
            }

        }

        private void setDiagonals()
        {
            Node2 currY = this.TopLeft;

            while (currY != null)
            {
                Node2 currX = currY;

                while (currX != null)
                {
                    Node2 NW = getNW(currX);
                    Node2 NE = getNE(currX);
                    Node2 SW = getSW(currX);
                    Node2 SE = getSE(currX);

                    if (NW != null) currX.NorthWestAdjacent = NW;
                    if (NE != null) currX.NorthEastAdjacent = NE;
                    if (SW != null) currX.SouthWestAdjacent = SW;
                    if (SE != null) currX.SouthEastAdjacent = SE;

                    currX = currX.EastAdjacent;
                }

                currY = currY.SouthAdjacent;
            }
        }

        private Node2 getNW(Node2 node)
        {
            if (node.WestAdjacent != null && node.NorthAdjacent != null) return node.WestAdjacent.NorthAdjacent;

            return null;
        }

        private Node2 getSW(Node2 node)
        {
            if (node.WestAdjacent != null && node.SouthAdjacent != null) return node.WestAdjacent.SouthAdjacent;

            return null;
        }

        private Node2 getNE(Node2 node)
        {
            if (node.EastAdjacent != null && node.NorthAdjacent != null) return node.EastAdjacent.NorthAdjacent;

            return null;
        }

        private Node2 getSE(Node2 node)
        {
            if (node.EastAdjacent != null && node.SouthAdjacent != null) return node.EastAdjacent.SouthAdjacent;

            return null;
        }

        public void Print()
        {
            Node2 currY = this.TopLeft;

            while (currY != null)
            {
                Node2 currX = currY;

                while (currX != null)
                {
                    currX.Print();

                    currX = currX.EastAdjacent;
                }

                Console.WriteLine();
                currY = currY.SouthAdjacent;
            }
        }
        public int CountLitPixels()
        {
            Node2 currY = this.TopLeft;
            int count = 0;

            while (currY != null)
            {
                Node2 currX = currY;

                while (currX != null)
                {
                    if (currX.Pixel == '#') count++;

                    currX = currX.EastAdjacent;
                }

                
                currY = currY.SouthAdjacent;
            }
            return count;
        }


        public static Graph ConstructGraph(Node2[,] arr)
        {
            Graph graph = null;

            int maxY = arr.GetLength(0);
            int maxX = arr.GetLength(1);

            for (int i = 0; i < maxY; i++)
            {
                for (int j = 0; j < maxX; j++)
                {
                    Node2 node = arr[i, j];

                    if (i == 0 && j == 0) graph = new Graph(node);

                    graph.n++;

                    node.NorthAdjacent = i >= 1 ? arr[i - 1, j] : null;
                    node.SouthAdjacent = i <= maxY - 1 - 1 ? arr[i + 1, j] : null;

                    node.WestAdjacent = j >= 1 ? arr[i, j - 1] : null;
                    node.EastAdjacent = j <= maxX - 1 - 1 ? arr[i, j + 1] : null;

                    if (node.NorthAdjacent != null && node.WestAdjacent != null) node.NorthWestAdjacent = arr[i - 1, j - 1];

                    if (node.NorthAdjacent != null && node.EastAdjacent != null) node.NorthEastAdjacent = arr[i - 1, j + 1];

                    if (node.SouthAdjacent != null && node.WestAdjacent != null) node.SouthWestAdjacent = arr[i + 1, j - 1];

                    if (node.SouthAdjacent != null && node.EastAdjacent != null) node.SouthEastAdjacent = arr[i + 1, j + 1];
                    
                }
            }


            return graph;
        }

    }

    public class Node2
    {
        public char Pixel;
        public char CalcPixel;

        public Node2 NorthAdjacent;
        public Node2 SouthAdjacent;
        public Node2 EastAdjacent;
        public Node2 WestAdjacent;

        public Node2 NorthWestAdjacent;
        public Node2 NorthEastAdjacent;
        public Node2 SouthWestAdjacent;
        public Node2 SouthEastAdjacent;

        public bool isCorner = false;

        public Node2(char pixel)
        {
            this.Pixel = pixel;
            this.CalcPixel = pixel;

            this.NorthAdjacent = null;
            this.SouthAdjacent = null;
            this.EastAdjacent = null;
            this.WestAdjacent = null;
            this.NorthWestAdjacent = null;
            this.NorthEastAdjacent = null;
            this.SouthWestAdjacent = null;
            this.SouthEastAdjacent = null;
        }

        public void Print()
        {
            Console.Write(Pixel);
        }

        public int binaryToIndex()
        {
            string binary = "";

            binary += (this.NorthWestAdjacent == null || this.NorthWestAdjacent.CalcPixel == '.') ? 0 : 1;
            binary += (this.NorthAdjacent == null || this.NorthAdjacent.CalcPixel == '.') ? 0 : 1;
            binary += (this.NorthEastAdjacent == null || this.NorthEastAdjacent.CalcPixel == '.') ? 0 : 1;
            binary += (this.WestAdjacent == null || this.WestAdjacent.CalcPixel == '.') ? 0 : 1;
            binary += (this.CalcPixel == '.') ? 0 : 1;
            binary += (this.EastAdjacent == null || this.EastAdjacent.CalcPixel == '.') ? 0 : 1;
            binary += (this.SouthWestAdjacent == null || this.SouthWestAdjacent.CalcPixel == '.') ? 0 : 1;
            binary += (this.SouthAdjacent == null || this.SouthAdjacent.CalcPixel == '.') ? 0 : 1;
            binary += (this.SouthEastAdjacent == null || this.SouthEastAdjacent.CalcPixel == '.') ? 0 : 1;

            return Convert.ToInt32(binary, 2);

        }

    }

    
}
