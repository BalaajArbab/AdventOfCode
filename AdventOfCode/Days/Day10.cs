using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode_2021.Days
{
    class Day10
    {      
        public static void Run()
        {
            List<string> lines = new List<string>();


            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day10_Brackets.txt"))
            {

                while (sr.Peek() >= 0)
                {
                    lines.Add(sr.ReadLine());
                }

            }


            // Part 1

            Dictionary<char, char> mapping = new Dictionary<char, char>();

            mapping.Add('(', ')');
            mapping['['] = ']';
            mapping['{'] = '}';
            mapping['<'] = '>';

            int[] incorrects = new int[4];

            List<int> indicesToRemove = new List<int>();
            int i = 0;
            foreach (string s in lines)
            {
                Stack<char> stack = new Stack<char>();

                foreach (char c in s)
                {

                    if ("({[<".Contains(c)) stack.Push(c);

                    else
                    {
                        
                        if (mapping[stack.Peek()] == c) stack.Pop();

                        else
                        {
                            switch (c)
                            {
                                case ')':
                                    incorrects[0]++;
                                    break;
                                case ']':
                                    incorrects[1]++;
                                    break;
                                case '}':
                                    incorrects[2]++;
                                    break;
                                case '>':
                                    incorrects[3]++;
                                    break;

                            }

                            indicesToRemove.Add(i);
                            break;
                        }                       
                        
                    }

                }

                i++;
            }

            int syntaxErrorScore = incorrects[0] * 3 + incorrects[1] * 57 + incorrects[2] * 1197 + incorrects[3] * 25137;

            Console.WriteLine($"Part 1 Syntax Error Score: {syntaxErrorScore}");


            // Part 2

            indicesToRemove.Sort();
            indicesToRemove.Reverse();

            foreach (int n in indicesToRemove) lines.RemoveAt(n);

            List<long> scores = new List<long>();

            foreach (string s in lines)
            {
                Stack<char> stack = new Stack<char>();

                foreach (char c in s)
                {

                    if ("([{<".Contains(c)) stack.Push(c);

                    else
                    {

                        if (mapping[stack.Peek()] == c) stack.Pop();

                    }
                }

                string completionString = "";

                while (stack.Count > 0) completionString += mapping[stack.Pop()];

                scores.Add(determineScore(completionString));

            }

            scores.Sort();

            long middleScore = scores[scores.Count / 2];

            Console.WriteLine($"Part 2 Middle Score: {middleScore}");


        }

        public static long determineScore(string completionString)
        {
            long score = 0;

            foreach (char c in completionString)
            {
                score *= 5;
                
                switch (c)
                {
                    case ')':
                        score += 1;
                        break;
                    case ']':
                        score += 2;
                        break;
                    case '}':
                        score += 3;
                        break;
                    case '>':
                        score += 4;
                        break;

                }

            }
            
            return score;
        }
        
        
    }   
}
