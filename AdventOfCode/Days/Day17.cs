using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode_2021.Days
{
    class Day17
    {

        public static void Run()
        {
            (int targetX1, int targetX2) targetXs;
            (int targetY1, int targetY2) targetYs;

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day17_TargetArea.txt"))
            {
                string line = sr.ReadLine();

                int index = line.IndexOf('x');

                string[] arr = line.Substring(index).Split(", ");
                arr[0] = arr[0].Substring(2);
                arr[1] = arr[1].Substring(2);

                string[] Xs = arr[0].Split("..");
                string[] Ys = arr[1].Split("..");

                targetXs = (int.Parse(Xs[0]), int.Parse(Xs[1]));
                targetYs = (int.Parse(Ys[1]), int.Parse(Ys[0]));

            }

            Console.WriteLine(targetXs.targetX1 + " " + targetXs.targetX2);
            Console.WriteLine(targetYs.targetY1 + " " + targetYs.targetY2);

            int maxYVelocity = Math.Abs(targetYs.targetY2) - 1;

            // Part 1

            List<int> possibleXVelocities = new List<int>();

            int sum = 0;

            for (int i = 1; i < targetXs.targetX2; i++)
            {
                sum += i;

                if (sum >= targetXs.targetX1 && sum <= targetXs.targetX2) possibleXVelocities.Add(i);
            }

            int maxY = -1;

            for (int y = maxYVelocity; y >= 0; y--)
            {
                foreach (int x in possibleXVelocities)
                {

                    if (simulateTrajectory(targetXs, targetYs, x, y, 0 + x, 0 + y)) maxY = y;

                    if (maxY != -1) break;
                }

                if (maxY != -1) break;
            }

            sum = maxY * (maxY + 1) / 2;

            Console.WriteLine("Part 1 Max Y Trajectory: " + sum);

            // Part 2

            int count = 0;

            for (int y = maxYVelocity; y >= targetYs.targetY2; y--)
            {
                for (int x = 0; x <= targetXs.targetX2; x++)
                {
                    //count += countTrajectories(targetXs, targetYs, x, y, 0, 0);

                    if (simulateTrajectory(targetXs, targetYs, x, y, 0 + x, 0 + y)) count++;
                    
                }
            }

            Console.WriteLine("Part 2 Count of Possible Trajectories: " + count);


        }

        private static bool simulateTrajectory((int targetX1, int targetX2) targetXs, (int targetY1, int targetY2) targetYs, int xVelocity, int yVelocity, int xPosition, int yPosition)
        {
            if (xPosition > targetXs.targetX2) return false;

            if (yPosition < targetYs.targetY2) return false;

            if (xPosition >= targetXs.targetX1 && xPosition <= targetXs.targetX2 && yPosition >= targetYs.targetY2 && yPosition <= targetYs.targetY1) return true;

            int newXVelocity = xVelocity == 0 ? 0 : xVelocity - 1;

            return simulateTrajectory(targetXs, targetYs, newXVelocity, yVelocity - 1, xPosition + newXVelocity, yPosition + (yVelocity - 1));
        }

    }
}
