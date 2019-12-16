using System;
using System.Collections.Generic;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Orbit> orbits = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day6/day6.txt".ParseOrbits();

         //   var dictionary = CreateOrbitalMap(orbits);

         Dictionary<string,string> orbitalParent = CreateOrbitalParent(orbits);

         int shouldBe3 = GetPaths(orbitalParent, "D");

         int ShouldBe7 = GetPaths(orbitalParent, "L");

         int paths = GetPaths(orbitalParent, "COM");

         int count = 0;

         foreach (var orbit in orbits)
         {
             count += GetPaths(orbitalParent,orbit.Child);
             
         }
         
         



         Console.Out.WriteLine("count " + count);
        }

        public static int GetPaths(Dictionary<string, string> dictionary, string Sattelite)
        {
            if (!dictionary.ContainsKey(Sattelite) )
            {
                return 0;
            }
            
            var count = 1;
            while (dictionary[Sattelite] != "COM")
            {
                count++;
                Sattelite = dictionary[Sattelite];
            }

            return count;
        }

        public static int GetCount(Dictionary<string, List<string>> dictionary, List<Orbit> orbits, int counter)
        {
            if (orbits.Count == 0)
            {
                return counter;
            }

            if (dictionary.ContainsKey(orbits.First().Child))
            {
                List<string> children = dictionary[orbits.First().Child];
                return counter + GetCount(dictionary, orbits.Skip(1).ToList(), counter + children.Count);
            }

            return GetCount(dictionary, orbits.Skip(1).ToList(), counter);
        }


        private static Dictionary<string, List<string>> CreateOrbitalMap(List<Orbit> orbits)
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();

            foreach (var orbit in orbits)
            {
                if (!dictionary.ContainsKey(orbit.Parent))
                {
                    dictionary[orbit.Parent] = new List<string> {orbit.Child};
                }
                else
                {
                    dictionary[orbit.Parent].Add(orbit.Child);
                }
            }

            return dictionary;
        }

        private static Dictionary<string, string> CreateOrbitalParent(List<Orbit> orbits)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (var orbit in orbits)
            {
                if (orbit.Child != null)
                {
                    dictionary[orbit.Child] = orbit.Parent;
                }
            }

            return dictionary;
        }
    }


    public class Orbit
    {
        public string Parent { get; }
        public string Child { get; }

        public Orbit(string parent, string child)
        {
            Parent = parent;
            Child = child;
        }

        public override string ToString()
        {
            return $"{Child} orbits {Parent}";
        }
    }
}