using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace AdventOfCode_2021.Days
{
    class Day16
    {

        public static void Run()
        {
            List<char> binaryString = new List<char>();

            Dictionary<char, string> hexToBinary = new Dictionary<char, string>()
            {
                { '0', "0000" },
                { '1', "0001" },
                { '2', "0010" },
                { '3', "0011" },
                { '4', "0100" },
                { '5', "0101" },
                { '6', "0110" },
                { '7', "0111" },
                { '8', "1000" },
                { '9', "1001" },
                { 'A', "1010" },
                { 'B', "1011" },
                { 'C', "1100" },
                { 'D', "1101" },
                { 'E', "1110" },
                { 'F', "1111" },
            };
            

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day16_Hexadecimal.txt"))
            {
                string file = sr.ReadToEnd();

                foreach(char c in file)
                {
                    
                    string binary = hexToBinary[c];

                    foreach (char c2 in binary)
                    {
                        binaryString.Add(c2);
                    }
                    
                }
            }

            (int count, int length, long value) t = countPacketVersions(binaryString, 0);

            Console.WriteLine($"\n\nVersion Sum: {t.count} Length: {t.length} Value: {t.value}");


        }

        private static (int count, int lengthOfThisPacket, long value) countPacketVersions(List<char> binaryString, int start)
        {

            string packetVersion;
            string typeID;

            try
            {
                packetVersion = concatBits(binaryString, start, start + 2);
                typeID = concatBits(binaryString, start + 3, start + 5);
            }

            catch (Exception e)
            {
                return (0, 0, 0);
            }

            int packetVersionInt = Convert.ToInt32(packetVersion, 2);
            int typeIDInt = Convert.ToInt32(typeID, 2);

            //if (packetVersionInt == 0) return (0, 0);

            bool isLiteral = typeIDInt == 4 ? true : false;

            int count = packetVersionInt;
            int packetLength = 6;

            if (!isLiteral)
            {
                char lengthTypeID = binaryString[start + 6];
                packetLength++;

                List<long> values = new List<long>();

                if (lengthTypeID == '0')
                {
                    int lengthOfSubPackets = Convert.ToInt32(concatBits(binaryString, start + 7, start + 21), 2);
                    packetLength += 15;

                    int totalLength = lengthOfSubPackets + packetLength;

                    (int count, int lengthOfThisPacket, long value) tuple = (0, 0, 0);

                    while (packetLength < totalLength)
                    {
                        tuple = countPacketVersions(binaryString, start + packetLength);

                        count += tuple.count;
                        packetLength += tuple.lengthOfThisPacket;
                        values.Add(tuple.value);
                    }

                }

                else // Length Type ID == '1'
                {
                    int numberOfSubPackets = Convert.ToInt32(concatBits(binaryString, start + 7, start + 17), 2);
                    packetLength += 11;

                    (int count, int lengthOfThisPacket, long value) tuple = countPacketVersions(binaryString, start + 17 + 1);

                    count += tuple.count;
                    values.Add(tuple.value);

                    numberOfSubPackets--;

                    packetLength += tuple.lengthOfThisPacket;

                    for (int i = numberOfSubPackets; i > 0; i--)
                    {
                        tuple = countPacketVersions(binaryString, start + packetLength);

                        count += tuple.count;
                        packetLength += tuple.lengthOfThisPacket;
                        values.Add(tuple.value);
                    }

                }

                long value = 0;

                switch (typeIDInt)
                {
                    case 0:
                        value = sum(values);
                        break;
                    case 1:
                        value = product(values);
                        break;
                    case 2:
                        value = minimum(values);
                        break;
                    case 3:
                        value = maximum(values);
                        break;
                    case 5:
                        value = greaterThan(values);
                        break;
                    case 6:
                        value = lessThan(values);
                        break;
                    case 7:
                        value = equal(values);
                        break;
                }

                return (count, packetLength, value);
            }
            else // Is a literal value
            {
                string fiveBits = "start";

                string allBits = "";


                while (fiveBits[0] != '0')
                {
                    fiveBits = "";

                    for (int i = 0; i < 5; i++)
                    {
                        fiveBits += binaryString[start + packetLength++];
                    }

                    allBits += fiveBits.Substring(1);
                }

                return (count, packetLength, Convert.ToInt64(allBits, 2));
            }
        }

        private static string concatBits(List<char> binaryString, int start, int end)
        {
            string s = "";

            for (int i = start; i <= end; i++) s += binaryString[i];

            return s;
        }

        private static long sum(List<long> values)
        {
            return values.Sum();
        }
        private static long product(List<long> values)
        {
            long product = 1;            
            foreach (long n in values) product *= n;

            return product;
        }
        private static long minimum(List<long> values)
        {
            return values.Min();
        }
        private static long maximum(List<long> values)
        {
            return values.Max();
        }
        private static int greaterThan(List<long> values)
        {
            return values[0] > values[1] ? 1 : 0;
        }
        private static int lessThan(List<long> values)
        {
            return values[0] < values[1] ? 1 : 0;
        }
        private static int equal(List<long> values)
        {
            return values[0] == values[1] ? 1 : 0;
        }



    }
}
