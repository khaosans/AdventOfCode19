using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using Xunit;
using static Day2.Day2;

namespace Day2
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var text = System.IO.File.ReadAllText("/Users/souriyakhaosanga/Documents/AdventOfCode/Day2/Day2.txt");

            var intList = text.Split(",").Select(int.Parse).ToList();

            var newLIst = new List<List<int>>();
            var table = CreateTable(intList);


            foreach (var line in table)
            {
                var lineList = line.Select(x => x).ToList();


                if (lineList.First() == 1)
                {
                    lineList = OpCode(lineList, intList.ToArray(), OpCodeValue.One).ToList();
                }

                if (lineList.First() == 2)
                {
                    lineList = OpCode(lineList, intList.ToArray(), OpCodeValue.Two).ToList();
                }

                if (lineList.First() == 99)
                {
                    break;
                }


                newLIst.Add(lineList);
            }

            var count = newLIst.Count;
        }
    }
}