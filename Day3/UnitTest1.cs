using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xunit;

namespace Day3V2
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var test1 = "R75,D30,R83,U83,L12,D49,R71,U7,L72\nU62,R66,U55,R34,D71,R55,D58,R83";

            var closest = Direction.FindClosest(test1);

            Assert.Equal(159, closest);
        }

        [Fact]
        public void Test2()
        {
            string test2 = "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\nU98,R91,D20,R16,D67,R40,U7,R15,U6,R7";
            var closest = Direction.FindClosest(test2);
            Assert.Equal(135, closest);
        }

        [Fact]
        public void Day3()
        {
            var text = System.IO.File.ReadAllText("/Users/souriyakhaosanga/Documents/AdventOfCode/Day3/Day3.txt");

            var findClosest = Direction.FindClosest(text);
            Assert.Equal(1084, findClosest);
        }

        [Fact]
        public void Day3Steps()
        {
            var text = "R75,D30,R83,U83,L12,D49,R71,U7,L72\nU62,R66,U55,R34,D71,R55,D58,R83";

            var steps = Direction.GetSteps(text);
            Assert.Equal(610, steps);
        }

        [Fact]
        public void Day3Steps2()
        {
            var text = "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\nU98,R91,D20,R16,D67,R40,U7,R15,U6,R7";

            var steps = Direction.GetSteps(text);
            Assert.Equal(410, steps);
        }
    }
}