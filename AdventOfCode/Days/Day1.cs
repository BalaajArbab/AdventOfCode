using System;
using System.IO;
using System.Collections.Generic;

namespace AdventOfCode_2021.Days
{
    class Day1
    {
        public static void Run()
        {
            List<int> depths = new List<int>();
                                               
            int increasingDepthCount = 0;      

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day1_Depths.txt"))
            {
                string line = "";

                while (true)
                {
                    line = sr.ReadLine();

                    if (line == null) break;

                    int depth = int.Parse(line);

                    depths.Add(depth);
                }
            }

            for (int i = 1; i < depths.Count; i++)
            {
                if (depths[i] > depths[i - 1]) increasingDepthCount++;
            }

            Console.WriteLine("Increasing Depth Count:\n" + increasingDepthCount);

            int threeMeasurementIncresingCount = 0;

            for (int i = 1; i < depths.Count - 2; i++)
            {
                int prevDepth = depths[i - 1] + depths[i] + depths[i + 1];
                int currDepth = depths[i] + depths[i + 1] + depths[i + 2];

                if (currDepth > prevDepth) threeMeasurementIncresingCount++;
            }

            Console.WriteLine("Three Measurement Sliding Window Increasing Depth Count:\n" + threeMeasurementIncresingCount);

        }
    }
}
