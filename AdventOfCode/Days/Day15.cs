using System;
using System.Collections.Generic;
using System.IO;


namespace AdventOfCode_2021.Days
{
    class Day15
    {

        public static void Run()
        {

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            List<List<int>> grid = new List<List<int>>();

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day15_RiskLevels.txt"))
            {
                while (sr.Peek() > -1)
                {
                    List<int> row = new List<int>();

                    string line = sr.ReadLine();

                    foreach (char c in line) row.Add((int)c - 48);

                    grid.Add(row);
                }
                
            }

            int xLength = grid[0].Count;
            int yLength = grid.Count;

            // Part 2 Grid Construction

            for (int i = 1; i < 5; i++)
            {
                elongateRow(grid, xLength * i, xLength * i + xLength, 0, yLength);
            }

            for (int i = 0; i < yLength * 4; i++)
            {
                List<int> newList = new List<int>();

                grid.Add(newList);
            }

            for (int i = 0; i < 5; i++)
            {               
                for (int j = 1; j < 5; j++)
                {
                    extendColumn(grid, xLength * i, xLength * i + xLength, yLength * j, yLength * j + yLength);
                }
            }

            // Part 2 Grid Construction end

            Vertex[,] vertices;

            xLength = grid[0].Count;
            yLength = grid.Count;

            constructGraph(grid, yLength, xLength, out vertices);

            Vertex start = vertices[0, 0];
            Vertex end = vertices[yLength - 1, xLength - 1];
            
            Console.WriteLine("Part 2: AStar: " + AStar(vertices, start, end));

            stopwatch.Stop();

            Console.WriteLine("Seconds: {0}", stopwatch.ElapsedMilliseconds / 1000f);

            // visualize(vertices, end);

            // gridVisualization(grid);        

        }

