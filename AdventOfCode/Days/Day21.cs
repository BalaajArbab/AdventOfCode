using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace AdventOfCode_2021.Days
{
    class Day21
    {

        public static void Run()
        {

            List<Player> playerList = new List<Player>();

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day21_Players.txt"))
            {
                while (sr.Peek() > -1)
                {
                    string[] line = sr.ReadLine().Split(" ");

                    Player player = new Player(int.Parse(line[1]), 0, int.Parse(line[4]) - 1);

                    playerList.Add(player);
                }
            }


            Player playerOne = playerList[0].Clone();
            Player playerTwo = playerList[1].Clone();


            // Part 1

            DeterministicDie die = new DeterministicDie(1, 100);

            int j = 0;
            while (true)
            {
                Player currentPlayer = playerList[j];

                int moveForward = 0;

                for (int i = 0; i < 3; i++) moveForward += die.getRoll();
                currentPlayer.NewPosition(moveForward);

                j = (j + 1) % playerList.Count;

                if (currentPlayer.Score >= 1000) break;
            }

            Player loser = playerList[mod(j, playerList.Count)];

            Console.WriteLine($"Part 1 Losing player score * number of dice rolls {loser.Score} * {die.numberOfRolls} = {loser.Score * die.numberOfRolls}");


            // Part 2

            Dictionary<(int p1, int p2, int s1, int s2), (ulong, ulong)> gameStates = new Dictionary<(int, int, int, int), (ulong, ulong)>();
            

            (ulong w1, ulong w2) = countWins(gameStates, playerOne, playerTwo);

            Console.WriteLine("Part 2\nPlayer One wins: " + w1 + "\nPlayer Two wins: " + w2);



        }

        // Written with help of https://www.youtube.com/watch?v=a6ZdJEntKkk
        private static (ulong w1, ulong w2) countWins(Dictionary<(int p1, int p2, int s1, int s2), (ulong w1, ulong w2)> gameStates, Player playerOne, Player playerTwo)
        {
            int currentPlayerPosition = playerOne.Position;
            int currentPlayerScore = playerOne.Score;

            int p2 = playerTwo.Position;
            int s2 = playerTwo.Score;

            if (currentPlayerScore >= 21) return (1, 0);

            if (s2 >= 21) return (0, 1);

            if (gameStates.ContainsKey((currentPlayerPosition, p2, currentPlayerScore, s2))) return gameStates[(currentPlayerPosition, p2, currentPlayerScore, s2)];

            (ulong, ulong) wins = (0L, 0L);

            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    for (int k = 1; k <= 3; k++)
                    {
                        int newPosition = (i + j + k + currentPlayerPosition) % 10;
                        int newScore = currentPlayerScore + newPosition + 1;

                        Player updatedPlayer = new Player(playerOne.PlayerID, newScore, newPosition);
                        Player otherPlayer = new Player(playerTwo.PlayerID, s2, p2);

                        (ulong x, ulong y) cWins = countWins(gameStates, otherPlayer, updatedPlayer);

                        wins = (wins.Item1 + cWins.y, wins.Item2 + cWins.x);
                    }
                }
            }

            gameStates[(currentPlayerPosition, p2, currentPlayerScore, s2)] = wins;

            return wins;

        }
        

        private static int mod(int x, int m)
        {
            int ans = x % m;
            return x < 0 ? (ans + m) : ans;
        }
    }


    public class Player
    {
        public int PlayerID;
        public int Score;
        public int Position;

        public Player(int id, int score, int position)
        {
            this.PlayerID = id;
            this.Score = score;
            this.Position = position;
        }

        public void NewPosition(int move)
        {
            if (move == 0) return;

            int newPosition = (Position + move) % 10;

            this.UpdateScore(newPosition + 1);

            this.Position = newPosition;

        }

        private void UpdateScore(int incScore)
        {
            this.Score += incScore;
        }

        public bool WinCheck(int winPoints)
        {
            if (this.Score >= winPoints) return true;

            return false;
        }

        public Player Clone()
        {
            return new Player(this.PlayerID, this.Score, this.Position);
        }


    }

    public class DeterministicDie
    {
        private int roll;
        public uint numberOfRolls = 0;

        private int sides;


        public DeterministicDie(int startingRoll, int sides)
        {
            this.roll = startingRoll;
            this.sides = sides;
        }

        public int getRoll()
        {
            int currentRoll = this.roll;

            roll++;
            roll = (roll % (sides + 1));

            roll = roll == 0 ? 1 : roll;

            numberOfRolls++;
            return currentRoll;
        }

    }
}
