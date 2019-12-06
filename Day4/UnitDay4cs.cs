using System;
using Xunit;

namespace Day4V1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var part1 = Day4Solution.isValid("245318-765747");
            
            Assert.Equal("1079", part1);
        }

       
    }
}