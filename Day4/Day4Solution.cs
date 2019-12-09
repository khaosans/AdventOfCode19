using System;
using System.Collections.Generic;
using System.Linq;

namespace Day4V1
{
    public static class Day4Solution
    {
        public static string IsValidPart1(string range)
        {
            var (min, max) = ParseRange(range);


            List<Int32> list = new List<int>();
            for (var i = min; i < max; i++)
            {
                list.Add(i);
            }

            return list.Select(x => x.ToString()).Where(HasDoubleOrMore)
                .Where(Increasing)
                .ToList().Count.ToString();
        }

        public static string UsValidPart2(string range)
        {
            var (min, max) = ParseRange(range);

            List<Int32> list = new List<int>();
            for (var i = min; i < max; i++)
            {
                list.Add(i);
            }

            return list.Select(x => x.ToString()).Where(HasDoubleExactly)
                .Where(Increasing)
                .ToList().Count.ToString();
        }


        private static bool HasDoubleOrMore(string input)
        {
            for (var i = 0; i < input.Length - 1; ++i)
            {
                if (input[i].Equals(input[i + 1]))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasDoubleExactly(string input)
        {
            Dictionary<string, int> counter = new Dictionary<string, int>();

            foreach (var num in Enumerable.Range(0, 10))
            {
                counter[num.ToString()] = 0;
            }

            var secondToLast = input.Length-2;
            
            for (var i = 0; i < input.Length - 1; ++i)
            {
                if (i < secondToLast && input[i].Equals( input[i + 1]))
                {
                    counter[input[i].ToString()]++;
                }
            }
            
            if ( input[secondToLast].Equals( input[input.Length]))
            {
                counter[input[input.Length].ToString()]++;
            }
            return counter.Values.Any(x => x.Equals(1));
        }

        private static bool Increasing(string input)
        {
            for (var i = 0; i < input.Length - 1; ++i)
            {
                if (input[i] > input[i + 1])
                {
                    return false;
                }
            }

            return true;
        }

        private static (int Min, int Max) ParseRange(string inp)
        {
            var parts = inp.Split('-').Select(x => Convert.ToInt32(x)).ToList();
            return (parts[0], parts[1]);
        }
    }
}