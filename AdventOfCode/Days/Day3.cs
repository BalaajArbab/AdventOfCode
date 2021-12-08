using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode_2021.Days
{
    class Day3
    {

        public static void Run()
        {
            List<string> bits = new List<string>();

         
            string gammaBits = "";
            string epsilonBits = "";

            int bitLength;

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day3_Bits.txt"))
            {

                while(sr.Peek() >= 0)
                {
                    string bit = sr.ReadLine();

                    bits.Add(bit);

                }

                bitLength = bits[0].Length;
            }


            //PART 1

            for (int i = 0; i < bitLength; i++)
            {

                int frequencyOf1 = 0;


                for (int j = 0; j < bits.Count; j++)
                {
                    if (bits[j][i] == '1') frequencyOf1++;
                }

                if (frequencyOf1 >= (bits.Count - frequencyOf1)) gammaBits += '1';

                else gammaBits += '0';
            }

            for (int i = 0; i < bitLength; i++)
            {
                if (gammaBits[i] == '1') epsilonBits += '0';

                else epsilonBits += '1';
            }

            Console.WriteLine($"Gamma bits: {gammaBits}\nEpsilon Bits: {epsilonBits}");

            int gamma = Convert.ToInt32(gammaBits, 2);
            int epsilon = Convert.ToInt32(epsilonBits, 2);

            Console.WriteLine($"Gamma: {gamma} Epsilon: {epsilon} Multiplied: {gamma * epsilon}");


            // PART 2

            List<int> indicesToCheck = new List<int>();

            for (int i = 0; i < bits.Count; i++) indicesToCheck.Add(i);

            string oxygenBits = "";
            string carbonBits = "";

            for (int i = 0; i < bitLength; i++)
            {
                List<int> oneNumbers = new List<int>();
                List<int> zeroNumbers = new List<int>();

                int frequencyOf1 = 0;

                foreach (int j in indicesToCheck)
                {
                    if (bits[j][i] == '1')
                    {
                        frequencyOf1++;
                        oneNumbers.Add(j);
                    }

                    else zeroNumbers.Add(j);
                }
               
                bool oneMoreFrequent = frequencyOf1 >= (indicesToCheck.Count - frequencyOf1);              

                if (oneMoreFrequent) indicesToCheck = oneNumbers;

                else indicesToCheck = zeroNumbers;

                if (indicesToCheck.Count == 1)
                {
                    oxygenBits = bits[indicesToCheck[0]];
                    break;
                }

            }

            indicesToCheck = new List<int>();

            for (int i = 0; i < bits.Count; i++) indicesToCheck.Add(i);

            for (int i = 0; i < bitLength; i++)
            {
                List<int> oneNumbers = new List<int>();
                List<int> zeroNumbers = new List<int>();

                int frequencyOf1 = 0;

                foreach (int j in indicesToCheck)
                {
                    if (bits[j][i] == '1')
                    {
                        frequencyOf1++;
                        oneNumbers.Add(j);
                    }

                    else zeroNumbers.Add(j);
                }

                bool oneMoreFrequent = frequencyOf1 >= (indicesToCheck.Count - frequencyOf1);

                if (!oneMoreFrequent) indicesToCheck = oneNumbers;

                else indicesToCheck = zeroNumbers;

                if (indicesToCheck.Count == 1)
                {
                    carbonBits = bits[indicesToCheck[0]];
                    break;
                }

            }

            Console.WriteLine($"Oxygen Bits: {oxygenBits}\nCarbon Bits: {carbonBits}");

            int oxygenInt = Convert.ToInt32(oxygenBits, 2);
            int carbonInt = Convert.ToInt32(carbonBits, 2);

            Console.WriteLine($"Oxygen: {oxygenInt} Carbon: {carbonInt} Multiplied: {oxygenInt * carbonInt}");

        }

    }

    
}
