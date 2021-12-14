using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode_2021.Days
{
    class Day13
    {

        public static void Run()
        {
            char[,] grid;
            List<(char axis, int coordinate)> folds = new List<(char, int)>();

            int maxX = 0;
            int maxY = 0;

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day13_DotsAndFolds.txt"))
            {
                List<(int x, int y)> coords = new List<(int, int)>();

                

                string line = sr.ReadLine();

                while (line.Length != 0)
                {
                    string[] arr = line.Split(",");

                    int x = int.Parse(arr[0]);
                    int y = int.Parse(arr[1]);

                    maxX = x > maxX ? x : maxX;
                    maxY = y > maxY ? y : maxY;

                    coords.Add((x, y));

                    line = sr.ReadLine();
                }

                maxX += 1; maxY += 1;
                grid = new char[maxX, maxY];

                for (int x = 0; x < maxX; x++)
                {
                    for (int y = 0; y < maxY; y++)
                    {
                        grid[x, y] = '.';
                    }
                }

                foreach ((int x, int y) in coords)
                {
                    grid[x, y] = '#';
                }
                
                while (sr.Peek() > -1)
                {
                    string[] arr = sr.ReadLine().Split("=");

                    char axis = arr[0][arr[0].Length - 1];
                    int coord = int.Parse(arr[1]);

                    folds.Add((axis, coord));
                }

            }


            // Part 2

           foreach ((char axis, int fold) in folds)
            {
                switch(axis)
                {
                    case 'x':
                        foldAboutX(grid, ref maxX, maxY, fold);
                        break;
                    case 'y':
                        foldAboutY(grid, ref maxY, maxX, fold);
                        break;
                }

            }

            for (int i = 0; i < maxY; i++)
             {
                 for (int j = 0; j < maxX; j++)
                 {
                     Console.Write(grid[j, i] + " ");
                 }
                 Console.WriteLine();
             }

        }

        public static void foldAboutX(char[,] grid, ref int maxX, int maxY, int fold)
        {
            for (int y = 0; y < maxY; y++)
            {
                for (int i = 1; i <= fold; i++)
                {
                    if (grid[fold + i, y] == '#') grid[fold - i, y] = '#';
                }
            }

            maxX /= 2;
        }

        public static void foldAboutY(char[,] grid, ref int maxY, int maxX, int fold)
        {
            for (int x = 0; x < maxX; x++)
            {
                for (int i = 1; i <= fold; i++)
                {
                    if (grid[x, fold + i] == '#') grid[x, fold - i] = '#';
                }
            }

            maxY /= 2;
        }

    }
}
