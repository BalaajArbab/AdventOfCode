using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode_2021.Days
{
    class Day2
    {
        public static void Run()
        {

            List<string> moves = new List<string>();
            List<int> moveValues = new List<int>();

            int depth = 0;
            int horizontal = 0;

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day2_Moves.txt"))
            {

                string line;

                while(sr.Peek() >= 0)
                {
                    line = sr.ReadLine();

                    if (line == null) break;
                 
                    string[] move = line.Split(" ");

                    moves.Add(move[0]);
                    moveValues.Add(int.Parse(move[1]));

                } 

            }

            /* for (int i = 0; i < moves.Count; i++)
            {
                Console.WriteLine($"{moves[i]} {moveValues[i]}");
            } */

            for (int i = 0; i < moves.Count; i++)
            {
                switch(moves[i])
                {
                    case "forward":
                        horizontal += moveValues[i];
                        break;
                    case "down":
                        depth += moveValues[i];
                        break;
                    case "up":
                        depth -= moveValues[i];
                        break;

                }
            }

            Console.WriteLine($"Part 1\nDepth: {depth} Horizontal: {horizontal}\nDepth * Horizontal: {depth * horizontal}");

            int aim = 0;
            depth = 0;


            for (int i = 0; i < moves.Count; i++)
            {
                switch (moves[i])
                {
                    case "forward":
                        depth += aim * moveValues[i];
                        break;
                    case "down":
                        aim += moveValues[i];
                        break;
                    case "up":
                        aim -= moveValues[i];
                        break;

                }
            }

            Console.WriteLine($"Part 2\nDepth: {depth} Horizontal: {horizontal}\nDepth * Horizontal: {depth * horizontal}");

        }
    }
}
