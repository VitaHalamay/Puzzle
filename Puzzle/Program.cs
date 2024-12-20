using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Puzzle
{
    static class Program
    {
        static void Main()
        {
            string filePath = "..\\..\\..\\fragments.txt";
            Console.WriteLine("Loading fragments...");
            var fragments = LoadFragments(filePath);
            Console.WriteLine("\nStarting calculations...");
            string longestChain = FindLongestChain(fragments);
            Console.WriteLine("\nCalculations completed.");
            Console.WriteLine("\nLongest sequence: " + longestChain);
        }

        static List<string> LoadFragments(string filePath)
        {
            var fragments = new List<string>();
            foreach (var line in File.ReadLines(filePath))
            {
                fragments.Add(line.Trim());
            }
            return fragments;
        }

        static Dictionary<string, List<string>> BuildGraph(List<string> fragments)
        {
            var graph = new Dictionary<string, List<string>>();
            for (int i = 0; i < fragments.Count; i++)
            {
                for (int j = 0; j < fragments.Count; j++)
                {
                    if (i == j) continue;
                    string firstEnd = fragments[i].Substring(fragments[i].Length - 2);  
                    string secondStart = fragments[j].Substring(0, 2); 
                    if (firstEnd == secondStart)
                    {
                        if (!graph.ContainsKey(fragments[i])) graph[fragments[i]] = new List<string>();
                        graph[fragments[i]].Add(fragments[j]);
                    }
                }
            }
            return graph;
        }

        static string FindLongestChain(List<string> fragments)
        {
            var graph = BuildGraph(fragments);
            string longestChain = string.Empty;

            foreach (var fragment in fragments)
            {
                var currentChain = DFS(fragment, graph, new HashSet<string>(), fragment);
                if (currentChain.Length > longestChain.Length)
                {
                    longestChain = currentChain;
                }
            }

            return longestChain;
        }

        static string DFS(string currentFragment, Dictionary<string, List<string>> graph, HashSet<string> visited, string currentChain)
        {
            visited.Add(currentFragment);
            string longest = currentChain;

            if (graph.ContainsKey(currentFragment))
            {
                foreach (var neighbor in graph[currentFragment])
                {
                    if (!visited.Contains(neighbor))
                    {
                        var newChain = DFS(neighbor, graph, new HashSet<string>(visited), currentChain + neighbor.Substring(2)); 
                        if (newChain.Length > longest.Length)
                        {
                            longest = newChain;
                        }
                    }
                }
            }

            visited.Remove(currentFragment);
            return longest;
        }
    }
}