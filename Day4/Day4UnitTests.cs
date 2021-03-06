using System;
using Xunit;

namespace Day4V1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var part1 = Day4Solution.IsValidPart1("245318-765747");
            Assert.Equal("1079", part1);
        }

        [Fact]
        public void Test2()
        {
            var part2 = Day4Solution.UsValidPart2("245318-765747");
            Assert.Equal("699", part2);
        }

       
    }
}