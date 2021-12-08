using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace AdventOfCode_2021.Days
{
    class Day4
    {
        public static void Run()
        {
            List<int> numbersCalledOut = new List<int>();

            List<int[,]> boards = new List<int[,]>();
         
            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day4_Bingo.txt"))
            {


                string line;

                line = sr.ReadLine();

                string[] numbers = line.Split(",");

                foreach (string s in numbers) numbersCalledOut.Add(int.Parse(s));

                while (sr.Peek() >= 0)
                {
                    sr.ReadLine();

                    int[,] arr = new  int[5, 5];

                    for (int j = 0; j < 5; j++)
                    {
                        line = sr.ReadLine();

                        numbers = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                        int i = 0;

                        foreach (string s in numbers)
                        {
                            int number = int.Parse(s);

                            arr[j, i] = number;

                            i++;
                        }
                    }

                    boards.Add(arr);
                }               

                // PART 1

                int[,] marked = new int[boards.Count, 10];

                int[,] winningBoard = new int[1, 1];

                bool toBreak = false;
                
                int howFar = 0;
                foreach (int n in numbersCalledOut)
                {
                    howFar++;

                    int k = 0;
                    foreach (int[,] board in boards)
                    {

                        for (int j = 0; j < 5; j++)
                        {

                            for (int i = 0; i < 5; i++)
                            {
                                if (board[j, i] == n)
                                {
                                    marked[k, j]++;
                                    marked[k, i + 5]++;
                                }
                            }
                        }

                        k++;
                    }
                    
                    for (int i = 0; i < boards.Count; i++)
                    {
                        for (int j = 0; j < marked.GetLength(1); j++)
                        {
                            if (marked[i, j] == 5)
                            {                               
                                winningBoard = boards[i];
                                
                                toBreak = true;

                            }
                        }
                    }                   

                    if (toBreak) break;
                }

                int score = 0;

                int[,] mate = new int[5, 5];

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        for (int n = 0; n < howFar; n++)
                        {
                            if (winningBoard[i, j] == numbersCalledOut[n]) mate[i, j] = 1;
                        }
                    }
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (mate[i, j] == 0)
                        {
                            score += winningBoard[i, j];
                        }                       
                        
                    }

                }

                score *= numbersCalledOut[howFar - 1];

                Console.WriteLine($"Score: {score}");

                // Part 2

                score = 0;
                toBreak = false;

                int[,] marked2 = new int[boards.Count, 10];

                int[,] losingBoard = new int[5, 5];

                howFar = 0;
                List<int> winnerBoards = new List<int>();

                foreach (int n in numbersCalledOut)
                {
                    howFar++;

                    int k = 0;
                    foreach (int[,] board in boards)
                    {

                        for (int j = 0; j < 5; j++)
                        {

                            for (int i = 0; i < 5; i++)
                            {
                                if (board[j, i] == n)
                                {
                                    marked2[k, j]++;
                                    marked2[k, i + 5]++;
                                }
                            }
                        }

                        k++;
                    }

                    for (int i = 0; i < boards.Count; i++)
                    {
                        for (int j = 0; j < marked2.GetLength(1); j++)
                        {

                            if ((marked2[i, j] == 5) && (winnerBoards.Count == (boards.Count - 1) && !winnerBoards.Contains(i)))
                            {
                                losingBoard = boards[i];

                                winnerBoards.Add(i);
                                toBreak = true;

                            }

                            if (marked2[i, j] == 5 && !winnerBoards.Contains(i))
                            {
                                winnerBoards.Add(i);
                            }

                            
                        }
                    }

                    if (toBreak) break;
                }

                mate = new int[5, 5];

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        for (int n = 0; n < howFar; n++)
                        {                            
                            if (losingBoard[i, j] == numbersCalledOut[n]) mate[i, j] = 1;

                        }
                    }
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (mate[i, j] == 0)
                        {
                            score += losingBoard[i, j];
                        }

                    }

                }

                score *= numbersCalledOut[howFar - 1];

                Console.WriteLine($"Score 2: {score}");

            }
        }
    }
}
