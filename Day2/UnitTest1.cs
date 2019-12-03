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
            var text = System.IO.File.ReadAllText("/Users/skhaosanga/Documents/AdventOfCode19/Day2/Day2.txt");

            var intList = text.Split(",").Select(int.Parse).ToList();

            List<int> runCode = RunCode(intList);
            
            Assert.True(runCode.Count>0);
        }
    }
}