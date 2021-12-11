using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode_2021.Days
{
    class Day8
    {

        public static void Run()
        {
            List<string[]> signals = new List<string[]>();
            List<string[]> outputs = new List<string[]>();

            // Parsing

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day8_Signals.txt"))
            {

                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    string[] arr = line.Split("|");

                    signals.Add(arr[0].Split(" ", StringSplitOptions.RemoveEmptyEntries));
                    outputs.Add(arr[1].Split(" ", StringSplitOptions.RemoveEmptyEntries));

                }

            }

            // Part 1

            int count = 0;

            foreach (string[] arr in outputs)
            {
                foreach (string s in arr)
                {
                    
                    switch(s.Length)
                    {
                        case (int)SegmentsRequiredForDigit.One:
                            count++;
                            break;
                        case (int)SegmentsRequiredForDigit.Four:
                            count++;
                            break;
                        case (int)SegmentsRequiredForDigit.Seven:
                            count++;
                            break;
                        case (int)SegmentsRequiredForDigit.Eight:
                            count++;
                            break;

                    }

                }
            }

            Console.WriteLine($"Count of 1, 4, 7 or 8 in output of entries: {count}");

        }

        public enum SegmentsRequiredForDigit
        {
            Zero = 6,
            One = 2,
            Two = 5,
            Three = 5,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 3,
            Eight = 7,
            Nine = 5

        }
    }

}
