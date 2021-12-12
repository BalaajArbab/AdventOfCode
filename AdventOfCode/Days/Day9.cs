using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode_2021.Days
{
    class Day9
    {

        public static void Run()
        {
            List<List<int>> heights = new List<List<int>>();

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day9_Heights.txt"))
            {
                
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();

                    List<int> currentLine = new List<int>();

                    foreach (char c in line) currentLine.Add(int.Parse(c.ToString()));

                    heights.Add(currentLine);
                }

            }

            int lines = heights.Count;
            int locations = heights[0].Count;


            // Part 1

            List<int> lowPoints = new List<int>();

            for (int i = 0; i < lines; i++)
            {

                for (int j = 0; j < locations; j++)
                {

                    bool lowest = true;

                    
                    for (int k = 0; k < 4; k++)
                    {

                        try
                        {

                            if (k == 0) lowest = heights[i][j] < heights[i - 1][j];

                            else if (k == 1) lowest = heights[i][j] < heights[i + 1][j];

                            else if (k == 2) lowest = heights[i][j] < heights[i][j - 1];

                            else if (k == 3) lowest = heights[i][j] < heights[i][j + 1];

                            if (!lowest) break;

                        }
                        catch (Exception e)
                        {

                        }
                        
                    }

                    if (lowest) lowPoints.Add(heights[i][j]);
                    
                }

            }

            int sum = 0;

            foreach (int n in lowPoints) sum += n + 1;

            Console.WriteLine($"Part 1 Sum of risk level of low points: {sum}");


            // Part 2

            List<int> basinSizes = new List<int>();

            for (int y = 0; y < lines; y++)
            {
                for (int x = 0; x < locations; x++)
                {
                    int basinSize = floodFill(heights, y, x);

                    if (basinSize > 0) basinSizes.Add(basinSize);
                }
            }

            basinSizes.Sort();
            basinSizes.Reverse();

            int answer = basinSizes[0] * basinSizes[1] * basinSizes[2];

            Console.WriteLine($"Part 2: {answer}");

        }


        public static int floodFill(List<List<int>> grid, int y, int x)
        {
            if (grid[y][x] >= 9) return 0;

            int count = 0;

            grid[y][x] = 10;
            count++;

            try
            {
                count += floodFill(grid, y - 1, x);
            }
            catch (Exception e)
            {

            }

            try
            {
                count += floodFill(grid, y + 1, x);
            }
            catch (Exception e)
            {

            }

            try
            {
                count += floodFill(grid, y, x - 1);
            }
            catch (Exception e)
            {

            }

            try
            {
                count += floodFill(grid, y, x + 1);
            }
            catch (Exception e)
            {

            }

            return count;

        }
    }
}
