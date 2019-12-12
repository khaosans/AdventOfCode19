using System;
using System.Collections.Generic;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Orbit> orbits = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day6/day6Sample.txt".ParseOrbits();

            var dictionary = CreateOrbitalMap(orbits);

            var keyValuePairs = dictionary.ToDictionary(x => x.Value, x => x.Key);


            int count = keyValuePairs.Keys.Count(x => x.Contains("D"));

        }

        private static Dictionary<string, List<string>> CreateOrbitalMap(List<Orbit> orbits)
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();

            foreach (var orbit in orbits)
            {
                if (!dictionary.ContainsKey(orbit.CenterOfMass))
                {
                    dictionary[orbit.CenterOfMass] = new List<string> {orbit.Satellite};
                }
                else
                {
                    dictionary[orbit.CenterOfMass].Add(orbit.Satellite);
                }
            }

            return dictionary;
        }
    }


    public class Orbit
    {
        public string CenterOfMass { get; }
        public string Satellite { get; }

        public Orbit(string centerOfMass, string satellite)
        {
            CenterOfMass = centerOfMass;
            Satellite = satellite;
        }

        public override string ToString()
        {
            return $"{Satellite} orbits {CenterOfMass}";
        }
    }
}