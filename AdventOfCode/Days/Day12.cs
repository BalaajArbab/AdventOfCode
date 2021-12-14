using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode_2021.Days
{
    class Day12
    {

        public static void Run()
        {

            Dictionary<string, List<string>> edges = new Dictionary<string, List<string>>();

            using (StreamReader sr = new StreamReader("..\\..\\..\\datasets\\Day12_CaveConnections.txt"))
            {
                while (sr.Peek() > -1)
                {
                    string[] edge = sr.ReadLine().Split("-");

                    string start = edge[0];
                    string dest = edge[1];

                    if (!edges.ContainsKey(start))
                    {
                        List<string> list = new List<string>();
                        list.Add(dest);

                        edges[start] = list;
                        
                    }
                    else
                    {
                        edges[start].Add(dest);
                    }

                    if (!edges.ContainsKey(dest))
                    {
                        List<string> list = new List<string>();
                        list.Add(start);

                        edges[dest] = list;

                    }
                    else
                    {
                        edges[dest].Add(start);
                    }

                }

            }

            // Part 2

            Node startNode = new Node("start");

            ConstructAllPaths(edges, startNode);

            Console.WriteLine($"Part 2 Number of Paths to End: {countEnds(startNode)}");



        }

        private static int countEnds(Node r)
        {
            if (r.Name == "end") return 1;

            int count = 0;

            foreach (Node n in r.Children)
            {
                count += countEnds(n);
            }

            return count;

        }

        private static void ConstructAllPaths(Dictionary<string, List<string>> edgeDict, Node r)
        {

            List<string> edges = edgeDict[r.Name]; // List of destinations for current node.

            foreach (string s in edges)
            {
                if (!r.PastNodesInPath.Contains(s)) // If destination node not contained in path so far.
                {
                    Node child = new Node(s);
                    addPastNodeChain(r.PastNodesInPath, child.PastNodesInPath);
                    child.AddPastNode(r.Name);

                    if (r.SomethingTwice) child.SomethingTwice = true;

                    r.AddChild(child);
                }
                else if (isStringUpper(s)) // If destination node is Big Cave and can be revisited.
                {
                    Node child = new Node(s);
                    addPastNodeChain(r.PastNodesInPath, child.PastNodesInPath);
                    child.AddPastNode(r.Name);

                    if (r.SomethingTwice) child.SomethingTwice = true;

                    r.AddChild(child);
                }

                else if (!r.SomethingTwice && s != "start")
                {
                    Node child = new Node(s);
                    addPastNodeChain(r.PastNodesInPath, child.PastNodesInPath);
                    child.AddPastNode(r.Name);

                    child.SomethingTwice = true;
                    

                    r.AddChild(child);
                }
                
            }

            foreach (Node child in r.Children)
            {
                if (child.Name == "end") continue;

                ConstructAllPaths(edgeDict, child);
            }


        }

        private static bool isStringUpper(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsUpper(c)) return false;
            }

            return true;
        }

        private static void addPastNodeChain(List<string> pastNodes, List<string> currentNodePastNodes)
        {
            foreach (string s in pastNodes)
            {
                currentNodePastNodes.Add(s);
            }
        }

    }

    class Node
    {
        public string Name { get; }

        public List<string> PastNodesInPath { get; }
        public int CountOfPastNodes { get; set; } 
        public List<Node> Children { get; }
        public int CountOfChildren { get; set; }

        public bool SomethingTwice { get; set; }

        public Node(string name)
        {
            this.Name = name;

            this.PastNodesInPath = new List<string>();
            this.Children = new List<Node>();
            this.CountOfPastNodes = 0;
            this.CountOfChildren = 0;
            this.SomethingTwice = false;
        }

        public void AddChild(Node child)
        {
            this.Children.Add(child);
            this.CountOfChildren++;
        }

        public void AddPastNode(string pastNode)
        {
            this.PastNodesInPath.Add(pastNode);
            this.CountOfPastNodes++;
        }

        public bool IsLeaf()
        {
            if (CountOfChildren == 0) return true;

            return false;
        }

    }

   
}
