using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode_2021.Days
{
    class Day18
    {
        
        public static void Run()
        {
            List<List<string>> snailfishNumbers = new List<List<string>>();

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day18_SnailfishNumbers.txt"))
            {
                while (sr.Peek() > -1)
                {
                    snailfishNumbers.Add(structureLine(sr.ReadLine()));
                }
            }

            List<string> structuredLine = snailfishNumbers[0];


            // Part 1

           for (int i = 1; i < snailfishNumbers.Count; i++)
            {
                structuredLine = add2Numbers(structuredLine, snailfishNumbers[i]);

                reduce(structuredLine);

            }

            Console.WriteLine($"Part 1 Magnitude: {computeMagnitude(structuredLine)}");


            // Part 2

            int highestMagnitude = -1;

            for (int i = 0; i < snailfishNumbers.Count - 1; i++)
            {
                for (int j = i + 1; j < snailfishNumbers.Count; j++)
                {
                    structuredLine = add2Numbers(snailfishNumbers[i], snailfishNumbers[j]);

                    reduce(structuredLine);

                    int mag = computeMagnitude(structuredLine);

                    if (mag > highestMagnitude) highestMagnitude = mag;
                }
            }

            snailfishNumbers.Reverse();

            for (int i = 0; i < snailfishNumbers.Count - 1; i++)
            {
                for (int j = i + 1; j < snailfishNumbers.Count; j++)
                {
                    structuredLine = add2Numbers(snailfishNumbers[i], snailfishNumbers[j]);

                    reduce(structuredLine);

                    int mag = computeMagnitude(structuredLine);

                    if (mag > highestMagnitude) highestMagnitude = mag;
                }
            }

            Console.WriteLine($"Part 2 Highest Magnitude: {highestMagnitude}");
        }

        private static int computeMagnitude(List<string> structuredLine)
        {
            Stack<string> stack = new Stack<string>();

            int magnitude = -1;

            foreach (string s in structuredLine)
            {
                if (s != "]") stack.Push(s);
                
                else
                {
                    int rightNumber = int.Parse(stack.Pop());
                    int leftNumber = int.Parse(stack.Pop());
                    stack.Pop();

                    rightNumber *= 2;
                    leftNumber *= 3;

                    string sum = (leftNumber + rightNumber).ToString();

                    stack.Push(sum);
                    magnitude = int.Parse(sum);
                }
            }

            return magnitude;

        }

        private static void reduce(List<string> structuredLine)
        {
            while (true)
            {
                int explodeIndex = explodeFinder(structuredLine);
                
                if (explodeIndex != -1)
                {
                    explode(structuredLine, explodeIndex);
                    continue;
                }

                int splitIndex = splitFinder(structuredLine);

                if(splitIndex != -1)
                {
                    split(structuredLine, splitIndex);
                    continue;
                }

                break;
            }
        }

        private static int explodeFinder(List<string> structuredLine)
        {
            int stack = 0;

            for (int i = 0; i < structuredLine.Count; i++)
            {
                if (structuredLine[i] == "[") stack++;

                else if (structuredLine[i] == "]") stack--;

                if (stack == 5) return i + 1;
            }

            return -1;
        }

        private static void explode(List<string> structuredLine, int index)
        {
            int indexOf2ndElement = index + 1;

            for (int i = index - 1; i >= 1; i--)
            {
                if (structuredLine[i] != "[" && structuredLine[i] != "]")
                {
                    structuredLine[i] = (int.Parse(structuredLine[i]) + int.Parse(structuredLine[index])).ToString();
                    break;
                }
            }

            for (int i = indexOf2ndElement + 1; i < structuredLine.Count - 1; i++)
            {
                if (structuredLine[i] != "[" && structuredLine[i] != "]")
                {
                    structuredLine[i] = (int.Parse(structuredLine[i]) + int.Parse(structuredLine[indexOf2ndElement])).ToString();
                    break;
                }
            }

            structuredLine.RemoveAt(index + 2);
            structuredLine.RemoveAt(index + 1);
            structuredLine.RemoveAt(index);

            structuredLine[index - 1] = "0";
        }

        private static int splitFinder(List<string> structuredLine)
        {
            for (int i = 0; i < structuredLine.Count; i++)
            {
                int n = -1;
                int.TryParse(structuredLine[i], out n);
                if (n >= 10) return i;
            }

            return -1;
        }

        private static void split(List<string> structuredLine, int index)
        {
            int n = int.Parse(structuredLine[index]);

            int n1, n2;

            if (n % 2 == 0)
            {
                n1 = n / 2;
                n2 = n1;
            }
            else
            {
                n1 = n / 2;
                n2 = n1 + 1;
            }

            structuredLine[index] = "[";
            structuredLine.Insert(index + 1, n1.ToString());
            structuredLine.Insert(index + 2, n2.ToString());
            structuredLine.Insert(index + 3, "]");
        }

        private static List<string> add2Numbers(List<string> number1, List<string> number2)
        {
            List<string> sum = new List<string>();

            sum.Add("[");

            foreach (string s in number1) sum.Add(s);

            foreach (string s in number2) sum.Add(s);

            sum.Add("]");

            return sum;
        }

        private static void writeNumberToConsole(List<string> structuredLine)
        {
            for (int i = 0; i < structuredLine.Count; i++)
            {
                Console.Write(structuredLine[i]);

                try
                {
                    int.Parse(structuredLine[i]);
                    if (structuredLine[i + 1] != "]") Console.Write(",");
                }
#pragma warning disable CS0168 // Variable is declared but never used
                catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
                {
                    if (i + 1 != structuredLine.Count && structuredLine[i] == "]" && structuredLine[i + 1] != "]") Console.Write(",");
                }
            }

            Console.WriteLine();
        }

        private static List<string> structureLine(string snailfishNumber)
        {
            List<string> number = new List<string>();

            foreach (char c in snailfishNumber)
            {
                if (c != ',') number.Add(c.ToString());
            }

            return number;
        }


    }
}
