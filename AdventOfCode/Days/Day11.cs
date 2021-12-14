using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode_2021.Days
{
    class Day11
    {

        public static void Run()
        {
            List<List<int>> energyLevels = new List<List<int>>();



            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day11_EnergyLevels.txt"))
            {

                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();

                    List<int> list = new List<int>();

                    foreach (char c in line) list.Add(int.Parse(c.ToString()));

                    energyLevels.Add(list);

                }

            }


            // Part 2, Part 1 was replaced 

            int lines = energyLevels.Count;
            int locations = energyLevels[0].Count;

            for (int i = 0;; i++)
            {

                Queue<(int value, int y, int x)> queue = new Queue<(int value, int y, int x)>();

                for (int y = 0; y < lines; y++)
                {
                    for (int x = 0; x < locations; x++)
                    {
                        energyLevels[y][x]++;

                        if (energyLevels[y][x] == 10) queue.Enqueue((energyLevels[y][x], y, x));
                    }
                }

                while (queue.Count > 0)
                {
                    (int value, int y, int x) t = queue.Dequeue();

                    incrementAdjacent(energyLevels, t.y, t.x, queue);

                }

                int octopiCount = lines * locations;
                int flashes = 0;

                for (int y = 0; y < lines; y++)
                {
                    for (int x = 0; x < locations; x++)
                    {
                        if (energyLevels[y][x] >= 10)
                        {
                            energyLevels[y][x] = 0;
                            flashes++;
                        }
                    }
                }

                if (flashes == octopiCount)
                {
                    Console.WriteLine("Part 2 Steps required: " + (i + 1));
                    break;
                }
                

            }
            

        }

        private static void incrementAdjacent(List<List<int>> grid, int y, int x, Queue<(int, int, int)> queue)
        {

            incrementOneAdjacent(grid, y, x, -1, -1, queue);
            incrementOneAdjacent(grid, y, x, -1, 0, queue);
            incrementOneAdjacent(grid, y, x, -1, 1, queue);
            incrementOneAdjacent(grid, y, x, 0, -1, queue);
            incrementOneAdjacent(grid, y, x, 0, 1, queue);
            incrementOneAdjacent(grid, y, x, 1, -1, queue);
            incrementOneAdjacent(grid, y, x, 1, 0, queue);
            incrementOneAdjacent(grid, y, x, 1, 1, queue);

        }

        private static void incrementOneAdjacent(List<List<int>> grid, int y, int x, int changeY, int changeX, Queue<(int value, int y, int x)> queue)
        {
            try
            {
                int val = ++grid[y + changeY][x + changeX];

                if (val == 10)
                {
                    queue.Enqueue((val, y + changeY, x + changeX));
                }
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
            {

            }
        }

    }
}
