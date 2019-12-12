using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day6
{
    public static class Extensions
    {
        public static List<Orbit> ParseOrbits(this string str)
        {
            var text = File.ReadAllText(str);
            var list = text.Split("\n").ToList();

            var list1 = list.Select(ParseOrbit).ToList();
            return list1;
        }

        public static Orbit ParseOrbit(string orbit)
        {
            var orbits = orbit.Split(")");
            return new Orbit(orbits[0], orbits[1]);
        }
    }
}