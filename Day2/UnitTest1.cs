using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using Xunit;
using static Day2.Day2;
using OpCode = System.Reflection.Emit.OpCode;

namespace Day2
{
    public class UnitTest1
    {
        [Fact]
        public void TestOpCode1()
        {
            Assert.Equal(new List<int> {2, 0, 0, 0, 99}, OpCode(new List<int> {1, 0, 0, 0, 99}));
            Assert.Equal(new List<int> {2, 3, 0, 6, 99}, OpCode(new List<int> {2, 3, 0, 3, 99}));
            Assert.Equal(new List<int> {2, 4, 4, 5, 99, 9801}, OpCode(new List<int> {2, 4, 4, 5, 99, 0}));
            Assert.Equal(new List<int> {30, 1, 1, 4, 2, 5, 6, 0, 99},
                OpCode(new List<int> {1, 1, 1, 4, 99, 5, 6, 0, 99}));
        }

        [Fact]
        public void Test1Part1()
        {
            var text = System.IO.File.ReadAllText("/Users/skhaosanga/Documents/AdventOfCode19/Day2/Day2.txt");
            var intList = text.Split(",").Select(int.Parse).ToList();
            var opCode = OpCode(intList);
            Assert.Equal(6327510, opCode.First());
        }

        [Fact]
        public void Test1Part2()
        {
            var text = System.IO.File.ReadAllText("/Users/skhaosanga/Documents/AdventOfCode19/Day2/Day2.txt");
            var intList = text.Split(",").Select(int.Parse).ToList();
            var result = 0;
            var nouns = Enumerable.Range(0, 99);
            var verbs = Enumerable.Range(0, 99);
            foreach (var noun in nouns)
            {
                foreach (var verb in verbs)
                {
                    intList = text.Split(",").Select(int.Parse).ToList();

                    intList[1] = noun;
                    intList[2] = verb;

                    if (OpCode(intList).First() == 19690720)
                    {
                         result = 100 * noun + verb;
                    }
                }

            }

            Console.Out.WriteLine(result);
        }
    }
}