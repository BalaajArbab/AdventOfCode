using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode_2021.Days
{
    class Day7
    {

        public static void Run()
        {
            List<short> crabs = new List<short>();

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day7_CrabPositions.txt"))
            {
                string[] arr = sr.ReadLine().Split(",");

                foreach (string s in arr) crabs.Add(short.Parse(s));

            }


            // Part 1

            int crabCount = crabs.Count;

            crabs.Sort();

            int median = crabs[crabCount / 2];

            int fuelUsage = 0;

            foreach (short n in crabs) fuelUsage += Math.Abs(median - n);

            Console.WriteLine($"\nPart 1 Fuel used: {fuelUsage} to move to horizontal position {median}");


            // Part 2

            int sum = 0;

            foreach (short n in crabs) sum += n;

            int average = (int)(sum / crabCount); // The mean rounded down used in following foreach loop

            fuelUsage = 0;
            int fuelUsage2 = 0;

            foreach (short n in crabs)
            {

                int difference = Math.Abs(average - n);

                int summation = (int)(difference * (difference + 1) / 2f);

                fuelUsage += summation;              

            }

            average += 1; // The mean rounded up used in following foreach loop

            foreach (short n in crabs)
            {

                int difference = Math.Abs(average - n);

                int summation = (int)(difference * (difference + 1) / 2f);

                fuelUsage2 += summation;

            }

            int minFuelUsage = fuelUsage < fuelUsage2 ? fuelUsage : fuelUsage2; // Minimum fuel usage between calculation of fuel usage between the 2 means (rounded up and down)

            Console.WriteLine($"Part 2 Fuel used: {minFuelUsage}");

        }

    }
}
