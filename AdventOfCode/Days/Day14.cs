using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode_2021.Days
{
    class Day14
    {

        public static void Run()
        {
            string polymerTemplate;

            Dictionary<string, char> polymerMapping = new Dictionary<string, char>();

            Dictionary<string, long> polymerCount = new Dictionary<string, long>();

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day14_PolymerizationChain.txt"))
            {

                polymerTemplate = sr.ReadLine();

                sr.ReadLine();

                while (sr.Peek() > -1 )
                {
                    string[] arr = sr.ReadLine().Split(" -> ");

                    polymerMapping[arr[0]] = arr[1][0];

                    if (!polymerCount.ContainsKey(arr[0])) polymerCount[arr[0]] = 0;
                }
            
            }



            // Part 2

            int stepCount = 40;

            for (int i = 0; i < polymerTemplate.Length - 1; i++) // Constructing dictionary of all possible polymers of length 2, also on line 31
            {
                string polymer = polymerTemplate[i].ToString() + polymerTemplate[i + 1];

                if (!polymerCount.ContainsKey(polymer)) polymerCount[polymer] = 1;

                else polymerCount[polymer]++;
            }


            for (int i = 0; i < stepCount; i++)
            {
                List<(string poly, long count)> updateStep = new List<(string, long)>();
                List<string> updateToZero = new List<string>();

                foreach (KeyValuePair<string, long> kvp in polymerCount)
                {
                    long multiplier = kvp.Value;
                    string polymer = kvp.Key;

                    if (polymerMapping.ContainsKey(polymer))
                    {
                        char newElement = polymerMapping[polymer];

                        string newPoly1 = polymer[0].ToString() + newElement;
                        string newPoly2 = newElement.ToString() + polymer[1];

                        updateToZero.Add(polymer);

                        updateStep.Add((newPoly1, 1 * multiplier));
                        updateStep.Add((newPoly2, 1 * multiplier));
                    }

                }

                foreach (string s in updateToZero) polymerCount[s] = 0;

                foreach ((string poly, long count) e in updateStep)
                {
                    polymerCount[e.poly] += e.count;
                }
            }

            Dictionary<char, long> letterCounts = new Dictionary<char, long>();

            for (int i = 65; i <= 90; i++) // Initialize key for each letter
            {
                char letter = Convert.ToChar(i);
                letterCounts[letter] = 0;
            }

            foreach (KeyValuePair<string, long> kvp in polymerCount)
            {
                string polymer = kvp.Key;

                letterCounts[polymer[0]] += kvp.Value;
                letterCounts[polymer[1]] += kvp.Value;
                
            }

            List<long> listOfCounts = new List<long>();

            foreach (KeyValuePair<char, long> kvp in letterCounts)
            {
                bool isAtStart = kvp.Key == polymerTemplate[0];
                bool isAtEnd = kvp.Key == polymerTemplate[polymerTemplate.Length - 1];

                listOfCounts.Add(computeTrueCount(kvp.Value, isAtStart, isAtEnd));

            }

            listOfCounts.Sort();

            long min = 0;

            foreach (long n in listOfCounts)
            {

                if (n != 0)
                {
                    min = n;
                    break;
                }

            }

            Console.WriteLine($"Part 2: Most common minus least common: {(listOfCounts[25] - min)}");

        }

        private static long computeTrueCount(long count, bool isAtStart, bool isAtEnd)
        {
            int countError = 0;

            if (isAtStart) countError++;

            if (isAtEnd) countError++;

            return (count + countError) / 2;

        }
       
    }
}
