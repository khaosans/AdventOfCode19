using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {/*
            TestCase1();
            TestCase2();
            TestCase3();*/
            TestPart2();
        }

        private static void TestCase1()
        {
            List<BigInteger> code2 = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day9/testcase1.txt".ParseCsvStringBigInteger();

            var computer = new Computer(code2);

            Computer run = computer.Run();

            PrintOutput(run);
        }

        private static void TestCase2()
        {
            List<BigInteger> code2 = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day9/testcase2.txt".ParseCsvStringBigInteger();

            var computer = new Computer(code2);

            Computer run = computer.Run(1);

            PrintOutput(run);

            if (run.OutputHistory.Last() > 999999999999999)
            {
                Console.Out.WriteLine("PASS");
            }
            else
            {
                Console.Out.WriteLine("FAIL");
            }
        }

        private static void TestCase3()
        {
            List<BigInteger> code2 = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day9/testcase3.txt".ParseCsvStringBigInteger();

            var computer = new Computer(code2);

            Computer run = computer.Run(1);

            PrintOutput(run);
            if (run.OutputHistory.Last() == 1125899906842624)
            {
                Console.Out.WriteLine("PASS");
            }
            else
            {
                Console.Out.WriteLine("FAIL");
            }
        }


        private static void TestPart1()
        {
            List<BigInteger> code2 = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day9/Part1.txt".ParseCsvStringBigInteger();

            var computer = new Computer(code2);

            Computer run = computer.Run(1);

            PrintOutput(run);
        }

        
        private static void TestPart2()
        {
            List<BigInteger> code2 = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day9/Part1.txt".ParseCsvStringBigInteger();

            var computer = new Computer(code2);

            Computer run = computer.Run(2);

            PrintOutput(run);
        }


        private static void PrintOutput(Computer run)
        {
            var strings = run.OutputHistory.Select(x => x.ToString()).ToArray();

            var output = String.Join(" ", strings);

            Console.Out.WriteLine("Printing Test Case:");

            Console.Out.WriteLine($"{output} ");
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