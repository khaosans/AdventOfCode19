using System;
using Xunit;

namespace Day4V1
{
    public class UnitTest1
    {
        private string _isValid2;

        [Fact]
        public void Test1()
        {
            var part1 = Day4Solution.isValid("245318-765747");
            Assert.Equal("1079", part1);
        }

        [Fact]
        public void Test2()
        {
            _isValid2 = Day4Solution.isValid2("245318-765747");
            var part2 = _isValid2;
            Assert.Equal("699", part2);
        }

       
    }
}