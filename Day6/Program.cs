using System;
using System.Collections.Generic;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            //List<Orbit> orbits = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day6/ExamplePart2.txt".ParseOrbits();
            var minOrbitt = GetMinOrbit("/Users/souriyakhaosanga/Documents/AdventOfCode/Day6/day6.txt".ParseOrbits());
            Console.Out.WriteLine(minOrbitt);
            
            //283 
            

            // var max = Math.Max(list1.Count, list2.Count) -1;


            //var minOrbs = Math.Max(lessMatch.Count, sanMatch.Count);


            /*int shouldBe3 = CountPaths(orbitalParent, "D");
   
            int ShouldBe7 = CountPaths(orbitalParent, "L");
   
            int paths = CountPaths(orbitalParent, "COM");
   
            int count = 0;
   
            foreach (var orbit in orbits)
            {
                count += CountPaths(orbitalParent,orbit.Child);
                
            }*/
        }

        private static int GetMinOrbit(List<Orbit> parseOrbits)
        {
            List<Orbit> orbits = parseOrbits;
            var orbitalParent = CreateOrbitalParent(orbits);
            
            List<string> san = GetPath(orbitalParent, "SAN");
            List<string> you = GetPath(orbitalParent, "YOU");

            var list1 = san.Except(you).ToList();
            var list2 = you.Except(san).ToList();

            return list1.Count + list2.Count;
        }

        public static int CountPaths(Dictionary<string, string> dictionary, string Sattelite)
        {
            if (!dictionary.ContainsKey(Sattelite))
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

        public static List<string> GetPath(Dictionary<string, string> dictionary, string sattelite)
        {
            List<string> paths = new List<string>();
            if (!dictionary.ContainsKey(sattelite))
            {
                return paths;
            }

            while (sattelite != null && dictionary.ContainsKey(sattelite) && dictionary[sattelite] != sattelite)
            {
                string collection = dictionary[sattelite];
                paths.Add(collection);
                sattelite = dictionary[sattelite];
            }

            return paths;
        }


        /*
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
        */

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