using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Xunit;

namespace AdventOfCode
{
    public class UnitTest1
    {
        [Fact]
        public void TestsMass()
        {
            Assert.Equal(33583, FuelMassCalculator.RequiredFuelForModule(100756));
            Assert.Equal(654, FuelMassCalculator.RequiredFuelForModule(1969));
            Assert.Equal(2, FuelMassCalculator.RequiredFuelForModule(14));
            Assert.Equal(2, FuelMassCalculator.RequiredFuelForModule(12));
        }

        [Fact]
        public void Day1Part1()
        {
            var text = System.IO.File.ReadAllText("/Users/souriyakhaosanga/Documents/Day1/Day1/Day1Part1.txt");

            var sum = Array.ConvertAll(text.Split("\n"), Double.Parse)
                .Select(x => FuelMassCalculator.RequiredFuelForModule((float) x))
                .ToList()
                .Sum();

            Assert.Equal(3311492, sum);
        }

        [Fact]
        public void Day1Part2()
        {
            Assert.Equal(966, FuelMassCalculator.RequiredFuelForFuel(1969));
            var text = System.IO.File.ReadAllText("/Users/souriyakhaosanga/Documents/Day1/Day1/Day1Part2.txt");

            var sum = Array.ConvertAll(text.Split("\n"), Double.Parse)
                .Select(x => FuelMassCalculator.RequiredFuelForFuel((float) x))
                .ToList()
                .Sum();
            
            Assert.NotNull(sum);
        }
    }
}