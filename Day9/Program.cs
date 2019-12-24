using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            List<BigInteger> code2 = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day9/testcase1.txt".ParseCsvStringBigInteger();


            var computer = new Computer(code2);

            Computer run = computer.Run(1);
            

            Console.Out.WriteLine($"Max ");
        }

        private static BigInteger FindMaxSignal(List<List<Computer>> allSeq)
        {
            BigInteger max = BigInteger.Zero;
            foreach (var computers in allSeq)
            {
                List<Computer> runWithFeedBack = RunWithFeedBack(computers);

                if (runWithFeedBack.Count > 0 && runWithFeedBack.Last().OutputHistory.Last() > max)
                {
                    max = runWithFeedBack.Last().OutputHistory.Last();
                }
            }

            return max;
        }

        private static List<Computer> RunWithFeedBack(List<Computer> list)
        {
            list[0] = list[0].Run(0);

            int skip = 1;

            while (!list.Last().IsHalted)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (skip > 0)
                    {
                        skip--;
                        continue;
                    }

                    if (i == 0)
                    {
                        list[i] = list[i].Run(list.Last().OutputValue);
                    }
                    else
                    {
                        list[i] = list[i].Run(list[i - 1].OutputValue);
                    }
                }
            }

            return list;
        }

        private static List<List<Computer>> CreateComputers(List<BigInteger> code)
        {
            var permuatatedComputers = new List<List<Computer>>();
            var permutations = GetPermutations(new List<int> {9, 8, 7, 6, 5}, 5).ToList();
            permutations.Reverse();

            foreach (var permutation in permutations)
            {
                var computers = new List<Computer>();
                var stackOfPhases = new Stack<int>(permutation.ToList());

                computers.Add(new Computer(code, stackOfPhases.Pop()));

                while (stackOfPhases.Count > 0)
                {
                    computers.Add(new Computer(code, stackOfPhases.Pop()));
                }

                permuatatedComputers.Add(computers);
            }

            return permuatatedComputers;
        }

        static IEnumerable<IEnumerable<T>>
            GetPermutations<T>(List<T> list, int length)
        {
            if (length == 1)
                return list.Select(t => new[]
                {
                    t
                });

            var enumerable = list.ToList();
            return GetPermutations(enumerable, length - 1)
                .SelectMany(t => enumerable
                        .Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new[]
                    {
                        t2
                    }));
        }

    }
}