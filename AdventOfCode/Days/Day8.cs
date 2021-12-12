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

            Console.WriteLine($"Part 1 Count of 1, 4, 7 or 8 in output of entries: {count}");


            // Part 2           

            /*
             *    2222
             *  3      0
             *  3      0
             *  3      0
             *    4444 
             *  6      1
             *  6      1
             *  6      1
             *    5555
             *    
             */

            foreach (string[] arr in signals) // Sort arrays of signals in ascending order of string length. 
            {
                selectionSortStringArrayByLength(arr);
            }

            int sum = 0;

            int k = 0;
            foreach (string[] arr in signals)
            {

                List<char>[] segmentPossibleChars = new List<char>[7];

                for (int i = 0; i < segmentPossibleChars.Length; i++)
                {
                    segmentPossibleChars[i] = new List<char>();
                }

                char[] chars;

                chars = splitStringIntoChars(arr[0]); // The 1

                storeChars(segmentPossibleChars, 0, chars.Length, chars);
                storeChars(segmentPossibleChars, 1, chars.Length, chars);

                foreach (char c in arr[1]) // The 7
                {
                    if (!arr[0].Contains(c))
                    {
                        segmentPossibleChars[2].Add(c); 
                        break;
                    }
                }

                chars = splitStringIntoChars(arr[2]); // The 4

                foreach (char c in arr[2])
                {
                    if (!arr[0].Contains(c))
                    {
                        segmentPossibleChars[3].Add(c);
                        segmentPossibleChars[4].Add(c);
                    }

                }

                string the5 = deduce5(arr[3], arr[4], arr[5], segmentPossibleChars);

                foreach (char c in the5)
                {
                    if (!arr[0].Contains(c) && !arr[1].Contains(c) && !arr[2].Contains(c)) segmentPossibleChars[5].Add(c);
                }

                string letters = "abcdefg";

                foreach (char c in letters)
                {
                    if (!arr[0].Contains(c) && !arr[1].Contains(c) && !arr[2].Contains(c) && !the5.Contains(c)) segmentPossibleChars[6].Add(c);
                }

                foreach (char c in segmentPossibleChars[1])
                {
                    if (the5.Contains(c))
                    {
                        segmentPossibleChars[0].Remove(c);
                        segmentPossibleChars[1].Remove(segmentPossibleChars[0][0]);

                        break;

                    }

                }

                string theNot5 = arr[3] == the5 ? arr[4] : arr[3];

                foreach (char c in segmentPossibleChars[4])
                {
                    if (theNot5.Contains(c))
                    {
                        segmentPossibleChars[3].Remove(c);
                        segmentPossibleChars[4].Remove(segmentPossibleChars[3][0]);

                        break;
                    }
                }


                string str = "";

                foreach (string s in outputs[k])
                {
                    if (s.Length == 5) str += determine5String(s, segmentPossibleChars);

                    else if (s.Length == 6) str += determine6String(s, segmentPossibleChars);

                    else str += determineUniques(s);
                }

                int digits = int.Parse(str);

                sum += digits;

                k++;

            }

            Console.WriteLine($"Part 2 Sum of all output digit strings: {sum}");

        }

        private static void selectionSortStringArrayByLength(string[] arr) // Need to learn linq then I wouldn't need to make this
        {
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                string temp = arr[i];

                int smallest = arr[i].Length;
                int indexOfSmallest = i;

                for (int j = 1 + i; j < n; j++)
                {
                    if (arr[j].Length < smallest)
                    {
                        smallest = arr[j].Length;
                        indexOfSmallest = j;
                    }
                }

                arr[i] = arr[indexOfSmallest];
                arr[indexOfSmallest] = temp;
            }

        }

        private static string deduce5(string one, string two, string three, List<char>[] segmentChars)
        {
            if (isThis5(one, segmentChars)) return one;
            if (isThis5(two, segmentChars)) return two;
            if (isThis5(three, segmentChars)) return three;

            return null;
        }

        private static bool isThis5(string str, List<char>[] segmentChars)
        {
            char possibleCharFor3And4_1 = segmentChars[3][0];
            char possibleCharFor3And4_2 = segmentChars[3][1];

            if (str.Contains(possibleCharFor3And4_1) && str.Contains(possibleCharFor3And4_2)) return true;

            return false;
        }

        private static char[] splitStringIntoChars(string s)
        {
            char[] chars = new char[s.Length];

            for(int i = 0; i < s.Length; i++)
            {
                chars[i] = s[i];
            }

            return chars;
        }

        private static void storeChars(List<char>[] arr, int index, int numberOfCharsToStore, char[] charsToStore)
        {

            for (int i = 0; i < numberOfCharsToStore; i++)
            {
                arr[index].Add(charsToStore[i]);
            }
        }

        private static char determine6String(string digit, List<char>[] segmentChars)
        {

            if (!digit.Contains(segmentChars[4][0])) return '0';

            if (digit.Contains(segmentChars[6][0])) return '6';

            return '9';
        }

        private static char determine5String(string digit, List<char>[] segmentChars)
        {
            if (digit.Contains(segmentChars[6][0])) return '2';

            if (digit.Contains(segmentChars[3][0])) return '5';

            return '3';
        }

        private static char determineUniques(string digits)
        {
            switch (digits.Length)
            {
                case (int)SegmentsRequiredForDigit.One:
                    return '1';
                case (int)SegmentsRequiredForDigit.Four:
                    return '4';
                case (int)SegmentsRequiredForDigit.Seven:
                    return '7';
                case (int)SegmentsRequiredForDigit.Eight:
                    return '8';
            }

            return 'a';
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
            Nine = 6

        }
    }

}
