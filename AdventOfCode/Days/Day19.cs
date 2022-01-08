using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode_2021.Days
{
    class Day19
    {

        public static void Run()
        {
            List<Scanner> scanners = new List<Scanner>();

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day19_BeaconPositions.txt"))
            {

                while (sr.Peek() > -1)
                {
                    sr.ReadLine();

                    Scanner scanner = new Scanner();

                    string line = sr.ReadLine();

                    while (line != "" && line != null)
                    {

                        string[] arr = line.Split(",");

                        scanner.Beacons.Add(new Beacon(int.Parse(arr[0]), int.Parse(arr[1]), int.Parse(arr[2])));

                        line = sr.ReadLine();
                    }

                    scanners.Add(scanner);
                }
            }
                

            // Part 1

            scanners[0].setCoords(0, 0, 0);
            scanners[0].Found = true;

            int found = scanners.Count;
            found--;

            while (found > 0)

            for (int i = 0; i < scanners.Count; i++)
            {
                if (scanners[i].Found)
                {
                    for (int j = 0; j < scanners.Count; j++)
                    {
                        if (!scanners[j].Found)
                        {                            
                            if (findOverlapsBetween2Scanners(scanners[i], scanners[j]))
                            {
                                scanners[j].Found = true;
                                found--;
                            }
                        }
                    }
                }
            }

            HashSet<Beacon> set = new HashSet<Beacon>();

            foreach (Scanner s in scanners)
            {

                foreach (Beacon b in s.Beacons)
                {
                    set.Add(b);
                }
            }

            Console.WriteLine($"Part 1 Beacon Count: {set.Count}");


            // Part 2

            int maxManhattan = 0;

            for (int i = 0; i < scanners.Count - 1; i++)
            {
                for (int j = i + 1; j < scanners.Count; j++)
                {
                    int d = manhattanDistance(scanners[i], scanners[j]);

                    if (d > maxManhattan) maxManhattan = d;
                }
            }

            Console.WriteLine($"Part 2 Max Manhattan Distance: {maxManhattan}");
            
        }

        private static int manhattanDistance(Scanner scan1, Scanner scan2)
        {
            int x = Math.Abs(scan1.X - scan2.X);
            int y = Math.Abs(scan1.Y - scan2.Y);
            int z = Math.Abs(scan1.Z - scan2.Z);

            return (x + y + z);
        }

        private static void updateBeaconLocations(Scanner scanner)
        {
            List<Beacon> absoluteBeacons = new List<Beacon>();

            foreach (Beacon b in scanner.Beacons)
            {
                Beacon c = new Beacon(scanner.X - b.X, scanner.Y - b.Y, scanner.Z - b.Z);
                c.overlapped = b.overlapped;
                absoluteBeacons.Add(c);
            }

            scanner.Beacons = absoluteBeacons;
        }

        private static bool findOverlapsBetween2Scanners(Scanner baseScanner, Scanner orbitScanner)
        {
            
            List<List<Beacon>> permutedBeaconsList = permuteAllBeacons(orbitScanner.Beacons);

            foreach (List<Beacon> permutedBeacons in permutedBeaconsList)
            {
                Dictionary<Scanner, int> possibleScannerLocations = new Dictionary<Scanner, int>();

                foreach (Beacon bbase in baseScanner.Beacons)
                {
                    foreach (Beacon orbit in permutedBeacons)
                    {
                        int x = bbase.X + orbit.X;
                        int y = bbase.Y + orbit.Y;
                        int z = bbase.Z + orbit.Z;

                        Scanner scanner = new Scanner(x, y, z);                    

                        if (possibleScannerLocations.ContainsKey(scanner))
                        {
                            possibleScannerLocations[scanner]++;
                            orbit.overlapped = true;

                            if (possibleScannerLocations[scanner] == 12)
                            {
                                orbitScanner.setCoords(x, y, z);
                                orbitScanner.Beacons = permutedBeacons;
                                updateBeaconLocations(orbitScanner);
                                return true;
                            }
                        }

                        else possibleScannerLocations[scanner] = 1;
                    }
                }

            }

            return false;
        }

        private static List<List<Beacon>> permuteAllBeacons(List<Beacon> beaconList)
        {
            List<List<Beacon>> permutedLists = new List<List<Beacon>>();

            for (int i = 0; i < 48; i++) permutedLists.Add(new List<Beacon>());

            foreach (Beacon b in beaconList)
            {
                List<Beacon> onePermutedBeacon = permuteBeacon(b);

                for (int i = 0; i < 48; i++)
                {
                    Beacon currentBeacon = onePermutedBeacon[i];

                    permutedLists[i].Add(currentBeacon);
                }    
            }

            return permutedLists;
        }

        private static List<Beacon> permuteBeacon(Beacon b)
        {
            List<Beacon> permutedListOfBeacons = new List<Beacon>();

            
            for (int i = 0; i < 2; i++)
            {
                int x = b.X;

                if (i == 0) x *= -1;

                for (int j = 0; j < 2; j++)
                {
                    int y = b.Y;
                    if (j == 0) y *= -1;

                    for (int k = 0; k < 2; k++)
                    {
                        int z = b.Z;
                        if (k == 0) z *= -1;

                        Beacon newB = new Beacon(x, y, z);

                        permutedListOfBeacons.Add(newB);
                    }
                }
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Beacon oldB = permutedListOfBeacons[i];
                    Beacon newB = swapCoordsAtIndices(oldB, j);

                    permutedListOfBeacons.Add(newB);
                }
            }
            return permutedListOfBeacons;
        }

        private static Beacon swapCoordsAtIndices(Beacon beacon, int i)
        {
            Beacon b = new Beacon(beacon.X, beacon.Y, beacon.Z);

            if (i == 0)
            {
                int temp = b.X;
                b.X = b.Y;
                b.Y = temp;
            }
            else if (i == 1)
            {
                int temp = b.Y;
                b.Y = b.Z;
                b.Z = temp;
            }
            else if (i == 2)
            {
                int temp = b.X;
                b.X = b.Z;
                b.Z = temp;
            }
            else if (i == 3)
            {
                int temp = b.X;
                b.X = b.Y;
                b.Y = b.Z;
                b.Z = temp;
            }
            else if (i == 4)
            {
                int temp = b.X;
                b.X = b.Z;
                b.Z = b.Y;
                b.Y = temp;
            }

            return b;
        }

        private static double length(int x, int y, int z)
        {
            x = x * x;
            y = y * y;
            z = z * z;

            return Math.Sqrt(x + y + z);
        }

    }

    class Scanner : IEquatable<Scanner>
    {
        public List<Beacon> Beacons { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public bool Found = false;

        public Scanner()
        {
            Beacons = new List<Beacon>();
        }

        public Scanner(int x, int y, int z)
        {
            Beacons = new List<Beacon>();

            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public void setCoords(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Scanner);
        }

        public bool Equals(Scanner otherScanner)
        {
            if (otherScanner == null) return false;

            return (this.X == otherScanner.X && this.Y == otherScanner.Y && this.Z == otherScanner.Z);
            
        }

        public override int GetHashCode()
        {
            return (int)(X * Math.Pow(33, 3) + Y * Math.Pow(33, 2) + Z * Math.Pow(33, 1));
        }
    }

    class Beacon : IEquatable<Beacon>
    {
        public int X;
        public int Y;
        public int Z;

        public bool overlapped = false;

        public Beacon(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double Length()
        {
            int x = this.X * this.X;
            int y = this.Y * this.Y;
            int z = this.Z * this.Z;

            return Math.Sqrt(x + y + z);
        }

        public void PrintLengths()
        {
            Console.WriteLine($"X: {this.X}, Y: {this.Y}, Z: {this.Z}, Length: {this.Length()}");
        }

        public bool Equals(Beacon otherBeacon)
        {
            if (otherBeacon == null) return false;

            return (this.X == otherBeacon.X && this.Y == otherBeacon.Y && this.Z == otherBeacon.Z);
        }

        public override int GetHashCode()
        {
            return (int)(X * Math.Pow(33, 3) + Y * Math.Pow(33, 2) + Z * Math.Pow(33, 1));
        }

    }
}
