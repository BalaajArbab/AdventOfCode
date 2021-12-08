using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode_2021.Days
{
    class Day5
    {

        public static void Run()
        {
            List<int[]> startPoints = new List<int[]>();
            List<int[]> endPoints = new List<int[]>();

            int highestX = 0;
            int highestY = 0;

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day5_Coordinates.txt"))
            {
                string line;

                while (sr.Peek() >= 0)
                {
                    line = sr.ReadLine();

                    string[] arr = line.Split(" -> ");

                    
                    string[] arr2 = arr[0].Split(",");

                    int[] arr3 = new int[2];

                    arr3[0] = int.Parse(arr2[0]);
                    arr3[1] = int.Parse(arr2[1]);

                    highestX = arr3[0] > highestX ? arr3[0] : highestX;
                    highestY = arr3[1] > highestY ? arr3[1] : highestY;

                    startPoints.Add(arr3);

                    arr2 = arr[1].Split(",");

                    arr3 = new int[2];

                    arr3[0] = int.Parse(arr2[0]);
                    arr3[1] = int.Parse(arr2[1]);

                    highestX = arr3[0] > highestX ? arr3[0] : highestX;
                    highestY = arr3[1] > highestY ? arr3[1] : highestY;

                    endPoints.Add(arr3);

                }



            }

            // Part 1
            highestX++;
            highestY++;

            int[,] map = new int[highestX, highestY];

            for (int i = 0; i < startPoints.Count; i++)
            {
                List<int> points = pointsInLineSegment(startPoints[i], endPoints[i]);

                if (points != null)
                {

                    int horizontalLine = points[0];

                    
                    for (int j = 1; j < points.Count; j++)
                    {
                        
                        if (horizontalLine == 1)
                        {
                            int y = startPoints[i][1];

                            map[points[j], y]++;
                        }

                        else
                        {
                            int x = startPoints[i][0];

                            map[x, points[j]]++;
                        }
                    }
                }

            }

            int count = 0;

            for (int i = 0; i < highestX; i++)
            {
                for (int j = 0; j < highestX; j++)
                {
                    if (map[i, j] >= 2) count++;
                }

            }

            Console.WriteLine("Part 1, Count of >= 2: " + count);

            // Part 2

            for (int i = 0; i < startPoints.Count; i++)
            {
                List<int[]> diagonalPoints;

                diagonalPoints = diagonals(startPoints[i], endPoints[i]);

                if (diagonalPoints == null) continue;

                foreach (int[] point in diagonalPoints)
                {
                    map[point[0], point[1]]++;
                }
            }

            count = 0;

            for (int i = 0; i < highestX; i++)
            {
                for (int j = 0; j < highestX; j++)
                {
                    if (map[i, j] >= 2) count++;

                }

            }

            Console.WriteLine("Part 2, Count of >= 2: " + count);

        }

        private static List<int> pointsInLineSegment(int[] start, int[] end)
        {
            int[] larger;
            int[] smaller;

            List<int> points = new List<int>();

            if (start[0] == end[0]) // Vertical Line
            {
                if (start[1] > end[1])
                {
                    larger = start;
                    smaller = end;
                }

                else
                {
                    larger = end;
                    smaller = start;
                }

                points.Add(0);

                for (int i = smaller[1]; i <= larger[1]; i++) points.Add(i);

                return points;
            }

            if (start[1] == end[1]) // Horizontal Line
            {
                if (start[0] > end[0])
                {
                    larger = start;
                    smaller = end;
                }

                else 
                {
                    larger = end;
                    smaller = start;
                }

                points.Add(1);
                for (int i = smaller[0]; i <= larger[0]; i++) points.Add(i);

                return points;
            }

            return null;
        }

        private static List<int[]> diagonals(int[] start, int[] end)
        {
            List<int[]> points = new List<int[]>();

            int[] lefter;
            int[] righter;

            bool increasing;

            if (start[0] < end[0])
            {
                lefter = start;
                righter = end;
            }

            else
            {
                lefter = end;
                righter = start;
            }

            int differenceInY = righter[1] - lefter[1];
            int differenceInX = righter[0] - lefter[0];

            if (differenceInX != Math.Abs(differenceInY)) return null;

            if (differenceInY > 0) increasing = true;

            else increasing = false;

            if (increasing)
            {
                for (int i = 0; i <= differenceInX; i++)
                {
                    int[] p = { lefter[0] + i, lefter[1] + i };

                    points.Add(p);
                }
            }

            else
            {
                for (int i = 0; i <= differenceInX; i++)
                {
                    int[] p = { lefter[0] + i, lefter[1] - i };

                    points.Add(p);
                }
            }

            return points;
        }
    }

}
