using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode_2021.Days
{
    class Day6
    {

        public static void Run()
        {
            List<sbyte> lanternFish = new List<sbyte>();
            List<sbyte> lanternFishPart2;

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day6_Fish.txt")) 
            {

                string[] arr = sr.ReadLine().Split(",");

                foreach (string s in arr) lanternFish.Add(sbyte.Parse(s));

                lanternFishPart2 = new List<sbyte>(lanternFish); 
            }

            // PART 1

            for (int i = 1; i <= 80; i++)
            {

                int currentFishCount = lanternFish.Count;

                for (int j = 0; j < currentFishCount; j++)
                {
                    lanternFish[j]--;

                    if (lanternFish[j] == -1)
                    {
                        lanternFish[j] = 6;

                        lanternFish.Add(8);
                    }
                }

            }

            Console.WriteLine($"Part 1 Lanternfish Count: {lanternFish.Count}");

            // PART 2

            long[] lanternFishPerDayTable = new long[9];

            foreach (int n in lanternFishPart2) lanternFishPerDayTable[n]++;

            for (int i = 1; i <= 256; i++)
            {
                long prev = lanternFishPerDayTable[8];

                for (int j = 8; j >= 0; j--)
                {
                    if (j != 0)
                    {

                        long temp = lanternFishPerDayTable[(j - 1) % 9];

                        lanternFishPerDayTable[(j - 1) % 9] = prev;

                        prev = temp;

                    }

                    else 
                    {
                        lanternFishPerDayTable[6] += prev;
                        lanternFishPerDayTable[8] = prev;
                    }

                }
               
            }

            long count = 0;

            foreach (long n in lanternFishPerDayTable) count += n;

            Console.WriteLine($"Part 2 Lanternfish Count: {count}");

        }

        
    }
}
