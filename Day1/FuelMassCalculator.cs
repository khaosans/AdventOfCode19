using System;
using System.Net.Http;

namespace AdventOfCode
{
    public class FuelMassCalculator
    {
        public static float RequiredFuelForModule(float mass)
        {
            return (float) (Math.Floor(mass / 3) - 2);
        }

        public static float RequiredFuelForFuel(float mass)
        {
            var requiredFuel = RequiredFuelForModule(mass);
            if (requiredFuel < 0)
            {
                return 0;
            }

            return requiredFuel + RequiredFuelForFuel(requiredFuel);
        }
    }
}