        private static void elongateRow(List<List<int>> grid, int startX, int endX, int startY, int endY)
        {
            int lengthOfXBlock = endX - startX;

            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    int numberToAdd = grid[y][x - lengthOfXBlock] + 1;
                    if (numberToAdd == 10) numberToAdd = 1;

                    grid[y].Add(numberToAdd);
                }
            }

        }

        private static void extendColumn(List<List<int>> grid, int startX, int endX, int startY, int endY)
        {
            int lengthOfYBlock = endY - startY;           

            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    int numberToAdd = grid[y - lengthOfYBlock][x] + 1;
                    if (numberToAdd == 10) numberToAdd = 1;

                    grid[y].Add(numberToAdd);
                }
            }

        }

        private static void gridVisualization(List<List<int>> grid)
        {
            int k = 0;
            foreach (List<int> list in grid)
            {
                if (k != 0 && k % 10 == 0) Console.WriteLine("----------------------------------------------------------------------------------------------------------------");

                for (int i = 0; i < list.Count; i++)
                {
                    if (i != 0 && i % 10 == 0) Console.Write(" | ");

                    Console.Write(list[i] + " ");

                }


                Console.WriteLine();
                k++;
            }

        }

        private static void backtrack(Vertex[,] vertices, Vertex end)
        {
            end.Heuristic = 0;
            if (end.Parent != null)
            {
                backtrack(vertices, end.Parent);
            }
        }

        private static void visualize(Vertex[,] vertices, Vertex end)
        {

            backtrack(vertices, end);

            for (int y = 0; y < vertices.GetLength(0); y++)
            {
                for (int x = 0; x < vertices.GetLength(1); x++)
                {
                    if (vertices[y, x].Heuristic == 0) Console.Write(String.Format("{0, 5} ", "*" + vertices[y, x].Cost));

                    else Console.Write(String.Format("{0, 5} ", vertices[y, x].Cost));
                }

                Console.WriteLine();
            }

            for (int y = 0; y < vertices.GetLength(0); y++)
            {
                for (int x = 0; x < vertices.GetLength(1); x++)
                {
                    Console.Write(String.Format("{0, -5}", vertices[y, x].Heuristic));
                }

                Console.WriteLine();
            }
        }


        private static void constructGraph(List<List<int>> grid, int yLength, int xLength, out Vertex[,] nodes)
        {
            Vertex[,] vertices = new Vertex[yLength, xLength];

            for (int y = 0; y < yLength; y++)
            {
                for (int x = 0; x < xLength; x++)
                {
                    if (vertices[y, x] == null)
                    {
                        Vertex vertex = new Vertex(grid[y][x]);
                        vertices[y, x] = vertex;

                        computeHeuristic(vertex, y, x, yLength, xLength);

                    }
                }
            }

            for (int y = 0; y < yLength; y++)
            {
                for (int x = 0; x < xLength; x++)
                {
                    connectEdgesToAVertex(vertices, y, x, yLength, xLength);
                }
            }

            nodes = vertices;

        }

        private static void connectEdgesToAVertex(Vertex[,] vertices, int y, int x, int yLength, int xLength)
        {
            Vertex vertex = vertices[y, x];

            if (x - 1 >= 0) vertex.adjacencyList.Add(vertices[y, x - 1]);

            if (y - 1 >= 0) vertex.adjacencyList.Add(vertices[y - 1, x]);

            if (x + 1 < xLength) vertex.adjacencyList.Add(vertices[y, x + 1]);

            if (y + 1 < yLength) vertex.adjacencyList.Add(vertices[y + 1, x]);
        }

        private static void computeHeuristic (Vertex vertex, int y, int x, int yLength, int xLength)
        {
            int h = Math.Abs(x - (xLength - 1)) + Math.Abs(y - (yLength - 1)); // -1 is for index count correction. 

            vertex.Heuristic = h;
        }

        private static int AStar (Vertex[,] vertices, Vertex start, Vertex end)
        {
            PQueue pQueue = new PQueue();

            pQueue.Enqueue(new PQueueItem(start, start.Cost, 0, null));

            int shortestPathCost = -1;

            while (!pQueue.IsEmpty())
            {
                PQueueItem item = pQueue.Dequeue();

                Vertex vertex = item.Vertex;
                int pathCost = item.PathCost;


                if (!vertex.IsMarked) vertex.Parent = item.Parent;
                vertex.IsMarked = true;

                

                if (vertex == end)
                {
                    shortestPathCost = pathCost;
                    break;
                }

                

                foreach (Vertex v in vertex.adjacencyList)
                {
                    if (!v.IsMarked)
                    {
                        PQueueItem newPath = new PQueueItem(v, v.Cost + pathCost, v.Cost + pathCost + v.Heuristic, vertex);
                        pQueue.Enqueue(newPath);

                    }
                }


            }

            //while (!pQueue.IsEmpty()) Console.WriteLine("path cost: " + pQueue.Dequeue().HeuristicCost);

            return shortestPathCost - start.Cost;
        }


    }


    class Vertex
    {
        public int Cost { get; set; }
        public int Heuristic { get; set; }

        public int d { get; set; }

        public List<Vertex> adjacencyList;

        public Vertex Parent;

        public bool IsMarked { get; set; } = false;

        public Vertex(int cost)
        {
            this.Cost = cost;
            this.d = cost;
            this.adjacencyList = new List<Vertex>();
            this.Parent = null;
        }

    }

    // Basic barebones Priority Queue implementation using a Min Heap structure with a list as the underlying data structure.
    // .NET didn't have an implementation for Priority Queue until .NET 6, hence this.
    class PQueue
    {
        List<PQueueItem> MinHeapList = new List<PQueueItem>();

        public int Count
        {
            get
            {
                return this.MinHeapList.Count;
            }
        }

        public PQueue()
        {

        }

        private int indexOfParent(int i)
        {
            if (i == 0) return -1; // i is the root

            return (i - 1) / 2;
        }

        private int indexOfLeftChild(int i)
        {
            return (i * 2) + 1;
        }

        private int indexOfRightChild(int i)
        {
            return (i * 2) + 2;
        }

        private void shiftUp(int i)
        {
            int parentIndex = indexOfParent(i);

            if (parentIndex >= 0 && MinHeapList[i].HeuristicCost < MinHeapList[parentIndex].HeuristicCost)
            {
                swap(i, parentIndex);
                shiftUp(parentIndex);
            }
        }

        private void shiftDown(int i)
        {
            int currentPriority = MinHeapList[i].HeuristicCost;

            int leftChild = indexOfLeftChild(i);
            int rightChild = indexOfRightChild(i);

            bool elseif = true;

            bool leftIsSmaller = true;

            try
            {

                if (MinHeapList[leftChild] != null && MinHeapList[rightChild] != null) leftIsSmaller = MinHeapList[leftChild].HeuristicCost < MinHeapList[rightChild].HeuristicCost;

                else if (MinHeapList[leftChild] == null) leftIsSmaller = false;

            }

#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception e)
            {

            }

            try
            {
                if (leftIsSmaller && currentPriority > MinHeapList[leftChild].HeuristicCost)
                {
                    swap(i, leftChild);
                    shiftDown(leftChild);

                    elseif = false;
                }
            }
            catch (Exception e)
            {

            }

            try
            {
            if (elseif && MinHeapList[rightChild] != null && currentPriority > MinHeapList[rightChild].HeuristicCost)
                {
                    swap(i, rightChild);
                    shiftDown(rightChild);
                }
            }
            catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
            {

            }

        }

        public PQueueItem Dequeue()
        {
            PQueueItem root = MinHeapList[0];

            int lastIndex = MinHeapList.Count - 1;

            MinHeapList[0] = MinHeapList[lastIndex];
            MinHeapList.RemoveAt(lastIndex);

            if (!IsEmpty()) shiftDown(0);

            return root;
        }

        public void Enqueue(PQueueItem item)
        {
            int lastIndex = MinHeapList.Count;

            MinHeapList.Add(item);

            shiftUp(lastIndex);
        }

        public bool IsEmpty()
        {
            return MinHeapList.Count == 0;
        }

        private void swap(int i, int j)
        {
            PQueueItem temp = MinHeapList[i];

            MinHeapList[i] = MinHeapList[j];

            MinHeapList[j] = temp;
        }

    }


    class PQueueItem : IComparable<PQueueItem>
    {
        public Vertex Vertex { get; }
        public int PathCost { get; }

        public int HeuristicCost { get; }

        public Vertex Parent;

        public PQueueItem(Vertex vertex, int pathCost, int heuristicCost, Vertex parent)
        {
            this.Vertex = vertex;
            this.PathCost = pathCost;
            this.HeuristicCost = heuristicCost;
            this.Parent = parent;
        }

        public int CompareTo(PQueueItem other)
        {
            if (this.PathCost == other.PathCost) return 0;

            if (this.PathCost > other.PathCost) return 1;

            return -1;
        }

    }

}
